using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Xml;
using System.Text;

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
        this.str_userky = SC.Userid;

        if (int_role == 0)
        {
            div_intimeshow.Visible = false;
            div_outtimeshow.Visible = false;

        }
        //else
        //{
        //    Response.Redirect("~/logout.aspx");
        //}


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
            string menunamewhere = "5eacbe17-4793-4bf6-8a9c-62272215ff9a";
            string str_sql1 = ("select Menukey,Menuname,ParentmenuID,Pagename,Foldername,Menuicon,menudescription,Menulist FROM TT_Menus where parentmenuid='" + menunamewhere + "' and destinationkey='" + this.str_userdesgn + "'  order by menulist asc");
            DataTable oDataTable1 = DA.GetDataTable(str_sql1);
            string str_sql = ("select Menukey,Menuname,ParentmenuID,Pagename,Foldername,Menuicon,menudescription,Menulist FROM TT_Menus where  destinationkey='" + this.str_userdesgn + "' order by menulist asc");
            DataTable oDataTable = DA.GetDataTable(str_sql);
            //string str_parid = oDataTable.Rows[0]["ParentmenuID"].ToString();
            DataRow[] drParentMenu1 = oDataTable1.Select();
            DataRow[] drParentMenu = oDataTable.Select("ParentmenuID is null");
           
            
            var oStringBuilder = new StringBuilder();
            var oStringBuilder1 = new StringBuilder();
            string MenuList = GenerateMenu(drParentMenu, oDataTable, oStringBuilder);
            string MenuList1 = GenerateSingleMenu(drParentMenu1, oDataTable1, oStringBuilder1);
            Literal1.Text = MenuList1 +MenuList;
        }
    
       }



    private string GenerateMenu(DataRow[] drParentMenu, DataTable oDataTable, StringBuilder oStringBuilder)
    {
        oStringBuilder.AppendLine("<ul class='navigation navigation-main navigation-accordion'>"); if (drParentMenu.Length > 0)
        {
            foreach (DataRow dr in drParentMenu)
            {
                string MenuURL = dr["Pagename"].ToString();
                string MenuName = dr["Menuname"].ToString();
                string folder = dr["Foldername"].ToString();
                string micons = dr["Menuicon"].ToString();
                string title = dr["menudescription"].ToString();
                string line = String.Format(@"<li ><a href=""../{0}/{1}"" title=""{4}""><i class=""{2}""></i><span>{3}</span></a>", folder, MenuURL, micons, MenuName, title);
                oStringBuilder.Append(line);
                string MenuID = dr["Menukey"].ToString();
                string ParentID = dr["ParentmenuID"].ToString();
                DataRow[] subMenu = oDataTable.Select(String.Format("ParentmenuID = '" + MenuID + "'"));
                if (subMenu.Length > 0 && !MenuID.Equals(ParentID))
                {
                    var subMenuBuilder = new StringBuilder();
                    oStringBuilder.Append(GenerateMenu(subMenu, oDataTable, subMenuBuilder));
                } oStringBuilder.Append("</li>");
            }
        }
        oStringBuilder.Append("</ul>");
        return oStringBuilder.ToString();
    }


    private string GenerateSingleMenu(DataRow[] drParentMenu1, DataTable oDataTable1, StringBuilder oStringBuilder1)
    {
        oStringBuilder1.AppendLine("<ul class='navigation navigation-main navigation-accordion'>"); if (drParentMenu1.Length > 0)
        {
            foreach (DataRow dr in drParentMenu1)
            {
                string MenuURL1 = dr["Pagename"].ToString();
                string MenuName1 = dr["Menuname"].ToString();
                string folder1 = dr["Foldername"].ToString();
                string micons1 = dr["Menuicon"].ToString();
                string title1 = dr["menudescription"].ToString();
                string line1 = String.Format(@"<li ><a href=""../{0}/{1}"" title=""{4}""><i class=""{2}""></i><span>{3}</span></a>", folder1, MenuURL1, micons1, MenuName1, title1);
                oStringBuilder1.Append(line1);
                //string MenuID1 = dr["Menukey"].ToString();
                //string ParentID1 = dr["ParentmenuID"].ToString();
                //DataRow[] subMenu = oDataTable1.Select(String.Format("ParentmenuID = '" + MenuID1 + "'"));
                //if (subMenu.Length > 0 && !MenuID1.Equals(ParentID1))
                //{
                //    var subMenuBuilder = new StringBuilder();
                //    oStringBuilder1.Append(GenerateMenu(subMenu, oDataTable1, subMenuBuilder));
                //} oStringBuilder1.Append("</ul>");
            }
        }
        oStringBuilder1.Append("</li>");
        return oStringBuilder1.ToString();
    }

    protected void btn_InTime_Click(object sender, EventArgs e)
    {
        string str_intime = DateTime.UtcNow.ToString();
      

        string str_Sql = ("insert into IT_InOutTime(Employeekey,InTime,Createdby)values(@Employeekey,@InTime,@Createdby)");
        SqlCommand cmd = new SqlCommand(str_Sql);
        cmd.Parameters.AddWithValue("@Employeekey", str_userky);
        cmd.Parameters.AddWithValue("@InTime", str_intime);

        cmd.Parameters.AddWithValue("@Createdby", str_userky);

        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/Timemonitoring.aspx");
    }
    protected void btn_OutTime_Click(object sender, EventArgs e)
    {
        string str_outime = DateTime.UtcNow.ToString();
     

        string str_res = "update IT_InOutTime SET OutTime=@OutTime,Modifiedon=@Modifiedon,Modifiedby=@Modifiedby where Createdby=@Createdby and Createdon between @createdonfrom and @createdonto";

        SqlCommand cmd = new SqlCommand(str_res);
        cmd.Parameters.AddWithValue("@OutTime", str_outime);
        cmd.Parameters.AddWithValue("@Modifiedon", str_outime);
        cmd.Parameters.AddWithValue("@Modifiedby", str_userky);
        cmd.Parameters.AddWithValue("@Createdby", str_userky);
        cmd.Parameters.AddWithValue("@createdonfrom", this.str_CurrentDate);
        cmd.Parameters.AddWithValue("@createdonto", this.str_addoneday);
        DA.ExecuteNonQuery(cmd);
        this.Date();
        Response.Redirect("~/Employee/Timemonitoring.aspx");


    }
    private void Timegrid()
    {
        string str_visible = "select InTime,outtime from IT_InOutTime where Createdby=@Createdby and Createdon between @createdonfrom and @createdonto";
        DateTime dateTimeutc = DateTime.UtcNow.Date;
        SqlCommand sc1 = new SqlCommand(str_visible);
        sc1.Parameters.AddWithValue("@Createdby", str_userky);
        sc1.Parameters.AddWithValue("@createdonfrom", this.str_CurrentDate);
        sc1.Parameters.AddWithValue("@createdonto", this.str_addoneday);
        DataTable dt_inout = DA.GetDataTable(sc1);

        if (dt_inout.Rows.Count > 0)
        {
            btn_InTime.Enabled = false;
            string str_stintime = dt_inout.Rows[0]["intime"].ToString();
            DateTime.Parse(str_stintime).AddHours(+5.30);
            string str_stouttime = dt_inout.Rows[0]["outtime"].ToString();
            DateTime.Parse(str_stouttime).AddHours(+5.30);
            if (str_stintime != null && str_stouttime == "")
            {
                btn_OutTime.Enabled = true;

            }
            else
            {
                btn_OutTime.Enabled = false;

            }
        }
        else
        {
            intime.Visible = true;
        }

    }
    private void Date()
    {
        string str_date = "select InTime,outtime,inouttimekey from IT_InOutTime where Createdby=@Createdby and Createdon between @createdonfrom and @createdonto";
        SqlCommand cmd1 = new SqlCommand(str_date);
        cmd1.Parameters.AddWithValue("@Createdby", str_userky);
        cmd1.Parameters.AddWithValue("@createdonfrom", this.str_CurrentDate);
        cmd1.Parameters.AddWithValue("@createdonto", this.str_addoneday);
        DataTable dt_hour = DA.GetDataTable(cmd1);
        if (dt_hour.Rows.Count > 0)
        {
            this.str_key = dt_hour.Rows[0]["inouttimekey"].ToString();
            DateTime d1 = Convert.ToDateTime(dt_hour.Rows[0]["InTime"]);
            DateTime d2 = Convert.ToDateTime(dt_hour.Rows[0]["outtime"]);
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





















