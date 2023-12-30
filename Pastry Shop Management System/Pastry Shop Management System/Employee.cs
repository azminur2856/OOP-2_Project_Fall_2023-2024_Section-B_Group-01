using iTextSharp.text;
using iTextSharp.text.pdf;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Need to do work for print person name at bill 
namespace Pastry_Shop_Management_System
{
    public partial class Employee : MaterialForm
    {
        readonly MaterialSkinManager materialSkinManager;
        private bool passwordVisible = false; // Track password visibility state
        Admin admin = new Admin();
        public Employee()
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
        private readonly string eid;
        private readonly string name;
        private readonly string userName;
        public Employee(string eid, string name, string userName) : this()
        {
            this.eid = eid;
            this.name = name;
            this.userName = userName;
        }

        // Form Load Operations
        private void Employee_Load(object sender, EventArgs e)
        {
            // Set the text of the form to be the name of the logged in employee
            this.Text = "Welcome " + name + " (Employee)";
            materialTextBoxEmployeeId.Text = eid;
            materialTextBoxEmployeeName.Text = name;
            materialTextBoxEmployeeUsername.Text = userName;

            // Initially set the password to be masked
            materialTextBoxEnterPassword.UseSystemPasswordChar = true;
            materialTextBoxEnterPassword.TrailingIcon = imageListEmployee.Images[4];
            passwordVisible = false;

            materialTextBoxReEnterPassword.UseSystemPasswordChar = true;
            materialTextBoxReEnterPassword.TrailingIcon = imageListEmployee.Images[4];
            passwordVisible = false;


            // Admin
            //string A = "Admin";
            //string E = "Employee";
            //string F = "Factory";
            //string S = "Shop";
            string I = "Item";
            string C = "Customer";
            //materialTextBoxAdminTotal.Text = "Total Admin: " + admin.GetTotalCount(A);
            //materialTextBoxEmployeeTotal.Text = "Total Employee: " + admin.GetTotalCount(E);
            //materialTextBoxFactoryTotal.Text = "Total Factory: " + admin.GetTotalCount(F);
            //materialTextBoxShopTotal.Text = "Total Shop: " + admin.GetTotalCount(S);
            materialTextBoxItemTotal.Text = "Total Item: " + admin.GetTotalCount(I);
            materialTextBoxCustomerTotal.Text = "Total Customer: " + admin.GetTotalCount(C);


            //Item
            admin.CustomizeDataGridView(dataGridViewItem);
            LoadShopNamesForItemComboBox();
            LoadItemData();
            //Customer
            admin.CustomizeDataGridView(dataGridViewCustomer);
            LoadCustomerData();
            // Sales
            admin.CustomizeDataGridView(dataGridViewSalesCustomer);
            LoadCustomerDataForSales();
            admin.CustomizeDataGridView(dataGridViewSalesItem);
            LoadItemDataForSales();
            admin.CustomizeDataGridView(dataGridViewSalesCart);
            UpdateCartDataGridView();
            materialTextBoxSalesAmountPayable.Text = "0.0";
            materialButtonSalesConfirmOrder.Enabled = false;
        }



        //All Dashboard related code
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
            admin.DrawHand(e.Graphics, centerX, centerY, clockRadius * 0.5f, hourAngle, Pens.Gold, 6);
            admin.DrawHand(e.Graphics, centerX, centerY, clockRadius * 0.7f, minuteAngle, Pens.Black, 4);
            admin.DrawHand(e.Graphics, centerX, centerY, clockRadius * 0.9f, secondAngle, Pens.Red, 2);
        }

