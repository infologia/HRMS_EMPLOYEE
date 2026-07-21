using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;

public partial class Employee_Project : System.Web.UI.Page
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
            BindClients();
            BindProjectTypes();
            BindProjectManagers();
            BindTeamLead();
            BindEmployees();
            string userRoleId = SC.UserRecordTable != null && SC.UserRecordTable.Rows.Count > 0
                ? SC.UserRecordTable.Rows[0]["Role"].ToString() : "";
            pnlBudget.Visible = userRoleId == "11";
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Project";
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int projectKey = int.Parse(Request.QueryString["id"]);
                hfProjectKey.Value = projectKey.ToString();
                PopulateProjectData(projectKey);

                btnSave.Visible = false;

               
            }
            else
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
            }
        }
    }
private DateTime? ParseDate(string dateText)
{
    if (string.IsNullOrWhiteSpace(dateText))
        return null;

    DateTime parsedDate;
    if (DateTime.TryParseExact(
            dateText,
             "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out parsedDate))
    {
        return parsedDate;
    }

    return null;
}

    private void BindClients()
    {
        string str_clients = "SELECT ClientKey, CompanyName FROM IT_ClientDetails WHERE Status='1'";
        SqlCommand cmd = new SqlCommand(str_clients);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlClient.DataSource = ds.Tables[0];
            ddlClient.DataTextField = "CompanyName";
            ddlClient.DataValueField = "ClientKey";
            ddlClient.DataBind();
        }

        ddlClient.Items.Insert(0, new ListItem("-- Select Client --", "0")); 

      

    }

    private void BindProjectTypes()
    {
        string query = "SELECT id, name FROM ProjectType ORDER BY id";
        SqlCommand cmd = new SqlCommand(query);
        DataSet ds = this.DA.GetDataSet(cmd);

        ddlProjectType.Items.Clear();
        ddlProjectType.Items.Insert(0, new ListItem("-- Select Project Type --", "0"));

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlProjectType.DataSource = ds.Tables[0];
            ddlProjectType.DataTextField = "name";
            ddlProjectType.DataValueField = "id";
            ddlProjectType.DataBind();
            ddlProjectType.Items.Insert(0, new ListItem("-- Select Project Type --", "0"));
        }
    }

    private void BindProjectManagers()
    {
        string query = @"SELECT EmployeeKey, (Firstname+' '+Lastname) as name
                     FROM IT_EmployeeRegister
                     WHERE Employeestatus=1 
                     AND (Firstname+' '+Lastname) IN ('Dhanaruban Velusamy', 'Rafeeq Raja M')
                     ORDER BY Firstname";
        SqlCommand cmd = new SqlCommand(query);
        DataSet ds = this.DA.GetDataSet(cmd);

        ddlProjectManager.Items.Clear();

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlProjectManager.DataSource = ds.Tables[0];
            ddlProjectManager.DataTextField = "name";
            ddlProjectManager.DataValueField = "EmployeeKey";
            ddlProjectManager.DataBind();
        }

        ddlProjectManager.Items.Insert(0, new ListItem("-- Select Project Manager --", "0"));
    }
    private void BindTeamLead()
    {
        string str_teamlead = @"SELECT EmployeeKey, (Firstname +' ' + Lastname) as name FROM IT_EmployeeRegister Where Employeestatus = 1 and Division = 1";

        SqlCommand cmd = new SqlCommand(str_teamlead);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            lstTeamLead.DataSource = ds.Tables[0];
            lstTeamLead.DataTextField = "name";
            lstTeamLead.DataValueField = "EmployeeKey";
            lstTeamLead.DataBind();
        }
    }
    
    private void BindEmployees()
    {
        string str_emp = @"SELECT EmployeeKey, (Firstname+' '+Lastname) as name FROM IT_EmployeeRegister WHERE Employeestatus=1 AND Destination IN (11, 12, 23, 24) ORDER BY Firstname";

        SqlCommand cmd = new SqlCommand(str_emp);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            lstEmployees.DataSource = ds.Tables[0];
            lstEmployees.DataTextField = "name";      // display name
            lstEmployees.DataValueField = "EmployeeKey";  // GUID value
            lstEmployees.DataBind();
        }
    }

    private void PopulateProjectData(int projectKey)
    {
        string query = "SELECT ProjectKey, ClientKey, ProjectName, ProjectCode, Description, CONVERT(varchar(10), StartDate, 103) AS StartDate, CONVERT(varchar(10), EndDate, 103) AS EndDate, Status, Budget, EstimatedHours, ProjectTypeId, ProjectManagerKey FROM IT_projects WHERE ProjectKey = @ProjectKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            ddlClient.SelectedValue = row["ClientKey"].ToString();
            txtProjectName.Text = row["ProjectName"].ToString();
            txtProjectCode.Text = row["ProjectCode"].ToString();
            txtDescription.Text = row["Description"].ToString();
            txtStartDate.Text = row["StartDate"].ToString();
            txtEndDate.Text = row["EndDate"].ToString();
            ddlStatus.SelectedValue = row["Status"].ToString();
            string str_ddlStatus = ddlStatus.SelectedValue;
            if (str_ddlStatus == "Planned")
            {
                btnUpdate.Visible = true;
                txtBudget.Text = row["Budget"].ToString();
            }
            else
            {
                
                if (str_ddlStatus == "Completed")
                { btnUpdate.Visible = false;
                }
                else
                {
                    btnUpdate.Visible = true;
                }
                    txtBudget.Text = row["Budget"].ToString();
                // txtBudget.ReadOnly = true;
            }

            txtEstimatedHours.Text = row["EstimatedHours"] != DBNull.Value ? row["EstimatedHours"].ToString() : "";

            if (row["ProjectTypeId"] != DBNull.Value)
                ddlProjectType.SelectedValue = row["ProjectTypeId"].ToString();

            if (row["ProjectManagerKey"] != DBNull.Value)
                ddlProjectManager.SelectedValue = row["ProjectManagerKey"].ToString();
        }

        // Populate Team Leads
        string teamLeadQuery = "SELECT EmployeeKey FROM IT_ProjectTeamLeads WHERE ProjectKey = @ProjectKey";
        SqlCommand cmdTeamLead = new SqlCommand(teamLeadQuery);
        cmdTeamLead.Parameters.AddWithValue("@ProjectKey", projectKey);
        DataSet dsTeamLead = this.DA.GetDataSet(cmdTeamLead);

        lstTeamLead.ClearSelection();
        foreach (DataRow tlRow in dsTeamLead.Tables[0].Rows)
        {
            string empKey = tlRow["EmployeeKey"].ToString();
            ListItem item = lstTeamLead.Items.FindByValue(empKey);
            if (item != null)
                item.Selected = true;
        }

        // Populate Participants
        string partQuery = "SELECT EmployeeKey FROM IT_ProjectsParticipants WHERE ProjectKey = @ProjectKey";
        SqlCommand cmdPart = new SqlCommand(partQuery);
        cmdPart.Parameters.AddWithValue("@ProjectKey", projectKey);
        DataSet dsPart = this.DA.GetDataSet(cmdPart);

        lstEmployees.ClearSelection();

        foreach (DataRow pRow in dsPart.Tables[0].Rows)
        {
            string empKey = pRow["EmployeeKey"].ToString();
            
                ListItem item = lstEmployees.Items.FindByValue(empKey);
                if (item != null)
                    item.Selected = true;
            
        }

        // Populate Document Details
        string docQuery = "SELECT DocumentKey, DocumentName, FilePath, CONVERT(varchar(10), ValidityFrom, 103) AS ValidityFrom, CONVERT(varchar(10), ValidityTo, 103) AS ValidityTo FROM IT_ProjectDocuments WHERE ProjectKey = @ProjectKey";
        SqlCommand cmdDoc = new SqlCommand(docQuery);
        cmdDoc.Parameters.AddWithValue("@ProjectKey", projectKey);
        DataSet dsDoc = this.DA.GetDataSet(cmdDoc);

        if (dsDoc != null && dsDoc.Tables.Count > 0 && dsDoc.Tables[0].Rows.Count > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataRow docRow in dsDoc.Tables[0].Rows)
            {
                string docName = docRow["DocumentName"].ToString();
                string validFrom = docRow["ValidityFrom"].ToString();
                string validTo = docRow["ValidityTo"].ToString();
                string filePath = docRow["FilePath"].ToString();

                string fileDisplay = "";
                if (!string.IsNullOrEmpty(filePath))
                {
                    string ext = Path.GetExtension(filePath).ToLower();
                    string typeStr = "other";
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif") typeStr = "image";
                    else if (ext == ".pdf") typeStr = "pdf";

                    fileDisplay = string.Format("<br/><a href=\"javascript:void(0);\" onclick=\"openPreview('{0}', '{1}')\" class=\"preview-link\" style=\"font-size:12px; color:#3a7bd5;\"><i class=\"icon-eye\"></i> View Attachment</a>", ResolveUrl(filePath), typeStr);
                }

                sb.Append("<tr>");
                sb.Append(string.Format("<td><input type=\"text\" class=\"form-control\" placeholder=\"Enter Document Name\" name=\"docName[]\" value=\"{0}\" /></td>", docName));
                sb.Append(string.Format("<td><input type=\"hidden\" name=\"existingDocFile[]\" value=\"{0}\" /><input type=\"file\" class=\"form-control\" name=\"docFile[]\" accept=\".pdf, .jpg, .jpeg, .png, .gif, .webp\" /><small class=\"text-muted\">Only PDF & Images</small>{1}</td>", filePath, fileDisplay));
                sb.Append("<td><div class=\"input-group\"><span class=\"input-group-addon\"><i class=\"icon-calendar22\"></i></span>");
                sb.Append(string.Format("<input type=\"text\" class=\"form-control pickadate\" placeholder=\"DD/MM/YYYY\" name=\"docValidFrom[]\" value=\"{0}\" /></div></td>", validFrom));
                sb.Append("<td><div class=\"input-group\"><span class=\"input-group-addon\"><i class=\"icon-calendar22\"></i></span>");
                sb.Append(string.Format("<input type=\"text\" class=\"form-control pickadate\" placeholder=\"DD/MM/YYYY\" name=\"docValidTo[]\" value=\"{0}\" /></div></td>", validTo));
                sb.Append("<td style=\"text-align:center;\"><button type=\"button\" class=\"btn-remove-inv removeDocRow\">Remove</button></td>");
                sb.Append("</tr>");
            }
            tBodyDocs.InnerHtml = sb.ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;
        if (string.IsNullOrEmpty(ddlClient.SelectedValue))
            return;

        if (!string.IsNullOrEmpty(txtStartDate.Text) &&
             !string.IsNullOrEmpty(txtEndDate.Text))
        {
            DateTime startDate, endDate;

            bool isStartValid = DateTime.TryParseExact(
                txtStartDate.Text.Trim(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out startDate
            );

            bool isEndValid = DateTime.TryParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate
            );


            if (endDate <= startDate)
            {
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "dateerror",
                    "showToastr('error','End Date must be greater than Start Date');",
                    true);
                return;
            }
        }

        Guid userId = new Guid(SC.Userid.ToString());

        string insertProject = @"
        INSERT INTO IT_projects
        (ClientKey, ProjectName, ProjectCode, Description, StartDate, EndDate, Status, Budget, EstimatedHours, ProjectTypeId, ProjectManagerKey, CreatedOn, CreatedBy)
        VALUES
        (@ClientKey, @ProjectName, @ProjectCode, @Description, @StartDate, @EndDate, @Status, @Budget, @EstimatedHours, @ProjectTypeId, @ProjectManagerKey, GETDATE(), @CreatedBy)";

        SqlCommand cmdProject = new SqlCommand(insertProject);
        cmdProject.Parameters.AddWithValue("@ClientKey", Guid.Parse(ddlClient.SelectedValue));
        cmdProject.Parameters.AddWithValue("@ProjectName", txtProjectName.Text.Trim());
        cmdProject.Parameters.AddWithValue("@ProjectCode", txtProjectCode.Text.Trim());
        cmdProject.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
       cmdProject.Parameters.AddWithValue("@StartDate",
   ParseDate(txtStartDate.Text) ?? (object)DBNull.Value);

