using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_worktyperequest : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    private string key = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            this.key = Request.QueryString["id"].ToString();

        if (!IsPostBack)
        {
            BindEmployeeDropdown();
            BindWorkTypeDropdown();

            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Work Type Request";

            if (!string.IsNullOrEmpty(key))
            {
                LoadRequestData();
                btn_request.Text = "Update";
            }
        }
    }

    private void BindEmployeeDropdown()
    {
        string query = @"SELECT Employeekey, Firstname + ' ' + Lastname AS Username
            FROM IT_EmployeeRegister
            WHERE Employeestatus = 1 AND Division IN (1,2,3,17,18,19)
            ORDER BY Username";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        ddlEmployee.DataSource = dt;
        ddlEmployee.DataTextField = "Username";
        ddlEmployee.DataValueField = "Employeekey";
        ddlEmployee.DataBind();

        ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
    }

    private void BindWorkTypeDropdown()
    {
        string query = @"SELECT WT_Id, WT_TypeName FROM IT_WorkType ORDER BY WT_TypeName";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);
        ddlWorkType.DataSource = dt;
        ddlWorkType.DataTextField = "WT_TypeName";
        ddlWorkType.DataValueField = "WT_Id";
        ddlWorkType.DataBind();

        ddlWorkType.Items.Insert(0, new ListItem("Select Work Type", "0"));
    }

    private void LoadRequestData()
    {
        string query = @"SELECT Employeekey, WorkTypeId, FromDate, ToDate, Reason
            FROM IT_WorkTypeRequest
            WHERE WR_Id = @RequestId";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@RequestId", key);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
        {
            ddlEmployee.SelectedValue = dt.Rows[0]["Employeekey"].ToString();
            ddlWorkType.SelectedValue = dt.Rows[0]["WorkTypeId"].ToString();
            
            DateTime fromDate = Convert.ToDateTime(dt.Rows[0]["FromDate"]);
            DateTime toDate = Convert.ToDateTime(dt.Rows[0]["ToDate"]);
            
            txt_fromdate.Text = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            txt_todate.Text = toDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            txt_reason.Value = dt.Rows[0]["Reason"].ToString();
        }
    }

    protected void btn_request_Click(object sender, EventArgs e)
    {
        if (ddlEmployee.SelectedValue == "0")
        {
            ShowError("Please select an employee");
            return;
        }

        if (ddlWorkType.SelectedValue == "0")
        {
            ShowError("Please select a work type");
            return;
        }

        DateTime fromDate, toDate;
        if (!DateTime.TryParseExact(txt_fromdate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
        {
            ShowError("Invalid From date");
            return;
        }

        if (!DateTime.TryParseExact(txt_todate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
        {
            ShowError("Invalid To date");
            return;
        }

        if (fromDate > toDate)
        {
            ShowError("From date cannot be greater than To date");
            return;
        }

        Guid empGuid, userGuid;
        if (!Guid.TryParse(ddlEmployee.SelectedValue, out empGuid))
        {
            ShowError("Invalid Employee GUID");
            return;
        }

        if (!Guid.TryParse(SC.Userid, out userGuid))
        {
            ShowError("Invalid User GUID");
            return;
        }

        using (SqlConnection conn = new SqlConnection(DA.ConnectionString))
        {
            conn.Open();

            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    // Insert
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO IT_WorkTypeRequest (Employeekey, WorkTypeId, FromDate, ToDate, Reason, CreatedBy, CreatedOn)
                            VALUES (@Employeekey, @WorkTypeId, @FromDate, @ToDate, @Reason, @CreatedBy, @CreatedOn)";

                        cmd.Parameters.Add("@Employeekey", SqlDbType.UniqueIdentifier).Value = empGuid;
                        cmd.Parameters.Add("@WorkTypeId", SqlDbType.Int).Value = ddlWorkType.SelectedValue;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = txt_reason.Value.Trim();
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userGuid;
                        cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.ExecuteNonQuery();
                    }
                    ShowSuccessAndRedirect("Request created successfully!", "worktyperequests.aspx");
                }
                else
                {
                    // Update
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE IT_WorkTypeRequest 
                            SET Employeekey = @Employeekey, WorkTypeId = @WorkTypeId, FromDate = @FromDate, 
                                ToDate = @ToDate, Reason = @Reason, ModifiedBy = @ModifiedBy, ModifiedOn = @ModifiedOn
                            WHERE WR_Id = @RequestId";

                        cmd.Parameters.Add("@Employeekey", SqlDbType.UniqueIdentifier).Value = empGuid;
                        cmd.Parameters.Add("@WorkTypeId", SqlDbType.Int).Value = ddlWorkType.SelectedValue;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = txt_reason.Value.Trim();
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userGuid;
                        cmd.Parameters.Add("@ModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@RequestId", SqlDbType.Int).Value = key;

                        cmd.ExecuteNonQuery();
                    }
                    ShowSuccessAndRedirect("Request updated successfully!", "worktyperequests.aspx");
                }
            }
            catch (Exception ex)
            {
                ShowError("Error: " + ex.Message);
                return;
            }
        }
    }

    private void ShowError(string message)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "showToastr('error','" + message + "');", true);
    }

    private void ShowSuccessAndRedirect(string message, string redirectUrl)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "showToastr('success','" + message + "');setTimeout(function(){ window.location.href = '" + redirectUrl + "'; }, 2000);", true);
    }

   
}
