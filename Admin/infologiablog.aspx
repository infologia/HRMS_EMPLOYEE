<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="infologiablog.aspx.cs" Inherits="Admin_infologiablog"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style>
        .font-size-wrapper {
            position: relative;
            display: inline-block;
        }

        .editor-input-number {
            width: 55px;
            height: 30px;
            font-size: 12px;
            border: 1px solid #d5d9e0;
            border-radius: 4px;
            padding: 0 6px;
            text-align: center;
        }

            .editor-input-number:focus {
                outline: none;
                border-color: #93c5fd;
            }

        .font-size-dropdown {
            display: none;
            position: absolute;
            top: 32px;
            left: 0;
            width: 55px;
            max-height: 180px;
            overflow-y: auto;
            background: #fff;
            border: 1px solid #d5d9e0;
            border-radius: 4px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.15);
            z-index: 1000;
        }

            .font-size-dropdown.show {
                display: block;
            }

        .font-size-option {
            padding: 5px 8px;
            font-size: 12px;
            cursor: pointer;
            text-align: center;
        }

            .font-size-option:hover {
                background: #e9ecef;
            }

        .simple-editor-wrapper {
            border: 1px solid #d5d9e0;
            border-radius: 6px;
            overflow: hidden;
            background: #fff;
        }

        .simple-editor-toolbar {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 4px;
            padding: 6px 8px;
            background: #f8f9fb;
            border-bottom: 1px solid #d5d9e0;
        }

        .editor-btn {
            min-width: 30px;
            height: 30px;
            padding: 0 6px;
            font-size: 12px;
            border: 1px solid transparent;
            background: transparent;
            border-radius: 4px;
            cursor: pointer;
            color: #333;
        }

            .editor-btn:hover {
                background: #e9ecef;
                border-color: #d5d9e0;
            }

            .editor-btn.active {
                background: #dbeafe;
                border-color: #93c5fd;
                color: #1d4ed8;
            }

        .editor-select {
            height: 30px;
            font-size: 12px;
            border: 1px solid #d5d9e0;
            border-radius: 4px;
            background: #fff;
            padding: 0 4px;
        }

        .editor-sep {
            width: 1px;
            height: 20px;
            background: #d5d9e0;
            margin: 0 4px;
        }

        .simple-editor-content {
            min-height: 220px;
            max-height: 500px;
            overflow-y: auto;
            padding: 10px 12px;
            font-size: 13px;
            line-height: 1.5;
            outline: none;
        }

            .simple-editor-content:empty:before {
                content: attr(data-placeholder);
                color: #9aa0a6;
            }

            .simple-editor-content h1 {
                font-size: 22px;
                margin: 8px 0;
            }

            .simple-editor-content h2 {
                font-size: 19px;
                margin: 6px 0;
            }

            .simple-editor-content h3 {
                font-size: 16px;
                margin: 5px 0;
            }

            .simple-editor-content h4 {
                font-size: 14px;
                margin: 4px 0;
            }

            .simple-editor-content h5 {
                font-size: 12px;
                margin: 4px 0;
            }

            .simple-editor-content h6 {
                font-size: 11px;
                margin: 3px 0;
                color: #555;
            }

            .simple-editor-content blockquote {
                border-left: 3px solid #ccc;
                margin: 6px 0;
                padding-left: 10px;
                color: #555;
            }
    </style>
    <style>
        /* ===== Page header (title + breadcrumb strip, sits above the form panel) ===== */
        .blog-page-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 10px;
            margin: 0 0 12px;
        }

            .blog-page-header .title-block h1 {
                margin: 0;
                font-size: 19px;
                font-weight: 700;
                color: #23324a;
                letter-spacing: .2px;
            }

            .blog-page-header .title-block p {
                margin: 2px 0 0;
                font-size: 12px;
                color: #8b95a5;
            }

        /* ===== Page shell ===== */
        .blog-form-section {
            border: none;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 4px 20px rgba(30, 45, 70, .09);
            background: #fff;
        }

            .blog-form-section .panel-heading {
                background: #fff;
                border-bottom: 1px solid #eef1f5;
                padding: 12px 20px;
                margin: 0 0 4px;
                display: flex;
                align-items: center;
                justify-content: flex-end;
            }

        .btn-back-list {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: #f2f6fc;
            color: #4a90d9;
            border: 1px solid #e1ebf7;
            border-radius: 18px;
            padding: 6px 14px;
            font-weight: 600;
            font-size: 12px;
            transition: all .15s ease-in-out;
        }

            .btn-back-list i {
                font-size: 11px;
            }

            .btn-back-list:hover,
            .btn-back-list:focus {
                color: #fff;
                background: #4a90d9;
                border-color: #4a90d9;
                text-decoration: none;
                transform: translateX(-2px);
            }

        .blog-form-section .panel-body {
            padding: 6px 20px 18px;
            background: #fff;
        }

        /* ===== Section headers inside the form ===== */
        .form-section-title {
            font-size: 13px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: .5px;
            color: #4a90d9;
            margin: 0 0 16px;
            padding-bottom: 8px;
            border-bottom: 2px solid #e8eef7;
            display: flex;
            align-items: center;
            gap: 8px;
        }

            .form-section-title:not(:first-child) {
                margin-top: 30px;
            }

        /* ===== Inputs ===== */
        .form-group {
            margin-bottom: 14px;
        }

            .form-group label {
                font-weight: 600;
                color: #33404f;
                font-size: 12.5px;
                margin-bottom: 4px;
                display: inline-block;
            }

        .form-control {
            width: 100%;
            height: 34px;
            border: 1px solid #dde3ec;
            border-radius: 6px;
            padding: 6px 10px;
            font-size: 12.5px;
            box-shadow: none;
            background: #fbfcfe;
            transition: border-color .15s ease-in-out, box-shadow .15s ease-in-out, background .15s ease-in-out;
        }

            .form-control:focus {
                border-color: #4a90d9;
                background: #fff;
                box-shadow: 0 0 0 3px rgba(74,144,217,.12);
            }

        textarea.form-control {
            height: auto;
            min-height: 34px;
            resize: vertical;
        }

        .field-hint {
            font-size: 11px;
            color: #9aa5b1;
            margin-top: 3px;
            display: block;
        }

        .char-counter {
            font-size: 11px;
            color: #b3bcc6;
            float: right;
        }

        .required-star {
            color: #e2543a;
        }

        .blog-form-section .panel-body .row {
            margin-bottom: 0;
        }

        /* ===== File upload fields (styled to match the other form-control inputs) ===== */
        .upload-card {
            position: relative;
            height: 34px;
            border: 1px solid #dde3ec;
            border-radius: 6px;
            background: #fbfcfe;
            display: flex;
            align-items: center;
            padding: 0 8px;
            overflow: hidden;
            transition: border-color .15s ease-in-out, background .15s ease-in-out, box-shadow .15s ease-in-out;
        }

            .upload-card:hover {
                border-color: #4a90d9;
                background: #fff;
                box-shadow: 0 0 0 3px rgba(74,144,217,.08);
            }

            .upload-card input[type=file] {
                border: none;
                padding: 0;
                font-size: 11.5px;
                width: 100%;
                background: transparent;
                cursor: pointer;
            }

                .upload-card input[type=file]::-webkit-file-upload-button,
                .upload-card input[type=file]::file-selector-button {
                    border: 1px solid #dde3ec;
                    background: #fff;
                    color: #4a5568;
                    border-radius: 5px;
                    padding: 3px 8px;
                    font-size: 11px;
                    font-weight: 600;
                    margin-right: 8px;
                    cursor: pointer;
                    transition: all .15s ease-in-out;
                }

                .upload-card input[type=file]:hover::-webkit-file-upload-button,
                .upload-card input[type=file]:hover::file-selector-button {
                    background: #4a90d9;
                    border-color: #4a90d9;
                    color: #fff;
                }

        .image-preview-box {
            width: 100%;
            max-width: 100%;
            height: 58px;
            border: 1px dashed #e5e9f0;
            border-radius: 6px;
            display: none;
            align-items: center;
            justify-content: center;
            background: #f9fafc;
            margin-top: 6px;
            overflow: hidden;
        }

            .image-preview-box.has-image {
                display: flex;
                border-style: solid;
            }

            .image-preview-box img {
                max-width: 100%;
                max-height: 100%;
                object-fit: cover;
            }

            .image-preview-box span {
                color: #b3bcc6;
                font-size: 11px;
            }

        /* ===== Summernote editor ===== */
        .note-editor.note-frame {
            border: 1px solid #dde3ec;
            border-radius: 6px;
            overflow: hidden;
        }

            .note-editor.note-frame .note-toolbar {
                background: #f5f7fa;
                border-bottom: 1px solid #e5e9f0;
                padding: 6px 8px;
            }

            .note-editor.note-frame .note-editable {
                min-height: 280px;
                padding: 18px;
                font-size: 14px;
                line-height: 1.8;
                color: #333;
                text-align: justify;
            }

            .note-editor.note-frame.fullscreen {
                z-index: 9999;
            }

            /* ===== Toolbar wraps responsively as a modern CMS editor ===== */
            .note-editor.note-frame .note-toolbar {
                display: flex;
                flex-wrap: wrap;
                row-gap: 4px;
            }

        /* ===== Word count status bar under the editor ===== */
        .blog-wordcount-bar {
            text-align: right;
            font-size: 11.5px;
            color: #9aa5b1;
            padding: 4px 2px 0;
        }

        /* ===== Special characters / emoji picker dropdown ===== */
        .note-picker-dropdown {
            padding: 10px;
            min-width: 220px;
        }

        .note-picker-grid {
            display: grid;
            grid-template-columns: repeat(8, 1fr);
            gap: 4px;
        }

        .note-picker-item {
            display: flex;
            align-items: center;
            justify-content: center;
            height: 28px;
            border-radius: 4px;
            font-size: 15px;
            color: #33404f;
            text-decoration: none;
        }

            .note-picker-item:hover {
                background: #eef2f9;
                text-decoration: none;
            }

        /* ===== Find & Replace popover ===== */
        .note-findreplace-dropdown {
            padding: 12px;
            min-width: 220px;
        }

        .note-findreplace-form input {
            width: 100%;
            margin-bottom: 8px;
        }

        .note-findreplace-form .note-fr-go {
            width: 100%;
        }

        /* ===== Preview overlay ===== */
        .blog-preview-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(20, 28, 40, .55);
            z-index: 10050;
            align-items: center;
            justify-content: center;
        }

        .blog-preview-panel {
            background: #fff;
            width: 90%;
            max-width: 780px;
            max-height: 85vh;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 10px 40px rgba(0,0,0,.25);
            display: flex;
            flex-direction: column;
        }

        .blog-preview-header {
            padding: 14px 20px;
            border-bottom: 1px solid #e8eef7;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

            .blog-preview-header strong {
                font-size: 14.5px;
                color: #33404f;
            }

        .blog-preview-close {
            border: none;
            background: none;
            font-size: 20px;
            line-height: 1;
            color: #9aa5b1;
            cursor: pointer;
        }

            .blog-preview-close:hover {
                color: #33404f;
            }

        .blog-preview-body {
            padding: 24px;
            overflow-y: auto;
            text-align: justify;
            line-height: 1.8;
            font-size: 14.5px;
            color: #333;
        }

            .blog-preview-body img {
                max-width: 100%;
                height: auto;
                border-radius: 6px;
            }

        #blogContentError {
            color: #e2543a;
            font-size: 12px;
            margin-top: 4px;
            display: none;
        }

        /* ===== Sticky action bar ===== */
        .blog-action-bar {
            position: sticky;
            bottom: 0;
            background: #fff;
            border-top: 1px solid #e8eef7;
            margin: 16px -20px -18px;
            padding: 12px 20px;
            display: flex;
            justify-content: flex-end;
            gap: 8px;
        }

            .blog-action-bar .btn {
                border-radius: 6px;
                padding: 7px 18px;
                font-weight: 600;
                font-size: 12.5px;
            }

        .btn-primary {
            background: #4a90d9;
            border-color: #4a90d9;
        }

            .btn-primary:hover {
                background: #357abd;
                border-color: #357abd;
            }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hfBlogKey" runat="server" />

    <!-- ===== PAGE HEADER ===== -->
    <div class="blog-page-header">
        <div class="title-block">
            <h1>Blog Creation Page</h1>
            <p>Fill in the details below to create or update a blog post.</p>
        </div>
    </div>

    <!-- ===== FORM PANEL ===== -->
    <div class="panel panel-flat blog-form-section">
        <div class="panel-heading">
            <a href="infologiablogs.aspx" class="btn-back-list">
                <i class="icon-arrow-left8 position-left"></i>Back
            </a>
        </div>

        <div class="panel-body">
            <br />
            <!-- Row 1: Blog Title | Blog Creator | Schedule Publish Date -->
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Blog Title <span class="required-star">*</span></label>
                        <asp:TextBox ID="txtBlogTitle" runat="server" CssClass="form-control"
                            placeholder="Enter blog title" MaxLength="500" />
                        <asp:RequiredFieldValidator ID="rfvBlogTitle" runat="server"
                            ControlToValidate="txtBlogTitle"
                            ErrorMessage="Blog Title is required."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Blog Creator <span class="required-star">*</span></label>
                        <asp:TextBox ID="txtBlogCreator" runat="server" CssClass="form-control"
                            placeholder="Enter creator name" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvBlogCreator" runat="server"
                            ControlToValidate="txtBlogCreator"
                            ErrorMessage="Blog Creator is required."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Schedule Publish Date <span class="required-star">*</span></label>
                        <asp:TextBox ID="txtSchedulePublishDate" runat="server" CssClass="form-control"
                            TextMode="Date" />
                        <asp:RequiredFieldValidator ID="rfvScheduleDate" runat="server"
                            ControlToValidate="txtSchedulePublishDate"
                            ErrorMessage="Schedule Publish Date is required."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
            </div>

            <!-- Row 2: Flag | Key Words | Description -->
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Flag <span class="required-star">*</span></label>
                        <asp:DropDownList ID="ddlFlag" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFlag" runat="server"
                            ControlToValidate="ddlFlag"
                            InitialValue=""
                            ErrorMessage="Please select a Flag."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Key Words</label>
                        <asp:TextBox ID="txtKeyWords" runat="server" CssClass="form-control"
                            placeholder="Enter key Words here..." MaxLength="1000" />
                        <span class="field-hint">Comma separated, used for search &amp; SEO tags.</span>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Description <span class="required-star">*</span></label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"
                            TextMode="MultiLine" Rows="1"
                            placeholder="Enter description here..." MaxLength="1000" />
                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server"
                            ControlToValidate="txtDescription"
                            ErrorMessage="Description is required."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
            </div>

            <!-- Row 3: Small Image | Blog Image | Title -->
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Small Image</label>
                        <div class="upload-card">
                            <input type="file" id="fuSmallImage" runat="server"
                                accept="image/*" class="form-control" />
                        </div>
                        <span class="field-hint">Thumbnail used in blog listing cards.</span>
                        <div class="image-preview-box" id="smallImagePreviewBox">
                            <img id="smallImagePreviewImg" src="" alt="Small image preview" />
                        </div>
                        <asp:HiddenField ID="hfSmallImagePath" runat="server" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Blog Image</label>
                        <div class="upload-card">
                            <input type="file" id="fuBlogImage" runat="server"
                                accept="image/*" class="form-control" />
                        </div>
                        <span class="field-hint">Main banner image shown on the blog page.</span>
                        <div class="image-preview-box" id="blogImagePreviewBox">
                            <img id="blogImagePreviewImg" src="" alt="Blog image preview" />
                        </div>
                        <asp:HiddenField ID="hfBlogImagePath" runat="server" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label>Title <span class="required-star">*</span></label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"
                            placeholder="Enter Title" MaxLength="500" />
                        <span class="field-hint">The headline shown on the public blog page.</span>
                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server"
                            ControlToValidate="txtTitle"
                            ErrorMessage="Title is required."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
            </div>

            <!-- Row 4: Blog Content (full width HTML editor) -->
            <!-- Row 4: Blog Content (full width HTML editor) -->
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label>Blog Content <span class="required-star">*</span></label>

                        <div class="simple-editor-wrapper">
                            <div class="simple-editor-toolbar" id="blogEditorToolbar">
                                <button type="button" class="editor-btn" data-cmd="bold" title="Bold"><b>B</b></button>
                                <button type="button" class="editor-btn" data-cmd="italic" title="Italic"><i>I</i></button>
                                <button type="button" class="editor-btn" data-cmd="underline" title="Underline"><u>U</u></button>
                                <button type="button" class="editor-btn" data-cmd="strikeThrough" title="Strike"><s>S</s></button>
                                <span class="editor-sep"></span>
                                <select class="editor-select" id="blogFormatBlock" title="Paragraph Style">
                                    <option value="p">Paragraph</option>
                                    <option value="h1">Heading 1</option>
                                    <option value="h2">Heading 2</option>
                                    <option value="h3">Heading 3</option>
                                    <option value="h4">Heading 4</option>
                                    <option value="h5">Heading 5</option>
                                    <option value="h6">Heading 6</option>
                                    <option value="blockquote">Quote</option>
                                </select>
                                <span class="editor-sep"></span>
                                <button type="button" class="editor-btn" data-cmd="insertUnorderedList" title="Bullet List">&#8226; List</button>
                                <button type="button" class="editor-btn" data-cmd="insertOrderedList" title="Numbered List">1. List</button>
                                <span class="editor-sep"></span>
                                <button type="button" class="editor-btn" data-cmd="justifyLeft" title="Align Left">&#8676;</button>
                                <button type="button" class="editor-btn" data-cmd="justifyCenter" title="Align Center">&#8596;</button>
                                <button type="button" class="editor-btn" data-cmd="justifyRight" title="Align Right">&#8677;</button>
                                <span class="editor-sep"></span>
                                <button type="button" class="editor-btn" id="blogLinkBtn" title="Insert Link">&#128279;</button>
                                <button type="button" class="editor-btn" data-cmd="unlink" title="Remove Link">&#9946;</button>
                                <span class="editor-sep"></span>
                                <button type="button" class="editor-btn" data-cmd="removeFormat" title="Clear Formatting">Clear</button>
                                <button type="button" class="editor-btn" data-cmd="undo" title="Undo">&#8630;</button>
                                <button type="button" class="editor-btn" data-cmd="redo" title="Redo">&#8631;</button>

                                <button type="button" class="editor-btn" data-cmd="justifyLeft" title="Align Left">&#8676;</button>
                                <button type="button" class="editor-btn" data-cmd="justifyCenter" title="Align Center">&#8596;</button>
                                <button type="button" class="editor-btn" data-cmd="justifyRight" title="Align Right">&#8677;</button>
                                <button type="button" class="editor-btn" data-cmd="justifyFull" title="Justify">&#9776;</button>

                                <select class="editor-select" id="blogFontName" title="Font Family">
                                    <option value="">Font</option>
                                    <option value="Arial" style="font-family: Arial;">Arial</option>
                                    <option value="Georgia" style="font-family: Georgia;">Georgia</option>
                                    <option value="'Times New Roman'" style="font-family: 'Times New Roman';">Times New Roman</option>
                                    <option value="Verdana" style="font-family: Verdana;">Verdana</option>
                                    <option value="Tahoma" style="font-family: Tahoma;">Tahoma</option>
                                    <option value="'Courier New'" style="font-family: 'Courier New';">Courier New</option>
                                    <option value="'Segoe UI'" style="font-family: 'Segoe UI';">Segoe UI</option>
                                </select>

                                <div class="font-size-wrapper">
                                    <input type="text"
                                        class="editor-input-number"
                                        id="blogFontSize"
                                        value="16"
                                        autocomplete="off"
                                        title="Font Size" />
                                    <div class="font-size-dropdown" id="fontSizeDropdown">
                                        <div class="font-size-option" data-size="8">8</div>
                                        <div class="font-size-option" data-size="10">10</div>
                                        <div class="font-size-option" data-size="12">12</div>
                                        <div class="font-size-option" data-size="14">14</div>
                                        <div class="font-size-option" data-size="16">16</div>
                                        <div class="font-size-option" data-size="18">18</div>
                                        <div class="font-size-option" data-size="20">20</div>
                                        <div class="font-size-option" data-size="24">24</div>
                                        <div class="font-size-option" data-size="28">28</div>
                                        <div class="font-size-option" data-size="32">32</div>
                                        <div class="font-size-option" data-size="36">36</div>
                                        <div class="font-size-option" data-size="48">48</div>
                                        <div class="font-size-option" data-size="72">72</div>
                                    </div>
                                </div>
                            </div>

                            <div class="simple-editor-content"
                                id="blogEditorContent"
                                contenteditable="true"
                                data-placeholder="Write your blog content here...">
                            </div>
                        </div>

                        <!-- Hidden field to sync HTML content back to server on postback -->
                        <asp:HiddenField ID="hdnBlogContent" runat="server" ClientIDMode="Static" />
                    </div>
                </div>
            </div>


            <!-- Sticky Action Bar -->
            <div class="blog-action-bar">
                <asp:Button ID="btnClear" runat="server" Text="Clear"
                    CssClass="btn btn-default" OnClick="btnClear_Click"
                    CausesValidation="false" />
                <asp:Button ID="btnSave" runat="server" Text="Save Blog"
                    CssClass="btn btn-primary" OnClick="btnSave_Click"
                    OnClientClick="if(this.disabled) return false; this.disabled=true; this.value='Saving...';" UseSubmitBehavior="false" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update Blog"
                    CssClass="btn btn-primary" OnClick="btnUpdate_Click"
                    Visible="false" />
            </div>

        </div>
    </div>
    <script>
        (function () {
            var toolbar = document.getElementById('blogEditorToolbar');
            var editor = document.getElementById('blogEditorContent');
            var hiddenField = document.getElementById('<%= hdnBlogContent.ClientID %>');
            var formatSelect = document.getElementById('blogFormatBlock');
            var linkBtn = document.getElementById('blogLinkBtn');

            // Load existing content on page load (edit mode)
            if (hiddenField && hiddenField.value) {
                try {
                    editor.innerHTML = decodeURIComponent(escape(atob(hiddenField.value)));
                } catch (e) {
                    editor.innerHTML = hiddenField.value;
                }
            }

            toolbar.querySelectorAll('.editor-btn[data-cmd]').forEach(function (btn) {
               
                btn.addEventListener('mousedown', function (e) {
                    e.preventDefault();
                    saveSelection();
                });
                btn.addEventListener('click', function () {
                    var cmd = btn.getAttribute('data-cmd');
                    editor.focus();
                    restoreSelection();
                    document.execCommand(cmd, false, null);
                    syncContent();
                    updateActiveStates();
                   
                    saveSelection();
                });
            });

         
            formatSelect.addEventListener('mousedown', saveSelection);
            formatSelect.addEventListener('change', function () {
                editor.focus();
                restoreSelection();
                document.execCommand('formatBlock', false, formatSelect.value);

                
                var sel = window.getSelection();
                if (sel.rangeCount > 0) {
                    var node = sel.getRangeAt(0).commonAncestorContainer;
                    var blockEl = (node.nodeType === 1) ? node : node.parentElement;
                    blockEl = blockEl ? blockEl.closest('h1,h2,h3,h4,h5,h6,p,blockquote,div') : null;
                    if (blockEl && editor.contains(blockEl)) {
                        blockEl.querySelectorAll('[style]').forEach(function (el) {
                            if (el.style.fontSize) {
                                el.style.fontSize = '';
                                if (el.getAttribute('style') === '') {
                                    el.removeAttribute('style');
                                }
                            }
                        });
                        
                        fontSizeInput.value = '';
                    }
                }

                syncContent();
                saveSelection();
            });

            
            linkBtn.addEventListener('mousedown', function (e) {
                e.preventDefault();
                saveSelection();
            });
            linkBtn.addEventListener('click', function () {
                var url = prompt('Enter URL:', 'https://');
                if (url) {
                    editor.focus();
                    restoreSelection();
                    document.execCommand('createLink', false, url);
                    syncContent();
                    saveSelection();
                }
            });

            editor.addEventListener('input', syncContent);
            editor.addEventListener('blur', syncContent);

            function syncContent() {
                if (hiddenField) {
                   
                    var clone = editor.cloneNode(true);
                    clone.querySelectorAll('span').forEach(function (el) {
                        if (el.textContent === '​') {
                            el.remove();
                        }
                    });
                    hiddenField.value = btoa(unescape(encodeURIComponent(clone.innerHTML)));
                }
            }
            //for font

            var fontNameSelect = document.getElementById('blogFontName');
            var fontSizeSelect = document.getElementById('blogFontSize');

            
            var savedRange = null;

            function saveSelection() {
                var sel = window.getSelection();
                if (sel && sel.rangeCount > 0 && editor.contains(sel.anchorNode)) {
                    savedRange = sel.getRangeAt(0).cloneRange();
                }
            }

            function restoreSelection() {
                if (!savedRange) return;
                var sel = window.getSelection();
                sel.removeAllRanges();
                sel.addRange(savedRange);
            }

            editor.addEventListener('mouseup', saveSelection);
            editor.addEventListener('keyup', saveSelection);

            fontNameSelect.addEventListener('mousedown', saveSelection);
            fontNameSelect.addEventListener('change', function () {
                var fontValue = fontNameSelect.value;
                if (!fontValue) return;

                editor.focus();

                var range = (savedRange && editor.contains(savedRange.startContainer))
                    ? savedRange.cloneRange()
                    : null;
                if (!range) {
                    range = document.createRange();
                    range.selectNodeContents(editor);
                    range.collapse(false);
                }

                var sel = window.getSelection();

                if (range.collapsed) {
                    
                    var span = document.createElement('span');
                    span.style.fontFamily = fontValue;
                    span.appendChild(document.createTextNode('​'));
                    range.insertNode(span);

                    var caretRange = document.createRange();
                    caretRange.setStart(span.firstChild, 1);
                    caretRange.setEnd(span.firstChild, 1);
                    sel.removeAllRanges();
                    sel.addRange(caretRange);
                    savedRange = caretRange.cloneRange();
                } else {
                    var span2 = document.createElement('span');
                    span2.style.fontFamily = fontValue;

                    var frag = range.extractContents();

                    
                    frag.querySelectorAll('*').forEach(function (el) {
                        if (el.style && el.style.fontFamily) {
                            el.style.fontFamily = '';
                            if (el.getAttribute('style') === '') {
                                el.removeAttribute('style');
                            }
                        }
                        if (el.tagName === 'FONT' && el.hasAttribute('face')) {
                            el.removeAttribute('face');
                        }
                    });

                    span2.appendChild(frag);
                    range.insertNode(span2);

                    var selectRange = document.createRange();
                    selectRange.selectNodeContents(span2);
                    sel.removeAllRanges();
                    sel.addRange(selectRange);
                    savedRange = selectRange.cloneRange();
                }

                syncContent();
            });

           
            var fontSizeInput = document.getElementById('blogFontSize');
            var fontSizeDropdown = document.getElementById('fontSizeDropdown');

            function applyFontSize(sizeValue) {
                var size = parseInt(sizeValue, 10);
                if (!size || size < 1) return;

                fontSizeInput.value = size;
                editor.focus();

              
                var range = (savedRange && editor.contains(savedRange.startContainer))
                    ? savedRange.cloneRange()
                    : null;

                if (!range) {
                    range = document.createRange();
                    range.selectNodeContents(editor);
                    range.collapse(false);
                }

                var sel = window.getSelection();

                if (range.collapsed) {
                    var span = document.createElement('span');
                    span.style.fontSize = size + 'px';
                    span.appendChild(document.createTextNode('\u200b'));
                    range.insertNode(span);

                    var caretRange = document.createRange();
                    caretRange.setStart(span.firstChild, 1);
                    caretRange.setEnd(span.firstChild, 1);
                    sel.removeAllRanges();
                    sel.addRange(caretRange);
                    savedRange = caretRange.cloneRange();
                } else {
                    var span2 = document.createElement('span');
                    span2.style.fontSize = size + 'px';

                    var frag = range.extractContents();

                    frag.querySelectorAll('*').forEach(function (el) {
                        if (el.style && el.style.fontSize) {
                            el.style.fontSize = '';
                            if (el.getAttribute('style') === '') {
                                el.removeAttribute('style');
                            }
                        }
                        if (el.tagName === 'FONT' && el.hasAttribute('size')) {
                            el.removeAttribute('size');
                        }
                    });

                    span2.appendChild(frag);
                    range.insertNode(span2);

                    var selectRange = document.createRange();
                    selectRange.selectNodeContents(span2);
                    sel.removeAllRanges();
                    sel.addRange(selectRange);
                    savedRange = selectRange.cloneRange();
                }

                syncContent();
                fontSizeDropdown.classList.remove('show');
            }
            fontSizeInput.addEventListener('mousedown', saveSelection);
            fontSizeInput.addEventListener('click', function (e) {
                e.stopPropagation();
                fontSizeDropdown.classList.add('show');
            });

            // Type panni Enter adichaalum apply aagum
            fontSizeInput.addEventListener('keydown', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    applyFontSize(fontSizeInput.value);
                }
            });

            
            fontSizeDropdown.querySelectorAll('.font-size-option').forEach(function (opt) {
                opt.addEventListener('mousedown', function (e) {
                    e.preventDefault();
                });
                opt.addEventListener('click', function (e) {
                    e.stopPropagation();
                    applyFontSize(opt.getAttribute('data-size'));
                });
            });

            document.addEventListener('click', function () {
                fontSizeDropdown.classList.remove('show');
            });
            function updateActiveStates() {
                toolbar.querySelectorAll('.editor-btn[data-cmd]').forEach(function (btn) {
                    var cmd = btn.getAttribute('data-cmd');
                    try {
                        if (document.queryCommandState(cmd)) {
                            btn.classList.add('active');
                        } else {
                            btn.classList.remove('active');
                        }
                    } catch (e) { /* ignore unsupported commands */ }
                });
            }

            editor.addEventListener('keyup', updateActiveStates);
            editor.addEventListener('mouseup', updateActiveStates);

            // Sync before postback (Save button click) - safety net
            var formEl = editor.closest('form');
            if (formEl) {
                formEl.addEventListener('submit', syncContent);
            }

            // ===== Live preview for Small Image / Blog Image uploads =====
            function wireImagePreview(inputId, boxId, imgId) {
                var input = document.getElementById(inputId);
                var box = document.getElementById(boxId);
                var img = document.getElementById(imgId);
                if (!input || !box || !img) return;

                input.addEventListener('change', function () {
                    var file = input.files && input.files[0];
                    if (!file) {
                        box.classList.remove('has-image');
                        img.src = '';
                        return;
                    }
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        img.src = e.target.result;
                        box.classList.add('has-image');
                    };
                    reader.readAsDataURL(file);
                });
            }

            wireImagePreview('<%= fuSmallImage.ClientID %>', 'smallImagePreviewBox', 'smallImagePreviewImg');
            wireImagePreview('<%= fuBlogImage.ClientID %>', 'blogImagePreviewBox', 'blogImagePreviewImg');
        })();

    </script>

</asp:Content>
