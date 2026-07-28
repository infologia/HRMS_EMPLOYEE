using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;

public partial class Masterpage_AdminMaster : System.Web.UI.MasterPage
{
    SessionCustom SC;
    DataAccess DA;
    AppVar AP;
    LogWriter LW;
    PhTemplate PH;
    PhTemplateExt PHT;

    string str_SectionDocumentUrl = new HttpLocate().WebRoot + "Uploads/";
    string str_userid = "";
    int int_role;
    string userdest = "";
    string str_userdesgn = "";
    string str_userky = "";
    string str_CurrentDate = "";
    string str_addoneday = "";
    string str_key = "";
    string IPAddress = "";



    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();
        this.AP = new AppVar();
        this.LW = new LogWriter();
        this.PH = new PhTemplate();
        this.PHT = new PhTemplateExt();
        this.str_userid = SC.Userid.ToString();
        this.int_role = Convert.ToInt16(SC.UserRole);
        this.userdest = this.SC.Userdesg;

        //ip

        //if (int_role == 0)
        //{
        //    div_intimeshow.Visible = false;
        //    div_outtimeshow.Visible = false;

        //}
        DateTime date = DateTime.UtcNow;
        string date1 = date.ToString("yyyy-MM-dd");
        this.str_CurrentDate = date1 + " " + "00:00:00";
        this.str_addoneday = DateTime.UtcNow.AddDays(+1).ToString("yyyy-MM-dd");
        this.str_addoneday = str_addoneday + " " + "00:00:00";

        if (!IsPostBack)
        {
            this.Timegrid();
            GetAndSetDepartment();

        }

        //Demo
        // Convert UTC → IST
        string today = DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


        // 1. INTIME / OUTTIME

        string qInOut = @"select top 1 intime,outtime,modifiedon 
                  from IT_Inouttime 
                  WHERE Createdby='" + str_userid + @"' 
                  and Convert(varchar, modifiedon, 103)= '" + today + @"'
                  and flag in (1,2)
                  ORDER BY modifiedon desc";

        DataTable dtInOut = DA.GetDataTable(qInOut);


        string finalMessage = "";
        DateTime finalTime = DateTime.MinValue;

        if (dtInOut.Rows.Count > 0)
        {
            string InTime = dtInOut.Rows[0]["intime"].ToString();
            string OutTime = dtInOut.Rows[0]["outtime"].ToString();
            DateTime modified = Convert.ToDateTime(dtInOut.Rows[0]["modifiedon"]);

            if (InTime != "")
            {
                finalMessage = "Your Intime is : ";
                finalTime = Convert.ToDateTime(InTime);
            }
            if (OutTime != "")
            {
                finalMessage = "Your Outtime is : ";
                finalTime = Convert.ToDateTime(OutTime);
            }
        }


        // 2. BREAK IN / BREAK OUT

        string qBreak = @"select top 1 breakin,breakout,modifiedon 
                  from IT_break 
                  WHERE Createdby='" + str_userid + @"'
                  and Convert(varchar, modifiedon, 103)= '" + today + @"'
                  and flag in (3,4)
                  ORDER BY modifiedon desc";

        DataTable dtBreak = DA.GetDataTable(qBreak);

        if (dtBreak.Rows.Count > 0)
        {
            DateTime modified = Convert.ToDateTime(dtBreak.Rows[0]["modifiedon"]);


            if (modified > finalTime)
            {
                string bin = dtBreak.Rows[0]["breakin"].ToString();
                string bout = dtBreak.Rows[0]["breakout"].ToString();

                if (bin != "")
                {
                    finalMessage = "Enjoy your time ";
                    finalTime = Convert.ToDateTime(bin);
                }
                if (bout != "")
                {
                    finalMessage = "Your time's up ";
                    finalTime = Convert.ToDateTime(bout);
                }
            }
        }

        // 3. LUNCH IN / LUNCH OUT

        string qLunch = @"select top 1 lunchin,lunchout,modifiedon 
                  from IT_lunch 
                  WHERE Createdby='" + str_userid + @"'
                  and Convert(varchar, modifiedon, 103)= '" + today + @"'
                  and flag in (5,6)
                  ORDER BY modifiedon desc";

