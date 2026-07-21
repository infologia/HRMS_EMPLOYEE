using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Menuicons : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
           Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Icons";
        
    }
}