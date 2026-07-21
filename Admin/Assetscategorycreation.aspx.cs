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

public partial class Admin_Assetscategorycreation : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Assets Category";
        if (!IsPostBack)
        {


            if (Request.QueryString["id"] != null)
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id))
                {
                    BindAssettype();
                    assignvalues(id);
                    create.InnerText = "Update Category";
                    submit.Text = "Update";
                }
            }
            else
            {
                BindAssettype();
                create.InnerText = "Create Category";
                submit.Text = "Create";
            }
        }

    }

    private void BindAssettype()
    {

        string str_sts = " SELECT AssetsTypeKey,AssetsType FROM IT_AssetsType ";
        {
            SqlCommand cmd = new SqlCommand(str_sts);
            DataSet reader = this.DA.GetDataSet(cmd);


            ddl_assettype.DataSource = reader;
            ddl_assettype.DataTextField = "AssetsType";
            ddl_assettype.DataValueField = "AssetsTypeKey";
            ddl_assettype.DataBind();
            ddl_assettype.Items.Insert(0, new ListItem("-- Select Asset Type --", ""));
        }
    }
    protected void btn_Create_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());
        if (Request.QueryString["id"] != null)
        {
            string updateQry = @"UPDATE IT_AssetsCategory SET Category=@Category,Type=@Type,Status=@Status,ModifiedBy=@ModifiedBy WHERE AssetsCategoryKey=@id";

            SqlCommand cmd = new SqlCommand(updateQry);
            cmd.Parameters.AddWithValue("@Type", ddl_assettype.SelectedValue);
            cmd.Parameters.AddWithValue("@Category", txt_Category.Text);
            cmd.Parameters.AddWithValue("@Status", rd_Status.SelectedValue);
            cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);
            cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;

            DA.ExecuteNonQuery(cmd);
            ScriptManager.RegisterStartupScript(
this,
this.GetType(),
"toastr_redirect",
"showToastr('success','Asset Category Update Successfully!');" +
"setTimeout(function(){ window.location.href = '/Admin/Assetscategory.aspx'; }, 2000);",
true
);
        }
        else
        {
            string insertQry = @"INSERT INTO IT_AssetsCategory (Category,Type,Status,CreatedBy) VALUES (@Category,@Type,@Status,@CreatedBy)";

            SqlCommand cmd = new SqlCommand(insertQry);
            cmd.Parameters.AddWithValue("@Type", ddl_assettype.SelectedValue);
            cmd.Parameters.AddWithValue("@Category", txt_Category.Text);
            cmd.Parameters.AddWithValue("@Status", rd_Status.SelectedValue);
            cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

            DA.ExecuteNonQuery(cmd);
            ScriptManager.RegisterStartupScript(
   this,
   this.GetType(),
   "toastr_redirect",
   "showToastr('success','Asset Category Created Successfully!');" +
   "setTimeout(function(){ window.location.href = '/Admin/Assetscategory.aspx'; }, 2000);",
   true
);
        }
    }

    public void assignvalues(int id)
    {
        string str_assing = "select * from IT_AssetsCategory where AssetsCategoryKey='" + id + "'";
        SqlCommand cmd = new SqlCommand(str_assing);
        cmd.Parameters.AddWithValue("LeadKey", this.str_id);
        DataTable dt_Category = this.DA.GetDataTable(cmd);
        if (dt_Category.Rows.Count > 0)
        {

            ddl_assettype.SelectedValue = dt_Category.Rows[0]["Type"].ToString();
            txt_Category.Text = dt_Category.Rows[0]["Category"].ToString();
            rd_Status.Text = dt_Category.Rows[0]["Status"].ToString();

        }
    }
}