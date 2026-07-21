using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_InternOnboardingView : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Interns View";

            this.LoadGrid();
        }
    }

    private void LoadGrid()
    {
        try
        {
            string query = "SELECT ID, ProfileImage, InternCode, FullName, Email, Phonenumber, Department FROM IT_InternOnboarding WHERE IsActive = 1 ORDER BY ID DESC";
            DataTable dt = DA.GetDataTable(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    string profileImage = row["ProfileImage"].ToString();
                    string imagePath;
                    
                    if (!string.IsNullOrEmpty(profileImage))
                    {
                        imagePath = ResolveUrl("~/images/InternProfilePictures/" + profileImage);
                    }
                    else
                    {
                        imagePath = ResolveUrl("~/images/default-avatar.png"); // Fallback image if none uploaded
                    }
                    
                    sb.Append("<tr>");
                    sb.Append("<td><img src='" + imagePath + "' alt='Profile' style='width:40px;height:40px;border-radius:50%; object-fit:cover;' /></td>");
                    sb.Append("<td>" + row["InternCode"].ToString() + "</td>");
                    sb.Append("<td>" + row["FullName"].ToString() + "</td>");
                    sb.Append("<td>" + row["Email"].ToString() + "</td>");
                    sb.Append("<td>" + row["Phonenumber"].ToString() + "</td>");
                    sb.Append("<td>" + row["Department"].ToString() + "</td>");
                    sb.Append("<td><a href='InternOnboarding.aspx?id=" + row["ID"].ToString() + "' class='btn btn-primary btn-sm'><i class='icon-pencil'></i> </a></td>");
                    sb.Append("</tr>");
                }
                PH_InternView.Controls.Add(new LiteralControl(sb.ToString()));
            }
        }
        catch (Exception ex)
        {
            // Handle error silently or show a message
        }
    }
}
