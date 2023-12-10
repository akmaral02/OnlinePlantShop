using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OnlinePlantShop
{
    public partial class AdminPage : Form
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PlantStoreDB;Integrated Security=True";
        private string imagePath;

        public AdminPage()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string plantName = textBox1.Text;
            string description = textBox2.Text;
            string category = textBox3.Text;
            decimal price = decimal.Parse(textBox4.Text);

            SavePlantToDatabase(plantName, description, imagePath, category, price);

            MessageBox.Show("Растение успешно добавлено!");

            MainPage mainPage = new MainPage();
            mainPage.Show();
            this.Hide();
        }

        private void SavePlantToDatabase(string plantName, string description, string imagePath, string category, decimal price)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Пример: сохранение изображения в базу данных в виде байтов
                /*byte[] imageBytes = File.ReadAllBytes(imagePath);*/

                string query = "INSERT INTO Plants (PlantName, Description, ImagePath, Category, Price) VALUES (@PlantName, @Description, @ImagePath, @Category, @Price)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlantName", plantName);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@ImagePath", imagePath);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@Price", price);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif|Все файлы (*.*)|*.*";
                openFileDialog.Title = "Выберите изображение";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = openFileDialog.FileName;
                    pictureBox1.Image = Image.FromFile(imagePath);
                }
            }
        }
    }
}
