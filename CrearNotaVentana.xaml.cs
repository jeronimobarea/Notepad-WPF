using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Bloc_notas_wpf
{
    /// <summary>
    /// Lógica de interacción para CrearNotaVentana.xaml
    /// </summary>
    public partial class CrearNotaVentana : Window
    {
        #region >>>>> Declaración de Variables.
        /* 
         * Declaramos un array carpeta que almacena las 4 principales carpetas en el 5 espacio almacenaremos la "Carpeta Actual".
         * Declaramos un string archivo que guarda el titulo del archivo que pasa la ventana principal.
         */
        private string[] carpeta = { @"C:\BlocNotas\", @"C:\BlocNotas\Default\", @"C:\BlocNotas\Papelera\", @"C:\BlocNotas\rutas\", "Actual" };
        private string archivo;
        #endregion
        string nombre;
        #region >>>>> Declaración de Listas.
        /*
         * Creamos una lista (listRutas) que almacenará objetos de datosRutas.
         */
        private List<datosRutas> listRutas = new List<datosRutas>();
        #endregion

        #region >>>>> Declaración de Plantillas/Binds.
        private DataTemplate dtRutas = new DataTemplate();
        private System.Windows.Data.Binding bindingRuta = new System.Windows.Data.Binding();
        #endregion

        #region >>>>> Declaración de Objetos (FrameworkElementFactory).
        private FrameworkElementFactory botonRuta = new FrameworkElementFactory(typeof(System.Windows.Controls.Button));
        private FrameworkElementFactory wrap = new FrameworkElementFactory(typeof(WrapPanel));
        #endregion

        #region >>>>> Constructor.
        /*
         * El constructor recibe dos variables de tipo string archivo y carpetaActual.
         * Ejecuta un if que determinará si hay que crear una nota nueva o editar una existente.
         * Ejecuta el metodo LeerRutas().
         */
        public CrearNotaVentana(string archivo, string carpetaActual)
        {
            InitializeComponent();

            try
            {
                this.archivo = archivo; // Almacenamos el valor del archivo que han pasado desde otra ventana.

                if (archivo != null || carpetaActual != null) // Si alguna de las dos variables que nos pasan son null ejecutamos el if.
                {
                    MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
                    {
                        Server = "Localhost",
                        UserID = "root",
                        Password = "",
                        Database = "bloc_notas"
                    };

                    string consulta = "SELECT Contenido FROM notas WHERE Titulo = '" + archivo + "';";

                    using (MySqlConnection con = new MySqlConnection(builder.ToString()))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                        {
                            try
                            {
                                MySqlDataReader reader;
                                reader = cmd.ExecuteReader();

                                while (reader.Read())
                                {
                                    string rt = reader.GetValue(0).ToString();
                                    textboxContenido.Text = rt;
                                }
                            }
                            catch (Exception e)
                            {
                                System.Windows.Forms.MessageBox.Show("Error " + e.ToString());
                                Console.Write("Error " + e.ToString());
                            }
                        }
                        con.Close();
                    }
                }
                else
                {
                    carpeta[4] = carpeta[1]; // Sino se cumple el if carpeta[4] (carpeta actual) pasa a ser carpeta[1] (carpeta default).
                }
                LeerRutas(); // Ejecutamos el metodo LeerRutas().
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
        #endregion

        #region >>>>> Todos los Metodos/Funciones.
        /*
         * El metodo generar rutas es el mismo que el de la anterior ventana.
         */
        private void GenerarRutas()
        {
            try
            {
                dtRutas = new DataTemplate();

                bindingRuta = new System.Windows.Data.Binding();
                bindingRuta.Path = new PropertyPath("Ruta");

                botonRuta = new FrameworkElementFactory(typeof(System.Windows.Controls.Button));
                botonRuta.SetBinding(ContentProperty, bindingRuta);
                botonRuta.SetValue(NameProperty, "botonRuta");
                botonRuta.SetValue(ForegroundProperty, Brushes.Black);
                botonRuta.SetValue(BackgroundProperty, Brushes.Transparent);
                botonRuta.SetValue(FontSizeProperty, 12d);
                botonRuta.SetValue(MinWidthProperty, 255d);
                botonRuta.SetValue(MinHeightProperty, 35d);
                botonRuta.SetValue(BorderThicknessProperty, new Thickness(0));
                botonRuta.SetValue(BorderThicknessProperty, new Thickness(0, 1, 0, 1));
                botonRuta.SetValue(BorderBrushProperty, Brushes.LightGray);
                botonRuta.AddHandler(System.Windows.Controls.Button.ClickEvent, new RoutedEventHandler(BotonRuta_Click));

                wrap = new FrameworkElementFactory(typeof(WrapPanel));
                wrap.SetValue(MaxWidthProperty, 271d);
                wrap.AppendChild(botonRuta);

                dtRutas.VisualTree = wrap;

                listaRutas.ItemTemplate = dtRutas;
                listaRutas.ItemsSource = listRutas;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
        /*
         * El metodo LeerRutas() es el mismo que el de la ventana anterior.
         */
        private void LeerRutas()
        {
            //try
            //{
            //    int numLineas = File.ReadLines(carpeta[3] + "rutas.txt").Count();

            //    listRutas.Clear();
            //    GenerarRutas();

            //    for (int i = 0; i < numLineas; i++)
            //    {
            //        string[] rt = File.ReadLines(carpeta[3] + "rutas.txt").ToArray();
            //        listRutas.Add(new datosRutas(rt[i].ToString()));
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.Write(e.ToString());
            //}

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "Localhost",
                UserID = "root",
                Password = "",
                Database = "bloc_notas"
            };

            string consulta = "SELECT Ruta FROM rutas;";

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    try
                    {
                        listRutas.Clear(); // Limpiamos la lista (listRutas).
                        GenerarRutas(); // Generamos el DataTemplate.

                        MySqlDataReader reader;
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            String rt = reader.GetValue(0).ToString();
                            listRutas.Add(new datosRutas(rt));
                        }
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show("Error " + e.ToString());
                        Console.Write("Error " + e.ToString());
                    }
                }
                con.Close();
            }
        }
        /*
         * El metodo BuscarCarpeta() es el mismo que el de la anterior ventana.
         */
        private void BuscarCarpeta()
        {
            //try
            //{
            //    using (var fbd = new FolderBrowserDialog())
            //    {
            //        DialogResult result = fbd.ShowDialog();

            //        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //        {
            //            string path = fbd.SelectedPath;

            //            System.IO.File.AppendAllText(carpeta[3] + "rutas.txt", path + Environment.NewLine);
            //            LeerRutas();
            //        }

            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.Write(e.ToString());
            //}

            ventanaRutas ru = new ventanaRutas();
            ru.Show();
            LeerRutas();
        }
        /*
         * El metodo CrearArchivo() crea un archivo txt con el contenido del textbox.
         */
        public void CrearArchivo()
        {
            //try
            //{
            //    String[] nombreArchivoPartes = textboxContenido.Text.Split(' '); // Separa el texto del textbox por espacios y lo añade al array.
            //    string nombreArchivo = ""; // Creamos un string que almacenará el nombre del archivo.

            //    for (int i = 0; i < nombreArchivoPartes.Length; i++) // Añadimos las tres primeras palabras del array al nombreArchivo.
            //    {
            //        if (i < 3)
            //        {
            //            nombreArchivo += nombreArchivoPartes[i];
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }

            //    string vacio = textboxContenido.Text; // Creamos un string que almacenará el el contenido del textbox.

            //    if (!File.Exists(carpeta[4] + nombreArchivo + ".txt")) // Si el archivo no existe añade el texto al archivo.
            //    {
            //        using (StreamWriter sw = File.CreateText(carpeta[4] + nombreArchivo + ".txt"))
            //        {
            //            sw.Write(vacio);
            //            sw.Close();
            //        }
            //    }
            //    else // Si el archivo existe añade un 1 al final del nombre.
            //    {
            //        using (StreamWriter sw = File.CreateText(carpeta[4] + nombreArchivo + "(1).txt"))
            //        {
            //            sw.Write(vacio);
            //            sw.Close();
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.Write(e.ToString());
            //}

            String[] nombreArchivoPartes = textboxContenido.Text.Split(' '); // Separa el texto del textbox por espacios y lo añade al array.
            string nombreArchivo = ""; // Creamos un string que almacenará el nombre del archivo.

            for (int i = 0; i < nombreArchivoPartes.Length; i++) // Añadimos las tres primeras palabras del array al nombreArchivo.
            {
                if (i < 3)
                {
                    nombreArchivo += nombreArchivoPartes[i];
                }
                else
                {
                    break;
                }
            }

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "Localhost",
                UserID = "root",
                Password = "",
                Database = "bloc_notas"
            };

            DateTime dt = new DateTime();

            string ruta = "'" + expanderRutas.Header.ToString() + "'";
            nombre = "'" + nombreArchivo + "'";
            string fecha = "'" + dt.Date.ToString() + "'";
            string contenido = "'" + textboxContenido.Text + "'";

            string consulta = "INSERT INTO notas (Titulo, Ruta, Fecha, Contenido) VALUES (" + nombre + "," + ruta + "," + fecha + "," + contenido + ");";

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show("Error " + e.ToString());
                        Console.Write("Error " + e.ToString());
                    }
                }
                con.Close();
            }

        }
        /*
         * El metodo GuardarArchivo() guarda el texto.
         */
        private void GuardarArchivo()
        {
            //try
            //{
            //    string contenido = textboxContenido.Text; // Este string almacena el contenido del textbox.

            //    if (archivo != null) // Si el archivo que nos han pasado no es null ejecuta el if.
            //    {
            //        if (File.Exists(carpeta[4] + archivo)) // Si el archivo exista ya en la carpeta lo borra para actualizar el contenido.
            //        {
            //            File.Delete(carpeta[4] + archivo); // Borra el archivo.
            //        }
            //        System.IO.File.WriteAllText(carpeta[4] + archivo, contenido); // Escribe el contenido en el archivo.
            //    }
            //    else // Si el archivo no es null y contenido no esta vacio ejecuta el metodo CrearArchivo().
            //    {
            //        if (contenido != "")
            //        {
            //            CrearArchivo();
            //        }

            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.Write(e.ToString());
            //}

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "Localhost",
                UserID = "root",
                Password = "",
                Database = "bloc_notas"
            };

            DateTime dt = new DateTime();
            string fecha = "'" + dt.Date.ToString() + "'";
            string contenido = "'" + textboxContenido.Text + "'";

            string consulta = "select count(*) from notas where Titulo = '" + archivo + "'";
            string consultaUpdate = "UPDATE notas SET Ruta = " + "'1'" + ", Fecha = " + fecha + ", Contenido = " + contenido + "WHERE Titulo = '" + archivo + "'";

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    try
                    {
                        MySqlDataReader reader;
                        reader = cmd.ExecuteReader();

                        int existe;

                        if (reader.Read())
                        {
                            existe = reader.GetInt32(0);

                            if (existe > 0)
                            {
                                using (MySqlConnection con2 = new MySqlConnection(builder.ToString()))
                                {
                                    con2.Open();
                                    using (MySqlCommand cmd2 = new MySqlCommand(consultaUpdate, con2))
                                    {
                                        cmd2.ExecuteNonQuery();
                                    }
                                    con2.Close();
                                }

                            }
                            else
                            {
                                CrearArchivo();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show("Error " + e.ToString());
                        Console.Write("Error " + e.ToString());
                    }
                }
                con.Close();
            }
        }
        /*
         * El metodo EditarNota() borra el contenido del textbox y añade el de la nota a editar.
         */
        private void EditarNota()
        {
            try
            {
                textboxContenido.Text = "";
                foreach (string file in Directory.EnumerateFiles(carpeta[4], archivo))
                {
                    String contenido = File.ReadAllText(file).ToString();
                    textboxContenido.Text += contenido;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
        #endregion

        #region >>>>> Todos los Eventos.
        /*
         * Al iniciar la ventana la centramos en pantalla.
         */
        private void Window_Initialized(object sender, EventArgs e)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        /*
         * El evento BotonRuta_Click nos permite cambiar la ruta al hacer click.
         */
        private void BotonRuta_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button botonPulsado = (System.Windows.Controls.Button)sender;

            try
            {
                expanderRutas.Header = botonPulsado.Content;
                carpeta[4] = expanderRutas.Header.ToString() + @"\";
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonVolver_Click oculta la ventana actual y vuelve a la main al hacer click.
         */
        private void BotonVolver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                MainWindow mn = new MainWindow();
                mn.Show();
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonGuardar_Click ejecuta el metodo GuardarArchivo() al ahcer click.
         */
        private void BotonGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardarArchivo();
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonNuevanota_Click nos permite editar el la nota al hacer click.
         */
        private void BotonAddCarpeta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BuscarCarpeta();
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }

        }
        /*
         * El evento TextboxContenido_PreviewMouseDown limpia el textbox en caso de que el texto actual sea el default al hacer click.
         */
        private void TextboxContenido_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (textboxContenido.Text.Equals("Escribe algo aquí..."))
                {
                    textboxContenido.Text = "";
                }
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }

        }
        /*
         * El evento TextboxContenido_LostFocus agrega el texto por defecto al textbox en caso de que este este vacio al perder el foco.
         */
        private void TextboxContenido_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textboxContenido.Text.Equals(""))
                {
                    textboxContenido.Text = "Escribe algo aquí...";
                }
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }

        }
        /*
         * El evento BotonLimpiar_Click limpia el texto del textbox al hacer click.
         */
        private void BotonLimpiar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textboxContenido.Text = "";
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }

        }
        /*
         * El evento BotonCerrar_Click nos permite cerrar la aplicacion al hacer click.
         */
        private void BotonCerrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonMinimizar_Click nos permite minimizar la ventana al hacer click.
         */
        private void BotonMinimizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento Rectangle_MouseDown nos permite arrastrar la ventana por la pantalla al hacer click y mantener.
         */
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        #endregion
    }
}