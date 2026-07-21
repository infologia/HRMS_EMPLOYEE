using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Employee_UpdateLaterecord : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_requestdate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Late Permissions";
        }

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this.str_id = Request.QueryString["id"].ToString();
        }

        if (!IsPostBack)
        {
          
            this.Loadlanguage();
        }
    }

    
    private void Loadlanguage()
    {
        string str_query = "select * from IT_LatePermissionDetails  where LatePermissionDetailskey=@LatePermissionDetailskey";
        SqlCommand sc = new SqlCommand(str_query);
        sc.Parameters.AddWithValue("@LatePermissionDetailskey", str_id);
        DataTable ds = this.DA.GetDataTable(sc);
        if (ds.Rows.Count > 0)
        {
            string str_leavestus = ds.Rows[0]["responsereason"].ToString();
            if (str_leavestus == "")
            {
                div_Reson.Visible = false;
                DateTime reqDate;

                if (DateTime.TryParse(ds.Rows[0]["Requestdate"].ToString(), out reqDate))
                {
                    txt_date.Text = reqDate.ToString("yyyy-MM-dd");
                }
                txt_fromtime.Text = ds.Rows[0]["Fromtime"].ToString();
                txt_totime.Text = ds.Rows[0]["Totime"].ToString();
                txt_reasons.InnerText = ds.Rows[0]["Reason"].ToString();
                btn_request.Visible = true;
            }
            else
            {
                div_Reson.Visible = true;
                txt_fromtime.Text = ds.Rows[0]["Fromtime"].ToString();
                txt_totime.Text = ds.Rows[0]["Totime"].ToString();
                txt_reasons.InnerText = ds.Rows[0]["Reason"].ToString();
                txt_date.Text = ds.Rows[0]["Requestdate"].ToString();

                txt_date.Attributes.Add("Readonly", "Readonly");
                txt_reason1.Attributes.Add("Readonly", "Readonly");
               
                txt_fromtime.Attributes.Add("Readonly", "Readonly");
                txt_totime.Attributes.Add("Readonly", "Readonly");
                txt_reasons.Attributes.Add("Readonly", "Readonly");
                txt_reason1.InnerText = ds.Rows[0]["responsereason"].ToString();
                btn_request.Visible = false;
            }

        }
    }
    protected void btn_request_Click(object sender, EventArgs e)
    {
     
        if (string.IsNullOrWhiteSpace(txt_date.Text) ||
            string.IsNullOrWhiteSpace(txt_fromtime.Text) ||
            string.IsNullOrWhiteSpace(txt_totime.Text))
        {
            ShowError("Please fill all required fields.");
            return;
        }


        DateTime requestDate;
        string[] acceptedFormats = { "yyyy-MM-dd", "dd/MM/yyyy" };

        bool validDate = DateTime.TryParseExact(
            txt_date.Text.Trim(),
            acceptedFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out requestDate
        );

        if (!validDate)
        {
            ShowError("Invalid date format.");
            return;
        }

        if (requestDate.Date != DateTime.Now.Date)
        {
            ShowError("Late Permission can be applied only for today.");
            return;
        }


        DateTime fromTime, toTime;

        bool fromOk = DateTime.TryParse(txt_fromtime.Text, out fromTime);
        bool toOk = DateTime.TryParse(txt_totime.Text, out toTime);

        if (!fromOk || !toOk)
        {
            ShowError("Invalid From Time or To Time.");
            return;
        }

        if (toTime <= fromTime)
        {
            ShowError("Please correct to time");
            return;
        }

     
        TimeSpan permissionHours = toTime - fromTime;

  
        string sql = @"
        UPDATE IT_LatePermissionDetails
        SET 
            Fromtime = @Fromtime,
            Totime = @Totime,
            permissionhourse = @permissionhourse,
            Reason = @Reason,
            Requestdate = @Requestdate,
            Modifiedon = @Modifiedon,
            Modifiedby = @Modifiedby
        WHERE LatePermissionDetailskey = @LatePermissionDetailskey";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@LatePermissionDetailskey", str_id);
        cmd.Parameters.AddWithValue("@Fromtime", fromTime.ToString("HH:mm tt"));
        cmd.Parameters.AddWithValue("@Totime", toTime.ToString("HH:mm tt"));
        cmd.Parameters.AddWithValue("@permissionhourse", permissionHours.ToString());
        cmd.Parameters.AddWithValue("@Reason", txt_reasons.InnerText.Trim());
        cmd.Parameters.AddWithValue("@Requestdate", requestDate);
        cmd.Parameters.AddWithValue("@Modifiedon", DateTime.Now);
        cmd.Parameters.AddWithValue("@Modifiedby", SC.Userid);

        DA.ExecuteNonQuery(cmd);

   
        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "update_success",
            "showToastr('success','Late permission updated successfully!');" +
            "setTimeout(function(){ window.location.href = '/Employee/LatePermissionRequestView.aspx'; }, 2000);",
            true
        );
    }

    private void ShowError(string message)
    {
        message = message.Replace("'", "\\'");

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_error",
            string.Format("showToastr('error','{0}');", message),
            true
        );
    }


}