using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Admin_AssetInventory : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();
        this.str_userkey = SC.Userid;
        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Assets Management";
        if (Request.QueryString["id"] != null)
        {
            int id;
            if (int.TryParse(Request.QueryString["id"], out id))
            {
                if (!Page.IsPostBack)
                {
                    div_status.Visible = true;
                    BindAssettype();
                    //this.BindAssetCategory();
                    PopulateFormDataForUpdate(id);
                    btn_Submit.Text = "Update";
                    create.Text = "Update Asset Inventory";
                }
            }
        }
        else
        {
            if (!Page.IsPostBack)
            {
                div_status.Visible = false;
                BindAssettype();
                // this.BindAssetCategory();
               // ddl_assetcategory.Items.Insert(0, new ListItem("-- Select Asset Category --", ""));
                btn_Submit.Text = "Submit";
                create.Text = "Create Asset Inventory";
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

    private void BindAssetCategory(string assetTypeKey)
    {
        string str_assetcategory = "select AssetsCategoryKey,Category from IT_AssetsCategory where Type= @AssetsTypeKey and Status =1";
        {
            SqlCommand cmd = new SqlCommand(str_assetcategory);
            cmd.Parameters.AddWithValue("@AssetsTypeKey", assetTypeKey);
            DataSet reader = this.DA.GetDataSet(cmd);
            ddl_assetcategory.DataSource = reader;
            ddl_assetcategory.DataTextField = "Category";
            ddl_assetcategory.DataValueField = "AssetsCategoryKey";
            ddl_assetcategory.DataBind();
            ddl_assetcategory.Items.Insert(0, new ListItem("-- Select Asset Category --", ""));
        }
    }
    private void PopulateFormDataForUpdate(int id)
    {
        div_status.Visible = true;
        string query = "select  AssetTag,EquipmentName,FileUpload,Brand,ModelSerialNumber,PlacedLocation,AssetCondition,PurchaseCost, CONVERT(varchar(10), PurchaseDate, 103)  AS PurchaseDate,Status,AMCDetails,Category,AssetType from IT_Assets where AssetKey='" + id + "'";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ddl_assettype.SelectedValue = dr["AssetType"].ToString();
            BindAssetCategory(dr["AssetType"].ToString());
            txt_assettag.Text = dr["AssetTag"].ToString();
            ddl_assetcategory.SelectedValue = dr["Category"].ToString();
            txt_equipmentname.Text = dr["EquipmentName"].ToString();
            txt_brand.Text = dr["Brand"].ToString();
            txt_modelserialno.Text = dr["ModelSerialNumber"].ToString();
            txt_placedlocation.Text = dr["PlacedLocation"].ToString();
            txt_assetcondition.Text = dr["AssetCondition"].ToString();
            txt_purchasedcost.Text = dr["PurchaseCost"].ToString();
            txt_purchaseddate.Text = dr["PurchaseDate"].ToString();
            txt_amcdetails.Text = dr["AMCDetails"].ToString();
            rd_Status.SelectedValue = dr["Status"].ToString();
            if (dr["FileUpload"] != DBNull.Value && !string.IsNullOrEmpty(dr["FileUpload"].ToString()))
            {
                string fileName = dr["FileUpload"].ToString();
                string virtualPath = "~/Document/Uploads/assetsdoc/" + fileName;

                lnkViewFile.NavigateUrl = ResolveUrl(virtualPath);
                lnkViewFile.Visible = true;
            }
            else
            {
                lnkViewFile.Visible = false;
            }
        }

    }


    private object ParseDateOrDBNull(string dateText)
    {
        if (string.IsNullOrWhiteSpace(dateText))
            return DBNull.Value;

        DateTime parsedDate;
        if (DateTime.TryParseExact(
            dateText,
            "dd/MM/yyyy",                
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out parsedDate))
        {
            return parsedDate;
        }

        return DBNull.Value;
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        SqlCommand cmd;
        string query;
        bool isEdit = Request.QueryString["id"] != null;
        int id = Convert.ToInt32(Request.QueryString["id"]);
        string fileName = "";
        if (fu_file.HasFile)
        {
            string ext = Path.GetExtension(fu_file.FileName);
            fileName = Guid.NewGuid().ToString() + ext;
            string path = Server.MapPath("~/Document/Uploads/assetsdoc/");
            fu_file.SaveAs(path + fileName);
        }

        if (isEdit)
        {
            id = Convert.ToInt32(Request.QueryString["id"]);
            query = @"UPDATE IT_Assets SET AssetTag = @AssetTag,FileUpload=@FileUpload, Category = @Category, EquipmentName = @EquipmentName, Brand = @Brand, ModelSerialNumber = @ModelSerialNumber, PlacedLocation = @PlacedLocation, AssetCondition = @AssetCondition, PurchaseCost = @PurchaseCost, PurchaseDate = @PurchaseDate, AMCDetails = @AMCDetails, Status = @Status, ModifiedOn = GETDATE(), ModifiedBy = @ModifiedBy,AssetType=@AssetType WHERE AssetKey = '" + id + "';";
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@ModifiedBy", str_userkey);
        }
        else
        {
            query = @"INSERT INTO IT_Assets ( AssetTag,FileUpload, Category, EquipmentName, Brand, ModelSerialNumber, PlacedLocation, AssetCondition, PurchaseCost, PurchaseDate, AMCDetails, Status, CreatedBy,AssetType ) VALUES ( @AssetTag,@FileUpload, @Category, @EquipmentName, @Brand, @ModelSerialNumber, @PlacedLocation, @AssetCondition, @PurchaseCost, @PurchaseDate, @AMCDetails, @Status, @CreatedBy,@AssetType );;";
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@CreatedBy", str_userkey);
        }
        cmd.Parameters.AddWithValue("@AssetType", ddl_assettype.SelectedValue);
        cmd.Parameters.AddWithValue("@AssetTag", txt_assettag.Text.Trim());
        cmd.Parameters.AddWithValue("@Category", ddl_assetcategory.SelectedValue);
        cmd.Parameters.AddWithValue("@EquipmentName", txt_equipmentname.Text);
        //cmd.Parameters.AddWithValue("@Quantity", txt_quantity.Text);
        cmd.Parameters.AddWithValue("@FileUpload", fileName);
        cmd.Parameters.AddWithValue("@Brand", txt_brand.Text);
        cmd.Parameters.AddWithValue("@ModelSerialNumber", txt_modelserialno.Text);
        cmd.Parameters.AddWithValue("@PlacedLocation", txt_placedlocation.Text);
        cmd.Parameters.AddWithValue("@AssetCondition", txt_assetcondition.Text);
        cmd.Parameters.AddWithValue("@PurchaseCost", txt_purchasedcost.Text);

        cmd.Parameters.AddWithValue("@PurchaseDate", ParseDateOrDBNull(txt_purchaseddate.Text));

       
        cmd.Parameters.AddWithValue("@AMCDetails", txt_amcdetails.Text);
        cmd.Parameters.AddWithValue("@Status", rd_Status.SelectedValue);
        DA.ExecuteNonQuery(cmd);
        
        string successMsg = isEdit ? "Asset Inventory Updated Successfully!" : "Asset Inventory Created Successfully!";
        
        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_redirect",
            "showToastr('success','" + successMsg + "');" +
            "setTimeout(function(){ window.location.href = 'AssetInventoryView.aspx'; }, 2000);",
            true
        );
    }

    protected void ddl_assettype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddl_assettype.SelectedValue))
        {
            BindAssetCategory(ddl_assettype.SelectedValue);
        }
        else
        {
            ddl_assetcategory.Items.Clear();
            ddl_assetcategory.Items.Insert(0, new ListItem("-- Select Asset Category --", ""));
        }
    }
}