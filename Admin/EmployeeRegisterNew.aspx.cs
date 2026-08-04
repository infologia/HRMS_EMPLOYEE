using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_EmployeeRegisterNew : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userky = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userky = SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Register";

            loaddivision();
            loaddepartment();
            loaddesignation();
            loadstate();
            BindEmployeeTypes();
            BindWorkTypes();
            BindBloodGroups();
            BindManagers();

            // Check if EmployeeKey is passed for edit
            if (!string.IsNullOrEmpty(Request.QueryString["EmployeeKey"]))
            {
                LoadEmployeeData(Request.QueryString["EmployeeKey"]);
                btn_register.Text = "Update";
            }
        }
    }

    private void loadstate()
    {
        string str_state = "SELECT Stateid, Statename FROM IT_State ORDER BY Statename ASC";
        SqlCommand cmd = new SqlCommand(str_state);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_state.DataSource = ds.Tables[0];
            ddl_state.DataValueField = "Stateid";
            ddl_state.DataTextField = "Statename";
            ddl_state.DataBind();
            ddl_state.Items.Insert(0, new ListItem("Select State", "0"));
        }
    }

    private void loaddesignation()
    {
        string str_des = "SELECT RoleId, RoleName, ModuleIds, Description FROM IT_Roles ORDER BY CreatedOn DESC";
        SqlCommand cmd = new SqlCommand(str_des);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_dest.DataSource = ds1.Tables[0];
            ddl_dest.DataValueField = "RoleId";
            ddl_dest.DataTextField = "RoleName";
            ddl_dest.DataBind();
            ddl_dest.Items.Insert(0, new ListItem("Select Role", "0"));
        }
    }

    private void loaddepartment()
    {
        string str_dep = "SELECT Departmentid, Departmentname FROM IT_Department ORDER BY Departmentname ASC";
        SqlCommand cmd = new SqlCommand(str_dep);
        DataSet ds2 = this.DA.GetDataSet(cmd);
        if (ds2 != null && ds2.Tables.Count > 0)
        {
            ddl_depart.DataSource = ds2.Tables[0];
            ddl_depart.DataValueField = "Departmentid";
            ddl_depart.DataTextField = "Departmentname";
            ddl_depart.DataBind();
            ddl_depart.Items.Insert(0, new ListItem("Select Designation", "0"));
        }
    }

    private void loaddivision()
    {
        string str_URL = "SELECT Divisionid, Divisionname FROM IT_Division ORDER BY Divisionname ASC";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds3 = this.DA.GetDataSet(cmd);
        if (ds3 != null && ds3.Tables.Count > 0)
        {
            ddl_division.DataSource = ds3.Tables[0];
            ddl_division.DataValueField = "Divisionid";
            ddl_division.DataTextField = "Divisionname";
            ddl_division.DataBind();
            ddl_division.Items.Insert(0, new ListItem("Select Division", "0"));
        }
    }

    private void BindEmployeeTypes()
    {
        string sql = "SELECT TypeID, TypeName FROM IT_EmployeeType WHERE IsActive = 1 ORDER BY TypeName";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_emptype.DataSource = ds.Tables[0];
            ddl_emptype.DataValueField = "TypeID";
            ddl_emptype.DataTextField = "TypeName";
            ddl_emptype.DataBind();
        }
        ddl_emptype.Items.Insert(0, new ListItem("Select Type", "0"));
    }

    private void BindWorkTypes()
    {
        string sql = "SELECT WM_Id, WM_TypeName FROM IT_Workmode ORDER BY WM_TypeName";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_worktype.DataSource = ds.Tables[0];
            ddl_worktype.DataValueField = "WM_Id";
            ddl_worktype.DataTextField = "WM_TypeName";
            ddl_worktype.DataBind();
        }
        ddl_worktype.Items.Insert(0, new ListItem("Select Work Type", "0"));
    }

    private void BindBloodGroups()
    {
        string sql = "SELECT BloodGroupID, BloodGroupName FROM IT_BloodGroup WHERE IsActive = 1 ORDER BY BloodGroupName";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_bloodgroup.DataSource = ds.Tables[0];
            ddl_bloodgroup.DataValueField = "BloodGroupID";
            ddl_bloodgroup.DataTextField = "BloodGroupName";
            ddl_bloodgroup.DataBind();
        }
        ddl_bloodgroup.Items.Insert(0, new ListItem("Select Blood Group", "0"));
    }

    private void BindManagers()
    {
        string sql = "SELECT Employeekey, Firstname + ' ' + Lastname AS EmployeeName FROM IT_EmployeeRegister WHERE EmployeeStatus = 1 ORDER BY Firstname";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_manager.DataSource = ds.Tables[0];
            ddl_manager.DataValueField = "Employeekey";
            ddl_manager.DataTextField = "EmployeeName";
            ddl_manager.DataBind();
        }
        ddl_manager.Items.Insert(0, new ListItem("Select Manager", "0"));
    }

    protected void ddl_dest_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_dest.SelectedValue != "0")
        {
            BindMenusByDesignation(ddl_dest.SelectedValue);
        }
        else
        {
            phMenus.Controls.Clear();
        }
    }

    private void BindMenusByDesignation(string destinationKey)
    {
        // Get modules assigned to the selected role from IT_Roles.ModuleIds
        string roleQuery = @"SELECT ModuleIds FROM IT_Roles WHERE RoleId = @RoleId";
        SqlCommand roleCmd = new SqlCommand(roleQuery);
        roleCmd.Parameters.AddWithValue("@RoleId", destinationKey);
        DataTable dtRole = DA.GetDataTable(roleCmd);
        
        if (dtRole.Rows.Count == 0 || dtRole.Rows[0]["ModuleIds"] == DBNull.Value || string.IsNullOrEmpty(dtRole.Rows[0]["ModuleIds"].ToString()))
        {
            phMenus.Controls.Clear();
            phMenus.Controls.Add(new LiteralControl("<div class='alert alert-warning'>No modules/menus assigned to this role</div>"));
            return;
        }
        
        string moduleIds = dtRole.Rows[0]["ModuleIds"].ToString().Trim();
        
        // Get menus that belong to the assigned modules
        string query = @"SELECT DISTINCT m.MenuKey, m.MenuName, m.MenuIcon, m.MenuListNo, 
                        m.ParentMenuKey, m.MenuType, m.ModuleId, mod.ModuleName
                        FROM IT_Menus m
                        LEFT JOIN IT_Modules mod ON m.ModuleId = mod.ModuleId
                        WHERE m.Status = 1 AND m.ModuleId IN (" + moduleIds + @")
                        ORDER BY mod.ModuleName, m.MenuType, m.MenuListNo";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count == 0)
        {
            phMenus.Controls.Clear();
            phMenus.Controls.Add(new LiteralControl("<div class='alert alert-warning'>No menus available</div>"));
            return;
        }

        phMenus.Controls.Clear();

        // Group by Module using DataView
        DataView dv = new DataView(dt);
        DataTable dtModules = dv.ToTable(true, "ModuleId", "ModuleName");

        int moduleIndex = 0;
        foreach (DataRow moduleRow in dtModules.Rows)
        {
            string moduleId = moduleRow["ModuleId"].ToString();
            string moduleName = moduleRow["ModuleName"].ToString();
            string collapseId = "module_" + moduleId;

            string html = @"<div class='panel panel-flat' style='margin-bottom: 10px;'>
<div class='panel-heading' style='background-color: #2196F3; color: white; padding: 10px 15px;'>
<h6 class='panel-title' style='margin: 0; font-weight: bold; display: flex; align-items: center;'>
<input type='checkbox' class='module-checkbox' value='" + moduleId + @"' onclick='toggleModule(this, event)' style='margin-right: 10px;'>
<span data-toggle='collapse' data-target='#" + collapseId + @"' style='flex: 1; cursor: pointer;'>" + moduleName + @"</span>
<span class='pull-right' style='margin-top: 3px; cursor: pointer;' data-toggle='collapse' data-target='#" + collapseId + @"'>&#9660;</span>
</h6>
</div>
<div id='" + collapseId + @"' class='panel-body collapse' style='padding: 15px;'>
<table class='table table-bordered table-sm permission-table' data-module='" + moduleId + @"'>
<thead class='table-light'>
<tr>
<th style='width:38%'>Menu</th>
<th class='text-center' style='width:12%'>Order</th>
<th class='text-center' style='width:12.5%'>View</th>
<th class='text-center' style='width:12.5%'>Create</th>
<th class='text-center' style='width:12.5%'>Edit</th>
<th class='text-center' style='width:12.5%'>Delete</th>
</tr>
</thead>
<tbody>";

            // Get parent menus for this module
            DataRow[] parentMenus = dt.Select("ModuleId='" + moduleId + "' AND MenuType=0 AND ParentMenuKey IS NULL");

            foreach (DataRow parentRow in parentMenus)
            {
                string parentId = parentRow["MenuKey"].ToString();
                string parentName = parentRow["MenuName"].ToString();
                string parentIcon = parentRow["MenuIcon"].ToString();

                // Get sub-menus for this parent
                DataRow[] subRows = dt.Select("MenuType=1 AND ParentMenuKey=" + parentId);

                if (subRows.Length > 0)
                {
                    // Parent menu with children - show as header row with order box
                    html += @"<tr style='background-color: #f5f5f5;'>
<td colspan='1' style='font-weight: 600; padding: 8px 15px;'>
<input type='checkbox' class='menu-check module-check' value='" + parentId + @"' data-parent='0' onclick='toggleChildren(this)'> " + parentName + @"
</td>
<td class='text-center'><input type='number' name='order_" + parentId + @"' min='1' style='width:55px;' class='form-control input-sm text-center' placeholder='#'></td>
<td colspan='4'></td>
</tr>";

                    // Show submenu rows
                    foreach (DataRow subRow in subRows)
                    {
                        string subMenuKey = subRow["MenuKey"].ToString();
                        string subMenuName = subRow["MenuName"].ToString();

                        html += @"<tr>
<td style='padding-left:40px;'>
<input type='checkbox' class='menu-check child-" + parentId + @"' value='" + subMenuKey + @"' data-parent='" + parentId + @"' onclick='menuClicked(this)'> " + subMenuName + @"
</td>
<td class='text-center'><input type='number' name='order_" + subMenuKey + @"' min='1' style='width:55px;' class='form-control input-sm text-center' placeholder='#'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='view_" + subMenuKey + @"'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='create_" + subMenuKey + @"'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='edit_" + subMenuKey + @"'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='delete_" + subMenuKey + @"'></td>
</tr>";
                    }
                }
                else
                {
                    // Single menu without children
                    html += @"<tr>
<td style='padding-left:15px;'>
<input type='checkbox' class='menu-check single-menu' value='" + parentId + @"' data-parent='0' onclick='menuClicked(this)'> " + parentName + @"
</td>
<td class='text-center'><input type='number' name='order_" + parentId + @"' min='1' style='width:55px;' class='form-control input-sm text-center' placeholder='#'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='view_" + parentId + @"'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='create_" + parentId + @"'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='edit_" + parentId + @"'></td>
<td class='text-center'><input type='checkbox' onclick='permissionClicked(this)' name='delete_" + parentId + @"'></td>
</tr>";
                }
            }

            html += "</tbody></table></div></div>";
            phMenus.Controls.Add(new LiteralControl(html));
            moduleIndex++;
        }
    }

    protected void btn_register_Click(object sender, EventArgs e)
    {
        // Check if role is selected
        if (ddl_dest.SelectedValue == "0" || string.IsNullOrEmpty(ddl_dest.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Please select a Role');", true);
            return;
        }
        
        // Check if department is selected
        if (ddl_depart.SelectedValue == "0" || string.IsNullOrEmpty(ddl_depart.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Please select a Department');", true);
            return;
        }
        
        // Check if division is selected
        if (ddl_division.SelectedValue == "0" || string.IsNullOrEmpty(ddl_division.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Please select a Division');", true);
            return;
        }
        
        // Check if status is selected
        if (ddl_status.SelectedValue == "2" || string.IsNullOrEmpty(ddl_status.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Please select a Status');", true);
            return;
        }
        
        // Check if at least one menu is selected
        if (string.IsNullOrEmpty(hfSelectedMenus.Value) || hfSelectedMenus.Value.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Please select at least one menu permission for the employee');", true);
            return;
        }
        
        // DOB 18 Years Validation
        DateTime dob;
        if (!DateTime.TryParseExact(txt_dob.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Please enter a valid Date of Birth');", true);
            return;
        }

        int age = DateTime.Now.Year - dob.Year;
        if (DateTime.Now < dob.AddYears(age)) age--;

        if (age < 18)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Employee must be at least 18 years old');", true);
            return;
        }

        string employeeKey = Request.QueryString["EmployeeKey"];

        if (!string.IsNullOrEmpty(employeeKey))
        {
            UpdateEmployee(employeeKey);
        }
        else
        {
            InsertEmployee();
        }
    }

    private void InsertEmployeeMenus(string employeeKey)
    {
        string permissionData = hfSelectedMenus.Value;

        if (string.IsNullOrEmpty(permissionData)) return;

        string[] rows = permissionData.Split(',');

        foreach (string row in rows)
        {
            string[] cols = row.Split('|');
            if (cols.Length != 8) continue;

            string moduleId  = cols[0];
            string parentId  = cols[1];
            string menuId    = cols[2];
            int view         = Convert.ToInt32(cols[3]);
            int create       = Convert.ToInt32(cols[4]);
            int edit         = Convert.ToInt32(cols[5]);
            int delete       = Convert.ToInt32(cols[6]);
            int menuOrder    = string.IsNullOrEmpty(cols[7]) ? 0 : Convert.ToInt32(cols[7]);

            string insertMenu = @"INSERT INTO IT_EmployeeMenus 
                (EmployeeKey, MenuId, ParentId, ModuleId, ViewPermission, CreatePermission, 
                EditPermission, DeletePermission, MenuOrder, CreatedBy, CreatedOn)
                VALUES 
                (@EmployeeKey, @MenuId, @ParentId, @ModuleId, @View, @Create, @Edit, @Delete, 
                @MenuOrder, @CreatedBy, GETDATE())";

            SqlCommand cmd = new SqlCommand(insertMenu);
            cmd.Parameters.AddWithValue("@EmployeeKey", employeeKey);
            cmd.Parameters.AddWithValue("@MenuId",      menuId);
            cmd.Parameters.AddWithValue("@ParentId",    string.IsNullOrEmpty(parentId) ? "0" : parentId);
            cmd.Parameters.AddWithValue("@ModuleId",    string.IsNullOrEmpty(moduleId) ? "0" : moduleId);
            cmd.Parameters.AddWithValue("@View",        view);
            cmd.Parameters.AddWithValue("@Create",      create);
            cmd.Parameters.AddWithValue("@Edit",        edit);
            cmd.Parameters.AddWithValue("@Delete",      delete);
            cmd.Parameters.AddWithValue("@MenuOrder",   menuOrder);
            cmd.Parameters.AddWithValue("@CreatedBy",   this.SC.Userid);

            DA.ExecuteNonQuery(cmd);
        }
    }

    private void LoadEmployeeData(string employeeKey)
    {
        string query = @"SELECT * FROM IT_EmployeeRegister WHERE Employeekey=@Employeekey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Employeekey", employeeKey);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            Txt_Employeeid.Text = row["Employeeid"].ToString();
            Txt_Employeeid.ReadOnly = true;
            Txt_Employeeid.CssClass = "form-control";
            txt_username.Text = row["Username"].ToString();
            txt_username.ReadOnly = true;
            txt_username.CssClass = "form-control";
            txt_fname.Text = row["Firstname"].ToString();
            txt_lname.Text = row["Lastname"].ToString();
            txt_email.Text = row["Email"].ToString();
            txt_phone.Text = row["Phonenumber"].ToString();
            txt_pwd.Attributes["value"] = row["Password"].ToString();
            txt_address.Text = row["Address"].ToString();
            txt_city.Text = row["City"].ToString();
            txt_zipcode.Text = row["Zipcode"].ToString();
            txt_dob.Text = row["DOB"].ToString();
            txt_qualification.Text = row["Qualification"].ToString();
            rd_gander.SelectedValue = row["Gender"].ToString();
            ddl_state.SelectedValue = row["State"].ToString();
            ddl_division.SelectedValue = row["Division"].ToString();
            ddl_depart.SelectedValue = row["Department"].ToString();
            ddl_dest.SelectedValue = row["Role"].ToString();
            ddl_status.SelectedValue = row["EmployeeStatus"].ToString();

            // New fields
            if (row["DateOfJoining"] != DBNull.Value)
                txt_doj.Text = Convert.ToDateTime(row["DateOfJoining"]).ToString("yyyy-MM-dd");
            if (row["EmployeeType"] != DBNull.Value && ddl_emptype.Items.FindByValue(row["EmployeeType"].ToString()) != null)
                ddl_emptype.SelectedValue = row["EmployeeType"].ToString();
            if (row["ReportingManager"] != DBNull.Value && ddl_manager.Items.FindByValue(row["ReportingManager"].ToString()) != null)
                ddl_manager.SelectedValue = row["ReportingManager"].ToString();
            if (row["WorkType"] != DBNull.Value && ddl_worktype.Items.FindByValue(row["WorkType"].ToString()) != null)
                ddl_worktype.SelectedValue = row["WorkType"].ToString();

            txt_aadhaar.Text = row["AadhaarNumber"].ToString();
            txt_pan.Text = row["PANNumber"].ToString();
            txt_uan.Text = row["UANNumber"].ToString();
            if (row["BloodGroup"] != DBNull.Value && ddl_bloodgroup.Items.FindByValue(row["BloodGroup"].ToString()) != null)
                ddl_bloodgroup.SelectedValue = row["BloodGroup"].ToString();

            txt_bankname.Text = row["BankName"].ToString();
            txt_accountno.Text = row["AccountNumber"].ToString();
            txt_ifsc.Text = row["IFSCCode"].ToString();

            txt_emergency_name.Text = row["EmergencyContactName"].ToString();
            txt_emergency_relation.Text = row["EmergencyContactRelation"].ToString();
            txt_emergency_phone.Text = row["EmergencyContactNumber"].ToString();
            // Disable password validators for update mode
            RequiredFieldValidator7.Enabled = false;
            valPassword.Enabled = false;

            BindMenusByDesignation(row["Role"].ToString());
            LoadEmployeeMenuPermissions(employeeKey);
            LoadEmployeeDocuments(employeeKey);
        }
    }

    private void LoadEmployeeMenuPermissions(string employeeKey)
    {
        string query = @"SELECT MenuId, ViewPermission, CreatePermission, EditPermission, DeletePermission,
                        ISNULL(MenuOrder, 0) AS MenuOrder
                        FROM IT_EmployeeMenus WHERE EmployeeKey=@EmployeeKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", employeeKey);
        DataTable dt = DA.GetDataTable(cmd);

        StringBuilder script = new StringBuilder();
        script.Append("<script>setTimeout(function(){");

        foreach (DataRow row in dt.Rows)
        {
            string menuId    = row["MenuId"].ToString();
            bool view        = Convert.ToBoolean(row["ViewPermission"]);
            bool create      = Convert.ToBoolean(row["CreatePermission"]);
            bool edit        = Convert.ToBoolean(row["EditPermission"]);
            bool delete      = Convert.ToBoolean(row["DeletePermission"]);
            string menuOrder = row["MenuOrder"].ToString();

            script.Append("var menuCheck = document.querySelector('.menu-check[value=\"" + menuId + "\"]');");
            script.Append("if(menuCheck) menuCheck.checked=true;");

            if (view)   script.Append("var viewCheck = document.querySelector('input[name=\"view_"     + menuId + "\"]'); if(viewCheck) viewCheck.checked=true;");
            if (create) script.Append("var createCheck = document.querySelector('input[name=\"create_" + menuId + "\"]'); if(createCheck) createCheck.checked=true;");
            if (edit)   script.Append("var editCheck = document.querySelector('input[name=\"edit_"     + menuId + "\"]'); if(editCheck) editCheck.checked=true;");
            if (delete) script.Append("var delCheck = document.querySelector('input[name=\"delete_"    + menuId + "\"]'); if(delCheck) delCheck.checked=true;");

            if (menuOrder != "0")
                script.Append("var orderInput = document.querySelector('input[name=\"order_" + menuId + "\"]'); if(orderInput) orderInput.value='" + menuOrder + "';");
        }

        script.Append("}, 500);</script>");
        ScriptManager.RegisterStartupScript(this, GetType(), "loadPermissions", script.ToString(), false);
    }

    private void LoadEmployeeDocuments(string employeeKey)
    {
        string query = "SELECT DocId, DocumentName, DocumentPath FROM IT_EmployeeDocuments WHERE EmployeeKey=@EmployeeKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", employeeKey);
        DataTable dt = DA.GetDataTable(cmd);

        StringBuilder sb = new StringBuilder();
        foreach (DataRow dr in dt.Rows)
        {
            string docId = dr["DocId"].ToString();
            string docName = dr["DocumentName"].ToString();
            string docPath = dr["DocumentPath"].ToString();

            sb.Append("<tr id='row_doc_" + docId + "'>");
            sb.Append("<td style='padding: 5px;'><input type='text' value='" + docName + "' class='form-control input-sm' readonly style='height: 32px; padding: 5px 10px;' /></td>");
            sb.Append("<td style='padding: 5px; vertical-align: middle;'><span class='text-success' style='font-size: 11px;'><i class='icon-checkmark3'></i> Uploaded</span></td>");
            sb.Append("<td class='text-center' style='padding: 5px;'><a href='../images/EmployeeDocuments/" + docPath + "' target='_blank' class='btn btn-info btn-xs' title='Preview'><i class='icon-eye'></i></a></td>");
            sb.Append("<td class='text-center' style='padding: 5px;'><button type='button' class='btn btn-danger btn-xs' onclick='deleteExistingDoc(" + docId + ")' title='Remove'><i class='icon-trash'></i></button></td>");
            sb.Append("</tr>");
        }

        litExistingDocs.Text = sb.ToString();
    }

    private void InsertEmployee()
    {
        // Validate password format on server side
        if (string.IsNullOrWhiteSpace(txt_pwd.Text))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Password is required');", true);
            return;
        }
        
        if (txt_pwd.Text.Length < 6 || txt_pwd.Text.Length > 30)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Password must be between 6 and 30 characters');", true);
            return;
        }
        
        // Check password contains only allowed characters
        if (!System.Text.RegularExpressions.Regex.IsMatch(txt_pwd.Text, @"^[a-zA-Z0-9@#$%^&+=*]{6,30}$"))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Password can only contain letters, numbers and special characters (@#$%^&+=*)');", true);
            return;
        }
        
        // Check if Employee ID exists
        string str_chkempid = "SELECT Employeeid FROM IT_EmployeeRegister WHERE Employeeid=@Employeeid";
        SqlCommand cmdCheck = new SqlCommand(str_chkempid);
        cmdCheck.Parameters.AddWithValue("@Employeeid", Txt_Employeeid.Text);
        DataTable dt_chkempid = DA.GetDataTable(cmdCheck);

        if (dt_chkempid.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Employee ID already exists');", true);
            return;
        }



        string str_userid = Guid.NewGuid().ToString();
        string str_newid = "";

        if (up_img.HasFile)
        {
            string filename = Path.GetFileName(up_img.FileName);
            string extension = Path.GetExtension(filename);
            str_newid = str_userid + extension;
            string str_path = Server.MapPath("~/images/AdminPRofilePictures/") + str_newid;
            up_img.SaveAs(str_path);
        }
        else
        {
            str_newid = "../MEN.png";
        }

        string str_sql = @"INSERT INTO IT_EmployeeRegister 
            (Employeekey, Employeeid, Username, Firstname, Lastname, Email, Phonenumber, Password, 
            Address, State, City, Zipcode, Image, Gender, DOB, Destination, Qualification, 
            Division, Department, Role, Createdby, EmployeeStatus, roles,
            DateOfJoining, EmployeeType, Designation, ReportingManager, WorkType,
            AadhaarNumber, PANNumber, UANNumber, BloodGroup,
            BankName, AccountNumber, IFSCCode,
            EmergencyContactName, EmergencyContactRelation, EmergencyContactNumber)
            VALUES 
            (@Employeekey, @Employeeid, @Username, @Firstname, @Lastname, @Email, @Phonenumber, @Password, 
            @Address, @State, @City, @Zipcode, @Image, @Gender, @DOB, @Destination, @Qualification, 
            @Division, @Department, @Role, @Createdby, @EmployeeStatus, @roles,
            @DateOfJoining, @EmployeeType, @Designation, @ReportingManager, @WorkType,
            @AadhaarNumber, @PANNumber, @UANNumber, @BloodGroup,
            @BankName, @AccountNumber, @IFSCCode,
            @EmergencyContactName, @EmergencyContactRelation, @EmergencyContactNumber)";

        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        cmd.Parameters.AddWithValue("@Employeeid", Txt_Employeeid.Text);
        cmd.Parameters.AddWithValue("@Username", txt_username.Text);
        cmd.Parameters.AddWithValue("@Firstname", txt_fname.Text);
        cmd.Parameters.AddWithValue("@Lastname", txt_lname.Text);
        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
        cmd.Parameters.AddWithValue("@Phonenumber", txt_phone.Text);
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txt_pwd.Text);
        cmd.Parameters.AddWithValue("@Password", hashedPassword);
        cmd.Parameters.AddWithValue("@Address", txt_address.Text);
        cmd.Parameters.AddWithValue("@State", ddl_state.SelectedValue);
        cmd.Parameters.AddWithValue("@City", txt_city.Text);
        cmd.Parameters.AddWithValue("@Zipcode", txt_zipcode.Text);
        cmd.Parameters.AddWithValue("@Image", str_newid);
        cmd.Parameters.AddWithValue("@Gender", rd_gander.SelectedValue);
        cmd.Parameters.AddWithValue("@DOB", txt_dob.Text);
        cmd.Parameters.AddWithValue("@Destination", ddl_dest.SelectedValue);
        cmd.Parameters.AddWithValue("@Qualification", txt_qualification.Text);
        cmd.Parameters.AddWithValue("@Division", ddl_division.SelectedValue);
        cmd.Parameters.AddWithValue("@Department", ddl_depart.SelectedValue);
        cmd.Parameters.AddWithValue("@Role", Convert.ToInt32(ddl_dest.SelectedValue));
        cmd.Parameters.AddWithValue("@Createdby", this.SC.Userid);
        cmd.Parameters.AddWithValue("@EmployeeStatus", ddl_status.SelectedValue);
        cmd.Parameters.AddWithValue("@roles", "1");
        
        // New Fields
        cmd.Parameters.AddWithValue("@DateOfJoining", string.IsNullOrEmpty(txt_doj.Text) ? (object)DBNull.Value : Convert.ToDateTime(txt_doj.Text));
        cmd.Parameters.AddWithValue("@EmployeeType", ddl_emptype.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddl_emptype.SelectedValue));
        cmd.Parameters.AddWithValue("@Designation", ddl_depart.SelectedValue);
        cmd.Parameters.AddWithValue("@ReportingManager", ddl_manager.SelectedValue == "0" ? (object)DBNull.Value : ddl_manager.SelectedValue);
        cmd.Parameters.AddWithValue("@WorkType", ddl_worktype.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddl_worktype.SelectedValue));

        cmd.Parameters.AddWithValue("@AadhaarNumber", txt_aadhaar.Text);
        cmd.Parameters.AddWithValue("@PANNumber", txt_pan.Text);
        cmd.Parameters.AddWithValue("@UANNumber", txt_uan.Text);
        cmd.Parameters.AddWithValue("@BloodGroup", ddl_bloodgroup.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddl_bloodgroup.SelectedValue));

        cmd.Parameters.AddWithValue("@BankName", txt_bankname.Text);
        cmd.Parameters.AddWithValue("@AccountNumber", txt_accountno.Text);
        cmd.Parameters.AddWithValue("@IFSCCode", txt_ifsc.Text);

        cmd.Parameters.AddWithValue("@EmergencyContactName", txt_emergency_name.Text);
        cmd.Parameters.AddWithValue("@EmergencyContactRelation", txt_emergency_relation.Text);
        cmd.Parameters.AddWithValue("@EmergencyContactNumber", txt_emergency_phone.Text);

        DA.ExecuteNonQuery(cmd);
        InsertEmployeeMenus(str_userid);
        SaveEmployeeDocuments(str_userid);

        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.success('Employee Registered Successfully'); setTimeout(function(){ window.location.href='EmployeeView.aspx'; }, 2000);", true);
    }

    private void UpdateEmployee(string employeeKey)
    {
        string passwordValue = txt_pwd.Text;
        if (string.IsNullOrEmpty(passwordValue) && !string.IsNullOrEmpty(Request.Form[txt_pwd.UniqueID]))
        {
            passwordValue = Request.Form[txt_pwd.UniqueID];
        }

        bool isAlreadyHashed = passwordValue != null && passwordValue.Length == 60 && 
                               (passwordValue.StartsWith("$2a$") || passwordValue.StartsWith("$2b$") || passwordValue.StartsWith("$2y$"));

        if (!isAlreadyHashed)
        {
            // Validate password format on server side
            if (string.IsNullOrWhiteSpace(passwordValue))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Password is required');", true);
                return;
            }
            
            if (passwordValue.Length < 6 || passwordValue.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Password must be between 6 and 30 characters');", true);
                return;
            }
            
            // Check password contains only allowed characters
            if (!System.Text.RegularExpressions.Regex.IsMatch(passwordValue, @"^[a-zA-Z0-9@#$%^&+=*]{6,30}$"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Password can only contain letters, numbers and special characters (@#$%^&+=*)');", true);
                return;
            }
        }
        
        string str_newid = "";

        if (up_img.HasFile)
        {
            string filename = Path.GetFileName(up_img.FileName);
            string extension = Path.GetExtension(filename);
            str_newid = employeeKey + extension;
            string str_path = Server.MapPath("~/images/AdminPRofilePictures/") + str_newid;
            up_img.SaveAs(str_path);
        }
        else
        {
            string getImage = "SELECT Image FROM IT_EmployeeRegister WHERE Employeekey=@Employeekey";
            SqlCommand cmdImg = new SqlCommand(getImage);
            cmdImg.Parameters.AddWithValue("@Employeekey", employeeKey);
            DataTable dtImg = DA.GetDataTable(cmdImg);
            if (dtImg.Rows.Count > 0) str_newid = dtImg.Rows[0]["Image"].ToString();
        }

        string str_sql = @"UPDATE IT_EmployeeRegister SET 
            Firstname=@Firstname, Lastname=@Lastname, Email=@Email, Phonenumber=@Phonenumber, 
            Password=@Password, Address=@Address, State=@State, City=@City, Zipcode=@Zipcode, 
            Image=@Image, Gender=@Gender, DOB=@DOB, Qualification=@Qualification, 
            Division=@Division, Department=@Department, Role=@Role, EmployeeStatus=@EmployeeStatus, 
            Modifiedby=@Modifiedby, Modifiedon=GETDATE(),
            DateOfJoining=@DateOfJoining, EmployeeType=@EmployeeType, Designation=@Designation, ReportingManager=@ReportingManager, WorkType=@WorkType,
            AadhaarNumber=@AadhaarNumber, PANNumber=@PANNumber, UANNumber=@UANNumber, BloodGroup=@BloodGroup,
            BankName=@BankName, AccountNumber=@AccountNumber, IFSCCode=@IFSCCode,
            EmergencyContactName=@EmergencyContactName, EmergencyContactRelation=@EmergencyContactRelation, EmergencyContactNumber=@EmergencyContactNumber
            WHERE Employeekey=@Employeekey";

        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@Employeekey", employeeKey);
        cmd.Parameters.AddWithValue("@Firstname", txt_fname.Text);
        cmd.Parameters.AddWithValue("@Lastname", txt_lname.Text);
        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
        cmd.Parameters.AddWithValue("@Phonenumber", txt_phone.Text);
        if (string.IsNullOrEmpty(passwordValue) && !string.IsNullOrEmpty(Request.Form[txt_pwd.UniqueID]))
        {
            passwordValue = Request.Form[txt_pwd.UniqueID];
        }
        if (!(passwordValue.Length == 60 && (passwordValue.StartsWith("$2a$") || passwordValue.StartsWith("$2b$") || passwordValue.StartsWith("$2y$"))))
        {
            passwordValue = BCrypt.Net.BCrypt.HashPassword(passwordValue);
        }
        cmd.Parameters.AddWithValue("@Password", passwordValue);
        cmd.Parameters.AddWithValue("@Address", txt_address.Text);
        cmd.Parameters.AddWithValue("@State", ddl_state.SelectedValue);
        cmd.Parameters.AddWithValue("@City", txt_city.Text);
        cmd.Parameters.AddWithValue("@Zipcode", txt_zipcode.Text);
        cmd.Parameters.AddWithValue("@Image", str_newid);
        cmd.Parameters.AddWithValue("@Gender", rd_gander.SelectedValue);
        cmd.Parameters.AddWithValue("@DOB", txt_dob.Text);
        cmd.Parameters.AddWithValue("@Qualification", txt_qualification.Text);
        cmd.Parameters.AddWithValue("@Division", ddl_division.SelectedValue);
        cmd.Parameters.AddWithValue("@Department", ddl_depart.SelectedValue);
        cmd.Parameters.AddWithValue("@Role", Convert.ToInt32(ddl_dest.SelectedValue));
        cmd.Parameters.AddWithValue("@EmployeeStatus", ddl_status.SelectedValue);
        cmd.Parameters.AddWithValue("@Modifiedby", this.SC.Userid);

        // New Fields
        cmd.Parameters.AddWithValue("@DateOfJoining", string.IsNullOrEmpty(txt_doj.Text) ? (object)DBNull.Value : Convert.ToDateTime(txt_doj.Text));
        cmd.Parameters.AddWithValue("@EmployeeType", ddl_emptype.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddl_emptype.SelectedValue));
        cmd.Parameters.AddWithValue("@Designation", ddl_depart.SelectedValue);
        cmd.Parameters.AddWithValue("@ReportingManager", ddl_manager.SelectedValue == "0" ? (object)DBNull.Value : ddl_manager.SelectedValue);
        cmd.Parameters.AddWithValue("@WorkType", ddl_worktype.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddl_worktype.SelectedValue));

        cmd.Parameters.AddWithValue("@AadhaarNumber", txt_aadhaar.Text);
        cmd.Parameters.AddWithValue("@PANNumber", txt_pan.Text);
        cmd.Parameters.AddWithValue("@UANNumber", txt_uan.Text);
        cmd.Parameters.AddWithValue("@BloodGroup", ddl_bloodgroup.SelectedValue == "0" ? (object)DBNull.Value : Convert.ToInt32(ddl_bloodgroup.SelectedValue));

        cmd.Parameters.AddWithValue("@BankName", txt_bankname.Text);
        cmd.Parameters.AddWithValue("@AccountNumber", txt_accountno.Text);
        cmd.Parameters.AddWithValue("@IFSCCode", txt_ifsc.Text);

        cmd.Parameters.AddWithValue("@EmergencyContactName", txt_emergency_name.Text);
        cmd.Parameters.AddWithValue("@EmergencyContactRelation", txt_emergency_relation.Text);
        cmd.Parameters.AddWithValue("@EmergencyContactNumber", txt_emergency_phone.Text);

        DA.ExecuteNonQuery(cmd);

        // Delete existing menu permissions and re-insert
        string deleteMenus = "DELETE FROM IT_EmployeeMenus WHERE EmployeeKey=@EmployeeKey";
        SqlCommand cmdDel = new SqlCommand(deleteMenus);
        cmdDel.Parameters.AddWithValue("@EmployeeKey", employeeKey);
        DA.ExecuteNonQuery(cmdDel);

        InsertEmployeeMenus(employeeKey);
        SaveEmployeeDocuments(employeeKey);

        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.success('Employee Updated Successfully'); setTimeout(function(){ window.location.href='EmployeeView.aspx'; }, 2000);", true);
    }

    private void SaveEmployeeDocuments(string employeeKey)
    {
        string uploadFolder = Server.MapPath("~/images/EmployeeDocuments/");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        int maxDocs = 20; // safe limit
        for (int i = 1; i <= maxDocs; i++)
        {
            string docName = Request.Form["DocName_" + i];
            HttpPostedFile docFile = Request.Files["DocFile_" + i];

            if (!string.IsNullOrWhiteSpace(docName) && docFile != null && docFile.ContentLength > 0)
            {
                string filename = Path.GetFileName(docFile.FileName);
                string extension = Path.GetExtension(filename);
                string newFilename = Guid.NewGuid().ToString() + extension;
                string savePath = Path.Combine(uploadFolder, newFilename);
                docFile.SaveAs(savePath);

                string query = @"INSERT INTO IT_EmployeeDocuments (EmployeeKey, DocumentName, DocumentPath, CreatedBy, CreatedOn) 
                                 VALUES (@EmployeeKey, @DocumentName, @DocumentPath, @CreatedBy, GETDATE())";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@EmployeeKey", employeeKey);
                cmd.Parameters.AddWithValue("@DocumentName", docName);
                cmd.Parameters.AddWithValue("@DocumentPath", newFilename);
                cmd.Parameters.AddWithValue("@CreatedBy", this.SC.Userid);
                DA.ExecuteNonQuery(cmd);
            }
        }
    }

    [System.Web.Services.WebMethod]
    public static string DeleteDocument(string id)
    {
        try
        {
            DataAccess da = new DataAccess();
            string getPath = "SELECT DocumentPath FROM IT_EmployeeDocuments WHERE DocId=@DocId";
            SqlCommand cmdGet = new SqlCommand(getPath);
            cmdGet.Parameters.AddWithValue("@DocId", id);
            DataTable dt = da.GetDataTable(cmdGet);

            if (dt.Rows.Count > 0)
            {
                string path = dt.Rows[0]["DocumentPath"].ToString();
                string fullPath = HttpContext.Current.Server.MapPath("~/images/EmployeeDocuments/" + path);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                string del = "DELETE FROM IT_EmployeeDocuments WHERE DocId=@DocId";
                SqlCommand cmdDel = new SqlCommand(del);
                cmdDel.Parameters.AddWithValue("@DocId", id);
                da.ExecuteNonQuery(cmdDel);
                return "Success";
            }
            return "Not Found";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
