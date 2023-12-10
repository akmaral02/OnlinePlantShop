using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;

namespace OnlinePlantShop
{
    public partial class SignUp : Form
    {

        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PlantStoreDB;Integrated Security=True";
        public SignUp()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newUsername = textBox3.Text;
            string newPassword = textBox2.Text;

            // Хешируем пароль
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Сохраняем пользователя в базу данных
            RegisterUser(newUsername, hashedPassword);

            MessageBox.Show("Регистрация успешна!");    
            
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
        private void RegisterUser(string username, string passwordHash)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    

                    command.ExecuteNonQuery();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}
