using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_Suggestion : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Suggestion";
        }
        if (!IsPostBack)
        {
            this.loaddropdown();

        }
      str_userid = this.SC.Userid;
    }
    public void loaddropdown()
    {
        string str_URL = "select * from IT_SuggestionCategory";
        //string str_URL = "select * from IT_SuggestionCategory where SuggestionCategoryid not in(select SuggestionCategoryid from IT_SuggestionCategory where SuggestionCategoryid in (select Suggestioncategory from IT_Suggestion where createdby='" + SC.Userid + "' and suggestionstatus='1'))"; ;
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_category.DataSource = ds1.Tables[0];
            ddl_category.DataTextField = "SuggestionCategoryName";
            ddl_category.DataValueField = "SuggestionCategoryid";
          
            ddl_category.DataBind();
            ddl_category.Items.Add(new ListItem("Select Your Category", "0"));
            ddl_category.SelectedValue = "0";

        }
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {
            Random generator = new Random();
            int number = generator.Next(0000, 9999);

            string str_rmnum = "ITSugg_" + number;
            int status = 1;

            string date = DateTime.Now.ToString();
            string str_Sql = ("insert into IT_Suggestion(SuggestionCategory,Reason,Createdby,Employeekey,SuggestionId,Suggestionstatus)values(@SuggestionCategory,@Reason,@Createdby,@Employeekey,@SuggestionId,@Suggestionstatus)");
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@SuggestionCategory", ddl_category.SelectedValue);
            cmd.Parameters.AddWithValue("@Reason", txt_reason.InnerText.Trim());
            cmd.Parameters.AddWithValue("@Createdby",this.str_userid);
            cmd.Parameters.AddWithValue("@Employeekey", this.str_userid);
            cmd.Parameters.AddWithValue("@SuggestionId", str_rmnum);
            cmd.Parameters.AddWithValue("@Suggestionstatus", status);
           
            DA.ExecuteNonQuery(cmd);
            Response.Redirect(@"~/Employee/suggestionresponseview.aspx");
        }
        catch (Exception ex)
        {
        }
    }
}