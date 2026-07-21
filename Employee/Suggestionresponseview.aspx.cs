using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_Suggestionresponseview : System.Web.UI.Page
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
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Suggestion";
        }


        string  str_userid = this.SC.Userid;
          string str_query = "SELECT b.Employeeid,b.Firstname+' '+b.lastname as username,a.suggestioncategory,a.Suggestionresponse,a.reason,a.Suggestionkey,a.SuggestionId,a.SuggestionStatus FROM IT_Suggestion a left outer join IT_EmployeeRegister b ON a.createdby = b.Employeekey where a.createdby=@createdby  order by a.createdon ASC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby",SC.Userid);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("suggestioncategory"))
                ds.Tables[0].Columns.Add("ActiveText");
           
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["suggestioncategory"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info'>Management</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-info'>Student Welfare</span>";

            }

            if (ds.Tables[0].Columns.Contains("SuggestionStatus"))
                ds.Tables[0].Columns.Add("ActiveCategory");
            ds.Tables[0].Columns.Add("ViewText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String str_reason = dr["Suggestionresponse"].ToString();
                int activetype = Convert.ToInt16(dr["suggestionstatus"].ToString());
                if (activetype == 1)
                {
                    dr["ActiveCategory"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                    dr["ViewText"] = "";
                }
                else if (activetype == 2)
                {
                    dr["ActiveCategory"] = "<span class='label label-success' title='" + str_reason + "'>Approved</span>";
                    dr["ViewText"] = "hidden";
                }
                else if (activetype == 3)
                {
                    dr["ActiveCategory"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";
                    dr["ViewText"] = "hidden";
                }
            }
            this.PH.LoadGridItem(ds, PH_Suggestion, "Suggestionview.txt", "");

        }


    }
    [WebMethod] //Delete
    public static string DeleteProject(string str_Suggestionkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from IT_Suggestion where Suggestionkey=@Suggestionkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Suggestionkey", str_Suggestionkey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}