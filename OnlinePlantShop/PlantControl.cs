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
    public partial class PlantControl : UserControl
    {

        public bool IsAdmin { get; set; }
        private string plantName;
        public PlantControl()
        {
            InitializeComponent();
        }

        /*public void SetButtonVisibility()
        {
            label2.Visible = IsAdmin;
        }*/

        public void SetPlantInfo(string plantName, string imagePath, string category, decimal price)
        {
            this.plantName = plantName;
            pictureBoxPlant.SizeMode = PictureBoxSizeMode.Zoom;

            // Установите информацию о растении в элементы UserControl
            pictureBoxPlant.Image = Image.FromFile(imagePath);
            labelPlantName.Text = $" {plantName}";
            labelCategory.Text = $" {category}";
            labelPrice.Text = $" {price} Cом";
        }

        public void SetUserInterfaceVisibility(string username)
        {
            // Предполагается, что label1 и pictureBox1 - это ваши элементы на форме MainPage

            /*string username = *//* Логика получения логина пользователя, например, textBox1.Text */

            if (IsAdminUser(username))
            {
                label2.Visible = true; // Это элемент, видимый только для админа
                /*pictureBox1.Visible = false;*/ // Этот элемент, видимый только для пользователя, скрываем
            }
            else
            {
                label2.Visible = false; // Этот элемент, видимый только для админа, скрываем
                /*pictureBox1.Visible = true;*/ // Этот элемент, видимый только для пользователя
            }
        }


        private bool IsAdminUser(string username)
        {
            // Здесь вы можете добавить логику для проверки, является ли пользователь администратором.
            // Например, вы можете сравнивать username с известным логином администратора.
            return username.ToLower() == "admin";
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить растение?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Ваш код для удаления растения из базы данных
                DeletePlantFromDatabase(plantName);
                MessageBox.Show("Растение успешно удалено!");
                
            }
            AdminPage adminPage = new AdminPage();
            adminPage.Show();
            this.Hide();
        }

        private void DeletePlantFromDatabase(string plantName)
        {
            // Ваша строка подключения к базе данных
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PlantStoreDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Plants WHERE PlantName = @PlantName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlantName", plantName);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
