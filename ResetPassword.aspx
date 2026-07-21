<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="SHORTCUT ICON" href="Template/assets/images/favicon.png" />
    <title>Infologia Reset Password</title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/core.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/components.css" rel="stylesheet" type="text/css" />
    <link href="Template/assets/css/colors.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Template/assets/js/plugins/loaders/pace.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/core/libraries/jquery.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/core/libraries/bootstrap.min.js"></script>
    <script type="text/javascript" src="Template/assets/js/plugins/loaders/blockui.min.js"></script>
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
                    <div class="panel panel-body login-form" style="margin-top: 26px;">

                        <div class="text-center">
                            <img src="Template/assets/images/infologiaglobe.png" alt="" class="img-circle img-lg" />
                            <h5 class="content-group">Reset Your Password <small class="display-block"></small></h5>
                        </div>

                        <div class="form-group has-feedback has-feedback-left">
                            <asp:TextBox ID="txt_Pass" runat="server" CssClass="form-control" required="" TextMode="Password" placeholder="New Password"></asp:TextBox>
                            <div class="form-control-feedback">
                                <i class="icon-lock2 text-muted"></i>
                            </div>
                        </div>
                        <div class="form-group has-feedback has-feedback-left">
                            <asp:TextBox ID="txt_newpass" runat="server" CssClass="form-control" required="" TextMode="Password" placeholder="Confirm New Password"></asp:TextBox>

                            <div class="form-control-feedback">
                                <i class="icon-lock2 text-muted"></i>
                            </div>
                        </div>
                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                            ControlToValidate="txt_newpass"
                            CssClass="ValidationError"
                            ControlToCompare="txt_Pass"
                            ErrorMessage="Password must be the same"
                            ForeColor="Red" />

                        <div class="form-group login-options">
                            <div class="row">
                                <div class="col-sm-6">
                                </div>
                                <div class="col-sm-6 text-right">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btn_Submit" runat="server" CssClass="btn btn-primary btn-block" Text="Submit" OnClick="btn_Submit_Click" />
                        </div>
                        <div class=" text-center">
                            <h6 class="m-t-20"><a href="ForgotPassword.aspx" class="link">Forgot password?</a></h6>
                        </div>
                         <div class=" text-center">
                            <h6 class="m-t-20"><a href="Login.aspx" class="link">Login?</a></h6>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
