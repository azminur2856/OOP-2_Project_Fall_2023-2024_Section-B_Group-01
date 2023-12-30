using System;
using System.Windows.Forms;

namespace Pastry_Shop_Management_System
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }
        private void Welcome_Load(object sender, EventArgs e)
        {
            progressBarTimer.Start();
        }
        int startpoint = 0;
        private void progressBarTimer_Tick(object sender, EventArgs e)
        {
            startpoint += 1;
            progressBar.Value = startpoint;
            percentageLabel.Text = startpoint + "%";
            if (progressBar.Value == 100)
            {
                progressBarTimer.Stop();
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }
    }
}
