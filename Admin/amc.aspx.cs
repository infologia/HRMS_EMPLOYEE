using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Admin_amc : System.Web.UI.Page
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
                control1.Text = "AMC";
        }

        if (SC.UserRole == "0")
        {
            a_createlead.Visible = true;
        }

        string str_userid = this.SC.Userid;
        string str_query = "SELECT amc.AMCKey as amc, c.companyname as ClientName, p.projectname as ProjectName, CONVERT(varchar(10), amc.GoLiveDate, 120) AS GoLiveDate, amc.status, CONVERT(varchar(10), amc.CreatedOn, 120) AS CreatedOn FROM it_amc amc INNER JOIN it_clientdetails c ON c.clientkey = amc.ClientKey INNER JOIN it_projects p ON p.ProjectKey = amc.ProjectKey;";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby", str_userid);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Status"))
                ds.Tables[0].Columns.Add("ActiveText");

            // Two tables that share the exact same schema as ds.Tables[0]
            DataTable dt_completed = ds.Tables[0].Clone();
            DataTable dt_incompleted = ds.Tables[0].Clone();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Status"].ToString());
                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Closed</span>";
                    dt_completed.ImportRow(dr);
                }
                else if (activetype == 0)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-danger'>Live</span>";
                    dt_incompleted.ImportRow(dr);
                }
            }

            DataSet ds_completed = new DataSet();
            ds_completed.Tables.Add(dt_completed.Copy());

            DataSet ds_incompleted = new DataSet();
            ds_incompleted.Tables.Add(dt_incompleted.Copy());

            if (dt_completed.Rows.Count > 0)
                this.PH.LoadGridItem(ds_completed, PH_Amc_Completed, "Amc.txt", "");

            if (dt_incompleted.Rows.Count > 0)
                this.PH.LoadGridItem(ds_incompleted, PH_Amc_Incompleted, "Amc.txt", "");
        }
        else
            return;

    }
     
    [WebMethod] 
    public static string DeleteProject(string str_leadkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "DELETE FROM IT_AMC WHERE AMCKey=@AMCKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@AMCKey", str_leadkey);
            DA1.ExecuteNonQuery(cmd);
                 
            return str_Response = "1";
        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}
