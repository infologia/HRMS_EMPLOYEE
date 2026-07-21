using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
public partial class Admin_ProjectFlowchart : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        
    }

    [WebMethod]
    public static List<object> GetChartData()
    {
        //Fetch the Statistical data from database.
        DataAccess DA = new DataAccess();
        string query = "Select  top 5 convert(char(5), Createdon, 108) [Time],DATEPART(DAY, Createdon)as date from IT_InOutTime";
        DataTable dt = DA.GetDataTable(query);

        //Get the DISTINCT Countries.
        List<object> chartData = new List<object>();
        List<string> Timings = (from p in dt.AsEnumerable()
                                select p.Field<string>("Time")).Distinct().ToList();

        //Insert Label for Country in First position.
        Timings.Insert(0, "Intime");

        //Add the Countries Array to the Chart Array.
        chartData.Add(Timings.ToArray());


        //Get the DISTINCT Years.
        List<int> years = (from p in dt.AsEnumerable()
                           select p.Field<int>("Date")).Distinct().ToList();

        //Loop through the Years.
        foreach (int year in years)
        {

            //Get the Total of Orders for each Country for the Year.
            List<object> totals = (from p in dt.AsEnumerable()
                                   where p.Field<int>("Date") == year
                                   select p.Field<int>("Date")).Cast<object>().ToList();

            //Insert the Year value as Label in First position.
            totals.Insert(0, year.ToString());

            //Add the Years Array to the Chart Array.
            chartData.Add(totals.ToArray());
        }

        return chartData;
    }

    [WebMethod]
 
    public  static List<object> GetChartData1()
    {
        //Fetch the Statistical data from database.
        DataAccess DA = new DataAccess();
        string query = "Select top 5 Username,Roles from IT_Employeeregister";
        DataTable dt = DA.GetDataTable(query);

        //Get the DISTINCT Countries.
        List<object> chartDatas = new List<object>();
        List<string> Timings = (from p in dt.AsEnumerable()
                                select p.Field<string>("Username")).Distinct().ToList();

        //Insert Label for Country in First position.
        Timings.Insert(0, "Username");

        //Add the Countries Array to the Chart Array.
        chartDatas.Add(Timings.ToArray());


        //Get the DISTINCT Years.
        List<int> years = (from p in dt.AsEnumerable()
                           select p.Field<int>("Roles")).Distinct().ToList();

    
        //Loop through the Years.
        foreach (int year in years)
        {

            //Get the Total of Orders for each Country for the Year.
            List<object> totals = (from p in dt.AsEnumerable()

                                   select p.Field<int>("Roles")).Cast<object>().ToList();

            //Insert the Year value as Label in First position.
            totals.Insert(0, year.ToString());

            //Add the Years Array to the Chart Array.
            chartDatas.Add(totals.ToArray());
        }

        return chartDatas;
    }

     [WebMethod]
    public static List<object> GetChartData2()
    {
        //Fetch the Statistical data from database.
        DataAccess DA = new DataAccess();
        string query = "Select top 5 Employeename,CAST(netpay AS DECIMAL(18, 2)) netpay from IT_EmployeeSalaryDetails";
        DataTable dt = DA.GetDataTable(query);

        //Get the DISTINCT Countries.
        List<object> chartDataes = new List<object>();
        List<string> Timings = (from p in dt.AsEnumerable()
                                select p.Field<string>("Employeename")).Distinct().ToList();

        //Insert Label for Country in First position.
        Timings.Insert(0, "Employeename");

        //Add the Countries Array to the Chart Array.
        chartDataes.Add(Timings.ToArray());


        //Get the DISTINCT Years.
        List<decimal> years = (from p in dt.AsEnumerable()
                           select p.Field<decimal>("Netpay")).Distinct().ToList();


        //Loop through the Years.
        foreach (decimal year in years)
        {

            //Get the Total of Orders for each Country for the Year.
            List<object> totals = (from p in dt.AsEnumerable()

                                   select p.Field<decimal>("Netpay")).Cast<object>().ToList();

            //Insert the Year value as Label in First position.
            totals.Insert(0, year.ToString());

            //Add the Years Array to the Chart Array.
            chartDataes.Add(totals.ToArray());
        }

        return chartDataes;
    }


       [WebMethod]
    public static List<object> GetChartData3()
    {
        //Fetch the Statistical data from database.
        DataAccess DA = new DataAccess();
        string query = "Select top 5 Summary,Preority from TT_CreateTaskDetails";
        DataTable dt = DA.GetDataTable(query);

        //Get the DISTINCT Countries.
        List<object> chartDatases = new List<object>();
        List<string> Timings = (from p in dt.AsEnumerable()
                                select p.Field<string>("Summary")).Distinct().ToList();

        //Insert Label for Country in First position.
        Timings.Insert(0, "Summary");

        //Add the Countries Array to the Chart Array.
        chartDatases.Add(Timings.ToArray());


        //Get the DISTINCT Years.
        List<int> years = (from p in dt.AsEnumerable()
                           select p.Field<int>("Preority")).Distinct().ToList();


        //Loop through the Years.
        foreach (int year in years)
        {

            //Get the Total of Orders for each Country for the Year.
            List<object> totals = (from p in dt.AsEnumerable()

                                   select p.Field<int>("Preority")).Cast<object>().ToList();

            //Insert the Year value as Label in First position.
            totals.Insert(0, year.ToString());

            //Add the Years Array to the Chart Array.
            chartDatases.Add(totals.ToArray());
        }

        return chartDatases;
    }



    


    //private static DataTable GetData(string query)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    //    using (SqlConnection con = new SqlConnection(constr))
    //    {
    //        using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
    //        {
    //            DataTable dt = new DataTable();
    //            sda.Fill(dt);
    //            return dt;
    //        }
    //    }
    //}
}