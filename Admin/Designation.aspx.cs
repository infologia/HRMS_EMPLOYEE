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

public partial class WEB_Employee_Designation : System.Web.UI.Page
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
        }

        if (!IsPostBack)
        {
            grid();
        }
    }

    public void grid()
    {
        SqlCommand sc;
        String str_sql = "SELECT Desgid,Destinationname FROM IT_Destination  order by createdon ASC";

        sc = new SqlCommand(str_sql);
        DataTable dt_UserSession = this.DA.GetDataTable(sc);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (DataRow row in dt_UserSession.Rows)
        {
            string desgId = row["Desgid"].ToString();
            string desgName = row["Destinationname"].ToString();
            
            sb.Append("<tr>");
            sb.Append("<td>" + desgId + "</td>");
            sb.Append("<td>" + desgName + "</td>");
            
            // Actions (Edit & Delete)
            sb.Append("<td class='text-center'>");
            sb.Append("<ul class='icons-list'>");
            sb.Append("<li><a href='javascript:void(0);' class='text-primary' onclick=\"openEditModal('" + desgId + "', '" + desgName.Replace("'", "\\'") + "')\" data-popup='tooltip' title='Edit'><i class='icon-pencil7'></i></a></li>");
            sb.Append("<li><a href='javascript:void(0);' class='text-danger' onclick=\"fn_DeleteDesignation('" + desgId + "')\" data-popup='tooltip' title='Delete'><i class='icon-trash'></i></a></li>");
            sb.Append("</ul>");
            sb.Append("</td>");
            
            sb.Append("</tr>");
        }
        
        PH_Designation.Controls.Add(new LiteralControl(sb.ToString()));
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
                    string str_InsertSql = "insert into IT_Destination(Destinationname,createdby,Employeekey)Values(@Destinationname,@createdby,@Employeekey)";
                    SqlCommand cmd = new SqlCommand(str_InsertSql);
                    cmd.Parameters.AddWithValue("@Destinationname", str_ColumnValues[0]);
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

    [WebMethod]
    public static string UpdateDesignation(string desgId, string desgName)
    {
        try
        {
            SessionCustom SC = new SessionCustom();
            string str_userid = SC.Userid;
            SqlCommand cmd = new SqlCommand("update IT_Destination set Destinationname=@Destinationname, Modifiedby=@Modifiedby, Modifiedon=@Modifiedon where Desgid=@Desgid");
            cmd.Parameters.AddWithValue("@Destinationname", desgName);
            cmd.Parameters.AddWithValue("@Modifiedby", str_userid);
            cmd.Parameters.AddWithValue("@Modifiedon", DateTime.Now);
            cmd.Parameters.AddWithValue("@Desgid", desgId);
            new DataAccess().ExecuteNonQuery(cmd);
            return "true";
        }
        catch
        {
            return "false";
        }
    }

    [WebMethod]
    public static string DeleteDesignation(string desgId)
    {
        try
        {
            SqlCommand cmd = new SqlCommand("delete FROM IT_Destination where Desgid=@Desgid");
            cmd.Parameters.AddWithValue("@Desgid", desgId);
            new DataAccess().ExecuteNonQuery(cmd);
            return "true";
        }
        catch
        {
            return "false";
        }
    }
}


