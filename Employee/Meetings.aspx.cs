using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Employee_Meetings : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        DataTable dt_dashboard = new DataTable();
        DataSet ds = new DataSet();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Meetings";
        }

        if (SC.UserRole == "1")
        {
            a_createlead.Visible = true;
        }
        else
        {
            a_createlead.Visible = false;
        }

        MeetingGrid();
        CompletedGrid();

      

    }


    private void CompletedGrid()
    {
        DataTable dt_dashboard = new DataTable();
        DataSet ds = new DataSet();
        string str_userid = this.SC.Userid;

        if (str_userid == "1987df80-f1a7-4efe-a6bb-af04ad6aa9bd")
        {
            string str_query = @"
 SELECT DISTINCT
    a.MeetingTitle,
  FORMAT(a.MeetingDate, 'yyyy-MM-dd') AS MeetingDate,
FORMAT(a.StartTime, 'hh:mm tt') AS StartTime,
FORMAT(a.EndTime, 'hh:mm tt') AS EndTime,
 CAST(
        DATEDIFF(MINUTE, a.StartTime, a.EndTime) / 60.0
        AS DECIMAL(10,2)
    ) AS Hours,
a.CreatedBy as CreatedId,
    a.ClientKey,
    a.Status,
    b.Firstname + ' ' + b.Lastname AS Username,
    a.MeetingKey
FROM IT_Meetings a
LEFT JOIN IT_EmployeeRegister b 
    ON a.CreatedBy = b.EmployeeKey
LEFT JOIN IT_MeetingParticipants c 
    ON a.MeetingKey = c.MeetingKey WHERE CAST(a.MeetingDate AS DATE) < CAST(GETDATE() AS DATE)";

            SqlCommand cmd = new SqlCommand(str_query);
            dt_dashboard = DA.GetDataTable(cmd);
            ds.Merge(dt_dashboard);
        }
        else
        {
            string str_query = @"
 SELECT DISTINCT
    a.MeetingTitle,

  FORMAT(a.MeetingDate, 'yyyy-MM-dd') AS MeetingDate,
FORMAT(a.StartTime, 'hh:mm tt') AS StartTime,
FORMAT(a.EndTime, 'hh:mm tt') AS EndTime,
 CAST(
        DATEDIFF(MINUTE, a.StartTime, a.EndTime) / 60.0
        AS DECIMAL(10,2)
    ) AS Hours,
a.CreatedBy as CreatedId,
    a.ClientKey,
    a.Status,
    b.Firstname + ' ' + b.Lastname AS Username,
    a.MeetingKey
FROM IT_Meetings a
LEFT JOIN IT_EmployeeRegister b 
    ON a.CreatedBy = b.EmployeeKey
LEFT JOIN IT_MeetingParticipants c 
    ON a.MeetingKey = c.MeetingKey
    WHERE CAST(a.MeetingDate AS DATE) < CAST(GETDATE() AS DATE) and
        (c.EmployeeKey = @UserId
        OR a.CreatedBy = @UserId )";

            SqlCommand cmd = new SqlCommand(str_query);
            cmd.Parameters.AddWithValue("@UserId", str_userid);
            dt_dashboard = DA.GetDataTable(cmd);
            ;
            ds.Merge(dt_dashboard);
        }

        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Status"))
                ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("View");
            ds.Tables[0].Columns.Add("delete");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Guid loggedInUserId = Guid.Parse(str_userid);
                int activetype = Convert.ToInt16(dr["Status"].ToString());
                int MeetingKey = Convert.ToInt16(dr["MeetingKey"].ToString());
                Guid createdId = Guid.Parse(dr["CreatedId"].ToString());


                    dr["ActiveText"] = "<span class='label label-sm label-success'>Completed</span>";
                

            }

            this.PH.LoadGridItem(ds, PH_Completed, "Completed.txt", "");
        }
        else
            return;
    }

    private void MeetingGrid()
    {
        DataTable dt_dashboard = new DataTable();
        DataSet ds = new DataSet();
        string str_userid = this.SC.Userid;

        if (str_userid == "1987df80-f1a7-4efe-a6bb-af04ad6aa9bd")
        {
            string str_query = @"
 SELECT DISTINCT
    a.MeetingTitle,

  FORMAT(a.MeetingDate, 'yyyy-MM-dd') AS MeetingDate,
FORMAT(a.StartTime, 'hh:mm tt') AS StartTime,
FORMAT(a.EndTime, 'hh:mm tt') AS EndTime,
a.CreatedBy as CreatedId,
    a.ClientKey,
    a.Status,
    b.Firstname + ' ' + b.Lastname AS Username,
    a.MeetingKey
FROM IT_Meetings a
LEFT JOIN IT_EmployeeRegister b 
    ON a.CreatedBy = b.EmployeeKey
LEFT JOIN IT_MeetingParticipants c 
    ON a.MeetingKey = c.MeetingKey where CAST(a.MeetingDate AS DATE) >= CAST(GETDATE() AS DATE)";

            SqlCommand cmd = new SqlCommand(str_query);
            dt_dashboard = DA.GetDataTable(cmd);
            ds.Merge(dt_dashboard);
        }
        else
        {
            string str_query = @"
 SELECT DISTINCT
    a.MeetingTitle,

  FORMAT(a.MeetingDate, 'yyyy-MM-dd') AS MeetingDate,
FORMAT(a.StartTime, 'hh:mm tt') AS StartTime,
FORMAT(a.EndTime, 'hh:mm tt') AS EndTime,
a.CreatedBy as CreatedId,
    a.ClientKey,
    a.Status,
    b.Firstname + ' ' + b.Lastname AS Username,
    a.MeetingKey
FROM IT_Meetings a
LEFT JOIN IT_EmployeeRegister b 
    ON a.CreatedBy = b.EmployeeKey
LEFT JOIN IT_MeetingParticipants c 
    ON a.MeetingKey = c.MeetingKey
    WHERE CAST(a.MeetingDate AS DATE) >= CAST(GETDATE() AS DATE) and
       ( c.EmployeeKey = @UserId
        OR a.CreatedBy = @UserId )";

            SqlCommand cmd = new SqlCommand(str_query);
            cmd.Parameters.AddWithValue("@UserId", str_userid);
            dt_dashboard = DA.GetDataTable(cmd);
            ;
            ds.Merge(dt_dashboard);
        }

        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Status"))
                ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("View");
            ds.Tables[0].Columns.Add("delete");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Guid loggedInUserId = Guid.Parse(str_userid);
                int activetype = Convert.ToInt16(dr["Status"].ToString());
                int MeetingKey = Convert.ToInt16(dr["MeetingKey"].ToString());
                Guid createdId = Guid.Parse(dr["CreatedId"].ToString());


                if (createdId == loggedInUserId)
                {
                    string encViewUpdate = UrlCrypto.Encrypt("1");

                    // ENABLE DELETE + UPDATE
                    dr["delete"] =
                        "<td><a href='javascript:void(0);'>" +
                        "<span class='label label-info' " +
                        "onclick=\"fn_DeleteProject('" + MeetingKey + "')\">Remove</span></a></td>";

                    dr["View"] =
                        "<td><a href='Meetingdetails.aspx?id=" + MeetingKey + "&Viewid=" + encViewUpdate + "'>" +
                        "<span class='label label-info'>Update</span></a></td>";
                }
                else
                {
                    string encViewOnly = UrlCrypto.Encrypt("0");
                    dr["delete"] =
                        "<td><span class='label label-default' " +
                        "style='cursor:not-allowed;opacity:0.6;'>Remove</span></td>";

                    dr["View"] =
                        "<td><a href='Meetingdetails.aspx?id=" + MeetingKey + "&Viewid=" + encViewOnly + "'>" +
                        "<span class='label label-info'>View</span></a></td>";
                }
                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-primary'>Scheduled</span>";
                }
                else if (activetype == 2)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Completed</span>";
                }
                else if (activetype == 3)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-danger'>Cancelled</span>";
                }
                else if (activetype == 4)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-warning'>Postponed</span>";
                }

            }

            this.PH.LoadGridItem(ds, PH_leave, "Meetings.txt", "");
        }
        else
            return;
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
            string str_Sql = "DELETE FROM IT_Meetings WHERE meetingkey=@Meetingkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Meetingkey", str_leadkey);

            DA1.ExecuteNonQuery(cmd);

            string str_partici = "Delete from IT_MeetingParticipants where MeetingKey=@MeetingKey ";
            SqlCommand cmd1 = new SqlCommand(str_partici);
            cmd1.Parameters.AddWithValue("@Meetingkey", str_leadkey);

            DA1.ExecuteNonQuery(cmd1);


            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}