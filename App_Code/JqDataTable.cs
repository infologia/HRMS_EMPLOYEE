using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Data;
using System.Text;
/// <summary>
/// Summary description for JqDataTable
/// </summary>
public class JqDataTable
{
	public JqDataTable()
	{
		//
		// TODO: Add constructor logic here
		//
	}

  
    public StringBuilder BuildTableDataForGrid(DataTable dt, string str_TableName, string str_Where, string str_Edit, string str_Remove)
    {
        return this.BuildTableDataForGridWithFooterSum(dt, str_TableName, str_Where, str_Edit, str_Remove, new ArrayList());
    }

    public StringBuilder BuildTableDataForGridWithFooterSum(DataTable dt, string str_TableName, string str_Where, string str_Edit, string str_Remove, ArrayList al_FooterSumIndex)
    {
        ArrayList al_FooterSumValue = new ArrayList();
        foreach (int i in al_FooterSumIndex) al_FooterSumValue.Add(0);
      

        StringBuilder SB = new StringBuilder();
        SB.Append("<table id=\"" + str_TableName + "\" class=\"table table-striped table-bordered table-hover\" width=\"100%\" cellspacing=\"0\">");


        SB.Append("<thead><tr>");
        foreach (DataColumn dc in dt.Columns)
        {
            SB.Append("<th>" + dc.Caption + "</th>");
        }
        if (str_Edit != "") SB.Append("<th>Edit</th>");
        if (str_Remove != "") SB.Append("<th>Remove</th>");
        SB.Append("</tr></thead>");

        string str_EditLink = str_Edit;
        string str_RemoveLink = str_Remove;
        SB.Append("<tbody>");
        //body records
        foreach (DataRow dr in dt.Select(str_Where))
        {
            str_EditLink = str_Edit;
            str_RemoveLink = str_Remove;
            SB.Append(" <tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                SB.Append("<td>" + dr[i].ToString() + "</td>");
                if (str_EditLink.Contains("{" + dt.Columns[i].ColumnName + "}"))
                {
                    str_EditLink = str_EditLink.Replace("{" + dt.Columns[i].ColumnName + "}", dr[i].ToString());
                    str_RemoveLink = str_RemoveLink.Replace("{" + dt.Columns[i].ColumnName + "}", dr[i].ToString());
                }
                int inFooterIndex = al_FooterSumIndex.IndexOf(i);
                if (inFooterIndex >= 0)
                {
                    try { al_FooterSumValue[inFooterIndex] = Convert.ToInt32(al_FooterSumValue[inFooterIndex]) + Convert.ToInt32(dr[i]); }
                    catch (Exception ex) { }
                }
            }

            if (str_Edit != "") SB.Append("<td><a href='" + str_EditLink + "'>" + "Edit" + "</a></td>");
            if (str_Remove != "") SB.Append("<td><a onclick=\"return confirm('Are you sure you want to delete this record?')\"; href='" + str_RemoveLink + "'>" + "Remove" + "</a></td>");
            SB.Append(" </tr>");
        }
        //footer 
        if (al_FooterSumIndex.Count > 0)
        {
            SB.Append(" <tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                int inFooterIndex = al_FooterSumIndex.IndexOf(i);
                if (inFooterIndex >= 0)
                    SB.Append("<td><b>" + al_FooterSumValue[inFooterIndex].ToString() + "</b></td>");
                else
                    SB.Append("<td></td>");
            }
            SB.Append(" </tr>");
        }
        SB.Append("</tbody>");
        SB.Append("</table>");

        return SB;
        //hl.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to delete this record?');");
    }
}