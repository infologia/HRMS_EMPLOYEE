using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OfficeLeavedaysUpdate : System.Web.UI.Page
{
    SessionCustom SC;
    DataAccess DA;
    string str_id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();


        if (Request.QueryString["id"] == "" || Request.QueryString["id"] == null)
        {

        }
        else
        {
            this.str_id = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                assignvalues();
            }
        }
    }


    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {

            if (this.str_id == "")
            {
                string str_int = "insert into TT_OfficeLeavedays (LeaveReason,Leavedate,LeaveDescription,createdby)values(@LeaveReason,@Leavedate,@LeaveDescription,@createdby)";
                SqlCommand cmd = new SqlCommand(str_int);
                cmd.Parameters.AddWithValue("LeaveReason", txt_LeaveReason.Text);
                cmd.Parameters.AddWithValue("Leavedate", txt_leavedate.Text);
                cmd.Parameters.AddWithValue("LeaveDescription", txt_Leavedesc.InnerText);
                cmd.Parameters.AddWithValue("createdby", this.SC.Userid);
                DA.ExecuteNonQuery(cmd);

            }
            else
            {
                string str_int = "UPDATE TT_OfficeLeavedays SET LeaveReason=@LeaveReason,Leavedate=@Leavedate,LeaveDescription=@LeaveDescription,Modifiedby=@Modifiedby,Modifiedon=getdate()  WHERE LeavedaysKey=@LeavedaysKey;";
                SqlCommand cmd = new SqlCommand(str_int);
                cmd.Parameters.AddWithValue("LeaveReason", txt_LeaveReason.Text);
                cmd.Parameters.AddWithValue("Leavedate", txt_leavedate.Text);
                cmd.Parameters.AddWithValue("LeaveDescription", txt_Leavedesc.InnerText);
                cmd.Parameters.AddWithValue("Modifiedby", this.SC.Userid);
                cmd.Parameters.AddWithValue("LeavedaysKey",this.str_id);
                DA.ExecuteNonQuery(cmd);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Zeesta", "<script>alert('Action failed please contact Team');</script>");
            return;
        }
        Response.Redirect("~/Admin/officeleavedaysgrid.aspx");
    }

    public void assignvalues()
    {
        string str_assing = "select * from TT_OfficeLeavedays where LeavedaysKey=@LeavedaysKey;";
        SqlCommand cmd = new SqlCommand(str_assing);
        cmd.Parameters.AddWithValue("LeavedaysKey", this.str_id);
        DataTable dt_parentmenu = this.DA.GetDataTable(cmd);
        if (dt_parentmenu.Rows.Count > 0)
        {
            txt_LeaveReason.Text = dt_parentmenu.Rows[0]["LeaveReason"].ToString();
            txt_leavedate.Text = dt_parentmenu.Rows[0]["Leavedate"].ToString();
            txt_Leavedesc.InnerText = dt_parentmenu.Rows[0]["LeaveDescription"].ToString();
        }

    }

}