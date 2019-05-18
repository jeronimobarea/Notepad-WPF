using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Bloc_notas_wpf
{
    /// <summary>
    /// Interaction logic for ventanaRutas.xaml
    /// </summary>
    public partial class ventanaRutas : Window
    {
        public ventanaRutas()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "Localhost",
                UserID = "root",
                Password = "",
                Database = "bloc_notas"
            };

            string consulta = "INSERT INTO rutas (Ruta) VALUES ('"+ textRuta.Text +"');";

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception xe)
                    {
                        System.Windows.Forms.MessageBox.Show("Error " + xe.ToString());
                        Console.Write("Error " + xe.ToString());
                    }
                }
                con.Close();
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "Localhost",
                UserID = "root",
                Password = "",
                Database = "bloc_notas"
            };

            string consulta = "DELETE FROM rutas WHERE Ruta = '" + textRuta.Text + "';";

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception xe)
                    {
                        System.Windows.Forms.MessageBox.Show("Error " + xe.ToString());
                        Console.Write("Error " + xe.ToString());
                    }
                }
                con.Close();
            }
        }
    }
}
