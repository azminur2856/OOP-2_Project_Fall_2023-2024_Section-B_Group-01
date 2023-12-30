namespace Pastry_Shop_Management_System
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.panelLoginL = new System.Windows.Forms.Panel();
            this.labelComment = new System.Windows.Forms.Label();
            this.panelLoginR = new System.Windows.Forms.Panel();
            this.linkLabelActivation = new System.Windows.Forms.LinkLabel();
            this.materialButtonLogin = new MaterialSkin.Controls.MaterialButton();
            this.materialTextBoxPassword = new MaterialSkin.Controls.MaterialTextBox2();
            this.materialTextBoxUserName = new MaterialSkin.Controls.MaterialTextBox2();
            this.linkLabelForgotPassword = new System.Windows.Forms.LinkLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelLoginTitle = new System.Windows.Forms.Label();
            this.imageListLogin = new System.Windows.Forms.ImageList(this.components);
            this.panelLoginL.SuspendLayout();
            this.panelLoginR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLoginL
            // 
            this.panelLoginL.BackColor = System.Drawing.Color.Black;
            this.panelLoginL.Controls.Add(this.labelComment);
            this.panelLoginL.Location = new System.Drawing.Point(-1, 29);
            this.panelLoginL.Name = "panelLoginL";
            this.panelLoginL.Size = new System.Drawing.Size(220, 321);
            this.panelLoginL.TabIndex = 0;
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.BackColor = System.Drawing.Color.Black;
            this.labelComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelComment.ForeColor = System.Drawing.Color.White;
            this.labelComment.Location = new System.Drawing.Point(1, 77);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(203, 125);
            this.labelComment.TabIndex = 9;
            this.labelComment.Text = "\"Pastries a culinary \r\ncanvas, blending \r\nartistry and \r\nheritage in \r\nevery bite" +
    ".\"\r\n";
            this.labelComment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelLoginR
            // 
            this.panelLoginR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelLoginR.Controls.Add(this.linkLabelActivation);
            this.panelLoginR.Controls.Add(this.materialButtonLogin);
            this.panelLoginR.Controls.Add(this.materialTextBoxPassword);
            this.panelLoginR.Controls.Add(this.materialTextBoxUserName);
            this.panelLoginR.Controls.Add(this.linkLabelForgotPassword);
            this.panelLoginR.Controls.Add(this.pictureBox);
            this.panelLoginR.Controls.Add(this.labelLoginTitle);
            this.panelLoginR.Location = new System.Drawing.Point(221, 29);
            this.panelLoginR.Name = "panelLoginR";
            this.panelLoginR.Size = new System.Drawing.Size(380, 321);
            this.panelLoginR.TabIndex = 1;
            // 
            // linkLabelActivation
            // 
            this.linkLabelActivation.AutoSize = true;
            this.linkLabelActivation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelActivation.Location = new System.Drawing.Point(257, 234);
            this.linkLabelActivation.Name = "linkLabelActivation";
            this.linkLabelActivation.Size = new System.Drawing.Size(82, 20);
            this.linkLabelActivation.TabIndex = 5;
            this.linkLabelActivation.TabStop = true;
            this.linkLabelActivation.Text = "Activation";
            this.linkLabelActivation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelActivation_LinkClicked);
            // 
            // materialButtonLogin
            // 
            this.materialButtonLogin.AutoSize = false;
            this.materialButtonLogin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButtonLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButtonLogin.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButtonLogin.Depth = 0;
            this.materialButtonLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialButtonLogin.HighEmphasis = true;
            this.materialButtonLogin.Icon = null;
            this.materialButtonLogin.Location = new System.Drawing.Point(39, 268);
            this.materialButtonLogin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButtonLogin.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButtonLogin.Name = "materialButtonLogin";
            this.materialButtonLogin.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButtonLogin.Size = new System.Drawing.Size(300, 35);
            this.materialButtonLogin.TabIndex = 3;
            this.materialButtonLogin.Text = "LOGIN";
            this.materialButtonLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButtonLogin.UseAccentColor = false;
            this.materialButtonLogin.UseVisualStyleBackColor = true;
            this.materialButtonLogin.Click += new System.EventHandler(this.materialButtonLogin_Click);
            // 
            // materialTextBoxPassword
            // 
            this.materialTextBoxPassword.AnimateReadOnly = false;
            this.materialTextBoxPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.materialTextBoxPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxPassword.Depth = 0;
            this.materialTextBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialTextBoxPassword.HideSelection = true;
            this.materialTextBoxPassword.Hint = "Password";
            this.materialTextBoxPassword.LeadingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxPassword.LeadingIcon")));
            this.materialTextBoxPassword.Location = new System.Drawing.Point(39, 169);
            this.materialTextBoxPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.materialTextBoxPassword.MaxLength = 32767;
            this.materialTextBoxPassword.MouseState = MaterialSkin.MouseState.OUT;
            this.materialTextBoxPassword.Name = "materialTextBoxPassword";
            this.materialTextBoxPassword.PasswordChar = '\0';
            this.materialTextBoxPassword.PrefixSuffixText = null;
            this.materialTextBoxPassword.ReadOnly = false;
            this.materialTextBoxPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.materialTextBoxPassword.SelectedText = "";
            this.materialTextBoxPassword.SelectionLength = 0;
            this.materialTextBoxPassword.SelectionStart = 0;
            this.materialTextBoxPassword.ShortcutsEnabled = true;
            this.materialTextBoxPassword.Size = new System.Drawing.Size(300, 48);
            this.materialTextBoxPassword.TabIndex = 1;
            this.materialTextBoxPassword.TabStop = false;
            this.materialTextBoxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxPassword.TrailingIcon = null;
            this.materialTextBoxPassword.UseSystemPasswordChar = false;
            this.materialTextBoxPassword.TrailingIconClick += new System.EventHandler(this.materialTextBoxPassword_TrailingIconClick);
            // 
            // materialTextBoxUserName
            // 
            this.materialTextBoxUserName.AnimateReadOnly = false;
            this.materialTextBoxUserName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.materialTextBoxUserName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.materialTextBoxUserName.Depth = 0;
            this.materialTextBoxUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialTextBoxUserName.HideSelection = true;
            this.materialTextBoxUserName.Hint = "Username";
            this.materialTextBoxUserName.LeadingIcon = ((System.Drawing.Image)(resources.GetObject("materialTextBoxUserName.LeadingIcon")));
            this.materialTextBoxUserName.Location = new System.Drawing.Point(39, 105);
            this.materialTextBoxUserName.MaxLength = 32767;
            this.materialTextBoxUserName.MouseState = MaterialSkin.MouseState.OUT;
            this.materialTextBoxUserName.Name = "materialTextBoxUserName";
            this.materialTextBoxUserName.PasswordChar = '\0';
            this.materialTextBoxUserName.PrefixSuffixText = null;
            this.materialTextBoxUserName.ReadOnly = false;
            this.materialTextBoxUserName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.materialTextBoxUserName.SelectedText = "";
            this.materialTextBoxUserName.SelectionLength = 0;
            this.materialTextBoxUserName.SelectionStart = 0;
            this.materialTextBoxUserName.ShortcutsEnabled = true;
            this.materialTextBoxUserName.Size = new System.Drawing.Size(300, 48);
            this.materialTextBoxUserName.TabIndex = 0;
            this.materialTextBoxUserName.TabStop = false;
            this.materialTextBoxUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.materialTextBoxUserName.TrailingIcon = null;
            this.materialTextBoxUserName.UseSystemPasswordChar = false;
            // 
            // linkLabelForgotPassword
            // 
            this.linkLabelForgotPassword.AutoSize = true;
            this.linkLabelForgotPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelForgotPassword.Location = new System.Drawing.Point(39, 234);
            this.linkLabelForgotPassword.Name = "linkLabelForgotPassword";
            this.linkLabelForgotPassword.Size = new System.Drawing.Size(145, 20);
            this.linkLabelForgotPassword.TabIndex = 4;
            this.linkLabelForgotPassword.TabStop = true;
            this.linkLabelForgotPassword.Text = "Forgot Password?";
            this.linkLabelForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForgotPassword_LinkClicked);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(158, 32);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(75, 71);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            // 
            // labelLoginTitle
            // 
            this.labelLoginTitle.AutoSize = true;
            this.labelLoginTitle.BackColor = System.Drawing.Color.White;
            this.labelLoginTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoginTitle.ForeColor = System.Drawing.SystemColors.Desktop;
            this.labelLoginTitle.Location = new System.Drawing.Point(128, 1);
            this.labelLoginTitle.Name = "labelLoginTitle";
            this.labelLoginTitle.Size = new System.Drawing.Size(153, 29);
            this.labelLoginTitle.TabIndex = 1;
            this.labelLoginTitle.Text = "Pastry Shop";
            // 
            // imageListLogin
            // 
            this.imageListLogin.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLogin.ImageStream")));
            this.imageListLogin.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLogin.Images.SetKeyName(0, "eyeClose.png");
            this.imageListLogin.Images.SetKeyName(1, "eyeOpen.png");
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Controls.Add(this.panelLoginR);
            this.Controls.Add(this.panelLoginL);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Login";
            this.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.panelLoginL.ResumeLayout(false);
            this.panelLoginL.PerformLayout();
            this.panelLoginR.ResumeLayout(false);
            this.panelLoginR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLoginL;
        private System.Windows.Forms.Panel panelLoginR;
        private System.Windows.Forms.Label labelLoginTitle;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.LinkLabel linkLabelForgotPassword;
        private MaterialSkin.Controls.MaterialTextBox2 materialTextBoxUserName;
        private MaterialSkin.Controls.MaterialTextBox2 materialTextBoxPassword;
        private MaterialSkin.Controls.MaterialButton materialButtonLogin;
        private System.Windows.Forms.LinkLabel linkLabelActivation;
        private System.Windows.Forms.ImageList imageListLogin;
    }
}