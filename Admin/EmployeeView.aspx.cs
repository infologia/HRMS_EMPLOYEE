using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class WEB_EmployeeView : System.Web.UI.Page
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
                control1.Text = "Employee Monitoring";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }

        String str_query = "Select a.Employeekey,a.Image,a.Username,a.Firstname+' '+a.Lastname as Name,ISNULL(desig.Departmentname, a.Designation) as Designation,a.Email,a.Phonenumber,a.EmployeeStatus,a.Gender,a.createdon,d.Divisionname,b.RoleName,c.Departmentname from IT_EmployeeRegister a left outer join IT_Roles b on a.Role=b.RoleId left outer join IT_Department c on a.Department=c.Departmentid left outer join IT_Division d on a.Division=d.Divisionid left outer join IT_Department desig on CAST(a.Designation as varchar) = CAST(desig.Departmentid as varchar) where a.roles in (0,1,2) order by a.createdon";
        SqlCommand cmd = new SqlCommand(str_query);
       
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            // Bind Widget Counts
            lblTotalCount.Text = dt_dashboard.Rows.Count.ToString();
            
            int activeCount = 0;
            int inactiveCount = 0;
            int maleCount = 0;
            int femaleCount = 0;
            int incompleteCount = 0;

            foreach (DataRow dr in dt_dashboard.Rows)
            {
                if (dr["EmployeeStatus"] != DBNull.Value && dr["EmployeeStatus"].ToString() == "1")
                    activeCount++;
                else
                    inactiveCount++;

                int gender = dr["Gender"] != DBNull.Value && !string.IsNullOrEmpty(dr["Gender"].ToString()) ? Convert.ToInt16(dr["Gender"]) : -1;
                if (gender == 1) femaleCount++;
                else if (gender == 0) maleCount++;

                bool isComplete = true;
                if (dr["Image"] == DBNull.Value || string.IsNullOrEmpty(dr["Image"].ToString()) || dr["Image"].ToString() == "../MEN.png") isComplete = false;
                if (dr["Phonenumber"] == DBNull.Value || string.IsNullOrEmpty(dr["Phonenumber"].ToString())) isComplete = false;
                if (dr["Designation"] == DBNull.Value || string.IsNullOrEmpty(dr["Designation"].ToString())) isComplete = false;

                if (!isComplete) incompleteCount++;
            }

            lblActiveCount.Text = activeCount.ToString();
            lblInactiveCount.Text = inactiveCount.ToString();
            lblMaleCount.Text = maleCount.ToString();
            lblFemaleCount.Text = femaleCount.ToString();
            lblIncompleteCount.Text = incompleteCount.ToString();

             if (ds.Tables[0].Columns.Contains("Gender"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = dr["Gender"] != DBNull.Value && !string.IsNullOrEmpty(dr["Gender"].ToString()) ? Convert.ToInt16(dr["Gender"]) : -1;
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='tag label bg-pink-400'>Female</span>";
                else if (activetype == 0)
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Male</span>";
            }
            
            if (ds.Tables[0].Columns.Contains("EmployeeStatus"))
                ds.Tables[0].Columns.Add("ActiveText1");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = dr["EmployeeStatus"] != DBNull.Value && !string.IsNullOrEmpty(dr["EmployeeStatus"].ToString()) ? Convert.ToInt16(dr["EmployeeStatus"]) : -1;
                if (activetype == 1)
                    dr["ActiveText1"] = "<span class='label label-sm label-success'>Active</span>";
                else if (activetype == 0)
                    dr["ActiveText1"] = "<span class='label label-sm label-danger'>InActive</span>";

                // Handle missing image
                if (dr["Image"] == DBNull.Value || string.IsNullOrEmpty(dr["Image"].ToString()))
                {
                    dr["Image"] = "../MEN.png";
                }

                // Handle missing Designation
                if (ds.Tables[0].Columns.Contains("Designation"))
                {
                    ds.Tables[0].Columns["Designation"].ReadOnly = false;
                }
                
                if (dr["Designation"] == DBNull.Value || string.IsNullOrEmpty(dr["Designation"].ToString().Trim()))
                {
                    dr["Designation"] = "N/A";
                }
            }

            // Split into Active and Inactive
            DataView dvActive = new DataView(ds.Tables[0]);
            dvActive.RowFilter = "EmployeeStatus = 1";
            DataSet dsActive = new DataSet();
            dsActive.Tables.Add(dvActive.ToTable());
            this.PH.LoadGridItem(dsActive, PH_ActiveEmployee, "EmployeeView.txt", "");

            DataView dvInactive = new DataView(ds.Tables[0]);
            dvInactive.RowFilter = "EmployeeStatus = 0";
            DataSet dsInactive = new DataSet();
            dsInactive.Tables.Add(dvInactive.ToTable());
            this.PH.LoadGridItem(dsInactive, PH_InactiveEmployee, "EmployeeView.txt", "");
        }

    }

    
   

}