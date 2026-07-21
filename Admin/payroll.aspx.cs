using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_payroll : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id;


    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Pay";
        }

        string str_pay = "SELECT DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS [MonthName], MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS [MonthNumber] ,DATEPART(year, getdate()) as currentyear FROM master.dbo.spt_values s WHERE [type] = 'P' AND s.number BETWEEN 0 AND 11 ORDER BY 2";
        SqlCommand cmd = new SqlCommand(str_pay);
        DataTable dt_pay = this.DA.GetDataTable(cmd);

        // ADD THIS BLOCK ONLY
        dt_pay.Columns.Add("ActionHtml", typeof(string));
        int currentMonth = DateTime.Now.Month;
        int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;

        //foreach (DataRow row in dt_pay.Rows)
        //{
        //    int month = Convert.ToInt32(row["MonthNumber"]);
        //    if (month == currentMonth || month == previousMonth)
        //    {
        //        row["ActionHtml"] = "<a href='payslip.aspx?key=" + month + "&id=" + row["currentyear"] + "'><span class='label label-primary'>Generate</span></a>";
        //    }
        //    else
        //    {
        //        row["ActionHtml"] = "<span class='label label-default'>Generate</span>";
        //    }
        //}
        //foreach (DataRow row in dt_pay.Rows)
        //{
        //    int month = Convert.ToInt32(row["MonthNumber"]);

        //    if (month == currentMonth)
        //    {
        //        row["ActionHtml"] = "<a href='payslip.aspx?key=" + month + "&id=" + row["currentyear"] + "'><span class='label label-primary'>Generate</span></a>";
        //    }
        //    else
        //    {
        //        row["ActionHtml"] = "<span class='label label-default'>Generate</span>";
        //    }
        //}
        //END

        DataSet ds = new DataSet();
        ds.Merge(dt_pay);

        if (dt_pay.Rows.Count > 0)
        {
            this.PH.LoadGridItem(ds, PH_payroll, "payroll.txt", "");
        }
    }
}