using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Admin_SalesLeads : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    private object gvLeads;
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
            BindLeadType();
            BindLeadsLeadType();
            BindStatus();
            BindLeadsStatus();
            BindYear();
            BindMonth();
            this.Leads();
        }

        

    }

    private void BindLeadType()
    {
        string str_lead = "SELECT LeadTypeKey, LeadType FROM IT_LeadType";

        SqlCommand cmd = new SqlCommand(str_lead);
        DataSet ds = DA.GetDataSet(cmd);

        ddl_leadtype.DataSource = ds;
        ddl_leadtype.DataTextField = "LeadType";
        ddl_leadtype.DataValueField = "LeadTypeKey";
        ddl_leadtype.DataBind();
        ddl_leadtype.Items.Insert(0, new ListItem("All", "0"));
    }
    private void BindLeadsLeadType()
    {
        string str_query = @"SELECT a.LeadKey,a.Name,a.Company,a.Position,c.LeadType,b.SalesStatus,CONVERT(date, a.CreatedOn) AS CreatedOn, CONVERT(date, a.ModifiedOn) AS ModifiedOn,d.Firstname FROM IT_Leads a LEFT JOIN IT_SalesStatus b ON a.Status=b.SalesStatusKey LEFT JOIN IT_LeadType c ON a.LeadType=c.LeadTypeKey LEFT JOIN IT_EmployeeRegister d ON a.CreatedBy=d.Employeekey WHERE 1 = 1 ";

        SqlCommand cmd = new SqlCommand();

        if (ddl_leadtype.SelectedValue != "0")
        {
            str_query += " AND a.LeadType = @LeadTypeKey";
            cmd.Parameters.AddWithValue("@LeadTypeKey", ddl_leadtype.SelectedValue);
        }

        cmd.CommandText = str_query;

        DataTable dt = DA.GetDataTable(cmd);


    }

    private void BindStatus()
    {
        string str_sts = "SELECT SalesStatusKey, SalesStatus FROM IT_SalesStatus";

        SqlCommand cmd = new SqlCommand(str_sts);
        DataTable dt = DA.GetDataTable(cmd);

        ddl_status.Items.Clear();

        ddl_status.Items.Add(new ListItem("All", "0"));

        ddl_status.DataSource = dt;
        ddl_status.DataTextField = "SalesStatus";
        ddl_status.DataValueField = "SalesStatusKey";
        ddl_status.DataBind();
        ddl_status.Items.Insert(0, new ListItem("All", "0"));
    }

    private void BindYear()
    {
        string str_sts = "select distinct year(CreatedOn) Year from IT_Leads";

        SqlCommand cmd = new SqlCommand(str_sts);
        DataTable dt = DA.GetDataTable(cmd);

        ddl_year.Items.Clear();

        ddl_year.Items.Add(new ListItem("All", "0"));

        ddl_year.DataSource = dt;
        ddl_year.DataTextField = "year";
        ddl_year.DataValueField = "year";
        ddl_year.DataBind();
        ddl_year.Items.Insert(0, new ListItem("All", "0"));
    }


    private void BindMonth()
    {
        string str_sts = "SELECT DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS MonthName,   MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS MonthNumber FROM master.dbo.spt_values s WHERE s.type = 'P' AND s.number BETWEEN 0 AND 11 ORDER BY MonthNumber";

        SqlCommand cmd = new SqlCommand(str_sts);
        DataTable dt = DA.GetDataTable(cmd);

        ddl_Month.Items.Clear();

        ddl_Month.Items.Add(new ListItem("All", "0"));

        ddl_Month.DataSource = dt;
        ddl_Month.DataTextField = "MonthName";
        ddl_Month.DataValueField = "MonthNumber";
        ddl_Month.DataBind();
        ddl_Month.Items.Insert(0, new ListItem("All", "0"));
    }

    private void BindLeadsStatus()
    {
        string str_query = @"SELECT a.LeadKey,a.Name,a.Company,a.Position,c.LeadType,b.SalesStatus,a.CreatedOn,a.ModifiedOn,d.Firstname FROM IT_Leads a LEFT JOIN IT_SalesStatus b ON a.Status=b.SalesStatusKey LEFT JOIN IT_LeadType c ON a.LeadType=c.LeadTypeKey LEFT JOIN IT_EmployeeRegister d ON a.CreatedBy=d.Employeekey WHERE 1=1";

        SqlCommand cmd = new SqlCommand();

        if (ddl_status.SelectedValue != "0")
        {
            str_query += " AND a.Status = @StatusKey";
            cmd.Parameters.Add("@StatusKey", SqlDbType.Int).Value = Convert.ToInt32(ddl_status.SelectedValue);
                          
        }

        cmd.CommandText = str_query;

        DataTable dt = DA.GetDataTable(cmd);


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
            string str_Sql = "DELETE FROM IT_leads WHERE leadkey=@leadkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@leadkey", Convert.ToInt32(str_leadkey));

            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }


    protected void btn_request_Click(object sender, EventArgs e)
    {
        Leads();
    }

    public void Leads()
    {
        string str_query;
        DataTable dt_salespersonsonly = DA.GetDataTable("select Employeekey from IT_EmployeeRegister where Employeekey in('458576E1-7F84-4880-962A-8B8C9B8C4DB7','7454E892-F420-437B-BB91-9AD1A40E01CE','5B9A9F89-48CE-47F8-BC51-F109348B0800','ED84484D-5366-426E-8AC2-16AEB2ED085C','F3F675B5-F1F9-4DCD-904B-3DA0F0DDE43D','84FC7BF9-99FA-4104-A8AE-DEC30AA64F80') and Employeekey='" + SC.Userid + "'");
        if (dt_salespersonsonly.Rows.Count > 0)
        {
            a_createlead.Visible = true;
            string str_userid = this.SC.Userid;
           // str_query = "SELECT a.LeadKey, a.Name, a.Company, a.Position, c.LeadType, b.SalesStatus, a.CreatedOn, a.ModifiedOn, d.Firstname FROM IT_Leads a LEFT JOIN IT_SalesStatus b ON a.Status = b.SalesStatusKey LEFT JOIN IT_LeadType c ON a.LeadType = c.LeadTypeKey LEFT JOIN IT_EmployeeRegister d ON a.CreatedBy = d.Employeekey WHERE EXISTS (SELECT 1 FROM IT_Leads x WHERE x.CreatedBy = a.CreatedBy)";
            str_query = "SELECT a.LeadKey, a.Name, a.Company, a.Position, c.LeadType, b.SalesStatus,a.CreatedBy, FORMAT(a.CreatedOn, 'dd-MM-yyyy') AS CreatedOn,FORMAT(a.ModifiedOn, 'dd-MM-yyyy') AS ModifiedOn, d.Firstname FROM IT_Leads a LEFT JOIN IT_SalesStatus b ON a.Status = b.SalesStatusKey LEFT JOIN IT_LeadType c ON a.LeadType = c.LeadTypeKey LEFT JOIN IT_EmployeeRegister d ON a.CreatedBy = d.Employeekey WHERE 1 = 1";

            SqlCommand cmd = new SqlCommand(str_query);

            if (ddl_leadtype.SelectedValue != "0")
            {
                str_query += " AND c.LeadTypeKey = @LeadTypeKey";
                cmd.Parameters.AddWithValue("@LeadTypeKey", ddl_leadtype.SelectedValue);
            }

            if (ddl_status.SelectedValue != "0")
            {
                str_query += " AND b.SalesStatusKey = @SalesStatusKey";
                cmd.Parameters.AddWithValue("@SalesStatusKey", ddl_status.SelectedValue);
            }

            if (ddl_year.SelectedValue != "0")
            {
                str_query += " AND YEAR(a.CreatedOn) = @Year";
                cmd.Parameters.AddWithValue("@Year", ddl_year.SelectedValue);
            }

            if (ddl_Month.SelectedValue != "0")
            {
                str_query += " AND MONTH(a.CreatedOn) = @Month";
                cmd.Parameters.AddWithValue("@Month", ddl_Month.SelectedValue);
            }

            cmd.Parameters.AddWithValue("@createdby", str_userid);
            cmd.CommandText = str_query;


            //SqlCommand cmd = new SqlCommand(str_query);
            

            DataTable dt_dashboard = DA.GetDataTable(cmd);
            DataSet ds = new DataSet();
            ds.Merge(dt_dashboard);
            if (dt_dashboard.Rows.Count > 0)
            {
                if (ds.Tables[0].Columns.Contains("Responsestatus"))
                    ds.Tables[0].Columns.Add("ActiveText");
                ds.Tables[0].Columns.Add("ViewText");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Guid loggedInUserId = Guid.Parse(str_userid);
                    Guid createdId = Guid.Parse(dr["CreatedBy"].ToString());
                    string LeadKeys = dr["LeadKey"].ToString();
                    if (createdId == loggedInUserId)
                    {


                       

                        dr["ViewText"] =
                            "<td><a href='/Employee/Myleadsdetails.aspx?id=" + LeadKeys + "&Viewid=1'>" +
                            "<span class='label label-info'>Update</span></a></td>";
                    }
                    else
                    {
                       

                        dr["ViewText"] =
                            "<td><a href='/Employee/Myleadsdetails.aspx?id=" + LeadKeys + "&Viewid=0'>" +
                            "<span class='label label-info'>View</span></a></td>";
                    }
                }





                    this.PH.LoadGridItem(ds, PH_leave, "Salesleads.txt", "");
            }
            else
                return;

        }


    }





}