using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class WEB_Employee_complaintresponseview : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_userid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        str_userid = this.SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Complaints";
        }

        string str_query = "select b.EmployeeId, CONCAT(b.FirstName, ' ', b.LastName) AS UserName, a.ComplaintCategory, a.ComplaintResponse, a.Reason, a.ComplaintKey, a.ComplaintId, a.ComplaintStatus, c.ComplaintCategoryName FROM IT_Complaint a LEFT JOIN IT_EmployeeRegister b ON a.CreatedBy = b.EmployeeKey LEFT JOIN IT_ComplaintCategory c ON a.ComplaintCategory = c.ComplaintCategoryId WHERE a.CreatedBy = @createdby ORDER BY a.CreatedOn ASC";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby", SC.Userid);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Complaintstatus"))
                ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("ViewText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String str_reason = dr["Complaintresponse"].ToString();
                int activetype = Convert.ToInt16(dr["Complaintstatus"].ToString());
                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                    dr["ViewText"] = "";
                }
                else if (activetype == 2)
                {
                    dr["ActiveText"] = "<span class='label label-sm label-success' title='" + str_reason + "'>Approved</span>";
                    dr["ViewText"] = "hidden";
                }
                else if (activetype == 3)
                {
                    dr["ActiveText"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";
                    dr["ViewText"] = "hidden";
                }
            }


            if (ds.Tables[0].Columns.Contains("complaintcategory"))
                ds.Tables[0].Columns.Add("ActiveCategory");
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr1["complaintcategory"].ToString());
                if (activetype == 1)
                    dr1["ActiveCategory"] = "<span class='label label-info'>Management</span>";
                else if (activetype == 2)
                    dr1["ActiveCategory"] = "<span class='label label-info'>Infrastructure</span>";
            }
            this.PH.LoadGridItem(ds, PH_Complaint, "Complaintview.txt", "");
        }
    }
    [WebMethod] //Delete
    public static string DeleteProject(string str_Complaintkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from IT_Complaint where ComplaintKey=@ComplaintKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@ComplaintKey", str_Complaintkey);
            DA1.ExecuteNonQuery(cmd);
            return str_Response = "1";
        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}