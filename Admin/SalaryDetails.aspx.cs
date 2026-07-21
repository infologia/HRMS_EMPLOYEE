using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WEB_Admin_SalaryDetails : System.Web.UI.Page
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
                control1.Text = "Employee Pay";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }


        string str_query = "select a.Employeesalarydetailskey, b.Employeeid, a.Employeename, a.Empoloyeemonthlysalary, a.Netpay, " +
                   "CONVERT(varchar(10), a.createdon, 103) as createdon " +  
                   "from IT_EmployeeSalaryDetails a " +
                   "left outer join IT_EmployeeRegister b on a.Employeekey = b.Employeekey " +
                   "order by a.createdon desc";

        SqlCommand cmd = new SqlCommand(str_query);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, PH_Salery, "SalaryView.txt", "");

        }

    }
}