using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_Complaints : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Complaints";
        }
        if (!IsPostBack)
        {
            this.loaddropdown();
        }
        str_userid = this.SC.Userid;
    }
    public void loaddropdown()
    {

        string str_URL = "select * from IT_ComplaintCategory";
        DataTable dt_coplt = DA.GetDataTable(str_URL);
       // DataTable dt_test2 = dataTable;

      //  string str_URL = "select * from IT_ComplaintCategory where ComplaintCategoryid not in(select ComplaintCategoryid from IT_ComplaintCategory where ComplaintCategoryid in (select complaintcategory from IT_Complaint where createdby='" + SC.Userid + "' and complaintstatus='1'))";
      //  SqlCommand cmd = new SqlCommand(str_URL);
      //  DataTable dt_test = DA.GetDataTable(str_URL);
       // DataSet ds1 = this.DA.GetDataSet(cmd);
        if (dt_coplt != null && dt_coplt.Rows.Count > 0)
        {

            ddl_category.DataSource = dt_coplt;
            ddl_category.DataTextField = "ComplaintCategoryName";
            ddl_category.DataValueField = "ComplaintCategoryid";

            ddl_category.DataBind();
            ddl_category.Items.Add(new ListItem("Select Your Category", "0"));
            ddl_category.SelectedValue = "0";
        }
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {
            Random generator = new Random();
            int number = generator.Next(0000, 9999);

            string str_rmnum = "ITcomp_" + number;
            int status = 1;

            string date = DateTime.Now.ToString();
            string str_Sql = ("insert into IT_Complaint(ComplaintCategory,Reason,Createdby,Employeekey,complaintid,complaintstatus)values(@ComplaintCategory,@Reason,@Createdby,@Employeekey,@complaintid,@complaintstatus)");
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@ComplaintCategory", ddl_category.SelectedValue);
            cmd.Parameters.AddWithValue("@Reason", txt_reason.InnerText.Trim());
            cmd.Parameters.AddWithValue("@Createdby", this.str_userid);
            cmd.Parameters.AddWithValue("@Employeekey", this.str_userid);
            cmd.Parameters.AddWithValue("@complaintid", str_rmnum);
            cmd.Parameters.AddWithValue("@complaintstatus", status);
            DA.ExecuteNonQuery(cmd);
            Response.Redirect(@"~/Employee/complaintresponseview.aspx");
        }
        catch (Exception ex)
        {
        }
    }
}