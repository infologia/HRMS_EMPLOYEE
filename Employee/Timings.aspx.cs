using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Timings : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_userid = "";
    string str_intime = "";
    string str_CurrentDate = "";
    string str_addoneday = "";
    string str_dayworkinghours = "";
    DataTable dtLunch = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.str_userid = SC.Userid.ToString();

        DateTime utcToday = DateTime.UtcNow.Date;
        this.str_CurrentDate = utcToday.ToString("yyyy-MM-dd 00:00:00");
        this.str_addoneday = utcToday.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

        if (!IsPostBack)
        {
            this.chart();
            LoadUserTimings();
            LoadQuotes();
            LoadEmployeeOfMonth();
            SetButtonStatus();
            SetPageMessage();
            userstatus();
            Guid employeeKey = new Guid(this.str_userid);
            LoadEmployeeTaskSummary(employeeKey);
            LoadTodayStatus(employeeKey);
            //LoadRemainingWorkTime();
            //LoadTotalWorkedHours();
            LoadMyAttendance();
            LoadMonthlyPermission();
        }
    }

    private void LoadUserTimings()
    {

        string todayStr = DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        // Intime / Outtime
        string str_intimelable = @"SELECT TOP 1 intime, outtime 
                                   FROM IT_Inouttime 
                                   WHERE Createdby = @userid 
                                   AND CONVERT(varchar, modifiedon, 103) = @today
                                   ORDER BY modifiedon DESC";
        SqlCommand cmd = new SqlCommand(str_intimelable);
        cmd.Parameters.AddWithValue("@userid", str_userid);
        cmd.Parameters.AddWithValue("@today", todayStr);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["intime"] != DBNull.Value)
            {
                DateTime intimedt = Convert.ToDateTime(dt.Rows[0]["intime"]).AddMinutes(330);
                lbl_InTime.Text = intimedt.ToString("HH:mm");
                lbl_InTimeDate.Text = intimedt.ToString("dd MMM yyyy");
            }
            if (dt.Rows[0]["outtime"] != DBNull.Value)
            {
                DateTime outtimedt = Convert.ToDateTime(dt.Rows[0]["outtime"]).AddMinutes(330);
                lbl_OutTime.Text = outtimedt.ToString("HH:mm");
                lbl_OutTimeDate.Text = outtimedt.ToString("dd MMM yyyy");
            }
            if (dt.Rows[0]["intime"] != DBNull.Value && dt.Rows[0]["outtime"] != DBNull.Value)
            {
                btn_intime.Enabled = false;
            }
        }

        // Break
        string str_break = @"SELECT TOP 1 breakin, breakout 
                             FROM IT_Break 
                             WHERE Createdby = @userid 
                             AND CONVERT(varchar, modifiedon, 103) = @today
                             ORDER BY modifiedon DESC";
        SqlCommand cmdBreak = new SqlCommand(str_break);
        cmdBreak.Parameters.AddWithValue("@userid", str_userid);
        cmdBreak.Parameters.AddWithValue("@today", todayStr);
        DataTable dtBreak = DA.GetDataTable(cmdBreak);

        if (dtBreak.Rows.Count > 0)
        {
            if (dtBreak.Rows[0]["breakin"] != DBNull.Value)
            {
                DateTime breakindt = Convert.ToDateTime(dtBreak.Rows[0]["breakin"]).AddMinutes(330);
                lbl_LastBreak.Text = breakindt.ToString("HH:mm");
                lbl_LastBreakDate.Text = breakindt.ToString("dd MMM yyyy");
            }
            if (dtBreak.Rows[0]["breakout"] != DBNull.Value)
            {
                DateTime breakoutdt = Convert.ToDateTime(dtBreak.Rows[0]["breakout"]).AddMinutes(330);
                lbl_LastBreak.Text = breakoutdt.ToString("HH:mm");
                lbl_LastBreakDate.Text = breakoutdt.ToString("dd MMM yyyy");
            }
        }

        // Lunch
        string str_lunch = @"SELECT TOP 1 lunchin, lunchout 
                             FROM IT_Lunch 
                             WHERE Createdby = @userid 
                             AND CONVERT(varchar, modifiedon, 103) = @today
                             ORDER BY modifiedon DESC";
        SqlCommand cmdLunch = new SqlCommand(str_lunch);
        cmdLunch.Parameters.AddWithValue("@userid", str_userid);
        cmdLunch.Parameters.AddWithValue("@today", todayStr);
        dtLunch = DA.GetDataTable(cmdLunch);

        if (dtLunch.Rows.Count > 0)
        {
            if (dtLunch.Rows[0]["lunchin"] != DBNull.Value)
            {
                DateTime lunchinddt = Convert.ToDateTime(dtLunch.Rows[0]["lunchin"]).AddMinutes(330);
                lbl_LunchIn.Text = lunchinddt.ToString("HH:mm");
                lbl_LunchInDate.Text = lunchinddt.ToString("dd MMM yyyy");
            }
            if (dtLunch.Rows[0]["lunchout"] != DBNull.Value)
            {
                DateTime lunchoutdt = Convert.ToDateTime(dtLunch.Rows[0]["lunchout"]).AddMinutes(330);
                lbl_LunchOut.Text = lunchoutdt.ToString("HH:mm");
                lbl_LunchOutDate.Text = lunchoutdt.ToString("dd MMM yyyy");
            }
        }
    }

    private void SetButtonStatus()
    {
        string today = DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        string query = @"SELECT flag 
                         FROM (
                            SELECT flag, modifiedon FROM IT_Inouttime WHERE Createdby=@userid AND CONVERT(varchar, modifiedon,103)=@today
                            UNION ALL
                            SELECT flag, modifiedon FROM IT_Lunch WHERE Createdby=@userid AND CONVERT(varchar, modifiedon,103)=@today
                            UNION ALL
                            SELECT flag, modifiedon FROM IT_Break WHERE Createdby=@userid AND CONVERT(varchar, modifiedon,103)=@today
                         ) t ORDER BY modifiedon DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@userid", str_userid);
        cmd.Parameters.AddWithValue("@today", today);
        DataTable dt = DA.GetDataTable(cmd);

        btn_intime.Enabled = false;
        btn_outtime.Enabled = false;
        btn_breakin.Enabled = false;
        btn_breakout.Enabled = false;
        btn_lunchin.Enabled = false;
        btn_lunchout.Enabled = false;

        if (dt.Rows.Count > 0)
        {
            string flag = dt.Rows[0]["flag"].ToString();
            switch (flag)
            {
                case "1": btn_outtime.Enabled = btn_breakin.Enabled = btn_lunchin.Enabled = true; break;
                case "3": btn_breakout.Enabled = true; break;
                case "4": btn_outtime.Enabled = btn_breakin.Enabled = btn_lunchin.Enabled = true; break;
                case "5": btn_lunchout.Enabled = true; break;
                case "6": btn_outtime.Enabled = btn_breakin.Enabled = true; break;
                    //default: btn_intime.Enabled = true; break;
            }
        }
        else
        {
            btn_intime.Enabled = true;
        }

        if (dtLunch != null && dtLunch.Rows.Count > 0)
        {
            if (dtLunch.Rows[0]["lunchout"] != DBNull.Value &&
                !string.IsNullOrEmpty(dtLunch.Rows[0]["lunchout"].ToString()))
            {
                btn_lunchin.Enabled = false;
                btn_lunchout.Enabled = false;
            }
        }
    }

    private void SetPageMessage()
    {
        if (Request.QueryString["flag"] == null) return;

        div_error.Visible = true;
        string f = Request.QueryString["flag"];
        switch (f)
        {
            case "1": lbl_error.Text = "Your in time taken successfully"; break;
            case "2": lbl_error.Text = "Your out time taken successfully"; break;
            case "3": lbl_error.Text = "Your break in time taken successfully"; break;
            case "4": lbl_error.Text = "Your break out time taken successfully"; break;
            case "5": lbl_error.Text = "Your lunch in time taken successfully"; break;
            case "6": lbl_error.Text = "Your lunch out time taken successfully"; break;
        }
    }

    protected void btn_intime_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.UtcNow;
        string sql = @"INSERT INTO IT_Inouttime(employeekey, InTime, Createdby, Modifiedon, Createdon, Flag)
                       VALUES(@key, @in, @by, @mod, @cre, 1)";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@key", str_userid);
        cmd.Parameters.AddWithValue("@in", now);
        cmd.Parameters.AddWithValue("@by", str_userid);
        cmd.Parameters.AddWithValue("@mod", now);
        cmd.Parameters.AddWithValue("@cre", now);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/timings.aspx?flag=1");
    }

    protected void btn_outtime_Click(object sender, EventArgs e)
    {
        SqlCommand cmdIn = new SqlCommand("SELECT TOP 1 intime FROM IT_Inouttime WHERE Createdby=@uid AND Flag=1 ORDER BY Createdon DESC");
        cmdIn.Parameters.AddWithValue("@uid", str_userid);
        DataTable dt = DA.GetDataTable(cmdIn);

        if (dt.Rows.Count == 0) return;

        DateTime intime = Convert.ToDateTime(dt.Rows[0]["intime"]);
        TimeSpan diff = DateTime.UtcNow.Subtract(intime);
        string formatted = diff.ToString(@"hh\:mm\:ss");

        string sql = @"UPDATE IT_Inouttime 
                       SET OutTime=@out, Flag=2, Modifiedby=@by, Modifiedon=@mod, workinghours=@hours 
                       WHERE Inouttimekey =(SELECT TOP 1 Inouttimekey FROM IT_Inouttime WHERE Employeekey=@by AND Flag=1 AND OutTime IS NULL ORDER BY InTime DESC)";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@out", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@mod", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@by", str_userid);
        cmd.Parameters.AddWithValue("@hours", formatted);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/timings.aspx?flag=2");
    }

    protected void btn_breakin_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.UtcNow;
        string sql = "INSERT INTO IT_Break(BreakIn, Createdby, Createdon, Flag) VALUES(@in, @by, @cre, 3)";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@in", now);
        cmd.Parameters.AddWithValue("@by", str_userid);
        cmd.Parameters.AddWithValue("@cre", now);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/timings.aspx?flag=3");
    }

    protected void btn_breakout_Click(object sender, EventArgs e)
    {
        SqlCommand cmdGet = new SqlCommand("SELECT TOP 1 BreakIn FROM IT_Break WHERE Createdby=@uid AND Flag=3 ORDER BY modifiedon DESC");
        cmdGet.Parameters.AddWithValue("@uid", str_userid);
        DataTable dt = DA.GetDataTable(cmdGet);
        if (dt.Rows.Count == 0) return;

        DateTime bin = Convert.ToDateTime(dt.Rows[0]["BreakIn"]);
        TimeSpan diff = DateTime.UtcNow.Subtract(bin);
        string formatted = diff.ToString(@"hh\:mm\:ss");

        string sql = @"UPDATE IT_Break 
                       SET Breakout=@out, Flag=4, Modifiedon=@mod, Breakhours=@hours 
                       WHERE Createdby=@by AND Breakout IS NULL";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@out", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@mod", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@hours", formatted);
        cmd.Parameters.AddWithValue("@by", str_userid);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/timings.aspx?flag=4");
    }

    protected void btn_lunchin_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.UtcNow;
        string sql = "INSERT INTO IT_Lunch(LunchIn, Createdby, Createdon, Flag) VALUES(@in, @by, @cre, 5)";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@in", now);
        cmd.Parameters.AddWithValue("@by", str_userid);
        cmd.Parameters.AddWithValue("@cre", now);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/timings.aspx?flag=5");
    }

    protected void btn_lunchout_Click(object sender, EventArgs e)
    {
        SqlCommand cmdGet = new SqlCommand("SELECT TOP 1 LunchIn FROM IT_Lunch WHERE Createdby=@uid AND Flag=5 ORDER BY modifiedon DESC");
        cmdGet.Parameters.AddWithValue("@uid", str_userid);
        DataTable dt = DA.GetDataTable(cmdGet);
        if (dt.Rows.Count == 0) return;

        DateTime lin = Convert.ToDateTime(dt.Rows[0]["LunchIn"]);
        TimeSpan diff = DateTime.UtcNow.Subtract(lin);
        string formatted = diff.ToString(@"hh\:mm\:ss");

        string sql = @"UPDATE IT_Lunch 
                       SET Lunchout=@out, Flag=6, Modifiedon=@mod, Lunchhours=@hours 
                       WHERE Createdby=@by AND Lunchout IS NULL";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@out", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@mod", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@hours", formatted);
        cmd.Parameters.AddWithValue("@by", str_userid);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/timings.aspx?flag=6");
    }

    private void LoadQuotes()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        // Check if today's quote already exists
        SqlCommand cmdCheck = new SqlCommand("SELECT Quote, Author FROM IT_DailyQuote WHERE QuoteDate=@Date");
        cmdCheck.Parameters.AddWithValue("@Date", today);

        DataTable dtToday = DA.GetDataTable(cmdCheck);
        if (dtToday.Rows.Count > 0)
        {
            // ✅ Show same quote for whole day
            lbl_quotes.Text = dtToday.Rows[0]["Quote"].ToString();
            lbl_author.Text = dtToday.Rows[0]["Author"].ToString();
            return;
        }

        // No quote saved today → pick a new random one
        DataTable dtRandom = DA.GetDataTable("SELECT TOP 1 Quote, Author FROM IT_Quotes ORDER BY NEWID()");

        if (dtRandom.Rows.Count > 0)
        {
            string quote = dtRandom.Rows[0]["Quote"].ToString();
            string author = dtRandom.Rows[0]["Author"].ToString();

            // Display it
            lbl_quotes.Text = quote;
            lbl_author.Text = author;

            // ✅ Insert for all users today
            SqlCommand cmdInsert = new SqlCommand("INSERT INTO IT_DailyQuote (QuoteDate, Quote, Author) VALUES (@Date, @Quote, @Author)");
            cmdInsert.Parameters.AddWithValue("@Date", today);
            cmdInsert.Parameters.AddWithValue("@Quote", quote);
            cmdInsert.Parameters.AddWithValue("@Author", author);

            DA.ExecuteNonQuery(cmdInsert);
        }
    }

    private void LoadEmployeeOfMonth()
    {
        string sql = @"SELECT b.firstname, b.lastname, b.image 
                       FROM IT_Employeeofthemonth a 
                       LEFT JOIN IT_EmployeeRegister b ON a.employeekey=b.employeekey 
                       WHERE a.employeeyear = YEAR(GETUTCDATE()) 
                       AND a.employeemonth = MONTH(GETUTCDATE()) and Employeestatus=1";
        DataTable DT_image = DA.GetDataTable(sql);
        //if (dt.Rows.Count > 0)
        // {
        //     lbl_employeeofthemonth.Text = dt.Rows[0]["firstname"].ToString() + " " + dt.Rows[0]["lastname"].ToString();
        //    Img_Profile.ImageUrl = "~/images/EmployeePRofilePictures/" + dt.Rows[0]["Image"].ToString();
        // }
        if (DT_image.Rows.Count > 0)
        {
            lbl_employeeofthemonth.Text = DT_image.Rows[0]["firstname"].ToString() + " " + DT_image.Rows[0]["lastname"].ToString();
            string str_loadimage = DT_image.Rows[0]["Image"].ToString();

            if (str_loadimage == "")
            {
                Img_Profile.ImageUrl = "~/Images/nopicture.jpg";
                Img_Profile.ImageUrl = "~/Images/nopicture.jpg";
            }
            else
            {
                Img_Profile.ImageUrl = "~/Images/Adminprofilepictures/" + str_loadimage;
                Img_Profile.ImageUrl = "~/Images/Adminprofilepictures/" + str_loadimage;
            }
        }
    }

    public void chart()
    {
        // Chart logic (unchanged, can re-enable later)
    }

    public void userstatus()
    {
        string str_userstatus = "select Username,Image,flag from IT_V_Userstatus";
        DataTable DA_userstatus = DA.GetDataTable(str_userstatus);
        if (DA_userstatus.Rows.Count > 0)
        {
            // Add Document column if it doesn't exist
            if (!DA_userstatus.Columns.Contains("Document"))
            {
                DA_userstatus.Columns.Add("Document", typeof(string));
                if (DA_userstatus.Columns.Contains("flag"))
                    DA_userstatus.Columns.Add("ActiveText");
            }

            foreach (DataRow row in DA_userstatus.Rows)
            {
                short activetype = row["flag"] == DBNull.Value ? (short)0 : Convert.ToInt16(row["flag"]);

                //int activetype = Convert.ToInt16(row["flag"].ToString());
                if (activetype == 1)
                    row["ActiveText"] = "<span class='label label-success'>Online</span>";
                else if (activetype == 2)
                    row["ActiveText"] = "<span class='label label-default'>Offline</span>";
                else if (activetype == 3)
                    row["ActiveText"] = "<span class='label label-primary'>Break</span>";
                else if (activetype == 4)
                    row["ActiveText"] = "<span class='label label-success'>Online</span>";
                else if (activetype == 5)
                    row["ActiveText"] = "<span class='label label-primary'>Lunch</span>";
                else if (activetype == 6)
                    row["ActiveText"] = "<span class='label label-success'>Online</span>";
                else if (activetype == 0)
                    row["ActiveText"] = "<span class='label label-danger'>Leave</span>";

                string imageFile = row["Image"].ToString();

                if (!string.IsNullOrEmpty(imageFile))
                {
                    row["Document"] = "../Images/Adminprofilepictures/" + imageFile;
                }
                else
                {
                    row["Document"] = "../Images/nopicture.jpg";
                }
            }
        }
        DataSet ds = new DataSet();
        ds.Merge(DA_userstatus);
        this.PH.LoadGridItem(ds, PH_Userlist, "Userstatus.txt", "");
    }

    public void LoadEmployeeTaskSummary(Guid employeeKey)
    {
        string sql = @"
    SELECT 
        p.ProjectName,
        CAST(tc.StartDate AS DATE) AS TaskDate,
        COUNT(td.TaskDetailID) AS TotalTasks,
        SUM(CASE WHEN td.Status = 1 THEN 1 ELSE 0 END) AS AssignedTasks,
        SUM(CASE WHEN td.Status = 2 THEN 1 ELSE 0 END) AS OngoingTasks,
        SUM(CASE WHEN td.Status = 3 THEN 1 ELSE 0 END) AS PendingTasks,
        SUM(CASE WHEN td.Status = 4 THEN 1 ELSE 0 END) AS CompletedTasks,
        SUM(CASE WHEN td.Status = 6 THEN 1 ELSE 0 END) AS OverdueTasks
    FROM IT_TaskCreation tc
    INNER JOIN IT_Projects p ON p.ProjectKey = tc.ProjectName
    LEFT JOIN IT_TaskDescriptiondetails td ON td.TaskKey = tc.TaskKey
    WHERE 
        tc.EmployeeList = @EmployeeKey
        AND CAST(tc.StartDate AS DATE) = CAST(GETDATE() AS DATE)
    GROUP BY p.ProjectName, CAST(tc.StartDate AS DATE)
    ORDER BY p.ProjectName";

        SqlCommand cmdTask = new SqlCommand(sql);
        cmdTask.Parameters.Add("@EmployeeKey", System.Data.SqlDbType.UniqueIdentifier).Value = employeeKey;
        DataTable dt = DA.GetDataTable(cmdTask);

        // If no projects/tasks for today, create a dummy row with zeros
        if (dt.Rows.Count == 0)
        {
            dt = new DataTable();
            dt.Columns.Add("ProjectName", typeof(string));
            dt.Columns.Add("TaskDate", typeof(DateTime));
            dt.Columns.Add("TotalTasks", typeof(int));
            dt.Columns.Add("AssignedTasks", typeof(int));
            dt.Columns.Add("OngoingTasks", typeof(int));
            dt.Columns.Add("PendingTasks", typeof(int));
            dt.Columns.Add("CompletedTasks", typeof(int));
            dt.Columns.Add("OverdueTasks", typeof(int));
            dt.Rows.Add("No Tasks Assigned", DateTime.Today, 0, 0, 0, 0, 0, 0);
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (dt.Rows.Count == 1 && dt.Rows[0]["ProjectName"].ToString() == "No Tasks Assigned")
        {
            sb.Append("<p style='color:#999;font-size:13px;text-align:center;padding:10px;'>No tasks assigned for today.</p>");
        }
        else
        {
            foreach (DataRow row in dt.Rows)
            {
                string project = row["ProjectName"].ToString();
                int total      = Convert.ToInt32(row["TotalTasks"]);
                int assigned   = Convert.ToInt32(row["AssignedTasks"]);
                int ongoing    = Convert.ToInt32(row["OngoingTasks"]);
                int pending    = Convert.ToInt32(row["PendingTasks"]);
                int completed  = Convert.ToInt32(row["CompletedTasks"]);
                int overdue    = Convert.ToInt32(row["OverdueTasks"]);

                sb.Append("<div style='margin-bottom:12px;border:1px solid #e0e0e0;border-radius:8px;overflow:hidden;'>");

                // Project header
                sb.Append("<div style='background:#f5f5f5;padding:7px 10px;font-size:12px;font-weight:700;color:#333;border-bottom:1px solid #e0e0e0;'>");
                sb.Append("&#128193; " + System.Web.HttpUtility.HtmlEncode(project));
                sb.Append("<span style='float:right;color:#888;font-weight:400;'>" + total + " task" + (total != 1 ? "s" : "") + "</span>");
                sb.Append("</div>");

                // Status rows sub-table
                sb.Append("<table style='width:100%;border-collapse:collapse;font-size:12px;'>");

                if (assigned > 0)
                    sb.Append("<tr style='border-bottom:1px solid #f0f0f0;'><td style='padding:5px 10px;color:#555;'>&#128309; Yet to Start</td><td style='padding:5px 10px;text-align:right;'><span style='background:#17a2b8;color:#fff;border-radius:10px;padding:2px 8px;font-weight:600;'>" + assigned + "</span></td></tr>");

                if (ongoing > 0)
                    sb.Append("<tr style='border-bottom:1px solid #f0f0f0;'><td style='padding:5px 10px;color:#555;'>&#128994; In Progress</td><td style='padding:5px 10px;text-align:right;'><span style='background:#2196f3;color:#fff;border-radius:10px;padding:2px 8px;font-weight:600;'>" + ongoing + "</span></td></tr>");

                if (pending > 0)
                    sb.Append("<tr style='border-bottom:1px solid #f0f0f0;'><td style='padding:5px 10px;color:#555;'>&#128993; Pending</td><td style='padding:5px 10px;text-align:right;'><span style='background:#f0ad4e;color:#fff;border-radius:10px;padding:2px 8px;font-weight:600;'>" + pending + "</span></td></tr>");

                if (completed > 0)
                    sb.Append("<tr style='border-bottom:1px solid #f0f0f0;'><td style='padding:5px 10px;color:#555;'>&#9989; Completed</td><td style='padding:5px 10px;text-align:right;'><span style='background:#4caf50;color:#fff;border-radius:10px;padding:2px 8px;font-weight:600;'>" + completed + "</span></td></tr>");

                if (overdue > 0)
                    sb.Append("<tr><td style='padding:5px 10px;color:#555;'>&#128308; Overdue</td><td style='padding:5px 10px;text-align:right;'><span style='background:#f44336;color:#fff;border-radius:10px;padding:2px 8px;font-weight:600;'>" + overdue + "</span></td></tr>");

                if (assigned == 0 && ongoing == 0 && pending == 0 && completed == 0 && overdue == 0)
                    sb.Append("<tr><td colspan='2' style='padding:6px 10px;color:#aaa;font-style:italic;'>No sub-tasks found</td></tr>");

                sb.Append("</table>");
                sb.Append("</div>");
            }
        }

        PH_TaskSummary.Controls.Add(new Literal { Text = sb.ToString() });
    }

    protected void LoadTodayStatus(Guid employeeKey)
    {
        DateTime inTime;
        DateTime Outtime;
        string sql_str = @"select WorkDate,InTime,outtime,BreakDuration,LunchDuration,NetWorkingDuration from IT_V_EmployeeDailyWorkSummary WHERE Employeekey =@userid AND WorkDate = CAST(GETDATE() AS DATE)";

        SqlCommand cmd1 = new SqlCommand(sql_str);
        cmd1.Parameters.AddWithValue("@userid", str_userid);
        // cmd.Parameters.AddWithValue("@today", today);

        DataTable dt_workingduration = DA.GetDataTable(cmd1);

        
        if (dt_workingduration.Rows.Count > 0)
        {

            if (dt_workingduration.Rows[0]["InTime"] != DBNull.Value)
            {
                 inTime = Convert.ToDateTime(dt_workingduration.Rows[0]["InTime"]).AddMinutes(330); // IST
                Ltr_Intime.Text = inTime.ToString("dd-MM-yyyy hh:mm tt");
            }
            if (dt_workingduration.Rows[0]["outtime"] != DBNull.Value)
            {
                Outtime = Convert.ToDateTime(dt_workingduration.Rows[0]["outtime"]).AddMinutes(330); // IST
                Ltr_Outtime.Text = Outtime.ToString("dd-MM-yyyy hh:mm tt");
            }
           
            
            Ltr_LunchDuration.Text = dt_workingduration.Rows[0]["lunchDuration"].ToString();
            Ltr_Breakduration.Text = dt_workingduration.Rows[0]["BreakDuration"].ToString();
            Ltr_WorkedHours.Text = dt_workingduration.Rows[0]["Networkingduration"].ToString();

        }
    }

    //private void LoadRemainingWorkTime()
    //{
    //    string today = DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    //    string sql = @"SELECT TOP 1 InTime 
    //               FROM IT_Inouttime 
    //               WHERE Createdby = @userid 
    //               AND CONVERT(varchar, modifiedon, 103) = @today
    //               ORDER BY modifiedon DESC";

    //    SqlCommand cmd = new SqlCommand(sql);
    //    cmd.Parameters.AddWithValue("@userid", str_userid);
    //    cmd.Parameters.AddWithValue("@today", today);

    //    DataTable dt = DA.GetDataTable(cmd);

    //    // InTime not marked
    //    if (dt.Rows.Count == 0 || dt.Rows[0]["InTime"] == DBNull.Value)
    //    {
    //        litTimeLeft.Text = "8h 0m";
    //        return;
    //    }

    //    DateTime inTime = Convert.ToDateTime(dt.Rows[0]["InTime"]).AddMinutes(330); // IST
    //    DateTime now = DateTime.UtcNow.AddMinutes(330);

    //    TimeSpan worked = now - inTime;
    //    TimeSpan required = TimeSpan.FromHours(8);
    //    TimeSpan remaining = required - worked;

    //    //if (remaining.TotalSeconds <= 0)
    //    //{
    //    //    litTimeLeft.Text =
    //    //        "<span class='text-success'>You can logout now 🎉</span>";
    //    //    return;
    //    //}

    //    litTimeLeft.Text = string.Format("{0}h {1}m", remaining.Hours, remaining.Minutes);
    //}

    //private void LoadTotalWorkedHours()
    //{
    //    string today = DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    //    string sql = @"SELECT TOP 1 InTime 
    //               FROM IT_Inouttime 
    //               WHERE Createdby = @userid
    //               AND CONVERT(varchar, modifiedon, 103) = @today
    //               ORDER BY modifiedon DESC";

    //    SqlCommand cmd = new SqlCommand(sql);
    //    cmd.Parameters.AddWithValue("@userid", str_userid);
    //    cmd.Parameters.AddWithValue("@today", today);

    //    DataTable dt = DA.GetDataTable(cmd);

    //    // Not clocked in
    //    if (dt.Rows.Count == 0 || dt.Rows[0]["InTime"] == DBNull.Value)
    //    {
    //        litTotalWorked.Text = "0h 0m";
    //        return;
    //    }

    //    DateTime inTime = Convert.ToDateTime(dt.Rows[0]["InTime"]).AddMinutes(330); // IST
    //    DateTime now = DateTime.UtcNow.AddMinutes(330);

    //    TimeSpan worked = now - inTime;

    //    if (worked.TotalSeconds < 0)
    //        worked = TimeSpan.Zero;

    //    litTotalWorked.Text = string.Format(
    //        "{0}h {1}m",
    //        worked.Hours,
    //        worked.Minutes
    //    );
    //}

    private void LoadMyAttendance()
    {
        Guid empKey = new Guid(str_userid);

        string sql = @"
        SELECT 
            Employeekey,
            SUM(CAST(ISNULL(LeaveDays, 0) AS DECIMAL(5,2))) AS TotalLeaveTaken,
            CASE 
                WHEN SUM(CAST(ISNULL(LeaveDays, 0) AS DECIMAL(5,2))) > 12
                THEN SUM(CAST(ISNULL(LeaveDays, 0) AS DECIMAL(5,2))) - 12
                ELSE 0
            END AS LOP
        FROM IT_EmployeeLeaveDetails
        WHERE Responsestatus = 2
          AND Employeekey = @EmpKey
          AND (
                (FromDate <= GETDATE() 
                 AND ToDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1))
                OR
                (FromDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
                 AND FromDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31))
              )
        GROUP BY Employeekey";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@EmpKey", empKey);

        DataTable dt = DA.GetDataTable(cmd);

        // No leave records
        if (dt.Rows.Count == 0)
        {
            litLeaveTaken.Text = "0";
            litLOPDays.Text = "0";
            litBalanceLeave.Text = "12";
            return;
        }

        decimal leaveTaken = Convert.ToDecimal(dt.Rows[0]["TotalLeaveTaken"]);
        decimal lopDays = Convert.ToDecimal(dt.Rows[0]["LOP"]);

        decimal paidLeaveLimit = 12;
        decimal balanceLeave = paidLeaveLimit - leaveTaken;
        if (balanceLeave < 0) balanceLeave = 0;

        // Display with clean formatting
        litLeaveTaken.Text = leaveTaken.ToString("0.##");
        litLOPDays.Text = lopDays.ToString("0.##");
        litBalanceLeave.Text = balanceLeave.ToString("0.##");
    }



    private void LoadMonthlyPermission()
    {
        Guid empKey = new Guid(str_userid);

        int monthlyLimitMinutes = 3 * 60; // 3 Hours per month
        int usedMinutes = 0;

        string sql = @"
        SELECT 
            ISNULL(SUM(DATEDIFF(MINUTE, Fromtime, Totime)), 0) AS UsedMinutes
        FROM IT_EmployeePermissionDetails
        WHERE Employeekey = @EmpKey
          AND Responsestatus = 2
          AND MONTH(Requestdate) = MONTH(GETDATE())
          AND YEAR(Requestdate) = YEAR(GETDATE())
          AND Requestdate <= CAST(GETDATE() AS DATE)";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@EmpKey", empKey);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            usedMinutes = Convert.ToInt32(dt.Rows[0]["UsedMinutes"]);
        }

        int balanceMinutes = monthlyLimitMinutes - usedMinutes;
        if (balanceMinutes < 0)
            balanceMinutes = 0;

        // Used
        int usedHours = usedMinutes / 60;
        int usedMins = usedMinutes % 60;

        // Balance
        int balHours = balanceMinutes / 60;
        int balMins = balanceMinutes % 60;

        string usedText =
            usedHours + " Hour" + (usedHours != 1 ? "s " : " ")
            + usedMins + " Mins";

        string balanceText =
            balHours + " Hour" + (balHours != 1 ? "s " : " ")
            + balMins + " Mins";

        // 🔥 Bind to UI
        litPermissionUsed.Text = usedText;
        litPermissionBalance.Text = balanceText;
    }

}
