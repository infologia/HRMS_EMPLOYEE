using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Latepermissionrequestview : System.Web.UI.Page
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
                control1.Text = "Late Permissions";
        }

        string str_query = @"
            SELECT 
                b.firstname + '' + b.lastname AS name,
                CONVERT(varchar, a.Requestdate, 103) AS request,
                a.Fromtime,
                a.Totime,
                a.responsereason,
                a.LatePermissionDetailskey,
                a.Permissionhourse,
                a.responsestatus
            FROM IT_LatePermissionDetails a
            LEFT OUTER JOIN IT_EmployeeRegister b 
                ON a.createdby = b.Employeekey
            WHERE a.createdby = @createdby
            ORDER BY a.createdon ASC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby", SC.Userid);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("ActiveText"))
                ds.Tables[0].Columns.Add("ActiveText");

            if (!ds.Tables[0].Columns.Contains("ViewText"))
                ds.Tables[0].Columns.Add("ViewText");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string str_reason = dr["responsereason"] != DBNull.Value ? dr["responsereason"].ToString() : "";
                string statusValue = dr["responsestatus"] != DBNull.Value ? dr["responsestatus"].ToString().Trim() : "";

                int activetype = 0; 

                bool isNumeric = int.TryParse(statusValue, out activetype);

                if (!isNumeric)
                {
                    string statusLower = statusValue.ToLower();

                    if (statusLower == "pending")
                        activetype = 1;
                    else if (statusLower == "approved")
                        activetype = 2;
                    else if (statusLower == "rejected")
                        activetype = 3;
                    else
                        activetype = 0; 
                }

                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                    dr["ViewText"] = "";
                }
                else if (activetype == 2)
                {
                    dr["ActiveText"] = "<span class='label label-success' title='" + str_reason + "'>Approved</span>";
                    dr["ViewText"] = "hidden";
                }
                else if (activetype == 3)
                {
                    dr["ActiveText"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";
                    dr["ViewText"] = "hidden";
                }
                else
                {
                    dr["ActiveText"] = "<span class='label label-default' title='" + str_reason + "'>Unknown</span>";
                    dr["ViewText"] = "";
                }
            }


            this.PH.LoadGridItem(ds, PH_Permission, "Laterecordviewemp.txt", "");
        }
    }

    [WebMethod]
    public static string DeleteProject(string str_employeepermissiondetailskey)
    {
        try
        {
            DataAccess DA1 = new DataAccess();

            string str_Sql = "DELETE FROM IT_LatePermissionDetails WHERE LatePermissionDetailskey = @LatePermissionDetailskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@LatePermissionDetailskey", str_employeepermissiondetailskey);

            DA1.ExecuteNonQuery(cmd);

            return "1";
        }
        catch
        {
            return "0";
        }
    }

}
