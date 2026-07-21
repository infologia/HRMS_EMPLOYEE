using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_employeeofthemonth : System.Web.UI.Page
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
            if (Request.QueryString["action"] == "delete" && !string.IsNullOrEmpty(Request.QueryString["key"]))
            {
                DeleteRecord(Request.QueryString["key"]);
                return;
            }

            if (Request.QueryString["msg"] == "deleted")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Record deleted successfully!');", true);
            }
            else if (Request.QueryString["msg"] == "error")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Error deleting record!');", true);
            }

            BindYear();
            BindMonth();
            BindEmployee();
            BindGrid();
        }
    }

    private void BindYear()
    {
        ddlYear.Items.Clear();
        ddlYear.Items.Add(new ListItem("Select Year", "0"));
        int currentYear = DateTime.Now.Year;
        for (int i = currentYear - 5; i <= currentYear + 5; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    private void BindMonth()
    {
        ddlMonth.Items.Clear();
        ddlMonth.Items.Add(new ListItem("Select Month", "0"));
        for (int i = 1; i <= 12; i++)
        {
            ddlMonth.Items.Add(new ListItem(new DateTime(2025, i, 1).ToString("MMMM"), i.ToString()));
        }
    }

    private void BindEmployee()
    {
        string query = @"SELECT EmployeeKey, (Firstname + ' ' + Lastname) AS EmployeeName 
                        FROM IT_EmployeeRegister 
                        WHERE Employeestatus = 1 and division in (1,2,17,18,19)
                        ORDER BY Firstname";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        ddlEmployee.Items.Clear();
        ddlEmployee.Items.Add(new ListItem("Select Employee", "0"));
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ddlEmployee.Items.Add(new ListItem(dr["EmployeeName"].ToString(), dr["EmployeeKey"].ToString()));
            }
        }
    }

    private void BindGrid()
    {
        string query = @"SELECT e.Employeeofthemonthkey, e.EmployeeYear, e.EmployeeMonth, 
                        (r.Firstname + ' ' + r.Lastname) AS EmployeeName, e.employeekey
                        FROM IT_Employeeofthemonth e
                        INNER JOIN IT_EmployeeRegister r ON e.employeekey = r.EmployeeKey
                        ORDER BY e.EmployeeYear DESC, e.EmployeeMonth DESC ,e.Createdon DESC";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt);

        if (!ds.Tables[0].Columns.Contains("MonthName"))
            ds.Tables[0].Columns.Add("MonthName");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int month = Convert.ToInt32(dr["EmployeeMonth"]);
            dr["MonthName"] = new DateTime(2025, month, 1).ToString("MMMM");
        }

        PH.LoadGridItem(ds, PH_EmployeeList, "employeeofthemonth.txt", "");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            Guid employeeKey = Guid.Parse(ddlEmployee.SelectedValue);
            Guid createdBy = Guid.Parse(SC.Userid);

            // Check if already exists
            string checkQuery = "SELECT COUNT(*) FROM IT_Employeeofthemonth WHERE EmployeeYear = @Year AND EmployeeMonth = @Month";
            SqlCommand cmdCheck = new SqlCommand(checkQuery);
            cmdCheck.Parameters.AddWithValue("@Year", year);
            cmdCheck.Parameters.AddWithValue("@Month", month);
            DataTable dtCheck = DA.GetDataTable(cmdCheck);

            if (dtCheck != null && dtCheck.Rows.Count > 0 && Convert.ToInt32(dtCheck.Rows[0][0]) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Employee of the month already exists for this year and month.');", true);
                return;
            }

            string insertQuery = @"INSERT INTO IT_Employeeofthemonth 
                                  (Employeeofthemonthkey, EmployeeYear, EmployeeMonth, Createdon, Createdby, employeekey) 
                                  VALUES (NEWID(), @Year, @Month, GETDATE(), @CreatedBy, @EmployeeKey)";
            SqlCommand cmd = new SqlCommand(insertQuery);
            cmd.Parameters.AddWithValue("@Year", year);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
            cmd.Parameters.AddWithValue("@EmployeeKey", employeeKey);

            DA.ExecuteNonQuery(cmd);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Employee of the month saved successfully!');", true);
            ClearFields();
            BindGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Guid recordKey = Guid.Parse(hfRecordKey.Value);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            Guid employeeKey = Guid.Parse(ddlEmployee.SelectedValue);

            string updateQuery = @"UPDATE IT_Employeeofthemonth 
                                  SET EmployeeYear = @Year, EmployeeMonth = @Month, employeekey = @EmployeeKey 
                                  WHERE Employeeofthemonthkey = @RecordKey";
            SqlCommand cmd = new SqlCommand(updateQuery);
            cmd.Parameters.AddWithValue("@Year", year);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@EmployeeKey", employeeKey);
            cmd.Parameters.AddWithValue("@RecordKey", recordKey);

            DA.ExecuteNonQuery(cmd);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "toastr.success('Record updated successfully!');", true);
            ClearFields();
            BindGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    private void ClearFields()
    {
        hfRecordKey.Value = "";
        ddlYear.SelectedIndex = 0;
        ddlMonth.SelectedIndex = 0;
        ddlEmployee.SelectedIndex = 0;
    }

    protected void gvEmployeeMonth_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRecord")
        {
            try
            {
                Guid key = Guid.Parse(e.CommandArgument.ToString());
                string deleteQuery = "DELETE FROM IT_Employeeofthemonth WHERE Employeeofthemonthkey = @Key";
                SqlCommand cmd = new SqlCommand(deleteQuery);
                cmd.Parameters.AddWithValue("@Key", key);
                DA.ExecuteNonQuery(cmd);

                lblMessage.Text = "Record deleted successfully!";
                lblError.Text = "";
                BindGrid();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblMessage.Text = "";
            }
        }
    }

    private void DeleteRecord(string key)
    {
        try
        {
            Guid recordKey = Guid.Parse(key);

            string deleteQuery = "DELETE FROM IT_Employeeofthemonth WHERE Employeeofthemonthkey = @Key";

            SqlCommand cmd = new SqlCommand(deleteQuery);
            cmd.Parameters.AddWithValue("@Key", recordKey);

            DA.ExecuteNonQuery(cmd);

            Response.Redirect("employeeofthemonth.aspx?msg=deleted", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            Response.Redirect("employeeofthemonth.aspx?msg=error", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}