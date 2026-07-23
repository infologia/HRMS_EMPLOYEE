using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_blogs : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Vedha Blogs";

            if (!string.IsNullOrEmpty(Request.QueryString["approve"]))
            {
                int blId;
                if (int.TryParse(Request.QueryString["approve"], out blId))
                {
                    ApproveBlog(blId);
                    Response.Redirect("blogs.aspx?msg=approved");
                    return;
                }
            }

            ShowRedirectMessage();
            LoadBlogList();
        }
    }

    private void ApproveBlog(int blId)
    {
        SqlCommand cmd = new SqlCommand("UPDATE IT_BlogCreation SET Status=1, IsPublished=1, PublishDate=@PublishDate WHERE BL_ID=@BL_ID");
        cmd.Parameters.AddWithValue("@PublishDate", DateTime.Today);
        cmd.Parameters.AddWithValue("@BL_ID", blId);
        DA.ExecuteNonQuery(cmd);
    }

    private void ShowRedirectMessage()
    {
        string msg = Request.QueryString["msg"];
        if (string.IsNullOrEmpty(msg)) return;

        if (msg == "saved")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Blog saved successfully!');", true);
        else if (msg == "approved")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Blog approved successfully!');", true);
    }

    private void LoadBlogList()
    {
        string query = "SELECT BL_ID, BlogTitle, BlogCreator, Status, IsPublished, dbo.fn_Slug(BlogTitle) AS Header FROM IT_BlogCreation ORDER BY CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0) return;

        dt.Columns.Add("StatusBadge", typeof(string));
        dt.Columns.Add("PublishedBadge", typeof(string));
        dt.Columns.Add("ViewAction", typeof(string));
        dt.Columns.Add("UpdateAction", typeof(string));

        foreach (DataRow dr in dt.Rows)
        {
            int blId         = Convert.ToInt32(dr["BL_ID"]);
            bool isActive    = dr["Status"] != DBNull.Value && Convert.ToInt32(dr["Status"]) == 1;
            bool isPublished = dr["IsPublished"] != DBNull.Value && Convert.ToInt32(dr["IsPublished"]) == 1;

            dr["StatusBadge"] = isActive
                ? "<span class=\"blog-status-btn is-approved\">Approved</span>"
                : "<a href=\"blogs.aspx?approve=" + blId + "\" class=\"blog-status-btn is-pending\" onclick=\"return confirm('Approve this blog?');\">Pending</a>";

            dr["PublishedBadge"] = isPublished
                ? "<span class=\"label label-success\">Yes</span>"
                : "<span class=\"label label-danger\">No</span>";

            string slugHeader = dr["Header"] != DBNull.Value ? dr["Header"].ToString() : "";
            dr["ViewAction"] = "<a href=\"https://vedhaglobal.com/web/BlogDetails.aspx?id=" + blId + "&Header=" + slugHeader + "\" target=\"_blank\" class=\"blog-view-btn\"><i class=\"icon-eye\"></i></a>";
            dr["UpdateAction"] = "<a href=\"blog.aspx?action=edit&key=" + blId + "\" class=\"blog-update-btn\"><i class=\"icon-pencil\"></i></a>";
        }

        DataSet ds = new DataSet();
        ds.Merge(dt);
        this.PH.LoadGridItem(ds, PH_BlogList, "blog.txt", "");
    }
}
