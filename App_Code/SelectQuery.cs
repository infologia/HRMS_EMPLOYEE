using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SelectQuery
/// </summary>
public class SelectQuery
{
    DataAccess DA = new DataAccess();
    public SelectQuery()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public DataTable SelectTableValue(string str_Sql, string str_ParameterValue, int ParameterCount)
    {
        DataTable dt_SelectProcedure = null;
        try
        {
            if (str_Sql == "") return dt_SelectProcedure;

            SqlCommand sc = new SqlCommand();
            if (str_ParameterValue != "")
            {
                string[] str_ParameterSplit = str_ParameterValue.Split('!');
                for (int i = 0; i < ParameterCount; i++)
                {
                    if (str_ParameterSplit[i].ToString() != "")
                        sc.Parameters.AddWithValue("@P" + i.ToString(), str_ParameterSplit[i].ToString());
                    else
                        sc.Parameters.AddWithValue("@P" + i.ToString(), DBNull.Value);
                }
            }

            sc.CommandText = str_Sql;
            dt_SelectProcedure = this.DA.GetDataTable(sc);

            return dt_SelectProcedure;
        }
        catch (Exception ex)
        {
            return dt_SelectProcedure;
        }
    }

    public DataSet SelectTableValueDataSet(string str_Sql, string str_ParameterValue, int ParameterCount)
    {
        DataSet ds_SelectProcedure = null;
        try
        {
            if (str_Sql == "") return ds_SelectProcedure;

            SqlCommand sc = new SqlCommand();
            if (str_ParameterValue != "")
            {
                string[] str_ParameterSplit = str_ParameterValue.Split('!');
                for (int i = 0; i < ParameterCount; i++)
                {
                    if (str_ParameterSplit[i].ToString() != "")
                        sc.Parameters.AddWithValue("@P" + i.ToString(), str_ParameterSplit[i].ToString());
                    else
                        sc.Parameters.AddWithValue("@P" + i.ToString(), DBNull.Value);
                }
            }

            sc.CommandText = str_Sql;
            ds_SelectProcedure = this.DA.GetDataSet(sc);

            return ds_SelectProcedure;
        }
        catch (Exception ex)
        {
            return ds_SelectProcedure;
        }
    }
}