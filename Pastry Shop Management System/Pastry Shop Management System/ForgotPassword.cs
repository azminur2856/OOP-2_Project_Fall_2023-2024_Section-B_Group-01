using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Data;
using System.Windows.Forms;

namespace Pastry_Shop_Management_System
{
    public partial class ForgotPassword : MaterialForm
    {
        readonly MaterialSkinManager materialSkinManager;
        private bool passwordVisible = false; // Track password visibility state
        public ForgotPassword()
        {
            InitializeComponent();

            // Initializin Materialskin
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.Amber700, Primary.Amber900, Primary.Amber800, Accent.Amber200, TextShade.WHITE);
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo800, Primary.Indigo900, Primary.Indigo500, Accent.Indigo400, TextShade.WHITE);
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.Red800, Primary.Red900, Primary.Red900,Accent.Red700, TextShade.WHITE);

        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {
            // Initially hide some controls
            // Admin
            labelAdminName.Visible = false;
            labelAdminMaskEmail.Visible = false;
            materialTextBoxAdminEmail.Visible = false;
            materialButtonAdminVerify.Visible = false;
            materialTextBoxAdminEnterPassword.Visible = false;
            materialTextBoxAdminReEnterPassword.Visible = false;
            materialButtonAdminPasswordUpdate.Visible = false;

            // Employee
            labelEmployeeName.Visible = false;
            labelEmployeeMaskEmail.Visible = false;
            materialTextBoxEmployeeEmail.Visible = false;
            materialButtonEmployeeVerify.Visible = false;
            materialTextBoxEmployeeEnterPassword.Visible = false;
            materialTextBoxEmployeeReEnterPassword.Visible = false;
            materialButtonEmployeePasswordUpdate.Visible = false;

            // Initially set the password to be masked
            // Admin
            materialTextBoxAdminEnterPassword.UseSystemPasswordChar = true; // Mask characters
            materialTextBoxAdminEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
            passwordVisible = false;

            materialTextBoxAdminReEnterPassword.UseSystemPasswordChar = true; // Mask characters
            materialTextBoxAdminReEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
            passwordVisible = false;

            // Employee
            materialTextBoxEmployeeEnterPassword.UseSystemPasswordChar = true; // Mask characters
            materialTextBoxEmployeeEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
            passwordVisible = false;

            materialTextBoxEmployeeReEnterPassword.UseSystemPasswordChar = true; // Mask characters
            materialTextBoxEmployeeReEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
            passwordVisible = false;
        }

        // Admin
        private void materialTextBoxAdminEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxAdminEnterPassword.UseSystemPasswordChar = false; // Show actual characters
                materialTextBoxAdminEnterPassword.TrailingIcon = imageListForgotPassword.Images[3];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxAdminEnterPassword.UseSystemPasswordChar = true; // Mask characters
                materialTextBoxAdminEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
                passwordVisible = false;
            }
        }

        private void materialTextBoxAdminReEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxAdminReEnterPassword.UseSystemPasswordChar = false; // Show actual characters
                materialTextBoxAdminReEnterPassword.TrailingIcon = imageListForgotPassword.Images[3];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxAdminReEnterPassword.UseSystemPasswordChar = true; // Mask characters
                materialTextBoxAdminReEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
                passwordVisible = false;
            }
        }

        // Method to mask email address
        private string MaskEmail(string email)
        {
            int atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                string localPart = email.Substring(0, atIndex);
                string domainPart = email.Substring(atIndex + 1); // Exclude '@'

                // Mask part of the local email address
                string maskedLocalPart = MaskCharacters(localPart, 2, 3);

                // Mask part of the domain
                string maskedDomainPart = MaskCharacters(domainPart, 2, 4);

                return $"{maskedLocalPart}@{maskedDomainPart}";
            }

            return email;
        }

        private string MaskCharacters(string str, int visibleCharsAtStart, int visibleCharsAtEnd)
        {
            if (str.Length <= visibleCharsAtStart + visibleCharsAtEnd)
            {
                return new string('*', str.Length);
            }

            string visibleStart = str.Substring(0, visibleCharsAtStart);
            string visibleEnd = str.Substring(str.Length - visibleCharsAtEnd);

            return $"{visibleStart}{new string('*', str.Length - visibleCharsAtStart - visibleCharsAtEnd)}{visibleEnd}";
        }

        private string adminId;
        private string adminEmail;

        private void materialButtonAdminSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string input = materialTextBoxAdminUsernameOrId.Text.Trim();
                if (!string.IsNullOrEmpty(input))
                {
                    // Check if the input is an ID or Username
                    string searchQuery = $"SELECT A_ID, A_Name, A_Email FROM Admin WHERE A_Id = '{input}' OR A_Username = '{input}'";

                    DataAccess dataAccess = new DataAccess();
                    DataSet adminDataSet = dataAccess.ExecuteQuery(searchQuery);

                    if (adminDataSet != null && adminDataSet.Tables.Count > 0 && adminDataSet.Tables[0].Rows.Count > 0)
                    {
                        // Admin found
                        adminId = adminDataSet.Tables[0].Rows[0]["A_Id"].ToString();
                        string adminName = adminDataSet.Tables[0].Rows[0]["A_Name"].ToString();
                        adminEmail = adminDataSet.Tables[0].Rows[0]["A_Email"].ToString();

                        // Show labelAdminName and populate with adminName
                        labelAdminName.Visible = true;
                        labelAdminName.Text = "Name: " + adminName;

                        // Show labelAdminMaskEmail and populate with masked email
                        labelAdminMaskEmail.Visible = true;
                        labelAdminMaskEmail.Text = "Email Hints: " + MaskEmail(adminEmail); // Assuming a method exists to mask the email

                        // Show materialTextBoxAdminEmail and materialButtonAdminVerify
                        materialTextBoxAdminEmail.Visible = true;
                        materialButtonAdminVerify.Visible = true;
                    }
                    else
                    {
                        // Admin not found
                        MessageBox.Show("Admin not found.");
                        labelAdminName.Visible = false;
                        labelAdminMaskEmail.Visible = false;
                        materialTextBoxAdminEmail.Visible = false;
                        materialButtonAdminVerify.Visible = false;
                        materialTextBoxAdminEnterPassword.Visible = false;
                        materialTextBoxAdminReEnterPassword.Visible = false;
                        materialButtonAdminPasswordUpdate.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter your username or ID.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void materialButtonAdminVerify_Click(object sender, EventArgs e)
        {
            try
            {
                string inputEmail = materialTextBoxAdminEmail.Text.Trim();

                if (!string.IsNullOrEmpty(inputEmail))
                {
                    if (adminEmail.Equals(inputEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        // If the provided email matches the fetched admin's email, reveal password update fields
                        materialTextBoxAdminEnterPassword.Visible = true;
                        materialTextBoxAdminReEnterPassword.Visible = true;
                        materialButtonAdminPasswordUpdate.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Email does not match. Please enter the correct email.");
                        materialTextBoxAdminEnterPassword.Visible = false;
                        materialTextBoxAdminReEnterPassword.Visible = false;
                        materialButtonAdminPasswordUpdate.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter your email to verify.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void materialButtonAdminPasswordUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string newPassword = materialTextBoxAdminEnterPassword.Text;
                string confirmNewPassword = materialTextBoxAdminReEnterPassword.Text;

                // Query to fetch the current password of the admin by admin ID
                string getPasswordQuery = $"SELECT A_Password FROM Admin WHERE A_Id = '{adminId}'";

                DataAccess dataAccess = new DataAccess();
                DataSet passwordDataSet = dataAccess.ExecuteQuery(getPasswordQuery);

                if (passwordDataSet != null && passwordDataSet.Tables.Count > 0 && passwordDataSet.Tables[0].Rows.Count > 0)
                {
                    // Admin object to
                    Admin admin = new Admin();

                    //string currentPassword = passwordDataSet.Tables[0].Rows[0]["A_Password"].ToString();
                    if (admin.IsPasswordValid(newPassword))
                    {
                        if (newPassword == confirmNewPassword)
                        {
                            PasswordManager passwordManageer = new PasswordManager();
                            newPassword = passwordManageer.HashPassword(newPassword);

                            string updatePasswordQuery = $"UPDATE Admin SET A_Password = '{newPassword}' WHERE A_Id = '{adminId}'";
                            int rowsAffected = dataAccess.ExecuteDMLQuery(updatePasswordQuery);

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Password updated successfully!");

                                Login login = new Login();
                                login.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update password.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Passwords do not match.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid password format.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid admin ID.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void materialButtonAdminBack_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }




        // Employee
        private void materialTextBoxEmployeeEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxEmployeeEnterPassword.UseSystemPasswordChar = false; // Show actual characters
                materialTextBoxEmployeeEnterPassword.TrailingIcon = imageListForgotPassword.Images[3];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxEmployeeEnterPassword.UseSystemPasswordChar = true; // Mask characters
                materialTextBoxEmployeeEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
                passwordVisible = false;
            }
        }

        private void materialTextBoxEmployeeReEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxEmployeeReEnterPassword.UseSystemPasswordChar = false; // Show actual characters
                materialTextBoxEmployeeReEnterPassword.TrailingIcon = imageListForgotPassword.Images[3];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxEmployeeReEnterPassword.UseSystemPasswordChar = true; // Mask characters
                materialTextBoxEmployeeReEnterPassword.TrailingIcon = imageListForgotPassword.Images[2];
                passwordVisible = false;
            }
        }

        private string employeeId;
        private string employeeEmail;

        private void materialButtonEmployeeSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string input = materialTextBoxEmployeeUsernameOrId.Text.Trim();
                if (!string.IsNullOrEmpty(input))
                {
                    // Check if the input is an ID or Username
                    string searchQuery = $"SELECT E_Id, P_Name, E_Email FROM Employee INNER JOIN Person ON Employee.P_Id = Person.P_Id WHERE Employee.E_Id = '{input}' OR Employee.E_Username = '{input}'";

                    DataAccess dataAccess = new DataAccess();
                    DataSet employeeDataSet = dataAccess.ExecuteQuery(searchQuery);

                    if (employeeDataSet != null && employeeDataSet.Tables.Count > 0 && employeeDataSet.Tables[0].Rows.Count > 0)
                    {
                        // Employee found
                        employeeId = employeeDataSet.Tables[0].Rows[0]["E_Id"].ToString();
                        string employeeName = employeeDataSet.Tables[0].Rows[0]["P_Name"].ToString(); // Using P_Name from Person table
                        employeeEmail = employeeDataSet.Tables[0].Rows[0]["E_Email"].ToString();

                        // Show labelEmployeeName and populate with employeeName
                        labelEmployeeName.Visible = true;
                        labelEmployeeName.Text = "Name: " + employeeName;

                        // Show labelEmployeeMaskEmail and populate with masked email
                        labelEmployeeMaskEmail.Visible = true;
                        labelEmployeeMaskEmail.Text = "Email Hints: " + MaskEmail(employeeEmail); // Assuming a method exists to mask the email

                        // Show materialTextBoxEmployeeEmail and materialButtonEmployeeVerify
                        materialTextBoxEmployeeEmail.Visible = true;
                        materialButtonEmployeeVerify.Visible = true;

                        // Hide password update fields (if shown previously)
                        materialTextBoxEmployeeEnterPassword.Visible = false;
                        materialTextBoxEmployeeReEnterPassword.Visible = false;
                        materialButtonEmployeePasswordUpdate.Visible = false;
                    }
                    else
                    {
                        // Employee not found
                        MessageBox.Show("Employee not found.");
                        labelEmployeeName.Visible = false;
                        labelEmployeeMaskEmail.Visible = false;
                        materialTextBoxEmployeeEmail.Visible = false;
                        materialButtonEmployeeVerify.Visible = false;

                        // Hide password update fields (if shown previously)
                        materialTextBoxEmployeeEnterPassword.Visible = false;
                        materialTextBoxEmployeeReEnterPassword.Visible = false;
                        materialButtonEmployeePasswordUpdate.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter your username or ID.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void materialButtonEmployeeVerify_Click(object sender, EventArgs e)
        {
            try
            {
                string inputEmail = materialTextBoxEmployeeEmail.Text.Trim();

                if (!string.IsNullOrEmpty(inputEmail))
                {
                    if (employeeEmail.Equals(inputEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        // If the provided email matches the fetched employee's email, reveal password update fields
                        materialTextBoxEmployeeEnterPassword.Visible = true;
                        materialTextBoxEmployeeReEnterPassword.Visible = true;
                        materialButtonEmployeePasswordUpdate.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Email does not match. Please enter the correct email.");
                        materialTextBoxEmployeeEnterPassword.Visible = false;
                        materialTextBoxEmployeeReEnterPassword.Visible = false;
                        materialButtonEmployeePasswordUpdate.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter your email to verify.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void materialButtonEmployeePasswordUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string newPassword = materialTextBoxEmployeeEnterPassword.Text;
                string confirmNewPassword = materialTextBoxEmployeeReEnterPassword.Text;

                // Query to fetch the current password of the employee by employee ID
                string getPasswordQuery = $"SELECT E_Password FROM Employee WHERE E_Id = '{employeeId}'";

                DataAccess dataAccess = new DataAccess();
                DataSet passwordDataSet = dataAccess.ExecuteQuery(getPasswordQuery);

                if (passwordDataSet != null && passwordDataSet.Tables.Count > 0 && passwordDataSet.Tables[0].Rows.Count > 0)
                {
                    // Admin object to
                    Admin admin = new Admin();
                    //string currentPassword = passwordDataSet.Tables[0].Rows[0]["E_Password"].ToString();
                    if (admin.IsPasswordValid(newPassword))
                    {
                        if (newPassword == confirmNewPassword)
                        {
                            PasswordManager passwordManageer = new PasswordManager();
                            newPassword = passwordManageer.HashPassword(newPassword);

                            string updatePasswordQuery = $"UPDATE Employee SET E_Password = '{newPassword}' WHERE E_Id = '{employeeId}'";
                            int rowsAffected = dataAccess.ExecuteDMLQuery(updatePasswordQuery);

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Password updated successfully!");

                                Login login = new Login();
                                login.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update password.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Passwords do not match.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid password format.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid employee ID.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void materialButtonEmployeeBack_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
