using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CreateSalesdocs : System.Web.UI.Page
{

    DataAccess DA;
    SessionCustom SC;
    CommonFunction CF;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.CF = new CommonFunction();
        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Sales Documents";

    }

    protected void btn_Create_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        string fileTitle = txt_filetitle.Text.Trim();
        string description = txt_des.Value;
        string fileName = "";

        if (fu_file.HasFile)
        {
            string ext = Path.GetExtension(fu_file.FileName);
            fileName = Guid.NewGuid().ToString() + ext;

           string path = Server.MapPath("~/Document/Uploads/SalesDocs/");
            fu_file.SaveAs(path + fileName);
        }

        string query = @"INSERT INTO IT_SalesDocuments
                         (FileTitle, FileName, Description, CreatedBy, CreatedOn)
                         VALUES
                         (@FileTitle,@FileName,@Description,@CreatedBy,GETDATE())";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@FileTitle", fileTitle);
        cmd.Parameters.AddWithValue("@FileName", fileName);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@CreatedBy", SC.Userid);

        DA.ExecuteNonQuery(cmd);

        ClientScript.RegisterStartupScript(
       this.GetType(),
       "ok",
       "<script>alert('Sales document created successfully');window.location='Salesdocs.aspx'</script>"
   );
    }

}