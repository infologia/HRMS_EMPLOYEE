using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
public partial class Web_AssetInventoryView : System.Web.UI.Page
{
    DataAccess DA;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.PH = new PhTemplate();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Assets Management";

            ddl_Month.SelectedValue = DateTime.Now.Month.ToString();
            ddl_Year.SelectedValue = DateTime.Now.Year.ToString();
        }
        this.Assetsgrid();
    }
    private void Assetsgrid()
    {
        string str_query = "select  a.AssetKey,a.AssetTag,a.EquipmentName,a.Quantity,a.Brand,a.ModelSerialNumber,a.PlacedLocation,a.AssetCondition,a.PurchaseCost,CONVERT(VARCHAR(10), a.PurchaseDate, 105) AS PurchaseDate,a.CreatedOn,a.Status,b.StatusName,c.Category from IT_Assets a left outer join IT_Status b on a.Status=b.StatusKey left outer join IT_AssetsCategory c on c.AssetsCategoryKey=a.Category WHERE 1=1";
        
        SqlCommand cmd = new SqlCommand();

        if (!string.IsNullOrEmpty(ddl_Month.SelectedValue))
        {
            str_query += " AND MONTH(a.PurchaseDate) = @Month";
            cmd.Parameters.AddWithValue("@Month", ddl_Month.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddl_Year.SelectedValue))
        {
            str_query += " AND YEAR(a.PurchaseDate) = @Year";
            cmd.Parameters.AddWithValue("@Year", ddl_Year.SelectedValue);
        }

        cmd.CommandText = str_query;
        DataTable dt_asset = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_asset);

        PH_assetinventoryview.Controls.Clear();
        if (dt_asset.Rows.Count > 0)
        {
            // Column add panna vendiyadhu FIRST
            if (!ds.Tables[0].Columns.Contains("ActiveText"))
                ds.Tables[0].Columns.Add("ActiveText", typeof(string));

            if (!ds.Tables[0].Columns.Contains("StatusOrder"))
                ds.Tables[0].Columns.Add("StatusOrder", typeof(int));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Status"]);

                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Active</span>";
                    dr["StatusOrder"] = 1;   // Active first
                }
                else
                {
                    dr["ActiveText"] = "<span class='label label-danger'>In Active</span>";
                    dr["StatusOrder"] = 2;   // Inactive next
                }
            }


            this.PH.LoadGridItem(ds, PH_assetinventoryview, "Assetinventoryview.txt", "");
        }
    }

    protected void ddl_Filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Assetsgrid();
    }
}