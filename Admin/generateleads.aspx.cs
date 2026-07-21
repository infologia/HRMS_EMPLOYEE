using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_generateleads : System.Web.UI.Page
{
    DataAccess DA;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        DA = new DataAccess();
        PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Generated Leads";

            ShowRedirectMessage();
            BindLeadsGrid();
        }
    }

    private void ShowRedirectMessage()
    {
        string msg = Request.QueryString["msg"];
        if (string.IsNullOrEmpty(msg)) return;

        if (msg == "saved")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_msg", "toastr.success('Lead created successfully!');", true);
        else if (msg == "updated")
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_msg", "toastr.success('Lead updated successfully!');", true);
    }

    private void BindLeadsGrid()
    {
        string query = @"SELECT 
                            CompanyKey, 
                            CompanyName, 
                            Industry, 
                            CompanyPhone, 
                            CompanyEmail, 
                            LeadStatus, 
                            Priority 
                         FROM IT_GenerateLeads 
                         WHERE IsActive = 1 
                         ORDER BY CreatedOn DESC";
        try
        {
            SqlCommand cmd = new SqlCommand(query);
            DataTable dt = DA.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Merge(dt);
                PH.LoadGridItem(ds, PH_Leads, "generateleads.txt", "");
            }
        }
        catch (Exception ex)
        {
            // Silently handle SQL errors if table is not created yet
        }
    }

    [System.Web.Services.WebMethod]
    public static string DeleteLead(string str_leadkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            string str_Sql = "UPDATE IT_GenerateLeads SET IsActive = 0, ModifiedOn = GETDATE() WHERE CompanyKey = @CompanyKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@CompanyKey", Convert.ToInt64(str_leadkey));

            DA1.ExecuteNonQuery(cmd);
            return "1";
        }
        catch (Exception ex)
        {
            return "0";
        }
    }
}
