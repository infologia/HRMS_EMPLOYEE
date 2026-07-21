using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Admin_AmcDetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    protected void Page_Load(object sender, EventArgs e)
    {
       

        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (Page.Form != null) { Page.Form.Attributes.Add("enctype", "multipart/form-data"); }
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "AMC";
            BindClients();

            SqlCommand cmdRole = new SqlCommand("SELECT role FROM IT_EmployeeRegister WHERE Employeekey = @Employeekey AND role = 11");
            cmdRole.Parameters.AddWithValue("@Employeekey", SC.Userid);
            DataTable dtRole = DA.GetDataTable(cmdRole);
            bool isRole11 = dtRole.Rows.Count > 0;

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int AmcKey;
                if (int.TryParse(Request.QueryString["id"], out AmcKey))
                {
                    hfProjectKey.Value = AmcKey.ToString();
                    PopulateProjectData(AmcKey);
                    btn_request.Visible = false;
                    btn_update.Visible = isRole11;
                }
            }
            else
            {
                btn_request.Visible = true;
                btn_update.Visible = false;
            }
        }
    }
    private void BindClients()
    {
        string str_clients = "SELECT ClientKey, CompanyName FROM IT_ClientDetails WHERE Status='1' and PartyType != 1 ;";
        SqlCommand cmd = new SqlCommand(str_clients);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DD_Client.DataSource = ds.Tables[0];
            DD_Client.DataTextField = "CompanyName";
            DD_Client.DataValueField = "ClientKey";
            DD_Client.DataBind();
        }

        DD_Client.Items.Insert(0, new ListItem("-- Select Client --", ""));



    }

    private void BindProjectByClient(string clientKey)
    {
        string query = @"
        SELECT ProjectKey, ProjectName 
        FROM it_projects 
        WHERE ClientKey = @ClientKey";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@ClientKey", clientKey);

        DataSet ds = DA.GetDataSet(cmd);

        DD_Project.DataSource = ds;
        DD_Project.DataTextField = "ProjectName";
        DD_Project.DataValueField = "ProjectKey";
        DD_Project.DataBind();

        DD_Project.Items.Insert(0, new ListItem("-- Select Project --", ""));
    }

    private void PopulateProjectData(int AmcKey)
    {
        string query = "SELECT AmcKey, ClientKey, ProjectKey, CONVERT(varchar(10), GoLiveDate, 103) AS GoLiveDate, CONVERT(varchar(10), AMCStartDate, 103) AS AMCStartDate, CONVERT(varchar(10), AMCEndDate, 103) AS AMCEndDate, Status, ProjectCost, AMCAmount, INRAmount, Description FROM IT_AMC WHERE AmcKey = @AmcKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@AmcKey", AmcKey);
        DataSet ds = this.DA.GetDataSet(cmd);
              
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            DD_Client.SelectedValue = row["ClientKey"].ToString();
            DD_Project.SelectedValue = row["ProjectKey"].ToString();
            txt_livedate.Text = row["GoLiveDate"].ToString();
            txt_StartdDate.Text = row["AMCStartDate"].ToString();
            txt_EndDate.Text = row["AMCEndDate"].ToString();
            DD_Status.SelectedValue = row["Status"].ToString();
            txt_PP_Cost.Text = row["ProjectCost"].ToString();
            txt_AmcAmount.Text = row["AMCAmount"].ToString();
            txt_INRAmount.Text = row["INRAmount"].ToString();
            txt_Description.Text = row["Description"].ToString();
            BindProjectByClient(DD_Client.SelectedValue);
        }

        // Load AMC Rows
        string rowQuery = "SELECT AMCAmount, Description, Status, CONVERT(varchar(10), AMCDate, 103) AS AMCDate, CONVERT(varchar(10), NextAMCDate, 103) AS NextAMCDate FROM IT_AMCSubTable WHERE AMCKey = @AMCKey";
        SqlCommand cmdRows = new SqlCommand(rowQuery);
        cmdRows.Parameters.AddWithValue("@AMCKey", AmcKey);
        DataSet dsRows = this.DA.GetDataSet(cmdRows);

        if (dsRows != null && dsRows.Tables.Count > 0 && dsRows.Tables[0].Rows.Count > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataRow r in dsRows.Tables[0].Rows)
            {
                string statusVal = r["Status"].ToString() == "1" ? "Not Received" : "Received";
                string statusOptions = string.Format(
                    "<option value=''>Select</option>" +
                    "<option value='Not Received'{0}>Not Received</option>" +
                    "<option value='Received'{1}>Received</option>",
                    statusVal == "Not Received" ? " selected" : "",
                    statusVal == "Received" ? " selected" : "");

                sb.Append("<tr>");
                sb.AppendFormat("<td><input type='text' class='form-control' name='rowAmcAmount[]' value='{0}' /></td>", r["AMCAmount"]);
                sb.AppendFormat("<td><textarea class='form-control' name='rowDescription[]' rows='2'>{0}</textarea></td>", r["Description"]);
                sb.AppendFormat("<td><select class='form-control' name='rowStatus[]'>{0}</select></td>", statusOptions);
                sb.AppendFormat("<td><div class='input-group'><span class='input-group-addon'><i class='icon-calendar22'></i></span><input type='text' class='form-control pickadate-doc' name='rowAmcDate[]' value='{0}' /></div></td>", r["AMCDate"]);
                sb.AppendFormat("<td><div class='input-group'><span class='input-group-addon'><i class='icon-calendar22'></i></span><input type='text' class='form-control pickadate-doc' name='rowNextDate[]' value='{0}' /></div></td>", r["NextAMCDate"]);
                sb.Append("<td style='text-align:center;'><button type='button' class='btn btn-xs btn-danger removeDocRow'><i class='icon-trash'></i></button></td>");
                sb.Append("</tr>");
            }
            tBodyDocs.InnerHtml = sb.ToString();
        }

    }


    protected void btn_request_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        Guid userId = new Guid(SC.Userid.ToString());
        decimal amcAmount = 0;
        string insertProject = @"
    INSERT INTO IT_AMC
    (ClientKey, ProjectKey, GoLiveDate, AMCStartDate, AMCEndDate, Status, ProjectCost, INRAmount, AMCAmount, Description, CreatedOn, CreatedBy)
    VALUES
    (@ClientKey, @ProjectKey, @GoLiveDate, @AMCStartDate, @AMCEndDate, @Status, @ProjectCost, @INRAmount, @AMCAmount, @Description, GETDATE(), @CreatedBy)";

        SqlCommand cmdProject = new SqlCommand(insertProject);
        cmdProject.Parameters.AddWithValue("@ClientKey", DD_Client.SelectedValue);
        cmdProject.Parameters.AddWithValue("@ProjectKey", DD_Project.SelectedValue);
        cmdProject.Parameters.AddWithValue("@GoLiveDate", string.IsNullOrEmpty(txt_livedate.Text) ? (object)DBNull.Value : DateTime.ParseExact(txt_livedate.Text.Trim(), new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None));
        cmdProject.Parameters.AddWithValue("@AMCStartDate", string.IsNullOrEmpty(txt_StartdDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(txt_StartdDate.Text.Trim(), new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None));
        cmdProject.Parameters.AddWithValue("@AMCEndDate", string.IsNullOrEmpty(txt_EndDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(txt_EndDate.Text.Trim(), new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None));
        cmdProject.Parameters.AddWithValue("@Status", DD_Status.SelectedValue);
        cmdProject.Parameters.AddWithValue("@ProjectCost", string.IsNullOrEmpty(txt_PP_Cost.Text) ? 0 : decimal.Parse(txt_PP_Cost.Text));
        cmdProject.Parameters.AddWithValue("@INRAmount", string.IsNullOrEmpty(txt_INRAmount.Text) ? 0 : decimal.Parse(txt_INRAmount.Text));
        decimal.TryParse(txt_AmcAmount.Text.Trim(), out amcAmount);
        cmdProject.Parameters.AddWithValue("@AMCAmount", amcAmount);
        cmdProject.Parameters.AddWithValue("@Description", txt_Description.Text.Trim());
        cmdProject.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

        DA.ExecuteNonQuery(cmdProject);

        // Get last inserted AmcKey
        int AmcKey2 = Convert.ToInt32(DA.GetDataTable(new SqlCommand("SELECT MAX(AmcKey) FROM IT_AMC")).Rows[0][0]);

        // Insert AMC Rows
        string[] rowAmcAmounts   = Request.Form.GetValues("rowAmcAmount[]");
        string[] rowDescriptions = Request.Form.GetValues("rowDescription[]");
        string[] rowStatuses     = Request.Form.GetValues("rowStatus[]");
        string[] rowAmcDates     = Request.Form.GetValues("rowAmcDate[]");
        string[] rowNextDates    = Request.Form.GetValues("rowNextDate[]");

        if (rowAmcAmounts != null)
        {
            for (int i = 0; i < rowAmcAmounts.Length; i++)
            {
                SqlCommand cmdRow = new SqlCommand(@"INSERT INTO IT_AMCSubTable (AMCKey, AMCAmount, Description, Status, AMCDate, NextAMCDate, CreatedOn, CreatedBy)
                    VALUES (@AMCKey, @AMCAmount, @Description, @Status, @AMCDate, @NextAMCDate, GETDATE(), @CreatedBy)");
                cmdRow.Parameters.AddWithValue("@AMCKey", AmcKey2);

                cmdRow.Parameters.AddWithValue(
                    "@AMCAmount",
                    decimal.TryParse(rowAmcAmounts[i], out amcAmount)
                        ? (object)amcAmount
                        : DBNull.Value
                );
                cmdRow.Parameters.AddWithValue("@Description", rowDescriptions != null && rowDescriptions.Length > i ? rowDescriptions[i] : "");
                cmdRow.Parameters.AddWithValue("@Status", rowStatuses != null && rowStatuses.Length > i && rowStatuses[i] == "Live" ? 1 : 0);
                cmdRow.Parameters.AddWithValue("@AMCDate", rowAmcDates != null && rowAmcDates.Length > i && !string.IsNullOrEmpty(rowAmcDates[i]) ? (object)DateTime.ParseExact(rowAmcDates[i], new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None) : DBNull.Value);
                cmdRow.Parameters.AddWithValue("@NextAMCDate", rowNextDates != null && rowNextDates.Length > i && !string.IsNullOrEmpty(rowNextDates[i]) ? (object)DateTime.ParseExact(rowNextDates[i], new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None) : DBNull.Value);
                cmdRow.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;
                DA.ExecuteNonQuery(cmdRow);
            }
        }

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_redirect",
            "showToastr('success','AMC Created Successfully!');" +
            "setTimeout(function(){ window.location.href = '/Admin/amc.aspx'; }, 2000);",
            true
        );
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DD_Client.SelectedValue) || string.IsNullOrEmpty(hfProjectKey.Value))
            return;
        Guid userId = new Guid(SC.Userid.ToString());
        int AmcKey = int.Parse(hfProjectKey.Value);
        // Update Project
        string updateProject = @"
        UPDATE IT_AMC
        SET ClientKey=@ClientKey, ProjectKey=@ProjectKey, GoLiveDate=@GoLiveDate,
            AMCStartDate=@AMCStartDate, AMCEndDate=@AMCEndDate, Status=@Status,
            ProjectCost=@ProjectCost,INRAmount=@INRAmount,AMCAmount=@AMCAmount, Description=@Description, ModifiedOn=GETDATE(), ModifiedBy=@ModifiedBy
        WHERE AMCKey=@AmcKey";
        SqlCommand cmdUpdate = new SqlCommand(updateProject);
        cmdUpdate.Parameters.AddWithValue("@ClientKey", Guid.Parse(DD_Client.SelectedValue));
        cmdUpdate.Parameters.AddWithValue("@ProjectKey", DD_Project.SelectedValue);
        cmdUpdate.Parameters.AddWithValue("@GoLiveDate", string.IsNullOrEmpty(txt_livedate.Text) ? (object)DBNull.Value : DateTime.ParseExact(txt_livedate.Text.Trim(), new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None));
        cmdUpdate.Parameters.AddWithValue("@AMCStartDate", string.IsNullOrEmpty(txt_StartdDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(txt_StartdDate.Text.Trim(), new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None));
        cmdUpdate.Parameters.AddWithValue("@AMCEndDate", string.IsNullOrEmpty(txt_EndDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(txt_EndDate.Text.Trim(), new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None));
        cmdUpdate.Parameters.AddWithValue("@Status", DD_Status.SelectedValue);
        cmdUpdate.Parameters.AddWithValue("@ProjectCost", string.IsNullOrEmpty(txt_PP_Cost.Text) ? 0 : decimal.Parse(txt_PP_Cost.Text));
        cmdUpdate.Parameters.AddWithValue("@INRAmount", string.IsNullOrEmpty(txt_INRAmount.Text) ? 0 : decimal.Parse(txt_INRAmount.Text));
        cmdUpdate.Parameters.AddWithValue("@AMCAmount", string.IsNullOrEmpty(txt_AmcAmount.Text) ? 0 : decimal.Parse(txt_AmcAmount.Text));
        cmdUpdate.Parameters.AddWithValue("@Description", txt_Description.Text.Trim());
        cmdUpdate.Parameters.AddWithValue("@ModifiedBy", userId);
        cmdUpdate.Parameters.AddWithValue("@AmcKey", AmcKey);
        this.DA.ExecuteNonQuery(cmdUpdate);
        // Delete old rows and re-insert
        SqlCommand cmdDelRows = new SqlCommand("DELETE FROM IT_AMCSubTable WHERE AMCKey=@AMCKey");
        cmdDelRows.Parameters.AddWithValue("@AMCKey", AmcKey);
        this.DA.ExecuteNonQuery(cmdDelRows);

        string[] rowAmcAmounts   = Request.Form.GetValues("rowAmcAmount[]");
        string[] rowDescriptions = Request.Form.GetValues("rowDescription[]");
        string[] rowStatuses     = Request.Form.GetValues("rowStatus[]");
        string[] rowAmcDates     = Request.Form.GetValues("rowAmcDate[]");
        string[] rowNextDates    = Request.Form.GetValues("rowNextDate[]");

        if (rowAmcAmounts != null)
        {
            for (int i = 0; i < rowAmcAmounts.Length; i++)
            {
                SqlCommand cmdRow = new SqlCommand(@"INSERT INTO IT_AMCSubTable (AMCKey, AMCAmount, Description, Status, AMCDate, NextAMCDate, CreatedOn, CreatedBy)
                    VALUES (@AMCKey, @AMCAmount, @Description, @Status, @AMCDate, @NextAMCDate, GETDATE(), @CreatedBy)");
                cmdRow.Parameters.AddWithValue("@AMCKey", AmcKey);
                cmdRow.Parameters.AddWithValue("@AMCAmount", string.IsNullOrEmpty(rowAmcAmounts[i]) ? (object)DBNull.Value : decimal.Parse(rowAmcAmounts[i]));
                cmdRow.Parameters.AddWithValue("@Description", rowDescriptions != null && rowDescriptions.Length > i ? rowDescriptions[i] : "");
                cmdRow.Parameters.AddWithValue("@Status", rowStatuses != null && rowStatuses.Length > i && rowStatuses[i] == "Live" ? 1 : 0);
                cmdRow.Parameters.AddWithValue("@AMCDate", rowAmcDates != null && rowAmcDates.Length > i && !string.IsNullOrEmpty(rowAmcDates[i]) ? (object)DateTime.ParseExact(rowAmcDates[i], new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None) : DBNull.Value);
                cmdRow.Parameters.AddWithValue("@NextAMCDate", rowNextDates != null && rowNextDates.Length > i && !string.IsNullOrEmpty(rowNextDates[i]) ? (object)DateTime.ParseExact(rowNextDates[i], new string[]{"dd/MM/yyyy","d MMMM, yyyy","dd MMMM, yyyy"}, CultureInfo.InvariantCulture, DateTimeStyles.None) : DBNull.Value);
                cmdRow.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;
                this.DA.ExecuteNonQuery(cmdRow);
            }
        }

        ScriptManager.RegisterStartupScript(
this,
this.GetType(),
"toastr_redirect",
"showToastr('success','AMC Updated Successfully!');" +
"setTimeout(function(){ window.location.href = '/Admin/amc.aspx'; }, 2000);",
true
);


    }

    protected void DD_Client_SelectedIndexChanged(object sender, EventArgs e)
    {
        DD_Project.Items.Clear();
        if (string.IsNullOrEmpty(DD_Client.SelectedValue))
        {
            DD_Project.Items.Insert(0, new ListItem("-- Select Project --", ""));
            return;
        }
        BindProjectByClient(DD_Client.SelectedValue);
    }
}