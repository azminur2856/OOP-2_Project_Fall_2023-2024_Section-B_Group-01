using iTextSharp.text;
using iTextSharp.text.pdf;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// Need to do wark for print person name 
namespace Pastry_Shop_Management_System
{
    public partial class Admin : MaterialForm
    {
        readonly MaterialSkinManager materialSkinManager;
        private bool passwordVisible = false; // Track password visibility state
        public Admin()
        {
            InitializeComponent();

            // Clock
            timerClock.Start();

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



        //Parameterized Constructor for pasiing data from login form
        private readonly string aid;
        private readonly string name;
        private readonly string userName;
        public Admin(string aid, string name, string userName) : this()
        {
            this.aid = aid;
            this.name = name;
            this.userName = userName;
        }

        // Form Load Operations
        private void Admin_Load(object sender, EventArgs e)
        {
            // Set the text of the form to be the name of the logged in admin
            this.Text = "Welcome " + name + " (Administrator)";
            materialTextBoxAdminId.Text = aid;
            materialTextBoxAdminName.Text = name;
            materialTextBoxAdminUsername.Text = userName;

            // Initially set the password to be masked
            materialTextBoxEnterPassword.UseSystemPasswordChar = true;
            materialTextBoxEnterPassword.TrailingIcon = imageListAdmin.Images[6];
            passwordVisible = false;

            materialTextBoxReEnterPassword.UseSystemPasswordChar = true;
            materialTextBoxReEnterPassword.TrailingIcon = imageListAdmin.Images[6];
            passwordVisible = false;

            materialTextBoxEmployeePassword.UseSystemPasswordChar = true;
            materialTextBoxEmployeePassword.TrailingIcon = imageListAdmin.Images[6];
            passwordVisible = false;

            // Dashboard
            loadCount();
            //Employee
            CustomizeDataGridView(dataGridViewEmployee);
            materialButtonEmployeeStatus.Enabled = false;
            LoadEmployeeData();
            LoadShopNamesForEmployeeComboBox();
            //Factory
            CustomizeDataGridView(dataGridViewFactory);
            LoadFactoryData();
            //Shop
            CustomizeDataGridView(dataGridViewShop);
            LoadFactoryNamesForShopComboBox();
            LoadShopData();
            //Item
            CustomizeDataGridView(dataGridViewItem);
            LoadShopNamesForItemComboBox();
            LoadItemData();
            //Customer
            CustomizeDataGridView(dataGridViewCustomer);
            LoadCustomerData();
        }



        //All Dashboard related code
        public void DrawHand(Graphics g, int centerX, int centerY, float length, float angle, Pen pen, int thickness)
        {
            float x = centerX + length * (float)Math.Sin(angle * Math.PI / 180);
            float y = centerY - length * (float)Math.Cos(angle * Math.PI / 180);
            g.DrawLine(new Pen(pen.Color, thickness), centerX, centerY, x, y);
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            pictureBoxClock.Invalidate(); // Redraw the clock on each timer tick
        }

        private void pictureBoxClock_Paint(object sender, PaintEventArgs e)
        {
            int centerX = pictureBoxClock.Width / 2;
            int centerY = pictureBoxClock.Height / 2;
            int clockRadius = Math.Min(centerX, centerY) - 10;

            // Draw clock face
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(Brushes.White, centerX - clockRadius, centerY - clockRadius, clockRadius * 2, clockRadius * 2);
            e.Graphics.DrawEllipse(Pens.Black, centerX - clockRadius, centerY - clockRadius, clockRadius * 2, clockRadius * 2);

            // Calculate hour, minute, and second angles
            int hours = DateTime.Now.Hour % 12;
            int minutes = DateTime.Now.Minute;
            int seconds = DateTime.Now.Second;

            float hourAngle = 360 * (hours + minutes / 60.0f) / 12;
            float minuteAngle = 360 * (minutes + seconds / 60.0f) / 60;
            float secondAngle = 360 * seconds / 60;

            // Draw clock hands
            DrawHand(e.Graphics, centerX, centerY, clockRadius * 0.5f, hourAngle, Pens.Gold, 6);
            DrawHand(e.Graphics, centerX, centerY, clockRadius * 0.7f, minuteAngle, Pens.Black, 4);
            DrawHand(e.Graphics, centerX, centerY, clockRadius * 0.9f, secondAngle, Pens.Red, 2);
        }

        private void materialSwitchThemeChange_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitchThemeChange.Checked)
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                this.materialSwitchThemeChange.Text = "Light Mode";

                this.dataGridViewEmployee.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewEmployee.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewEmployee.BackgroundColor = Color.Black;


                this.dataGridViewFactory.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewFactory.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewFactory.BackgroundColor = Color.Black;


                this.dataGridViewShop.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewShop.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewShop.BackgroundColor = Color.Black;

                this.dataGridViewItem.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewItem.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewItem.BackgroundColor = Color.Black;

                this.dataGridViewCustomer.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewCustomer.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewCustomer.BackgroundColor = Color.Black;
            }
            else
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                this.materialSwitchThemeChange.Text = "Dark Mode";

                this.dataGridViewEmployee.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewEmployee.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewEmployee.BackgroundColor = Color.White;


                this.dataGridViewFactory.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewFactory.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewFactory.BackgroundColor = Color.White;


                this.dataGridViewShop.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewShop.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewShop.BackgroundColor = Color.White;

                this.dataGridViewItem.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewItem.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewItem.BackgroundColor = Color.White;

