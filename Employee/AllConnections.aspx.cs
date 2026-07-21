using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_AllConnections : System.Web.UI.Page
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
                control1.Text = "All Connections";

            Connections();
        }
    }

    private void Connections()
    {
        string query = @"
    SELECT 
        MC.ConnectionKey,
        MC.Name,
        MC.Company,
        MC.Position,
        LT.LeadType,
        MC.CreatedOn,
        MC.CreatedBy
    FROM MyConnections MC
    LEFT JOIN IT_LeadType LT 
        ON MC.LeadType = LT.LeadTypeKey
    ORDER BY MC.CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);

        DataSet ds = DA.GetDataSet(cmd);

        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return;

        DataTable dt = ds.Tables[0];

        // Add ViewText column
        if (!dt.Columns.Contains("ViewText"))
            dt.Columns.Add("ViewText");

        string str_userid = this.SC.Userid;

        foreach (DataRow dr in dt.Rows)
        {
            // Same as Leads()
            Guid loggedInUserId = Guid.Parse(str_userid);
            Guid createdId = Guid.Parse(dr["CreatedBy"].ToString());

            string connectionKey = dr["ConnectionKey"].ToString();

            if (createdId == loggedInUserId)
            {
                // Owner → Update
                dr["ViewText"] =
                    "<td><a href='MyConnections.aspx?id=" + connectionKey + "&Viewid=1'>" +
                    "<span class='label label-info'>Update</span></a></td>";
            }
            else
            {
                // Not owner → View
                dr["ViewText"] =
                    "<td><a href='MyConnections.aspx?id=" + connectionKey + "&Viewid=0'>" +
                    "<span class='label label-info'>View</span></a></td>";
            }
        }

        PH_connection.Controls.Clear();
        this.PH.LoadGridItem(ds, PH_connection, "Allconnections.txt", "");
    }

}