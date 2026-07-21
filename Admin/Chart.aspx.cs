using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;

public partial class Admin_Chart : System.Web.UI.Page
{
    string str_pjkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["id"] == "" || Request.QueryString["id"] == null)
        {
            return;
        }
        else
        {

            this.str_pjkey = Request.QueryString["id"].ToString();
            hn_pjkey.Value = this.str_pjkey;
        }


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Document & Maintanence";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }

    }


    [WebMethod]
    public static List<object> GetChartData(string str_PjtCategorykey)
    {
            List<object> chartData = new List<object>();
            
        try
        {
            AppVar AP = new AppVar();
            string constr = AP.DatabaseConnectionString;
            string query = "SELECT   count(status)as status,'TaskStatus'= CASE WHEN status =  1 THEN 'pending' WHEN status =2 THEN 'Inprogress' ELSE 'Done'END FROM TT_createtask where projectkey='" + str_PjtCategorykey + "'  group BY status";
             chartData = new List<object>();
            chartData.Add(new object[]
    {
        "TaskStatus", "status"
    });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                    {
                        sdr["TaskStatus"], sdr["status"]
                    });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }
        catch(Exception ex)
        {
            return chartData;
        }
    }




}