using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ExtraFields
{
    public partial class ExtraFields : Form
    {
        //string serverName = "192.168.2.2";
        //string dbname = "LIVE";
        //string usname = "SA";
        //string pass = "1q2w#E$R%T";

        string serverName = "JFEVISUAL7";
        string dbname = "LIVE";
        string usname = "SYSADM";
        string pass = "COPOW05";

        string type = "ad";
        string searchField = "";


        public ExtraFields(string[] args)
        {
            string SqlConnectionString = $"Data Source={serverName}; Initial Catalog = {dbname}; User ID = {usname}; Password = {pass}; MultipleActiveResultSets=True";
            InitializeComponent();
            ProcessArguments(args);

            //MessageBox.Show(args.ToString());

            this.Text = "Extra fields - " + searchField.ToString();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, 0);

        }

        private void ProcessArguments(string[] args)
        {
            // Process the arguments here and update your form accordingly
            // For example, if args[0] contains a parameter you want to use:
            // label1.Text = args[0];
            if (args.Length >= 2)
            { 
                type = args[1].ToString();
                searchField = args[2].ToString();
            }
            if (type.ToUpper() == "Z_PART")
            {
                label1.Text = "Part ID: ";
                label2.Text = "Build Tolerance Minus: ";
                label3.Text = "Build Tolerance Plus: ";
            }
            else
            {
                MessageBox.Show("Type unknown!");
                this.Close();
                return;
            }

        }

        private void check_if_record_exists()
        {
            string check_else_insert = @"DECLARE @SEARCH_FIELD VARCHAR(60)
                            SET @SEARCH_FIELD = @SEARCH_FIELD_PARAM
                            IF NOT EXISTS (SELECT 1 FROM Z_PART WHERE PART_ID = @SEARCH_FIELD)
                                BEGIN
                                    INSERT INTO Z_PART (PART_ID)
                                    VALUES (@SEARCH_FIELD);
                                END ";
            string SqlConnectionString = $"Data Source={serverName}; Initial Catalog = {dbname}; User ID = {usname}; Password = {pass}; MultipleActiveResultSets=True";

            try
            {
                SqlConnection conn = new SqlConnection(SqlConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = check_else_insert;
                cmd.Parameters.AddWithValue("@SEARCH_FIELD_PARAM", searchField);
                conn.Open();
                var record = cmd.ExecuteNonQuery();
                conn.Close();

                textBox2.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void onLoad(object sender, EventArgs e)
        {
            check_if_record_exists();
            get_information();
        }

        private void get_information()
        {
            string details = @"SELECT TOP 1 * FROM Z_PART WHERE PART_ID = @PART_ID ";
            string SqlConnectionString = $"Data Source={serverName}; Initial Catalog = {dbname}; User ID = {usname}; Password = {pass}; MultipleActiveResultSets=True";

            try
            {
                SqlConnection conn = new SqlConnection(SqlConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = details;
                cmd.Parameters.AddWithValue("@PART_ID", searchField);
                conn.Open();
                var record = cmd.ExecuteReader();

                if (record.Read())
                {
                    // Retrieve values from the reader and set them in the text fields
                    textBox1.Text = searchField;
                    textBox2.Text = record["BUILD_TOL_PLUS"].ToString();
                    textBox3.Text = record["BUILD_TOL_MINUS"].ToString();
                }
                else
                {
                    textBox2.Text = "";
                    textBox3.Text = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateLabel2(object sender, EventArgs e)
        {
            //Validate
            if (textBox2.Text == "")
            {
                MessageBox.Show("Value is required!");
                textBox2.Focus();
                return;
            }

            string COLUMNS = "BUILD_TOL_PLUS";
            string VALUE = textBox2.Text;

            string update_statement = @"UPDATE Z_PART SET " +  COLUMNS + " = " + VALUE + " WHERE PART_ID = '" + searchField + "'";
            string SqlConnectionString = $"Data Source={serverName}; Initial Catalog = {dbname}; User ID = {usname}; Password = {pass}; MultipleActiveResultSets=True";
            SqlConnection conn = new SqlConnection(SqlConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = update_statement;
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();


        }

        private void updateLabel3(object sender, EventArgs e)
        {
            //Validate 
            if (textBox3.Text == "")
            {
                MessageBox.Show("Value is required!");
                textBox2.Focus();
                return;
            }

            string COLUMNS = "BUILD_TOL_MINUS";
            string VALUE = textBox3.Text;

            string update_statement = @"UPDATE Z_PART SET " + COLUMNS + " = " + VALUE + " WHERE PART_ID = '" + searchField + "'";
            string SqlConnectionString = $"Data Source={serverName}; Initial Catalog = {dbname}; User ID = {usname}; Password = {pass}; MultipleActiveResultSets=True";
            SqlConnection conn = new SqlConnection(SqlConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = update_statement;
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
        }
    }

}