cmdProject.Parameters.AddWithValue("@EndDate",
    ParseDate(txtEndDate.Text) ?? (object)DBNull.Value);
        cmdProject.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
        cmdProject.Parameters.AddWithValue("@Budget", string.IsNullOrEmpty(txtBudget.Text) ? 0 : decimal.Parse(txtBudget.Text));
        cmdProject.Parameters.AddWithValue("@EstimatedHours", string.IsNullOrEmpty(txtEstimatedHours.Text) ? (object)DBNull.Value : int.Parse(txtEstimatedHours.Text));
        cmdProject.Parameters.AddWithValue("@ProjectTypeId", ddlProjectType.SelectedValue == "0" ? (object)DBNull.Value : int.Parse(ddlProjectType.SelectedValue));
        cmdProject.Parameters.AddWithValue("@ProjectManagerKey", ddlProjectManager.SelectedValue == "0" ? (object)DBNull.Value : Guid.Parse(ddlProjectManager.SelectedValue));
        cmdProject.Parameters.AddWithValue("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

        this.DA.ExecuteNonQuery(cmdProject);

        // Get last inserted ProjectKey
        string getProjectKey = "SELECT MAX(ProjectKey) FROM IT_projects";
        SqlCommand cmdGetKey = new SqlCommand(getProjectKey);
        DataSet ds = this.DA.GetDataSet(cmdGetKey);
        int projectKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

        // Insert team leads
        foreach (ListItem item in lstTeamLead.Items)
        {
            if (item.Selected)
            {
                string teamLeadQuery = @"
                INSERT INTO IT_ProjectTeamLeads
                (ProjectKey, EmployeeKey, createdby, createdon)
                VALUES (@ProjectKey, @EmployeeKey, @CreatedBy, GETUTCDATE())";
                SqlCommand cmdTeamLead = new SqlCommand(teamLeadQuery);
                cmdTeamLead.Parameters.AddWithValue("@ProjectKey", projectKey);
                cmdTeamLead.Parameters.AddWithValue("@EmployeeKey", Guid.Parse(item.Value));
                cmdTeamLead.Parameters.AddWithValue("@CreatedBy", userId);
                this.DA.ExecuteNonQuery(cmdTeamLead);
            }
        }

        // Insert employees
        foreach (ListItem item in lstEmployees.Items)
        {
            if (item.Selected)
            {
                string empQuery = @"
                INSERT INTO IT_ProjectsParticipants
                (ProjectKey, EmployeeKey, AddedOn)
                VALUES (@ProjectKey, @EmployeeKey, GETDATE())";
                SqlCommand cmdEmp = new SqlCommand(empQuery);
                cmdEmp.Parameters.AddWithValue("@ProjectKey", projectKey);
                cmdEmp.Parameters.AddWithValue("@EmployeeKey", Guid.Parse(item.Value));
                this.DA.ExecuteNonQuery(cmdEmp);
            }
        }

        // Insert Document Details
        string[] docNames = Request.Form.GetValues("docName[]");
        string[] existingDocFiles = Request.Form.GetValues("existingDocFile[]");
        string[] docValidFroms = Request.Form.GetValues("docValidFrom[]");
        string[] docValidTos = Request.Form.GetValues("docValidTo[]");

        if (docNames != null)
        {
            IList<HttpPostedFile> fileList = Request.Files.GetMultiple("docFile[]");
            for (int i = 0; i < docNames.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(docNames[i]))
                {
                    string filePath = "";
                    if (existingDocFiles != null && existingDocFiles.Length > i && !string.IsNullOrEmpty(existingDocFiles[i]))
                    {
                        filePath = existingDocFiles[i];
                    }

                    if (fileList.Count > i && fileList[i].ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fileList[i].FileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        string saveDir = Server.MapPath("~/Uploads/ProjectDocuments/");
                        if (!Directory.Exists(saveDir))
                        {
                            Directory.CreateDirectory(saveDir);
                        }
                        string savePath = Path.Combine(saveDir, uniqueFileName);
                        fileList[i].SaveAs(savePath);
                        filePath = "~/Uploads/ProjectDocuments/" + uniqueFileName;
                    }

                    string insertDoc = @"
                    INSERT INTO IT_ProjectDocuments 
                    (ProjectKey, DocumentName, FilePath, ValidityFrom, ValidityTo, CreatedBy, CreatedOn)
                    VALUES (@ProjectKey, @DocumentName, @FilePath, @ValidityFrom, @ValidityTo, @CreatedBy, GETUTCDATE())";
                    
                    SqlCommand cmdDoc = new SqlCommand(insertDoc);
                    cmdDoc.Parameters.AddWithValue("@ProjectKey", projectKey);
                    cmdDoc.Parameters.AddWithValue("@DocumentName", docNames[i]);
                    cmdDoc.Parameters.AddWithValue("@FilePath", filePath);
                    
                    if (docValidFroms != null && docValidFroms.Length > i)
                        cmdDoc.Parameters.AddWithValue("@ValidityFrom", ParseDate(docValidFroms[i]) ?? (object)DBNull.Value);
                    else
                        cmdDoc.Parameters.AddWithValue("@ValidityFrom", DBNull.Value);

                    if (docValidTos != null && docValidTos.Length > i)
                        cmdDoc.Parameters.AddWithValue("@ValidityTo", ParseDate(docValidTos[i]) ?? (object)DBNull.Value);
                    else
                        cmdDoc.Parameters.AddWithValue("@ValidityTo", DBNull.Value);

                    cmdDoc.Parameters.AddWithValue("@CreatedBy", userId);
                    this.DA.ExecuteNonQuery(cmdDoc);
                }
            }
        }

        ScriptManager.RegisterStartupScript(
 this,
 this.GetType(),
 "toastr_redirect",
 "showToastr('success','Project & Participants Saved Successfully!');" +
 "setTimeout(function(){ window.location.href = '/Employee/Projectgrid.aspx'; }, 2000);",
 true
);

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlClient.SelectedValue) || string.IsNullOrEmpty(hfProjectKey.Value))
            return;
       

        if (!string.IsNullOrEmpty(txtStartDate.Text) &&
           !string.IsNullOrEmpty(txtEndDate.Text))
        {
            DateTime startDate, endDate;

            bool isStartValid = DateTime.TryParseExact(
                txtStartDate.Text.Trim(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out startDate
            );

            bool isEndValid = DateTime.TryParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate
            );


            if (endDate <= startDate)
            {
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "dateerror",
                    "showToastr('error','End Date must be greater than Start Date');",
                    true);
                return;
            }
        }
        Guid userId = new Guid(SC.Userid.ToString());
        int projectKey = int.Parse(hfProjectKey.Value);

        // Update Project
        string updateProject = @"
        UPDATE IT_projects
        SET ClientKey=@ClientKey, ProjectName=@ProjectName, ProjectCode=@ProjectCode,
            Description=@Description, StartDate=@StartDate, EndDate=@EndDate,
            Status=@Status, Budget=@Budget, EstimatedHours=@EstimatedHours,
            ProjectTypeId=@ProjectTypeId, ProjectManagerKey=@ProjectManagerKey,
            ModifiedOn=GETDATE(), ModifiedBy=@ModifiedBy
        WHERE ProjectKey=@ProjectKey";

        SqlCommand cmdUpdate = new SqlCommand(updateProject);
        cmdUpdate.Parameters.AddWithValue("@ClientKey", Guid.Parse(ddlClient.SelectedValue));
        cmdUpdate.Parameters.AddWithValue("@ProjectName", txtProjectName.Text.Trim());
        cmdUpdate.Parameters.AddWithValue("@ProjectCode", txtProjectCode.Text.Trim());
        cmdUpdate.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
       cmdUpdate.Parameters.AddWithValue("@StartDate",
   ParseDate(txtStartDate.Text) ?? (object)DBNull.Value);

