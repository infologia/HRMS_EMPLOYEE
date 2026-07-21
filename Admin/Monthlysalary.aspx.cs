using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Monthlysalary : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id = "";
    string str_pay = "";
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
                control1.Text = "Document & Maintanence";
        }

        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {
            this.str_id = Request.QueryString["key"].ToString();
        }
        else
        {
            loadgrid();
            return;
        }
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {
            this.str_pay = Request.QueryString["id"].ToString();
        }

        string str_month = "select  SalaryMonth,salaryyear from IT_Payroll where SalaryMonth=@SalaryMonth AND salaryyear=@salaryyear";
        SqlCommand sc = new SqlCommand(str_month);
        sc.Parameters.AddWithValue("@SalaryMonth", str_id);
         sc.Parameters.AddWithValue("@salaryyear", str_pay);
        DataTable dt_month = this.DA.GetDataTable(sc);
        if (dt_month.Rows.Count > 0 && dt_month.Rows.Count != null)
        {
            string month = dt_month.Rows[0]["SalaryMonth"].ToString();
            string year = dt_month.Rows[0]["salaryyear"].ToString();
            if (month == str_id && year == str_pay)
            {
                this.loadgrid();
                div_error.Visible = true;
                lbl_error.Text = "It's already registered";
                //ClientScript.RegisterStartupScript(this.GetType(), "Internal tool", "<script>alert('  It's already registered');</script>");
                return;
            }

        }
        if (!IsPostBack)
        {
            this.loadpayroll();
            this.loadgrid();
        }

    }
    private void loadgrid()
    {
        string str_pay = "select * from IT_Payroll";
        SqlCommand sc1 = new SqlCommand(str_pay);
        DataTable dt_payrolls = DA.GetDataTable(sc1);
        DataSet ds = new DataSet();
        ds.Merge(dt_payrolls);
        if (dt_payrolls.Rows.Count > 0 && dt_payrolls.Rows.Count != null)
        {

            PH.LoadGridItem(ds,Ph_salary, "payrollview.txt", "");
        }
    }

    public void loadpayroll()
    {
        string str_payroll = "select a.employeebasipay,a.netpay,b.firstname,b.employeekey,b.employeeid,c.monthvalue,c.Year,c.numberofworkdaysinmonth," +
                            " d.Leavedays,d.Cashmode,d.PayslipIssued  from IT_EmployeeSalaryDetails a LEFT OUTER JOIN IT_EmployeeRegister b on a.Employeekey = b.Employeekey LEFT OUTER JOIN " +
                            " IT_EmployeeWorkingDayDetails c on a.createdby = c.createdby  LEFT OUTER JOIN IT_EmployeeLeaveBalance  d on a.Employeekey = b.Employeekey  " +
                            " where c.monthvalue =@monthvalue and c.year =@year and d.LOPmonth=@monthvalue and d.Lopyear=@year group by a.employeebasipay,a.netpay,b.firstname,b.employeekey,b.employeeid,c.monthvalue,c.Year,c.numberofworkdaysinmonth,d.Leavedays,d.Cashmode,d.PayslipIssued";
        SqlCommand cmd1 = new SqlCommand(str_payroll);
        cmd1.Parameters.AddWithValue("@monthvalue", str_id);
        cmd1.Parameters.AddWithValue("@year", str_pay);
        DataTable dt_payroll = this.DA.GetDataTable(cmd1);

        if (dt_payroll.Rows.Count > 0)
        {
            for (int i = 0; i < dt_payroll.Rows.Count; i++)
            {
                decimal basicpay;
                basicpay = 0;
                if (dt_payroll.Rows[i]["employeebasipay"].ToString() != null && dt_payroll.Rows[i]["employeebasipay"].ToString() != "")
                {
                    var varbasicpay = dt_payroll.Rows[i]["employeebasipay"].ToString();
                    basicpay = Convert.ToDecimal(varbasicpay);
                }
                else
                {
                    basicpay = 0;
                }

                int workingdays;
                workingdays = 0;
                if (dt_payroll.Rows[i]["Numberofworkdaysinmonth"].ToString() != null && dt_payroll.Rows[i]["Numberofworkdaysinmonth"].ToString() != "")
                {
                    var varworkdays = dt_payroll.Rows[i]["Numberofworkdaysinmonth"].ToString();
                    workingdays = Convert.ToInt32(varworkdays);
                }
                else
                {
                    workingdays = 0;
                }


                int leavedays;
                leavedays = 0;
                if (dt_payroll.Rows[i]["Leavedays"].ToString() != null && dt_payroll.Rows[i]["Leavedays"].ToString() != "")
                {
                    var varleave = dt_payroll.Rows[i]["Leavedays"].ToString();
                    leavedays = Convert.ToInt32(varleave);
                }
                else
                {
                    leavedays = 0;
                }


                decimal netpay;
                netpay = 0;
                if (dt_payroll.Rows[i]["netpay"].ToString() != null && dt_payroll.Rows[i]["netpay"].ToString() != "")
                {
                    var varnetpay = dt_payroll.Rows[i]["netpay"].ToString();
                    netpay = Convert.ToDecimal(varnetpay);
                }
                else
                {
                    netpay = 0;
                }


                decimal onedaysalary = netpay / workingdays;
                //decimal calleavedays = workingdays - leavedays;
                decimal LOP = leavedays * onedaysalary;
                decimal salary = netpay - LOP;
                int status;
                if (salary != null)
                {
                    status = 2;

                }
                else
                {
                    status = 1;
                }

                string str_empkey = dt_payroll.Rows[i]["Employeekey"].ToString();
                string str_empid = dt_payroll.Rows[i]["Employeeid"].ToString();
                string str_empname = dt_payroll.Rows[i]["firstname"].ToString();
                string month = dt_payroll.Rows[i]["monthvalue"].ToString();
                string year = dt_payroll.Rows[i]["Year"].ToString();
                string Cashmode= dt_payroll.Rows[i]["Cashmode"].ToString();
                string payissued= dt_payroll.Rows[i]["payslipissued"].ToString();
                Random rdr = new Random();
                string num ="IT_"+rdr.Next(1000);


                string str_insert = "insert into IT_Payroll (Payrollid,Employeekey,Employeeid,Employeename,Salarystatus,Workingdays,Leavedays,SalaryMonth,SalaryYear,Totalsalary,Basicpay,Createdby,LOP,Netpay,Cashmode,payslipissued) " +
                    "values(@Payrollid,@Employeekey,@Employeeid,@Employeename,@Salarystatus,@Workingdays,@Leavedays,@SalaryMonth,@SalaryYear,@Totalsalary,@Basicpay,@Createdby,@LOP,@Netpay,@Cashmode,@payslipissued)";
                SqlCommand cmd = new SqlCommand(str_insert);
                cmd.Parameters.AddWithValue("@Payrollid",num);
                cmd.Parameters.AddWithValue("@Employeekey", str_empkey);
                cmd.Parameters.AddWithValue("@Employeeid", str_empid);
                cmd.Parameters.AddWithValue("@Employeename", str_empname);
                cmd.Parameters.AddWithValue("@Salarystatus", status);
                cmd.Parameters.AddWithValue("@Workingdays", workingdays);
                cmd.Parameters.AddWithValue("@Leavedays", leavedays);
                cmd.Parameters.AddWithValue("@SalaryMonth", month);
                cmd.Parameters.AddWithValue("@SalaryYear", year);
                cmd.Parameters.AddWithValue("@Totalsalary", salary);
                //cmd.Parameters.AddWithValue("@Netpay", netpay);
                cmd.Parameters.AddWithValue("@LOP", LOP);
                cmd.Parameters.AddWithValue("@Basicpay", basicpay);
                cmd.Parameters.AddWithValue("@Createdby", str_key);
                cmd.Parameters.AddWithValue("@Netpay", netpay);
                cmd.Parameters.AddWithValue("@Cashmode",Cashmode);
                cmd.Parameters.AddWithValue("@payslipissued", payissued);

                DA.ExecuteNonQuery(cmd);
            }

        }

        else
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Check Working Days and leave days";

        }
        }
    }