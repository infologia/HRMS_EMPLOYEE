using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Overallreport : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Overall Report Details";

            LoadOverallAssetsGrid();
        }
    }
    private void LoadOverallAssetsGrid()
    {
        string str_query = @"SELECT   a.AssetKey,a.AssetTag,c.Category,a.EquipmentName,a.Brand,a.Quantity,a.ModelSerialNumber,a.PlacedLocation,a.AssetCondition, a.PurchaseCost,CONVERT(VARCHAR(10), a.PurchaseDate, 105) AS PurchaseDate, CONVERT(VARCHAR(10), b.AssignedDate, 105) AS AssignedDate, CASE WHEN b.AssetKey IS NOT NULL AND b.Status = 1 THEN 1 ELSE 0 END AS AssignedStatus,d.Username FROM IT_Assets a left outer  join IT_AssignedAssets b ON a.AssetKey = b.AssetKey left outer join IT_assetscategory c on a.Category=c.AssetsCategoryKey    left outer join IT_EmployeeRegister d on d.Employeekey=b.EmployeeKey ORDER BY a.CreatedOn DESC";
        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt_assets = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_assets);

        if (dt_assets.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("AssignmentText"))
                ds.Tables[0].Columns.Add("AssignmentText");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int assignedStatus = Convert.ToInt32(dr["AssignedStatus"]);

                if (assignedStatus == 1)
                    dr["AssignmentText"] = "<span class='label label-success'>Assigned</span>";
                else
                    dr["AssignmentText"] = "<span class='label label-warning'>Unassigned</span>";
            }

            PH.LoadGridItem(ds, PH_OverallAssets, "OverallAssets.txt", "");
        }
    }

}