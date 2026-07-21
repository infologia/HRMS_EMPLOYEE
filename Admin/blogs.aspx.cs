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
                control1.Text = "Blogs";

            ShowRedirectMessage();
            LoadBlogList();
        }
    }

    private void ShowRedirectMessage()
    {
        string msg = Request.QueryString["msg"];
        if (string.IsNullOrEmpty(msg)) return;

        if (msg == "saved")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Blog saved successfully!');", true);
        else if (msg == "updated")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Blog updated successfully!');", true);
    }

    private void LoadBlogList()
    {
        string query = "SELECT BL_ID, BlogTitle, BlogCreator, Description, Title FROM IT_BlogCreation ORDER BY CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0) return;

        DataSet ds = new DataSet();
        ds.Merge(dt);
        this.PH.LoadGridItem(ds, PH_BlogList, "blog.txt", "");
    }
}