        private void materialSwitchThemeChange_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitchThemeChange.Checked)
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                this.materialSwitchThemeChange.Text = "Light Mode";

                this.dataGridViewItem.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewItem.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewItem.BackgroundColor = Color.Black;

                this.dataGridViewCustomer.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewCustomer.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewCustomer.BackgroundColor = Color.Black;

                this.dataGridViewSalesCustomer.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewSalesCustomer.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewSalesCustomer.BackgroundColor = Color.Black;

                this.dataGridViewSalesItem.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewSalesItem.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewSalesItem.BackgroundColor = Color.Black;

                this.dataGridViewSalesCart.DefaultCellStyle.BackColor = Color.Black;
                this.dataGridViewSalesCart.DefaultCellStyle.ForeColor = Color.White;
                this.dataGridViewSalesCart.BackgroundColor = Color.Black;
            }
            else
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                this.materialSwitchThemeChange.Text = "Dark Mode";

                this.dataGridViewItem.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewItem.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewItem.BackgroundColor = Color.White;

                this.dataGridViewCustomer.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewCustomer.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewCustomer.BackgroundColor = Color.White;

                this.dataGridViewSalesCustomer.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewSalesCustomer.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewSalesCustomer.BackgroundColor = Color.White;

                this.dataGridViewSalesItem.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewSalesItem.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewSalesItem.BackgroundColor = Color.White;

                this.dataGridViewSalesCart.DefaultCellStyle.BackColor = Color.White;
                this.dataGridViewSalesCart.DefaultCellStyle.ForeColor = Color.Black;
                this.dataGridViewSalesCart.BackgroundColor = Color.White;
            }
        }

        private void materialButtonUpdateEmployeeNewPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = materialTextBoxEmployeeId.Text;
                string newPassword = materialTextBoxEnterPassword.Text;
                string confirmNewPassword = materialTextBoxReEnterPassword.Text;

                // Query to fetch the current password of the admin by admin ID
                string getPasswordQuery = $"SELECT E_Password FROM Employee WHERE E_Id = '{employeeId}'";

                DataAccess dataAccess = new DataAccess();
                DataSet passwordDataSet = dataAccess.ExecuteQuery(getPasswordQuery);

                if (passwordDataSet != null && passwordDataSet.Tables.Count > 0 && passwordDataSet.Tables[0].Rows.Count > 0)
                {
                    //string currentPassword = passwordDataSet.Tables[0].Rows[0]["A_Password"].ToString();
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
                materialTextBoxEnterPassword.TrailingIcon = imageListEmployee.Images[5];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxEnterPassword.UseSystemPasswordChar = true;
                materialTextBoxEnterPassword.TrailingIcon = imageListEmployee.Images[4];
                passwordVisible = false;
            }
        }

        private void materialTextBoxReEnterPassword_TrailingIconClick(object sender, EventArgs e)
        {
            if (!passwordVisible)
            {
                // Show the password
                materialTextBoxReEnterPassword.UseSystemPasswordChar = false;
                materialTextBoxReEnterPassword.TrailingIcon = imageListEmployee.Images[5];
                passwordVisible = true;
            }
            else
            {
                // Hide the password
                materialTextBoxReEnterPassword.UseSystemPasswordChar = true;
                materialTextBoxReEnterPassword.TrailingIcon = imageListEmployee.Images[4];
                passwordVisible = false;
            }
        }



        //All Item related code
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
                string shopId = admin.GetShopIdByName(selectedShopName);

                if (shopId == null)
                {
                    MessageBox.Show("Shop ID not found for the selected Shop Name.");
                    return;
                }

                // Generate Auto-Generated Item ID
                string itemId = admin.AutoItemId();

                // SQL query to insert data into the Item table
                string sqlQuery = $"INSERT INTO Item (I_Id, I_Name, I_Price, I_ExpireDate, I_Quantity, S_Id) VALUES ('{itemId}', '{itemName}', '{itemPrice}', '{itemExpireDate}', '{itemQuantity}', '{shopId}')";

                DataAccess dataAccess = new DataAccess();
                int rowsAffected = dataAccess.ExecuteDMLQuery(sqlQuery);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Item added successfully!");
                    ClearAllItemInput();
                    LoadItemData();
                    // Sales
                    LoadItemDataForSales();
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
                string shopId = admin.GetShopIdByName(selectedShopName);

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
                        // Sales
                        LoadItemDataForSales();
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
                        // Sales
                        LoadItemDataForSales();
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
                    title.Add(new Chunk("Printed by: " + materialTextBoxEmployeeUsername.Text, font14));
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

                if (!admin.IsPhoneNumberValid(customerPhoneNumber))
                {
                    MessageBox.Show("Invalid phone number.");
                    return;
                }

                if (!admin.IsPhoneNumberUniqueForCustomer(customerPhoneNumber))
                {
                    MessageBox.Show("Phone number already exists.");
                    return;
                }

                if (!admin.IsEmailValid(customerEmail))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                if (!admin.IsEmailUniqueForCustomer(customerEmail))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }

                string personId = admin.GetPersonIdByPhoneNumber(customerPhoneNumber);

                if (personId == null)
                {
                    personId = admin.AutoPersonId();

                    string personQuery = $"INSERT INTO Person (P_Id, P_Name, P_Gender, P_PhoneNumber) " +
                                         $"VALUES ('{personId}', '{customerName}', '{customerGender}', '{customerPhoneNumber}')";

                    int personRowsAffected = dataAccess.ExecuteDMLQuery(personQuery);

                    if (personRowsAffected <= 0)
                    {
                        MessageBox.Show("Failed to add person details.");
                        return;
                    }
                }

                string customerId = admin.AutoCustomerId();

                string customerQuery = $"INSERT INTO Customer (C_Id, C_Email, C_Address, P_Id) " +
                                       $"VALUES ('{customerId}', '{customerEmail}', '{customerAddress}', '{personId}')";

                int customerRowsAffected = dataAccess.ExecuteDMLQuery(customerQuery);

                if (customerRowsAffected > 0)
                {
                    MessageBox.Show("Customer added successfully!");
                    ClearAllCustomerInput();
                    LoadCustomerData();
                    // Sales
                    LoadCustomerDataForSales();
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

                if (!admin.IsPhoneNumberValid(customerPhoneNumber))
                {
                    MessageBox.Show("Invalid phone number.");
                    return;
                }

                // Check if the phone number is unique and valid
                if (!admin.IsPhoneNumberUniqueForUpdate(customerPhoneNumber, admin.GetPersonIdByCustomerId(customerId)))
                {
                    MessageBox.Show("Phone number already exists.");
                    return;
                }

                if (!admin.IsEmailValid(customerEmail))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                // Check if the email is unique and valid
                if (!admin.IsEmailUniqueForCustomer(customerEmail, customerId))
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
                    // Sales
                    LoadCustomerDataForSales();
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
                string personIdToDelete = admin.GetPersonIdByCustomerId(customerIdToDelete);

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
                        // Sales
                        LoadCustomerDataForSales();
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
                    title.Add(new Chunk("Printed by: " + materialTextBoxEmployeeUsername.Text, font14));
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



        //All Sales related code
        // Customer
        private bool IsValidToAddToCartCustomer()
        {
            return !String.IsNullOrEmpty(this.materialTextBoxSalesCustomerId.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxSalesCustomerName.Text) &&
                   !String.IsNullOrEmpty(this.materialTextBoxSalesCustomerPhoneNumber.Text);
        }

        private void LoadCustomerDataForSales(string searchValue = "")
        {
            try
            {
                string sqlQuery;

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    // SQL query to select Customer data along with related Person details if searchValue is empty
                    sqlQuery = "SELECT C.C_Id AS 'Customer ID', P.P_Name As 'Name', P.P_PhoneNumber AS 'Phone Number' " +
                               "FROM Customer C " +
                               "INNER JOIN Person P ON C.P_Id = P.P_Id ";
                }
                else
                {
                    // SQL query to select Customer data based on the searchValue with related Person details, filtered by name
                    sqlQuery = "SELECT C.C_Id AS 'Customer ID', P.P_Name As 'Name', P.P_PhoneNumber AS 'Phone Number' " +
                                $"FROM Customer C " +
                                $"INNER JOIN Person P ON C.P_Id = P.P_Id WHERE P.P_Name LIKE '%{searchValue}%' OR P.P_PhoneNumber LIKE '%{searchValue}%'";

                }

                DataAccess dataAccess = new DataAccess();
                DataSet customerDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (customerDataSet != null && customerDataSet.Tables.Count > 0)
                {
                    dataGridViewSalesCustomer.DataSource = customerDataSet.Tables[0];

                    // Auto-size columns based on content
                    dataGridViewSalesCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Set specific widths for columns
                    dataGridViewSalesCustomer.Columns["Customer ID"].Width = 100;
                    dataGridViewSalesCustomer.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewSalesCustomer.Columns["Phone Number"].Width = 200;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading customer data: {exc.Message}");
            }
        }
        private void dataGridViewSalesCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridViewSalesCustomer.Rows[e.RowIndex];

                    // Retrieve data from the selected row's cells
                    string customerId = selectedRow.Cells["Customer ID"].Value.ToString();
                    string customerName = selectedRow.Cells["Name"].Value.ToString();
                    string customerPhoneNumber = selectedRow.Cells["Phone Number"].Value.ToString();

                    // Populate UI elements with the retrieved data
                    materialTextBoxSalesCustomerId.Text = customerId;
                    materialTextBoxSalesCustomerName.Text = customerName;
                    materialTextBoxSalesCustomerPhoneNumber.Text = customerPhoneNumber;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void materialTextBoxSalesCusomerSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = materialTextBoxSalesCustomerSearch.Text.Trim();
            LoadCustomerDataForSales(searchValue);
        }

        // Item
        private bool IsValidToAddToCartItem()
        {
            // Check if the required text fields are not empty or null and validate specific formats
            return !string.IsNullOrEmpty(materialTextBoxSalesItemId.Text) &&
                   !string.IsNullOrEmpty(materialTextBoxSalesItemName.Text) &&
                   double.TryParse(materialTextBoxSalesItemPrice.Text, out _) && // Check for valid double
                   int.TryParse(materialTextBoxSalesItemQuantity.Text, out int quantity) && // Check for valid integer
                   quantity > 0; // Ensure quantity is greater than zero
        }

        private void LoadItemDataForSales(string searchValue = "")
        {
            try
            {
                string sqlQuery;

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    // SQL query to select Item data along with the related Shop and Factory names if searchValue is empty
                    sqlQuery = "SELECT I_Id AS 'Item ID', I_Name AS 'Item Name', I_Price AS 'Item Price', I_ExpireDate AS 'Item Expire Date', I_Quantity AS 'Item Quantity' FROM Item ";
                }
                else
                {
                    // SQL query to select Item data based on the searchValue with the related Shop and Factory names, filtered by Item Name
                    sqlQuery = $"SELECT I_Id AS 'Item ID', I_Name AS 'Item Name', I_Price AS 'Item Price', I_ExpireDate AS 'Item Expire Date', I_Quantity AS 'Item Quantity' FROM Item WHERE I_Name LIKE '%{searchValue}%'";
                }

                DataAccess dataAccess = new DataAccess();
                DataSet itemDataSet = dataAccess.ExecuteQuery(sqlQuery);

                if (itemDataSet != null && itemDataSet.Tables.Count > 0)
                {
                    dataGridViewSalesItem.DataSource = itemDataSet.Tables[0];

                    // Auto-size columns based on content
                    dataGridViewSalesItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // set specific widths for columns
                    dataGridViewSalesItem.Columns["Item ID"].Width = 100;
                    dataGridViewSalesItem.Columns["Item Name"].Width = 200;
                    dataGridViewSalesItem.Columns["Item Price"].Width = 150;
                    dataGridViewSalesItem.Columns["Item Expire Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewSalesItem.Columns["Item Quantity"].Width = 150;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"An error occurred while loading item data: {exc.Message}");
            }
        }

        private void materialTextBoxSalesItemSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = materialTextBoxSalesItemSearch.Text.Trim();
            LoadItemDataForSales(searchValue);
        }

        private void dataGridViewSalesItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridViewSalesItem.Rows[e.RowIndex];

                    // Populate text boxes with the clicked row's data
                    materialTextBoxSalesItemId.Text = row.Cells["Item ID"].Value.ToString();
                    materialTextBoxSalesItemName.Text = row.Cells["Item Name"].Value.ToString();
                    materialTextBoxSalesItemPrice.Text = row.Cells["Item Price"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // A list to hold cart items
        List<CartItem> cartItems = new List<CartItem>();

        // Method to add items to the cart
        private void materialButtonSalesAddToCart_Click(object sender, EventArgs e)
        {
            if (IsValidToAddToCartCustomer())
            {
                // Ensure that required fields are not empty and the data is valid before adding to the cart
                if (IsValidToAddToCartItem())
                {
                    string itemId = materialTextBoxSalesItemId.Text;
                    string itemName = materialTextBoxSalesItemName.Text;
                    double itemPrice = Convert.ToDouble(materialTextBoxSalesItemPrice.Text);
                    int quantity = Convert.ToInt32(materialTextBoxSalesItemQuantity.Text);

                    // Fetch item details from the database to validate quantity and expiry date
                    DataAccess dataAccess = new DataAccess();
                    string sqlQuery = $"SELECT I_Quantity, I_ExpireDate FROM Item WHERE I_Id = '{itemId}'";
                    DataSet itemDetails = dataAccess.ExecuteQuery(sqlQuery);

                    if (itemDetails != null && itemDetails.Tables.Count > 0 && itemDetails.Tables[0].Rows.Count > 0)
                    {
                        int availableQuantity = Convert.ToInt32(itemDetails.Tables[0].Rows[0]["I_Quantity"]);
                        DateTime expireDate = Convert.ToDateTime(itemDetails.Tables[0].Rows[0]["I_ExpireDate"]);

                        if (expireDate > DateTime.Now)
                        {
                            int totalQuantityInCart = cartItems.Where(item => item.ItemId == itemId).Sum(item => item.Quantity);
                            int remainingQuantity = availableQuantity - totalQuantityInCart;

                            if (remainingQuantity >= quantity)
                            {
                                CartItem existingItem = cartItems.FirstOrDefault(item => item.ItemId == itemId);

                                if (existingItem != null)
                                {
                                    existingItem.Quantity += quantity;
                                    existingItem.TotalPrice = existingItem.ItemPrice * existingItem.Quantity;
                                }
                                else
                                {
                                    // Find the maximum serial number in the existing cart items
                                    int maxSerialNumber = cartItems.Count > 0 ? cartItems.Max(item => item.SerialNumber) : 0;

                                    // Assign the next available serial number
                                    int newSerialNumber = maxSerialNumber + 1;
                                    CartItem newItem = new CartItem(newSerialNumber, itemId, itemName, itemPrice, quantity);
                                    cartItems.Add(newItem);
                                }

                                UpdateCartDataGridView();
                                CalculateAmountPayable();
                                materialButtonSalesConfirmOrder.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("The quantity exceeds the available stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("The item has expired.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all required fields with valid data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a customer.");
            }
        }

        private void UpdateCartDataGridView()
        {
            // Clear the DataGridView
            dataGridViewSalesCart.Rows.Clear();

            // Add cart items to the DataGridView
            foreach (CartItem item in cartItems)
            {
                dataGridViewSalesCart.Rows.Add(item.SerialNumber, item.ItemId, item.ItemName, item.ItemPrice, item.Quantity, item.TotalPrice);
            }

            dataGridViewSalesCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            // Set column headers and adjust column widths
            dataGridViewSalesCart.Columns["SerialNumber"].Width = 10; // Adjust width for SerialNumber column
            dataGridViewSalesCart.Columns["ItemId"].Width = 50; // Adjust width for ItemId column
            dataGridViewSalesCart.Columns["ItemName"].Width = 100; // Adjust width for ItemName column
            dataGridViewSalesCart.Columns["ItemPrice"].Width = 80; // Adjust width for ItemPrice column
            dataGridViewSalesCart.Columns["Quantity"].Width = 80; // Adjust width for Quantity column
            dataGridViewSalesCart.Columns["TotalPrice"].Width = 100; // Adjust width for TotalPrice column
        }

        private double amountPayable = 0.0;

        private void CalculateAmountPayable()
        {
            amountPayable = cartItems.Sum(item => item.TotalPrice);

            // Assuming materialTextBoxSalesAmountPayable is the TextBox where you want to display the total amount
            materialTextBoxSalesAmountPayable.Text = amountPayable.ToString(); // Display the total amount in the textbox
        }

        private void ClearCartItems()
        {
            cartItems.Clear(); // Clear all items in the cart
            UpdateCartDataGridView(); // Update the DataGridView to reflect the cleared cart
            CalculateAmountPayable(); // Recalculate and update the total amount after clearing the cart
            LoadItemDataForSales(); // Reload the item data in the DataGridView
            materialButtonSalesConfirmOrder.Enabled = false; // Disable the Confirm Order button
            materialTextBoxSalesAmountPayable.Text = "0.0"; // Clear Amount payable
            materialTextBoxSalesAmountPaid.Text = ""; // Clear Amount paid
        }

        private void materialButtonSalesClearCart_Click(object sender, EventArgs e)
        {
            ClearCartItems();
        }

        private void materialButtonSalesDeleteFromCart_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewSalesCart.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridViewSalesCart.SelectedRows[0];
                    int serialNumberToRemove = Convert.ToInt32(selectedRow.Cells["SerialNumber"].Value);

                    CartItem itemToRemove = cartItems.FirstOrDefault(item => item.SerialNumber == serialNumberToRemove);
                    if (itemToRemove != null)
                    {
                        cartItems.Remove(itemToRemove); // Remove the item from the cartItems list
                        UpdateCartDataGridView();
                        CalculateAmountPayable();
                        ResetSerialNumbers();

                        // Check if the cart is empty, and disable the Confirm Order button if it is
                        if (cartItems.Count == 0)
                        {
                            materialButtonSalesConfirmOrder.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selected item not found in the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an item to delete from the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the item from the cart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetSerialNumbers()
        {
            int newSerialNumber = 1;
            foreach (CartItem item in cartItems)
            {
                item.SerialNumber = newSerialNumber++;
            }
            UpdateCartDataGridView();
        }

        private string AutoOrderId()
        {
            DataAccess dataAccess = new DataAccess();
            var dt = dataAccess.ExecuteQuery(@"SELECT TOP 1 O_Id FROM Orders ORDER BY O_Id DESC;");

            if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
            {
                string lastId = dt.Tables[0].Rows[0][0].ToString();
                string[] id = lastId.Split('-');

                if (id.Length == 2 && id[0] == "O")
                {
                    if (int.TryParse(id[1], out int newIdNum))
                    {
                        newIdNum++; // Increment the ID
                        string newId = "O-" + newIdNum.ToString().PadLeft(10, '0'); // Padding to ensure two digits
                        return newId;
                    }
                }
            }

            // If no rows are returned or an invalid ID format is encountered, start the ID from "P-001"
            return "O-0000000001";
        }

        private bool IsValidToConfirmOrder()
        {
            // Check if the required text fields are not empty or null and validate specific formats
            if (string.IsNullOrEmpty(materialTextBoxSalesAmountPayable.Text) ||
                string.IsNullOrEmpty(materialTextBoxSalesAmountPaid.Text))
            {
                return false; // One or both fields are empty or null
            }

            return double.TryParse(materialTextBoxSalesAmountPayable.Text, out double payableAmount) &&
                   double.TryParse(materialTextBoxSalesAmountPaid.Text, out double paidAmount) &&
                   payableAmount > 0 &&
                   paidAmount > 0;
        }

        private void materialButtonSalesConfirmOrder_Click(object sender, EventArgs e)
        {
            if (IsValidToConfirmOrder())
            {
                double payableAmount = Convert.ToDouble(materialTextBoxSalesAmountPayable.Text);
                double paidAmount = Convert.ToDouble(materialTextBoxSalesAmountPaid.Text);

                if (payableAmount == paidAmount)
                {
                    string customerId = materialTextBoxSalesCustomerId.Text; // Get customer ID from the textbox
                    string employeeId = materialTextBoxEmployeeId.Text; // Get employee ID from the textbox

                    string orderId = AutoOrderId(); // Generate Order ID using AutoOrderId() method

                    DataAccess dataAccess = new DataAccess();

                    try
                    {
                        // Insert data into Orders table
                        decimal totalAmount = Convert.ToDecimal(payableAmount);
                        DateTime purchaseDate = DateTime.Now;

                        string insertOrderQuery = "INSERT INTO Orders (O_Id, O_TotalAmount, O_PurchaseDate, C_Id, E_Id) " +
                            "VALUES (@OrderId, @TotalAmount, @PurchaseDate, @CustomerId, @EmployeeId)";

                        SqlParameter[] orderParameters = {
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@TotalAmount", totalAmount),
                        new SqlParameter("@PurchaseDate", purchaseDate),
                        new SqlParameter("@CustomerId", customerId),
                        new SqlParameter("@EmployeeId", employeeId)
                        };

                        dataAccess.ExecuteDMLQuery(insertOrderQuery, orderParameters);

                        foreach (CartItem item in cartItems)
                        {
                            string itemId = item.ItemId;
                            int quantity = item.Quantity;

                            // Insert into OrderDetails table
                            string insertOrderDetailsQuery = "INSERT INTO OrderDetails (O_Id, I_Id, O_Quantity) " +
                                "VALUES (@OrderId, @ItemId, @Quantity)";

                            SqlParameter[] parameters = {
                            new SqlParameter("@OrderId", orderId),
                            new SqlParameter("@ItemId", itemId),
                            new SqlParameter("@Quantity", quantity)
                            };

                            dataAccess.ExecuteDMLQuery(insertOrderDetailsQuery, parameters);

                            // Update Item table I_Quantity
                            string updateItemQuantityQuery = "UPDATE Item SET I_Quantity = I_Quantity - @Quantity WHERE I_Id = @ItemId";

                            SqlParameter[] updateParameters = {
                            new SqlParameter("@Quantity", quantity),
                            new SqlParameter("@ItemId", itemId)
                            };

                            dataAccess.ExecuteDMLQuery(updateItemQuantityQuery, updateParameters);
                        }

                        // Clear cartItems list and update DataGridView after successful order confirmation
                        ClearCartItems();

                        // Generate payment slip
                        GeneratePaymentSlip(customerId, employeeId, orderId);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while confirming the order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Paid amount doesn't match with bill amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Enter Paid Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void GeneratePaymentSlip(string customerId, string employeeId, string orderId)
        {
            try
            {
                DataAccess dataAccess = new DataAccess();

                string currentDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filePath = $@"C:\Users\AZMINUR RAHMAN\Desktop\2023-2024, Fall\OBJECT ORIENTED PROGRAMMING 2\Project C#\Receipt\PaymentSlip_{currentDateTime}.pdf";

                // Creating a PDF document
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Retrieve customer details
                string customerQuery = "SELECT P_Name, P_Gender, P_PhoneNumber FROM Person INNER JOIN Customer ON Person.P_Id = Customer.P_Id WHERE C_Id = @CustomerId";
                SqlParameter[] customerParams = { new SqlParameter("@CustomerId", customerId) };
                DataSet customerData = dataAccess.ExecuteQuery(customerQuery, customerParams);

                string customerName = string.Empty;
                string customerGender = string.Empty;
                string customerPhoneNumber = string.Empty;

                if (customerData != null && customerData.Tables.Count > 0 && customerData.Tables[0].Rows.Count > 0)
                {
                    DataRow customerRow = customerData.Tables[0].Rows[0];
                    customerName = customerRow["P_Name"].ToString();
                    customerGender = customerRow["P_Gender"].ToString();
                    customerPhoneNumber = customerRow["P_PhoneNumber"].ToString();
                }

                // Retrieve employee details
                string employeeQuery = "SELECT P_Name FROM Person INNER JOIN Employee ON Person.P_Id = Employee.P_Id WHERE E_Id = @EmployeeId";
                SqlParameter[] employeeParams = { new SqlParameter("@EmployeeId", employeeId) };
                DataSet employeeData = dataAccess.ExecuteQuery(employeeQuery, employeeParams);

                string employeeName = string.Empty;

                if (employeeData != null && employeeData.Tables.Count > 0 && employeeData.Tables[0].Rows.Count > 0)
                {
                    DataRow employeeRow = employeeData.Tables[0].Rows[0];
                    employeeName = employeeRow["P_Name"].ToString();
                }

                // Retrieve items bought for the given orderId
                string orderDetailsQuery = "SELECT Item.I_Id, Item.I_Name, Item.I_Price, OrderDetails.O_Quantity FROM Item INNER JOIN OrderDetails ON Item.I_Id = OrderDetails.I_Id WHERE O_Id = @OrderId";
                SqlParameter[] orderDetailsParams = { new SqlParameter("@OrderId", orderId) };
                DataSet orderDetailsData = dataAccess.ExecuteQuery(orderDetailsQuery, orderDetailsParams);

                StringBuilder itemDetails = new StringBuilder();
                itemDetails.AppendLine("Items Bought:");

                decimal totalPaidAmount = 0;

                foreach (DataRow itemRow in orderDetailsData.Tables[0].Rows)
                {
                    string itemId = itemRow["I_Id"].ToString();
                    string itemName = itemRow["I_Name"].ToString();
                    decimal itemPrice = Convert.ToDecimal(itemRow["I_Price"]);
                    int quantity = Convert.ToInt32(itemRow["O_Quantity"]);

                    totalPaidAmount += itemPrice * quantity;

                    itemDetails.AppendLine($"Item ID: {itemId}, Name: {itemName}, Quantity: {quantity}, Price: {itemPrice:C}");
                }

                // Generate the payment slip with fetched details
                Paragraph paymentSlip = new Paragraph();
                paymentSlip.Add(new Chunk("Payment Slip\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
                paymentSlip.Add($"Order ID: {orderId}\n");
                paymentSlip.Add($"Customer Name: {customerName}\n");
                paymentSlip.Add($"Customer Gender: {customerGender}\n");
                paymentSlip.Add($"Customer Phone Number: {customerPhoneNumber}\n");
                paymentSlip.Add($"Employee Name: {employeeName}\n");
                paymentSlip.Add(itemDetails.ToString());
                paymentSlip.Add(new Chunk($"Total Paid Amount: {totalPaidAmount:C}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));

                document.Add(paymentSlip);
                document.Close();

                MessageBox.Show("Payment slip generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating payment slip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}