using iTextSharp.tool.xml.html;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Testinggrid : System.Web.UI.Page
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
            // Breadcrumb
            Label lblBread = this.Master.FindControl("lbl_bread") as Label;
            if (lblBread != null)
                lblBread.Text = "Testing Tasks";
            LoadTestingGrid();
        }
    }

    private void LoadTestingGrid()
    {
        string str_query = @"
    SELECT 
        t.TaskTestingKey,
        t.TaskKey,
        t.ProjectKey,
        p.ProjectName,
        cb.FirstName + ' ' + cb.LastName AS AssignedByName,
        t.StartDate,
        t.EndDate,
        t.TaskStatus,
        s.StatusName,
 t.CreatedBy,
        mb.FirstName + ' ' + mb.LastName AS ModifiedByName
    FROM IT_TaskTesting t
    INNER JOIN IT_Projects p ON t.ProjectKey = p.ProjectKey
    INNER JOIN IT_StatusMaster s ON t.TaskStatus = s.StatusID
    LEFT JOIN IT_EmployeeRegister cb ON t.CreatedBy = cb.EmployeeKey
    LEFT JOIN IT_EmployeeRegister mb ON t.ModifiedBy = mb.EmployeeKey
    WHERE t.AssignedTo = @UserID OR t.CreatedBy = @UserID
    ORDER BY t.CreatedOn ASC;
    ";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@UserID", SC.Userid);

        DataTable dtTesting = DA.GetDataTable(cmd);

        if (dtTesting.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dtTesting);
            ds.Tables[0].Columns.Add("DeleteVisible", typeof(string));
            Guid currentUserId = new Guid(SC.Userid.ToString());

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // Format dates
                if (dr["StartDate"] != DBNull.Value)
                    dr["StartDate"] = Convert.ToDateTime(dr["StartDate"]).ToString("yyyy-MM-dd");

                if (dr["EndDate"] != DBNull.Value)
                    dr["EndDate"] = Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd");

                // Convert int status to styled label
                if (dr["TaskStatus"] != DBNull.Value)
                {
                    int statusID = Convert.ToInt32(dr["TaskStatus"]);
                    string label = "<span class='label label-secondary'>Unknown</span>"; // default

                    if (statusID == 1)
                        label = "<span class='label label-primary'>Assigned</span>";
                    else if (statusID == 2)
                        label = "<span class='label label-success'>Ongoing</span>";
                    else if (statusID == 3)
                        label = "<span class='label label-warning'>Pending</span>";
                    else if (statusID == 4)
                        label = "<span class='label label-success'>Completed</span>";
                    else if (statusID == 5)
                        label = "<span class='label label-warning'>Testing</span>";

                    dr["StatusName"] = label;


                    if (dr["CreatedBy"] != DBNull.Value &&
                        new Guid(dr["CreatedBy"].ToString()) == currentUserId)
                    {
                        dr["DeleteVisible"] = "visible";
                    }
                    else
                    {
                        dr["DeleteVisible"] = "hidden";
                    }

                }
            }

            PH.LoadGridItem(ds, PH_Task, "Testinggrid.txt", "");
        }
    }

    [WebMethod] //Delete

    public static string DeleteProject(string str_TaskTestingKey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "DELETE FROM IT_TaskTesting WHERE TaskTestingKey=@TaskTestingKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@TaskTestingKey", Convert.ToInt32(str_TaskTestingKey));

            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }

}
