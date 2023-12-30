using System.Data;
using System.Data.SqlClient;

namespace Pastry_Shop_Management_System
{
    internal class DataAccess
    {
        private SqlConnection sqlcon;
        public SqlConnection Sqlcon
        {
            get { return this.sqlcon; }
            set { this.sqlcon = value; }
        }

        private SqlCommand sqlcom;
        public SqlCommand Sqlcom
        {
            get { return this.sqlcom; }
            set { this.sqlcom = value; }
        }

        private SqlDataAdapter sda;
        public SqlDataAdapter Sda
        {
            get { return this.sda; }
            set { this.sda = value; }
        }

        private DataSet ds;
        public DataSet Ds
        {
            get { return this.ds; }
            set { this.ds = value; }
        }

        //internal DataTable dt;

        public DataAccess()
        {
            this.sqlcon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\AZMINUR RAHMAN\Desktop\2023-2024, Fall\OBJECT ORIENTED PROGRAMMING 2\Project C#\SQL\Pastry Shop Management System SQL.mdf;Integrated Security=True;Connect Timeout=30;");
            Sqlcon.Open();
        }

        private void QueryText(string query)
        {
            this.Sqlcom = new SqlCommand(query, this.Sqlcon);
        }

        public DataSet ExecuteQuery(string sql)
        {
            this.Sqlcom = new SqlCommand(sql, this.Sqlcon);
            this.Sda = new SqlDataAdapter(this.Sqlcom);
            this.Ds = new DataSet();
            this.Sda.Fill(this.Ds);
            return Ds;
        }

        public DataSet ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            this.Sqlcom = new SqlCommand(sql, this.Sqlcon);
            if (parameters != null)
            {
                Sqlcom.Parameters.AddRange(parameters);
            }

            this.Sda = new SqlDataAdapter(Sqlcom);
            this.Ds = new DataSet();
            this.Sda.Fill(this.Ds);
            return Ds;
        }


        public int ExecuteDMLQuery(string sql)
        {
            this.Sqlcom = new SqlCommand(sql, this.Sqlcon);
            int u = this.Sqlcom.ExecuteNonQuery();
            return u;
        }

        public int ExecuteDMLQuery(string sql, SqlParameter[] parameters = null)
        {
            this.Sqlcom = new SqlCommand(sql, this.Sqlcon);

            if (parameters != null)
            {
                this.Sqlcom.Parameters.AddRange(parameters);
            }

            int affectedRows = this.Sqlcom.ExecuteNonQuery();
            this.Sqlcom.Parameters.Clear(); // Clear parameters for reusability

            return affectedRows;
        }

    }

}