        DataTable dtLunch = DA.GetDataTable(qLunch);

        if (dtLunch.Rows.Count > 0)
        {
            DateTime modified = Convert.ToDateTime(dtLunch.Rows[0]["modifiedon"]);


            if (modified > finalTime)
            {
                string lin = dtLunch.Rows[0]["lunchin"].ToString();
                string lout = dtLunch.Rows[0]["lunchout"].ToString();

                if (lin != "")
                {
                    finalMessage = "Enjoy your meal ";
                    finalTime = Convert.ToDateTime(lin);
                }
                if (lout != "")
                {
                    finalMessage = "Lunch Out ";
                    finalTime = Convert.ToDateTime(lout);
                }
            }
        }


        // FINAL OUTPUT

        if (finalMessage != "")
        {
            lbl_dashtime.Text = finalMessage + finalTime.AddMinutes(330).ToString("dd-MM-yyyy HH:mm:ss");
        }
        else
        {
            lbl_dashtime.Text = "Welcome to Infologia";
        }



        //Demo


        string str_image = "select * from IT_employeeregister where Employeekey=@Employeekey";
        SqlCommand cmd = new SqlCommand(str_image);
        cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        DataTable DT_image = DA.GetDataTable(cmd);
        if (DT_image.Rows.Count > 0)
        {

            string str_loadimage = DT_image.Rows[0]["Image"].ToString();

            if (str_loadimage == "")
            {
                Img_Profile.ImageUrl = "~/Images/nopicture.jpg";
                image_small.ImageUrl = "~/Images/nopicture.jpg";
            }
            else
            {
                Img_Profile.ImageUrl = "~/Images/Adminprofilepictures/" + str_loadimage;
                image_small.ImageUrl = "~/Images/Adminprofilepictures/" + str_loadimage;
            }
        }


        //string str_intimecheckdiff = DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        //string str_dash = "  select top 1  intime from IT_inouttimes where createdby='" + str_userid+ "' and Convert(varchar, createdon, 103)= '" + str_intimecheckdiff + "' order by createdon desc";
        //DataTable dt_dashlabel = DA.GetDataTable(str_dash);
        //if (dt_dashlabel.Rows.Count > 0)
        //{
        //	string lbl_intimeshow = dt_dashlabel.Rows[0]["intime"].ToString();
        //	if (lbl_intimeshow != null && lbl_intimeshow != "")
        //	{
        //		DateTime datelbl_InTime = Convert.ToDateTime(lbl_intimeshow);
        //		lbl_dashtime.Text = datelbl_InTime.AddMinutes(330).ToString();
        //	}
        //	else
        //	{
        //		lbl_dashtime.Text = " Welcome !! Oops you're missed ";
        //	}
        //if (dt_dashlabel.Rows.Count > 0)
        //{
        //	lbl_dashtime.Text = dt_dashlabel.Rows[0]["intime"].ToString();
        //}


        string str_desg = "select * from it_destination where destinationid='" + this.userdest + "'";
        SqlCommand cmd1 = new SqlCommand(str_desg);
        cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        DataTable DT_desg = DA.GetDataTable(cmd1);
        if (DT_desg.Rows.Count > 0)
        {
            this.str_userdesgn = DT_desg.Rows[0]["Destinationkey"].ToString();

        }

        // string str_desg = "select * from it_destination where destinationid='"+this.userdest+"'";
        //SqlCommand cmd1 = new SqlCommand(str_desg);
        ////cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        //DataTable DT_desg = DA.GetDataTable(cmd1);
        //if (DT_desg.Rows.Count > 0)
        //{
        //    this.str_userdesgn = DT_desg.Rows[0]["Destinationkey"].ToString();

        //}

        //string VisitorsIPAddr = string.Empty;
        //if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        //{
        //    VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        //}
        //else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        //{
        //    VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        //}

        //XmlDocument doc = new XmlDocument();

