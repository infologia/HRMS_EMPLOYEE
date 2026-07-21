using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_LeadsDetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_responsestatus = "";
    string str_userkey = "";
    string str_requestleave = "";
    string str_requestleave1 = "";
    private string key = "";
    string str_id = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Leads";

        // Always check if QueryString has id
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            this.str_id = Request.QueryString["id"].ToString();

            int viewId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Viewid"]))
            {
                string view_id = Request.QueryString["Viewid"].ToString();
                viewId = Convert.ToInt32(view_id);

            }
                if (!IsPostBack)
            {
                
                BindLeadType();
                BindStatus();
                assignvalues(); 
            }

            if (viewId == 0)
            {
                btn_update.Visible = false;
                btn_request.Visible = false;
            }
            else
            {
                btn_update.Visible = true;
                btn_request.Visible = false;
            }
          
        }
        else
        {
            if (!IsPostBack)
            {
                BindLeadType();
                BindStatus();
            }

            btn_request.Visible = true;
            btn_update.Visible = false;
        }
    }


    private void BindLeadType()
    {
        string str_lead = "select * from IT_LeadType";


        {
            SqlCommand cmd = new SqlCommand(str_lead);
            DataSet reader = this.DA.GetDataSet(cmd);


            ddl_leadtype.DataSource = reader;
            ddl_leadtype.DataTextField = "LeadType";  
            ddl_leadtype.DataValueField = "LeadTypeKey";  
            ddl_leadtype.DataBind();

            ddl_leadtype.Items.Insert(0, new ListItem("-- Select Lead Type --", ""));
        }
    }
    private void BindStatus()
    {

        string str_sts = "select * from IT_SalesStatus";
        {
            SqlCommand cmd = new SqlCommand(str_sts);
            DataSet reader = this.DA.GetDataSet(cmd);


            ddl_status.DataSource = reader;
            ddl_status.DataTextField = "SalesStatus";
            ddl_status.DataValueField = "SalesStatusKey";
            ddl_status.DataBind();

            ddl_status.Items.Insert(0, new ListItem("-- Select Status --", ""));
        }
    }

    protected void btn_request_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());

        SqlCommand cmd = new SqlCommand(@"INSERT INTO IT_Leads (Name,Company,Position,Email,Mobile,Source,Description,LeadType,Status,CreatedBy) VALUES (@Name,@Company,@Position,@Email,@Mobile,@Source,@Description,@LeadType,@Status,@CreatedBy)");           
        cmd.Parameters.AddWithValue("@Name", txt_name.Text.Trim());
        cmd.Parameters.AddWithValue("@Company", txt_company.Text.Trim());
        cmd.Parameters.AddWithValue("@Position", txt_position.Text.Trim());
        cmd.Parameters.AddWithValue("@Email", txt_email.Text.Trim());
        cmd.Parameters.AddWithValue("@Mobile", txt_mobile.Text.Trim());
        cmd.Parameters.AddWithValue("@Source", txt_source.Text.Trim());
        cmd.Parameters.AddWithValue("@Description", txt_description.Text.Trim());
        cmd.Parameters.AddWithValue("@LeadType", ddl_leadtype.SelectedValue);
        cmd.Parameters.AddWithValue("@Status", ddl_status.SelectedValue);
        cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

        DA.ExecuteNonQuery(cmd);
        Response.Redirect("SalesLeads.aspx");
        ClearForm();
    }




    private void ClearForm()
    {
        txt_name.Text = "";
        txt_company.Text = "";
        txt_position.Text = "";
        txt_email.Text = "";
        txt_mobile.Text = "";
        txt_source.Text = "";
        txt_description.Text = "";
        ddl_leadtype.SelectedIndex = 0;
        ddl_status.SelectedIndex = 0;
    }

    public void assignvalues()
    {
        string str_assing = "select * from IT_Leads a left outer join IT_SalesStatus b on a.Status=b.SalesStatusKey left outer join IT_LeadType c on a.LeadType=c.LeadTypeKey where a.LeadKey=@LeadKey";
        SqlCommand cmd = new SqlCommand(str_assing);
        cmd.Parameters.AddWithValue("LeadKey", this.str_id);
        DataTable dt_leadvalue = this.DA.GetDataTable(cmd);
        if (dt_leadvalue.Rows.Count > 0)
        {

            txt_name.Text = dt_leadvalue.Rows[0]["Name"].ToString();
            txt_company.Text = dt_leadvalue.Rows[0]["Company"].ToString();
            txt_position.Text = dt_leadvalue.Rows[0]["Position"].ToString();
            txt_mobile.Text = dt_leadvalue.Rows[0]["Mobile"].ToString();
            txt_email.Text = dt_leadvalue.Rows[0]["Email"].ToString();
            txt_description.Text = dt_leadvalue.Rows[0]["Description"].ToString();
            txt_source.Text = dt_leadvalue.Rows[0]["Source"].ToString();
            ddl_status.SelectedValue = dt_leadvalue.Rows[0]["Status"].ToString();
            ddl_leadtype.SelectedValue = dt_leadvalue.Rows[0]["LeadType"].ToString();
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());
        int leadKey;
        if (!int.TryParse(this.str_id, out leadKey))
        {
            return;
        }
        SqlCommand cmd = new SqlCommand(@"UPDATE IT_Leads SET Name=@Name,Company=@Company,Position=@Position,Email=@Email,Mobile=@Mobile,Source=@Source,Description=@Description,LeadType=@LeadType,Status=@Status, ModifiedBy=@ModifiedBy,ModifiedOn=GETDATE() WHERE LeadKey=@LeadKey ");
        cmd.Parameters.Add("@LeadKey", SqlDbType.Int).Value = leadKey;
        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = txt_name.Text.Trim();
        cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 100).Value = txt_company.Text.Trim();
        cmd.Parameters.Add("@Position", SqlDbType.NVarChar, 50).Value = txt_position.Text.Trim();
        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = txt_email.Text.Trim();
        cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar, 15).Value = txt_mobile.Text.Trim();
        cmd.Parameters.Add("@Source", SqlDbType.NVarChar, 50).Value = txt_source.Text.Trim();
        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = txt_description.Text.Trim();
        cmd.Parameters.Add("@LeadType", SqlDbType.Int).Value = Convert.ToInt32(ddl_leadtype.SelectedValue);
        cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Convert.ToInt32(ddl_status.SelectedValue);
        cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("SalesLeads.aspx");
    }
}
