using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
/// <summary>
/// Summary description for SaveQuery
/// </summary>
public class SaveQuery
{
    DataAccess DA = new DataAccess();
    public SaveQuery()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool SaveSQLQuery(string str_TablesNames, string str_Values, string str_PrimakeyValues, string str_WhereConditon, string str_Type)
    {
        bool bl_Save = false;
        if (str_TablesNames == "" || str_Values == "" || str_PrimakeyValues == "" || str_Type == "") return bl_Save;
        try
        {
            string[] str_TablesNameSplit = str_TablesNames.Split(',');
            string[] str_ValuesSplit = str_Values.Split('|');
            string[] str_PrimarKeyValuesSplit = str_PrimakeyValues.Split(',');
            string[] str_WhereConditonSplit = null;
            if (str_WhereConditon != "")
                str_WhereConditonSplit = str_WhereConditon.Split(',');
            bool bln_AddMode = false;
            if (str_Type.ToLower() == "insert") bln_AddMode = true;
            ArrayList al_SC = new ArrayList();
            SqlCommand sc = new SqlCommand();
            for (int i = 0; i < str_TablesNameSplit.Length; i++)
            {
                string str_TableName = str_TablesNameSplit[i].ToString();
                string str_ColumnFullValues = str_ValuesSplit[i].ToString();
                string str_PrimaryValue = str_PrimarKeyValuesSplit[i].ToString();
                string str_WhereCondtionValues = "";
                if (str_WhereConditon != "")
                    str_WhereCondtionValues = str_WhereConditonSplit[i];
                string str_InsertQuery = " insert into " + str_TableName + " %%ColumnName%% values %%ColumnValue%%";
                string str_UpdateQuery = " update " + str_TableName + " set %%UpdateColumnAndValues%% where %%WhereColumn%% %%WhereCondition%%";
                //string[] str_ValueString = str_OtherColumns.Split('&');
                //int int_length = str_ValueString.Length;
                string str_Query = "";
                string str_ColumnName = "", str_ColumnValue = "", str_UpdateQueryValues = "", str_DbColName = "", str_DbColValue = "";


                string[] str_SplitColumnandValues = str_ColumnFullValues.Split('!');
                string[] str_SplitPrimaryKey = str_PrimaryValue.Split(':');

                for (int j = 0; j < str_SplitColumnandValues.Length; j++)
                {
                    string[] str_DbColNameandValueSplit = str_SplitColumnandValues[j].ToString().Split('~');
                    str_DbColName = str_DbColNameandValueSplit[0].ToString();
                    str_DbColValue = str_DbColNameandValueSplit[1].ToString();

                    if (bln_AddMode)
                    {
                        str_ColumnName += str_DbColName + ",";
                        str_ColumnValue += "@" + str_DbColName + ",";
                    }
                    else
                    {
                        str_UpdateQueryValues += "[" + str_DbColName + "]=@" + str_DbColName.Replace(" ", "_") + ",";
                    }
                    sc.Parameters.AddWithValue("@" + str_DbColName, str_DbColValue);
                }
                //prepare final SQL
                if (bln_AddMode)
                {

                    str_ColumnName += str_SplitPrimaryKey[0].ToString() + ",";
                    str_ColumnValue += "@" + str_SplitPrimaryKey[0].ToString() + ",";
                    sc.Parameters.AddWithValue("@" + str_SplitPrimaryKey[0].ToString(), str_SplitPrimaryKey[1].ToString());
                    str_ColumnName = str_ColumnName.Substring(0, str_ColumnName.Length - 1);
                    str_ColumnValue = str_ColumnValue.Substring(0, str_ColumnValue.Length - 1);
                    str_Query = str_InsertQuery.Replace("%%ColumnName%%", "(" + str_ColumnName + ")").Replace("%%ColumnValue%%", "(" + str_ColumnValue + ")");
                }
                else
                {
                    if (str_WhereConditon == "")
                        str_UpdateQuery = str_UpdateQuery.Replace("%%WhereCondition%%", "");
                    else
                    {
                        string[] str_SplitWhereCondition = str_WhereCondtionValues.Split('|');
                        string str_WhereCondtionReplace = "";
                        for (int k = 0; k < str_SplitWhereCondition.Length; k++)
                        {
                            string[] str_WhereDbColumnCondition = str_SplitWhereCondition.ToString().Split(':');
                            string str_WhereCondtionDbColName = str_WhereDbColumnCondition[0].ToString();
                            string str_WhereCondtionDbColValue = str_WhereDbColumnCondition[1].ToString();
                            str_WhereCondtionReplace += str_WhereCondtionDbColName + "=@" + str_WhereCondtionDbColName + " and";
                            sc.Parameters.AddWithValue("@" + str_WhereCondtionDbColName, str_WhereCondtionDbColValue);
                        }
                        str_WhereCondtionReplace = str_WhereCondtionReplace.Substring(0, str_WhereCondtionReplace.Length - 4);
                        str_UpdateQuery = str_UpdateQuery.Replace("%%WhereCondition%%", str_WhereCondtionReplace);
                    }
                    string str_WhereUpdate =  str_SplitPrimaryKey[0].ToString() + "=@" + str_SplitPrimaryKey[0].ToString();
                    sc.Parameters.AddWithValue("@" + str_SplitPrimaryKey[0].ToString(), str_SplitPrimaryKey[1].ToString());
                    str_UpdateQueryValues = str_UpdateQueryValues.Substring(0, str_UpdateQueryValues.Length - 1);
                    str_Query = str_UpdateQuery.Replace("%%UpdateColumnAndValues%%", str_UpdateQueryValues).Replace("%%WhereColumn%%", str_WhereUpdate);
                }
                sc.CommandText = str_Query;
                al_SC.Add(sc);
            }
            this.DA.ExecuteNonQuery(al_SC);
            bl_Save = true;
            return bl_Save;
        }
        catch (Exception ex)
        {
            return bl_Save;
        }
    }


