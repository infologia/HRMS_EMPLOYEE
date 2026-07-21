using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Handles all user session-related data safely.
/// </summary>
public class SessionCustom
{
    DataTable dt_Company = null;
    DataTable dt_Menu = null;
    DataTable dt_QuickAccess = null;
    DataTable dt_Provider = null;
    DataTable dt_Empmanager = null;

    // Default constructor
    public SessionCustom()
    {
    }

    // Constructor with session validation
    public SessionCustom(bool bln_SessionCheckRequired)
    {
        try
        {
            if (bln_SessionCheckRequired && HttpContext.Current.Session["userid"] == null)
                this.NavigateToLoginPage();
        }
        catch
        {
            this.NavigateToLoginPage();
        }
    }

    // Helper method to safely get User ID
    public string GetUserid()
    {
        return Userid;
    }

    // Base URL (safe)
    public string BaseUrl()
    {
        if (HttpContext.Current.Session["httproot"] != null)
            return HttpContext.Current.Session["httproot"].ToString();
        else
            return string.Empty;
    }

    // Redirect user to login
    public void NavigateToLoginPage()
    {
        HttpContext.Current.Response.Redirect("~/Default.aspx");
    }

    // ---------------------------
    // Safe Session Properties
    // ---------------------------

    public string Userid
    {
        get
        {
            if (HttpContext.Current.Session["userid"] != null)
                return HttpContext.Current.Session["userid"].ToString();
            else
            {
                // Redirect if session expired
                HttpContext.Current.Response.Redirect("~/Default.aspx");
                return string.Empty; // Won’t execute after redirect
            }
        }
        set
        {
            HttpContext.Current.Session["userid"] = value;
        }
    }

    public string Userdesg
    {
        get
        {
            if (HttpContext.Current.Session["Destination"] != null)
                return HttpContext.Current.Session["Destination"].ToString();
            else
                return string.Empty;
        }
        set { HttpContext.Current.Session["Destination"] = value; }
    }

    public string AccKey
    {
        get
        {
            if (HttpContext.Current.Session["acckey"] != null)
                return HttpContext.Current.Session["acckey"].ToString();
            else
                return string.Empty;
        }
        set { HttpContext.Current.Session["acckey"] = value; }
    }

    public string LogKey
    {
        get
        {
            if (HttpContext.Current.Session["logkey"] != null)
                return HttpContext.Current.Session["logkey"].ToString();
            else
                return string.Empty;
        }
        set { HttpContext.Current.Session["logkey"] = value; }
    }

    public string username
    {
        get
        {
            if (HttpContext.Current.Session["Username"] != null)
                return HttpContext.Current.Session["Username"].ToString();
            else
                return string.Empty;
        }
        set { HttpContext.Current.Session["Username"] = value; }
    }

    public string UserImage
    {
        get
        {
            if (HttpContext.Current.Session["image"] != null)
                return HttpContext.Current.Session["image"].ToString();
            else
                return string.Empty;
        }
        set { HttpContext.Current.Session["image"] = value; }
    }

    public DataRow UserRecord
    {
        get
        {
            if (HttpContext.Current.Session["userrecord"] != null)
                return (DataRow)HttpContext.Current.Session["userrecord"];
            else
                return null;
        }
        set { HttpContext.Current.Session["userrecord"] = value; }
    }

    public DataTable UserRecordTable
    {
        get
        {
            if (HttpContext.Current.Session["userrecordtable"] != null)
                return (DataTable)HttpContext.Current.Session["userrecordtable"];
            else
                return null;
        }
        set { HttpContext.Current.Session["userrecordtable"] = value; }
    }

    public string UserRole
    {
        get
        {
            if (HttpContext.Current.Session["Roles"] != null)
                return HttpContext.Current.Session["Roles"].ToString();
            else
                return string.Empty;
        }
        set { HttpContext.Current.Session["Roles"] = value; }
    }

    // ---------------------------
    // Helper Functions
    // ---------------------------

    public bool SessionExists()
    {
        if (HttpContext.Current.Session["userid"] == null || 
            string.IsNullOrEmpty(HttpContext.Current.Session["userid"].ToString()))
            return false;
        else
            return true;
    }

    // Check Menu Permissions
    public bool HasPermission(string menuName, string permissionType)
    {
        try
        {
            if (HttpContext.Current.Session["userid"] == null)
                return false;

            string userId = HttpContext.Current.Session["userid"].ToString();
            
            DataAccess DA = new DataAccess();
            
            string query = @"SELECT em.ViewPermission, em.CreatePermission, 
                            em.EditPermission, em.DeletePermission
                            FROM IT_EmployeeMenus em
                            INNER JOIN IT_Menus m ON em.MenuId = m.MenuKey
                            WHERE em.EmployeeKey = @EmployeeKey 
                            AND m.MenuName = @MenuName";
            
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query);
            cmd.Parameters.AddWithValue("@EmployeeKey", userId);
            cmd.Parameters.AddWithValue("@MenuName", menuName);
            
            DataTable dt = DA.GetDataTable(cmd);
            
            if (dt.Rows.Count > 0)
            {
                switch (permissionType.ToLower())
                {
                    case "view":
                        return Convert.ToBoolean(dt.Rows[0]["ViewPermission"]);
                    case "create":
                        return Convert.ToBoolean(dt.Rows[0]["CreatePermission"]);
                    case "edit":
                        return Convert.ToBoolean(dt.Rows[0]["EditPermission"]);
                    case "delete":
                        return Convert.ToBoolean(dt.Rows[0]["DeletePermission"]);
                    default:
                        return false;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
