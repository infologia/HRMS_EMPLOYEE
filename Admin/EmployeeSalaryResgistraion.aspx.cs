using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class WEB_Admin_EmployeeSalaryResgistraion : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    string str_id = "";



    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userkey = SC.Userid.ToString();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Pay";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }
    
        txt_netpay.Attributes.Add("Readonly", "Readonly");
        txt_earnings.Attributes.Add("readonly", "readonly");
        txt_deduction.Attributes.Add("readonly", "readonly");
       // txt_netpay.Attributes.Add("readonly", "readonly");

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this.str_id = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    this.str_id = Request.QueryString["id"].ToString();

                    // first get employee key
                    string empGuid = GetEmployeeKeyBySalaryId(str_id);

                    checks(empGuid);                 // dropdown bind with current employee
                    ddl_Empid.SelectedValue = empGuid;
                    this.loaddivision();
                    this.loaddepartment();
                    this.loaddesignation();
                    this.assignvalues();  
                    btn_update.Visible = true;
                   
                }

                
            }

        }
        else
        {
            if (!IsPostBack)
            {
                this.loaddivision();
                this.loaddepartment();
                this.loaddesignation();
                this.checks();
                btn_register.Visible = true;

            }
        }
    }
    
    public void checks(string currentEmpKey = null)
    {
        string str_checks = @"
        SELECT *
        FROM IT_EmployeeRegister
        WHERE
        (
            EmployeeKey NOT IN (
                SELECT EmployeeKey FROM IT_EmployeeSalaryDetails
            )
            OR EmployeeKey = @CurrentEmpKey
        )
        AND EmployeeStatus = 1
        AND Division IN (1,2,17,18,19)";

        SqlCommand cmd = new SqlCommand(str_checks);

        if (string.IsNullOrEmpty(currentEmpKey))
            cmd.Parameters.Add("@CurrentEmpKey", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
        else
            cmd.Parameters.Add("@CurrentEmpKey", SqlDbType.UniqueIdentifier).Value = new Guid(currentEmpKey);

        DataSet ds1 = DA.GetDataSet(cmd);

        ddl_Empid.Items.Clear();
        ddl_Empid.Items.Add(new ListItem("Select Employee", "0"));

        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
        {
            ddl_Empid.DataSource = ds1.Tables[0];
            ddl_Empid.DataTextField = "Employeeid";
            ddl_Empid.DataValueField = "Employeekey";
            ddl_Empid.DataBind();
        }

        ddl_Empid.SelectedValue = "0";
    }

    private string GetEmployeeKeyBySalaryId(string salaryKey)
    {
        string q = "SELECT Employeekey FROM IT_EmployeeSalaryDetails WHERE Employeesalarydetailskey=@id";
        SqlCommand cmd = new SqlCommand(q);
        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = new Guid(salaryKey);

        DataTable dt = DA.GetDataTable(cmd);
        return dt.Rows[0]["Employeekey"].ToString();
    }


    private void assignvalues()
    {
        String str_key = "SELECT S.*, E.Department, E.Division, E.Destination FROM IT_EmployeeSalaryDetails S INNER JOIN IT_EmployeeRegister E ON S.Employeekey = E.Employeekey WHERE S.Employeesalarydetailskey=@Employeesalarydetailskey";

        SqlCommand cmd = new SqlCommand(str_key);
        cmd.Parameters.AddWithValue("@Employeesalarydetailskey", this.str_id);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        if (dt_dashboard != null && dt_dashboard.Rows.Count > 0)
        {
            string empGuid = dt_dashboard.Rows[0]["Employeekey"].ToString();

            if (ddl_Empid.Items.FindByValue(empGuid) != null)
            {
                ddl_Empid.SelectedValue = empGuid;
            }

            string dep = dt_dashboard.Rows[0]["Department"].ToString();
            if (ddl_Empdep.Items.FindByValue(dep) != null)
                ddl_Empdep.SelectedValue = dep;

            string div = dt_dashboard.Rows[0]["Division"].ToString();
            if (ddl_Empdiv.Items.FindByValue(div) != null)
                ddl_Empdiv.SelectedValue = div;

            string des = dt_dashboard.Rows[0]["Destination"].ToString();
            if (ddl_Empdeg.Items.FindByValue(des) != null)
                ddl_Empdeg.SelectedValue = des;

            txt_Empname.Text = dt_dashboard.Rows[0]["Employeename"].ToString();
            txt_pfnumber.Text = dt_dashboard.Rows[0]["EmployeePFnumber"].ToString();
            txt_Esinumber.Text = dt_dashboard.Rows[0]["EmployeeESInumber"].ToString();
            txt_Pannumber.Text = dt_dashboard.Rows[0]["EmployeePanNUmber"].ToString();
            object dojValue = dt_dashboard.Rows[0]["EmployeeDOJ"];

            if (dojValue != DBNull.Value && !string.IsNullOrWhiteSpace(dojValue.ToString()))
            {
                DateTime doj;
                if (DateTime.TryParse(dojValue.ToString(), out doj))
                {
                    txt_doj.Text = doj.ToString("dd/MM/yyyy");
                }
                else
                {
                    txt_doj.Text = ""; // or log error
                }
            }
            else
            {
                txt_doj.Text = "";
            }
            txt_Monthlysalary.Text = dt_dashboard.Rows[0]["Empoloyeemonthlysalary"].ToString();


            txt_basicsalary.Text = dt_dashboard.Rows[0]["Employeebasipay"].ToString();
            txt_hra.Text = dt_dashboard.Rows[0]["EmployeeHRA"].ToString();
            txt_mediall.Text = dt_dashboard.Rows[0]["Employeemedicalallowance"].ToString();
            txt_Conveyance.Text = dt_dashboard.Rows[0]["Employeeconveyance"].ToString();
            txt_pfamount.Text = dt_dashboard.Rows[0]["Employeepfamoount"].ToString();
       
            txt_esiamount.Text = dt_dashboard.Rows[0]["Employeeesiamount"].ToString();
            txt_earnings.Text = dt_dashboard.Rows[0]["Totalearnings"].ToString();
            txt_deduction.Text = dt_dashboard.Rows[0]["Totaldeduction"].ToString();
            txt_netpay.Text = dt_dashboard.Rows[0]["Netpay"].ToString();
            txt_allowance.Text = dt_dashboard.Rows[0]["SPLallowance"].ToString();


            txt_hraint.Text = dt_dashboard.Rows[0]["Hrainterest"].ToString();
            txtpfint.Text = dt_dashboard.Rows[0]["PFinterest"].ToString();
            txt_esiint.Text = dt_dashboard.Rows[0]["ESIinterest"].ToString();
            txt_mdint.Text = dt_dashboard.Rows[0]["Medicalinterest"].ToString();
            txt_conveyint.Text = dt_dashboard.Rows[0]["Conveyenceinterest"].ToString();
            txt_basicint.Text = dt_dashboard.Rows[0]["Basicinterest"].ToString();
            txt_splint.Text = dt_dashboard.Rows[0]["Splinterest"].ToString();

            txt_Empname.Attributes.Add("Readonly", "Readonly");
            //txt_netpay.Attributes.Add("Readonly", "Readonly");
            //txt_Conveyance.Attributes.Add("Readonly", "Readonly");
            //txt_allowance.Attributes.Add("Readonly", "Readonly");

            //txt_basicint.Attributes.Add("Readonly", "Readonly");
            //txt_conveyint.Attributes.Add("Readonly", "Readonly");
            //txt_esiint.Attributes.Add("Readonly", "Readonly");
            //txt_hraint.Attributes.Add("Readonly", "Readonly");
            //txt_mdint.Attributes.Add("Readonly", "Readonly");
            //txt_splint.Attributes.Add("Readonly", "Readonly");
            txt_earnings.Attributes.Add("readonly", "readonly");
            txt_deduction.Attributes.Add("readonly", "readonly");
            txt_netpay.Attributes.Add("readonly", "readonly");

            ddl_Empid.Attributes.Add("style", "pointer-events: none; background-color: #fafafa;");
            ddl_Empid.Attributes.Add("onfocus", "this.blur();");
           
        }
    }
   
    private void loaddesignation()
    {
        string str_des = "select Destinationid,Destinationname from IT_Destination";
        SqlCommand cmd = new SqlCommand(str_des);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_Empdeg.DataSource = ds1.Tables[0];
            ddl_Empdeg.DataTextField = "Destinationname";
            ddl_Empdeg.DataValueField = "Destinationid";
            ddl_Empdeg.DataBind();
            ddl_Empdeg.Items.Add(new ListItem("Select  Destinationname ", "0"));
            ddl_Empdeg.SelectedValue = "0";
        }
    }

    private void loaddepartment()
    {
        string str_dep = "select Departmentid,Departmentname from IT_Department";
        SqlCommand cmd = new SqlCommand(str_dep);
        DataSet ds2 = this.DA.GetDataSet(cmd);
        if (ds2 != null && ds2.Tables.Count > 0)
        {
            ddl_Empdep.DataSource = ds2.Tables[0];
            ddl_Empdep.DataTextField = "Departmentname";
            ddl_Empdep.DataValueField = "Departmentid";
            ddl_Empdep.DataBind();
            ddl_Empdep.Items.Add(new ListItem("Select  Departmentname", "0"));
            ddl_Empdep.SelectedValue = "0";
        }
    }

    private void loaddivision()
    {
        string str_URL = "select Divisionid,Divisionname from IT_Division";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds3 = this.DA.GetDataSet(cmd);
        if (ds3 != null && ds3.Tables.Count > 0)
        {
            ddl_Empdiv.DataSource = ds3.Tables[0];
            ddl_Empdiv.DataTextField = "Divisionname";
            ddl_Empdiv.DataValueField = "Divisionid";
            ddl_Empdiv.DataBind();
            ddl_Empdiv.Items.Add(new ListItem("Select  Divisionname", "0"));
            ddl_Empdiv.SelectedValue = "0";
        }
    }

    protected void ddl_Empid_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str_empdatils = "select Username,Destination,Division,Department from IT_EmployeeRegister where Employeekey=@Employeekey";
        SqlCommand cmd = new SqlCommand(str_empdatils);
        cmd.Parameters.AddWithValue("@Employeekey", ddl_Empid.SelectedValue);
        DataTable dt_empdetails = DA.GetDataTable(cmd);
        if (dt_empdetails.Rows.Count > 0)
        {
            txt_Empname.Attributes.Add("Readonly", "Readonly");
            txt_netpay.Attributes.Add("Readonly", "Readonly");
         
            txt_Empname.Text = dt_empdetails.Rows[0]["Username"].ToString();
            //ddl_Empdep.SelectedValue = dt_empdetails.Rows[0]["Department"].ToString();
            string dep = dt_empdetails.Rows[0]["Department"].ToString();
            var item = ddl_Empdep.Items.FindByValue(dep);

            if (item != null)
                ddl_Empdep.SelectedValue = dep;
            else
                ddl_Empdep.SelectedIndex = 0; // fallback

            string div = dt_empdetails.Rows[0]["Division"].ToString();
            if (ddl_Empdiv.Items.FindByValue(div) != null)
                ddl_Empdiv.SelectedValue = div;
            else
                ddl_Empdiv.SelectedIndex = 0;

            string desg = dt_empdetails.Rows[0]["Destination"].ToString();
            if (ddl_Empdeg.Items.FindByValue(desg) != null)
                ddl_Empdeg.SelectedValue = desg;
            else
                ddl_Empdeg.SelectedIndex = 0;


            //ddl_Empdiv.SelectedValue = dt_empdetails.Rows[0]["Division"].ToString();
            //ddl_Empdeg.SelectedValue = dt_empdetails.Rows[0]["Destination"].ToString();
        }

    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        this.str_userkey = SC.Userid.ToString();
        string str_insert = "insert into IT_EmployeeSalaryDetails (Employeekey,Employeename,Employeedepartment,EmployeeDivision,EmployeeDesigination,EmployeePFnumber,EmployeeESInumber,EmployeePanNUmber,EmployeeDOJ,Empoloyeemonthlysalary,Employeebasipay,EmployeeHRA,Employeemedicalallowance,Employeeconveyance,Employeepfamoount,Employeeesiamount,Employeetdsamount,NetPay,createdby,PFinterest,ESIinterest,Medicalinterest,Conveyenceinterest,Basicinterest,TDSinterest,Splinterest,Totalearnings,Totaldeduction,Hrainterest,SPLallowance)values(@Employeekey,@Employeename,@Employeedepartment,@EmployeeDivision,@EmployeeDesigination,@EmployeePFnumber,@EmployeeESInumber,@EmployeePanNUmber,@EmployeeDOJ,@Empoloyeemonthlysalary,@Employeebasipay,@EmployeeHRA,@Employeemedicalallowance,@Employeeconveyance,@Employeepfamoount,@Employeeesiamount,@Employeetdsamount,@NetPay,@createdby,@PFinterest,@ESIinterest,@Medicalinterest,@Conveyanceinterest,@Basicinterest,@TDSinterest,@Splinterest,@Totalearnings,@Totaldeduction,@Hrainterest,@SPLallowance)";
        SqlCommand sc = new SqlCommand(str_insert);
        sc.Parameters.AddWithValue("Employeekey", ddl_Empid.SelectedValue);
        sc.Parameters.AddWithValue("@Employeename", txt_Empname.Text);
        sc.Parameters.AddWithValue("@Employeedepartment", ddl_Empdep.SelectedValue);
        sc.Parameters.AddWithValue("EmployeeDivision", ddl_Empdiv.SelectedValue);
        sc.Parameters.AddWithValue("@EmployeeDesigination", ddl_Empdeg.SelectedValue);
        sc.Parameters.AddWithValue("@EmployeePFnumber", txt_pfnumber.Text);
        sc.Parameters.AddWithValue("@EmployeeESInumber", txt_Esinumber.Text);
        sc.Parameters.AddWithValue("@EmployeePanNUmber", txt_Pannumber.Text);
        if (string.IsNullOrWhiteSpace(txt_doj.Text))
        {
            sc.Parameters.Add("@EmployeeDOJ", SqlDbType.DateTime).Value = DBNull.Value;
        }
        else
        {
            DateTime doj;
            string[] formats = { "dd/MM/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };

            if (DateTime.TryParseExact(txt_doj.Text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out doj))
            {
                sc.Parameters.Add("@EmployeeDOJ", SqlDbType.DateTime).Value = doj;
            }
            else
            {
                throw new Exception("Invalid Date: " + txt_doj.Text);
            }
        }
        sc.Parameters.Add("@Empoloyeemonthlysalary", SqlDbType.Decimal).Value =
    string.IsNullOrEmpty(txt_Monthlysalary.Text) ? 0 : Convert.ToDecimal(txt_Monthlysalary.Text);
        sc.Parameters.AddWithValue("@Employeebasipay", string.IsNullOrEmpty(txt_basicsalary.Text) ? "0" : txt_basicsalary.Text);
        sc.Parameters.AddWithValue("@EmployeeHRA", string.IsNullOrEmpty(txt_hra.Text) ? "0" : txt_hra.Text);
        sc.Parameters.AddWithValue("@Employeemedicalallowance", string.IsNullOrEmpty(txt_mediall.Text) ? "0" : txt_mediall.Text);
        sc.Parameters.AddWithValue("@Employeeconveyance", string.IsNullOrEmpty(txt_Conveyance.Text) ? "0" : txt_Conveyance.Text);
        sc.Parameters.AddWithValue("@Employeepfamoount", string.IsNullOrEmpty(txt_pfamount.Text) ? "0" : txt_pfamount.Text);
        sc.Parameters.AddWithValue("@Employeeesiamount", string.IsNullOrEmpty(txt_esiamount.Text) ? "0" : txt_esiamount.Text);
        sc.Parameters.AddWithValue("@Employeetdsamount", "0");
        sc.Parameters.AddWithValue("@NetPay", string.IsNullOrEmpty(txt_netpay.Text) ? "0" : txt_netpay.Text);
        sc.Parameters.AddWithValue("@createdby", str_userkey);
        sc.Parameters.AddWithValue("@PFinterest", string.IsNullOrEmpty(txtpfint.Text) ? "0" : txtpfint.Text);
        sc.Parameters.AddWithValue("@ESIinterest", string.IsNullOrEmpty(txt_esiint.Text) ? "0" : txt_esiint.Text);
        sc.Parameters.AddWithValue("@Medicalinterest", string.IsNullOrEmpty(txt_mdint.Text) ? "0" : txt_mdint.Text);
        sc.Parameters.AddWithValue("@Conveyanceinterest", string.IsNullOrEmpty(txt_conveyint.Text) ? "0" : txt_conveyint.Text);
        sc.Parameters.AddWithValue("@Basicinterest", string.IsNullOrEmpty(txt_basicint.Text) ? "0" : txt_basicint.Text);
        sc.Parameters.AddWithValue("@TDSinterest", "0");
        sc.Parameters.AddWithValue("@Splinterest", string.IsNullOrEmpty(txt_splint.Text) ? "0" : txt_splint.Text);
        sc.Parameters.AddWithValue("@Totalearnings", string.IsNullOrEmpty(txt_earnings.Text) ? "0" : txt_earnings.Text);
        sc.Parameters.AddWithValue("@Totaldeduction", string.IsNullOrEmpty(txt_deduction.Text) ? "0" : txt_deduction.Text);
        sc.Parameters.AddWithValue("@Hrainterest", string.IsNullOrEmpty(txt_hraint.Text) ? "0" : txt_hraint.Text);
        sc.Parameters.AddWithValue("@SPLallowance", string.IsNullOrEmpty(txt_allowance.Text) ? "0" : txt_allowance.Text);

        DA.ExecuteNonQuery(sc);
        Response.Redirect("~/admin/Salarydetails.aspx");


    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        Guid userKey = Guid.Parse(SC.Userid.ToString());
        Guid salaryKey = Guid.Parse(str_id);

        //this.str_userkey = SC.Userid.ToString();

        //string str_modifiedon = DateTime.UtcNow.ToString();
        string str_insert = "Update IT_EmployeeSalaryDetails set Employeekey=@Employeekey,Employeename=@Employeename,Employeedepartment=@Employeedepartment,EmployeeDivision=@EmployeeDivision,EmployeeDesigination=@EmployeeDesigination,EmployeePFnumber=@EmployeePFnumber,EmployeeESInumber=@EmployeeESInumber,EmployeePanNUmber=@EmployeePanNUmber,EmployeeDOJ=@EmployeeDOJ,Empoloyeemonthlysalary=@Empoloyeemonthlysalary,Employeebasipay=@Employeebasipay,EmployeeHRA=@EmployeeHRA,Employeemedicalallowance=@Employeemedicalallowance,Employeeconveyance=@Employeeconveyance,Employeepfamoount=@Employeepfamoount,Employeeesiamount=@Employeeesiamount,Employeetdsamount=@Employeetdsamount,NetPay=@NetPay,Modifiedby=@Modifiedby,Modifiedon=@Modifiedon,PFinterest=@PFinterest,ESIinterest=@ESIinterest,Medicalinterest=@Medicalinterest,Conveyenceinterest=@Conveyanceinterest,Basicinterest=@Basicinterest,TDSinterest=@TDSinterest,Splinterest=@Splinterest,Totalearnings=@Totalearnings,Totaldeduction=@Totaldeduction,Hrainterest=@Hrainterest,SPLallowance=@SPLallowance where Employeesalarydetailskey=@Employeesalarydetailskey";
        SqlCommand sc = new SqlCommand(str_insert);
        sc.Parameters.Add("@Employeesalarydetailskey", SqlDbType.UniqueIdentifier)
                     .Value = salaryKey;
        sc.Parameters.AddWithValue("Employeekey", ddl_Empid.SelectedValue);
        sc.Parameters.AddWithValue("@Employeename", txt_Empname.Text);
        sc.Parameters.AddWithValue("@Employeedepartment", ddl_Empdep.SelectedValue);
        sc.Parameters.AddWithValue("EmployeeDivision", ddl_Empdiv.SelectedValue);
        sc.Parameters.AddWithValue("@EmployeeDesigination", ddl_Empdeg.SelectedValue);
        sc.Parameters.AddWithValue("@EmployeePFnumber", txt_pfnumber.Text);
        sc.Parameters.AddWithValue("@EmployeeESInumber", txt_Esinumber.Text);
        sc.Parameters.AddWithValue("@EmployeePanNUmber", txt_Pannumber.Text);
        if (string.IsNullOrWhiteSpace(txt_doj.Text))
        {
            sc.Parameters.Add("@EmployeeDOJ", SqlDbType.DateTime).Value = DBNull.Value;
        }
        else
        {
            DateTime doj;
            string[] formats = { "dd/MM/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };

            if (DateTime.TryParseExact(txt_doj.Text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out doj))
            {
                sc.Parameters.Add("@EmployeeDOJ", SqlDbType.DateTime).Value = doj;
            }
            else
            {
                throw new Exception("Invalid Date: " + txt_doj.Text);
            }
        }
        sc.Parameters.Add("@Empoloyeemonthlysalary", SqlDbType.Decimal).Value =
    string.IsNullOrEmpty(txt_Monthlysalary.Text) ? 0 : Convert.ToDecimal(txt_Monthlysalary.Text);
        sc.Parameters.AddWithValue("@Employeebasipay", string.IsNullOrEmpty(txt_basicsalary.Text) ? "0" : txt_basicsalary.Text);
        sc.Parameters.AddWithValue("@EmployeeHRA", string.IsNullOrEmpty(txt_hra.Text) ? "0" : txt_hra.Text);
        sc.Parameters.AddWithValue("@Employeemedicalallowance", string.IsNullOrEmpty(txt_mediall.Text) ? "0" : txt_mediall.Text);
        sc.Parameters.AddWithValue("@Employeeconveyance", string.IsNullOrEmpty(txt_Conveyance.Text) ? "0" : txt_Conveyance.Text);
        sc.Parameters.AddWithValue("@Employeepfamoount", string.IsNullOrEmpty(txt_pfamount.Text) ? "0" : txt_pfamount.Text);
        sc.Parameters.AddWithValue("@Employeeesiamount", string.IsNullOrEmpty(txt_esiamount.Text) ? "0" : txt_esiamount.Text);
        sc.Parameters.AddWithValue("@Employeetdsamount", "0");
        sc.Parameters.AddWithValue("@NetPay", string.IsNullOrEmpty(txt_netpay.Text) ? "0" : txt_netpay.Text);
        sc.Parameters.Add("@Modifiedby", SqlDbType.UniqueIdentifier)
                     .Value = userKey;
        sc.Parameters.Add("@Modifiedon", SqlDbType.DateTime).Value = DateTime.UtcNow;
        sc.Parameters.AddWithValue("@PFinterest", string.IsNullOrEmpty(txtpfint.Text) ? "0" : txtpfint.Text);
        sc.Parameters.AddWithValue("@ESIinterest", string.IsNullOrEmpty(txt_esiint.Text) ? "0" : txt_esiint.Text);
        sc.Parameters.AddWithValue("@Medicalinterest", string.IsNullOrEmpty(txt_mdint.Text) ? "0" : txt_mdint.Text);
        sc.Parameters.AddWithValue("@Conveyanceinterest", string.IsNullOrEmpty(txt_conveyint.Text) ? "0" : txt_conveyint.Text);
        sc.Parameters.AddWithValue("@Basicinterest", string.IsNullOrEmpty(txt_basicint.Text) ? "0" : txt_basicint.Text);
        sc.Parameters.AddWithValue("@TDSinterest", "0");
        sc.Parameters.AddWithValue("@Splinterest", string.IsNullOrEmpty(txt_splint.Text) ? "0" : txt_splint.Text);
        sc.Parameters.AddWithValue("@Totalearnings", string.IsNullOrEmpty(txt_earnings.Text) ? "0" : txt_earnings.Text);
        sc.Parameters.AddWithValue("@Totaldeduction", string.IsNullOrEmpty(txt_deduction.Text) ? "0" : txt_deduction.Text);
        sc.Parameters.AddWithValue("@Hrainterest", string.IsNullOrEmpty(txt_hraint.Text) ? "0" : txt_hraint.Text);
        sc.Parameters.AddWithValue("@SPLallowance", string.IsNullOrEmpty(txt_allowance.Text) ? "0" : txt_allowance.Text);

        DA.ExecuteNonQuery(sc);
        Response.Redirect("~/admin/Salarydetails.aspx");
    }

    protected void btn_calculate_Click(object sender, EventArgs e)
    {
        decimal basicsalary = string.IsNullOrEmpty(txt_basicsalary.Text) ? 0 : Convert.ToDecimal(txt_basicsalary.Text); decimal hra = string.IsNullOrEmpty(txt_hra.Text) ? 0 : decimal.Parse(txt_hra.Text);
        decimal mediall = string.IsNullOrEmpty(txt_mediall.Text) ? 0 : decimal.Parse(txt_mediall.Text);
        decimal conveyance = string.IsNullOrEmpty(txt_Conveyance.Text) ? 0 : decimal.Parse(txt_Conveyance.Text);
        decimal allowance = string.IsNullOrEmpty(txt_allowance.Text) ? 0 : decimal.Parse(txt_allowance.Text);

        decimal totalEarnings = basicsalary + hra + mediall + conveyance + allowance;
        txt_earnings.Text = totalEarnings.ToString();

        decimal pfamount = string.IsNullOrEmpty(txt_pfamount.Text) ? 0 : decimal.Parse(txt_pfamount.Text);
        decimal esiamount = string.IsNullOrEmpty(txt_esiamount.Text) ? 0 : decimal.Parse(txt_esiamount.Text);

        decimal totalDeduction = pfamount + esiamount;
        txt_deduction.Text = totalDeduction.ToString();

        decimal netPay = totalEarnings - totalDeduction;
        txt_netpay.Text = netPay.ToString();
    }



}