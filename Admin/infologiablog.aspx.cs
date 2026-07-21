using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
public partial class Admin_infologiablog : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (Page.Form != null)
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null) control1.Text = "Blog";

            BindFlag();

            if (!string.IsNullOrEmpty(Request.QueryString["action"]) &&
                Request.QueryString["action"] == "edit" &&
                !string.IsNullOrEmpty(Request.QueryString["key"]))
            {
                int blId;
                if (int.TryParse(Request.QueryString["key"], out blId))
                {
                    PopulateData(blId);
                    hfBlogKey.Value = blId.ToString();
                    btnSave.Visible = false;
                    btnUpdate.Visible = true;
                }
            }
        }
    }

    private void BindFlag()
    {
        string query = "SELECT BF_ID, FlagName FROM IT_BlogForFlag ORDER BY FlagName";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        ddlFlag.Items.Clear();
        ddlFlag.Items.Add(new ListItem("-- Select Flag --", ""));

        if (dt != null && dt.Rows.Count > 0)
            foreach (DataRow dr in dt.Rows)
                ddlFlag.Items.Add(new ListItem(dr["FlagName"].ToString(), dr["BF_ID"].ToString()));
    }

    private void PopulateData(int blId)
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM IT_InfologiaBlogcreation WHERE bd_id = @bd_id");
        cmd.Parameters.AddWithValue("@bd_id", blId);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0) return;

        DataRow dr = dt.Rows[0];
        txtBlogTitle.Text       = dr["blogtitle"].ToString();
        txtBlogCreator.Text     = dr["blogcreatername"].ToString();
        txtTitle.Text           = dr["Headertitle"].ToString();
        txtKeyWords.Text        = dr["keywords"].ToString();
        txtDescription.Text     = dr["Keydescription"].ToString();
        string rawContent = dr["description"].ToString();
        hdnBlogContent.Value = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rawContent));
        hfSmallImagePath.Value  = dr["smallimage"].ToString();
        hfBlogImagePath.Value   = dr["fileupload"].ToString();

        if (dr["Publisheddate"] != DBNull.Value)
            txtSchedulePublishDate.Text = Convert.ToDateTime(dr["Publisheddate"]).ToString("yyyy-MM-dd");

        if (dr["flag"] != DBNull.Value)
            ddlFlag.SelectedValue = dr["flag"].ToString();
    }

    private const string UploadDir = @"C:\inetpub\wwwroot\Production build\Infologia_PRD\Infologia_Website\Infologia\browser\assets\Blog\";

    private string SaveImage(HtmlInputFile fileInput, string existingPath)
    {
        if (fileInput.PostedFile == null || fileInput.PostedFile.ContentLength == 0)
            return existingPath;

        if (!Directory.Exists(UploadDir))
            Directory.CreateDirectory(UploadDir);

        string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(fileInput.PostedFile.FileName);
        fileInput.PostedFile.SaveAs(UploadDir + fileName);
        return fileName; // only the file name is stored in the DB column
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string smallImage = SaveImage(fuSmallImage, "");
        string blogImage  = SaveImage(fuBlogImage, "");

        string query = @"INSERT INTO IT_InfologiaBlogcreation
            (blogtitle, blogcreatername, Publisheddate, flag, keywords, Keydescription, smallimage, fileupload, Headertitle, description, tag, status, ispublished, createdon)
            VALUES (@blogtitle, @blogcreatername, @Publisheddate, @flag, @keywords, @Keydescription, @smallimage, @fileupload, @Headertitle, @description, @tag, 0, 0, GETDATE())";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@blogtitle",       txtBlogTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@blogcreatername", txtBlogCreator.Text.Trim());
        cmd.Parameters.AddWithValue("@Publisheddate",   string.IsNullOrEmpty(txtSchedulePublishDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtSchedulePublishDate.Text));
        cmd.Parameters.AddWithValue("@flag",            string.IsNullOrEmpty(ddlFlag.SelectedValue) ? (object)DBNull.Value : int.Parse(ddlFlag.SelectedValue));
        cmd.Parameters.AddWithValue("@keywords",        txtKeyWords.Text.Trim());
        cmd.Parameters.AddWithValue("@Keydescription",  txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@smallimage",      smallImage);
        cmd.Parameters.AddWithValue("@fileupload",      blogImage);
        cmd.Parameters.AddWithValue("@Headertitle",     txtTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@description",     GetBlogContent());
        cmd.Parameters.AddWithValue("@tag",             (object)DBNull.Value);

        DA.ExecuteNonQuery(cmd);

        Response.Redirect("infologiablogs.aspx?msg=saved");
    }

    private bool IsAlreadyApproved(int blId)
    {
        SqlCommand cmd = new SqlCommand("SELECT status FROM IT_InfologiaBlogcreation WHERE bd_id = @bd_id");
        cmd.Parameters.AddWithValue("@bd_id", blId);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0) return false;
        object statusVal = dt.Rows[0]["status"];
        return statusVal != DBNull.Value && Convert.ToInt32(statusVal) == 1;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || string.IsNullOrEmpty(hfBlogKey.Value)) return;

        int blId = int.Parse(hfBlogKey.Value);

        string smallImage = SaveImage(fuSmallImage, hfSmallImagePath.Value);
        string blogImage  = SaveImage(fuBlogImage,  hfBlogImagePath.Value);

        string query = @"UPDATE IT_InfologiaBlogcreation SET
            blogtitle=@blogtitle, blogcreatername=@blogcreatername, Publisheddate=@Publisheddate,
            flag=@flag, keywords=@keywords, Keydescription=@Keydescription,
            smallimage=@smallimage, fileupload=@fileupload, Headertitle=@Headertitle,
            description=@description, modifiedon=GETDATE()
            WHERE bd_id=@bd_id";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@blogtitle",       txtBlogTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@blogcreatername", txtBlogCreator.Text.Trim());
        cmd.Parameters.AddWithValue("@Publisheddate",   string.IsNullOrEmpty(txtSchedulePublishDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtSchedulePublishDate.Text));
        cmd.Parameters.AddWithValue("@flag",            string.IsNullOrEmpty(ddlFlag.SelectedValue) ? (object)DBNull.Value : int.Parse(ddlFlag.SelectedValue));
        cmd.Parameters.AddWithValue("@keywords",        txtKeyWords.Text.Trim());
        cmd.Parameters.AddWithValue("@Keydescription",  txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@smallimage",      smallImage);
        cmd.Parameters.AddWithValue("@fileupload",      blogImage);
        cmd.Parameters.AddWithValue("@Headertitle",     txtTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@description",     GetBlogContent());
        cmd.Parameters.AddWithValue("@bd_id",           blId);

        DA.ExecuteNonQuery(cmd);

        Response.Redirect("infologiablogs.aspx?msg=updated");
    }

    private string GetBlogContent()
    {
        string encoded = Request.Unvalidated.Form[hdnBlogContent.UniqueID];
        if (string.IsNullOrEmpty(encoded)) return "";
        return SanitizeHtml(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encoded)));
    }

    private string SanitizeHtml(string html)
    {
        if (string.IsNullOrEmpty(html)) return html;
        // Remove script, iframe, object, embed tags and event attributes
        html = Regex.Replace(html, @"<(script|iframe|object|embed|form)[^>]*>.*?<\/\1>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        html = Regex.Replace(html, @"<(script|iframe|object|embed|form)[^>]*\/>", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"\s+on\w+\s*=\s*([""'])[^""']*\1", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"\s+on\w+\s*=[^\s>]+", "", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"javascript\s*:", "", RegexOptions.IgnoreCase);
        return html;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtBlogTitle.Text            = "";
        txtBlogCreator.Text          = "";
        txtSchedulePublishDate.Text  = "";
        ddlFlag.SelectedIndex        = 0;
        txtKeyWords.Text             = "";
        txtDescription.Text          = "";
        txtTitle.Text                = "";
        hdnBlogContent.Value         = "";
        hfSmallImagePath.Value       = "";
        hfBlogImagePath.Value        = "";
        hfBlogKey.Value              = "";
    }
}
