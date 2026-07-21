using System;
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AssignassetsCreation : System.Web.UI.Page
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
            control1.Text = "Assigned Assets";
        if (!IsPostBack)
        {


            if (Request.QueryString["id"] != null)
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id))
                {
                    BindEmployeeName();
                    BindAssettype();
                    create.InnerText = "Update Assigned Assets";
                    assignvalues(id);
                    submit.Text = "Update";

                }
            }
            else
            {
                create.InnerText = "Create Assigned Assets";
                submit.Text = "Create";
                BindEmployeeName();
                BindAssettype();
               
            }
        }
    }
    private void BindEmployeeName()
    {

        string str_sts = " SELECT Employeekey,CONCAT(Firstname, ' ', Lastname) AS FullName FROM IT_EmployeeRegister WHERE Employeestatus = 1 and Division in (1,2,3,17,18)";
        {
            SqlCommand cmd = new SqlCommand(str_sts);
            DataSet reader = this.DA.GetDataSet(cmd);


            ddl_employee.DataSource = reader;
            ddl_employee.DataTextField = "FullName";
            ddl_employee.DataValueField = "Employeekey";
            ddl_employee.DataBind();
            ddl_employee.Items.Insert(0, new ListItem("-- Select Employee Name --", ""));
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
        string query = @"
    SELECT 
        MIN(a.AssetKey) AS AssetKey,
        b.AssetsCategoryKey,
        b.Category
    FROM IT_Assets a
    INNER JOIN IT_AssetsCategory b 
        ON a.Category = b.AssetsCategoryKey
    WHERE b.Type = @AssetsTypeKey
      AND a.Status = 1
    GROUP BY b.AssetsCategoryKey, b.Category";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@AssetsTypeKey", assetTypeKey);

        DataSet ds = DA.GetDataSet(cmd);

        ddl_assecategory.DataSource = ds;
        ddl_assecategory.DataTextField = "Category";
        ddl_assecategory.DataValueField = "AssetKey";
        ddl_assecategory.DataBind();

        ddl_assecategory.Items.Insert(0, new ListItem("-- Select Asset Category --", ""));
    }

    private void BindEquipment(string assetKey)
    {
        string query = @"
    SELECT MIN(a.AssetKey) AS AssetKey, a.EquipmentName
    FROM IT_Assets a
    WHERE a.Status = 1
      AND a.Category = (
            SELECT Category 
            FROM IT_Assets 
            WHERE AssetKey = @AssetKey
      )
    GROUP BY a.EquipmentName";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@AssetKey", assetKey);

        DataSet ds = DA.GetDataSet(cmd);

        ddl_equipment.DataSource = ds;
        ddl_equipment.DataTextField = "EquipmentName";
        ddl_equipment.DataValueField = "AssetKey";
        ddl_equipment.DataBind();

        ddl_equipment.Items.Insert(0, new ListItem("-- Select Equipment --", ""));
    }

    private void Bindbrand(string equipmentAssetKey)
    {
        bool isEdit = Request.QueryString["id"] != null;
        int assignedId = 0;

        if (isEdit)
            int.TryParse(Request.QueryString["id"], out assignedId);

        string query = @"
    SELECT 
        MIN(A.AssetKey) AS BrandAssetKey,
        A.Brand,
        COUNT(*) AS BrandCount
    FROM IT_Assets A
    WHERE A.Status = 1
      AND A.EquipmentName = (
            SELECT EquipmentName 
            FROM IT_Assets 
            WHERE AssetKey = @EquipmentAssetKey
      )
      AND (
            A.AssetKey NOT IN (
                SELECT ModelSerialNumber 
                FROM IT_AssignedAssets
                WHERE ModelSerialNumber IS NOT NULL
            )
            OR A.AssetKey = (
                SELECT ModelSerialNumber 
                FROM IT_AssignedAssets 
                WHERE AssignedAssets = @AssignedId
            )
      )
    GROUP BY A.Brand"
        ;


        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EquipmentAssetKey", equipmentAssetKey);
        cmd.Parameters.AddWithValue("@AssignedId", assignedId);

        DataSet ds = DA.GetDataSet(cmd);

        ddl_brand.Items.Clear();
        ddl_brand.Items.Insert(0, new ListItem("-- Select Brand --", ""));

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            ddl_brand.Items.Add(
                new ListItem(
                    row["Brand"].ToString() + " (" + row["BrandCount"] + ")",
                    row["BrandAssetKey"].ToString()
                )
            );
        }
    }

 
    private void BindModalserialno(string brandAssetKey, string equipmentAssetKey)
    {
        bool isEdit = Request.QueryString["id"] != null;
        int assignedId = 0;

        if (isEdit)
            int.TryParse(Request.QueryString["id"], out assignedId);

        string query = @"
    SELECT A.AssetKey, A.ModelSerialNumber
    FROM IT_Assets A
    WHERE A.Brand = (
        SELECT Brand FROM IT_Assets WHERE AssetKey = @BrandAssetKey
    )
    AND A.EquipmentName = (
        SELECT EquipmentName FROM IT_Assets WHERE AssetKey = @EquipmentAssetKey
    )
    AND A.Status = 1
    AND (
        A.AssetKey NOT IN (
            SELECT ModelSerialNumber 
            FROM IT_AssignedAssets
            WHERE ModelSerialNumber IS NOT NULL
        )
        OR A.AssetKey = (
            SELECT ModelSerialNumber 
            FROM IT_AssignedAssets 
            WHERE AssignedAssets = @AssignedId
        )
    )";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@BrandAssetKey", brandAssetKey);
        cmd.Parameters.AddWithValue("@EquipmentAssetKey", equipmentAssetKey);
        cmd.Parameters.AddWithValue("@AssignedId", assignedId);

        DataSet ds = DA.GetDataSet(cmd);

        ddl_modal.DataSource = ds;
        ddl_modal.DataTextField = "ModelSerialNumber";
        ddl_modal.DataValueField = "AssetKey";
        ddl_modal.DataBind();

        ddl_modal.Items.Insert(0, new ListItem("-- Select Model --", ""));
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





    protected void btn_Create_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());

        if (Request.QueryString["id"] != null)
        {
           

            string updateQry = @"UPDATE IT_AssignedAssets SET EmployeeKey=@EmployeeKey,AssetKey=@AssetKey,EquipmentName=@EquipmentName,ModelSerialNumber=@ModelSerialNumber, Description=@Description,AssignedDate=@AssignedDate,ReturnedDate=@ReturnedDate,Status=@Status,ModifiedBy=@ModifiedBy, ModifiedOn = GETDATE(),AssetType=@AssetType,Brand=@Brand WHERE AssignedAssets=@id";

            SqlCommand cmd = new SqlCommand(updateQry);
            cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);
            cmd.Parameters.AddWithValue("@AssetType", ddl_assettype.SelectedValue);
            cmd.Parameters.AddWithValue("@EmployeeKey", ddl_employee.SelectedValue);
            cmd.Parameters.AddWithValue("@AssetKey", ddl_assecategory.SelectedValue);
            cmd.Parameters.AddWithValue("@EquipmentName", ddl_equipment.SelectedValue);
            cmd.Parameters.AddWithValue("@Brand", ddl_brand.SelectedValue);
            cmd.Parameters.AddWithValue("@ModelSerialNumber", ddl_modal.SelectedValue);
            cmd.Parameters.AddWithValue("@Description", txt_description.Text);
            cmd.Parameters.AddWithValue("@AssignedDate",
        ParseDateOrDBNull(txt_assigneddate.Text));

            cmd.Parameters.AddWithValue("@ReturnedDate",
                ParseDateOrDBNull(txt_returneddate.Text));
            cmd.Parameters.AddWithValue("@Status", rd_Status.Text);
            cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;

            if (!string.IsNullOrEmpty(txt_returneddate.Text))
            {
                DateTime assignDate = DateTime.ParseExact(txt_assigneddate.Text, "dd/MM/yyyy", null);
                DateTime returnDate = DateTime.ParseExact(txt_returneddate.Text, "dd/MM/yyyy", null);

                if (returnDate <= assignDate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "err",
                        "<script>alert('Returned Date must be greater than Assigned Date');</script>");
                    return;
                }
            }

            DA.ExecuteNonQuery(cmd);
            ScriptManager.RegisterStartupScript(
  this,
  this.GetType(),
  "toastr_redirect",
  "showToastr('success','Assigned Asset Updated Successfully!');" +
  "setTimeout(function(){ window.location.href = '/Admin/Assignassets.aspx'; }, 2000);",
  true
);
        }
        else
        {
            string insertQry = @"INSERT INTO IT_AssignedAssets (EmployeeKey,AssetKey,EquipmentName,ModelSerialNumber,Description,AssignedDate,ReturnedDate,Status,CreatedBy,AssetType,Brand) VALUES (@EmployeeKey,@AssetKey,@EquipmentName,@ModelSerialNumber,@Description,@AssignedDate,@ReturnedDate,@Status,@CreatedBy,@AssetType,@Brand)";

            SqlCommand cmd = new SqlCommand(insertQry);
            cmd.Parameters.AddWithValue("@EmployeeKey", ddl_employee.SelectedValue);
            cmd.Parameters.AddWithValue("@AssetType", ddl_assettype.SelectedValue);
            cmd.Parameters.AddWithValue("@AssetKey", ddl_equipment.SelectedValue);
            cmd.Parameters.AddWithValue("@EquipmentName", ddl_equipment.SelectedValue);
            cmd.Parameters.AddWithValue("@Brand", ddl_brand.SelectedValue);
            cmd.Parameters.AddWithValue("@ModelSerialNumber", ddl_modal.SelectedValue);
            cmd.Parameters.AddWithValue("@Description", txt_description.Text);
            cmd.Parameters.AddWithValue("@AssignedDate",
        ParseDateOrDBNull(txt_assigneddate.Text));

            cmd.Parameters.AddWithValue("@ReturnedDate",
                ParseDateOrDBNull(txt_returneddate.Text));
            cmd.Parameters.AddWithValue("@Status", rd_Status.Text);
            cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

            DA.ExecuteNonQuery(cmd);
            ScriptManager.RegisterStartupScript(
   this,
   this.GetType(),
   "toastr_redirect",
   "showToastr('success','Asset Assigned Successfully!');" +
   "setTimeout(function(){ window.location.href = '/Admin/Assignassets.aspx'; }, 2000);",
   true
);
        }
    }
    public void assignvalues(int id)
    {
        SqlCommand cmd = new SqlCommand(
            "select AssignedAssets,AssetKey,EmployeeKey,Description,CONVERT(varchar(10), AssignedDate, 103) AS AssignedDate ,CONVERT(varchar(10), ReturnedDate, 103)  AS ReturnedDate,Status,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,EquipmentName,ModelSerialNumber,AssetType,Brand,Category from IT_AssignedAssets WHERE AssignedAssets=@id");
        cmd.Parameters.AddWithValue("@id", id);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count == 0) return;

        DataRow dr = dt.Rows[0];


        ddl_employee.SelectedValue = dr["EmployeeKey"].ToString();

        ddl_assettype.SelectedValue = dr["AssetType"].ToString();
        BindAssetCategory(ddl_assettype.SelectedValue);

        ddl_assecategory.SelectedValue = dr["AssetKey"].ToString();
        BindEquipment(ddl_assecategory.SelectedValue);

        ddl_equipment.SelectedValue = dr["EquipmentName"].ToString();
        Bindbrand(ddl_equipment.SelectedValue);

        ddl_brand.SelectedValue = dr["Brand"].ToString();
        BindModalserialno(ddl_brand.SelectedValue, ddl_equipment.SelectedValue);
        // BindModalserialno(ddl_brand.SelectedValue);

        ddl_modal.SelectedValue = dr["ModelSerialNumber"].ToString();

        txt_description.Text = dr["Description"].ToString();
        rd_Status.Text = dr["Status"].ToString();

        txt_assigneddate.Text = dr["AssignedDate"].ToString();

        txt_returneddate.Text = dr["ReturnedDate"].ToString();
    }



    protected void ddl_assettype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddl_assettype.SelectedValue))
        {
            BindAssetCategory(ddl_assettype.SelectedValue);
        }
        else
        {
            ddl_assecategory.Items.Clear();
            ddl_assecategory.Items.Insert(0, new ListItem("-- Select Asset Category --", ""));
        }
    }

    protected void ddl_assecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindEquipment(ddl_assecategory.SelectedValue);
    }




    protected void ddl_equipment_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bindbrand(ddl_equipment.SelectedValue);
    }

    protected void ddl_brand_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindModalserialno(ddl_brand.SelectedValue, ddl_equipment.SelectedValue);

    }


}