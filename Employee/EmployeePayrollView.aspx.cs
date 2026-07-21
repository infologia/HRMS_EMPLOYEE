using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

public partial class Employee_EmployeePayrollView : System.Web.UI.Page
{
    DataAccess DA = new DataAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string payrollKey = Request.QueryString["key"];
            if (!string.IsNullOrEmpty(payrollKey))
            {
                LoadPayrollDetails(payrollKey);
            }
        }
    }

    private void LoadPayrollDetails(string payrollKey)
    {
        string query = @"SELECT a.salarymonth, a.Workingdays, a.leavedays, a.lop, a.Totalsalary, a.netpay, 
                                b.firstname, b.Employeeid
                         FROM IT_Payroll a
                         LEFT JOIN IT_employeeregister b ON a.employeekey = b.employeekey
                         WHERE a.payrollkey = @payrollkey";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@payrollkey", payrollKey);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            // Build a simple HTML payslip
            StringBuilder sb = new StringBuilder();

            sb.Append("<div style='font-family:Arial; line-height:1.6;'>");
            sb.Append("<h3 style='text-align:center;'>Employee Payslip</h3>");
            sb.Append("<hr/>");
            sb.Append("<table style='width:100%; border-collapse:collapse;'>");
            sb.Append("<tr><td><b>Employee ID:</b></td><td>" + dt.Rows[0]["Employeeid"].ToString() + "</td></tr>");
            sb.Append("<tr><td><b>Name:</b></td><td>" + dt.Rows[0]["firstname"].ToString() + "</td></tr>");
            sb.Append("<tr><td><b>Salary Month:</b></td><td>" + dt.Rows[0]["salarymonth"].ToString() + "</td></tr>");
            sb.Append("<tr><td><b>Working Days:</b></td><td>" + dt.Rows[0]["Workingdays"].ToString() + "</td></tr>");
            sb.Append("<tr><td><b>Leave Days:</b></td><td>" + dt.Rows[0]["leavedays"].ToString() + "</td></tr>");
            sb.Append("<tr><td><b>LOP:</b></td><td>" + dt.Rows[0]["lop"].ToString() + "</td></tr>");
            sb.Append("<tr><td><b>Total Salary:</b></td><td>" + dt.Rows[0]["Totalsalary"].ToString() + "</td></tr>");
            sb.Append("<tr><td><b>Net Pay:</b></td><td>" + dt.Rows[0]["netpay"].ToString() + "</td></tr>");
            sb.Append("</table>");
            sb.Append("</div>");

            payroll.Controls.Add(new LiteralControl(sb.ToString()));
        }
        else
        {
            payroll.Controls.Add(new LiteralControl("<p style='color:red;'>No payroll data found.</p>"));
        }
    }
}
