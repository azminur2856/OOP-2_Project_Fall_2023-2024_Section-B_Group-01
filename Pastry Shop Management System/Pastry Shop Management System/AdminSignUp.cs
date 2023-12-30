using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Data;
using System.Windows.Forms;

namespace Pastry_Shop_Management_System
{
    public partial class AdminSignUp : MaterialForm
    {
        readonly MaterialSkinManager materialSkinManager;
        private bool passwordVisible = false;
        public AdminSignUp()
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

        private void AdminSignUp_Load(object sender, EventArgs e)
        {
            // Initially set the password to be masked
            materialTextBoxEnterPassword.UseSystemPasswordChar = true;
            materialTextBoxEnterPassword.TrailingIcon = imageListAdminSingUp.Images[0];
            passwordVisible = false;

            materialTextBoxReEnterPassword.UseSystemPasswordChar = true;
            materialTextBoxReEnterPassword.TrailingIcon = imageListAdminSingUp.Images[0];
            passwordVisible = false;
        }

        private void materialTextBoxEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                materialTextBoxEnterPassword.UseSystemPasswordChar = false;
                materialTextBoxEnterPassword.TrailingIcon = imageListAdminSingUp.Images[1];
                passwordVisible = true;
            }
            else
            {
                materialTextBoxEnterPassword.UseSystemPasswordChar = true;
                materialTextBoxEnterPassword.TrailingIcon = imageListAdminSingUp.Images[0];
                passwordVisible = false;
            }
        }

        private void materialTextBoxReEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                materialTextBoxReEnterPassword.UseSystemPasswordChar = false;
                materialTextBoxReEnterPassword.TrailingIcon = imageListAdminSingUp.Images[1];
                passwordVisible = true;
            }
            else
            {
                materialTextBoxReEnterPassword.UseSystemPasswordChar = true;
                materialTextBoxReEnterPassword.TrailingIcon = imageListAdminSingUp.Images[0];
                passwordVisible = false;
            }
        }

        private string AutoAdminId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"Select * from Admin order by A_Id Desc;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');
                int newIdNum = Convert.ToInt32(id[1]);
                string newId = "A-00" + (++newIdNum).ToString();
                return newId;
            }
            else
            {
                // If no rows are returned, start the ID from 1 or another suitable starting value
                return "A-001";
            }
        }

        private bool IsValidToSave()
        {
            return !String.IsNullOrEmpty(this.materialTextBoxActivationCode.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxAdminUsername.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxEnterPassword.Text);
        }
        private int GetAdminCount()
        {
            // Query to count the number of admins
            string countQuery = "SELECT COUNT(*) FROM Admin";

            // Use DataAccess to execute the query and get the count
            DataAccess dataAccess = new DataAccess();
            DataSet countDataSet = dataAccess.ExecuteQuery(countQuery);

            if (countDataSet != null && countDataSet.Tables.Count > 0)
            {
                int adminCount = Convert.ToInt32(countDataSet.Tables[0].Rows[0][0]);
                return adminCount;
            }
            else
            {
                // Handle if count retrieval fails
                return -1; // Or another suitable default value
            }
        }

        private void materialButtonSaveActive_Click(object sender, EventArgs e)
        {
            try
            {
                int adminCount = GetAdminCount();
                if (adminCount < 5)
                {
                    if (IsValidToSave())
                    {
                        string activationCode = "22-46588-1";
                        string adminName = materialTextBoxAdminName.Text;
                        string adminUsername = materialTextBoxAdminUsername.Text;
                        string adminPassword = materialTextBoxEnterPassword.Text;
                        string reEnteredPassword = materialTextBoxReEnterPassword.Text;
                        string adminEmail = materialTextBoxAdminEmail.Text;

                        if (materialTextBoxActivationCode.Text == activationCode)
                        {
                            // Creating an Admin object
                            Admin admin = new Admin();

                            if (admin.IsUsernameValid(adminUsername))
                            {
                                // Check if username already exists
                                if (admin.IsUsernameUnique(adminUsername))
                                {
                                    // Check email validity
                                    if (admin.IsEmailValid(adminEmail))
                                    {
                                        // Check if email already exists
                                        if (admin.IsEmailUniqueForAdminAndEmployee(adminEmail))
                                        {
                                            // Check if password is valid
                                            if (admin.IsPasswordValid(adminPassword))
                                            {
                                                if (adminPassword == reEnteredPassword)
                                                {
                                                    // Generating new Admin ID
                                                    string newAdminId = AutoAdminId();

                                                    // Hashing the password
                                                    PasswordManager passwordManageer = new PasswordManager();
                                                    adminPassword = passwordManageer.HashPassword(adminPassword);

                                                    // Creating a DataAccess object
                                                    DataAccess dataAccess = new DataAccess();

                                                    // Inserting data into the Admin table
                                                    string insertQuery = $"INSERT INTO Admin (A_Id, A_Name, A_Username, A_Password, A_Email) VALUES ('{newAdminId}', '{adminName}', '{adminUsername}', '{adminPassword}', '{adminEmail}')";
                                                    int rowsAffected = dataAccess.ExecuteDMLQuery(insertQuery);
                                                    if (rowsAffected > 0)
                                                    {
                                                        MessageBox.Show("Admin data saved successfully!");

                                                        // Close the current form
                                                        this.Hide();

                                                        // Open the login form
                                                        Login login = new Login();
                                                        login.Show();
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Failed to save admin data.");
                                                    }

                                                }
                                                else
                                                {
                                                    MessageBox.Show("Passwords do not match.");
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Password is invalid. Please choose a different password.");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Email already exists. Please enter a different email.");
                                        }
                                        // Check if password is valid
                                    }
                                    else
                                    {
                                        MessageBox.Show("Email is invalid. Please enter a valid email.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Username already exists. Please choose a different username.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Username is invalid. Please choose a different username.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid activation code.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fill in all required fields.");
                    }
                }
                else
                {
                    MessageBox.Show("Maximum admin limit reached (5 admins). Cannot add more.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occurred due to invalid input.\nError Info:" + exc.Message);
            }
        }

        private void materialButtonAdminBack_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
