<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="updatetimings.aspx.cs" Inherits="Admin_updatetimings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .ut-wrap { max-width: 900px; margin: 0 auto; }

        .ut-card {
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 1px 3px rgba(0,0,0,.06), 0 1px 2px rgba(0,0,0,.08);
            margin-bottom: 16px;
        }

        .ut-header {
            background: linear-gradient(135deg, #1e88e5, #1565c0);
            padding: 16px 20px;
            display: flex;
            align-items: center;
            border-radius: 10px 10px 0 0;
        }
        .ut-header i { color: #fff; font-size: 20px; margin-right: 10px; }
        .ut-header h5 { color: #fff; margin: 0; font-weight: 600; font-size: 15px; letter-spacing: .01em; }

        .ut-body { padding: 18px 20px; }

        /* --- Employee / Date selection --- */
        .ut-select-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px;
            margin-bottom: 4px;
        }
        @media (max-width: 640px) {
            .ut-select-row { grid-template-columns: 1fr; }
        }
        .ut-field-group label {
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: .05em;
            color: #78909c;
            margin-bottom: 5px;
            display: block;
        }
        .ut-field-group .input-group-addon {
            background: #f5f7fa;
            border-right: none;
            color: #607d8b;
        }
        .ut-field-group .form-control {
            border-left: none;
            box-shadow: none;
        }
        .ut-field-group .input-group:focus-within .input-group-addon,
        .ut-field-group .input-group:focus-within .form-control {
            border-color: #64b5f6;
        }

        .ut-divider {
            height: 1px;
            background: #eceff1;
            margin: 18px 0;
        }

        /* --- Timing grid: 2x2 compact cards --- */
        .ut-timing-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 12px;
        }
        @media (max-width: 640px) {
            .ut-timing-grid { grid-template-columns: 1fr; }
        }

        .ut-timing-card {
            border: 1px solid #eceff1;
            border-radius: 8px;
            padding: 12px 14px;
            transition: border-color .15s ease, box-shadow .15s ease;
        }
        .ut-timing-card:hover { border-color: #cfd8dc; }

        .ut-timing-card-head {
            display: flex;
            align-items: center;
            margin-bottom: 10px;
        }
        .ut-timing-dot {
            width: 8px; height: 8px; border-radius: 50%;
            margin-right: 8px; flex-shrink: 0;
        }
        .ut-dot-in       { background: #2196F3; }
        .ut-dot-out      { background: #FF7043; }
        .ut-dot-lunchin  { background: #66BB6A; }
        .ut-dot-lunchout { background: #AB47BC; }

        .ut-timing-card-title {
            font-size: 13px;
            font-weight: 700;
            color: #37474f;
        }

        .ut-timing-inputs {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
        }
        .ut-timing-inputs .input-group { flex: 1 1 140px; min-width: 140px; }
        .ut-timing-inputs .form-control { font-size: 13px; padding: 6px 10px; height: 34px; }
        .ut-timing-inputs .input-group-addon { padding: 6px 8px; font-size: 12px; }

        /* --- Hint + actions --- */
        .ut-hint-box {
            background: #f5f9ff;
            border-left: 3px solid #64b5f6;
            padding: 8px 12px;
            font-size: 12px;
            color: #607d8b;
            border-radius: 4px;
            margin-top: 16px;
        }
        .ut-hint-box i { margin-right: 4px; }

        .ut-actions {
            display: flex;
            justify-content: flex-end;
            gap: 8px;
            margin-top: 16px;
        }
        .ut-actions .btn { padding: 7px 20px; font-size: 13px; border-radius: 6px; }

        .ut-message { margin-top: 14px; }
        .ut-message .alert { padding: 9px 14px; font-size: 13px; border-radius: 6px; margin-bottom: 0; }

        /* --- Custom time dropdown (no external picker library needed) --- */
        .ut-time-group { cursor: pointer; }
        .ut-time-input { cursor: pointer; background: #fff !important; }

        .ut-time-dropdown {
            position: fixed;
            z-index: 4000;
            background: #fff;
            border: 1px solid #dfe3e8;
            border-radius: 8px;
            box-shadow: 0 10px 30px rgba(0,0,0,.16);
            max-height: 230px;
            overflow-y: auto;
            display: none;
        }
        .ut-time-dropdown.open { display: block; }
        .ut-time-option {
            padding: 8px 16px;
            font-size: 13px;
            color: #37474f;
            cursor: pointer;
            white-space: nowrap;
        }
        .ut-time-option:hover { background: #f5f9ff; }
        .ut-time-option.active { background: #e3f2fd; color: #1565c0; font-weight: 600; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="ut-wrap">
        <div class="ut-card">
            <div class="ut-header">
                <i class="icon-alarm"></i>
                <h5>Update Employee In/Out &amp; Lunch Timings</h5>
            </div>

            <div class="ut-body">

                <div class="ut-select-row">
                    <div class="ut-field-group">
                        <label>Employee <span style="color:#e53935">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-users"></i></span>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                <asp:ListItem Text="-- Select Employee --" Value="" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="ut-field-group">
                        <label>Date <span style="color:#e53935">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txtSelectedDate" runat="server" CssClass="form-control pickadate" AutoPostBack="true"
                                OnTextChanged="txtSelectedDate_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="ut-divider"></div>

                <div class="ut-timing-grid">
                    <div class="ut-timing-card">
                        <div class="ut-timing-card-head">
                            <span class="ut-timing-dot ut-dot-in"></span>
                            <span class="ut-timing-card-title">In Time</span>
                        </div>
                        <div class="ut-timing-inputs">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txtInDate" runat="server" CssClass="form-control pickadate" placeholder="Date"></asp:TextBox>
                            </div>
                            <div class="input-group ut-time-group">
                                <span class="input-group-addon"><i class="icon-clock2"></i></span>
                                <asp:TextBox ID="txtInTime" runat="server" ReadOnly="true" CssClass="form-control ut-time-input" placeholder="Time"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="ut-timing-card">
                        <div class="ut-timing-card-head">
                            <span class="ut-timing-dot ut-dot-out"></span>
                            <span class="ut-timing-card-title">Out Time</span>
                        </div>
                        <div class="ut-timing-inputs">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txtOutDate" runat="server" CssClass="form-control pickadate" placeholder="Date"></asp:TextBox>
                            </div>
                            <div class="input-group ut-time-group">
                                <span class="input-group-addon"><i class="icon-clock2"></i></span>
                                <asp:TextBox ID="txtOutTime" runat="server" ReadOnly="true" CssClass="form-control ut-time-input" placeholder="Time"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="ut-timing-card">
                        <div class="ut-timing-card-head">
                            <span class="ut-timing-dot ut-dot-lunchin"></span>
                            <span class="ut-timing-card-title">Lunch In</span>
                        </div>
                        <div class="ut-timing-inputs">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txtLunchInDate" runat="server" CssClass="form-control pickadate" placeholder="Date"></asp:TextBox>
                            </div>
                            <div class="input-group ut-time-group">
                                <span class="input-group-addon"><i class="icon-clock2"></i></span>
                                <asp:TextBox ID="txtLunchInTime" runat="server" ReadOnly="true" CssClass="form-control ut-time-input" placeholder="Time"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="ut-timing-card">
                        <div class="ut-timing-card-head">
                            <span class="ut-timing-dot ut-dot-lunchout"></span>
                            <span class="ut-timing-card-title">Lunch Out</span>
                        </div>
                        <div class="ut-timing-inputs">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txtLunchOutDate" runat="server" CssClass="form-control pickadate" placeholder="Date"></asp:TextBox>
                            </div>
                            <div class="input-group ut-time-group">
                                <span class="input-group-addon"><i class="icon-clock2"></i></span>
                                <asp:TextBox ID="txtLunchOutTime" runat="server" ReadOnly="true" CssClass="form-control ut-time-input" placeholder="Time"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="ut-hint-box">
                    <i class="icon-info22"></i>Select an employee and a date first &ndash; saved timings for that day load automatically. Only the field(s) you change get updated.
                </div>

                <div class="ut-actions">
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-default" CausesValidation="false" OnClick="btnClear_Click" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                </div>

                <div class="ut-message">
                    <asp:Literal ID="litMessage" runat="server" />
                </div>

                <asp:HiddenField ID="hfInOutTimekey" runat="server" />
                <asp:HiddenField ID="hfLunchTimekey" runat="server" />
                <asp:HiddenField ID="hfOrigInTime" runat="server" />
                <asp:HiddenField ID="hfOrigOutTime" runat="server" />
                <asp:HiddenField ID="hfOrigLunchIn" runat="server" />
                <asp:HiddenField ID="hfOrigLunchOut" runat="server" />
            </div>
        </div>
    </div>

    <script>
        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });

        // Lightweight, dependency-free time picker.
        // (The theme's date picker plugin doesn't ship a time component, and the native
        // <input type="time"> popup is positioned by the browser/OS itself, which on some
        // mobile browsers rendered squeezed or opened far from the field, near the footer.
        // This version computes the field's on-screen position every time it's opened and
        // anchors the dropdown right under it, so it always opens next to the field.)
        (function () {
            var dropdown = null;
            var activeInput = null;

            function formatTime12(h, m) {
                var period = h < 12 ? 'AM' : 'PM';
                var h12 = h % 12;
                if (h12 === 0) h12 = 12;
                var hh = (h12 < 10 ? '0' : '') + h12;
                var mm = (m < 10 ? '0' : '') + m;
                return hh + ':' + mm + ' ' + period;
            }

            function buildDropdown() {
                if (dropdown) return dropdown;
                dropdown = document.createElement('div');
                dropdown.className = 'ut-time-dropdown';
                for (var h = 0; h < 24; h++) {
                    for (var m = 0; m < 60; m += 15) {
                        var value = formatTime12(h, m);
                        var opt = document.createElement('div');
                        opt.className = 'ut-time-option';
                        opt.textContent = value;
                        opt.setAttribute('data-value', value);
                        opt.addEventListener('mousedown', function (ev) {
                            ev.preventDefault();
                            if (activeInput) {
                                activeInput.value = this.getAttribute('data-value');
                            }
                            closeDropdown();
                        });
                        dropdown.appendChild(opt);
                    }
                }
                document.body.appendChild(dropdown);
                return dropdown;
            }

            function openDropdown(input) {
                var dd = buildDropdown();
                activeInput = input;

                var rect = input.getBoundingClientRect();
                var spaceBelow = window.innerHeight - rect.bottom;
                dd.style.minWidth = Math.max(rect.width, 110) + 'px';
                dd.style.left = Math.max(4, rect.left) + 'px';

                dd.classList.add('open');
                var ddHeight = dd.offsetHeight;

                if (spaceBelow < ddHeight + 8 && rect.top > ddHeight) {
                    dd.style.top = (rect.top - ddHeight - 4) + 'px';
                } else {
                    dd.style.top = (rect.bottom + 4) + 'px';
                }

                var val = input.value;
                var opts = dd.querySelectorAll('.ut-time-option');
                var activeEl = null;
                for (var i = 0; i < opts.length; i++) {
                    var isActive = opts[i].getAttribute('data-value') === val;
                    opts[i].classList.toggle('active', isActive);
                    if (isActive) activeEl = opts[i];
                }
                if (activeEl) {
                    activeEl.scrollIntoView({ block: 'center' });
                } else {
                    dd.scrollTop = 0;
                }
            }

            function closeDropdown() {
                if (dropdown) dropdown.classList.remove('open');
                activeInput = null;
            }

            document.addEventListener('click', function (e) {
                var group = e.target.closest ? e.target.closest('.ut-time-group') : null;
                if (group) {
                    var input = group.querySelector('.ut-time-input');
                    if (input) openDropdown(input);
                    e.stopPropagation();
                } else if (!dropdown || !dropdown.contains(e.target)) {
                    closeDropdown();
                }
            });

            window.addEventListener('scroll', function (e) {
                if (dropdown && (e.target === dropdown || dropdown.contains(e.target))) {
                    return; // scrolling inside the dropdown's own list - don't close it
                }
                closeDropdown();
            }, true);
            window.addEventListener('resize', closeDropdown);
        })();
    </script>

</asp:Content>
