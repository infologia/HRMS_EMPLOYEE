using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for LogException
/// </summary>
public class LogException
{
	public LogException()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void Write(HttpRequest req, string str_Exception)
    {
        Guid logseq = Guid.NewGuid();
        System.Collections.ArrayList al_Sql = new System.Collections.ArrayList();
        string str_Sql = "INSERT INTO [LogException] "
                               + " (LogSeq,UserAgent,UserHostAddress,UserHostName,UserLanguage,TotalBytes,IsBrowser,IsMobileDevice,BrowserName,BrowserVersion,MobileDeviceManufacturer,MobileDeviceModel,BrowserPlatform,UrlReferrer,SecureConnection,HTTPMethod,CurrentExecutionPath,ContentType,ContentLengh,Exception)"
                               + " select @LogSeq,@UserAgent,@UserHostAddress,@UserHostName,@UserLanguage,@TotalBytes,@IsBrowser,@IsMobileDevice,@BrowserName,@BrowserVersion,@MobileDeviceManufacturer,@MobileDeviceModel,@BrowserPlatform,@UrlReferrer,@SecureConnection,@HTTPMethod,@CurrentExecutionPath,@ContentType,@ContentLengh,@Exception ";
        SqlCommand sc = new SqlCommand(str_Sql);
        sc.Parameters.Add(new SqlParameter("@LogSeq", SqlDbType.UniqueIdentifier)).Value = logseq;
        sc.Parameters.Add(new SqlParameter("@UserAgent", SqlDbType.NVarChar)).Value = req.UserAgent;
        sc.Parameters.Add(new SqlParameter("@UserHostAddress", SqlDbType.NVarChar)).Value = req.UserHostAddress;
        sc.Parameters.Add(new SqlParameter("@UserHostName", SqlDbType.NVarChar)).Value = req.UserHostName;
        sc.Parameters.Add(new SqlParameter("@UserLanguage", SqlDbType.NVarChar)).Value = req.UserLanguages[0].ToString();
        sc.Parameters.Add(new SqlParameter("@TotalBytes", SqlDbType.Int)).Value = req.TotalBytes;
        sc.Parameters.Add(new SqlParameter("@IsBrowser", SqlDbType.Bit)).Value = DBNull.Value;
        if (req.Browser != null)
        {
            sc.Parameters.Add(new SqlParameter("@IsMobileDevice", SqlDbType.Bit)).Value = req.Browser.IsMobileDevice;
            sc.Parameters.Add(new SqlParameter("@BrowserName", SqlDbType.NVarChar)).Value = req.Browser.Browser;
            sc.Parameters.Add(new SqlParameter("@BrowserVersion", SqlDbType.NVarChar)).Value = req.Browser.Version;
            sc.Parameters.Add(new SqlParameter("@MobileDeviceManufacturer", SqlDbType.NVarChar)).Value = req.Browser.MobileDeviceManufacturer;
            sc.Parameters.Add(new SqlParameter("@MobileDeviceModel", SqlDbType.NVarChar)).Value = req.Browser.MobileDeviceModel;
            sc.Parameters.Add(new SqlParameter("@BrowserPlatform", SqlDbType.NVarChar)).Value = req.Browser.Platform;
        }
        else
        {
            sc.Parameters.Add(new SqlParameter("@IsMobileDevice", SqlDbType.Bit)).Value = DBNull.Value;
            sc.Parameters.Add(new SqlParameter("@BrowserName", SqlDbType.NVarChar)).Value = DBNull.Value;
            sc.Parameters.Add(new SqlParameter("@BrowserVersion", SqlDbType.NVarChar)).Value = DBNull.Value;
            sc.Parameters.Add(new SqlParameter("@MobileDeviceManufacturer", SqlDbType.NVarChar)).Value = DBNull.Value;
            sc.Parameters.Add(new SqlParameter("@MobileDeviceModel", SqlDbType.NVarChar)).Value = DBNull.Value;
            sc.Parameters.Add(new SqlParameter("@BrowserPlatform", SqlDbType.NVarChar)).Value = DBNull.Value;
        }

        if (req.UrlReferrer != null && req.UrlReferrer.AbsoluteUri != null)
            sc.Parameters.Add(new SqlParameter("@UrlReferrer", SqlDbType.NVarChar)).Value = req.UrlReferrer.AbsoluteUri;
        else
            sc.Parameters.Add(new SqlParameter("@UrlReferrer", SqlDbType.NVarChar)).Value = DBNull.Value;
        sc.Parameters.Add(new SqlParameter("@SecureConnection", SqlDbType.Bit)).Value = req.IsSecureConnection;
        sc.Parameters.Add(new SqlParameter("@HTTPMethod", SqlDbType.NVarChar)).Value = req.HttpMethod;
        sc.Parameters.Add(new SqlParameter("@CurrentExecutionPath", SqlDbType.NVarChar)).Value = req.CurrentExecutionFilePath;
        sc.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.NVarChar)).Value = req.ContentType;
        sc.Parameters.Add(new SqlParameter("@ContentLengh", SqlDbType.Int)).Value = req.ContentLength;
        sc.Parameters.Add(new SqlParameter("@Exception", SqlDbType.VarChar)).Value = str_Exception;
        al_Sql.Add(sc);

        if (req.QueryString != null)
        {
            str_Sql = "INSERT INTO [LogExceptionParam] (LogSeq, ParamName, ParamValue) "
                + " select @LogSeq, @ParamName, @ParamValue ";

            foreach (string paramname in req.QueryString)
            {
                sc = new SqlCommand(str_Sql);
                sc.Parameters.Add(new SqlParameter("@LogSeq", SqlDbType.UniqueIdentifier)).Value = logseq;
                sc.Parameters.Add(new SqlParameter("@ParamName", SqlDbType.NVarChar)).Value = paramname;
                sc.Parameters.Add(new SqlParameter("@ParamValue", SqlDbType.NVarChar)).Value = req.QueryString[paramname];
                al_Sql.Add(sc);
                //Response.Write(paramname + ":" + req.QueryString[paramname]);
            }
        }
        if (req.Form != null)
        {
            str_Sql = "INSERT INTO [LogExceptionParam] (LogSeq, ParamName, ParamValue) "
                + " select @LogSeq, @ParamName, @ParamValue ";

            foreach (string paramname in req.Form)
            {
                sc = new SqlCommand(str_Sql);
                sc.Parameters.Add(new SqlParameter("@LogSeq", SqlDbType.UniqueIdentifier)).Value = logseq;
                sc.Parameters.Add(new SqlParameter("@ParamName", SqlDbType.NVarChar)).Value = paramname;
                sc.Parameters.Add(new SqlParameter("@ParamValue", SqlDbType.NVarChar)).Value = req.Form[paramname];
                al_Sql.Add(sc);
                //Response.Write(paramname + ":" + req.QueryString[paramname]);
            }
        }
        try
        {
            new DataAccess().ExecuteNonQuery(al_Sql);
        }
        catch (Exception ex)
        {
            string str = ex.ToString();
        }
    }

}
