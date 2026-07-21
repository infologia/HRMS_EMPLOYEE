using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Web.Services;

public partial class WEB_Admin_Department : System.Web.UI.Page
{
	DataAccess DA;
	SessionCustom SC;
	PhTemplate PH;
	string str_userid = "";


	string str_TemplateContent = "", str_HTMLContent = "", str_ReplaceContent = "";
	public int int_Seq = 0;
	public string str_ConfigName = "";

	protected void Page_Load(object sender, EventArgs e)
	{
		this.DA = new DataAccess();
		this.SC = new SessionCustom();
		this.PH = new PhTemplate();
		str_userid = this.SC.Userid;


		if (!IsPostBack)
		{
			Label control1 = this.Master.FindControl("lbl_bread") as Label;
			if (control1 != null)
				control1.Text = "Categories";
}		if (!IsPostBack)
		{
			grid();
		}
	}

	public void grid()
	{
		SqlCommand sc;
		String str_sql = "SELECT Depid,Departmentname FROM IT_Department  order by createdon ASC";
		sc = new SqlCommand(str_sql);
		DataTable dt_UserSession = this.DA.GetDataTable(sc);
		GridView1.DataSource = dt_UserSession;
		GridView1.DataBind();
	}
	protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{

		string id = GridView1.DataKeys[e.RowIndex].Values["Depid"].ToString();
		SqlCommand cmd = new SqlCommand("delete FROM IT_Department where Depid='" + id + "'");
		DA.ExecuteNonQuery(cmd);
        ScriptManager.RegisterStartupScript(
          this,
          this.GetType(),
          "delete_success",
          "showToastr('success','Department deleted successfully!');",
          true
      );
        grid();
	}
	protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView1.EditIndex = e.NewEditIndex;
		grid();
	}
	protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		string date = DateTime.Now.ToString();
		
		string id1 = GridView1.DataKeys[e.RowIndex].Values["Depid"].ToString();
		GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
		Label lblID = (Label)row.FindControl("lblID");
		TextBox Depname = (TextBox)row.Cells[1].Controls[0];
		GridView1.EditIndex = -1;
		SqlCommand cmd = new SqlCommand("update IT_Department set Departmentname='" + Depname.Text + "',Modifiedby='" + this.str_userid + "',Modifiedon=@Modifiedon where Depid='" + id1 + "'");
        cmd.Parameters.Add("@Modifiedon", SqlDbType.DateTime).Value = DateTime.Now;
        DA.ExecuteNonQuery(cmd);
        ScriptManager.RegisterStartupScript(
             this,
             this.GetType(),
             "update_success",
             "showToastr('success','Department updated successfully!');" +
             "setTimeout(function(){ window.location.href='/Admin/Department.aspx'; }, 2000);",
             true
         );
        grid();
	}
	protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView1.EditIndex = -1;
		grid();
	}

	[WebMethod]
	public static string SaveConfigItems(string str_ControlValue)
	{
		string str_ConfigId = "";
		ArrayList al_Cmd = new ArrayList();
		try
		{
		SessionCustom SC = new SessionCustom();
			string str_id = SC.Userid;
			if (str_ControlValue != "")
			{
				str_ControlValue = str_ControlValue.Substring(0, str_ControlValue.Length - 3);
				string[] str_SplitRow = str_ControlValue.Split(new string[] { "###" }, StringSplitOptions.None);
				for (int i = 0; i < str_SplitRow.Length; i++)
				{
					string[] str_ColumnValues = str_SplitRow[i].Split(new string[] { "###" }, StringSplitOptions.None);
					string str_InsertSql = "insert into IT_department(Departmentname,createdby)Values(@Departmentname,@createdby)";

					SqlCommand cmd = new SqlCommand(str_InsertSql);

					cmd.Parameters.AddWithValue("@Departmentname", str_ColumnValues[0]);
					cmd.Parameters.AddWithValue("@createdby", str_id);
					cmd.Parameters.AddWithValue("@Employeekey", str_id);
					al_Cmd.Add(cmd);


				}
			}
			new DataAccess().ExecuteNonQuery(al_Cmd);
		}
		catch (Exception ex)
		{
			return "false";
		}
		return "true";
	}

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                return;

            LinkButton btnDelete = e.Row.Cells[3].Controls[0] as LinkButton;

            if (btnDelete != null && btnDelete.CommandName == "Delete")
            {
                btnDelete.OnClientClick =
                    "return confirm('Are you sure you want to delete this Department?');";
            }
        }
    }

}