    public int SaveProcedure(string str_Sql, string str_ParameterValue, int ParameterCount)
    {
        int int_SaveProcedure = 0;
        try
        {
            if (str_Sql == "") return int_SaveProcedure;

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
            DataTable dt_SaveProcedure = this.DA.GetDataTable(sc);
            if (dt_SaveProcedure != null && dt_SaveProcedure.Rows.Count > 0 && Convert.ToString(dt_SaveProcedure.Rows[0][0]) == "1")
                int_SaveProcedure = 1;
            else if (dt_SaveProcedure != null && dt_SaveProcedure.Rows.Count > 0 && Convert.ToString(dt_SaveProcedure.Rows[0][0]) == "2")
                int_SaveProcedure = 2;
            else if (dt_SaveProcedure != null && dt_SaveProcedure.Rows.Count > 0 && Convert.ToString(dt_SaveProcedure.Rows[0][0]) == "3")
                int_SaveProcedure = 3;

            return int_SaveProcedure;
        }
        catch (Exception ex)
        {
            return int_SaveProcedure;
        }
    }
    public bool CreateLogKey(string str_UserKey)
    {
        bool CreateLogKey = false;
        try
        {
            string str_LogKey = Guid.NewGuid().ToString();
            string str_Sql = "insert into mm_logdetail(LogKey, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy) values (@LogKey, @CreatedOn, @ModifiedOn, @CreatedBy, @ModifiedBy)";
            SqlCommand sc = new SqlCommand(str_Sql);
            sc.Parameters.AddWithValue("@LogKey", str_LogKey);
            sc.Parameters.AddWithValue("@CreatedBy", str_UserKey);
            sc.Parameters.AddWithValue("@ModifiedBy", str_UserKey);
            sc.Parameters.AddWithValue("@CreatedOn", new CommonFunction().GetIndianDateTime());
            sc.Parameters.AddWithValue("@ModifiedOn", new CommonFunction().GetIndianDateTime());
            this.DA.ExecuteNonQuery(sc);

            CreateLogKey = true;
            return CreateLogKey;
        }
        catch (Exception ex)
        {
            return CreateLogKey;
        }
    }

    public bool ModifyLogKey(string str_LogKey, string str_UserKey)
    {
        bool ModifyLogKey = false;
        try
        {
            string str_Sql = "Update mm_logdetail set ModifiedOn=getdate(), ModifiedBy=@ModifiedBy where LogKey=@LogKey";
            SqlCommand sc = new SqlCommand(str_Sql);
            sc.Parameters.AddWithValue("@LogKey", str_LogKey);
            sc.Parameters.AddWithValue("@ModifiedBy", str_UserKey);
            this.DA.ExecuteNonQuery(sc);
            ModifyLogKey = true;
            return ModifyLogKey;
        }
        catch (Exception ex)
        {
            return ModifyLogKey;
        }
    }
}