cmdUpdate.Parameters.AddWithValue("@EndDate",
    ParseDate(txtEndDate.Text) ?? (object)DBNull.Value);
        cmdUpdate.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
        cmdUpdate.Parameters.AddWithValue("@Budget", string.IsNullOrEmpty(txtBudget.Text) ? 0 : decimal.Parse(txtBudget.Text));
        cmdUpdate.Parameters.AddWithValue("@EstimatedHours", string.IsNullOrEmpty(txtEstimatedHours.Text) ? (object)DBNull.Value : int.Parse(txtEstimatedHours.Text));
        cmdUpdate.Parameters.AddWithValue("@ProjectTypeId", ddlProjectType.SelectedValue == "0" ? (object)DBNull.Value : int.Parse(ddlProjectType.SelectedValue));
        cmdUpdate.Parameters.AddWithValue("@ProjectManagerKey", ddlProjectManager.SelectedValue == "0" ? (object)DBNull.Value : Guid.Parse(ddlProjectManager.SelectedValue));
        cmdUpdate.Parameters.AddWithValue("@ModifiedBy", userId);
        cmdUpdate.Parameters.AddWithValue("@ProjectKey", projectKey);
        this.DA.ExecuteNonQuery(cmdUpdate);

        // Delete existing team leads
        string deleteTeamLeads = "DELETE FROM IT_ProjectTeamLeads WHERE ProjectKey=@ProjectKey";
        SqlCommand cmdDeleteTeamLead = new SqlCommand(deleteTeamLeads);
        cmdDeleteTeamLead.Parameters.AddWithValue("@ProjectKey", projectKey);
        this.DA.ExecuteNonQuery(cmdDeleteTeamLead);

        // Insert new team leads
        foreach (ListItem item in lstTeamLead.Items)
        {
            if (item.Selected)
            {
                string insertTeamLead = "INSERT INTO IT_ProjectTeamLeads (ProjectKey, EmployeeKey, modifiedby, modifiedon) VALUES (@ProjectKey,@EmployeeKey,@ModifiedBy,SYSDATETIMEOFFSET())";
                SqlCommand cmdTeamLead = new SqlCommand(insertTeamLead);
                cmdTeamLead.Parameters.AddWithValue("@ProjectKey", projectKey);
                cmdTeamLead.Parameters.AddWithValue("@EmployeeKey", Guid.Parse(item.Value));
                cmdTeamLead.Parameters.AddWithValue("@ModifiedBy", userId);
                this.DA.ExecuteNonQuery(cmdTeamLead);
            }
        }

        // Update employees
        string deleteEmployees = "DELETE FROM IT_ProjectsParticipants WHERE ProjectKey=@ProjectKey";
        SqlCommand cmdDeleteEmp = new SqlCommand(deleteEmployees);
        cmdDeleteEmp.Parameters.AddWithValue("@ProjectKey", projectKey);
        this.DA.ExecuteNonQuery(cmdDeleteEmp);

        foreach (ListItem item in lstEmployees.Items)
        {
            if (item.Selected)
            {
                string insertEmp = "INSERT INTO IT_ProjectsParticipants (ProjectKey, EmployeeKey, AddedOn) VALUES (@ProjectKey,@EmployeeKey,GETDATE())";
                SqlCommand cmdEmp = new SqlCommand(insertEmp);
                cmdEmp.Parameters.AddWithValue("@ProjectKey", projectKey);
                cmdEmp.Parameters.AddWithValue("@EmployeeKey", Guid.Parse(item.Value));
                this.DA.ExecuteNonQuery(cmdEmp);
            }
        }

        // Handle Document Details
        string deleteDocs = "DELETE FROM IT_ProjectDocuments WHERE ProjectKey=@ProjectKey";
        SqlCommand cmdDeleteDocs = new SqlCommand(deleteDocs);
        cmdDeleteDocs.Parameters.AddWithValue("@ProjectKey", projectKey);
        this.DA.ExecuteNonQuery(cmdDeleteDocs);

        string[] docNames = Request.Form.GetValues("docName[]");
        string[] existingDocFiles = Request.Form.GetValues("existingDocFile[]");
        string[] docValidFroms = Request.Form.GetValues("docValidFrom[]");
        string[] docValidTos = Request.Form.GetValues("docValidTo[]");

        if (docNames != null)
        {
            IList<HttpPostedFile> fileList = Request.Files.GetMultiple("docFile[]");
            for (int i = 0; i < docNames.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(docNames[i]))
                {
                    string filePath = "";
                    if (existingDocFiles != null && existingDocFiles.Length > i && !string.IsNullOrEmpty(existingDocFiles[i]))
                    {
                        filePath = existingDocFiles[i];
                    }

                    if (fileList.Count > i && fileList[i].ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fileList[i].FileName);
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        string saveDir = Server.MapPath("~/Uploads/ProjectDocuments/");
                        if (!Directory.Exists(saveDir))
                        {
                            Directory.CreateDirectory(saveDir);
                        }
                        string savePath = Path.Combine(saveDir, uniqueFileName);
                        fileList[i].SaveAs(savePath);
                        filePath = "~/Uploads/ProjectDocuments/" + uniqueFileName;
                    }

                    string insertDoc = @"
                    INSERT INTO IT_ProjectDocuments 
                    (ProjectKey, DocumentName, FilePath, ValidityFrom, ValidityTo, CreatedBy, CreatedOn)
                    VALUES (@ProjectKey, @DocumentName, @FilePath, @ValidityFrom, @ValidityTo, @CreatedBy, GETUTCDATE())";
                    
                    SqlCommand cmdDoc = new SqlCommand(insertDoc);
                    cmdDoc.Parameters.AddWithValue("@ProjectKey", projectKey);
                    cmdDoc.Parameters.AddWithValue("@DocumentName", docNames[i]);
                    cmdDoc.Parameters.AddWithValue("@FilePath", filePath);
                    
                    if (docValidFroms != null && docValidFroms.Length > i)
                        cmdDoc.Parameters.AddWithValue("@ValidityFrom", ParseDate(docValidFroms[i]) ?? (object)DBNull.Value);
                    else
                        cmdDoc.Parameters.AddWithValue("@ValidityFrom", DBNull.Value);

                    if (docValidTos != null && docValidTos.Length > i)
                        cmdDoc.Parameters.AddWithValue("@ValidityTo", ParseDate(docValidTos[i]) ?? (object)DBNull.Value);
                    else
                        cmdDoc.Parameters.AddWithValue("@ValidityTo", DBNull.Value);

                    cmdDoc.Parameters.AddWithValue("@CreatedBy", userId);
                    this.DA.ExecuteNonQuery(cmdDoc);
                }
            }
        }

        ScriptManager.RegisterStartupScript(
 this,
 this.GetType(),
 "toastr_redirect",
 "showToastr('success','Project & Participants Updated Successfully!');" +
 "setTimeout(function(){ window.location.href = '/Employee/Projectgrid.aspx'; }, 2000);",
 true
);
    }

}