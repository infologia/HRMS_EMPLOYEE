using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Admin_DocumentSend : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userkey = SC.Userid;


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Documents";
        }

        if (!IsPostBack)
        {
            this.send();
        }
    }
    
    public void send()
    {

        string str_des = " select Employeeid,(firstname +' ' +Lastname) Employeename,Employeekey from IT_EmployeeRegister where roles='1' and Employeestatus = '1' and Division IN (1,2,3)";
        SqlCommand cmd = new SqlCommand(str_des);
        DataSet ds2 = this.DA.GetDataSet(cmd);
        if (ds2 != null && ds2.Tables.Count > 0)
        {
            ddl_id.DataSource = ds2.Tables[0];
            ddl_id.DataValueField = "Employeekey";
            ddl_id.DataTextField = "Employeeid";
           
            ddl_id.DataBind();
            ddl_id.Items.Add(new ListItem("Select Employee", "0"));
            ddl_id.SelectedValue = "0";

        }
    }
  
    protected void btn_send_Click(object sender, EventArgs e)
    {
        if (up_document.HasFile == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "internal tool", "<script>alert('Please Upload document');</script>");
            return;
        }

        string str_userky = Guid.NewGuid().ToString();
        string filename = Path.GetFileName(up_document.FileName);
        string extension = Path.GetExtension(filename);
        string str_newid = str_userky + extension;
        string str_path = Server.MapPath("~/Document/") + str_newid;
        up_document.SaveAs(str_path);
        string str_send1 = " insert into IT_Document (Employeekey,Username,Document,Documentname,createdby)values(@Employeekey,@Username,@Document,@Documentname,@createdby)";

        SqlCommand cmd2 = new SqlCommand(str_send1);
       
        cmd2.Parameters.AddWithValue("@Employeekey", ddl_id.SelectedValue);
        cmd2.Parameters.AddWithValue("@Username", txt_user.Text);
        cmd2.Parameters.AddWithValue("@Document", str_newid);
        cmd2.Parameters.AddWithValue("@Documentname", txt_letter.Text);
        cmd2.Parameters.AddWithValue("@createdby", str_userkey);


        DA.ExecuteNonQuery(cmd2);
        Response.Redirect("~/Admin/Documents.aspx");

    }

    protected void ddl_id_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str_empdatils = "select Username from IT_EmployeeRegister where Employeekey=@Employeekey";
        SqlCommand cmd = new SqlCommand(str_empdatils);
        cmd.Parameters.AddWithValue("@Employeekey", ddl_id.SelectedValue);
        DataTable dt_empdetails = DA.GetDataTable(cmd);
        if (dt_empdetails.Rows.Count > 0)
        {
            //ddl_user.Attributes.Add("disabled", "disabled");
           // ddl_user.Attributes.Add("disabled", "disabled");
            txt_user.Text = dt_empdetails.Rows[0]["Username"].ToString();
            txt_user.Attributes.Add("readonly", "readonly");
         
        }

    }
}