using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
public partial class Employee_ChatWindow : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    AppVar AP;
    LogWriter LW;
    string str_SectionDocumentUrl = new HttpLocate().WebRoot + "Uploads/";
    string str_userid = "";

    //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.AP = new AppVar();
        this.LW = new LogWriter();
        this.str_userid = SC.Userid.ToString();
        //if (!IsPostBack)
        //{
        //    Label control1 = this.Master.FindControl("lbl_bread") as Label;
        //    if (control1 != null)
        //        control1.Text = "Chat";
        //}

        string VisitorsIPAddr = string.Empty;
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        {
            VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        }
        XmlDocument doc = new XmlDocument();
        string getdetails = "";
        //string getdetails = "http://www.freegeoip.app/xml/" + VisitorsIPAddr;
        //doc.Load(getdetails);
        //XmlNodeList nodeLstCity = doc.GetElementsByTagName("City");
       // XmlNodeList nodeLstCountry = doc.GetElementsByTagName("CountryName");
       // lbl_time.Text = nodeLstCity[0].InnerText + "," + nodeLstCountry[0].InnerText;
        //LoadChatbox();
        get_User();
        Load_Frends();
    }
    public void LoadChatbox()
    {
        DateTime date = DateTime.Now.Date;

        string str = @"SELECT * FROM Chatbox 
                   WHERE 
                     ((Sender=@Sender1 AND Reciever=@Receiver1) 
                     OR 
                     (Sender=@Sender2 AND Reciever=@Receiver2))
                   AND CAST([date] AS DATE) = @ChatDate
                   ORDER BY ID";

        SqlCommand cmd = new SqlCommand(str);

        cmd.Parameters.AddWithValue("@Sender1", Label1.Text);
        cmd.Parameters.AddWithValue("@Receiver1", Label2.Text);
        cmd.Parameters.AddWithValue("@Sender2", Label2.Text);
        cmd.Parameters.AddWithValue("@Receiver2", Label1.Text);

        cmd.Parameters.AddWithValue("@ChatDate", date);

        DataSet ds = this.DA.GetDataSet(cmd);
        DataList3.DataSource = ds;
        DataList3.DataBind();
    }

    public void get_User()
    {
        Image1.ImageUrl = "~/Images/employeeprofilepictures/" + Session["image"].ToString();
        //Label1.Text = Session["Admin"].ToString();
        Label1.Text = this.SC.username;
    }
    protected void Unnamed_ServerClick(object sender, EventArgs e)
    {
        DateTime date = DateTime.Now;

        string date3 = date.ToString("dd-MM-yyyy");
        string time = date.ToString("HH:mm:ss");
        // conn.Open();
        string query = "insert into Chatbox(sender,reciever,message,date,time,image) values('" + Label1.Text + "','" + Label2.Text + "','" + TextBox1.Text + "','" + date + "','" + time + "','" + Image1.ImageUrl.ToString() + "')";
        SqlCommand cmd = new SqlCommand(query);
        int i = Convert.ToInt16(this.DA.ExecuteNonQuery(cmd));
        //conn.Close();
        if (i >= 1)
        {
            TextBox1.Text = "";
           
            LoadChatbox();
            Load_Frends();
        }
    }
    public void Load_Frends()
    {
      

        string str = "SELECT a.Username,a.Image,b.Sender, MAX(b.Lastchatdate) AS LastChatDat FROM IT_EmployeeRegister a LEFT JOIN Chatbox b ON a.Username = b.Sender WHERE a.Username <> '" + SC.username+ "' GROUP BY b.Lastchatdate,a.Username, a.Image, b.Sender ORDER BY b.LastChatDate DESC;";
        SqlCommand cmd = new SqlCommand(str);

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        ds = this.DA.GetDataSet(cmd);
        da.Fill(ds);
        //DataList2.DataSource = ds;
        //DataList2.DataBind();
        DataList1.DataSource = ds;
        DataList1.DataBind();
      
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        LinkButton lBtn = sender as LinkButton;
        string id = ((LinkButton)sender).CommandArgument.ToString();
        //Label1.Text = id;
        Label2.Text = lBtn.Text;

        DataListItem item = (DataListItem)lBtn.NamingContainer;
        Image NameLabel = (Image)item.FindControl("Image2");
        string url = NameLabel.ImageUrl.ToString();
        Image3.ImageUrl = url;
        LoadChatbox();
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        LoadChatbox();
    }
    protected void BtnBack_Click(object sender, EventArgs e)
{
    if (Request.UrlReferrer != null)
    {
            Response.Redirect("~/Employee/Timings.aspx");
        }
    else
    {
        
            Response.Redirect(Request.UrlReferrer.ToString());
    }

}

}