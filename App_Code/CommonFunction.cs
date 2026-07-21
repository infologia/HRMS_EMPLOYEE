using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net.Mime;
using System.Activities;
using System.Web.Script.Serialization;
using System.Net;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Globalization;


/// <summary>
/// Summary description for CommonFunction
/// </summary>
public class CommonFunction
{
    DataAccess DA;
    PhTemplate Ph;
    public CommonFunction()
    {
        this.DA = new DataAccess();
        this.Ph = new PhTemplate();
    }

    public string PostData(string str_PostDataUrl, string str_PostData)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:4646/Forex/sys/formmail.ashx");
        request.Method = "POST";
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(str_PostData);
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteArray.Length;
        System.IO.Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        WebResponse response = request.GetResponse();
        dataStream = response.GetResponseStream();
        System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
        string responseFromServer = HttpUtility.UrlDecode(reader.ReadToEnd());
        reader.Close();
        dataStream.Close();
        response.Close();
        return responseFromServer;
    }



    public string PasswordRecovery(string str_email, string type, string str_subject, string str_link, string str_firstname)
    {
        string str_msg = "success";
        string str_template = "";

        if (type == "password")
        {

            str_template = "Passwordrecover.txt";
        }
        if (type == "Project")
        {

            str_template = "EmailForregistration.txt";
        }
        if (type == "Issue")
        {

            str_template = "EmailForIssue.txt";
        }
        if (type == "registration")
        {

            str_template = "AddMember.txt";
        }

        SmtpClient smtpClient = new SmtpClient();

        var smtp = new System.Net.Mail.SmtpClient();
        {
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
           smtp.Credentials = new NetworkCredential("infologiatechnologies@gmail.com", "mnoe yqed dvim ujns");
            smtp.Timeout = 20000;
        }
        MailMessage mail = new MailMessage();

        //Setting From , To and CC
        mail.From = new MailAddress("infologiatechnologies@gmail.com", "Infologia Technologies");
        mail.To.Add(new MailAddress(str_email));
        string subject = str_subject;
        string body = this.Ph.ReadFileToString(str_template);
        body = this.Ph.ReplaceVariableWithValueForEmailsend(str_firstname, str_link, body);
        mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));
        mail.IsBodyHtml = true;
        mail.Subject = subject;

        smtp.Send(mail);

        return str_msg;
    }

    public DateTime GetIndianDateTime()
    {
        TimeZoneInfo Indian_Zone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        DateTime dt_IndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Indian_Zone);
        return dt_IndianTime;
    }


    public string GetIPAddress()
    {
        try
        {
            string externalIP;
            externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
            externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                         .Matches(externalIP)[0].ToString();
            return externalIP;
        }
        catch { return null; }

    }
    public string SendMailToEmployee(string str_UserName, string str_Password, string str_FirstName)
    {
        string str_Response = "0";
        try
        {
            AppVar AV = new AppVar();
            string str_LoginURL = AV.WebRoot + "Web/Default.aspx";
            string str_EmailStatus = this.PostData(AV.WebRoot + "sys/formmail.ashx", "mailkey=3CC81FCE-D6B4-452E-A8F9-912AB6EDEFC7&email=" + str_UserName + "&p1=" + HttpContext.Current.Server.UrlEncode(HttpUtility.HtmlEncode(str_FirstName)) + "&p2=" + HttpContext.Current.Server.UrlEncode(HttpUtility.HtmlEncode(str_UserName)) + "&p3=" + HttpContext.Current.Server.UrlEncode(HttpUtility.HtmlEncode(str_Password)) + "&p4=" + HttpContext.Current.Server.UrlEncode(HttpUtility.HtmlEncode(str_LoginURL)));
            if (str_EmailStatus == "1") str_Response = "1";
            else str_Response = "0";

            return str_Response;
        }
        catch (Exception ex)
        {
            return str_Response;
        }

    }
    public void LoadMonthToDropdown(System.Web.UI.WebControls.DropDownList ddl_month)
    {
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("January", "1"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("February", "2"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("March", "3"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("April", "4"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("May", "5"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("June", "6"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("July", "7"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("August", "8"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("September", "9"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("October", "10"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("November", "11"));
        ddl_month.Items.Add(new System.Web.UI.WebControls.ListItem("December", "12"));
    }
    public void LoadDateToDropdown(System.Web.UI.WebControls.DropDownList ddl_date)
    {
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("1", "1"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("2", "2"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("3", "3"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("4", "4"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("5", "5"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("6", "6"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("7", "7"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("8", "8"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("8", "9"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("10", "10"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("11", "11"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("12", "12"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("13", "13"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("14", "14 "));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("15", "15"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("16", "16"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("17", "17"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("18", "18"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("20", "20"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("21", "21"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("22", "22"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("23", "23"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("24", "24"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("25", "25"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("26", "26"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("27", "27"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("28", "28"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("29", "29"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("30", "30"));
        ddl_date.Items.Add(new System.Web.UI.WebControls.ListItem("31", "31"));
    }

    public void LoadYearToDropdown(System.Web.UI.WebControls.DropDownList ddl_year)
    {

        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1975", "1975"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1976", "1976"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1977", "1977"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1978", "1978"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1979", "1979"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1980", "1980"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1981", "1981"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1982", "1982"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1983", "1983"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1984", "1984"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1985", "1985"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1986", "1986"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1987", "1987"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1988", "1988"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1989", "1989"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1990", "1990"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1991", "1991"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1992", "1992"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1993", "1993"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1994", "1994"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1995", "1995"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1996", "1996"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1997", "1997"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1998", "1998"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("1999", "1999"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2000", "2000"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2001", "2001"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2002", "2002"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2003", "2003"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2004", "2004"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2005", "2005"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2006", "2006"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2007", "2007"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2008", "2008"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2009", "2009"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2010", "2010"));
        //ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2011", "2011"));
       // ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2018", "2018"));
        ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2019", "2019"));
        ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2020", "2020"));
        ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2021", "2021"));
        ddl_year.Items.Add(new System.Web.UI.WebControls.ListItem("2022", "2012"));
    }

    public SqlCommand CreateLogKey(string str_UserKey)
    {
        DateTime now = DateTime.UtcNow;
        var timenow = now;
        // string utcnow = timenow;
        string str_LogKey = Guid.NewGuid().ToString();
        string str_Sql = "insert into  IT_Logdetail(LogKey, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy) values (@LogKey, @CreatedOn, getdate(), @CreatedBy, @ModifiedBy)";
        SqlCommand sc = new SqlCommand(str_Sql);
        sc.Parameters.AddWithValue("@LogKey", str_LogKey);
        sc.Parameters.AddWithValue("@CreatedBy", str_UserKey);
        sc.Parameters.AddWithValue("@ModifiedBy", str_UserKey);
        sc.Parameters.AddWithValue("@CreatedOn", now);
        return sc;
    }

    public string PasswordRecover(string str_email, string type, string str_head, string str_sub, string str_content, string str_img, string str_footer)
    {
        string str_msg = "success";
        string str_template = "";

        if (type == "password")
        {

            str_template = "Passwordrecover.txt";
        }
        if (type == "response")
        {

            str_template = "EmailForRegistration.txt";
        }
        SmtpClient smtpClient = new SmtpClient();

        var smtp = new System.Net.Mail.SmtpClient();
        {
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential("infologiatechnologies@gmail.com", "infologia1");
            smtp.Timeout = 20000;
        }
        MailMessage mail = new MailMessage();

        //Setting From , To and CC
        mail.From = new MailAddress("info@infologia.in", "Infologia Technologies");
        mail.To.Add(new MailAddress(str_email));
        string subject = str_sub;
        string body = this.Ph.ReadFileToString(str_template);

        //string path = .MapPath(@"Images/photo.jpg");
        //LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
        //Img.ContentId = "MyImage"; 


        body = this.Ph.ReplaceVariableWithValueForEmail(str_head, str_sub, str_content, str_img, str_footer, body);
        mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));
        mail.IsBodyHtml = true;
        mail.Subject = subject;

        smtp.Send(mail);

        return str_msg;
    }
    public string AssignTask(string str_email, string type, string str_head, string str_sub, string str_content, string str_td, string str_Admianname, string str_Projectcatcategory)
    {
        string str_msg = "success";
        string str_template = "";

        if (type == "password")
        {

            str_template = "Passwordrecover.txt";
        }
        if (type == "registration")
        {

            str_template = "Task.txt";
        }
        SmtpClient smtpClient = new SmtpClient();

        var smtp = new System.Net.Mail.SmtpClient();
        {
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential("infologiatechnologies@gmail.com", "infologia1");
            smtp.Timeout = 20000;
        }
        MailMessage mail = new MailMessage();

        //Setting From , To and CC
        mail.From = new MailAddress("info@infologia.in", "Infologia Technologies");
        mail.To.Add(new MailAddress(str_email));
        string subject = "Infologia Task Update";
        string body = this.Ph.ReadFileToString(str_template);
        //System.Net.Mail.Attachment attachment;
        //attachment = new System.Net.Mail.Attachment(str_attachment);
        //string str_attchment = attachment.ToString();
        body = this.Ph.ReplaceVariableWithValueForSendEmails(str_head, str_sub, str_content, str_td, body, str_Admianname, str_Projectcatcategory);
        mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));
        mail.IsBodyHtml = true;
        mail.Subject = subject;
        // mail.Attachments.Add(attachment);
        smtp.Send(mail);

        return str_msg;
    }

    public string assignemployee(string str_email, string type, string str_username, string str_projectname,string str_category)
    {
        string str_msg = "success";
        string str_template = "";

        if (type == "Employee")
        {

            str_template = "ProjectAssign.txt";
        }

        SmtpClient smtpClient = new SmtpClient();

        var smtp = new System.Net.Mail.SmtpClient();
        {
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential("infologiatechnologies@gmail.com", "infologia1");
            smtp.Timeout = 20000;
        }
        MailMessage mail = new MailMessage();

        //Setting From , To and CC
        mail.From = new MailAddress("info@infologia.in", "Infologia Technologies");
        mail.To.Add(new MailAddress(str_email));
        string subject = "Project Details";
        string body = this.Ph.ReadFileToString(str_template);
        body = this.Ph.mailtemplate(str_username, str_projectname,str_category,body);
        mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));
        mail.IsBodyHtml = true;
        mail.Subject = subject;

        smtp.Send(mail);

        return str_msg;
    }


    public DateTime currentdatetime(string str_utc)
    {


        DateTime dt_utc = Convert.ToDateTime(str_utc);

        CultureInfo ci = new CultureInfo("en-NZ");
        string date = dt_utc.ToString("R", ci);
        DateTime convertedDate = DateTime.Parse(date);     
        var Request = HttpContext.Current.Request;
        string final = Request.ToString();
        // string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //if (string.IsNullOrEmpty(ipAddress))
        //{
        //    ipAddress = Request.ServerVariables["REMOTE_ADDR"];
        //}
        //ipAddress = "13.126.196.138";
        //string APIKey = "327ae8f9e5a9e8340bbeebcbaa5637777cf712a69c74a817fc15dfd5c6285dbe";
        //string url = string.Format("http://api.ipinfodb.com/v3/ip-city/?key={0}&ip={1}&format=json", APIKey, ipAddress);
        //using (WebClient client = new WebClient())
        //{
        //    string json = client.DownloadString(url);
        //    Location location = new JavaScriptSerializer().Deserialize<Location>(json);
        //    List<Location> locations = new List<Location>();
        //    locations.Add(location);
        //    string str_timezone = location.TimeZone;
        //  //  DateTimeOffset result = DateTimeOffset.Parse(str_timezone, CultureInfo.InvariantCulture);
        //  //  DateTimeOffset result =DateTimeOffset.TryParse(str_timezone, out result)
        //  //  var offset = DateTimeOffset.ParseExact(offsetString);
        //    DateTimeOffset offset = dt_utc;
        //    if (!DateTimeOffset.TryParse(str_timezone, out offset))
        //    {
        //        offset = DateTimeOffset.Now;
        //    }

        //   // TimeSpan ts = dt_utc - offset;
        //    DateTime dt_final = offset.DateTime;
        return convertedDate;
        
    }



    public class Location
    {
        public string IPAddress { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TimeZone { get; set; }
    }

    public class Menu
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
    }
    public class SubMenu
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuUrl { get; set; }
    }
    public class ChildSubMenu
    {
        public int SubParentId { get; set; }
        public string ChildSubMenuName { get; set; }
        public string ChildSubMenuUrl { get; set; }
    }

}
