using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class TicketingTool_Projects : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id = "";
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        this.AssignPanel();
        this.assigntable();
        
    }

    private void assigntable()
    {
        string str_table = "select a.projectkey,a.pjname,a.ProgressStatus,b.Categoryname,c.username,CONVERT(Varchar,a.createdon,103)as createdon,(select count(employeekey) as total from tt_adduser where projectkey=a.Projectkey) as memebers from TT_Project a left outer join TT_PjtCategory b on a.PjCategory=b.Pjtcategorykey left outer join it_employeeregister c on a.createdby=c.employeekey order by a.Createdon desc";
        SqlCommand cmd = new SqlCommand(str_table);

        DataTable dt_table = DA.GetDataTable(cmd);
        DataSet DS = new DataSet();
        DS.Merge(dt_table);
        if (dt_table.Rows.Count > 0)
        {
            if (DS.Tables[0].Columns.Contains("ProgressStatus"))
                DS.Tables[0].Columns.Add("ActiveCategory");
            foreach (DataRow dr1 in DS.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr1["ProgressStatus"].ToString());
                if (activetype == 1)
                    dr1["ActiveCategory"] = "<span class='label label-success'>Active</span>";
                
                if (activetype == 2)
                    dr1["ActiveCategory"] = "<span class='label label-danger'>In Active</span>";


            }

            this.PH.LoadGridItem(DS, PH_Project, "Project.txt", "");

        }
    }

    public void AssignPanel()
    {
        string str_query = "Select a.pjname,a.projectkey,b.Categoryname from TT_Project a left outer join TT_PjtCategory b on a.PjCategory=b.Pjtcategorykey where ProgressStatus='1'";

        SqlCommand cmd = new SqlCommand(str_query);

        DataTable dt_Prj = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_Prj);
        if (dt_Prj.Rows.Count > 0)
        {
            this.PH.LoadGridItem(ds, PH_Panel, "ProjectPanel.txt", "");
        }

    }



}