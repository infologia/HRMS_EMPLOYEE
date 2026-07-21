using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Employee_EmployeePayroll : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_key = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        str_key = this.SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "File Maintenance";
        }

        string str_payroll = @"SELECT a.payrollkey, a.Workingdays, a.salarymonth, a.Totalsalary, a.netpay, 
                                      a.lop, a.leavedays, b.firstname, b.Employeeid
                               FROM IT_Payroll a
                               LEFT JOIN IT_employeeregister b ON a.employeekey = b.employeekey
                               WHERE a.Employeekey = @Employeekey";

        SqlCommand cmd = new SqlCommand(str_payroll);
        cmd.Parameters.AddWithValue("@Employeekey", str_key);

        DataTable dt_payroll = this.DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_payroll);

        if (dt_payroll.Rows.Count > 0)
        {
            PH.LoadGridItem(ds, payroll, "Employeepayroll.txt", "");
        }
    }
}
