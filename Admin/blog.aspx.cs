using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
public partial class Admin_blog : System.Web.UI.Page
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
        SqlCommand cmd = new SqlCommand("SELECT * FROM IT_BlogCreation WHERE BL_ID = @BL_ID");
        cmd.Parameters.AddWithValue("@BL_ID", blId);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0) return;

        DataRow dr = dt.Rows[0];
        txtBlogTitle.Text       = dr["BlogTitle"].ToString();
        txtBlogCreator.Text     = dr["BlogCreator"].ToString();
        txtTitle.Text           = dr["Title"].ToString();
        txtKeyWords.Text        = dr["Keywords"].ToString();
        txtDescription.Text     = dr["Description"].ToString();
        string rawContent = dr["BlogContent"].ToString();
        hdnBlogContent.Value = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rawContent));
        hfSmallImagePath.Value  = dr["SmallImage"].ToString();
        hfBlogImagePath.Value   = dr["BlogImage"].ToString();

        if (dr["PublishDate"] != DBNull.Value)
            txtSchedulePublishDate.Text = Convert.ToDateTime(dr["PublishDate"]).ToString("yyyy-MM-dd");

        if (dr["Flag"] != DBNull.Value)
            ddlFlag.SelectedValue = dr["Flag"].ToString();
    }

    private string SaveImage(HtmlInputFile fileInput, string existingPath, string folder)
    {
        if (fileInput.PostedFile == null || fileInput.PostedFile.ContentLength == 0)
            return existingPath;

        string uploadDir = @"C:\VedhaUploads\" + folder + @"\";
        if (!Directory.Exists(uploadDir))
            Directory.CreateDirectory(uploadDir);

        string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(fileInput.PostedFile.FileName);
        fileInput.PostedFile.SaveAs(uploadDir + fileName);
        return fileName;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        Guid userId = new Guid(SC.Userid.ToString());
        string smallImage = SaveImage(fuSmallImage, "", "SmallImages");
        string blogImage  = SaveImage(fuBlogImage, "", "BlogImages");

        string query = @"INSERT INTO IT_BlogCreation 
            (BlogTitle, BlogCreator, PublishDate, Flag, Keywords, Description, SmallImage, BlogImage, Title, BlogContent, Status, CreatedBy, CreatedOn)
            VALUES (@BlogTitle, @BlogCreator, @PublishDate, @Flag, @Keywords, @Description, @SmallImage, @BlogImage, @Title, @BlogContent, 1, @CreatedBy, GETDATE())";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@BlogTitle",    txtBlogTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@BlogCreator",  txtBlogCreator.Text.Trim());
        cmd.Parameters.AddWithValue("@PublishDate",  string.IsNullOrEmpty(txtSchedulePublishDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtSchedulePublishDate.Text));
        cmd.Parameters.AddWithValue("@Flag",         string.IsNullOrEmpty(ddlFlag.SelectedValue) ? (object)DBNull.Value : int.Parse(ddlFlag.SelectedValue));
        cmd.Parameters.AddWithValue("@Keywords",     txtKeyWords.Text.Trim());
        cmd.Parameters.AddWithValue("@Description",  txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@SmallImage",   smallImage);
        cmd.Parameters.AddWithValue("@BlogImage",    blogImage);
        cmd.Parameters.AddWithValue("@Title",        txtTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@BlogContent",  GetBlogContent());
        cmd.Parameters.Add("@CreatedBy", System.Data.SqlDbType.UniqueIdentifier).Value = userId;

        DA.ExecuteNonQuery(cmd);

        Response.Redirect("blogs.aspx?msg=saved");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || string.IsNullOrEmpty(hfBlogKey.Value)) return;

        Guid userId   = new Guid(SC.Userid.ToString());
        int blId      = int.Parse(hfBlogKey.Value);
        string smallImage = SaveImage(fuSmallImage, hfSmallImagePath.Value, "SmallImages");
        string blogImage  = SaveImage(fuBlogImage,  hfBlogImagePath.Value,  "BlogImages");

        string query = @"UPDATE IT_BlogCreation SET
            BlogTitle=@BlogTitle, BlogCreator=@BlogCreator, PublishDate=@PublishDate,
            Flag=@Flag, Keywords=@Keywords, Description=@Description,
            SmallImage=@SmallImage, BlogImage=@BlogImage, Title=@Title,
            BlogContent=@BlogContent, ModifiedBy=@ModifiedBy, ModifiedOn=GETDATE()
            WHERE BL_ID=@BL_ID";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@BlogTitle",    txtBlogTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@BlogCreator",  txtBlogCreator.Text.Trim());
        cmd.Parameters.AddWithValue("@PublishDate",  string.IsNullOrEmpty(txtSchedulePublishDate.Text) ? (object)DBNull.Value : DateTime.Parse(txtSchedulePublishDate.Text));
        cmd.Parameters.AddWithValue("@Flag",         string.IsNullOrEmpty(ddlFlag.SelectedValue) ? (object)DBNull.Value : int.Parse(ddlFlag.SelectedValue));
        cmd.Parameters.AddWithValue("@Keywords",     txtKeyWords.Text.Trim());
        cmd.Parameters.AddWithValue("@Description",  txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@SmallImage",   smallImage);
        cmd.Parameters.AddWithValue("@BlogImage",    blogImage);
        cmd.Parameters.AddWithValue("@Title",        txtTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@BlogContent",  GetBlogContent());
        cmd.Parameters.Add("@ModifiedBy", System.Data.SqlDbType.UniqueIdentifier).Value = userId;
        cmd.Parameters.AddWithValue("@BL_ID",        blId);

        DA.ExecuteNonQuery(cmd);

        Response.Redirect("blogs.aspx?msg=updated");
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
