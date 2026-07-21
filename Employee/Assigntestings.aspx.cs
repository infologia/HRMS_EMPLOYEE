using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Assigntestings : System.Web.UI.Page
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
                control1.Text = "Testing Assignments";
        }

        string str_query = @"SELECT 
                            tt.TaskTestingkey,
                            p.ProjectName,
                            tt.TaskName,
                            (creator.Firstname + ' ' + creator.Lastname) AS AssignedBy,
                            ISNULL(creator.Image, '') AS AssignedByImage,
                            (e.Firstname + ' ' + e.Lastname) AS AssignedTo,
                            ISNULL(e.Image, '') AS AssignedToImage,
                            CONVERT(varchar, tt.StartDate, 103) AS StartDate,
                            tt.AssignedHours,
                            s.StatusName,
                            tt.taskstatus,
                            tt.CreatedBy
                          FROM IT_TaskTesting tt
                          LEFT JOIN IT_Projects p ON tt.projectkey = p.ProjectKey
                          LEFT JOIN IT_EmployeeRegister e ON tt.assignedto = e.EmployeeKey
                          LEFT JOIN IT_EmployeeRegister creator ON tt.CreatedBy = creator.EmployeeKey
                          LEFT JOIN IT_StatusMaster s ON tt.taskstatus = s.StatusID
                          WHERE (tt.assignedto = @EmployeeKey OR tt.CreatedBy = @EmployeeKey)
                          ORDER BY tt.CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@EmployeeKey", SC.Userid);
        DataTable dt_testing = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_testing);

        if (dt_testing.Rows.Count > 0)
        {
            ds.Tables[0].Columns.Add("StatusText");
            ds.Tables[0].Columns.Add("ViewText");
            ds.Tables[0].Columns.Add("RemoveText");
            ds.Tables[0].Columns.Add("RemoveDisabled");
            ds.Tables[0].Columns.Add("AssignedByImg");
            ds.Tables[0].Columns.Add("AssignedToImg");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string statusName = dr["StatusName"].ToString();
                int statusId = Convert.ToInt32(dr["taskstatus"].ToString());
                string createdBy = dr["CreatedBy"].ToString();

                string byImg = dr["AssignedByImage"].ToString();
                dr["AssignedByImg"] = string.IsNullOrEmpty(byImg) ? "../Images/nopicture.jpg" : "../Images/Adminprofilepictures/" + byImg;

                string toImg = dr["AssignedToImage"].ToString();
                dr["AssignedToImg"] = string.IsNullOrEmpty(toImg) ? "../Images/nopicture.jpg" : "../Images/Adminprofilepictures/" + toImg;

                if (statusId == 1)
                {
                    dr["StatusText"] = "<span class='label label-warning'>" + statusName + "</span>";
                }
                else if (statusId == 2)
                {
                    dr["StatusText"] = "<span class='label label-primary'>" + statusName + "</span>";
                }
                else if (statusId == 3)
                {
                    dr["StatusText"] = "<span class='label label-danger'>" + statusName + "</span>";
                }
                else if (statusId == 4)
                {
                    dr["StatusText"] = "<span class='label label-success'>" + statusName + "</span>";
                }
                else
                {
                    dr["StatusText"] = "<span class='label label-default'>" + statusName + "</span>";
                }

                dr["RemoveText"] = (createdBy == SC.Userid && statusId != 4) ? "enabled" : "disabled";
                dr["RemoveDisabled"] = (createdBy == SC.Userid && statusId != 4) ? "" : "disabled";
            }

            this.PH.LoadGridItem(ds, PH_Testing, "Assigntestings.txt", "");
        }
    }

    [WebMethod]
    public static string DeleteTesting(string str_taskTestingkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            string str_Sql = "DELETE FROM IT_TaskTesting WHERE TaskTestingkey = @TaskTestingkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@TaskTestingkey", str_taskTestingkey);
            DA1.ExecuteNonQuery(cmd);
            return str_Response = "1";
        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}
