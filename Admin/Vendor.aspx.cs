using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Vendor : System.Web.UI.Page
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
                control1.Text = "Vendors";
        }


        string str_userid = this.SC.Userid;
        string str_query = "Select VendorKey,VendorCode,VendorName,ContactPerson,Status,CreatedOn from IT_Vendors";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby", str_userid);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Status"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Status"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Active</span>";
                else if (activetype == 0)
                    dr["ActiveText"] = "<span class='label label-sm label-danger'>InActive</span>";
            }

            this.PH.LoadGridItem(ds, PH_Vendor, "Vendor.txt", "");
        }
        else
            return;

    }

    [WebMethod] 
    public static string DeleteProject(string str_vendorkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "DELETE FROM IT_Vendors WHERE VendorKey=@VendorKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@VendorKey", str_vendorkey);

            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}