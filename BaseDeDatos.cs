using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Bloc_notas_wpf
{
    class BaseDeDatos
    {

        public void CrearBaseDeDatos()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "Localhost",
                UserID = "root",
                Password = ""
            };

            String consulta =
                              "DROP DATABASE IF EXISTS `bloc_notas`;" +
                              "CREATE DATABASE IF NOT EXISTS `bloc_notas` /*!40100 DEFAULT CHARACTER SET latin1 */;" +
                              "USE `bloc_notas`;" +
                              "DROP TABLE IF EXISTS `notas`;" +
                              "CREATE TABLE IF NOT EXISTS `notas` (" +
                              "`Titulo` varchar(50) NOT NULL," +
                              "`Ruta` varchar(250) NOT NULL," +
                              "`Contenido` varchar(20000) NOT NULL" +
                              ") ENGINE = InnoDB DEFAULT CHARSET = latin1;" +
                              "--Volcando datos para la tabla bloc_notas.notas: ~0 rows(aproximadamente)" +
                              "DELETE FROM `notas`;";

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.Write("Base de datos creada");
                    }
                    catch (Exception e)
                    {
                        Console.Write("Error " + e.ToString());
                    }
                }
                con.Close();
            }
        }
    }
}
