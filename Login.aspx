<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Logical HR - login</title>

    <!-- Global stylesheets -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css">
    <link href="Template/assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css">
    <link href="Template/assets/css/bootstrap.css" rel="stylesheet" type="text/css">
    <link href="Template/assets/css/core.css" rel="stylesheet" type="text/css">
    <link href="Template/assets/css/components.css" rel="stylesheet" type="text/css">
    <link href="Template/assets/css/colors.css" rel="stylesheet" type="text/css">
    <!-- /global stylesheets -->

    <!-- Core JS files -->
    <script type="text/javascript" src="Template/assets/js/plugins/loaders/pace.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/core/libraries/jquery.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/core/libraries/bootstrap.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/plugins/loaders/blockui.min.js"></script>
    <!-- /core JS files -->

    <!-- Theme JS files -->
    <script type="text/javascript" src="Template/assets/js/plugins/forms/validation/validate.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/plugins/forms/styling/uniform.min.js"></script>

    <script type="text/javascript" src="Template/assets/js/core/app.js"></script>
    <script type="text/javascript" src="Template/assets/js/pages/login_validation.js"></script>
    <!-- /theme JS files -->
</head>
<body class="login-container login-cover">
    <form id="form1" runat="server">
        <div class="page-container">

            <!-- Page content -->
            <div class="page-content">

                <!-- Main content -->
                <div class="content-wrapper">

                    <!-- Content area -->
                    <div class="content pb-20">

                        <!-- Form with validation -->
                        <div action="#" class="form-validate">
                            <div class="panel panel-body login-form">
                                <div class="text-center">
                                    <img src="../Template/assets/images/infologiaglobe.png" class="img-circle img-lg" />
                                    <%-- <div class="icon-object border-slate-300 text-slate-300"><i class="icon-reading"></i></div>--%>
                                    <h5 class="content-group">Login to your account <small class="display-block">Your credentials</small></h5>
                                </div>

                                <div class="form-group has-feedback has-feedback-left">
                                    <%--<input type="text" class="form-control" placeholder="Username" name="username" required="required">--%>
                                    <asp:TextBox ID="txt_Uname" runat="server" CssClass="form-control" required=""></asp:TextBox>
                                    <div class="form-control-feedback">
                                        <i class="icon-user text-muted"></i>
                                    </div>
                                </div>

                                <div class="form-group has-feedback has-feedback-left">
                                    <%--	<input type="password" class="form-control" placeholder="Password" name="password" required="required">
                                    --%>
                                    <asp:TextBox ID="txt_Pwd" runat="server" TextMode="Password" CssClass="form-control" required=""></asp:TextBox>
                                    <div class="form-control-feedback">
                                        <i class="icon-lock2 text-muted"></i>
                                    </div>
                                </div>

                                <div class="form-group login-options">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <label class="checkbox-inline">
                                                <input type="checkbox" class="styled" checked="checked">
                                                Remember
									
                                            </label>
                                        </div>

                                        <div class="col-sm-6 text-right">
                                            <a href="ForgotPassword.aspx">Forgot password?</a>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <%--	<button type="submit" class="btn bg-blue btn-block">Login <i class="icon-arrow-right14 position-right"></i></button>
                                    --%>

                                    <asp:Button ID="btn_Submit" runat="server" CssClass="btn btn-primary btn-block" Text="Login" OnClick="btn_Submit_Click" />

                                </div>

                                <div id="div_error" runat="server" class="alert alert-danger" visible="false">
                                    <button type="button" class="close" data-dismiss="alert"><span>&times;</span><span class="sr-only">Close</span></button>
                                    <asp:Label ID="lbl_error" runat="server"></asp:Label>
                                </div>



                                <div class="content-divider text-muted form-group"><span>Follow Us</span></div>
                                <ul class="list-inline form-group list-inline-condensed text-center">
                                    <li><a href="https://www.facebook.com/infologiatechnologies/" target="_blank" class="btn border-indigo text-indigo btn-flat btn-icon btn-rounded"><i class="icon-facebook"></i></a></li>
                                    <li><a href="https://www.instagram.com/infologiatechnologies/?hl=en" target="_blank" class="btn border-pink-300 text-pink-300 btn-flat btn-icon btn-rounded"><i class="icon-instagram"></i></a></li>
                                    <li><a href="https://in.linkedin.com/company/infologia" target="_blank" class="btn border-slate-600 text-slate-600 btn-flat btn-icon btn-rounded"><i class="icon-linkedin"></i></a></li>
                                    <%--<li><a href="#" class="btn border-info text-info btn-flat btn-icon btn-rounded"><i class="icon-word"></i></a></li>--%>
                                </ul>

                                <div class="content-divider text-muted form-group"><span>Welcome to infologia</span></div>
                                <%--<a href="login_registration.html" class="btn btn-default btn-block content-group">Sign up</a>--%>
                                <span class="help-block text-center no-margin">By continuing, you're confirming that you've read our <a href="https://infologia.in/terms">Terms &amp; Conditions</a> and <a href="https://infologia.in/privacy">Cookie Policy</a></span>
                            </div>
                        </div>
                        <!-- /form with validation -->

                    </div>
                    <!-- /content area -->

                </div>
                <!-- /main content -->

            </div>
            <!-- /page content -->

        </div>
    </form>
</body>
</html>

