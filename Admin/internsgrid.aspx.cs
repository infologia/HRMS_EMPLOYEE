using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Admin_internsgrid : System.Web.UI.Page
{
    DataAccess DA;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        DA = new DataAccess();
        PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Interns Informations";

            BindGrid();
        }
    }

    private void BindGrid()
    {
        string query = @"SELECT firstname, lastname, highestqualification, passedout,
                                university_college, currentlocation, phno, resume, createdon
                         FROM it_internsdetails
                         WHERE id IN (
                             SELECT MAX(id) FROM it_internsdetails GROUP BY email
                         )
                         ORDER BY createdon DESC";
        try
        {
            SqlCommand cmd = new SqlCommand(query);
            DataTable dt = DA.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("FullName");
                dt.Columns.Add("ResumeLink");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["FullName"] = (dr["firstname"].ToString() + " " + dr["lastname"].ToString()).Trim();
                    string resume = dr["resume"].ToString().Trim();
                    dr["ResumeLink"] = string.IsNullOrEmpty(resume)
                        ? "-"
                        : "<a href='https://backend.infologia.in/public/resumes/" + resume + "' target='_blank' class='btn btn-sm btn-primary'><i class='icon-file-text2'></i> View</a>";
                }
                DataSet ds = new DataSet();
                ds.Merge(dt);
                PH.LoadGridItem(ds, PH_Interns, "interns.txt", "");
            }
        }
        catch { }
    }
}
