<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LinkExpired.aspx.cs" Inherits="LinkExpired" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="SHORTCUT ICON" href="Template/assets/images/favicon.png" />
    <title>Infologia link expired</title>

    <!-- Global stylesheets -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/core.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/components.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/colors.css" rel="stylesheet" type="text/css" />
    <!-- /global stylesheets -->

    <!-- Core JS files -->
    <script type="text/javascript" src="Template/assets/js/plugins/loaders/pace.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/core/libraries/jquery.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/core/libraries/bootstrap.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/plugins/loaders/blockui.min.js"></script>
    <!-- /core JS files -->
    <script type="text/javascript" src="Template/assets/js/core/app.js"></script>

    <style>
        .header-colour {
            background-color: teal !important;
        }

        .navbar-brand {
            display: flex;
            align-items: center;
            gap: 8px;
            padding: 0 15px;
        }

            .navbar-brand .brand-icon {
                height: 35px;
                width: auto;
                margin-top: -4px;
            }

        .brand-text {
            color: #ffffff;
            font-size: 15px;
            font-weight: 600;
            line-height: 1;
            white-space: nowrap;
            display: inline-block;
        }
    </style>
</head>
<body class="login-container">
    <form id="form2" runat="server">
        <div class="navbar navbar-inverse header-colour">
            <div class="navbar-header">
                <a class="navbar-brand" href="Default.aspx">
                    <img src="../Template/assets/images/infologiaglobe.png" class="img-circle img-lg" />
                    <span class="brand-text">INFOLOGIA</span></a>
            </div>
        </div>
        <div class="page-container">
            <div class="page-content">
                <div class="content-wrapper">
                    <div class="content">
                        <div class="text-center content-group">
                            <h1 class="error-title" style="font-size:100px;">Expried !!!</h1>
                            <h5>Your link has been expired please try again!</h5>
                        </div>
                        <div class="row">
                            <div class="col-lg-4 col-lg-offset-4 col-sm-6 col-sm-offset-3">
                                <div action="#" class="main-search">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <a href="Login.aspx" class="btn btn-primary btn-block content-group"><i class="icon-circle-left2 position-left"></i>Login</a>
                                        </div>
                                        <div class="col-sm-6">
                                            <a href="ForgotPassword.aspx" class="btn btn-primary btn-block content-group">Forgot Password <i class="icon-circle-right2 position-right"></i></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="footer text-muted text-center">
                            &copy; 2026. <a href="#">Logiacal HR</a> by <a href="https://infologia.in/" target="_blank">Infologia Technologies</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
