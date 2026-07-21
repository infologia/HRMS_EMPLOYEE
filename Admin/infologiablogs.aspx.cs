using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_infologiablogs : System.Web.UI.Page
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
                control1.Text = "Blogs";

            if (!string.IsNullOrEmpty(Request.QueryString["approve"]))
            {
                int blId;
                if (int.TryParse(Request.QueryString["approve"], out blId))
                {
                    ApproveBlog(blId);
                    Response.Redirect("infologiablogs.aspx?msg=approved");
                    return;
                }
            }

            ShowRedirectMessage();
            LoadBlogList();
        }
    }

    private void ApproveBlog(int blId)
    {
        string query = @"UPDATE IT_InfologiaBlogcreation SET
            status = 1,
            ispublished = 1,
            Publisheddate = @Publisheddate
            WHERE bd_id = @bd_id";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Publisheddate", DateTime.Today);
        cmd.Parameters.AddWithValue("@bd_id", blId);

        DA.ExecuteNonQuery(cmd);
    }

    private void ShowRedirectMessage()
    {
        string msg = Request.QueryString["msg"];
        if (string.IsNullOrEmpty(msg)) return;

        if (msg == "saved")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Blog saved successfully!');", true);
        else if (msg == "updated")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Blog updated successfully!');", true);
        else if (msg == "approved")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Blog approved successfully!');", true);
        else if (msg == "readonly")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_warning", "toastr.warning('This blog is already approved and cannot be edited.');", true);
    }

    private void LoadBlogList()
    {
        string query = "SELECT bd_id, blogtitle, blogcreatername, status, ispublished FROM IT_InfologiaBlogcreation ORDER BY createdon DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0) return;

        dt.Columns.Add("StatusBadge", typeof(string));
        dt.Columns.Add("PublishedBadge", typeof(string));
        dt.Columns.Add("ViewAction", typeof(string));
        dt.Columns.Add("UpdateAction", typeof(string));

        foreach (DataRow dr in dt.Rows)
        {
            int blId         = Convert.ToInt32(dr["bd_id"]);
            bool isActive    = dr["status"] != DBNull.Value && Convert.ToInt32(dr["status"]) == 1;
            bool isPublished = dr["ispublished"] != DBNull.Value && Convert.ToInt32(dr["ispublished"]) == 1;

            dr["StatusBadge"] = isActive
                ? "<span class=\"blog-status-btn is-approved\">Approved</span>"
                : "<a href=\"infologiablogs.aspx?approve=" + blId + "\" class=\"blog-status-btn is-pending\" onclick=\"return confirm('Approve this blog?');\">Pending</a>";

            dr["PublishedBadge"] = isPublished
                ? "<span class=\"label label-success\">Yes</span>"
                : "<span class=\"label label-danger\">No</span>";

            dr["ViewAction"] = "<a href=\"infologiablog.aspx?action=edit&key=" + blId + "\" class=\"blog-view-btn\"><i class=\"icon-eye\"></i></a>";

            dr["UpdateAction"] = isActive
                ? "<span class=\"blog-update-btn is-disabled\" title=\"Approved blogs are read-only\"><i class=\"icon-pencil\"></i></span>"
                : "<a href=\"infologiablog.aspx?action=edit&key=" + blId + "\" class=\"blog-update-btn\"><i class=\"icon-pencil\"></i></a>";
        }

        DataSet ds = new DataSet();
        ds.Merge(dt);
        this.PH.LoadGridItem(ds, PH_BlogList, "infologiablogs.txt", "");
    }
}
