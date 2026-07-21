using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;

public partial class WEB_InternOnboarding : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Internship Onboarding Form";

            if (Request.QueryString["id"] != null)
            {
                btn_register.Text = "Update Application";
                LoadInternData(Request.QueryString["id"].ToString());
            }
            else
            {
                // Auto-generate Intern Code
                txt_intern_code.Text = GenerateInternCode();
            }
        }
    }

    private void LoadInternData(string id)
    {
        string query = "SELECT * FROM IT_InternOnboarding WHERE ID = @ID";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@ID", id);
        DataSet ds = DA.GetDataSet(cmd);
        
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            
            txt_intern_code.Text = row["InternCode"].ToString();
            txt_name.Text = row["FullName"].ToString();
            
            if (row["DOB"] != DBNull.Value)
                txt_dob.Text = Convert.ToDateTime(row["DOB"]).ToString("dd/MM/yyyy");
                
            if (row["Gender"] != DBNull.Value)
                rd_gander.SelectedValue = row["Gender"].ToString();
                
            txt_email.Text = row["Email"].ToString();
            txt_phone.Text = row["Phonenumber"].ToString();
            txt_emergency_name.Text = row["EmergencyContactName"].ToString();
            txt_emergency_number.Text = row["EmergencyContactNumber"].ToString();
            txt_blood_group.Text = row["BloodGroup"].ToString();
            txt_permanent_address.Text = row["PermanentAddress"].ToString();
            txt_present_address.Text = row["PresentAddress"].ToString();
            txt_university.Text = row["University"].ToString();
            txt_course.Text = row["Course"].ToString();
            txt_year_of_study.Text = row["YearOfStudy"].ToString();
            txt_internship_duration.Text = row["InternshipDuration"].ToString();
            txt_department.Text = row["Department"].ToString();
            
            if (row["StartDate"] != DBNull.Value)
                txt_start_date.Text = Convert.ToDateTime(row["StartDate"]).ToString("dd/MM/yyyy");
            if (row["EndDate"] != DBNull.Value)
                txt_end_date.Text = Convert.ToDateTime(row["EndDate"]).ToString("dd/MM/yyyy");
                
            txt_digital_signature.Text = row["DigitalSignature"].ToString();
            
            if (row["AgreementDate"] != DBNull.Value)
                txt_agreement_date.Text = Convert.ToDateTime(row["AgreementDate"]).ToString("dd/MM/yyyy");
                
            // Save existing file paths in ViewState
            ViewState["ProfileImage"] = row["ProfileImage"].ToString();
            ViewState["ResumeDoc"] = row["ResumeDoc"].ToString();
            ViewState["AadharDoc"] = row["AadharDoc"].ToString();
            ViewState["PANDoc"] = row["PANDoc"].ToString();
            ViewState["PassportDoc"] = row["PassportDoc"].ToString();
            ViewState["TenthMarkDoc"] = row["TenthMarkDoc"].ToString();
            ViewState["TwelfthMarkDoc"] = row["TwelfthMarkDoc"].ToString();
            ViewState["DegreeDoc"] = row["DegreeDoc"].ToString();
            ViewState["BonafideDoc"] = row["BonafideDoc"].ToString();
            
            // Disable RequiredFieldValidators for files if they already exist
            if (!string.IsNullOrEmpty(row["ProfileImage"].ToString())) RequiredFieldValidator_img.Enabled = false;
            if (!string.IsNullOrEmpty(row["ResumeDoc"].ToString())) RequiredFieldValidator_Resume.Enabled = false;
            if (!string.IsNullOrEmpty(row["AadharDoc"].ToString())) RequiredFieldValidator_Aadhar.Enabled = false;
            if (!string.IsNullOrEmpty(row["TenthMarkDoc"].ToString())) RequiredFieldValidator_10th.Enabled = false;
            if (!string.IsNullOrEmpty(row["TwelfthMarkDoc"].ToString())) RequiredFieldValidator_12th.Enabled = false;
            if (!string.IsNullOrEmpty(row["DegreeDoc"].ToString())) RequiredFieldValidator_Degree.Enabled = false;
            if (!string.IsNullOrEmpty(row["BonafideDoc"].ToString())) RequiredFieldValidator_Bonafide.Enabled = false;
            
            chk_agreement.Checked = true;
        }
    }

    private string GenerateInternCode()
    {
        string newCode = "ILTIN0001";
        try
        {
            // Assuming the table name is IT_InternOnboarding and the column is InternCode.
            // You may need to update this query once your database table is finalized.
            string query = "SELECT TOP 1 InternCode FROM IT_InternOnboarding ORDER BY InternCode DESC";
            DataTable dt = DA.GetDataTable(query);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                string lastCode = dt.Rows[0][0].ToString(); // e.g. ILTIN0001
                if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith("ILTIN"))
                {
                    string numberPart = lastCode.Substring(5);
                    int number = 0;
                    if (int.TryParse(numberPart, out number))
                    {
                        number++;
                        newCode = "ILTIN" + number.ToString("D4"); // Pads with leading zeros
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // If the table doesn't exist yet, it will catch the error and return the default code ILTIN0001
        }
        return newCode;
    }

    protected void btn_register_Click(object sender, EventArgs e)
    {
        try
        {
            bool isUpdate = Request.QueryString["id"] != null;
            string str_sql = "";

            if (isUpdate)
            {
                str_sql = @"UPDATE IT_InternOnboarding SET
                    FullName = @FullName, DOB = @DOB, Gender = @Gender, Email = @Email, Phonenumber = @Phonenumber, 
                    EmergencyContactName = @EmergencyContactName, EmergencyContactNumber = @EmergencyContactNumber, BloodGroup = @BloodGroup, 
                    PermanentAddress = @PermanentAddress, PresentAddress = @PresentAddress, University = @University, Course = @Course, 
                    YearOfStudy = @YearOfStudy, InternshipDuration = @InternshipDuration, Department = @Department, StartDate = @StartDate, 
                    EndDate = @EndDate, DigitalSignature = @DigitalSignature, AgreementDate = @AgreementDate, 
                    ProfileImage = @ProfileImage, ResumeDoc = @ResumeDoc, AadharDoc = @AadharDoc, PANDoc = @PANDoc, PassportDoc = @PassportDoc, 
                    TenthMarkDoc = @TenthMarkDoc, TwelfthMarkDoc = @TwelfthMarkDoc, DegreeDoc = @DegreeDoc, BonafideDoc = @BonafideDoc
                    WHERE ID = @ID";
            }
            else
            {
                str_sql = @"INSERT INTO IT_InternOnboarding (
                    InternCode, FullName, DOB, Gender, Email, Phonenumber, 
                    EmergencyContactName, EmergencyContactNumber, BloodGroup, 
                    PermanentAddress, PresentAddress, University, Course, 
                    YearOfStudy, InternshipDuration, Department, StartDate, 
                    EndDate, DigitalSignature, AgreementDate, IsActive, Createdby,
                    ProfileImage, ResumeDoc, AadharDoc, PANDoc, PassportDoc, 
                    TenthMarkDoc, TwelfthMarkDoc, DegreeDoc, BonafideDoc
                ) VALUES (
                    @InternCode, @FullName, @DOB, @Gender, @Email, @Phonenumber, 
                    @EmergencyContactName, @EmergencyContactNumber, @BloodGroup, 
                    @PermanentAddress, @PresentAddress, @University, @Course, 
                    @YearOfStudy, @InternshipDuration, @Department, @StartDate, 
                    @EndDate, @DigitalSignature, @AgreementDate, 1, @Createdby,
                    @ProfileImage, @ResumeDoc, @AadharDoc, @PANDoc, @PassportDoc, 
                    @TenthMarkDoc, @TwelfthMarkDoc, @DegreeDoc, @BonafideDoc
                )";
            }

            SqlCommand cmd = new SqlCommand(str_sql);

            if (isUpdate)
            {
                cmd.Parameters.AddWithValue("@ID", Request.QueryString["id"].ToString());
            }

            // Save Image File
            string profileImageName = "";
            if (up_img.HasFile)
            {
                string filename = System.IO.Path.GetFileName(up_img.FileName);
                string extension = System.IO.Path.GetExtension(filename);
                profileImageName = txt_intern_code.Text + "_Profile_" + Guid.NewGuid().ToString().Substring(0, 5) + extension;
                
                string dir_path = Server.MapPath("~/images/InternProfilePictures/");
                if (!System.IO.Directory.Exists(dir_path))
                {
                    System.IO.Directory.CreateDirectory(dir_path);
                }
                
                string str_path = dir_path + profileImageName;
                up_img.SaveAs(str_path);
            }
            else if (isUpdate && ViewState["ProfileImage"] != null)
            {
                profileImageName = ViewState["ProfileImage"].ToString();
            }
            cmd.Parameters.AddWithValue("@ProfileImage", profileImageName);

            // Save individual documents
            cmd.Parameters.AddWithValue("@ResumeDoc", SaveDocumentWithFallback(up_resume, "Resume", "ResumeDoc", isUpdate));
            cmd.Parameters.AddWithValue("@AadharDoc", SaveDocumentWithFallback(up_aadhar, "Aadhar", "AadharDoc", isUpdate));
            cmd.Parameters.AddWithValue("@PANDoc", SaveDocumentWithFallback(up_pan, "PAN", "PANDoc", isUpdate));
            cmd.Parameters.AddWithValue("@PassportDoc", SaveDocumentWithFallback(up_passport, "Passport", "PassportDoc", isUpdate));
            cmd.Parameters.AddWithValue("@TenthMarkDoc", SaveDocumentWithFallback(up_10th, "10th", "TenthMarkDoc", isUpdate));
            cmd.Parameters.AddWithValue("@TwelfthMarkDoc", SaveDocumentWithFallback(up_12th, "12th", "TwelfthMarkDoc", isUpdate));
            cmd.Parameters.AddWithValue("@DegreeDoc", SaveDocumentWithFallback(up_degree, "Degree", "DegreeDoc", isUpdate));
            cmd.Parameters.AddWithValue("@BonafideDoc", SaveDocumentWithFallback(up_bonafide, "Bonafide", "BonafideDoc", isUpdate));

            cmd.Parameters.AddWithValue("@InternCode", txt_intern_code.Text);
            cmd.Parameters.AddWithValue("@FullName", txt_name.Text);
            
            // Parse Dates safely from DD/MM/YYYY
            DateTime dob, startDate, endDate, agreementDate;
            if (DateTime.TryParseExact(txt_dob.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                cmd.Parameters.AddWithValue("@DOB", dob);
            else
                cmd.Parameters.AddWithValue("@DOB", DBNull.Value);

            cmd.Parameters.AddWithValue("@Gender", rd_gander.SelectedValue);
            cmd.Parameters.AddWithValue("@Email", txt_email.Text);
            cmd.Parameters.AddWithValue("@Phonenumber", txt_phone.Text);
            cmd.Parameters.AddWithValue("@EmergencyContactName", txt_emergency_name.Text);
            cmd.Parameters.AddWithValue("@EmergencyContactNumber", txt_emergency_number.Text);
            cmd.Parameters.AddWithValue("@BloodGroup", txt_blood_group.Text);
            cmd.Parameters.AddWithValue("@PermanentAddress", txt_permanent_address.Text);
            cmd.Parameters.AddWithValue("@PresentAddress", txt_present_address.Text);
            cmd.Parameters.AddWithValue("@University", txt_university.Text);
            cmd.Parameters.AddWithValue("@Course", txt_course.Text);
            cmd.Parameters.AddWithValue("@YearOfStudy", txt_year_of_study.Text);
            cmd.Parameters.AddWithValue("@InternshipDuration", txt_internship_duration.Text);
            cmd.Parameters.AddWithValue("@Department", txt_department.Text);

            if (DateTime.TryParseExact(txt_start_date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                cmd.Parameters.AddWithValue("@StartDate", startDate);
            else
                cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);

            if (DateTime.TryParseExact(txt_end_date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                cmd.Parameters.AddWithValue("@EndDate", endDate);
            else
                cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);

            cmd.Parameters.AddWithValue("@DigitalSignature", txt_digital_signature.Text);

            if (DateTime.TryParseExact(txt_agreement_date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out agreementDate))
                cmd.Parameters.AddWithValue("@AgreementDate", agreementDate);
            else
                cmd.Parameters.AddWithValue("@AgreementDate", DBNull.Value);

            // Handle Guid securely
            Guid createdByGuid;
            if (Guid.TryParse(this.SC.Userid, out createdByGuid))
                cmd.Parameters.AddWithValue("@Createdby", createdByGuid);
            else
                cmd.Parameters.AddWithValue("@Createdby", DBNull.Value);

            DA.ExecuteNonQuery(cmd);

            ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Onboarding Details Submitted Successfully!');window.location.href='EmployeeView.aspx';</script>");
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Error: " + ex.Message.Replace("'", "\\'") + "');</script>");
        }
    }

    private object SaveDocumentWithFallback(FileUpload fileUpload, string prefix, string viewStateKey, bool isUpdate)
    {
        if (fileUpload != null && fileUpload.HasFile)
        {
            string filename = System.IO.Path.GetFileName(fileUpload.FileName);
            string extension = System.IO.Path.GetExtension(filename);
            string newFileName = txt_intern_code.Text + "_" + prefix + "_" + Guid.NewGuid().ToString().Substring(0, 5) + extension;
            string dir_path = Server.MapPath("~/images/InternDocuments/");
            if (!System.IO.Directory.Exists(dir_path))
            {
                System.IO.Directory.CreateDirectory(dir_path);
            }
            string str_path = dir_path + newFileName;
            fileUpload.SaveAs(str_path);
            return newFileName;
        }
        else if (isUpdate && ViewState[viewStateKey] != null && !string.IsNullOrEmpty(ViewState[viewStateKey].ToString()))
        {
            return ViewState[viewStateKey].ToString();
        }
        return DBNull.Value;
    }
}
