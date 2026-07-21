using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_UpdatePermission : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_mode = "";
    string str_requestdate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Permissions";
        }

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this.str_id = Request.QueryString["id"].ToString();
        }
        if (Request.QueryString["mode"] != null)
        {
            this.str_mode = Request.QueryString["mode"].ToString();
        }

        if (!IsPostBack)
        {
            this.loaddropdown();
            this.Loadlanguage();
        }
    }

    public void loaddropdown()
    {

        string str_URL = "   select * from StatusResponse order by StatusResponsekey";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_category.DataSource = ds1.Tables[0];
            ddl_category.DataTextField = "status";
            ddl_category.DataValueField = "StatusResponseId";

            ddl_category.DataBind();
            ddl_category.Items.Add(new ListItem("Select Status", "0"));
            ddl_category.SelectedValue = "0";

        }
    }
    private void Loadlanguage()
    {
        string str_query = "select CONVERT(varchar(10), Requestdate, 103) AS Requestdate,Fromtime,Totime,Reason,Responsestatus,responsereason from IT_EmployeePermissionDetails  where employeepermissiondetailskey=@employeepermissiondetailskey";
        SqlCommand sc = new SqlCommand(str_query);
        sc.Parameters.AddWithValue("@employeepermissiondetailskey", str_id);
        DataTable ds = this.DA.GetDataTable(sc);
        if (ds.Rows.Count > 0)
        {
            string str_leavestus = ds.Rows[0]["Responsestatus"].ToString();
            if (str_leavestus == "1" && this.str_mode != "view")
            {
                div_Reson.Visible = false;
                txt_date.Text = ds.Rows[0]["Requestdate"].ToString();
                txt_fromtime.Text = ds.Rows[0]["Fromtime"].ToString();
                txt_totime.Text = ds.Rows[0]["Totime"].ToString();
                txt_reasons.InnerText = ds.Rows[0]["Reason"].ToString();
                btn_request.Visible = true;
            }
            else
            {
                div_Reson.Visible = true;
                txt_date.Text = ds.Rows[0]["Requestdate"].ToString();
                txt_fromtime.Text = ds.Rows[0]["Fromtime"].ToString();
                txt_totime.Text = ds.Rows[0]["Totime"].ToString();
                txt_reasons.InnerText = ds.Rows[0]["Reason"].ToString();

                txt_date.Attributes.Add("Readonly", "Readonly");
                txt_reason1.Attributes.Add("Readonly", "Readonly");
                ddl_category.Attributes.Add("disabled", "disabled");
                txt_fromtime.Attributes.Add("Readonly", "Readonly");
                txt_totime.Attributes.Add("Readonly", "Readonly");
                txt_reasons.Attributes.Add("Readonly", "Readonly");


                ddl_category.SelectedValue = str_leavestus;
                txt_reason1.InnerText = ds.Rows[0]["responsereason"].ToString();
                btn_request.Visible = false;
            }

        }
    }
    protected void btn_request_Click(object sender, EventArgs e)
    {
        if (txt_date.Text == "" && txt_fromtime.Text == "" && txt_totime.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select Required Field');</script>");
            return;
        }
        if (txt_date.Text == "" || txt_fromtime.Text == "" || txt_totime.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select Required Field');</script>");
            return;
        }

        //DateTime dt_current = DateTime.Now;
        //DateTime dt_date = Convert.ToDateTime(txt_date.Text);
        DateTime dt_current = DateTime.Now.Date;
        DateTime dt_date;

        bool isValid = DateTime.TryParseExact(
            txt_date.Text.Trim(),
            "dd/MM/yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out dt_date
        );

        if (!isValid)
        {
            // show error
            return;
        }

        dt_date = dt_date.Date;


        dt_current = dt_current.Date;
        if (dt_current > dt_date)
        {
            
            ShowError("Please select valid date");
            return;
        }

        //Validatetime For compnay times//
        TimeSpan tf = DateTime.Parse("09:30 AM").TimeOfDay;
        TimeSpan tt = DateTime.Parse("06:30 PM").TimeOfDay;

        string str_fromtime = txt_fromtime.Text;
        string str_totime = txt_totime.Text;

        TimeSpan tf1 = DateTime.Parse(str_fromtime).TimeOfDay;
        TimeSpan tt1 = DateTime.Parse(str_totime).TimeOfDay;

        if (tf > tf1)
        {
          
            ShowError("Please select valid From Time");
            return;
        }
        else if (tt < tt1)
        {
           
            ShowError("Please select valid To Time");
            return;
        }

        else if (tf > tt1)
        {
          
            ShowError("Please select valid To Time");
            return;
        }
        else if (tt < tf1)
        {
            ShowError("Please select valid To Time");
            return;
        }
        else if (tt1 < tf)
        {
            ShowError("Please select valid To Time");
            return;
        }
        else if (tf1 > tt)
        {
            ShowError("Please select valid To Time");
            return;
        }
        else if (tf1 == tt1)
        {
            ShowError("Please select valid To Time");
            return;
        }
        else if (tf1 > tt1)
        {
            ShowError("Please select valid To Time");
            return;
        }

        else if (tt1 < tf1)
        {
              ShowError("Please select valid To Time");
            return;
        }



        //hours Calculation
        DateTime dt = Convert.ToDateTime(txt_fromtime.Text);
        DateTime dt1 = Convert.ToDateTime(txt_totime.Text);
        TimeSpan ts = dt1 - dt;



        DateTime modifiedOn = DateTime.Now;

        // Safe date parsing
        string[] allowedFormats = { "dd/MM/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };
        DateTime requestDate;
        bool isValidDate = DateTime.TryParseExact(txt_date.Text.Trim(), allowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out requestDate);
        if (!isValidDate)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please enter a valid date in DD/MM/YYYY format');</script>");
            return;
        }

        Guid permissionKey = Guid.Parse(str_id);


        string str_sql = "Update  IT_EmployeePermissionDetails SET Fromtime=@Fromtime,permissionhourse=@permissionhourse,Totime=@Totime,Reason=@Reason,Requestdate=@Requestdate,modifiedon=@modifiedon,Modifiedby=@Modifiedby where employeepermissiondetailskey=@employeepermissiondetailskey ";
        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@employeepermissiondetailskey", permissionKey);
        cmd.Parameters.AddWithValue("@Modifiedby", SC.Userid);
        cmd.Parameters.AddWithValue("@modifiedon",modifiedOn);
        cmd.Parameters.AddWithValue("@Requestdate", requestDate);
        cmd.Parameters.AddWithValue("@Fromtime", txt_fromtime.Text);
        cmd.Parameters.AddWithValue("@permissionhourse", ts);
        // cmd.Parameters.AddWithValue("@Requestdate",txt_date.Text);
        cmd.Parameters.AddWithValue("@Totime", txt_totime.Text);
        cmd.Parameters.AddWithValue("@Reason", txt_reasons.InnerText);
        DA.ExecuteNonQuery(cmd);
        ShowSuccessAndRedirect(
        "Request updated successfully!",
        "/Employee/PermissionRequestView.aspx"
   
        
        
        );
    }
    private void ShowError(string message)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_error",
            "showToastr('error','" + message + "');",
            true
        );
    }

    private void ShowSuccessAndRedirect(string message, string redirectUrl)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_success",
            "showToastr('success','" + message + "');" +
            "setTimeout(function(){ window.location.href = '" + redirectUrl + "'; }, 2000);",
            true
        );
    }

}