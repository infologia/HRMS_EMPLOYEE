using System;
using System.Web;
using System.Web.UI;

/// <summary>
/// Base page class for all admin pages with permission checking
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected SessionCustom SC;
    protected DataAccess DA;
    
    // Override this in each page to set the menu name
    protected virtual string PageMenuName
    {
        get { return ""; }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        // Initialize common objects
        this.SC = new SessionCustom();
        this.DA = new DataAccess();
        
        // Check if user is logged in
        if (!SC.SessionExists())
        {
            Response.Redirect("~/Default.aspx");
            return;
        }
        
        // Check page permission
        CheckPagePermission();
    }

    private void CheckPagePermission()
    {
        // Skip permission check if PageMenuName is not set
        if (string.IsNullOrEmpty(PageMenuName))
            return;
            
        // Check if user has view permission for this page
        if (!SC.HasPermission(PageMenuName, "view"))
        {
            // Redirect to access denied or dashboard
            Response.Redirect("~/Admin/Dashboard.aspx?error=access_denied");
            return;
        }
    }

    // Helper methods for permission checks
    protected bool CanCreate()
    {
        if (string.IsNullOrEmpty(PageMenuName))
            return false;
        return SC.HasPermission(PageMenuName, "create");
    }

    protected bool CanEdit()
    {
        if (string.IsNullOrEmpty(PageMenuName))
            return false;
        return SC.HasPermission(PageMenuName, "edit");
    }

    protected bool CanDelete()
    {
        if (string.IsNullOrEmpty(PageMenuName))
            return false;
        return SC.HasPermission(PageMenuName, "delete");
    }

    protected bool CanView()
    {
        if (string.IsNullOrEmpty(PageMenuName))
            return false;
        return SC.HasPermission(PageMenuName, "view");
    }
}
