using BCrypt.Net;
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

namespace OnlinePlantShop
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PlantStoreDB;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

         

            // Получаем хеш пароля из базы данных
            string storedPasswordHash = GetPasswordHash(username);

            // Проверяем введенный пароль
            try
            {
                // Проверяем введенный пароль
                if (IsAdmin(username) && password == storedPasswordHash)
                {
                    MessageBox.Show("Авторизация успешна для админа!");
                }
                else if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                {
                    MessageBox.Show("Авторизация успешна!");
                    
                }
                else
                {
                    MessageBox.Show("Неверные логин или пароль.");
                }
            }
            catch (SaltParseException ex)
            {
                MessageBox.Show($"Ошибка при проверке пароля: {ex.Message}");
            }
            MainPage mainPage = new MainPage();           
            mainPage.InitializeForm(username);
            mainPage.Show();
            this.Hide();
            PlantControl plantControl = new PlantControl();
            plantControl.SetUserInterfaceVisibility(username);
        }

        private bool IsAdmin(string username)
        {
            // Здесь вы можете добавить проверку на администратора, например, по имени пользователя или роли.
            return username.ToLower() == "admin";
        }




        private string GetPasswordHash(string username)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    return (string)command.ExecuteScalar();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}
