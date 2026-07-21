using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_MenuUpdate : System.Web.UI.Page
{
    SessionCustom SC;
    DataAccess DA;
    string str_id = "";
    //string str_parid ="NULL";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();
        //string path = @"E:\Ticket\";
        // var directories = Directory.GetDirectories(path);


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Menus";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }


        if (Request.QueryString["id"] == "" || Request.QueryString["id"] == null)
        {
            if (!IsPostBack)
            {
                
                this.Destination();
                btn_submit.Text = "Submit";
            }

        }
        else
        {
            this.str_id = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                
                this.Destination();
                this.assignvalues();
                btn_submit.Text = "Update";

                if (ddl_ParentMenuName.SelectedValue=="5eacbe17-4793-4bf6-8a9c-62272215ff9a")

                {
                    txt_iconslist.Visible = true;

                }

            }
        }
    }


    private void Destination()
    {
        string str_desg = "select * from it_destination";
        SqlCommand cmd3 = new SqlCommand(str_desg);
        DataSet ds = this.DA.GetDataSet(cmd3);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_desgn.DataSource = ds.Tables[0];
            ddl_desgn.DataTextField = "Destinationname";
            ddl_desgn.DataValueField = "Destinationkey";
            ddl_desgn.DataBind();
            ddl_desgn.Items.Add(new ListItem("Select Designation ", "0"));
            ddl_desgn.SelectedValue = "0";
        }


    }


    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {
            string menulistmaster = "5eacbe17-4793-4bf6-8a9c-62272215ff9a";
            string menulistParentMenuName = ddl_ParentMenuName.SelectedValue;

            if (menulistParentMenuName == menulistmaster)
            {
                if (this.str_id == "")
                {
                    int partype = 0;
                    //string str_userky = Guid.NewGuid().ToString();
                    string str_int = "insert into TT_Menus(Foldername,MenuName,Menudescription,Menutype,Menulist,Menuicon,Createdby,Destinationkey,Status,pagename)values(@menufoldername,@ParentMenuName,@Menudescription,@Menutype,@Menulist,@Menuicon,@Createdby,@Destinationkey,@Status,@pagename)";
                    SqlCommand cmd = new SqlCommand(str_int);
                    cmd.Parameters.AddWithValue("Status", Convert.ToInt32(rblStatus.SelectedValue));
                    cmd.Parameters.AddWithValue("ParentMenuName", txt_menuname.Text);
                    cmd.Parameters.AddWithValue("Menutype", partype);
                    cmd.Parameters.AddWithValue("Menulist", txt_menuno.Text);
                    cmd.Parameters.AddWithValue("Menuicon", txt_icondesign.Text);
                    cmd.Parameters.AddWithValue("Menudescription", txt_menudesc.InnerText);
                    cmd.Parameters.AddWithValue("Createdby", this.SC.Userid);
                    cmd.Parameters.AddWithValue("Destinationkey", ddl_desgn.SelectedValue);
                    cmd.Parameters.AddWithValue("pagename", txt_pagename.Text);
                    cmd.Parameters.AddWithValue("menufoldername", txt_foldername.Text);
                    DA.ExecuteNonQuery(cmd);
                }
                else
                {
                    string date = DateTime.Now.ToString();
                    string str_int = "UPDATE TT_Menus SET Foldername=menufoldername,@MenuName=@ParentMenuName,Menuicon=@Menuicon,Menulist=@Menulist,Menudescription=@Menudescription,Modifiedby=@Modifiedby,Modifiedon=@Modifiedon,Destinationkey=@Destinationkey,Status=@Status,pagename=@pagename WHERE MenuKey=@ParentMenuKey;";
                    SqlCommand cmd = new SqlCommand(str_int);
                    cmd.Parameters.AddWithValue("Status", Convert.ToInt32(rblStatus.SelectedValue));
                    cmd.Parameters.AddWithValue("ParentMenuName", txt_menuname.Text);
                    cmd.Parameters.AddWithValue("Menudescription", txt_menudesc.InnerText);
                    cmd.Parameters.AddWithValue("ParentMenuKey", this.str_id);
                    cmd.Parameters.AddWithValue("Menuicon", txt_icondesign.Text);
                    cmd.Parameters.AddWithValue("Menulist", txt_menuno.Text);
                    cmd.Parameters.AddWithValue("Modifiedby", this.SC.Userid);
                    cmd.Parameters.Add("Modifiedon", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.AddWithValue("pagename", txt_pagename.Text);
                    cmd.Parameters.AddWithValue("Destinationkey", ddl_desgn.SelectedValue);
                    cmd.Parameters.AddWithValue("menufoldername", txt_foldername.Text);
                    DA.ExecuteNonQuery(cmd);
                }
            }
            else
            {
                if (this.str_id == "")
                {

                    int subid = 1;
                    string str_int = "insert into TT_Menus(MenuName,pagename,menulist,Foldername,Menudescription,Parentmenuid,Menutype,Menuicon,createdby,Destinationkey,Status)values(@MenuName,@pagename,@menulistno,@menufoldername,@Menudescription,@parentmenukey,@Menutype,@Menuicon,@createdby,@Destinationkey,@Status)";

                    SqlCommand cmd = new SqlCommand(str_int);
                    cmd.Parameters.AddWithValue("Status", Convert.ToInt32(rblStatus.SelectedValue));
                    cmd.Parameters.AddWithValue("MenuName", txt_menuname.Text);
                    cmd.Parameters.AddWithValue("pagename", txt_pagename.Text);
                    cmd.Parameters.AddWithValue("menulistno", txt_menuno.Text);
                    cmd.Parameters.AddWithValue("menufoldername", txt_foldername.Text);
                    cmd.Parameters.AddWithValue("Menudescription", txt_menudesc.InnerText);
                    if (ddl_ParentMenuName.SelectedValue != "0")
                    {

                        cmd.Parameters.AddWithValue("parentmenukey", ddl_ParentMenuName.SelectedValue);
                    }
                    else
                    {
                        string input = "NoParentMenu";
                        using (MD5 md5 = MD5.Create())
                        {
                            byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                            Guid result = new Guid(hash);
                            cmd.Parameters.AddWithValue("parentmenukey", result);
                        }

                    }
                    cmd.Parameters.AddWithValue("Menutype", subid);
                    cmd.Parameters.AddWithValue("Menuicon", txt_icondesign.Text);
                    cmd.Parameters.AddWithValue("createdby", this.SC.Userid);
                    cmd.Parameters.AddWithValue("Destinationkey", ddl_desgn.SelectedValue);
                    // cmd.Parameters.AddWithValue("Mainmenu", this.chk_mnmenu.Checked ? "1" : "0");

                    DA.ExecuteNonQuery(cmd);

                }
                else
                {
                    string date = DateTime.Now.ToString();
                    string str_int = "UPDATE TT_Menus SET MenuName=@MenuName,pagename=@pagename,menulist=@menulistno,Foldername=@menufoldername,Menudescription=@Menudescription,Menuicon=@Menuicon,Parentmenuid=@Parentmenuid,Modifiedon=@Modifiedon,Destinationkey=@Destinationkey,Status=@Status WHERE MenuKey=@MenuKey;";
                    SqlCommand cmd = new SqlCommand(str_int);
                    cmd.Parameters.AddWithValue("Status", Convert.ToInt32(rblStatus.SelectedValue));
                    cmd.Parameters.AddWithValue("MenuName", txt_menuname.Text);
                    cmd.Parameters.AddWithValue("pagename", txt_pagename.Text);
                    cmd.Parameters.AddWithValue("menulistno", txt_menuno.Text);
                    cmd.Parameters.AddWithValue("menufoldername", txt_foldername.Text);
                    cmd.Parameters.AddWithValue("Menudescription", txt_menudesc.InnerText);
                    cmd.Parameters.AddWithValue("Parentmenuid", ddl_ParentMenuName.SelectedValue);
                    cmd.Parameters.AddWithValue("MenuKey", this.str_id);
                    cmd.Parameters.AddWithValue("Menuicon", txt_icondesign.Text);
                    cmd.Parameters.AddWithValue("modifiedby", this.SC.Userid);


                    cmd.Parameters.Add("@Modifiedon", SqlDbType.DateTime).Value = DateTime.Now;

                    cmd.Parameters.AddWithValue("Destinationkey", ddl_desgn.SelectedValue);

                    DA.ExecuteNonQuery(cmd);

                }
            }

            
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Zeesta", "<script>alert('Action failed please contact Team');</script>");
            return;
        }
        Response.Redirect("~/Admin/Menu.aspx");
    }

    public void assignvalues()
    {
        string str_assing = "select * from TT_Menus where MenuKey=@MenuKey;";
        SqlCommand cmd = new SqlCommand(str_assing);
        cmd.Parameters.AddWithValue("MenuKey", this.str_id);
        DataTable dt_parentmenu = this.DA.GetDataTable(cmd);
        if (dt_parentmenu.Rows.Count > 0)
        {
            txt_menuname.Text = dt_parentmenu.Rows[0]["MenuName"].ToString();
            txt_pagename.Text = dt_parentmenu.Rows[0]["pagename"].ToString();
            txt_menuno.Text = dt_parentmenu.Rows[0]["menulist"].ToString();
            txt_foldername.Text = dt_parentmenu.Rows[0]["foldername"].ToString();
            ddl_desgn.SelectedValue = dt_parentmenu.Rows[0]["Destinationkey"].ToString();
            string menuname = ddl_desgn.SelectedValue;

            BindParentMenuDropdown();

            ddl_ParentMenuName.SelectedValue = dt_parentmenu.Rows[0]["parentmenuid"].ToString();
            txt_menudesc.InnerText = dt_parentmenu.Rows[0]["Menudescription"].ToString();
            txt_icondesign.Text = dt_parentmenu.Rows[0]["Menuicon"].ToString();
          
            string str_chk= dt_parentmenu.Rows[0]["Mainmenu"].ToString();

            //if (dt_parentmenu.Rows[0]["Status"].ToString()=="true")
            //{
            //    rblStatus.SelectedValue = "1";
            //}
            //else
            //{
            //    rblStatus.SelectedValue = "0";
            //}
           rblStatus.SelectedValue = dt_parentmenu.Rows[0]["Status"].ToString();



            //if (str_chk!="")
            //{
            //    chk_mnmenu.Checked = true;

            //}

        }
    }

    private void BindParentMenuDropdown()
    {
        string menuname = ddl_desgn.SelectedValue;

        string str_des = "select menuname,menukey from TT_Menus where Menutype=0 and destinationkey='" + menuname + "' union all select menuname,menukey from TT_Menus where Menutype=0 and menukey='5EACBE17-4793-4BF6-8A9C-62272215FF9A'";
        SqlCommand cmd2 = new SqlCommand(str_des);
        DataSet ds = this.DA.GetDataSet(cmd2);

        ddl_ParentMenuName.Items.Clear();
        ddl_ParentMenuName.Items.Add(new ListItem("Select Parent Menu", "0"));

        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_ParentMenuName.DataSource = ds.Tables[0];
            ddl_ParentMenuName.DataTextField = "menuname";
            ddl_ParentMenuName.DataValueField = "menukey";
            ddl_ParentMenuName.DataBind();
        }
    }

    [WebMethod] // Check Numbers
    public static string Checkemployeeid(string str_menuno)
    {
        try
        {

            DataAccess DA1 = new DataAccess();
            SessionCustom SC=new SessionCustom();
            string strusg = SC.Userdesg;
            string str_userdesgn = "";
            string str_desg = "select * from it_destination where destinationid='" + strusg + "'";
            SqlCommand cmd1 = new SqlCommand(str_desg);
            //cmd.Parameters.AddWithValue("@Employeekey", str_userid);
            DataTable DT_desg = DA1.GetDataTable(cmd1);
            if (DT_desg.Rows.Count > 0)
            {
                str_userdesgn = DT_desg.Rows[0]["Destinationkey"].ToString();

            }


            //SqlCommand sc_username = new SqlCommand("select * from TT_Menus where menulist=@menulist and menutype=1 and  Destinationkey='" + str_userdesgn + "'");
            //sc_username.Parameters.Add(new SqlParameter("@menulist", SqlDbType.NVarChar)).Value = str_menuno;
            //DataTable dt_username = new DataAccess().GetDataTable(sc_username);

            //if (dt_username.Rows.Count > 0)
            //{
            //    return "This number already assigned try with new row number";
            //}
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }
    protected void ddl_ParentMenuName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_ParentMenuName.SelectedValue == "5eacbe17-4793-4bf6-8a9c-62272215ff9a")
        {
            txt_iconslist.Visible = true;

        }
    }

    protected void ddl_desgn_SelectedIndexChanged(object sender, EventArgs e)
    {
        string menuname = ddl_desgn.SelectedValue;

        string str_des = "select menuname,menukey from TT_Menus where Menutype=0 and destinationkey='"+ menuname + "' union all select menuname,menukey from TT_Menus where Menutype=0 and menukey='5EACBE17-4793-4BF6-8A9C-62272215FF9A'";
        SqlCommand cmd2 = new SqlCommand(str_des);
        DataSet ds = this.DA.GetDataSet(cmd2);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_ParentMenuName.DataSource = ds.Tables[0];
            ddl_ParentMenuName.DataTextField = "menuname";
            ddl_ParentMenuName.DataValueField = "menukey";
            ddl_ParentMenuName.DataBind();
            ddl_ParentMenuName.Items.Add(new ListItem("Select Parent Menu ", "0"));

        }
    }
    }