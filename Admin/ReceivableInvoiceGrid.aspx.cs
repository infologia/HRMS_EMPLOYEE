using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.tool.xml;
using System.Text;
using System.Globalization;

public partial class Admin_ReceivableInvoiceGrid : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        if (Request.QueryString["InvoiceKey"] != null)
        {
            DownloadInvoiceTxt(Request.QueryString["InvoiceKey"]);
            return;
        }
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Receivable Invoice";
            BindFinancialYearDropdown();
            BindGrid();
        }
        }

    private void BindFinancialYearDropdown()
    {
        ddlFinancialYear.Items.Clear();
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int startYear = currentMonth >= 4 ? currentYear : currentYear - 1;

        for (int y = startYear; y >= 2020; y--)
        {
            string fyText = "FY " + y + "-" + (y + 1).ToString().Substring(2, 2);
            string fyValue = y.ToString();
            ddlFinancialYear.Items.Add(new System.Web.UI.WebControls.ListItem(fyText, fyValue));
        }
    }

    protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    private void GetFinancialYearDates(out DateTime startDate, out DateTime endDate)
    {
        int startYear = Convert.ToInt32(ddlFinancialYear.SelectedValue);
        startDate = new DateTime(startYear, 4, 1);
        endDate = new DateTime(startYear + 1, 3, 31, 23, 59, 59);
    }

    private void BindGrid()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

        string query1 = @"SELECT   a.InvoiceKey,
    b.CompanyName,
    a.InvoiceNumber,c.ProjectName,
    CAST(a.InvoiceDate AS DATE)  AS InvoiceDate,
    CAST(a.ReceivedOn AS DATE)   AS ReceivedOn,
    CAST(a.CreatedOn AS DATE)    AS CreatedOn,a.Status
