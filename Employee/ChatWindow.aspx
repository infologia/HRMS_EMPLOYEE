<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChatWindow.aspx.cs" Inherits="Employee_ChatWindow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Template/ChatProperties/css/bootstrap.min.css" rel="stylesheet">
    <link href="../Template/ChatProperties/css/messsages.css" rel="stylesheet">
    <link href="../Template/ChatProperties/./fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <style type="text/css">
        #ms-scrollbar::-webkit-scrollbar-track {
            background-color: #CCCCCC;
        }

        #ms-scrollbar::-webkit-scrollbar {
            width: 7px;
            background-color: #F5F5F5;
        }

        #ms-scrollbar::-webkit-scrollbar-thumb {
            background-color: #eeeeee;
            -webkit-box-shadow: inset 0 0 0px rgba(0,0,0,0.3);
        }

        .ms-new {
            box-shadow: 0 2px 5px rgba(0,0,0,0.16),0 2px 10px rgba(0,0,0,0.12);
            background-color: #2196f3;
        }

        .back-btn {
            position: absolute;
            top: 15px;
            left: 15px;
            background-color: #007BFF;
            color: white;
            padding: 10px 18px;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            cursor: pointer;
            box-shadow: 0 3px 6px rgba(0,0,0,0.2);
            transition: 0.2s ease;
            justify-items:right;
        }

            .back-btn:hover {
                background-color: #0056b3;
                transform: translateY(-2px);
            }
    </style>
    <script src="jQuery-2.1.4.min.js"></script>
    <!-- jQuery UI 1.11.4 -->
    <script src="https://code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //function SetScrollPosition() {
        //    var div = document.getElementById('div.lv-body');
        //    div.scrollTop = 100000000000;
        //}
        //<![CDATA[
        //$(window).load(function () {
        //    $(".chat").animate({ scrollTop: $(document).height() }, "slow");
        //    return false;
        //});//]]> 
        //var $cont = $('.chat');
        //$cont[0].scrollTop = $cont[0].scrollHeight;
        window.onload = function () {
            $("div.lv-body").scrollTop(10000);
        };
        //function scroll() {

        //    var objDiv = document.getElementById('div.lv-body');
        //        objDiv.scrollTop = objDiv.scrollHeight;
        //       setTimeout("scroll()", 1);
        //}
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div class="container-fluid">
                <div class="container ng-scope">
                    <div class="block-header">

                        <h2></h2>

                    </div>
                    <div class="card m-b-0" id="messages-main" style="box-shadow: 0 0 40px 1px #c9cccd;">
                        <div class="ms-menu" style="overflow: scroll; overflow-x: hidden;" id="ms-scrollbar">
                            <div class="ms-block">
                                <div class="ms-user">
                                    <%--<img src="./images/avatar.jpg" alt=""> --%>
                                    <asp:Image ID="Image1" runat="server" />
                                    <h5 class="q-title" align="center">
                                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                                        <br />
                                        <asp:Label ID="lbl_time" runat="server"></asp:Label></h5>
                                </div>
                            </div>

                            <asp:Button ID="BtnBack" runat="server" Text="Back"
                                CssClass="back-btn" OnClick="BtnBack_Click" />
                            <div class="ms-block">
                                <a class="btn btn-primary btn-block ms-new" href="#"><span class="glyphicon glyphicon-envelope"></span>&nbsp; New Message</a>
                            </div>
                            <hr />
                            <div class="listview lv-user m-t-20">
                                <asp:DataList ID="DataList1" runat="server">
                                    <ItemTemplate>
                                        <div class="lv-item media">
                                            <div class="lv-avatar pull-left">
                                                <%--<img src="./images/bhai.jpg" alt=""> --%>
                                                <asp:Image ID="Image2" runat="server" ImageUrl='<%# Bind("Image","~/images/Employeeprofilepictures/{0}") %>' />
                                            </div>
                                            <div class="media-body">
                                                <div class="lv-title">
                                                    <%--<asp:Label ID="Label2" runat="server" Text='<%# Bind("Name") %>'></asp:Label>--%>
                                                </div>
                                                <asp:LinkButton ID="LinkButton1" ForeColor="Black" runat="server" Text='<%# Bind("username") %>' OnClick="LinkButton1_Click"></asp:LinkButton>
                                                <%--<div class="lv-small"> Acadnote a world class website is processing surveys for </div>--%>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>

                                <%--<div class="lv-item media"> 
                                      <div class="lv-avatar pull-left"> <img src="./images/ajit.jpg" alt=""> </div>
                                      <div class="media-body"> 
                                          <div class="lv-title"><b>Ajit Gupta</b><span class="pull-right">10 new</div>
                                          <div class="lv-small"><b>Hello bro whatsup , how are you</b></div>
                                      </div>
                                  </div>
                                  <div class="lv-item media"> 
                                      <div class="lv-avatar pull-left"> <img src="./images/chota.jpg" alt=""> </div>
                                      <div class="media-body"> 
                                          <div class="lv-title"><b>Deepak Yadav</b><span class="pull-right">2 new</span></div>
                                          <div class="lv-small"><b>aur bhai collage kse chale rhai hai </b></div>
                                      </div>
                                  </div><div class="lv-item media"> <div class="lv-avatar pull-left"> <img src="./images/sumit.jpg" alt=""> </div><div class="media-body"> <div class="lv-title">Sumit kumar</div><div class="lv-small">aur suna kya haal hai bhai, aur</div></div></div>
                                  <div class="lv-item media"> <div class="lv-avatar pull-left"> <img src="./images/sega.jpg" alt=""> </div><div class="media-body"> <div class="lv-title">Sage Kalia</div><div class="lv-small">abey kaha chala gya ?? mar gya kya ??</div></div></div>
                                  <div class="lv-item media"> <div class="lv-avatar pull-left"> <img src="./images/gan.jpg" alt=""> </div><div class="media-body"> <div class="lv-title">Gagandeep Singh</div><div class="lv-small">yeh ley eamil address sachin.yadav1212@gmail.com</div></div></div>
                                  <div class="lv-item media"> <div class="lv-avatar pull-left"> <img src="./images/vasu.jpg" alt=""> </div><div class="media-body"> <div class="lv-title">Vasu</div><div class="lv-small">kal se classess start hai koi holiday nahi hai </div></div></div>
                                  <div class="lv-item media"> <div class="lv-avatar pull-left"> <img src="./images/abc.jpg" alt=""> </div><div class="media-body"> <div class="lv-title">Deepu Singh</div><div class="lv-small">okk byee gud night dude kal baaat karte hai </div></div></div>--%>
                            </div>
                        </div>
                        <div class="ms-body">
                            <div class="listview lv-message">
                                <div class="lv-header-alt clearfix">
                                    <div id="ms-menu-trigger">
                                        <div class="line-wrap">
                                            <div class="line top">
                                            </div>
                                            <div class="line center"></div>
                                            <div class="line bottom"></div>
                                        </div>
                                    </div>
                                    <div class="lvh-label hidden-xs">
                                        <div class="lv-avatar pull-left">
                                            <%--<img src="./images/bhai.jpg" alt="">--%>
                                            <asp:Image ID="Image3" runat="server" />
                                        </div>
                                        <span class="c-black">
                                            <asp:Label ID="Label2" runat="server" Text=""></asp:Label><span style="margin-left: 8px; position: absolute; margin-top: 12px; width: 8px; height: 8px; line-height: 8px; border-radius: 50%; background-color: #fff;"></span></span>
                                    </div>
                                    <%--<ul class="lv-actions actions list-unstyled list-inline"> <li> <a href="#" > <i class="fa fa-check"></i> </a> </li><li> <a href="#" > <i class="fa fa-clock-o"></i> </a> </li><li> <a data-toggle="dropdown" href="#" > <i class="fa fa-list"></i></a> <ul class="dropdown-menu user-detail" role="menu"> <li> <a href="">Latest</a> </li><li> <a href="">Oldest</a> </li></ul> </li><li> <a data-toggle="dropdown" href="#" data-toggle="tooltip" data-placement="left" title="Tooltip on left"><span class="glyphicon glyphicon-trash"></span></a> <ul class="dropdown-menu user-detail" role="menu"> <li> <a href="">Delete Messages</a> </li></ul> </li></ul>--%>
                                </div>
                                <div class="lv-body" id="Div1" style="overflow: scroll; overflow-x: hidden; height: 520px;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000"></asp:Timer>
                                            <asp:DataList ID="DataList3" runat="server">
                                                <ItemTemplate>
                                                    <div class="lv-item media">
                                                        <div class="lv-avatar pull-left">
                                                            <%--<img src="./images/bhai.jpg" alt="">--%>
                                                            <asp:Image ID="Image4" runat="server" ImageUrl='<%# Bind("Image") %>' />

                                                        </div>
                                                        <div class="media-body">
                                                            <div class="ms-item">
                                                                <span class="glyphicon glyphicon-triangle-left" style="color: #000000;"></span>
                                                                <asp:Label ID="Message" runat="server" Text='<%# Bind("Message") %>'></asp:Label>
                                                            </div>
                                                            <small class="ms-date"><span class="glyphicon glyphicon-time"></span>&nbsp;
                                                                <asp:Label ID="Date" runat="server" Text='<%# Bind("Time") %>'></asp:Label></small>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--<div class="lv-item media right"> <div class="lv-avatar pull-right"> <img src="./images/avatar.jpg" alt=""> </div><div class="media-body"> <div class="ms-item"> We started this site with clear mission that we want to deliver complete details knowledge of Programming to our audience. We are sharing this knowledge in all areas that you can see in our site. </div><small class="ms-date"><span class="glyphicon glyphicon-time"></span>&nbsp; 05/10/2015 at 09:30</small> </div></div>
                                     <div class="lv-item media"> <div class="lv-avatar pull-left"> <img src="./images/bhai.jpg" alt=""> </div><div class="media-body"> <div class="ms-item"> It's gives the power to synthesis anything anywhere you want to. Its the ultimate tool to solve any problem. And we help you excel in that by working with you. </div><small class="ms-date"><span class="glyphicon glyphicon-time"></span>&nbsp; 20/02/2015 at 09:33</small> </div></div>
                                     <div class="lv-item media right"> <div class="lv-avatar pull-right"> <img src="./images/avatar.jpg" alt=""> </div><div class="media-body"> <div class="ms-item"> The basic essence of life is to learn, explore and synthesis. We provide you with the tools to make your dreams come true.Our website is totally for free and available 24/7 and does not consume your data packs and works like a charm on the supersonic lovely internet. </div><small class="ms-date"><span class="glyphicon glyphicon-time"></span>&nbsp; 05/10/2015 at 10:10</small> </div></div>
                                     <div class="lv-item media"> <div class="lv-avatar pull-left"> <img src="./images/bhai.jpg" alt=""> </div><div class="media-body"> <div class="ms-item"> Acadnote a world class website is processing surveys for every student who wants to do something new and different in the field of academics. so it is a right place for every student to share their opinions about their present academics so this website can provide every single student requirements and it is possible for us to do if every student explains about their academics requirements. Last but not the least tell the needs and collect your study materials which we will provide to you. </div><small class="ms-date"><span class="glyphicon glyphicon-time"></span>&nbsp; 05/10/2015 at 10:24</small> </div></div>--%>
                                </div>
                                <div class="clearfix"></div>
                                <div class="lv-footer ms-reply">
                                    <%--<form runat="server">--%>
                                    <%--<textarea rows="10" placeholder="Write messages..."></textarea>--%>
                                    <asp:TextBox CssClass="textarea" ID="TextBox1" placeholder="Write messages..." runat="server"></asp:TextBox>
                                    <%--<button class=""><span class="glyphicon glyphicon-send"></span></button>--%>
                                    <%--<asp:LinkButton ID="Button1" CssClass="button" runat="server" ><span class="glyphicon glyphicon-send"></span></asp:LinkButton>--%>
                                    <button id="Button1" runat="server" class="button" onserverclick="Unnamed_ServerClick" height="" width="">
                                        <span class="glyphicon glyphicon-send"></span>
                                    </button>


                                    <%--</form>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--</div></div></section>--%>

            <script type="text/javascript" src="../Template/ChatProperties/css/jquery.js"></script>
            <script src="../Template/ChatProperties/css/bootstrap.min.js"></script>
        </div>

    </form>
</body>
</html>
