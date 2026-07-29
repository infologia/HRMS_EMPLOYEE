using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Employee_payslipdetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_userkey = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.str_userkey = SC.Userid;

        if (Request.QueryString["action"] != null)
        {
            string action = Request.QueryString["action"].ToString();
            string month = Request.QueryString["month"];
            string year = Request.QueryString["year"];
            
            if (!string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year))
            {
                GenerateAndServePDF(action, Convert.ToInt32(month), Convert.ToInt32(year));
            }
            return;
        }

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "My Payslips";

            LoadGrid();
        }
    }

    private void LoadGrid()
    {
        string query = @"
            SELECT 
                p.PayrollMonth,
                p.PayrollYear,
                p.NetPay,
                ISNULL(e.Firstname + ' ' + e.Lastname, 'Admin') AS GeneratedBy
            FROM IT_EmployeePayrollDetails p
            LEFT JOIN IT_EmployeeRegister e ON p.Createdby = e.Employeekey
            WHERE p.Employeekey = @Employeekey
            ORDER BY p.PayrollYear DESC, p.PayrollMonth ASC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        DataTable dt_source = DA.GetDataTable(cmd);

        DataTable dt_grid = new DataTable();
        dt_grid.Columns.Add("Month");
        dt_grid.Columns.Add("Year");
        dt_grid.Columns.Add("NetPay");
        dt_grid.Columns.Add("GeneratedBy");
        dt_grid.Columns.Add("ViewLink");
        dt_grid.Columns.Add("DownloadLink");
        dt_grid.Columns.Add("SummaryLink");

        if (dt_source != null && dt_source.Rows.Count > 0)
        {
            foreach (DataRow row in dt_source.Rows)
            {
                int month = Convert.ToInt32(row["PayrollMonth"]);
                int year = Convert.ToInt32(row["PayrollYear"]);
                string monthName = new DateTime(year, month, 1).ToString("MMMM");

                string netPay = row["NetPay"] != DBNull.Value ? row["NetPay"].ToString() : "0.00";
                
                string generatedBy = row["GeneratedBy"] != DBNull.Value ? row["GeneratedBy"].ToString() : "-";

                string viewLink = "<a href='payslipdetails.aspx?action=view&month=" + month + "&year=" + year + "' target='_blank'><button type='button' class='label label-info'>View</button></a>";
                string downloadLink = "<a href='payslipdetails.aspx?action=download&month=" + month + "&year=" + year + "'><button type='button' class='label label-sm label-success'>Download</button></a>";
                string summaryLink = "<a href='Payslipsummary.aspx?month=" + month + "&year=" + year + "'><button type='button' class='label label-warning'>Summary</button></a>";

                DataRow dr = dt_grid.NewRow();
                dr["Month"] = monthName;
                dr["Year"] = year;
                dr["NetPay"] = netPay;
                dr["GeneratedBy"] = generatedBy;
                dr["ViewLink"] = viewLink;
                dr["DownloadLink"] = downloadLink;
                dr["SummaryLink"] = summaryLink;
                dt_grid.Rows.Add(dr);
            }
        }

        PH_payslipdetails.Controls.Clear();
        if (dt_grid.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt_grid);
            PH.LoadGridItem(ds, PH_payslipdetails, "payslipdetails.txt", "");
            lbl_nodata.Visible = false;
        }
        else
        {
            lbl_nodata.Visible = true;
        }
    }

    private void GenerateAndServePDF(string action, int month, int year)
    {
        // Get Payroll Data
        string prQuery = "SELECT * FROM IT_EmployeePayrollDetails WHERE Employeekey = @Employeekey AND PayrollMonth = @Month AND PayrollYear = @Year";
        SqlCommand prCmd = new SqlCommand(prQuery);
        prCmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        prCmd.Parameters.AddWithValue("@Month", month);
        prCmd.Parameters.AddWithValue("@Year", year);
        DataTable dtPR = DA.GetDataTable(prCmd);

        // Get Salary Data
        string salQuery = "SELECT * FROM IT_EmployeeSalaryDetails WHERE Employeekey = @Employeekey";
        SqlCommand salCmd = new SqlCommand(salQuery);
        salCmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        DataTable dtSal = DA.GetDataTable(salCmd);
        
        // Get Employee Details with Designation Name from Department
        string empQuery = @"SELECT e.Employeekey,e.employeeid,e.Firstname,e.address,e.Department,e.DateOfJoining,Convert(DATE,e.DOB,103) AS DOB,e.PANNumber, d.Departmentname AS DesignationName 
                            FROM IT_EmployeeRegister e 
                            LEFT JOIN IT_Department d ON e.Department = d.Departmentid 
                            WHERE e.Employeekey = @Employeekey";
        SqlCommand empCmd = new SqlCommand(empQuery);
        empCmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        DataTable dtEmp = DA.GetDataTable(empCmd);

        if (dtPR == null || dtPR.Rows.Count == 0 || dtSal == null || dtSal.Rows.Count == 0 || dtEmp == null || dtEmp.Rows.Count == 0)
        {
            Response.Write("Payslip data not found.");
            Response.End();
            return;
        }

        DataRow rowPR = dtPR.Rows[0];
        DataRow rowSal = dtSal.Rows[0];
        DataRow rowEmp = dtEmp.Rows[0];

        string monthName = new DateTime(year, month, 1).ToString("MMMM");

        using (MemoryStream ms = new MemoryStream())
        {
            Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            // Fonts
            BaseFont bfNormal = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont bfBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            
            Font fontHeader = new Font(bfBold, 14, Font.NORMAL, BaseColor.BLACK);
            Font fontSubHeader = new Font(bfNormal, 11, Font.NORMAL, BaseColor.DARK_GRAY);
            Font fontBold = new Font(bfBold, 10, Font.NORMAL, BaseColor.BLACK);
            Font fontNormal = new Font(bfNormal, 10, Font.NORMAL, BaseColor.BLACK);
            Font fontSmall = new Font(bfNormal, 8, Font.NORMAL, BaseColor.GRAY);

            // Table for Header (Logo and Company Details)
            PdfPTable tableHeader = new PdfPTable(2);
            tableHeader.WidthPercentage = 100;
            tableHeader.SetWidths(new float[] { 3f, 7f });

            string logoPath = Server.MapPath("~/images/AdminProfilePictures/Companylogo.png");
            if (File.Exists(logoPath))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleToFit(120f, 50f);
                PdfPCell cellLogo = new PdfPCell(logo);
                cellLogo.Border = PdfPCell.NO_BORDER;
                cellLogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                tableHeader.AddCell(cellLogo);
            }
            else
            {
                PdfPCell cellNoLogo = new PdfPCell(new Phrase("INFOLOGIA", fontHeader));
                cellNoLogo.Border = PdfPCell.NO_BORDER;
                tableHeader.AddCell(cellNoLogo);
            }

            PdfPCell cellCompanyInfo = new PdfPCell();
            cellCompanyInfo.Border = PdfPCell.NO_BORDER;
            cellCompanyInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellCompanyInfo.AddElement(new Paragraph("Infologia Technologies Private Limited", fontHeader) { Alignment = Element.ALIGN_RIGHT });
            cellCompanyInfo.AddElement(new Paragraph("Payslip for the month of " + monthName + " " + year, fontSubHeader) { Alignment = Element.ALIGN_RIGHT });
            tableHeader.AddCell(cellCompanyInfo);

            doc.Add(tableHeader);
            doc.Add(new Paragraph("\n"));

            // Employee Details Table
            PdfPTable tableEmp = new PdfPTable(4);
            tableEmp.WidthPercentage = 100;
            tableEmp.SetWidths(new float[] { 2f, 3f, 2f, 3f });

            tableEmp.AddCell(new PdfPCell(new Phrase("Employee Name", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase(": " + rowPR["EmployeeName"].ToString(), fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase("PAN", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase(": " + rowSal["EmployeePanNUmber"].ToString(), fontNormal)) { Border = PdfPCell.NO_BORDER });

            tableEmp.AddCell(new PdfPCell(new Phrase("Employee ID", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase(": " + rowPR["Employeeid"].ToString(), fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase("Date of Birth", fontNormal)) { Border = PdfPCell.NO_BORDER });
            string value = rowEmp["DOB"].ToString();
            string dob = rowEmp.Table.Columns.Contains("DOB") && rowEmp["DOB"] != DBNull.Value ? Convert.ToDateTime(rowEmp["DOB"]).ToString("dd.MM.yyyy") : "-";

            tableEmp.AddCell(new PdfPCell(new Phrase(": " + dob, fontNormal)) { Border = PdfPCell.NO_BORDER });

            tableEmp.AddCell(new PdfPCell(new Phrase("Designation", fontNormal)) { Border = PdfPCell.NO_BORDER });
            string desig = rowEmp.Table.Columns.Contains("DesignationName") && rowEmp["DesignationName"] != DBNull.Value ? rowEmp["DesignationName"].ToString() : "-";
            tableEmp.AddCell(new PdfPCell(new Phrase(": " + desig, fontNormal)) { Border = PdfPCell.NO_BORDER }); 
            tableEmp.AddCell(new PdfPCell(new Phrase("Date of Joining", fontNormal)) { Border = PdfPCell.NO_BORDER });
            string doj = rowSal["EmployeeDOJ"] != DBNull.Value ? Convert.ToDateTime(rowSal["EmployeeDOJ"]).ToString("dd.MM.yyyy") : "-";
            tableEmp.AddCell(new PdfPCell(new Phrase(": " + doj, fontNormal)) { Border = PdfPCell.NO_BORDER });

            tableEmp.AddCell(new PdfPCell(new Phrase("Location", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase(": Tamil Nadu", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });

            tableEmp.AddCell(new PdfPCell(new Phrase("Days Worked", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase(": " + rowPR["NoOfWorkingDays"].ToString(), fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableEmp.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });

            doc.Add(tableEmp);
            doc.Add(new Paragraph("\n"));

            // Main Earnings / Deductions Table
            PdfPTable tableSalary = new PdfPTable(6);
            tableSalary.WidthPercentage = 100;
            tableSalary.SetWidths(new float[] { 3f, 1f, 1.5f, 3f, 1f, 1.5f });

            // Headers
            PdfPCell c1 = new PdfPCell(new Phrase("EARNINGS & ALLOWANCES", fontBold)) { Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER, Padding = 5, BorderColor = BaseColor.LIGHT_GRAY };
            PdfPCell c2 = new PdfPCell(new Phrase("UNITS", fontBold)) { Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER, Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = BaseColor.LIGHT_GRAY };
            PdfPCell c3 = new PdfPCell(new Phrase("INR", fontBold)) { Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER, Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = BaseColor.LIGHT_GRAY };
            
            PdfPCell c4 = new PdfPCell(new Phrase("DEDUCTIONS", fontBold)) { Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER, Padding = 5, BorderColor = BaseColor.LIGHT_GRAY };
            PdfPCell c5 = new PdfPCell(new Phrase("UNITS", fontBold)) { Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER, Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = BaseColor.LIGHT_GRAY };
            PdfPCell c6 = new PdfPCell(new Phrase("INR", fontBold)) { Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER, Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = BaseColor.LIGHT_GRAY };

            tableSalary.AddCell(c1); tableSalary.AddCell(c2); tableSalary.AddCell(c3);
            tableSalary.AddCell(c4); tableSalary.AddCell(c5); tableSalary.AddCell(c6);

            // Adding Rows - Earnings vs Deductions
            string[] earningNames = { "Base Salary", "House Rent Allowance", "Conveyance", "Special Allowance" };
            string[] earningVals = { 
                rowSal["Employeebasipay"].ToString(), 
                rowSal["EmployeeHRA"].ToString(), 
                rowSal["Employeeconveyance"].ToString(),
                rowSal.Table.Columns.Contains("SPLallowance") ? rowSal["SPLallowance"].ToString() : "0.00"
            };

            decimal lopAmount = 0;
            if (rowPR.Table.Columns.Contains("TotalDeductionDays") && rowPR.Table.Columns.Contains("PerDaySalary"))
            {
                decimal totalDedDays = rowPR["TotalDeductionDays"] != DBNull.Value ? Convert.ToDecimal(rowPR["TotalDeductionDays"]) : 0;
                decimal perDaySal = rowPR["PerDaySalary"] != DBNull.Value ? Convert.ToDecimal(rowPR["PerDaySalary"]) : 0;
                lopAmount = totalDedDays * perDaySal;
            }

            string[] deductionNames = { "Provident Fund", "ESI", "TDS", "Loss of Pay" };
            string[] deductionVals = {
                rowSal["Employeepfamoount"].ToString(),
                rowSal["Employeeesiamount"].ToString(),
                rowSal["Employeetdsamount"].ToString(),
                lopAmount.ToString("0.00")
            };

            int maxRows = Math.Max(earningNames.Length, deductionNames.Length);

            for (int i = 0; i < maxRows; i++)
            {
                if (i < earningNames.Length)
                {
                    tableSalary.AddCell(new PdfPCell(new Phrase(earningNames[i], fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
                    tableSalary.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(earningVals[i]).ToString("N2"), fontNormal)) { Border = PdfPCell.RIGHT_BORDER, Padding = 4, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = BaseColor.LIGHT_GRAY });
                }
                else
                {
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.RIGHT_BORDER, BorderColor = BaseColor.LIGHT_GRAY });
                }

                if (i < deductionNames.Length)
                {
                    tableSalary.AddCell(new PdfPCell(new Phrase(deductionNames[i], fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
                    tableSalary.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(deductionVals[i]).ToString("N2"), fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4, HorizontalAlignment = Element.ALIGN_RIGHT });
                }
                else
                {
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
                    tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
                }
            }

            for (int i = 0; i < 5; i++)
            {
                tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
                tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
                tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.RIGHT_BORDER, BorderColor = BaseColor.LIGHT_GRAY });
                tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
                tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
                tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
            }

            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.RIGHT_BORDER, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase("Gross Deductions", fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
            tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
            tableSalary.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(rowSal["Totaldeduction"]).ToString("N2"), fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.RIGHT_BORDER, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.NO_BORDER });

            tableSalary.AddCell(new PdfPCell(new Phrase("(*) denotes back pay adjustment.", fontNormal)) { Border = PdfPCell.NO_BORDER, Colspan = 2, Padding = 4 });
            tableSalary.AddCell(new PdfPCell(new Phrase(" ", fontNormal)) { Border = PdfPCell.RIGHT_BORDER, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase("PAY SUMMARY", fontBold)) { Border = PdfPCell.TOP_BORDER, Padding = 5, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase("UNITS", fontBold)) { Border = PdfPCell.TOP_BORDER, Padding = 5, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase("INR", fontBold)) { Border = PdfPCell.TOP_BORDER, Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = BaseColor.LIGHT_GRAY });

            tableSalary.AddCell(new PdfPCell(new Phrase("Gross Earning", fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
            tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(rowSal["Totalearnings"]).ToString("N2"), fontNormal)) { Border = PdfPCell.RIGHT_BORDER, Padding = 4, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase("Net Pay", fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4 });
            tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER });
            tableSalary.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(rowPR["NetPay"]).ToString("N2"), fontNormal)) { Border = PdfPCell.NO_BORDER, Padding = 4, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER, Colspan = 3, Padding = 8 });
            tableSalary.AddCell(new PdfPCell(new Phrase("", fontNormal)) { Border = PdfPCell.NO_BORDER, Colspan = 3, Padding = 8 });

            tableSalary.AddCell(new PdfPCell(new Phrase("Payment mode: Bank", fontNormal)) { Border = PdfPCell.TOP_BORDER, Colspan = 3, Padding = 8, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase("CLAIMS\nClaims", fontNormal)) { Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER, Padding = 8, Colspan = 2, BorderColor = BaseColor.LIGHT_GRAY });
            tableSalary.AddCell(new PdfPCell(new Phrase("INR\n0.00", fontNormal)) { Border = PdfPCell.TOP_BORDER, Padding = 8, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = BaseColor.LIGHT_GRAY });

            PdfPCell cMsg = new PdfPCell(new Phrase("MESSAGE:", fontNormal));
            cMsg.Colspan = 6;
            cMsg.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
            cMsg.Padding = 5;
            cMsg.BorderColor = BaseColor.LIGHT_GRAY;
            tableSalary.AddCell(cMsg);

            PdfPTable outerTable = new PdfPTable(1);
            outerTable.WidthPercentage = 100;
            PdfPCell outerCell = new PdfPCell(tableSalary);
            outerCell.Border = PdfPCell.BOX;
            outerCell.BorderColor = BaseColor.BLACK;
            outerCell.Padding = 0;
            outerTable.AddCell(outerCell);

            doc.Add(outerTable);
            doc.Add(new Paragraph("\n"));

            string footerText = "This document contains confidential information. if you are not the intended recipient, you are not authorized to use or disclose it in any form. If you have received this in error, please destroy it along with any copies and notify the sender immediately.";
            Paragraph footer = new Paragraph(footerText, fontSmall);
            footer.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(footer);

            doc.Close();
            writer.Close();

            byte[] pdfBytes = ms.ToArray();
            Response.Clear();
            Response.ContentType = "application/pdf";
            if (action == "download")
            {
                Response.AddHeader("content-disposition", "attachment;filename=Payslip_" + monthName + "_" + year + ".pdf");
            }
            else
            {
                Response.AddHeader("content-disposition", "inline;filename=Payslip_" + monthName + "_" + year + ".pdf");
            }
            Response.BinaryWrite(pdfBytes);
            Response.Flush();
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
