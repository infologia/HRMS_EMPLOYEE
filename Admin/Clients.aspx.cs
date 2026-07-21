using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Admin_Clients : System.Web.UI.Page
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
                control1.Text = "Organization";
        }

        string userRoleId = SC.UserRecordTable != null && SC.UserRecordTable.Rows.Count > 0
            ? SC.UserRecordTable.Rows[0]["Role"].ToString() : "";
        a_createlead.Visible = userRoleId == "11";

        string str_query = "SELECT c.ClientKey, c.ClientCode, c.ClientName, c.CompanyName, c.ContactPerson, c.Status, c.CreatedOn, ISNULL(pt.PT_Name, '') AS PT_Name FROM IT_ClientDetails c LEFT JOIN IT_PartyType pt ON pt.PT_ID = c.PartyType";
        SqlCommand cmd = new SqlCommand(str_query);

        DataTable dt_all = DA.GetDataTable(cmd);

        if (dt_all == null || dt_all.Rows.Count == 0)
            return;

        // Add display column
        dt_all.Columns.Add("ActiveText");
        dt_all.Columns.Add("ActionText");
        
        foreach (DataRow dr in dt_all.Rows)
        {
            int activetype = Convert.ToInt16(dr["Status"].ToString());
            dr["ActiveText"] = (activetype == 1)
                ? "<span class='label label-sm label-success'>Active</span>"
                : "<span class='label label-sm label-danger'>InActive</span>";
                
            if (userRoleId == "11")
            {
                dr["ActionText"] = "<a href=\"Clientsdetails.aspx?id=" + dr["ClientKey"].ToString() + "\" style=\"margin-right: 5px;\" title=\"Update\"><span class=\"label label-info\"><i class=\"icon-pencil\"></i></span></a>" + 
                                   "<a style=\"cursor:pointer;\" title=\"Delete\"><span class=\"label label-danger\" onclick=\"fn_DeleteProject('" + dr["ClientKey"].ToString() + "')\"><i class=\"icon-trash\"></i></span></a>";
            }
            else
            {
                dr["ActionText"] = "<a href=\"Clientsdetails.aspx?id=" + dr["ClientKey"].ToString() + "\" title=\"View\"><span class=\"label label-info\"><i class=\"icon-eye\"></i></span></a>";
            }
        }

        // Filter Active (Status = 1)
        DataTable dt_active = dt_all.Clone();
        foreach (DataRow dr in dt_all.Select("Status = 1"))
            dt_active.ImportRow(dr);

        // Filter Inactive (Status = 0)
        DataTable dt_inactive = dt_all.Clone();
        foreach (DataRow dr in dt_all.Select("Status = 0"))
            dt_inactive.ImportRow(dr);

        // Load Active grid
        if (dt_active.Rows.Count > 0)
        {
            DataSet ds_active = new DataSet();
            ds_active.Merge(dt_active);
            this.PH.LoadGridItem(ds_active, PH_ActiveClients, "ClientsActive.txt", "");
        }

        // Load Inactive grid
        if (dt_inactive.Rows.Count > 0)
        {
            DataSet ds_inactive = new DataSet();
            ds_inactive.Merge(dt_inactive);
            this.PH.LoadGridItem(ds_inactive, PH_InactiveClients, "ClientsInactive.txt", "");
        }
    }

    [WebMethod] // Delete
    public static string DeleteProject(string str_leadkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            string str_Sql = "DELETE FROM IT_ClientDetails WHERE ClientKey = @ClientKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@ClientKey", str_leadkey);
            DA1.ExecuteNonQuery(cmd);
            return str_Response = "1";
        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}