        //string getdetails = "http://www.freegeoip.net/xml/" + VisitorsIPAddr;

        //doc.Load(getdetails);

        //XmlNodeList nodeLstCity = doc.GetElementsByTagName("City");
        //XmlNodeList nodeLstCountry = doc.GetElementsByTagName("CountryName");

        //Label1.Text = nodeLstCity[0].InnerText + "," + nodeLstCountry[0].InnerText;
        lbl_username.Text = this.SC.username;
        lbl_usernametop.Text = this.SC.username;

        if (!IsPostBack)
        {
            LoadUserMenu();
        }
        this.GetIPAddress();

    }

    private void GetAndSetDepartment()
    {
        string departmentName = "";

        string query = @"
        SELECT d.Departmentname FROM IT_EmployeeRegister e
INNER JOIN IT_Department d
    ON e.Department = d.Departmentid where e.Employeekey='" + SC.Userid + "'";

        string str_userid = this.SC.Userid;
        using (SqlCommand cmd = new SqlCommand(query))
        {
            cmd.Parameters.AddWithValue("@EmployeeKey", "EmployeeKey");

            DataTable dt = DA.GetDataTable(cmd);


            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                departmentName = dt.Rows[0]["Departmentname"].ToString();
            }
        }

        lbl_destination.Text = departmentName;
    }



    public string GetIPAddress()
    {

        string hostName = Dns.GetHostName(); // Retrive the Name of HOST  

        string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

        IPHostEntry Host = default(IPHostEntry);
        string Hostname = null;
        Hostname = System.Environment.MachineName;
        Host = Dns.GetHostEntry(Hostname);
        foreach (IPAddress IP in Host.AddressList)
        {
            if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                IPAddress = Convert.ToString(IP);
            }
        }

        string VisitorsIPAddr = string.Empty;
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        {
            VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        }

        //if (VisitorsIPAddr == "192.168.1.16" || VisitorsIPAddr == "27.57.24.71" ||VisitorsIPAddr =="122.174.203.134")
        //{
        //    intime.Visible = true;
        //    outTime.Visible = true;
        //}
        //else
        //{

        //    intime.Visible = false;
        //    outTime.Visible = false;

        //}
        return IPAddress;
    }

    private void LoadUserMenu()
    {
        // MenuOrder per employee, fallback to MenuListNo from IT_Menus
        string query = @"SELECT DISTINCT m.MenuKey, m.MenuName, m.MenuIcon, m.MenuListNo,
                        m.ParentMenuKey, m.PageName, m.FolderName, m.MenuType, m.ModuleId,
                        CASE WHEN ISNULL(em.MenuOrder, 0) = 0 THEN m.MenuListNo ELSE em.MenuOrder END AS EffectiveOrder
                        FROM IT_Menus m
                        INNER JOIN IT_EmployeeMenus em ON m.MenuKey = em.MenuId
                        WHERE m.Status = 1 AND em.EmployeeKey = @EmployeeKey AND em.ViewPermission = 1
                        ORDER BY m.ModuleId, CASE WHEN ISNULL(em.MenuOrder, 0) = 0 THEN m.MenuListNo ELSE em.MenuOrder END";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", SC.Userid);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count == 0)
        {
            Literal1.Text = "<li><a href='#'><i class='icon-menu'></i><span>No Menu Available</span></a></li>";
            return;
        }

        StringBuilder html = new StringBuilder();

        var parentMenus = dt.AsEnumerable()
            .Where(r => r["MenuType"].ToString() == "0" && r.IsNull("ParentMenuKey"))
            .OrderBy(r => r["ModuleId"] == DBNull.Value ? 0 : Convert.ToInt32(r["ModuleId"]))
            .ThenBy(r => Convert.ToInt32(r["EffectiveOrder"]))
            .ToArray();

        string prevModuleId = null;
        bool isFirst = true;

        foreach (DataRow parent in parentMenus)
        {
            string menuKey    = parent["MenuKey"].ToString();
            string menuName   = parent["MenuName"].ToString();
            string menuIcon   = string.IsNullOrEmpty(parent["MenuIcon"].ToString()) ? "icon-menu" : parent["MenuIcon"].ToString();
            string pageName   = parent["PageName"].ToString();
            string folderName = parent["FolderName"].ToString();
            string moduleId   = parent["ModuleId"] != DBNull.Value ? parent["ModuleId"].ToString() : "0";

            if (!isFirst && moduleId != prevModuleId && prevModuleId == "5")
            {
                // Add a dotted separator (thicker and in primary theme color)
                html.Append("<li class='navigation-divider' style='border-top: 2px dotted #667eea; margin: 12px 15px; list-style: none; opacity: 0.8;'></li>");
            }
            isFirst = false;
            prevModuleId = moduleId;

            // Children — per-employee order-ல் sort
            var children = dt.AsEnumerable()
                .Where(r => !r.IsNull("ParentMenuKey") && r["ParentMenuKey"].ToString() == menuKey)
                .OrderBy(r => Convert.ToInt32(r["EffectiveOrder"]))
                .ToArray();

            if (children.Length > 0)
            {
                html.AppendFormat("<li><a href='javascript:void(0);'><i class='{0}'></i><span>{1}</span></a><ul>",
                    menuIcon, menuName);
                BuildSubMenus(html, dt, children);
                html.Append("</ul></li>");
            }
            else
            {
                if (!string.IsNullOrEmpty(pageName))
                    html.AppendFormat("<li><a href='../{0}/{1}'><i class='{2}'></i><span>{3}</span></a></li>",
                        folderName, pageName, menuIcon, menuName);
                else
                    html.AppendFormat("<li><a href='javascript:void(0);'><i class='{0}'></i><span>{1}</span></a></li>",
                        menuIcon, menuName);
            }
        }

        Literal1.Text = html.ToString();
    }

    private void BuildSubMenus(StringBuilder html, DataTable dt, DataRow[] menus)
    {
        foreach (DataRow menu in menus)
        {
            string menuKey    = menu["MenuKey"].ToString();
            string menuName   = menu["MenuName"].ToString();
            string menuIcon   = menu["MenuIcon"].ToString();
            string pageName   = menu["PageName"].ToString();
            string folderName = menu["FolderName"].ToString();

            var children = dt.AsEnumerable()
                .Where(r => !r.IsNull("ParentMenuKey") && r["ParentMenuKey"].ToString() == menuKey)
                .OrderBy(r => Convert.ToInt32(r["EffectiveOrder"]))
                .ToArray();

            if (children.Length > 0)
            {
                if (!string.IsNullOrEmpty(menuIcon))
                    html.AppendFormat("<li><a href='javascript:void(0);'><i class='{0}'></i><span>{1}</span></a><ul>", menuIcon, menuName);
                else
                    html.AppendFormat("<li><a href='javascript:void(0);'><span>{0}</span></a><ul>", menuName);

                BuildSubMenus(html, dt, children);
                html.Append("</ul></li>");
            }
            else
            {
                if (!string.IsNullOrEmpty(menuIcon))
                    html.AppendFormat("<li><a href='../{0}/{1}'><i class='{2}'></i><span>{3}</span></a></li>",
                        folderName, pageName, menuIcon, menuName);
                else
                    html.AppendFormat("<li><a href='../{0}/{1}'><span>{2}</span></a></li>",
                        folderName, pageName, menuName);
            }
        }
    }
    protected void btn_InTime_Click(object sender, EventArgs e)
    {
        //btn_InTime.Visible = true;
        //btn_OutTime.Visible = false;
        string str_intime = DateTime.UtcNow.ToString();


        string str_Sql = ("insert into IT_InOutTime(Employeekey,InTime,Createdby)values(@Employeekey,@InTime,@Createdby)");
        SqlCommand cmd = new SqlCommand(str_Sql);
        cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        cmd.Parameters.AddWithValue("@InTime", str_intime);

        cmd.Parameters.AddWithValue("@Createdby", str_userid);

        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/Timemonitoring.aspx");
    }
    protected void btn_OutTime_Click(object sender, EventArgs e)
    {

        //btn_InTime.Visible = false;
        //btn_OutTime.Visible = true;
        string str_outime = DateTime.UtcNow.ToString();


        string str_res = "update IT_InOutTime SET OutTime=@OutTime,Modifiedon=@Modifiedon,Modifiedby=@Modifiedby where Createdby=@Createdby and Createdon between @createdonfrom and @createdonto";

        SqlCommand cmd = new SqlCommand(str_res);
        cmd.Parameters.AddWithValue("@OutTime", str_outime);
        cmd.Parameters.AddWithValue("@Modifiedon", str_outime);
        cmd.Parameters.AddWithValue("@Modifiedby", str_userid);
        cmd.Parameters.AddWithValue("@Createdby", str_userid);
        cmd.Parameters.AddWithValue("@createdonfrom", this.str_CurrentDate);
        cmd.Parameters.AddWithValue("@createdonto", this.str_addoneday);
        DA.ExecuteNonQuery(cmd);
        this.Date();
        Response.Redirect("~/Employee/Timemonitoring.aspx");


    }
    private void Timegrid()
    {
        string str_visible = "select InTime,OutTime from IT_InOutTime where Createdby=@Createdby and Createdon between @createdonfrom and @createdonto";
        DateTime dateTimeutc = DateTime.UtcNow.Date;
        SqlCommand sc1 = new SqlCommand(str_visible);
        sc1.Parameters.AddWithValue("@Createdby", str_userid);
        sc1.Parameters.AddWithValue("@createdonfrom", this.str_CurrentDate);
        sc1.Parameters.AddWithValue("@createdonto", this.str_addoneday);
        DataTable dt_inout = DA.GetDataTable(sc1);

        //if (dt_inout.Rows.Count > 0)
        //{
        //    btn_InTime.Enabled = false;
        //    string str_stintime = dt_inout.Rows[0]["intime"].ToString();
        //    //DateTime.Parse(str_stintime).AddHours(+5.30);
        //    string str_stouttime = dt_inout.Rows[0]["outtime"].ToString();
        //    //DateTime.Parse(str_stouttime).AddHours(+5.30);
        //    if (str_stintime != null && str_stouttime == "")
        //    {
        //        btn_OutTime.Enabled = true;

        //    }
        //    else
        //    {
        //        btn_OutTime.Enabled = false;

        //    }
        //}
        //else
        //{
        //    intime.Visible = true;
        //}

    }
    private void Date()
    {
        string str_date = "select InTime,OutTime,inouttimekey from IT_InOutTime where Createdby=@Createdby and Createdon between @createdonfrom and @createdonto";
        SqlCommand cmd1 = new SqlCommand(str_date);
        cmd1.Parameters.AddWithValue("@Createdby", str_userid);
        cmd1.Parameters.AddWithValue("@createdonfrom", this.str_CurrentDate);
        cmd1.Parameters.AddWithValue("@createdonto", this.str_addoneday);
        DataTable dt_hour = DA.GetDataTable(cmd1);
        if (dt_hour.Rows.Count > 0)
        {
            this.str_key = dt_hour.Rows[0]["inouttimekey"].ToString();
            DateTime d1 = Convert.ToDateTime(dt_hour.Rows[0]["InTime"]);
            DateTime d2 = Convert.ToDateTime(dt_hour.Rows[0]["OutTime"]);
            TimeSpan TS = d2 - d1;
            int hour = TS.Hours;
            int mins = TS.Minutes;
            int secs = TS.Seconds;
            string timeDiff = hour.ToString("00") + ":" + mins.ToString("00") + ":" + secs.ToString("00");

            string str_time = "update IT_InOutTime SET workinghours=@workinghours where inouttimekey='" + this.str_key + "'";
            SqlCommand cmd2 = new SqlCommand(str_time);
            cmd2.Parameters.AddWithValue("@workinghours", timeDiff);
            //cmd2.Parameters.AddWithValue("@Employeekey",str_userky);
            DA.ExecuteNonQuery(cmd2);
        }
    }


}





