                this.dataGridViewCustomer.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewCustomer.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewCustomer.BackgroundColor = Color.White;
            }
        }

        public void CustomizeDataGridView(DataGridView dataGridView)
        {
            // Customize DataGridView appearance
            dataGridView.DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.GridColor = Color.Black;
            dataGridView.BorderStyle = BorderStyle.Fixed3D;
            dataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView.RowsDefaultCellStyle.SelectionBackColor = Color.DarkGray;
            dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Customize individual columns if needed
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                // Example: Change the header color of specific columns
                column.HeaderCell.Style.BackColor = Color.LightGray;

                // Add more customizations for specific columns if required
                // Example: Change font styles, column widths, etc.
            }
        }

        public bool IsPasswordValid(string password)
        {
            // Define regex patterns for uppercase, lowercase, digit, and special character
            Regex uppercaseRegex = new Regex(@"[A-Z]");
            Regex lowercaseRegex = new Regex(@"[a-z]");
            Regex digitRegex = new Regex(@"[0-9]");
            Regex specialCharRegex = new Regex(@"[@_!#$%^&*()<>?/\|}{~:]");

            // Check length
            if (password.Length < 8 || password.Length > 20)
                return false;

            // Check if password contains at least one character from each category
            if (!uppercaseRegex.IsMatch(password) ||
                !lowercaseRegex.IsMatch(password) ||
                !digitRegex.IsMatch(password) ||
                !specialCharRegex.IsMatch(password))
                return false;

            // Password contains at least one character from each category
            return true;
        }

        private void materialButtonUpdateAdminNewPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string adminId = materialTextBoxAdminId.Text;
                string newPassword = materialTextBoxEnterPassword.Text;
                string confirmNewPassword = materialTextBoxReEnterPassword.Text;

                // Query to fetch the current password of the admin by admin ID
                string getPasswordQuery = $"SELECT A_Password FROM Admin WHERE A_Id = '{adminId}'";

                DataAccess dataAccess = new DataAccess();
                DataSet passwordDataSet = dataAccess.ExecuteQuery(getPasswordQuery);

                if (passwordDataSet != null && passwordDataSet.Tables.Count > 0 && passwordDataSet.Tables[0].Rows.Count > 0)
                {
                    //string currentPassword = passwordDataSet.Tables[0].Rows[0]["A_Password"].ToString();
                    if (IsPasswordValid(newPassword))
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
                                // Clear the password fields after updating password
                                materialTextBoxEnterPassword.Text = "";
                                materialTextBoxReEnterPassword.Text = "";
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

        private void materialButtonLogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void materialTextBoxEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxEnterPassword.UseSystemPasswordChar = false;
                materialTextBoxEnterPassword.TrailingIcon = imageListAdmin.Images[7];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxEnterPassword.UseSystemPasswordChar = true;
                materialTextBoxEnterPassword.TrailingIcon = imageListAdmin.Images[6];
                passwordVisible = false;
            }
        }

        private void materialTextBoxReEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxReEnterPassword.UseSystemPasswordChar = false;
                materialTextBoxReEnterPassword.TrailingIcon = imageListAdmin.Images[7];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxReEnterPassword.UseSystemPasswordChar = true;
                materialTextBoxReEnterPassword.TrailingIcon = imageListAdmin.Images[6];
                passwordVisible = false;
            }
        }

        public int GetTotalCount(string tableName)
        {
            int totalCount = 0;
            DataAccess dataAccess = new DataAccess();
            string query = $"SELECT COUNT(*) FROM dbo.{tableName}";

            try
            {
                DataSet result = dataAccess.ExecuteQuery(query);

                if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    totalCount = Convert.ToInt32(result.Tables[0].Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return totalCount;
        }


        private void loadCount()
        {
            // Admin
            string A = "Admin";
            string E = "Employee";
            string F = "Factory";
            string S = "Shop";
            string I = "Item";
            string C = "Customer";
            materialTextBoxAdminTotal.Text = "Total Admin: " + GetTotalCount(A);
            materialTextBoxEmployeeTotal.Text = "Total Employee: " + GetTotalCount(E);
            materialTextBoxFactoryTotal.Text = "Total Factory: " + GetTotalCount(F);
            materialTextBoxShopTotal.Text = "Total Shop: " + GetTotalCount(S);
            materialTextBoxItemTotal.Text = "Total Item: " + GetTotalCount(I);
            materialTextBoxCustomerTotal.Text = "Total Customer: " + GetTotalCount(C);
        }



        //Person
        public string AutoPersonId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"SELECT TOP 1 P_Id FROM Person ORDER BY P_Id DESC;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');

                if (id.Length == 2 && id[0] == "P")
                {
                    if (int.TryParse(id[1], out int newIdNum))
                    {
                        newIdNum++; // Increment the ID
                        string newId = "P-" + newIdNum.ToString().PadLeft(9, '0'); // Padding to ensure two digits
                        return newId;
                    }
                }
            }

            // If no rows are returned or an invalid ID format is encountered, start the ID from "P-001"
            return "P-000000001";
        }



        //All Employee related code
        private void materialTextBoxEmployeePassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxEmployeePassword.UseSystemPasswordChar = false;
                materialTextBoxEmployeePassword.TrailingIcon = imageListAdmin.Images[7];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxEmployeePassword.UseSystemPasswordChar = true;
                materialTextBoxEmployeePassword.TrailingIcon = imageListAdmin.Images[6];
                passwordVisible = false;
            }
        }

        private string AutoEmployeeId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"SELECT TOP 1 E_Id FROM Employee ORDER BY E_Id DESC;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');

                if (id.Length == 2 && id[0] == "E")
                {
                    if (int.TryParse(id[1], out int newIdNum))
                    {
                        newIdNum++; // Increment the ID
                        string newId = "E-" + newIdNum.ToString().PadLeft(3, '0'); // Padding to ensure two digits
                        return newId;
                    }
                }
            }

            // If no rows are returned or an invalid ID format is encountered, start the ID from "E-001"
            return "E-001";
        }
        public bool IsPhoneNumberValid(string phoneNumber)
        {
            // Regular expression pattern to match Bangladeshi phone numbers
            string bdPhoneNumberpattern = @"^(?:\+?88)?01[3-9]\d{8}$";//@"^\+?(88)?01[0-9]{9}$";

            // Check if the phone number matches the pattern
            if (Regex.IsMatch(phoneNumber, bdPhoneNumberpattern))
            {
                return true; // Valid Bangladeshi phone number
            }
            else
            {
                return false; // Invalid Bangladeshi phone number
            }
        }

        private bool IsPhoneNumberUniqueForEmployee(string phoneNumber)
        {
            bool isUnique = false;

            DataAccess dataAccess = new DataAccess();

            // Check if the phone number is not in the Person table
            string query = $"SELECT COUNT(*) FROM Person WHERE P_PhoneNumber = '{phoneNumber}'";
            int count = Convert.ToInt32(dataAccess.ExecuteQuery(query).Tables[0].Rows[0][0]);

            if (count == 0)
            {
                isUnique = true; // Phone number is not in the Person table
            }
            else
            {
                // Check if the phone number is associated with an Employee
                query = $"SELECT COUNT(*) FROM Employee WHERE P_Id IN (SELECT P_Id FROM Person WHERE P_PhoneNumber = '{phoneNumber}')";
                count = Convert.ToInt32(dataAccess.ExecuteQuery(query).Tables[0].Rows[0][0]);

                if (count == 0)
                {
                    isUnique = true; // Phone number is in the Person table, but not associated with an Employee
                }
            }

            return isUnique;
        }

        public bool IsPhoneNumberUniqueForUpdate(string phoneNumber, string personId = null)
        {
            // Query to check if the phone number already exists in the Person table
            string checkPhoneNumberQuery = string.IsNullOrEmpty(personId) ?
                $"SELECT COUNT(*) FROM Person WHERE P_PhoneNumber = '{phoneNumber}'" :
                $"SELECT COUNT(*) FROM Person WHERE P_PhoneNumber = '{phoneNumber}' AND P_Id != '{personId}'";

            // Use DataAccess to execute the query and check if count > 0 (phone number exists)
            DataAccess dataAccess = new DataAccess();
            DataSet countDataSet = dataAccess.ExecuteQuery(checkPhoneNumberQuery);

            if (countDataSet != null && countDataSet.Tables.Count > 0)
            {
                int phoneNumberCount = Convert.ToInt32(countDataSet.Tables[0].Rows[0][0]);
                return phoneNumberCount == 0; // Returns true if the phone number doesn't exist (or doesn't exist for other persons), else false
            }

            return false;
        }

        public bool IsEmailValid(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$";

            // Check if the email matches the pattern
            if (Regex.IsMatch(email, emailPattern))
            {
                return true; // Valid email
            }
            else
            {
                return false; // Invalid email
            }
        }

        public bool IsEmailUniqueForAdminAndEmployee(string email, string employeeId = null)
        {
            // Query to check if the email already exists in the Employee table or Admin table
            string checkEmailQuery = string.IsNullOrEmpty(employeeId) ?
                $"SELECT (SELECT COUNT(*) FROM Employee WHERE E_Email = '{email}') AS EmployeeCount, (SELECT COUNT(*) FROM Admin WHERE A_Email = '{email}') AS AdminCount" :
                $"SELECT (SELECT COUNT(*) FROM Employee WHERE E_Email = '{email}' AND E_Id != '{employeeId}') AS EmployeeCount, (SELECT COUNT(*) FROM Admin WHERE A_Email = '{email}') AS AdminCount";


            // Use DataAccess to execute the query and check if count > 0 (email exists)
            DataAccess dataAccess = new DataAccess();
            DataSet countDataSet = dataAccess.ExecuteQuery(checkEmailQuery);

            if (countDataSet != null && countDataSet.Tables.Count > 0)
            {
                int employeeEmailCount = Convert.ToInt32(countDataSet.Tables[0].Rows[0]["EmployeeCount"]);
                int adminEmailCount = Convert.ToInt32(countDataSet.Tables[0].Rows[0]["AdminCount"]);

                // Returns true if the email doesn't exist in both Employee and Admin tables (or doesn't exist for other employees), else false
                return employeeEmailCount == 0 && adminEmailCount == 0;
            }

            return false;
        }

        public bool IsUsernameValid(string username)
        {
            // Define criteria for a valid username
            // For example, username should be between 3 and 20 characters long,
            // and it should consist of only letters, numbers, underscores, and dashes.
            // It should start with a letter (not a digit), not contain uppercase letters,
            // and only allow underscores or dashes as special characters.

            // Regular expression pattern to match allowed characters
            string pattern = @"^(?![\d_])(?!.*[A-Z])[a-z0-9](?:[a-z0-9_-]{1,18}[a-z0-9])?$";

            // Check if the username matches the pattern
            if (System.Text.RegularExpressions.Regex.IsMatch(username, pattern))
            {
                return true; // Username is valid
            }
            else
            {
                return false; // Username is invalid
            }
        }

        public bool IsUsernameUnique(string username, string employeeId = null)
        {
            // Query to check if the username already exists in the Employee or Admin table
            string checkUsernameQuery = string.IsNullOrEmpty(employeeId) ?
                $"SELECT (SELECT COUNT(*) FROM Employee WHERE E_Username = '{username}') AS EmployeeCount, (SELECT COUNT(*) FROM Admin WHERE A_Username = '{username}') AS AdminCount" :
                $"SELECT (SELECT COUNT(*) FROM Employee WHERE E_Username = '{username}' AND E_Id != '{employeeId}') AS EmployeeCount, (SELECT COUNT(*) FROM Admin WHERE A_Username = '{username}') AS AdminCount";

            // Use DataAccess to execute the query and check if count > 0 (username exists)
            DataAccess dataAccess = new DataAccess();
            DataSet countDataSet = dataAccess.ExecuteQuery(checkUsernameQuery);

            if (countDataSet != null && countDataSet.Tables.Count > 0 && countDataSet.Tables[0].Rows.Count > 0)
            {
                int employeeUsernameCount = Convert.ToInt32(countDataSet.Tables[0].Rows[0]["EmployeeCount"]);
                int adminUsernameCount = Convert.ToInt32(countDataSet.Tables[0].Rows[0]["AdminCount"]);

                // Returns true if the username doesn't exist in both Employee and Admin tables (or doesn't exist for other employees), else false
                return employeeUsernameCount == 0 && adminUsernameCount == 0;
            }
            return false;
        }

        public string GetPersonIdByPhoneNumber(string phoneNumber)
        {
            DataAccess dataAccess = new DataAccess();

            string query = $"SELECT P_Id FROM Person WHERE P_PhoneNumber = '{phoneNumber}'";

            DataSet dataSet = dataAccess.ExecuteQuery(query);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return dataSet.Tables[0].Rows[0]["P_Id"].ToString();
            }

            return null;
        }

        private string GetPersonIdByEmployeeId(string employeeId)
        {
            // Retrieve the associated person ID (P_Id) based on the employee ID
            string query = $"SELECT P_Id FROM Employee WHERE E_Id = '{employeeId}'";
            DataAccess dataAccess = new DataAccess();
            DataSet dataSet = dataAccess.ExecuteQuery(query);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return dataSet.Tables[0].Rows[0]["P_Id"].ToString();
            }

            return null; // Return null if the associated person ID is not found
        }

        // Function to get Shop ID by Shop Name from the database
        public string GetShopIdByName(string shopName)
        {
            try
            {
                DataAccess dataAccess = new DataAccess();
                string query = $"SELECT S_Id FROM Shop WHERE S_Name = '{shopName}'";
                var result = dataAccess.ExecuteQuery(query);

                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    return result.Tables[0].Rows[0]["S_Id"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching Shop ID: {ex.Message}");
            }

            return null;
        }

        private bool IsValidToSaveEmployee()
        {
            return !String.IsNullOrEmpty(this.materialTextBoxEmployeeName.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeeUsername.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeePassword.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeePhoneNumber.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeeEmail.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeeAddress.Text) &&
                   double.TryParse(this.materialTextBoxEmployeeSalary.Text, out double salary) && // Check if the salary is a valid number
                   salary > 0; // Ensure salary is greater than zero
        }

        private bool IsValidToUpdateEmployee()
        {
            return !String.IsNullOrEmpty(this.materialTextBoxEmployeeName.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeeAddress.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeePhoneNumber.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeeEmail.Text) &&
                   //!String.IsNullOrEmpty(this.materialTextBoxEmployeePassword.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEmployeeUsername.Text) &&
                   double.TryParse(this.materialTextBoxEmployeeSalary.Text, out double salary) && // Check if the salary is a valid number
                   salary > 0; // Ensure salary is greater than zero
        }

        private void LoadEmployeeData(string searchValue = "")
        {
            try
            {
                string sqlQuery;

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    // SQL query to select Employee data along with related Person and Shop details if searchValue is empty
                    sqlQuery = "SELECT E.E_Id AS 'Employee ID', P.P_Name As 'Name', E.E_Username AS 'Username', P.P_PhoneNumber AS 'Phone Number', P.P_Gender AS 'Gender', E.E_Email AS 'Email', E.E_Address AS 'Address', E.E_Salary AS 'Salary', E.E_Status AS 'Status', S.S_Name AS 'Shop Name' " +
                               "FROM Employee E " +
                               "INNER JOIN Person P ON E.P_Id = P.P_Id " +
                               "INNER JOIN Shop S ON E.S_Id = S.S_Id";
                }
                else
                {
                    // SQL query to select Employee data based on the searchValue with related Person and Shop details, filtered by name
                    sqlQuery = "SELECT E.E_Id AS 'Employee ID', P.P_Name As 'Name', E.E_Username AS 'Username', P.P_PhoneNumber AS 'Phone Number', P.P_Gender AS 'Gender', E.E_Email AS 'Email', E.E_Address AS 'Address', E.E_Salary AS 'Salary', E.E_Status AS 'Status', S.S_Name AS 'Shop Name' " +
                                $"FROM Employee E " +
                                $"INNER JOIN Person P ON E.P_Id = P.P_Id " +
                                $"INNER JOIN Shop S ON E.S_Id = S.S_Id WHERE P.P_Name LIKE '%{searchValue}%' OR P.P_PhoneNumber LIKE '%{searchValue}%'";
                }

                DataAccess dataAccess = new DataAccess();
                DataSet employeeDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (employeeDataSet != null && employeeDataSet.Tables.Count > 0)
                {
                    dataGridViewEmployee.DataSource = employeeDataSet.Tables[0];

                    // Auto-size columns based on content
                    dataGridViewEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Set specific widths for columns
                    dataGridViewEmployee.Columns["Employee ID"].Width = 100;
                    dataGridViewEmployee.Columns["Name"].Width = 200;
                    dataGridViewEmployee.Columns["Username"].Width = 100;
                    dataGridViewEmployee.Columns["Phone Number"].Width = 150;
                    dataGridViewEmployee.Columns["Gender"].Width = 100;
                    dataGridViewEmployee.Columns["Email"].Width = 200;
                    dataGridViewEmployee.Columns["Address"].Width = 250;
                    dataGridViewEmployee.Columns["Salary"].Width = 150;
                    dataGridViewEmployee.Columns["Status"].Width = 100;
                    dataGridViewEmployee.Columns["Shop Name"].Width = 100;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading employee data: {exc.Message}");
            }
        }

        private void ClearAllEmployeeInput()
        {
            materialTextBoxEmployeeId.Text = "";
            materialTextBoxEmployeeName.Text = "";
            materialTextBoxEmployeeUsername.Text = "";
            materialTextBoxEmployeePassword.Text = "";
            materialRadioButtonEmployeeMale.Checked = false;
            materialRadioButtonEmployeeFemale.Checked = false;
            materialTextBoxEmployeePhoneNumber.Text = "";
            materialTextBoxEmployeeEmail.Text = "";
            materialComboBoxEmployeeShop.SelectedIndex = -1;
            materialTextBoxEmployeeSalary.Text = "";
            materialTextBoxEmployeeAddress.Text = "";
            materialButtonEmployeeStatus.Enabled = false;
            materialButtonEmployeeStatus.Text = "Status";
        }

        private void LoadShopNamesForEmployeeComboBox()
        {
            try
            {
                // SQL query to select all Shop names
                string sqlQuery = "SELECT S_Name FROM Shop";

                DataAccess dataAccess = new DataAccess();
                DataSet shopDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (shopDataSet != null && shopDataSet.Tables.Count > 0)
                {
                    // Clear existing items in the combo box
                    materialComboBoxEmployeeShop.Items.Clear();

                    // Populate the combo box with Shop names
                    foreach (DataRow row in shopDataSet.Tables[0].Rows)
                    {
                        string shopName = row["S_Name"].ToString();
                        materialComboBoxEmployeeShop.Items.Add(shopName);
                    }

                    // Optionally, set a default value or the first item as selected
                    if (materialComboBoxEmployeeShop.Items.Count > 0)
                    {
                        materialComboBoxEmployeeShop.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading Shop names: {ex.Message}");
            }
        }

        private void materialButtonEmployeeAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidToSaveEmployee())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                string employeeName = materialTextBoxEmployeeName.Text;
                string employeeUsername = materialTextBoxEmployeeUsername.Text;
                string employeePhoneNumber = materialTextBoxEmployeePhoneNumber.Text;
                string employeeEmail = materialTextBoxEmployeeEmail.Text;
                string employeeAddress = materialTextBoxEmployeeAddress.Text;
                string employeeSalary = materialTextBoxEmployeeSalary.Text;
                string employeePassword = materialTextBoxEmployeePassword.Text;

                string employeeGender = "";

                if (materialRadioButtonEmployeeMale.Checked)
                {
                    employeeGender = "Male";
                }
                else if (materialRadioButtonEmployeeFemale.Checked)
                {
                    employeeGender = "Female";
                }

                string selectedShopName = materialComboBoxEmployeeShop.Text;
                string shopId = GetShopIdByName(selectedShopName);

                if (shopId == null)
                {
                    MessageBox.Show("Shop ID not found for the selected Shop Name.");
                    return;
                }

                DataAccess dataAccess = new DataAccess();

                if (!IsUsernameValid(employeeUsername))
                {
                    MessageBox.Show("Invalid username format.");
                    return;
                }

                if (!IsUsernameUnique(employeeUsername))
                {
                    MessageBox.Show("Username already exists. Please choose a different username.");
                    return;
                }

                if (!IsPasswordValid(employeePassword))
                {
                    MessageBox.Show("Invalid password format.");
                    return;
                }

                else if (IsPasswordValid(employeePassword))
                {
                    PasswordManager passwordManageer = new PasswordManager();
                    employeePassword = passwordManageer.HashPassword(employeePassword);
                }

                if (!IsPhoneNumberValid(employeePhoneNumber))
                {
                    MessageBox.Show("Invalid phone number.");
                    return;
                }

                if (!IsPhoneNumberUniqueForEmployee(employeePhoneNumber))
                {
                    MessageBox.Show("Phone number already exists.");
                    return;
                }

                if (!IsEmailValid(employeeEmail))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                if (!IsEmailUniqueForAdminAndEmployee(employeeEmail))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }

                string personId = GetPersonIdByPhoneNumber(employeePhoneNumber);

                if (personId == null)
                {
                    personId = AutoPersonId();

                    string personQuery = $"INSERT INTO Person (P_Id, P_Name, P_Gender, P_PhoneNumber) " +
                                         $"VALUES ('{personId}', '{employeeName}', '{employeeGender}', '{employeePhoneNumber}')";

                    int personRowsAffected = dataAccess.ExecuteDMLQuery(personQuery);

                    if (personRowsAffected <= 0)
                    {
                        MessageBox.Show("Failed to add person details.");
                        return;
                    }
                }

                string employeeId = AutoEmployeeId();

                string employeeQuery = $"INSERT INTO Employee (E_Id, E_Username, E_Password, E_Email, E_Address, E_Salary, E_Status, P_Id, S_Id) " +
                                       $"VALUES ('{employeeId}', '{employeeUsername}', '{employeePassword}', '{employeeEmail}', '{employeeAddress}', '{employeeSalary}', 'Enabled', '{personId}', '{shopId}')";

                int employeeRowsAffected = dataAccess.ExecuteDMLQuery(employeeQuery);

                if (employeeRowsAffected > 0)
                {
                    MessageBox.Show("Employee added successfully!");
                    ClearAllEmployeeInput();
                    LoadEmployeeData();
                    // Customer
                    LoadCustomerData();
                    // Dashboard
                    loadCount();
                }
                else
                {
                    MessageBox.Show("Failed to add employee.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void dataGridViewEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridViewEmployee.Rows[e.RowIndex];

                    // Retrieve data from the selected row's cells
                    string employeeId = selectedRow.Cells["Employee ID"].Value.ToString();
                    string employeeName = selectedRow.Cells["Name"].Value.ToString();
                    string employeeUsername = selectedRow.Cells["Username"].Value.ToString();
                    string employeeGender = selectedRow.Cells["Gender"].Value.ToString();
                    string employeePhoneNumber = selectedRow.Cells["Phone Number"].Value.ToString();
                    string employeeEmail = selectedRow.Cells["Email"].Value.ToString();
                    string shopName = selectedRow.Cells["Shop Name"].Value.ToString();
                    string employeeSalary = selectedRow.Cells["Salary"].Value.ToString();
                    string employeeStatus = selectedRow.Cells["Status"].Value.ToString();
                    string employeeAddress = selectedRow.Cells["Address"].Value.ToString();

                    // Populate UI elements with the retrieved data
                    materialTextBoxEmployeeId.Text = employeeId;
                    materialTextBoxEmployeeName.Text = employeeName;
                    materialTextBoxEmployeeUsername.Text = employeeUsername;
                    if (employeeGender == "Male")
                    {
                        materialRadioButtonEmployeeMale.Checked = true;
                        materialRadioButtonEmployeeFemale.Checked = false;
                    }
                    else if (employeeGender == "Female")
                    {
                        materialRadioButtonEmployeeMale.Checked = false;
                        materialRadioButtonEmployeeFemale.Checked = true;
                    }
                    else
                    {
                        materialRadioButtonEmployeeMale.Checked = false;
                        materialRadioButtonEmployeeFemale.Checked = false;
                    }
                    materialTextBoxEmployeePhoneNumber.Text = employeePhoneNumber;
                    materialTextBoxEmployeeEmail.Text = employeeEmail;
                    materialComboBoxEmployeeShop.SelectedItem = shopName;
                    materialTextBoxEmployeeSalary.Text = employeeSalary;
                    materialTextBoxEmployeeAddress.Text = employeeAddress;

                    // Status Button Configeration
                    materialButtonEmployeeStatus.Enabled = true;
                    if (employeeStatus == "Enabled")
                    {
                        materialButtonEmployeeStatus.Text = "Disabled";
                        //materialButtonEmployeeStatus.ForeColor = Color.Red;
                    }
                    else if (employeeStatus == "Disabled")
                    {
                        materialButtonEmployeeStatus.Text = "Enabled";
                        //materialButtonEmployeeStatus.ForeColor = Color.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButtonEmployeeUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if an employee is selected in the DataGridView
                if (dataGridViewEmployee.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an employee to update.");
                    return;
                }

                if (!IsValidToUpdateEmployee())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Fetch Employee details from UI elements
                string employeeId = materialTextBoxEmployeeId.Text;
                string employeeName = materialTextBoxEmployeeName.Text;
                string employeeUsername = materialTextBoxEmployeeUsername.Text;
                string employeePhoneNumber = materialTextBoxEmployeePhoneNumber.Text;
                string employeeEmail = materialTextBoxEmployeeEmail.Text;
                string employeeAddress = materialTextBoxEmployeeAddress.Text;
                string employeeSalary = materialTextBoxEmployeeSalary.Text;
                string employeePassword = materialTextBoxEmployeePassword.Text;

                if (string.IsNullOrEmpty(employeePassword))
                {
                    employeePassword = null;
                }
                else if (!IsPasswordValid(employeePassword))
                {
                    MessageBox.Show("Invalid password format.");
                    return;
                }
                else if (IsPasswordValid(employeePassword))
                {
                    PasswordManager passwordManageer = new PasswordManager();
                    employeePassword = passwordManageer.HashPassword(employeePassword);
                }

                string employeeGender = "";

                if (materialRadioButtonEmployeeMale.Checked)
                {
                    employeeGender = "Male";
                }
                else if (materialRadioButtonEmployeeFemale.Checked)
                {
                    employeeGender = "Female";
                }

                string selectedShopName = materialComboBoxEmployeeShop.Text;
                string shopId = GetShopIdByName(selectedShopName);

                if (shopId == null)
                {
                    MessageBox.Show("Shop ID not found for the selected Shop Name.");
                    return;
                }

                DataAccess dataAccess = new DataAccess();

                // Check if the username is valid
                if (!IsUsernameValid(employeeUsername))
                {
                    MessageBox.Show("Invalid username format.");
                    return;
                }

                // Check if the username is unique
                if (!IsUsernameUnique(employeeUsername, employeeId))
                {
                    MessageBox.Show("Username already exists. Please choose a different username.");
                    return;
                }
                if (!IsPhoneNumberValid(employeePhoneNumber))
                {
                    MessageBox.Show("Invalid phone number.");
                    return;
                }
                // Check if the phone number is unique and valid
                if (!IsPhoneNumberUniqueForUpdate(employeePhoneNumber, GetPersonIdByEmployeeId(employeeId)))
                {
                    MessageBox.Show("Phone number already exists.");
                    return;
                }
                // Check if the email is valid
                if (!IsEmailValid(employeeEmail))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }
                // Check if the email is unique and valid
                if (!IsEmailUniqueForAdminAndEmployee(employeeEmail, employeeId))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }

                // SQL query to update Person details
                string updatePersonQuery = $@"UPDATE Person SET 
                P_Name = '{employeeName}', 
                P_Gender = '{employeeGender}', 
                P_PhoneNumber = '{employeePhoneNumber}' 
                WHERE P_Id = (SELECT P_Id FROM Employee WHERE E_Id = '{employeeId}')";

                int personRowsAffected = dataAccess.ExecuteDMLQuery(updatePersonQuery);

                // SQL query to update Employee details
                string updateEmployeeQuery = $@"UPDATE Employee SET 
                E_Username = '{employeeUsername}', 
                E_Password = {(string.IsNullOrEmpty(employeePassword) ? "E_Password" : $"'{employeePassword}'")}, 
                E_Email = '{employeeEmail}', 
                E_Address = '{employeeAddress}', 
                E_Salary = '{employeeSalary}',
                S_Id = '{shopId}' 
                WHERE E_Id = '{employeeId}'";

                int employeeRowsAffected = dataAccess.ExecuteDMLQuery(updateEmployeeQuery);

                if (personRowsAffected > 0 && employeeRowsAffected > 0)
                {
                    MessageBox.Show("Employee updated successfully!");
                    ClearAllEmployeeInput();
                    LoadEmployeeData();
                    // Customer
                    LoadCustomerData();
                }
                else
                {
                    MessageBox.Show("Failed to update employee details.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void materialButtonEmployeeDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if an employee is selected in the DataGridView
                if (dataGridViewEmployee.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an employee to delete.");
                    return;
                }

                string employeeIdToDelete = materialTextBoxEmployeeId.Text;

                // Retrieve the associated person ID (P_Id) before deleting the employee
                string personIdToDelete = GetPersonIdByEmployeeId(employeeIdToDelete);

                // Confirmation message before deleting the record
                DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Perform deletion operation
                    string deleteEmployeeQuery = $"DELETE FROM Employee WHERE E_Id = '{employeeIdToDelete}'";
                    string deletePersonQuery = $"DELETE FROM Person WHERE P_Id = '{personIdToDelete}'";

                    DataAccess dataAccess = new DataAccess();
                    int employeeRowsAffected = dataAccess.ExecuteDMLQuery(deleteEmployeeQuery);
                    int personRowsAffected = dataAccess.ExecuteDMLQuery(deletePersonQuery);

                    if (employeeRowsAffected > 0 && personRowsAffected > 0)
                    {
                        MessageBox.Show("Employee deleted successfully!");
                        ClearAllEmployeeInput();
                        LoadEmployeeData();
                        // Customer
                        LoadCustomerData();
                        // Dashboard
                        loadCount();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete employee.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void materialTextBoxEmployeeSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = materialTextBoxEmployeeSearch.Text.Trim();
            LoadEmployeeData(searchValue);
        }

        private void materialButtonEmployeePrint_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlQuery = "SELECT E.E_Id AS 'Employee ID', P.P_Name As 'Name', E.E_Username AS 'Username', P.P_PhoneNumber AS 'Phone Number', P.P_Gender AS 'Gender', E.E_Email AS 'Email', E.E_Address AS 'Address', E.E_Salary AS 'Salary', E.E_Status AS 'Status', S.S_Name AS 'Shop Name' " +
                               "FROM Employee E " +
                               "INNER JOIN Person P ON E.P_Id = P.P_Id " +
                               "INNER JOIN Shop S ON E.S_Id = S.S_Id";

                DataAccess dataAccess = new DataAccess();
                DataSet employeeDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (employeeDataSet != null && employeeDataSet.Tables.Count > 0)
                {
                    // Create a PDF document
                    Document pdfDoc = new Document();
                    string fileName = $"EmployeeData_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"; // File name with current date and time
                    string filePath = Path.Combine(@"C:\Users\AZMINUR RAHMAN\Desktop\2023-2024, Fall\OBJECT ORIENTED PROGRAMMING 2\Project C#\Employee", fileName);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                    pdfDoc.Open();

                    // Set Times New Roman font for the entire document
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font18 = new iTextSharp.text.Font(baseFont, 18);
                    iTextSharp.text.Font font16 = new iTextSharp.text.Font(baseFont, 16);
                    iTextSharp.text.Font font14 = new iTextSharp.text.Font(baseFont, 14);
                    iTextSharp.text.Font font12 = new iTextSharp.text.Font(baseFont, 12);
                    iTextSharp.text.Font font11 = new iTextSharp.text.Font(baseFont, 11);

                    // Title section
                    Paragraph title = new Paragraph();
                    title.Alignment = Element.ALIGN_CENTER;
                    title.Font = font18;
                    title.Add(new Chunk("ABC Pastry Company", font18));
                    title.Add(new Chunk("\nEmployee Data\n", font16));
                    title.Add(new Chunk("Printed by: " + materialTextBoxAdminUsername.Text, font14));
                    title.Add(new Chunk("\n\n", font12));

                    pdfDoc.Add(title);

                    // Create a PdfPTable to hold the data
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(employeeDataSet.Tables[0].Columns.Count);
                    table.DefaultCell.Padding = 5; // Set cell padding

                    // Set table header font and height
                    foreach (DataColumn column in employeeDataSet.Tables[0].Columns)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, font12));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        headerCell.PaddingTop = 5; // Add padding to the top of each cell
                        headerCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                        //headerCell.FixedHeight = 25; // Set fixed height for header cells
                        table.AddCell(headerCell);
                    }

                    // Set table cell font and height
                    foreach (DataRow row in employeeDataSet.Tables[0].Rows)
                    {
                        foreach (object cell in row.ItemArray)
                        {
                            PdfPCell dataCell = new PdfPCell(new Phrase(cell.ToString(), font11));
                            dataCell.PaddingTop = 5; // Add padding to the top of each cell
                            dataCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                            dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(dataCell);
                        }
                    }

                    pdfDoc.Add(table);

                    pdfDoc.Close();

                    // Show a message indicating successful creation of the PDF file
                    MessageBox.Show("PDF file generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No data available to generate PDF.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while generating PDF: {exc.Message}\n{exc.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButtonEmployeeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewEmployee.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an employee.");
                    return;
                }

                // Get the selected employee's ID from the DataGridView
                string employeeIdToUpdateStatus = materialTextBoxEmployeeId.Text;

                DataAccess dataAccess = new DataAccess();

                // Retrieve the current status of the selected employee from the database
                string getStatusQuery = $"SELECT E_Status FROM Employee WHERE E_Id = '{employeeIdToUpdateStatus}'";
                DataSet statusDataSet = dataAccess.ExecuteQuery(getStatusQuery);

                if (statusDataSet.Tables.Count > 0 && statusDataSet.Tables[0].Rows.Count > 0)
                {
                    string currentStatus = statusDataSet.Tables[0].Rows[0]["E_Status"].ToString();

                    // Determine the new status based on the current status
                    string newStatus = (currentStatus == "Enabled") ? "Disabled" : "Enabled";

                    // Update the status of the employee in the database
                    string updateStatusQuery = $"UPDATE Employee SET E_Status = '{newStatus}' WHERE E_Id = '{employeeIdToUpdateStatus}'";
                    int rowsAffected = dataAccess.ExecuteDMLQuery(updateStatusQuery);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Employee status {newStatus.ToLower()}d successfully!");
                        ClearAllEmployeeInput();
                        LoadEmployeeData(); // Refresh the data in DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Failed to update employee status.");
                    }
                }
                else
                {
                    MessageBox.Show("No status found for the selected employee.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



        //All Factory related code
        private string AutoFactoryId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"SELECT TOP 1 F_Id FROM Factory ORDER BY F_Id DESC;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');

                if (id.Length == 2 && id[0] == "F")
                {
                    if (int.TryParse(id[1], out int newIdNum))
                    {
                        newIdNum++; // Increment the ID
                        string newId = "F-" + newIdNum.ToString().PadLeft(2, '0'); // Padding to ensure two digits
                        return newId;
                    }
                }
            }

            // If no rows are returned or an invalid ID format is encountered, start the ID from "F-01"
            return "F-01";
        }

        private bool IsValidToSaveFactory()
        {
            // Check if the required text fields are not empty or null
            return !String.IsNullOrEmpty(this.materialTextBoxFactoryName.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxFactoryAddress.Text);
        }

        private void LoadFactoryData(string searchValue = "")
        {
            try
            {
                string sqlQuery;

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    // SQL query to select all Factory data if searchValue is empty
                    sqlQuery = "SELECT F_Id AS 'Factory ID', F_Name AS 'Factory Name', F_Address AS 'Factory Address' FROM Factory";
                }
                else
                {
                    // SQL query to select Factory data based on the searchValue
                    sqlQuery = $"SELECT F_Id AS 'Factory ID', F_Name AS 'Factory Name', F_Address AS 'Factory Address' FROM Factory WHERE F_Name LIKE '%{searchValue}%'";
                }

                DataAccess dataAccess = new DataAccess();
                DataSet factoryDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (factoryDataSet != null && factoryDataSet.Tables.Count > 0)
                {
                    dataGridViewFactory.DataSource = factoryDataSet.Tables[0];

                    // Auto-size columns based on content
                    dataGridViewFactory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // set specific widths for columns
                    dataGridViewFactory.Columns["Factory ID"].Width = 100;
                    dataGridViewFactory.Columns["Factory Name"].Width = 150;
                    dataGridViewFactory.Columns["Factory Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Fill remaining space with Factory Address column
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading factory data: {exc.Message}");
            }
        }

        private void ClearAllFactoryInput()
        {
            materialTextBoxFactoryId.Text = "";
            materialTextBoxFactoryName.Text = "";
            materialTextBoxFactoryAddress.Text = "";
        }

        private void materialButtonFactoryAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Check validity before attempting to add factory details
                if (!IsValidToSaveFactory())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Fetch the Factory Name and Address from UI elements
                string factoryName = materialTextBoxFactoryName.Text;
                string factoryAddress = materialTextBoxFactoryAddress.Text;

                // Generate Auto-Generated Factory ID
                string factoryId = AutoFactoryId();

                // SQL query to insert data into the Factory table
                string sqlQuery = $"INSERT INTO Factory (F_Id, F_Name, F_Address) VALUES ('{factoryId}', '{factoryName}', '{factoryAddress}')";

                DataAccess dataAccess = new DataAccess();
                int rowsAffected = dataAccess.ExecuteDMLQuery(sqlQuery);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Factory added successfully!");
                    ClearAllFactoryInput();
                    LoadFactoryData();
                    LoadFactoryNamesForShopComboBox();
                    // Dashboard
                    loadCount();

                }
                else
                {
                    MessageBox.Show("Failed to add factory.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occurred due to invalid input.\nError Info:" + exc.Message);
            }
        }

        private void dataGridViewFactory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Check if the clicked cell is in a valid row
                {
                    DataGridViewRow selectedRow = dataGridViewFactory.Rows[e.RowIndex];

                    // Retrieve data from the selected row's cells
                    string fId = selectedRow.Cells["Factory ID"].Value.ToString();
                    string fName = selectedRow.Cells["Factory Name"].Value.ToString();
                    string fAddress = selectedRow.Cells["Factory Address"].Value.ToString();

                    // Populate UI elements with the retrieved data
                    materialTextBoxFactoryId.Text = fId;
                    materialTextBoxFactoryName.Text = fName;
                    materialTextBoxFactoryAddress.Text = fAddress;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButtonFactoryUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewFactory.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a factory to update.");
                    return;
                }
                // Check validity before attempting to update factory details
                if (!IsValidToSaveFactory())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Fetch the Factory ID, Name, and Address from UI elements
                string factoryIdToUpdate = materialTextBoxFactoryId.Text;
                string updatedFactoryName = materialTextBoxFactoryName.Text;
                string updatedFactoryAddress = materialTextBoxFactoryAddress.Text;

                // SQL query to update data in the Factory table
                string sqlQuery = $"UPDATE Factory SET F_Name = '{updatedFactoryName}', F_Address = '{updatedFactoryAddress}' WHERE F_Id = '{factoryIdToUpdate}'";

                DataAccess dataAccess = new DataAccess();
                int rowsAffected = dataAccess.ExecuteDMLQuery(sqlQuery);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Factory updated successfully!");
                    // Factory
                    ClearAllFactoryInput();
                    LoadFactoryData();
                    // Shop
                    LoadFactoryNamesForShopComboBox();
                    LoadShopData();
                }
                else
                {
                    MessageBox.Show("Failed to update factory.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occurred due to invalid input.\nError Info:" + exc.Message);
            }
        }

        // Function to retrieve associated shop names for a factory
        private List<string> GetAssociatedShopNames(string factoryId)
        {
            List<string> shopNames = new List<string>();

            DataAccess dataAccess = new DataAccess();
            string shopQuery = $"SELECT S_Name FROM Shop WHERE F_Id = '{factoryId}'";

            DataSet shopDataSet = dataAccess.ExecuteQuery(shopQuery);

            if (shopDataSet != null && shopDataSet.Tables.Count > 0)
            {
                foreach (DataRow row in shopDataSet.Tables[0].Rows)
                {
                    shopNames.Add(row["S_Name"].ToString());
                }
            }

            return shopNames;
        }

        private void materialButtonFactoryDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewFactory.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a factory for delete.");
                    return;
                }
                string factoryId = materialTextBoxFactoryId.Text.Trim();

                // Get associated shop names before deletion
                List<string> associatedShopNames = GetAssociatedShopNames(factoryId);

                if (associatedShopNames.Count > 0)
                {
                    string shopNames = string.Join(", ", associatedShopNames);
                    MessageBox.Show($"Cannot delete the factory without deleting associated shops: {shopNames}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the function to avoid further deletion process
                }

                // Confirmation message before deleting the record
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Perform deletion operation
                    string deleteQuery = $"DELETE FROM Factory WHERE F_Id = '{factoryId}'";

                    DataAccess dataAccess = new DataAccess();
                    int rowsAffected = dataAccess.ExecuteDMLQuery(deleteQuery);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllFactoryInput();
                        LoadFactoryData();
                        // Shop
                        LoadFactoryNamesForShopComboBox();
                        // Dashboard
                        loadCount();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while deleting the record: {exc.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialTextBoxFactorySearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = materialTextBoxFactorySearch.Text.Trim();
            LoadFactoryData(searchValue);
        }

        private void materialButtonFactoryPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlQuery = "SELECT F_Id AS 'Factory ID', F_Name AS 'Factory Name', F_Address AS 'Factory Address' FROM Factory";

                DataAccess dataAccess = new DataAccess();
                DataSet factoryDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (factoryDataSet != null && factoryDataSet.Tables.Count > 0)
                {
                    // Create a PDF document
                    Document pdfDoc = new Document();
                    string fileName = $"FactoryData_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"; // File name with current date and time
                    string filePath = Path.Combine(@"C:\Users\AZMINUR RAHMAN\Desktop\2023-2024, Fall\OBJECT ORIENTED PROGRAMMING 2\Project C#\Factory", fileName);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                    pdfDoc.Open();

                    // Set Times New Roman font for the entire document
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font18 = new iTextSharp.text.Font(baseFont, 18);
                    iTextSharp.text.Font font16 = new iTextSharp.text.Font(baseFont, 16);
                    iTextSharp.text.Font font14 = new iTextSharp.text.Font(baseFont, 14);
                    iTextSharp.text.Font font12 = new iTextSharp.text.Font(baseFont, 12);
                    iTextSharp.text.Font font11 = new iTextSharp.text.Font(baseFont, 11);

                    // Title section
                    Paragraph title = new Paragraph();
                    title.Alignment = Element.ALIGN_CENTER;
                    title.Font = font18;
                    title.Add(new Chunk("ABC Pastry Company", font18));
                    title.Add(new Chunk("\nFactory Data\n", font16));
                    title.Add(new Chunk("Printed by: " + materialTextBoxAdminUsername.Text, font14));
                    title.Add(new Chunk("\n\n", font12));

                    pdfDoc.Add(title);

                    // Create a PdfPTable to hold the data
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(factoryDataSet.Tables[0].Columns.Count);
                    table.DefaultCell.Padding = 5; // Set cell padding

                    // Set table header font and height
                    foreach (DataColumn column in factoryDataSet.Tables[0].Columns)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, font12));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        headerCell.FixedHeight = 25; // Set fixed height for header cells
                        table.AddCell(headerCell);
                    }

                    // Set table cell font and height
                    foreach (DataRow row in factoryDataSet.Tables[0].Rows)
                    {
                        foreach (object cell in row.ItemArray)
                        {
                            PdfPCell dataCell = new PdfPCell(new Phrase(cell.ToString(), font11));
                            dataCell.PaddingTop = 5; // Add padding to the top of each cell
                            dataCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                            dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(dataCell);
                        }
                    }

                    pdfDoc.Add(table);

                    pdfDoc.Close();

                    // Show a message indicating successful creation of the PDF file
                    MessageBox.Show("PDF file generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No data available to generate PDF.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while generating PDF: {exc.Message}\n{exc.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //All Shop related code
        private string AutoShopId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"SELECT TOP 1 S_Id FROM Shop ORDER BY S_Id DESC;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');

                if (id.Length == 2 && id[0] == "S")
                {
                    if (int.TryParse(id[1], out int newIdNum))
                    {
                        newIdNum++; // Increment the ID
                        string newId = "S-" + newIdNum.ToString().PadLeft(3, '0'); // Padding to ensure two digits
                        return newId;
                    }
                }
            }

            // If no rows are returned or an invalid ID format is encountered, start the ID from "S-01"
            return "S-001";
        }

        private bool IsValidToSaveShop()
        {
            // Check if the required text fields are not empty or null
            return !String.IsNullOrEmpty(this.materialTextBoxShopName.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxShopAddress.Text) &&
                   !String.IsNullOrEmpty(this.materialComboBoxShopFactory.Text);
        }

        private void LoadShopData(string searchValue = "")
        {
            try
            {
                string sqlQuery;

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    // SQL query to select Shop data along with the related Factory name if searchValue is empty
                    sqlQuery = "SELECT S.S_Id AS 'Shop ID', S.S_Name AS 'Shop Name', S.S_Address AS 'Shop Address', F.F_Name AS 'Factory Name' " +
                               "FROM Shop S " +
                               "INNER JOIN Factory F ON S.F_Id = F.F_Id";
                }
                else
                {
                    // SQL query to select Shop data based on the searchValue with the related Factory name and filtered by Shop Name
                    sqlQuery = "SELECT S.S_Id AS 'Shop ID', S.S_Name AS 'Shop Name', S.S_Address AS 'Shop Address', F.F_Name AS 'Factory Name' " +
                                $"FROM Shop S " +
                                $"INNER JOIN Factory F ON S.F_Id = F.F_Id WHERE S.S_Name LIKE '%{searchValue}%'";
                }

                DataAccess dataAccess = new DataAccess();
                DataSet shopDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (shopDataSet != null && shopDataSet.Tables.Count > 0)
                {
                    dataGridViewShop.DataSource = shopDataSet.Tables[0];

                    // Auto-size columns based on content
                    dataGridViewShop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // set specific widths for columns
                    dataGridViewShop.Columns["Shop ID"].Width = 100;
                    dataGridViewShop.Columns["Shop Name"].Width = 150;
                    dataGridViewShop.Columns["Shop Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewShop.Columns["Factory Name"].Width = 150;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading shop data: {exc.Message}");
            }
        }

        private void ClearAllShopInput()
        {
            materialTextBoxShopId.Text = "";
            materialTextBoxShopName.Text = "";
            materialTextBoxShopAddress.Text = "";
            materialComboBoxShopFactory.SelectedIndex = -1;
        }

        private void LoadFactoryNamesForShopComboBox()
        {
            try
            {
                // SQL query to select all Factory names
                string sqlQuery = "SELECT F_Name FROM Factory";

                DataAccess dataAccess = new DataAccess();
                DataSet factoryDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (factoryDataSet != null && factoryDataSet.Tables.Count > 0)
                {
                    // Clear existing items in the combo box
                    materialComboBoxShopFactory.Items.Clear();

                    // Populate the combo box with Factory names
                    foreach (DataRow row in factoryDataSet.Tables[0].Rows)
                    {
                        string factoryName = row["F_Name"].ToString();
                        materialComboBoxShopFactory.Items.Add(factoryName);
                    }

                    // Optionally, set a default value or the first item as selected
                    if (materialComboBoxShopFactory.Items.Count > 0)
                    {
                        materialComboBoxShopFactory.SelectedIndex = -1; // Select the first item by default
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading Factory names: {ex.Message}");
            }
        }

        // Function to get Factory ID by Factory Name from the database
        private string GetFactoryIdByName(string factoryName)
        {
            try
            {
                DataAccess dataAccess = new DataAccess();
                string query = $"SELECT F_Id FROM Factory WHERE F_Name = '{factoryName}'";
                var result = dataAccess.ExecuteQuery(query);

                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    return result.Tables[0].Rows[0]["F_Id"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching Factory ID: {ex.Message}");
            }

            return null;
        }

        private void materialButtonShopAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Check validity before attempting to add shop details
                if (!IsValidToSaveShop())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Fetch Shop Name and Address from UI elements
                string shopName = materialTextBoxShopName.Text;
                string shopAddress = materialTextBoxShopAddress.Text;

                // Fetch Factory ID based on the selected Factory Name from the combo box
                string selectedFactoryName = materialComboBoxShopFactory.Text;
                string factoryId = GetFactoryIdByName(selectedFactoryName);

                if (factoryId == null)
                {
                    MessageBox.Show("Factory ID not found for the selected Factory Name.");
                    return;
                }

                // Generate Auto-Generated Shop ID
                string shopId = AutoShopId();

                // SQL query to insert data into the Shop table
                string sqlQuery = $"INSERT INTO Shop (S_Id, S_Name, S_Address, F_Id) VALUES ('{shopId}', '{shopName}', '{shopAddress}', '{factoryId}')";

                DataAccess dataAccess = new DataAccess();
                int rowsAffected = dataAccess.ExecuteDMLQuery(sqlQuery);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Shop added successfully!");
                    ClearAllShopInput();
                    LoadShopData();
                    // Item
                    LoadShopNamesForItemComboBox();
                    // Employee
                    LoadShopNamesForEmployeeComboBox();
                    // Dashboard
                    loadCount();
                }
                else
                {
                    MessageBox.Show("Failed to add shop.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void dataGridViewShop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridViewShop.Rows[e.RowIndex];

                    // Populate text boxes with the clicked row's data
                    materialTextBoxShopId.Text = row.Cells["Shop ID"].Value.ToString();
                    materialTextBoxShopName.Text = row.Cells["Shop Name"].Value.ToString();
                    materialTextBoxShopAddress.Text = row.Cells["Shop Address"].Value.ToString();
                    materialComboBoxShopFactory.SelectedItem = row.Cells["Factory Name"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButtonShopUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewShop.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a Shop to update.");
                    return;
                }
                if (!IsValidToSaveShop())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }
                string shopId = materialTextBoxShopId.Text;
                string shopName = materialTextBoxShopName.Text;
                string shopAddress = materialTextBoxShopAddress.Text;
                string factoryName = materialComboBoxShopFactory.Text;

                // Fetch Factory ID based on the selected Factory Name using GetFactoryIdByName method
                string factoryId = GetFactoryIdByName(factoryName);

                if (factoryId != null)
                {
                    DataAccess dataAccess = new DataAccess();

                    // Update Shop data in the database
                    string updateQuery = $"UPDATE Shop SET S_Name = '{shopName}', S_Address = '{shopAddress}', F_Id = '{factoryId}' WHERE S_Id = '{shopId}'";
                    int rowsAffected = dataAccess.ExecuteDMLQuery(updateQuery);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Shop details updated successfully!");
                        ClearAllShopInput();
                        LoadShopData();
                        // Item
                        LoadShopNamesForItemComboBox();
                        LoadItemData();
                        // Employee
                        LoadShopNamesForEmployeeComboBox();
                        LoadEmployeeData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update shop details.");
                    }
                }
                else
                {
                    MessageBox.Show("Factory ID retrieval failed. Factory not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Function to retrieve associated item names for a shop
        private List<string> GetAssociatedItemNames(string shopId)
        {
            List<string> itemNames = new List<string>();

            DataAccess dataAccess = new DataAccess();
            string itemQuery = $"SELECT I_Name FROM Item WHERE S_Id = '{shopId}'";

            DataSet itemDataSet = dataAccess.ExecuteQuery(itemQuery);

            if (itemDataSet != null && itemDataSet.Tables.Count > 0)
            {
                foreach (DataRow row in itemDataSet.Tables[0].Rows)
                {
                    itemNames.Add(row["I_Name"].ToString());
                }
            }

            return itemNames;
        }

        private void materialButtonShopDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewShop.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a Shop for delete.");
                    return;
                }

                string shopId = materialTextBoxShopId.Text.Trim();

                // Get associated item names before deletion
                List<string> associatedItemNames = GetAssociatedItemNames(shopId);

                if (associatedItemNames.Count > 0)
                {
                    string itemNames = string.Join(", ", associatedItemNames);
                    MessageBox.Show($"Cannot delete the shop without deleting associated items: {itemNames}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the function to avoid further deletion process
                }

                // Confirmation message before deleting the record
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Perform deletion operation
                    string deleteQuery = $"DELETE FROM Shop WHERE S_Id = '{shopId}'";

                    DataAccess dataAccess = new DataAccess();
                    int rowsAffected = dataAccess.ExecuteDMLQuery(deleteQuery);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Shop deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllShopInput();
                        LoadShopData();
                        // Item
                        LoadShopNamesForItemComboBox();
                        // Employee
                        LoadShopNamesForEmployeeComboBox();
                        // Dashboard
                        loadCount();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the shop.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while deleting the shop: {exc.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialTextBoxShopSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = materialTextBoxShopSearch.Text.Trim();
            LoadShopData(searchValue);
        }

        private void materialButtonShopPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlQuery = "SELECT S.S_Id AS 'Shop ID', S.S_Name AS 'Shop Name', S.S_Address AS 'Shop Address', F.F_Name AS 'Factory Name' " +
                                   "FROM Shop S " +
                                   "INNER JOIN Factory F ON S.F_Id = F.F_Id";
                DataAccess dataAccess = new DataAccess();
                DataSet shopDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (shopDataSet != null && shopDataSet.Tables.Count > 0)
                {
                    // Create a PDF document
                    Document pdfDoc = new Document();
                    string fileName = $"ShopData_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"; // File name with current date and time
                    string filePath = Path.Combine(@"C:\Users\AZMINUR RAHMAN\Desktop\2023-2024, Fall\OBJECT ORIENTED PROGRAMMING 2\Project C#\Shop", fileName);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                    pdfDoc.Open();

                    // Set Times New Roman font for the entire document
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font18 = new iTextSharp.text.Font(baseFont, 18);
                    iTextSharp.text.Font font16 = new iTextSharp.text.Font(baseFont, 16);
                    iTextSharp.text.Font font14 = new iTextSharp.text.Font(baseFont, 14);
                    iTextSharp.text.Font font12 = new iTextSharp.text.Font(baseFont, 12);
                    iTextSharp.text.Font font11 = new iTextSharp.text.Font(baseFont, 11);

                    // Title section
                    Paragraph title = new Paragraph();
                    title.Alignment = Element.ALIGN_CENTER;
                    title.Font = font18;
                    title.Add(new Chunk("ABC Pastry Company", font18));
                    title.Add(new Chunk("\nShop Data\n", font16));
                    title.Add(new Chunk("Printed by: " + materialTextBoxAdminUsername.Text, font14));
                    title.Add(new Chunk("\n\n", font12));

                    pdfDoc.Add(title);

                    // Create a PdfPTable to hold the data
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(shopDataSet.Tables[0].Columns.Count);
                    table.DefaultCell.Padding = 5; // Set cell padding

                    // Set table header font and height
                    foreach (DataColumn column in shopDataSet.Tables[0].Columns)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, font12));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        headerCell.FixedHeight = 25; // Set fixed height for header cells
                        table.AddCell(headerCell);
                    }

                    // Set table cell font and height
                    foreach (DataRow row in shopDataSet.Tables[0].Rows)
                    {
                        foreach (object cell in row.ItemArray)
                        {
                            PdfPCell dataCell = new PdfPCell(new Phrase(cell.ToString(), font11));
                            dataCell.PaddingTop = 5; // Add padding to the top of each cell
                            dataCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                            dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(dataCell);
                        }
                    }

                    pdfDoc.Add(table);

                    pdfDoc.Close();

                    // Show a message indicating successful creation of the PDF file
                    MessageBox.Show("PDF file generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No data available to generate PDF.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while generating PDF: {exc.Message}\n{exc.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //All Item related code
        public string AutoItemId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"SELECT TOP 1 I_Id FROM Item ORDER BY I_Id DESC;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');

                if (id.Length == 2 && id[0] == "I")
                {
                    if (int.TryParse(id[1], out int newIdNum))
                    {
                        newIdNum++; // Increment the ID
                        string newId = "I-" + newIdNum.ToString().PadLeft(4, '0'); // Padding to ensure two digits
                        return newId;
                    }
                }
            }

            // If no rows are returned or an invalid ID format is encountered, start the ID from "I-001"
            return "I-0001";
        }

        private bool IsValidToSaveItem()
        {
            // Check if the required text fields are not empty or null and validate specific formats
            return !string.IsNullOrEmpty(materialTextBoxItemName.Text) &&
                   double.TryParse(materialTextBoxItemPrice.Text, out double price) && // Check for valid double
                   price > 0 && // Ensure price is greater than zero
                   DateTime.TryParse(dateTimePickerItemExpireDate.Text, out _) && // Check for valid date
                   int.TryParse(materialTextBoxItemQuantity.Text, out int quantity) && // Check for valid integer
                   quantity > -1 && // Ensure quantity is greater than zero
                   !string.IsNullOrEmpty(materialComboBoxItemShop.Text);

        }

        private void LoadItemData(string searchValue = "")
        {
            try
            {
                string sqlQuery;

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    // SQL query to select Item data along with the related Shop and Factory names if searchValue is empty
                    sqlQuery = "SELECT I.I_Id AS 'Item ID', I.I_Name AS 'Item Name', I.I_Price AS 'Item Price', I.I_ExpireDate AS 'Item Expire Date', I.I_Quantity AS 'Item Quantity', S.S_Name AS 'Shop Name', F.F_Name AS 'Factory Name' " +
                               "FROM Item I " +
                               "INNER JOIN Shop S ON I.S_Id = S.S_Id " +
                               "INNER JOIN Factory F ON S.F_Id = F.F_Id";
                }
                else
                {
                    // SQL query to select Item data based on the searchValue with the related Shop and Factory names, filtered by Item Name
                    sqlQuery = "SELECT I.I_Id AS 'Item ID', I.I_Name AS 'Item Name', I.I_Price AS 'Item Price', I.I_ExpireDate AS 'Item Expire Date', I.I_Quantity AS 'Item Quantity', S.S_Name AS 'Shop Name', F.F_Name AS 'Factory Name' " +
                                $"FROM Item I " +
                                $"INNER JOIN Shop S ON I.S_Id = S.S_Id " +
                                $"INNER JOIN Factory F ON S.F_Id = F.F_Id WHERE I.I_Name LIKE '%{searchValue}%'";
                }

                DataAccess dataAccess = new DataAccess();
                DataSet itemDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (itemDataSet != null && itemDataSet.Tables.Count > 0)
                {
                    dataGridViewItem.DataSource = itemDataSet.Tables[0];

                    // Auto-size columns based on content
                    dataGridViewItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // set specific widths for columns
                    dataGridViewItem.Columns["Item ID"].Width = 100;
                    dataGridViewItem.Columns["Item Name"].Width = 200;
                    dataGridViewItem.Columns["Item Price"].Width = 150;
                    dataGridViewItem.Columns["Item Expire Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewItem.Columns["Item Quantity"].Width = 150;
                    dataGridViewItem.Columns["Shop Name"].Width = 200;
                    dataGridViewItem.Columns["Factory Name"].Width = 200;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading item data: {exc.Message}");
            }
        }

        private void ClearAllItemInput()
        {
            materialTextBoxItemId.Text = "";
            materialTextBoxItemName.Text = "";
            materialTextBoxItemPrice.Text = "";
            dateTimePickerItemExpireDate.Value = DateTime.Now;
            materialTextBoxItemQuantity.Text = "";
            materialComboBoxItemShop.SelectedIndex = -1;
        }

        private void LoadShopNamesForItemComboBox()
        {
            try
            {
                // SQL query to select all Shop names
                string sqlQuery = "SELECT S_Name FROM Shop";

                DataAccess dataAccess = new DataAccess();
                DataSet shopDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (shopDataSet != null && shopDataSet.Tables.Count > 0)
                {
                    // Clear existing items in the combo box
                    materialComboBoxItemShop.Items.Clear();

                    // Populate the combo box with Shop names
                    foreach (DataRow row in shopDataSet.Tables[0].Rows)
                    {
                        string shopName = row["S_Name"].ToString();
                        materialComboBoxItemShop.Items.Add(shopName);
                    }

                    // Optionally, set a default value or the first item as selected
                    if (materialComboBoxItemShop.Items.Count > 0)
                    {
                        materialComboBoxItemShop.SelectedIndex = -1; // Select the first item by default
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading Shop names: {ex.Message}");
            }
        }

        private void materialButtonItemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Check validity before attempting to add item details
                if (!IsValidToSaveItem())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Fetch Item Name, Price, Expire Date, and Quantity from UI elements
                string itemName = materialTextBoxItemName.Text;
                string itemPrice = materialTextBoxItemPrice.Text;
                string itemExpireDate = dateTimePickerItemExpireDate.Value.ToString("yyyy-MM-dd");
                string itemQuantity = materialTextBoxItemQuantity.Text;

                // Fetch Shop ID based on the selected Shop Name from the combo box
                string selectedShopName = materialComboBoxItemShop.Text;
                string shopId = GetShopIdByName(selectedShopName);

                if (shopId == null)
                {
                    MessageBox.Show("Shop ID not found for the selected Shop Name.");
                    return;
                }

                // Generate Auto-Generated Item ID
                string itemId = AutoItemId();

                // SQL query to insert data into the Item table
                string sqlQuery = $"INSERT INTO Item (I_Id, I_Name, I_Price, I_ExpireDate, I_Quantity, S_Id) VALUES ('{itemId}', '{itemName}', '{itemPrice}', '{itemExpireDate}', '{itemQuantity}', '{shopId}')";

                DataAccess dataAccess = new DataAccess();
                int rowsAffected = dataAccess.ExecuteDMLQuery(sqlQuery);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Item added successfully!");
                    ClearAllItemInput();
                    LoadItemData();
                    // Dashboard
                    loadCount();
                }
                else
                {
                    MessageBox.Show("Failed to add item.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void dataGridViewItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridViewItem.Rows[e.RowIndex];

                    // Populate text boxes with the clicked row's data
                    materialTextBoxItemId.Text = row.Cells["Item ID"].Value.ToString();
                    materialTextBoxItemName.Text = row.Cells["Item Name"].Value.ToString();
                    materialTextBoxItemPrice.Text = row.Cells["Item Price"].Value.ToString();
                    dateTimePickerItemExpireDate.Value = DateTime.Parse(row.Cells["Item Expire Date"].Value.ToString());
                    materialTextBoxItemQuantity.Text = row.Cells["Item Quantity"].Value.ToString();
                    materialComboBoxItemShop.SelectedItem = row.Cells["Shop Name"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButtonItemUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewItem.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an item to update.");
                    return;
                }
                if (!IsValidToSaveItem())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }
                string itemId = materialTextBoxItemId.Text;
                string itemName = materialTextBoxItemName.Text;
                string itemPrice = materialTextBoxItemPrice.Text;
                string itemExpireDate = dateTimePickerItemExpireDate.Value.ToString("yyyy-MM-dd");
                string itemQuantity = materialTextBoxItemQuantity.Text;
                string selectedShopName = materialComboBoxItemShop.Text;

                // Fetch Shop ID based on the selected Shop Name using GetShopIdByName method
                string shopId = GetShopIdByName(selectedShopName);

                if (shopId != null)
                {
                    DataAccess dataAccess = new DataAccess();

                    // Update Item data in the database
                    string updateQuery = $"UPDATE Item SET I_Name = '{itemName}', I_Price = '{itemPrice}', I_ExpireDate = '{itemExpireDate}', I_Quantity = '{itemQuantity}', S_Id = '{shopId}' WHERE I_Id = '{itemId}'";
                    int rowsAffected = dataAccess.ExecuteDMLQuery(updateQuery);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item details updated successfully!");
                        ClearAllItemInput();
                        LoadItemData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update item details.");
                    }
                }
                else
                {
                    MessageBox.Show("Shop ID retrieval failed. Shop not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButtonItemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewItem.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an item for delete.");
                    return;
                }

                string itemId = materialTextBoxItemId.Text; // Fetch Item ID from the TextBox

                // Confirmation message before deleting the record
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Perform deletion operation
                    string deleteQuery = $"DELETE FROM Item WHERE I_Id = '{itemId}'";

                    DataAccess dataAccess = new DataAccess();
                    int rowsAffected = dataAccess.ExecuteDMLQuery(deleteQuery);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllItemInput();
                        LoadItemData();
                        // Dashboard
                        loadCount();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while deleting the item: {exc.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialTextBoxItemSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = materialTextBoxItemSearch.Text.Trim();
            LoadItemData(searchValue);
        }

        private void materialButtonItemPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlQuery = "SELECT I.I_Id AS 'Item ID', I.I_Name AS 'Item Name', I.I_Price AS 'Item Price', I.I_ExpireDate AS 'Item Expire Date', I.I_Quantity AS 'Item Quantity', S.S_Name AS 'Shop Name', F.F_Name AS 'Factory Name' " +
                                "FROM Item I " +
                                "INNER JOIN Shop S ON I.S_Id = S.S_Id " +
                                "INNER JOIN Factory F ON S.F_Id = F.F_Id";

                DataAccess dataAccess = new DataAccess();
                DataSet itemDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (itemDataSet != null && itemDataSet.Tables.Count > 0)
                {
                    // Create a PDF document
                    Document pdfDoc = new Document();
                    string fileName = $"ItemData_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"; // File name with current date and time
                    string filePath = Path.Combine(@"C:\Users\AZMINUR RAHMAN\Desktop\2023-2024, Fall\OBJECT ORIENTED PROGRAMMING 2\Project C#\Item", fileName);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                    pdfDoc.Open();

                    // Set Times New Roman font for the entire document
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font18 = new iTextSharp.text.Font(baseFont, 18);
                    iTextSharp.text.Font font16 = new iTextSharp.text.Font(baseFont, 16);
                    iTextSharp.text.Font font14 = new iTextSharp.text.Font(baseFont, 14);
                    iTextSharp.text.Font font12 = new iTextSharp.text.Font(baseFont, 12);
                    iTextSharp.text.Font font11 = new iTextSharp.text.Font(baseFont, 11);

                    // Title section
                    Paragraph title = new Paragraph();
                    title.Alignment = Element.ALIGN_CENTER;
                    title.Font = font18;
                    title.Add(new Chunk("ABC Pastry Company", font18));
                    title.Add(new Chunk("\nItem Data\n", font16));
                    title.Add(new Chunk("Printed by: " + materialTextBoxAdminUsername.Text, font14));
                    title.Add(new Chunk("\n\n", font12));

                    pdfDoc.Add(title);

                    // Create a PdfPTable to hold the data
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(itemDataSet.Tables[0].Columns.Count);
                    table.DefaultCell.Padding = 5; // Set cell padding

                    // Set table header font and height
                    foreach (DataColumn column in itemDataSet.Tables[0].Columns)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, font12));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        headerCell.PaddingTop = 5; // Add padding to the top of each cell
                        headerCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                        //headerCell.FixedHeight = 25; // Set fixed height for header cells
                        table.AddCell(headerCell);
                    }

                    // Set table cell font and height
                    foreach (DataRow row in itemDataSet.Tables[0].Rows)
                    {
                        foreach (object cell in row.ItemArray)
                        {
                            PdfPCell dataCell = new PdfPCell(new Phrase(cell.ToString(), font11));
                            dataCell.PaddingTop = 5; // Add padding to the top of each cell
                            dataCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                            dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(dataCell);
                        }
                    }

                    pdfDoc.Add(table);

                    pdfDoc.Close();

                    // Show a message indicating successful creation of the PDF file
                    MessageBox.Show("PDF file generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No data available to generate PDF.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while generating PDF: {exc.Message}\n{exc.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //All Customer related code
        public string AutoCustomerId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"SELECT TOP 1 C_Id FROM Customer ORDER BY C_Id DESC;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');

                if (id.Length == 2 && id[0] == "C")
                {
                    if (int.TryParse(id[1], out int newIdNum))
                    {
                        newIdNum++;
                        string newId = "C-" + newIdNum.ToString().PadLeft(10, '0');
                        return newId;
                    }
                }
            }
            return "C-0000000001";
        }

        public bool IsPhoneNumberUniqueForCustomer(string phoneNumber)
        {
            bool isUnique = false;

            DataAccess dataAccess = new DataAccess();

            // Check if the phone number is not in the Person table
            string query = $"SELECT COUNT(*) FROM Person WHERE P_PhoneNumber = '{phoneNumber}'";
            int count = Convert.ToInt32(dataAccess.ExecuteQuery(query).Tables[0].Rows[0][0]);

            if (count == 0)
            {
                isUnique = true; // Phone number is not in the Person table
            }
            else
            {
                // Check if the phone number is associated with an Employee
                query = $"SELECT COUNT(*) FROM Customer WHERE P_Id IN (SELECT P_Id FROM Person WHERE P_PhoneNumber = '{phoneNumber}')";
                count = Convert.ToInt32(dataAccess.ExecuteQuery(query).Tables[0].Rows[0][0]);

                if (count == 0)
                {
                    isUnique = true; // Phone number is in the Person table, but not associated with an Employee
                }
            }

            return isUnique;
        }


        public bool IsEmailUniqueForCustomer(string email, string customerId = null)
        {
            // Query to check if the email already exists in the Employee table
            string checkEmailQuery = string.IsNullOrEmpty(customerId) ?
                $"SELECT COUNT(*) FROM Customer WHERE C_Email = '{email}'" :
                $"SELECT COUNT(*) FROM Customer WHERE C_Email = '{email}' AND C_Id != '{customerId}'";

            // Use DataAccess to execute the query and check if count > 0 (email exists)
            DataAccess dataAccess = new DataAccess();
            DataSet countDataSet = dataAccess.ExecuteQuery(checkEmailQuery);

            if (countDataSet != null && countDataSet.Tables.Count > 0)
            {
                int emailCount = Convert.ToInt32(countDataSet.Tables[0].Rows[0][0]);
                return emailCount == 0; // Returns true if the email doesn't exist (or doesn't exist for other employees), else false
            }

            return false;
        }

        public string GetPersonIdByCustomerId(string customerId)
        {
            // Retrieve the associated person ID (P_Id) based on the employee ID
            string query = $"SELECT P_Id FROM Customer WHERE C_Id = '{customerId}'";
            DataAccess dataAccess = new DataAccess();
            DataSet dataSet = dataAccess.ExecuteQuery(query);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return dataSet.Tables[0].Rows[0]["P_Id"].ToString();
            }

            return null; // Return null if the associated person ID is not found
        }

        private bool IsValidToSaveCustomer()
        {
            return !String.IsNullOrEmpty(this.materialTextBoxCustomerName.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxCustomerPhoneNumber.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxCustomerEmail.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxCustomerAddress.Text);
        }

        private void LoadCustomerData(string searchValue = "")
        {
            try
            {
                string sqlQuery;

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    // SQL query to select Customer data along with related Person details if searchValue is empty
                    sqlQuery = "SELECT C.C_Id AS 'Customer ID', P.P_Name As 'Name', P.P_PhoneNumber AS 'Phone Number', P.P_Gender AS 'Gender', C.C_Email AS 'Email', C.C_Address AS 'Address' " +
                               "FROM Customer C " +
                               "INNER JOIN Person P ON C.P_Id = P.P_Id ";
                }
                else
                {
                    // SQL query to select Customer data based on the searchValue with related Person details, filtered by name
                    sqlQuery = "SELECT C.C_Id AS 'Customer ID', P.P_Name As 'Name', P.P_PhoneNumber AS 'Phone Number', P.P_Gender AS 'Gender', C.C_Email AS 'Email', C.C_Address AS 'Address' " +
                                $"FROM Customer C " +
                                $"INNER JOIN Person P ON C.P_Id = P.P_Id WHERE P.P_Name LIKE '%{searchValue}%' OR P.P_PhoneNumber LIKE '%{searchValue}%'";

                }

                DataAccess dataAccess = new DataAccess();
                DataSet customerDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (customerDataSet != null && customerDataSet.Tables.Count > 0)
                {
                    dataGridViewCustomer.DataSource = customerDataSet.Tables[0];

                    // Auto-size columns based on content
                    dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Set specific widths for columns
                    dataGridViewCustomer.Columns["Customer ID"].Width = 100;
                    dataGridViewCustomer.Columns["Name"].Width = 200;
                    dataGridViewCustomer.Columns["Phone Number"].Width = 150;
                    dataGridViewCustomer.Columns["Gender"].Width = 100;
                    dataGridViewCustomer.Columns["Email"].Width = 200;
                    dataGridViewCustomer.Columns["Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading customer data: {exc.Message}");
            }
        }

        private void ClearAllCustomerInput()
        {
            materialTextBoxCustomerId.Text = "";
            materialTextBoxCustomerName.Text = "";
            materialRadioButtonCustomerMale.Checked = false;
            materialRadioButtonCustomerFemale.Checked = false;
            materialTextBoxCustomerPhoneNumber.Text = "";
            materialTextBoxCustomerEmail.Text = "";
            materialTextBoxCustomerAddress.Text = "";
        }

        private void materialButtonCustomerAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidToSaveCustomer())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                string customerName = materialTextBoxCustomerName.Text;
                string customerPhoneNumber = materialTextBoxCustomerPhoneNumber.Text;
                string customerEmail = materialTextBoxCustomerEmail.Text;
                string customerAddress = materialTextBoxCustomerAddress.Text;

                string customerGender = "";
                if (materialRadioButtonCustomerMale.Checked)
                {
                    customerGender = "Male";
                }
                else if (materialRadioButtonCustomerFemale.Checked)
                {
                    customerGender = "Female";
                }

                DataAccess dataAccess = new DataAccess();

                if (!IsPhoneNumberValid(customerPhoneNumber))
                {
                    MessageBox.Show("Invalid phone number.");
                    return;
                }

                if (!IsPhoneNumberUniqueForCustomer(customerPhoneNumber))
                {
                    MessageBox.Show("Phone number already exists.");
                    return;
                }

                if (!IsEmailValid(customerEmail))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                if (!IsEmailUniqueForCustomer(customerEmail))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }

                string personId = GetPersonIdByPhoneNumber(customerPhoneNumber);

                if (personId == null)
                {
                    personId = AutoPersonId();

                    string personQuery = $"INSERT INTO Person (P_Id, P_Name, P_Gender, P_PhoneNumber) " +
                                         $"VALUES ('{personId}', '{customerName}', '{customerGender}', '{customerPhoneNumber}')";

                    int personRowsAffected = dataAccess.ExecuteDMLQuery(personQuery);

                    if (personRowsAffected <= 0)
                    {
                        MessageBox.Show("Failed to add person details.");
                        return;
                    }
                }

                string customerId = AutoCustomerId();

                string customerQuery = $"INSERT INTO Customer (C_Id, C_Email, C_Address, P_Id) " +
                                       $"VALUES ('{customerId}', '{customerEmail}', '{customerAddress}', '{personId}')";

                int customerRowsAffected = dataAccess.ExecuteDMLQuery(customerQuery);

                if (customerRowsAffected > 0)
                {
                    MessageBox.Show("Customer added successfully!");
                    ClearAllCustomerInput();
                    LoadCustomerData();
                    // Employee
                    LoadEmployeeData();
                    // Dashboard
                    loadCount();
                }
                else
                {
                    MessageBox.Show("Failed to add customer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void dataGridViewCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridViewCustomer.Rows[e.RowIndex];

                    // Retrieve data from the selected row's cells
                    string customerId = selectedRow.Cells["Customer ID"].Value.ToString();
                    string customerName = selectedRow.Cells["Name"].Value.ToString();
                    string customerGender = selectedRow.Cells["Gender"].Value.ToString();
                    string customerPhoneNumber = selectedRow.Cells["Phone Number"].Value.ToString();
                    string customerEmail = selectedRow.Cells["Email"].Value.ToString();
                    string customerAddress = selectedRow.Cells["Address"].Value.ToString();

                    // Populate UI elements with the retrieved data
                    materialTextBoxCustomerId.Text = customerId;
                    materialTextBoxCustomerName.Text = customerName;
                    if (customerGender == "Male")
                    {
                        materialRadioButtonCustomerMale.Checked = true;
                        materialRadioButtonCustomerFemale.Checked = false;
                    }
                    else if (customerGender == "Female")
                    {
                        materialRadioButtonCustomerMale.Checked = false;
                        materialRadioButtonCustomerFemale.Checked = true;
                    }
                    else
                    {
                        materialRadioButtonCustomerMale.Checked = false;
                        materialRadioButtonCustomerFemale.Checked = false;
                    }
                    materialTextBoxCustomerPhoneNumber.Text = customerPhoneNumber;
                    materialTextBoxCustomerEmail.Text = customerEmail;
                    materialTextBoxCustomerAddress.Text = customerAddress;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButtonCustomerUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a customer is selected in the DataGridView
                if (dataGridViewCustomer.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an customer to update.");
                    return;
                }

                if (!IsValidToSaveCustomer())
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // Fetch Customer details from UI elements
                string customerId = materialTextBoxCustomerId.Text;
                string customerName = materialTextBoxCustomerName.Text;
                string customerPhoneNumber = materialTextBoxCustomerPhoneNumber.Text;
                string customerEmail = materialTextBoxCustomerEmail.Text;
                string customerAddress = materialTextBoxCustomerAddress.Text;

                string customerGender = "";
                if (materialRadioButtonCustomerMale.Checked)
                {
                    customerGender = "Male";
                }
                else if (materialRadioButtonCustomerFemale.Checked)
                {
                    customerGender = "Female";
                }

                DataAccess dataAccess = new DataAccess();

                if (!IsPhoneNumberValid(customerPhoneNumber))
                {
                    MessageBox.Show("Invalid phone number.");
                    return;
                }

                // Check if the phone number is unique and valid
                if (!IsPhoneNumberUniqueForUpdate(customerPhoneNumber, GetPersonIdByCustomerId(customerId)))
                {
                    MessageBox.Show("Phone number already exists.");
                    return;
                }

                if (!IsEmailValid(customerEmail))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                // Check if the email is unique and valid
                if (!IsEmailUniqueForCustomer(customerEmail, customerId))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }

                // SQL query to update Person details
                string updatePersonQuery = $@"UPDATE Person SET 
                P_Name = '{customerName}', 
                P_Gender = '{customerGender}', 
                P_PhoneNumber = '{customerPhoneNumber}' 
                WHERE P_Id = (SELECT P_Id FROM Customer WHERE C_Id = '{customerId}')";

                int personRowsAffected = dataAccess.ExecuteDMLQuery(updatePersonQuery);

                // SQL query to update Employee details
                string updateCustomerQuery = $@"UPDATE Customer SET 
                C_Email = '{customerEmail}', 
                C_Address = '{customerAddress}'
                WHERE C_Id = '{customerId}'";

                int customerRowsAffected = dataAccess.ExecuteDMLQuery(updateCustomerQuery);

                if (personRowsAffected > 0 && customerRowsAffected > 0)
                {
                    MessageBox.Show("Customer updated successfully!");
                    ClearAllCustomerInput();
                    LoadCustomerData();
                    // Employee
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show("Failed to update customer details.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void materialButtonCustomerDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a customer is selected in the DataGridView
                if (dataGridViewCustomer.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an customer to delete.");
                    return;
                }

                string customerIdToDelete = materialTextBoxCustomerId.Text;

                // Retrieve the associated person ID (P_Id) before deleting the customer
                string personIdToDelete = GetPersonIdByCustomerId(customerIdToDelete);

                // Confirmation message before deleting the record
                DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Perform deletion operation
                    string deleteCustomerQuery = $"DELETE FROM Customer WHERE C_Id = '{customerIdToDelete}'";
                    string deletePersonQuery = $"DELETE FROM Person WHERE P_Id = '{personIdToDelete}'";

                    DataAccess dataAccess = new DataAccess();
                    int customerRowsAffected = dataAccess.ExecuteDMLQuery(deleteCustomerQuery);
                    int personRowsAffected = dataAccess.ExecuteDMLQuery(deletePersonQuery);

                    if (customerRowsAffected > 0 && personRowsAffected > 0)
                    {
                        MessageBox.Show("Customer deleted successfully!");
                        ClearAllCustomerInput();
                        LoadCustomerData();
                        // Employee
                        LoadEmployeeData();
                        // Dashboard
                        loadCount();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete customer.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private void materialTextBoxCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = materialTextBoxCustomerSearch.Text.Trim();
            LoadCustomerData(searchValue);
        }

        private void materialButtonCustomerPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlQuery = "SELECT C.C_Id AS 'Customer ID', P.P_Name As 'Name', P.P_PhoneNumber AS 'Phone Number', P.P_Gender AS 'Gender', C.C_Email AS 'Email', C.C_Address AS 'Address' " +
                                   $"FROM Customer C " +
                                   $"INNER JOIN Person P ON C.P_Id = P.P_Id ";

                DataAccess dataAccess = new DataAccess();
                DataSet customerDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (customerDataSet != null && customerDataSet.Tables.Count > 0)
                {
                    // Create a PDF document
                    Document pdfDoc = new Document();
                    string fileName = $"CustomerData_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"; // File name with current date and time
                    string filePath = Path.Combine(@"C:\Users\AZMINUR RAHMAN\Desktop\2023-2024, Fall\OBJECT ORIENTED PROGRAMMING 2\Project C#\Customer", fileName);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                    pdfDoc.Open();

                    // Set Times New Roman font for the entire document
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font18 = new iTextSharp.text.Font(baseFont, 18);
                    iTextSharp.text.Font font16 = new iTextSharp.text.Font(baseFont, 16);
                    iTextSharp.text.Font font14 = new iTextSharp.text.Font(baseFont, 14);
                    iTextSharp.text.Font font12 = new iTextSharp.text.Font(baseFont, 12);
                    iTextSharp.text.Font font11 = new iTextSharp.text.Font(baseFont, 11);

                    // Title section
                    Paragraph title = new Paragraph();
                    title.Alignment = Element.ALIGN_CENTER;
                    title.Font = font18;
                    title.Add(new Chunk("ABC Pastry Company", font18));
                    title.Add(new Chunk("\nCustomer Data\n", font16));
                    title.Add(new Chunk("Printed by: " + materialTextBoxAdminUsername.Text));
                    title.Add(new Chunk("\n\n", font12));

                    pdfDoc.Add(title);

                    // Create a PdfPTable to hold the data
                    iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(customerDataSet.Tables[0].Columns.Count);
                    table.DefaultCell.Padding = 5; // Set cell padding

                    // Set table header font and height
                    foreach (DataColumn column in customerDataSet.Tables[0].Columns)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, font12));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        headerCell.PaddingTop = 5; // Add padding to the top of each cell
                        headerCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                        //headerCell.FixedHeight = 25; // Set fixed height for header cells
                        table.AddCell(headerCell);
                    }

                    // Set table cell font and height
                    foreach (DataRow row in customerDataSet.Tables[0].Rows)
                    {
                        foreach (object cell in row.ItemArray)
                        {
                            PdfPCell dataCell = new PdfPCell(new Phrase(cell.ToString(), font11));
                            dataCell.PaddingTop = 5; // Add padding to the top of each cell
                            dataCell.PaddingBottom = 5; // Add padding to the bottom of each cell
                            dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(dataCell);
                        }
                    }

                    pdfDoc.Add(table);

                    pdfDoc.Close();

                    // Show a message indicating successful creation of the PDF file
                    MessageBox.Show("PDF file generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No data available to generate PDF.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while generating PDF: {exc.Message}\n{exc.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}