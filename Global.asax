<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        //string line;
        //System.IO.StreamReader file =
        //   new System.IO.StreamReader(@"c:\windows\infologia\InfologiaAll.txt");
        //while ((line = file.ReadLine()) != null)
        //{
        //    if (line.Split(':')[0] == "webroot") Application["webroot"] = line.Replace("webroot:", "").Trim();
        //    if (line.Split(':')[0] == "dbconnect") Application["dbconnect"] = line.Split(':')[1];
        //    if (line.Split(':')[0] == "importconnect") Application["importconnect"] = line.Split(':')[1];

        //}

        //file.Close();

        Application["dbconnect"] = "Data Source=103.197.121.228,1433;Initial Catalog=InfologiaInternalTool;User ID=sa;Password=Data@2023$";
        Application["importconnect"] = "Data Source=103.197.121.228,1433;Initial Catalog=InfologiaInternalTool;User ID=sa;Password=Data@2023$";
        Application["webroot"] = "http://13.126.196.138/IndianRecipes/Web/adminloginweb.aspx";

        //Application["dbconnect"] = "Data Source=DESKTOP-93GBF2B\\MSSQLSERVER_RAFE;Initial Catalog=InfologiaInternalTool;User ID=sa;Password=Infologia_1";
        //Application["importconnect"] = "Data Source=DESKTOP-93GBF2B\\MSSQLSERVER_RAFE;Initial Catalog=InfologiaInternalTool;User ID=sa;Password=Infologia_1";
        //Application["webroot"] = "http://13.126.196.138/InfologiaInternalTool/InfologiaInternalTool/Default.aspx";


        //Application["IvrUser"] = "cguwill";
        //Application["IvrPassword"] = "Power@56";
        Application["email"] = "memesworldnetwork@gmail.com";
        Application["emaildisplayname"] = "Meme’s World";

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
      //  HttpContext.Current.Response.Redirect("~/Default.aspx");

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
