using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_MyConnectionsdetails : System.Web.UI.Page
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
                control1.Text = "My Connections";

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
        ISNULL(E.Firstname + ' ' + E.Lastname, 'Unknown') AS CreatedBy
    FROM MyConnections MC
    LEFT JOIN IT_LeadType LT 
        ON MC.LeadType = LT.LeadTypeKey
    LEFT JOIN IT_EmployeeRegister E
        ON MC.CreatedBy = E.Employeekey
    WHERE MC.CreatedBy = @CreatedBy
    ORDER BY MC.CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@CreatedBy", SC.Userid);

        DataSet ds = DA.GetDataSet(cmd);
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return;

        DataTable dt = ds.Tables[0];

        // Add ViewText column
        if (!dt.Columns.Contains("ViewText"))
            dt.Columns.Add("ViewText");

        foreach (DataRow dr in dt.Rows)
        {
            string connectionKey = dr["ConnectionKey"].ToString();

            dr["ViewText"] =
                "<td><a href='/Employee/MyConnections.aspx?id=" + connectionKey + "&Viewid=1'>" +
                "<span class='label label-info'>Update</span></a></td>";
        }

        PH_connection.Controls.Clear();

        this.PH.LoadGridItem(ds, PH_connection, "Myconnections.txt", "");
    }

    [WebMethod] //Delete

    public static string DeleteProject(string str_ConnectionKey)
    {
        string str_Response = "0";

        try
        {
            int connectionId;

            // Validate INT id
            if (!int.TryParse(str_ConnectionKey, out connectionId))
            {
                return "0";
            }

            DataAccess DA1 = new DataAccess();

            string str_Sql = @"
        DELETE FROM MyConnectionCompetence WHERE MyConnectionID = @ConnectionKey;
        DELETE FROM MyConnections WHERE ConnectionKey = @ConnectionKey;
        ";

            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.Add("@ConnectionKey", SqlDbType.Int).Value = connectionId;

            DA1.ExecuteNonQuery(cmd);

            str_Response = "1";
        }
        catch (Exception ex)
        {
            // You can log ex here if needed
            str_Response = "0";
        }

        return str_Response;
    }
}