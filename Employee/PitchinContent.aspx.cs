using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_PitchinContent : System.Web.UI.Page
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
                control1.Text = "Leads";
        }


        DataTable dt_salespersonsonly = DA.GetDataTable("select Employeekey from IT_EmployeeRegister where Employeekey in('458576E1-7F84-4880-962A-8B8C9B8C4DB7','7454E892-F420-437B-BB91-9AD1A40E01CE','5B9A9F89-48CE-47F8-BC51-F109348B0800','ED84484D-5366-426E-8AC2-16AEB2ED085C','F3F675B5-F1F9-4DCD-904B-3DA0F0DDE43D','84FC7BF9-99FA-4104-A8AE-DEC30AA64F80') and Employeekey='" + SC.Userid + "'");
        if (dt_salespersonsonly.Rows.Count > 0)
        {
           // a_createlead.Visible = true;
            string str_userid = this.SC.Userid;
            string str_query = "select LeadType,PitchinContent,CreatedOn,ModifiedOn from IT_PitchinContent";
            SqlCommand cmd = new SqlCommand(str_query);
            cmd.Parameters.AddWithValue("@createdby", str_userid);

            DataTable dt_dashboard = DA.GetDataTable(cmd);
            DataSet ds = new DataSet();
            ds.Merge(dt_dashboard);
            if (dt_dashboard.Rows.Count > 0)
            {
                //if (ds.Tables[0].Columns.Contains("Responsestatus"))
                //    ds.Tables[0].Columns.Add("ActiveText");
                //ds.Tables[0].Columns.Add("ViewText");
                this.PH.LoadGridItem(ds, PH_leave, "Pitchin.txt", "");
            }
            else
                return;

        }

    }

    [WebMethod] //Delete

    public static string DeleteProject(string str_leadkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "DELETE FROM IT_leads WHERE Leadkey=@Leadkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Leadkey", Convert.ToInt32(str_leadkey));

            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }

}