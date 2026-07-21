using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Taskcreation : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    CommonFunction CF;
    string str_status;
    string str_queryid = "";
    string str_userkey = "";
    string str_newid = "";
    string str_key = "";
    string str_newid1 = "";
    string str_upkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.CF = new CommonFunction();
        this.str_userkey = SC.Userid.ToString();
        if (!IsPostBack)
        {
            
            this.issue();
            this.priority();
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Create Task";
        }
      
        if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
        {
            btn_Create.Text = "Create";

            a_btn.HRef = "managetask.aspx";
            if(!IsPostBack)
            {
                this.status();
                this.projectname();
            }
            
        }
        else
        {
            btn_Create.Text = "Update";
            this.str_key = Request.QueryString["id"].ToString();
            div_status.Visible = true;
            div_assign.Visible = true;

            if (!IsPostBack)
            {
                this.status();
                this.projectname();
                this.Assignvalues();
             

            }

            a_btn.HRef = "Leadwork.aspx";
        }
        if (Request.QueryString["key"] == null || Request.QueryString["key"] == "")
        {
            a_btn.HRef = "Leadwork.aspx";
        }
        else
        {
            this.str_upkey = Request.QueryString["key"].ToString();
            if(this.str_upkey=="1")
            {
                btn_Create.Visible = false;
            }
            else
            {
                ddl_pjname.Attributes.Add("Readonly", "Readonly");
                ddl_assign.Attributes.Add("Readonly", "Readonly");
                ddl_isstype.Attributes.Add("Readonly", "Readonly");
                ddl_prty.Attributes.Add("Readonly", "Readonly");
                txt_cldate.Attributes.Add("Readonly", "Readonly");
                txt_des.Attributes.Add("Readonly", "Readonly");
                txt_tasname.Attributes.Add("Readonly", "Readonly");
                txt_time.Attributes.Add("Readonly", "Readonly");
                //ddl_status.Enabled = true;
            }

            a_btn.HRef = "managetask.aspx";
        }


    }
    public void projectassignee()
    {
        string str_des = "select b.Username,a.employeekey from TT_Addmember a  left outer join  IT_EmployeeRegister b on a.employeekey=b.employeekey where projectkey=@projectkey";
        SqlCommand cmd5 = new SqlCommand(str_des);
        cmd5.Parameters.AddWithValue("@projectkey", ddl_pjname.SelectedValue);
        DataSet ds4 = this.DA.GetDataSet(cmd5);
        if (ds4 != null && ds4.Tables.Count > 0)
        {
            ddl_assign.DataSource = ds4.Tables[0];
            ddl_assign.DataTextField = "username";
            ddl_assign.DataValueField = "Employeekey";
            ddl_assign.DataBind();
            ddl_assign.Items.Add(new ListItem("Select Username", "0"));
            ddl_assign.SelectedValue = "0";

        }
    }

    private void Assignvalues()
    {
        string str_query = "select b.Projectkey,c.taskcategorykey,a.taskname,a.attachments,a.worktiming,a.Description,a.Duedate,d.Priortykey,a.Attachments,e.taskstatusid,a.assignee from TT_CreateTask a left outer join TT_project b on a.projectkey=b.projectkey  left outer join TT_TaskCategory c on a.Issuetype=c.Taskcategorykey left outer join TT_Priority d on a.priority =d.PriortyKey left outer join TT_Taskstatus e on e.taskstatusid=a.status where Taskkey=@Taskkey ";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Taskkey", this.str_key);
        DataTable dt_Prj = DA.GetDataTable(cmd);
        if (dt_Prj.Rows.Count > 0)
        {

            ddl_pjname.SelectedValue = dt_Prj.Rows[0]["Projectkey"].ToString();
            ddl_isstype.SelectedValue = dt_Prj.Rows[0]["taskcategorykey"].ToString();
            // ddl_prty.SelectedValue = dt_Prj.Rows[0]["PriorityName"].ToString();
            txt_tasname.Text = dt_Prj.Rows[0]["taskname"].ToString();
            txt_des.InnerText = dt_Prj.Rows[0]["description"].ToString();
            txt_cldate.Text = dt_Prj.Rows[0]["Duedate"].ToString();
            ddl_prty.SelectedValue = dt_Prj.Rows[0]["Priortykey"].ToString();
            ddl_status.SelectedValue = dt_Prj.Rows[0]["taskstatusid"].ToString();
            this.projectassignee();
            ddl_assign.SelectedValue = dt_Prj.Rows[0]["assignee"].ToString();
            txt_time.Text = dt_Prj.Rows[0]["worktiming"].ToString();
            hd_attach.Value = dt_Prj.Rows[0]["attachments"].ToString();
        }
    }


    protected void ddl_pjname_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_assign.Visible = true;
        this.projectassignee();

    }

    private void projectname()
    {
        string str_des = "select a.pjname,a.Projectkey from TT_Project a inner join TT_addmember b on a.projectkey=b.projectkey where b.Employeekey='" + this.str_userkey + "' ";
        SqlCommand cmd2 = new SqlCommand(str_des);
        DataSet ds = this.DA.GetDataSet(cmd2);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_pjname.DataSource = ds.Tables[0];
            ddl_pjname.DataTextField = "pjname";
            ddl_pjname.DataValueField = "projectkey";
            ddl_pjname.DataBind();
            ddl_pjname.Items.Add(new ListItem("Select Project", "0"));
            ddl_pjname.SelectedValue = "0";
        }
    }

    private void status()
    {
        string str_des = "select taskstatusid,statusname from TT_Taskstatus";
        SqlCommand cmd2 = new SqlCommand(str_des);
        DataSet ds = this.DA.GetDataSet(cmd2);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_status.DataSource = ds.Tables[0];
            ddl_status.DataTextField = "statusname";
            ddl_status.DataValueField = "taskstatusid";
            ddl_status.DataBind();
            //ddl_status.Items.Add(new ListItem("Select Status", "0"));
            ddl_status.SelectedValue = "1";
        }
    }

    private void issue()
    {
        string str_des1 = "select categoryname,taskcategorykey from TT_taskcategory ";
        SqlCommand cmd3 = new SqlCommand(str_des1);
        DataSet ds1 = this.DA.GetDataSet(cmd3);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_isstype.DataSource = ds1.Tables[0];
            ddl_isstype.DataTextField = "categoryname";
            ddl_isstype.DataValueField = "taskcategorykey";
            ddl_isstype.DataBind();
            ddl_isstype.Items.Add(new ListItem("Select  IssueType ", "0"));
            ddl_isstype.SelectedValue = "0";
        }
    }

    private void priority()
    {
        string str_des2 = "select priorityname,priortykey from TT_Priority ";
        SqlCommand cmd4 = new SqlCommand(str_des2);
        DataSet ds2 = this.DA.GetDataSet(cmd4);
        if (ds2 != null && ds2.Tables.Count > 0)
        {
            ddl_prty.DataSource = ds2.Tables[0];
            ddl_prty.DataTextField = "priorityname";
            ddl_prty.DataValueField = "priortykey";
            ddl_prty.DataBind();
            ddl_prty.Items.Add(new ListItem("Select  priority ", "0"));
            ddl_prty.SelectedValue = "0";
        }
    }

    protected void btn_create_Click(object sender, EventArgs e)
    {
        string str_newid = "";
        string time = txt_cldate.Text;
        DateTime dt = DateTime.ParseExact(time, "MM/dd/yyyy", null);
        DateTime dt_current = DateTime.Now;
        dt_current = dt_current.Date;
        DateTime dt_fromdate = dt.Date;
    


        DataTable dt_check = DA.GetDataTable("select Username from IT_EmployeeRegister where Username='" + ddl_assign.Text + "'");
        if (dt_check.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Infologia", "<script>alert('This Assignee already created');</script>");
            ddl_assign.Text = "";
            return;
        }

        string str_user = Guid.NewGuid().ToString();
        string filename = Path.GetFileName(up_file.FileName);
        if (filename != "")
        {
            string extension = Path.GetExtension(filename);
            str_newid = str_user + extension;
            string str_path = Server.MapPath("~/Document/") + str_newid;
            up_file.SaveAs(str_path);
        }


        this.str_status = "1";
        string timeFromYourTextBox = txt_time.Text;
        TimeSpan time1 = TimeSpan.Parse(timeFromYourTextBox);
        string fromTimeString = time1.ToString();


        //string fromTimeString = time1.ToString("hh':'mm");


        try
        {
            if (str_key == "")
            {
                if (dt_current > dt_fromdate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid from date');</script>");
                    return;

                }
                string str_task = "insert into TT_CreateTask(projectkey,issuetype,description,priority,attachments,assignee,createdby,taskname,status,duedate,Worktiming)values(@projectkey,@issuetype,@description,@priority,@attachments,@assignee,@createdby,@taskname,@status,@duedate,@Worktiming)";
                SqlCommand cmd1 = new SqlCommand(str_task);
                cmd1.Parameters.AddWithValue("@projectkey", ddl_pjname.SelectedValue);
                cmd1.Parameters.AddWithValue("@issuetype", ddl_isstype.SelectedValue);
                cmd1.Parameters.AddWithValue("@taskname", txt_tasname.Text);
                cmd1.Parameters.AddWithValue("@description", txt_des.InnerText);
                cmd1.Parameters.AddWithValue("@priority", ddl_prty.SelectedValue);
                cmd1.Parameters.AddWithValue("@status", str_status);
                cmd1.Parameters.AddWithValue("@duedate", txt_cldate.Text);
                cmd1.Parameters.AddWithValue("@Worktiming", fromTimeString);
                if (filename != "")
                {
                    cmd1.Parameters.AddWithValue("@attachments", str_newid);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@attachments", DBNull.Value);
                }
                cmd1.Parameters.AddWithValue("@assignee", ddl_assign.SelectedValue);
                cmd1.Parameters.AddWithValue("@createdby", str_userkey);
                DA.ExecuteNonQuery(cmd1);

                string str_email = "select Email,Username from IT_EmployeeRegister  where Employeekey='" + ddl_assign.SelectedValue + "' ";
                SqlCommand sc = new SqlCommand(str_email);
                DataTable dt_email = DA.GetDataTable(sc);
                if (dt_email.Rows.Count > 0)
                {
                    string str_username = dt_email.Rows[0]["Email"].ToString();
                    string str_empid = dt_email.Rows[0]["Username"].ToString();


                    string str_td = txt_tasname.Text;
                    string str_prjname = ddl_pjname.SelectedItem.Text;
                    string str_Projectcatcategory = ddl_isstype.SelectedItem.Text;
                    string str_adminname = SC.username;

                    string email_fun = this.CF.AssignTask(str_username, "registration", "Infologia Technologies", str_empid, str_prjname, str_td, str_Projectcatcategory, str_adminname);

                }
                ClientScript.RegisterStartupScript(this.GetType(), "Infologia", "<script>alert('Task created successfuly...')</script>");

            }
            else
            {

                if (ddl_status.SelectedValue == "2" && this.str_upkey == "2")
                {
                    string str_update1 = "update TT_CreateTask set Taskstarttime=getdate() where Taskkey=@Taskkey";
                    SqlCommand cmd7 = new SqlCommand(str_update1);
                    cmd7.Parameters.AddWithValue("@Taskkey", str_key);
                    DA.ExecuteNonQuery(cmd7);

                }
                else if (ddl_status.SelectedValue == "3" && this.str_upkey == "2")
                {
                    string str_update1 = "update TT_CreateTask set Taskendtime=getdate() where Taskkey=@Taskkey";
                    SqlCommand cmd7 = new SqlCommand(str_update1);
                    cmd7.Parameters.AddWithValue("@Taskkey", str_key);
                    DA.ExecuteNonQuery(cmd7);
                }
                string str_update = "update TT_CreateTask set projectkey=@projectkey,Worktiming=@Worktiming,issuetype=@issuetype,description=@description,priority=@priority,assignee=@assignee,Modifiedby=@Modifiedby,taskname=@taskname,status=@status,duedate=@duedate where Taskkey=@Taskkey";
                SqlCommand cmd6 = new SqlCommand(str_update);
                cmd6.Parameters.AddWithValue("@Taskkey", str_key);
                cmd6.Parameters.AddWithValue("@projectkey", ddl_pjname.SelectedValue);
                cmd6.Parameters.AddWithValue("@issuetype", ddl_isstype.SelectedValue);
                cmd6.Parameters.AddWithValue("@taskname", txt_tasname.Text);
                cmd6.Parameters.AddWithValue("@description", txt_des.InnerText);
                cmd6.Parameters.AddWithValue("@priority", ddl_prty.SelectedValue);
                cmd6.Parameters.AddWithValue("@status", ddl_status.SelectedValue);
                cmd6.Parameters.AddWithValue("@assignee", ddl_assign.SelectedValue);
                cmd6.Parameters.AddWithValue("@Modifiedby", str_userkey);
                cmd6.Parameters.AddWithValue("@duedate", txt_cldate.Text);
                cmd6.Parameters.AddWithValue("@Worktiming", fromTimeString);
                if (filename != "")
                {
                    cmd6.Parameters.AddWithValue("@attachments", str_newid);
                }
                else
                {
                    cmd6.Parameters.AddWithValue("@attachments", hd_attach.Value);
                }
                DA.ExecuteNonQuery(cmd6);

                ClientScript.RegisterStartupScript(this.GetType(), "Infologia", "<script>alert('Task updated successfuly...')</script>");

            }
        }

        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Infologia", "<script>alert('Action failed contact Admin...')</script>");

        }


    }
}