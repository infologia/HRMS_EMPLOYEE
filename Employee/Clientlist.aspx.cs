using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
public partial class Employee_Clientlist : System.Web.UI.Page
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
                control1.Text = "Clients";
        }

        string str_userid = this.SC.Userid;
        string str_query = "SELECT ClientKey, ClientCode, ClientName, CompanyName, ContactPerson, Status, FORMAT(CreatedOn, 'dd-MM-yyyy') AS CreatedOn FROM IT_ClientDetails";
        SqlCommand cmd = new SqlCommand(str_query);

        DataTable dt_all = DA.GetDataTable(cmd);

        if (dt_all == null || dt_all.Rows.Count == 0) return;

        dt_all.Columns.Add("ActiveText");
        foreach (DataRow dr in dt_all.Rows)
        {
            int s = Convert.ToInt16(dr["Status"].ToString());
            dr["ActiveText"] = (s == 1)
                ? "<span class='label label-sm label-success'>Active</span>"
                : "<span class='label label-sm label-danger'>InActive</span>";
        }

        DataTable dt_active = dt_all.Clone();
        foreach (DataRow dr in dt_all.Select("Status = 1"))
            dt_active.ImportRow(dr);

        DataTable dt_inactive = dt_all.Clone();
        foreach (DataRow dr in dt_all.Select("Status = 0"))
            dt_inactive.ImportRow(dr);

        if (dt_active.Rows.Count > 0)
        {
            DataSet ds_active = new DataSet();
            ds_active.Merge(dt_active);
            this.PH.LoadGridItem(ds_active, PH_Clientlist, "ClientlistActive.txt", "");
        }

        if (dt_inactive.Rows.Count > 0)
        {
            DataSet ds_inactive = new DataSet();
            ds_inactive.Merge(dt_inactive);
            this.PH.LoadGridItem(ds_inactive, PH_InactiveClients, "ClientlistInactive.txt", "");
        }
    }
}