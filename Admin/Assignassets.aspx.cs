using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Assignassets : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Assets Management";
        if (!Page.IsPostBack)
        {
            this.Assetsgrid();
        }
    }
    private void Assetsgrid()
    {
        string str_viewUser = @"SELECT a.AssignedAssets,CONCAT(b.Firstname, ' ',b.Lastname) AS EmployeeKey,d.Brand as Brand, c.EquipmentName as EquipmentName, e.ModelSerialNumber as ModelSerialNumber, CONVERT(VARCHAR(10), a.AssignedDate, 105) AS AssignedDate, a.Status FROM IT_AssignedAssets a LEFT JOIN IT_EmployeeRegister b ON a.EmployeeKey = b.Employeekey LEFT JOIN IT_Assets e on a.ModelSerialNumber = e.AssetKey left join IT_Assets d on a.Brand = d.AssetKey left join IT_Assets c on a.AssetKey=c.AssetKey";

        SqlCommand cmd = new SqlCommand(str_viewUser);
        DataTable dt = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt);

        if (dt.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("ActiveText"))
                ds.Tables[0].Columns.Add("ActiveText");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Status"]);
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-sm label-success' >Active</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-danger' >In Active</span>";
            }
            this.PH.LoadGridItem(ds, PH_assests, "Assignedassets.txt", "");
        }
    }
}