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
    public partial class MainPage : Form
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PlantStoreDB;Integrated Security=True";

        public MainPage()
        {
            InitializeComponent();
            LoadPlants();
        }

        private void LoadPlants()
        {
            flowLayoutPanelPlants.Controls.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT PlantName, ImagePath, Category, Price FROM Plants";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string plantName = reader["PlantName"].ToString();
                            string imagePath = reader["ImagePath"].ToString();
                            string category = reader["Category"].ToString();
                            decimal price = (decimal)reader["Price"];

                            // Создайте и добавьте контрол для отображения информации о растении
                            AddPlantControl(plantName, imagePath, category, price);
                        }
                    }
                }
            }
        }

        private void AddPlantControl(string plantName, string imagePath, string category, decimal price)
        {
            // Создайте новый UserControl или Panel для отображения информации о растении
            PlantControl plantControl = new PlantControl();
            plantControl.SetPlantInfo(plantName, imagePath, category, price);
            
            // Добавьте UserControl в FlowLayoutPanel
            flowLayoutPanelPlants.Controls.Add(plantControl);
        }



        /*private void DisplayPlantInfo(string plantName, string imagePath, string category, decimal price)
        {
            // Загрузка изображения
            pictureBox2.Image = Image.FromFile(imagePath);

            // Отображение информации в Label'ах
            label3.Text = $"Название: {plantName}";
            label4.Text = $"Категория: {category}";
            label5.Text = $"Цена: {price:C2}";
        }*/
        public void InitializeForm(string username)
        {
            // Предполагается, что lblAdminVisible и lblUserVisible - это ваши лейблы на форме MainPage

            if (IsAdminUser(username))
            {
                label1.Visible = true;
                pictureBox1.Visible = false;
            }
            else
            {
                label1.Visible = false;
                pictureBox1.Visible = true;
            }
        }

        private bool IsAdminUser(string username)
        {
            // Здесь вы можете добавить логику для проверки, является ли пользователь администратором.
            // Например, вы можете сравнивать username с известным логином администратора.
            return username.ToLower() == "admin";
        }
        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            AdminPage adminPage = new AdminPage();
            adminPage.Show();
            this.Hide();
        }
    }  

}
