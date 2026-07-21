using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketingTool_Project_sSummaryDetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.LoadSummary();
    }

    private void LoadSummary()
    {
        this.str_userkey = SC.Userid.ToString();
        string str_sql = "select b.firstname,a.Pjname,a.pjdescription,a.Createdon from TT_project a  left outer join IT_EmployeeRegister b   on a.Createdby =b.Employeekey ";
        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@projectkey", this.str_userkey);
        DataTable dt_task = DA.GetDataTable(cmd);

        if (dt_task != null && dt_task.Rows.Count > 0)
        {

            lbl_pjname.Text = dt_task.Rows[0]["Pjname"].ToString();
            lbl_crdate.Text = dt_task.Rows[0]["Createdon"].ToString();
            lbl_cldate.Text = dt_task.Rows[0]["Createdon"].ToString();
            lbl_name.Text = dt_task.Rows[0]["firstname"].ToString();
            txt_des.Text = dt_task.Rows[0]["pjdescription"].ToString();


        }
    }
}