FROM IT_Invoices a
INNER JOIN IT_ClientDetails b  ON a.ClientKey = b.ClientKey
inner join IT_Projects c on a.ProjectKey=c.ProjectKey
WHERE ISNULL(a.InvoiceDate, a.CreatedOn) >= @FYStart AND ISNULL(a.InvoiceDate, a.CreatedOn) <= @FYEnd";
        SqlCommand cmd1 = new SqlCommand(query1);
        cmd1.Parameters.AddWithValue("@FYStart", fyStart);
        cmd1.Parameters.AddWithValue("@FYEnd", fyEnd);
        DataTable dt_dashboard = DA.GetDataTable(cmd1);

        DataSet ds = new DataSet();

        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)

        {

            if (ds.Tables[0].Columns.Contains("Status"))

            ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("Company_Name");
            ds.Tables[0].Columns.Add("Invoice_Number");
            ds.Tables[0].Columns.Add("Invoice_Date");
            ds.Tables[0].Columns.Add("Due_Date");
            ds.Tables[0].Columns.Add("Created_Date");
            ds.Tables[0].Columns.Add("Download");

            string str_InvoiceDate = "";
            string str_ReceivedOn = "";
            string str_CreatedOn = "";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["Download"] =
   "<a href='ReceivableInvoiceGrid.aspx?InvoiceKey=" + dr["InvoiceKey"] + "'>" +
   "<button type='button' class='label label-sm label-success'>Download</button></a>";
                String str_Status = dr["Status"].ToString();
                String str_Employee = dr["CompanyName"].ToString();
                String str_InvoiceNumber = dr["InvoiceNumber"].ToString();
                //String str_InvoiceDate = dr["InvoiceDate"].ToString();
                //String str_ReceivedOn = dr["ReceivedOn"].ToString();
                //String str_CreatedOn = dr["CreatedOn"].ToString();

                int activetype = Convert.ToInt32(str_Status);
                if (dr["InvoiceDate"] != DBNull.Value)
                {
                    str_InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"])
                                        .ToString("yyyy-MM-dd");
                }

                if (dr["ReceivedOn"] != DBNull.Value)
                {
                    str_ReceivedOn = Convert.ToDateTime(dr["ReceivedOn"])
                                        .ToString("yyyy-MM-dd");
                }
                if (dr["CreatedOn"] != DBNull.Value)
                {
                    str_CreatedOn = Convert.ToDateTime(dr["CreatedOn"])
                                        .ToString("yyyy-MM-dd");
                }
                dr["Company_Name"] = str_Employee;
                dr["Invoice_Number"] = str_InvoiceNumber;
                dr["Invoice_Date"] = str_InvoiceDate;
                dr["Due_Date"] = str_ReceivedOn;
                dr["Created_Date"] = str_CreatedOn;
                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_Status + "'>Received</span>";
                }
                else 
                {
                    dr["ActiveText"] = "<span class='label label-sm label-warning' title='" + str_Status + "'>Pending</span>";
                }
            }
            this.PH.LoadGridItem(ds, PH_RECEIVABLEINVOICE, "Receivableinvoice.txt", "");
        }
    }

    private void DownloadInvoiceTxt(string invoiceKey)
    {
        string query = @"SELECT a.InvoiceNumber, a.InvoiceDate, a.InvoiceAmount, a.GSTAmount,a.SGSTAmount,a.CGSTAmount,a.IGSTAmount,a.TotalAmount, e.CurrencyCode, c.Description, c.Amount, a.ReceivedOn, b.CompanyName, b.Mobile, b.Email, b.GstNumber,b.Country,f.Country as countryname,b.AddressLine1,b.AddressLine2,City,b.State, d.ProjectName FROM IT_Invoices a INNER JOIN IT_ClientDetails b ON a.ClientKey = b.ClientKey INNER JOIN IT_InvoiceDescription c ON a.InvoiceKey = c.InvoiceKey INNER JOIN IT_Projects d ON a.ProjectKey = d.ProjectKey INNER JOIN IT_Currency e ON e.LocalCurrencyID = a.Currency left outer join IT_Countries f on f.CountryKey=b.Country WHERE a.InvoiceKey = @InvoiceKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@InvoiceKey", invoiceKey);
        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count == 0) return;
        DataRow dr = dt.Rows[0];
        StringBuilder companyInfo = new StringBuilder();
        if (!string.IsNullOrEmpty(SafeValue(dr, "CompanyName"))) companyInfo.Append(SafeValue(dr, "CompanyName") + "<br/>");
        if (!string.IsNullOrEmpty(SafeValue(dr, "AddressLine1"))) companyInfo.Append(SafeValue(dr, "AddressLine1") + "<br/>");
        if (!string.IsNullOrEmpty(SafeValue(dr, "AddressLine2"))) companyInfo.Append(SafeValue(dr, "AddressLine2") + "<br/>");
        if (!string.IsNullOrEmpty(SafeValue(dr, "City"))) companyInfo.Append(SafeValue(dr, "City") + "<br/>");
        if (!string.IsNullOrEmpty(SafeValue(dr, "State"))) companyInfo.Append(SafeValue(dr, "State") + "<br/>");
        if (!string.IsNullOrEmpty(SafeValue(dr, "countryname"))) companyInfo.Append(SafeValue(dr, "countryname") + "<br/>");
        if (!string.IsNullOrEmpty(SafeValue(dr, "Mobile"))) companyInfo.Append(SafeValue(dr, "Mobile") + "<br/>");
        if (!string.IsNullOrEmpty(SafeValue(dr, "GstNumber"))) companyInfo.Append("<strong>GSTIN :</strong> : " + SafeValue(dr, "GstNumber"));

        StringBuilder html = new StringBuilder();
        
        html.AppendLine("<style>");
        html.AppendLine("body  { font-family: serif; color:#5a5252; font-size:12px; line-height:1.8; padding-bottom:80px; }");
        html.AppendLine("table { color:#555555; }");
        html.AppendLine("th { color:#444444; }");
        html.AppendLine("td { color:#666666; }");
        html.AppendLine("strong { color:#333333; }");
        html.AppendLine("</style>");
        html.AppendLine("<br/>");
        html.AppendLine("<br/>");
        html.AppendLine("<br/>");
        html.AppendLine("<table width='100%' style='font-size:12px; border:0;'>");
        html.AppendLine("<tr>");
        html.AppendLine("<td style='line-height:1.8;'>");
        html.AppendLine("<strong style='font-size:15px;font-family: serif;line-height:1.8;'>INFOLOGIA TECHNOLOGIES PVT LTD</strong><br/>");
        html.AppendLine("No 15, 3rd Floor, Sarayu Park,2nd main road, <br/>");
        html.AppendLine("New colony, Chrompet, Chennai- 600 044,<br/>");
        html.AppendLine("Tamilnadu, India. <br/>");
        html.AppendLine("<a href='https://www.infologia.in' style='color:#00bcd4; text-decoration:underline;'>www.infologia.in</a>");
        html.AppendLine("</td>");
        html.AppendLine("<td align='right' valign='top' width='50%' style='padding-right:-40px;'>"); // 👈 move right
        html.AppendLine("<div style='text-align:center;'>");
        html.AppendLine("<strong style='font-size:20px;'>INVOICE</strong><br/>");
        html.AppendLine("<img src='file:///"
            + Server.MapPath("../images/AdminProfilePictures/84fc7bf9-99fa-4104-a8ae-dec30aa64f80.jpg").Replace("\\", "/")
            + "' width='120' style='display:block; margin:6px auto;'/>");
        html.AppendLine("</div>");
        html.AppendLine("</td>");
        html.AppendLine("</tr>");
        html.AppendLine("</table>");

        html.AppendLine("<table width='100%' style='margin-top:20px; font-size:12px;'>");
        html.AppendLine("<tr>");

        html.AppendLine("<td valign='top' align='left' style='line-height:1.8;'>");
        html.AppendLine("<strong>TO</strong><br/>");
        html.AppendLine(companyInfo.ToString());
        html.AppendLine("</td>");

        html.AppendLine("<td valign='top' width='30%' style='text-align:right;'>");
        html.AppendLine("<table align='right' style='font-size:12px; border:none;'>");

        // Data array
        string currency = SafeValue(dr, "CurrencyCode");
        
        List<string[]> invoiceFields = new List<string[]>();
        
        string invNum = SafeValue(dr, "InvoiceNumber");
        if (!invNum.StartsWith("#")) invNum = "#" + invNum;
        
        invoiceFields.Add(new string[] { "Invoice No", ": " + invNum });
        invoiceFields.Add(new string[] { "Invoice Date", ": " + Convert.ToDateTime(dr["InvoiceDate"]).ToString("dd/MM/yyyy") });
        
        // GST only if currency is INR
        if (currency == "INR")
        {
            invoiceFields.Add(new string[] { "GSTIN", ": 33AAECI0201E1Z5" });
            invoiceFields.Add(new string[] { "SAC Code", ": 998313" });
        }
        else
        {
            invoiceFields.Add(new string[] { "Due Date", ": " + Convert.ToDateTime(dr["ReceivedOn"]).ToString("dd/MM/yyyy") });
        }

            // Loop (same as your logic)
            foreach (var field in invoiceFields)
            {
                html.AppendLine("<tr>");
                html.AppendLine(
                    "<td style='font-weight:bold; color:#444; padding-bottom:4px; text-align:left;'>"
                    + field[0] + "</td>");
                html.AppendLine(
                    "<td style='padding-bottom:4px; text-align:left;'>" + field[1] + "</td>");
                html.AppendLine("</tr>");
            }


        html.AppendLine("</table>");
        html.AppendLine("</td>");


        html.AppendLine("</tr>");
        html.AppendLine("</table>");
        
        string subjectLine = "";
        if (dt.Rows.Count > 0)
        {
            string firstDesc = SafeValue(dt.Rows[0], "Description");
            string[] descLines = firstDesc.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (descLines.Length > 0)
            {
                subjectLine = descLines[0];
            }
        }

        if (!string.IsNullOrEmpty(subjectLine))
        {
            html.AppendLine("<p style='font-weight:bold; font-size:12px; margin-top:15px; margin-bottom:10px; color:#000;'>SUB: " + subjectLine + "</p>");
        }
        
        html.AppendLine(
        "<table width='100%' cellspacing='0' cellpadding='14' " +
        "style='border-collapse:collapse; " +
        "border:0.1px solid #e0e0e0; font-size:11px;'>");


        html.AppendLine(
  "<tr style='height:45px;'>" +
  "<th width='5%' style='border:0.1px solid #e0e0e0; padding:14px; text-align:center; color:#555;font-size:10px;'>S.NO</th>" +
  "<th width='75%' style='border:0.1px solid #e0e0e0; padding:14px; text-align:center; color:#555;font-size:10px;'>DESCRIPTION</th>" +
  "<th width='20%' style='border:0.1px solid #e0e0e0; padding:14px; text-align:center; color:#555;font-size:10px;'>Total</th>" +
  "</tr>");

        decimal subTotal = 0;
        int sno = 1;

        string symbol = dr["CurrencyCode"].ToString() == "INR" ? "Rs." :
                       dr["CurrencyCode"].ToString() == "USD" ? "$" :
                       dr["CurrencyCode"].ToString() == "GBP" ? "&pound;" :
                       dr["CurrencyCode"].ToString() == "EUR" ? "&euro;" : "";

        foreach (DataRow row in dt.Rows)
        {
            decimal amount = row["Amount"] != DBNull.Value ? Convert.ToDecimal(row["Amount"]) : 0;
            subTotal += amount;
            string bgColor = (sno % 2 == 1) ? "#f5f5f5" : "#ffffff";
            
            string descText = SafeValue(row, "Description");
            string[] descLines = descText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string formattedDesc = "";
            if (descLines.Length > 0)
            {
                formattedDesc += "<strong>" + descLines[0] + "</strong><br/>";
                for (int i = 1; i < descLines.Length; i++)
                {
                    formattedDesc += descLines[i] + "<br/>";
                }
            }

            html.AppendLine(
            "<tr style='background:" + bgColor + "; height:48px;'>" +
            "<td align='center' style='border:0.1px solid #e0e0e0; padding:16px; line-height:2;'>"
                + sno++ + "</td>" +

            "<td style='border:0.1px solid #e0e0e0; padding:16px; line-height:2;'>"
                + formattedDesc + "</td>" +

            "<td align='left' style='border:0.1px solid #e0e0e0; padding:16px; line-height:2;'>" + symbol + " "
                + (amount % 1 == 0 ? amount.ToString("N0") : amount.ToString("N2")) + "</td>" +
            "</tr>");
        }

        if (subTotal == 0)
        {
            html.AppendLine(
            "<tr style='height:48px;'>" +
            "<td colspan='3' align='center' style='border:0.1px solid #eaeaea; color:#888;padding:16px; line-height:2;'>No items available</td>" +
            "</tr>");
        }

        decimal gst = dr["GSTAmount"] != DBNull.Value ? Convert.ToDecimal(dr["GSTAmount"]) : 0;
        decimal sgst = dr["SGSTAmount"] != DBNull.Value ? Convert.ToDecimal(dr["SGSTAmount"]) : 0;
        decimal cgst = dr["CGSTAmount"] != DBNull.Value ? Convert.ToDecimal(dr["CGSTAmount"]) : 0;
        decimal igst = dr["IGSTAmount"] != DBNull.Value ? Convert.ToDecimal(dr["IGSTAmount"]) : 0;
        decimal total = dr["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(dr["TotalAmount"]) : 0;
       
        string CurrencyCode = dr["CurrencyCode"].ToString();
        string totalInWords = ConvertAmountToWords(total, CurrencyCode);

        if (CurrencyCode == "INR")
        {
            if (igst > 0)
            {
                html.AppendLine(
                "<tr  style='height:48px;' >" +
                "<td colspan='2' align='right' style='border:0.1px solid #e0e0e0; padding:16px;'>IGST (18%)</td>" +
                "<td align='left' style='border:0.1px solid #e0e0e0; padding:16px;'>" + symbol + " "
                + (igst % 1 == 0 ? igst.ToString("N0") : igst.ToString("N2")) + "</td>" +
                "</tr>");
            }
            else
            {

                html.AppendLine(
                "<tr  style='height:48px;'>" +
                "<td colspan='2' align='right' style='border:0.1px solid #e0e0e0; padding:16px;'>SGST (9%)</td>" +
                "<td align='left' style='border:0.1px solid #e0e0e0; padding:16px;'>" + symbol + " "
                + (sgst % 1 == 0 ? sgst.ToString("N0") : sgst.ToString("N2")) + "</td>" +
                "</tr>");

                html.AppendLine(
                "<tr  style='height:48px;'>" +
                "<td colspan='2' align='right' style='border:0.1px solid #e0e0e0; padding:16px;'>CGST (9%)</td>" +
                "<td align='left' style='border:0.1px solid #e0e0e0; padding:16px;'>" + symbol + " "
                + (cgst % 1 == 0 ? cgst.ToString("N0") : cgst.ToString("N2")) + "</td>" +
                "</tr>");
            }
        }

        html.AppendLine(
    "<tr style='height:48px;'>" +
    "<td colspan='2' align='right' " +
    "style='border:0.1px solid #e0e0e0; padding:18px; font-weight:bold;'>Total</td>" +
    "<td align='left' " +
    "style='border:0.1px solid #e0e0e0; padding:18px; font-weight:bold;'>"
    + symbol + " " + (total % 1 == 0 ? total.ToString("N0") : total.ToString("N2")) +
    "</td>" +
    "</tr>");

        html.AppendLine("</table>");
        html.AppendLine("<p style='clear:both; font-size:12px; margin-top:20px; font-weight:bold;'>");
        html.AppendLine("Amount in words: " + totalInWords + "");
        html.AppendLine("</p>");

        html.AppendLine("<table width='100%' cellpadding='0' cellspacing='0' style='margin-top:25px;'>");
        html.AppendLine("<tr>");

        /* ================= LEFT SIDE ================= */
        html.AppendLine("<td valign='top' align='left'>");

        html.AppendLine("<p style='font-size:13px; margin-top:15px; line-height:1.9; color:#000000; '>");
        html.AppendLine(
        "<strong><a style='color:#000000; text-decoration:underline; text-underline-offset:3px;'>Account details</a></strong><br/>");
        html.AppendLine("Account name: Infologia Technologies Pvt Ltd<br/>");
        html.AppendLine("Account number: 10167976090<br/>");
        html.AppendLine("IFSC Code: IDFB008138<br/>");
        html.AppendLine("Swift code: IDFBINBBMUM<br/>");
        html.AppendLine("Bank: IDFC First Bank");
        html.AppendLine("</p>");

        html.AppendLine("</td>");

        /* ================= RIGHT SIDE ================= */
        html.AppendLine("<td valign='top' align='right' width='30%'>");

        html.AppendLine("<div style='text-align:right; font-size:12px;'>");

        html.AppendLine("<table align='right' cellpadding='0' cellspacing='0'>");
        html.AppendLine("<tr>");
        html.AppendLine("<td style='text-align:right;'>");

        /* Wrapper for overlap */
        html.AppendLine("<div style='position:relative; width:120px; margin-left:auto;'>");

        /* Seal */
        html.AppendLine(
        "<img src='file:///" +
        Server.MapPath("../images/AdminProfilePictures/sealwithsign.png").Replace("\\", "/") +
        "' width='120' style='opacity:0.6; display:block;'/>");

        html.AppendLine("</div>");

        html.AppendLine("</td>");
        html.AppendLine("</tr>");
        html.AppendLine("</table>");

        html.AppendLine("<p style='text-align:right;line-height:1.8;'>");
        html.AppendLine("<strong>DHANARUBAN VELUSAMY</strong><br/>");
        html.AppendLine("(CHIEF EXECUTIVE OFFICER)");
        html.AppendLine("</p>");

        html.AppendLine("</div>");
        html.AppendLine("</td>");

        html.AppendLine("</tr>");
        html.AppendLine("</table>");








        html.AppendLine("<style>");
        html.AppendLine(".footer { width:100%; font-size:14px; color:#555; margin-top:200px; }");
        html.AppendLine(".footer-center { text-align:center; font-style:italic; margin-bottom:12px; }");
        html.AppendLine(".social { text-align:left; }");
        html.AppendLine(".social-item { display:inline-block; margin-left:12px; font-size:13px; }");
        html.AppendLine(".social-item img { vertical-align:middle; margin-right:4px; }");
        html.AppendLine("</style>");

        html.AppendLine("<div class='footer'>");
        
        /* CENTER TEXT */
        html.AppendLine("<div class='footer-center'>");
        html.AppendLine("THANK YOU FOR YOUR BUSINESS!");
        html.AppendLine("</div>");

        /* LAST LINE – SOCIAL ICONS (AT ABSOLUTE BOTTOM) */
        html.AppendLine("<table width='100%' cellpadding='0' cellspacing='0' style='margin-top:100px;'>");
        html.AppendLine("<tr>");
        html.AppendLine("<td></td>"); // left empty
        html.AppendLine("<td class='social' width='40%'>");

        /* Facebook */
        html.AppendLine("<span class='social-item'>");
        html.AppendLine("<img src='file:///" + Server.MapPath("~/images/social/fb.png").Replace("\\", "/") + "' width='14' style='vertical-align:middle; margin-right:6px;'/>");
        html.AppendLine("<span> @infologiatechnologies</span>");
        html.AppendLine("</span>");

        /* Twitter */
        html.AppendLine("<span class='social-item'>");
        html.AppendLine("<img src='file:///" + Server.MapPath("~/images/social/twitter.png").Replace("\\", "/") + "' width='14' style='vertical-align:middle; margin-right:6px;'/>");
        html.AppendLine("<span> @infologiailt</span>");
        html.AppendLine("</span>");

        /* LinkedIn */
        html.AppendLine("<span class='social-item'>");
        html.AppendLine("<img src='file:///" + Server.MapPath("~/images/social/linkedin.png").Replace("\\", "/") + "' width='14' style='vertical-align:middle; margin-right:6px;'/>");
        html.AppendLine("<span> @infologia</span>");
        html.AppendLine("</span>");

        html.AppendLine("</td>");
        html.AppendLine("</tr>");
        html.AppendLine("</table>");

        html.AppendLine("</div>");





        using (MemoryStream ms = new MemoryStream())
        {
            using (Document doc = new Document(PageSize.A4, 20f, 20f, 20f, 20f))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                using (StringReader sr = new StringReader(html.ToString()))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);
                }

                doc.Close();
            }

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Invoice_" + dr["InvoiceNumber"] + ".pdf");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }

    private string SafeValue(DataRow row, string column)
    {
        return row[column] != DBNull.Value ? row[column].ToString() : string.Empty;
    }

    public string ConvertAmountToWords(decimal amount, string currencyCode)
    {
        long mainUnit = (long)amount;
        int subUnit = (int)((amount - mainUnit) * 100);

        string mainUnitName = currencyCode == "USD" ? "dollars" :
                              currencyCode == "GBP" ? "pounds" :
                              currencyCode == "EUR" ? "euros" : "rupees";

        string subUnitName = currencyCode == "USD" ? "cents" :
                             currencyCode == "GBP" ? "pence" :
                             currencyCode == "EUR" ? "cents" : "paise";

        string words = NumberToWords(mainUnit) + " " + mainUnitName;

        if (subUnit > 0)
            words += " and " + NumberToWords(subUnit) + " " + subUnitName;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words + " only");
    }

    private string NumberToWords(long number)
    {
        if (number == 0)
            return "zero";

        if (number < 0)
            return "minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 10000000) > 0)
        {
            words += NumberToWords(number / 10000000) + " crore ";
            number %= 10000000;
        }

        if ((number / 100000) > 0)
        {
            words += NumberToWords(number / 100000) + " lakh ";
            number %= 100000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            string[] unitsMap = { "zero","one","two","three","four","five","six","seven","eight","nine","ten",
                               "eleven","twelve","thirteen","fourteen","fifteen","sixteen","seventeen",
                               "eighteen","nineteen" };
            string[] tensMap = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words.Trim();
    }


}