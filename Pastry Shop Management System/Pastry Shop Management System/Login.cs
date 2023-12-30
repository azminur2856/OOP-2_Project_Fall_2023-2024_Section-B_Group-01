using MaterialSkin.Controls;
using System;
using System.Data;
using System.Windows.Forms;

namespace Pastry_Shop_Management_System
{
    public partial class Login : MaterialForm
    {
        private bool passwordVisible = false; // Track password visibility state
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            // Initially set the password to be masked
            materialTextBoxPassword.UseSystemPasswordChar = true; // Mask characters
            materialTextBoxPassword.TrailingIcon = imageListLogin.Images[0];
            passwordVisible = false;

            this.KeyPreview = true;
            this.KeyDown += Login_KeyDown;
        }
        private void materialTextBoxPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxPassword.UseSystemPasswordChar = false; // Show actual characters
                materialTextBoxPassword.TrailingIcon = imageListLogin.Images[1];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxPassword.UseSystemPasswordChar = true; // Mask characters
                materialTextBoxPassword.TrailingIcon = imageListLogin.Images[0];
            }
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Perform the login action by triggering the Click event of the login button.
                materialButtonLogin.PerformClick();
                e.SuppressKeyPress = true; // Prevent the beep sound
            }
        }

        private void materialButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = materialTextBoxUserName.Text;
                string password = materialTextBoxPassword.Text;

                DataAccess dataAccess = new DataAccess();
                PasswordManager passwordManager = new PasswordManager();

                // Query to check if the entered username exists for Admin
                string checkAdminQuery = $"SELECT A_Id, A_Name, A_Username, A_Password FROM Admin WHERE A_Username = '{username}'";
                DataSet adminDataSet = dataAccess.ExecuteQuery(checkAdminQuery);

                // Query to check if the entered username exists for Employee
                string checkEmployeeQuery = $"SELECT E_Id, P_Name, E_Username, E_Password, E_Status FROM Employee INNER JOIN Person ON Employee.P_Id = Person.P_Id WHERE E_Username = '{username}'";
                DataSet employeeDataSet = dataAccess.ExecuteQuery(checkEmployeeQuery);

                if (adminDataSet != null && adminDataSet.Tables.Count > 0 && adminDataSet.Tables[0].Rows.Count > 0)
                {
                    // Admin username found
                    string storedHashedPassword = adminDataSet.Tables[0].Rows[0]["A_Password"].ToString();
                    bool passwordMatched = passwordManager.VerifyPassword(password, storedHashedPassword);

                    if (passwordMatched)
                    {
                        string aId = adminDataSet.Tables[0].Rows[0]["A_Id"].ToString();
                        string aName = adminDataSet.Tables[0].Rows[0]["A_Name"].ToString();
                        string userName = adminDataSet.Tables[0].Rows[0]["A_Username"].ToString();

                        // Create an Admin object with the retrieved data
                        Admin loggedInAdmin = new Admin(aId, aName, userName);
                        loggedInAdmin.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password for admin account.");
                        return;
                    }
                }
                else if (employeeDataSet != null && employeeDataSet.Tables.Count > 0 && employeeDataSet.Tables[0].Rows.Count > 0)
                {
                    // Employee username found
                    string storedHashedPassword = employeeDataSet.Tables[0].Rows[0]["E_Password"].ToString();
                    string employeeStatus = employeeDataSet.Tables[0].Rows[0]["E_Status"].ToString();
                    bool passwordMatched = passwordManager.VerifyPassword(password, storedHashedPassword);

                    if (passwordMatched)
                    {
                        if (employeeStatus == "Disabled")
                        {
                            MessageBox.Show("Employee account is disabled. Please contact an admin to enable your account.");
                            return;
                        }
                        else
                        {
                            string eId = employeeDataSet.Tables[0].Rows[0]["E_Id"].ToString();
                            string eName = employeeDataSet.Tables[0].Rows[0]["P_Name"].ToString(); // Using P_Name instead of E_Name
                            string eUserName = employeeDataSet.Tables[0].Rows[0]["E_Username"].ToString();

                            // Create an Employee object with the retrieved data
                            Employee employee = new Employee(eId, eName, eUserName);
                            employee.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password for employee account.");
                        return;
                    }
                }
                else
                {
                    // Username not found in both Admin and Employee tables
                    MessageBox.Show("Invalid username or password.");
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void linkLabelActivation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdminSignUp adminSignUp = new AdminSignUp();
            adminSignUp.Show();
            this.Hide();
        }

        private void linkLabelForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            this.Hide();
        }
    }
}
