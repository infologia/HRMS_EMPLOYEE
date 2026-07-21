using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_PolicyDocument : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.str_userkey = SC.Userid;
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Policy Documents";
        }

        string str_query = "select PolicyDocumentKey,PolicyDocument,DocumentName,Status,CreatedOn,Modifiedon from IT_PolicyDocument ";
        SqlCommand cmd = new SqlCommand(str_query);
        
        DataTable dt_document = DA.GetDataTable(cmd);

        if (dt_document.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt_document);

            ds.Tables[0].Columns.Add("ViewDocument");
            ds.Tables[0].Columns.Add("Download");


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string fileName = dr["PolicyDocument"].ToString();
                string filePath = "../Document/" + fileName;

                dr["ViewDocument"] = "<a href='" + filePath + "' target='_blank'><button type='button' class='label label-info'>View</button></a>";
                dr["Download"] = "<a href='" + filePath + "' download><button type='button' class='label label-sm label-success'>Download</button></a>";

            }

            if (ds.Tables[0].Columns.Contains("Status"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Status"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Active</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-sm label-danger'>InActive</span>";
            }
            this.PH.LoadGridItem(ds, PH_EmployeeDocumentView,"Policy.txt","");
        }


    }

}