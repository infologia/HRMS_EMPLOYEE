using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Assetscategory : System.Web.UI.Page
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
            control1.Text = "Assets Category";
        if (!Page.IsPostBack)
        {
            this.Assetsgrid();
        }
    }
    private void Assetsgrid()
    {
        string str_viewUser = @"select AssetsCategoryKey,b.AssetsType as Type,Category,CONVERT(VARCHAR(10), a.CreatedOn, 105) AS CreatedOn, CONVERT(VARCHAR(10), ModifiedOn, 105) AS ModifiedOn,Status from IT_AssetsCategory a left join IT_AssetsType b on a.Type= b.AssetsTypeKey";

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

            this.PH.LoadGridItem(ds, PH_category, "Assetscategory.txt", "");
        }
    }
}