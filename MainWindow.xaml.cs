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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Bloc_notas_wpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region >>>>> Declaración de Variables.
        /* 
         * Declaramos un array carpeta que almacena las 4 principales carpetas en el 5 espacio almacenaremos la "Carpeta Actual".
         * Declaramos un array Titulos que almacena los titulos de las notas que previsualizamos.
         * Creamos un contador que nos ayudará a crear diferentes identificadores y navegar por los arrays.
         */
        private string[] Titulos = new string[1000];
        private string carpetaActual;
        private int contador = 0;
        #endregion

        #region >>>>> Declaración de Listas.
        /* 
         * Declaramos una Lista llamada "ListType" que almacenará un objeto datosNota.
         * Declaramos una Lista llamada "ListRutas" que alamacenará un objeto datosRutas.
         */
        private List<datosNotas> listNotas = new List<datosNotas>();
        private List<datosRutas> listRutas = new List<datosRutas>();
        #endregion

        #region >>>>> Declaración de Plantillas (DataTemplate).
        private DataTemplate dt = new DataTemplate();
        private DataTemplate dtRutas = new DataTemplate();
        #endregion

        #region >>>>> Declaración de Objetos (FrameworkElementFactory).
        private FrameworkElementFactory textoTitulo = new FrameworkElementFactory(typeof(TextBlock));
        private FrameworkElementFactory textoContenido = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBox));
        private FrameworkElementFactory textoFecha = new FrameworkElementFactory(typeof(TextBlock));
        private FrameworkElementFactory botonEliminar = new FrameworkElementFactory(typeof(System.Windows.Controls.Button));
        private FrameworkElementFactory botonEditar = new FrameworkElementFactory(typeof(System.Windows.Controls.Button));
        private FrameworkElementFactory stackTitulo = new FrameworkElementFactory(typeof(StackPanel));
        private FrameworkElementFactory wrap = new FrameworkElementFactory(typeof(WrapPanel));
        private FrameworkElementFactory stack = new FrameworkElementFactory(typeof(StackPanel));
        private FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
        private FrameworkElementFactory botonRuta = new FrameworkElementFactory(typeof(System.Windows.Controls.Button));
        private FrameworkElementFactory wrapRutas = new FrameworkElementFactory(typeof(WrapPanel));
        #endregion

        #region >>>>> Declaración de Bindings.
        private System.Windows.Data.Binding bindingId = new System.Windows.Data.Binding();
        private System.Windows.Data.Binding bindingTitulo = new System.Windows.Data.Binding();
        private System.Windows.Data.Binding bindingContenido = new System.Windows.Data.Binding();
        private System.Windows.Data.Binding bindingFecha = new System.Windows.Data.Binding();
        private System.Windows.Data.Binding bindingRuta = new System.Windows.Data.Binding();
        #endregion

        #region >>>>> Constructor.
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region >>>>> Todos los Metodos/Funciones.
        /*
         * El metodo CrearDirectoriosDefault() crea todos las carpetas por defecto al inicio del programa en caso de que no existan.
         * En el archivo rutas txt se almacenarán todas las carpetas que agreguemos al expander.
         * Comprobamos la ejecución con un try y un catch.
         */
        private void CrearBaseDeDatos()
        {
            try
            {
                BaseDeDatos bd = new BaseDeDatos();
                bd.CrearBaseDeDatos();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

        }
        /*
         * El metodo GenerarObjetos() crea un template para el listbox pruebaLista.
         * El objeto DataTemplate sirve para crear la plantilla de datos.
         * El objeto DataBinding sirve para obtener la informacón de las clases.
         * El objeto FrameWorkElementFactory nos ayuda a crear objetos mediante codigo.       
         */
        private void GenerarObjetos()
        {
            try
            {
                #region -> Plantilla y Bindeos.
                dt = new DataTemplate(); // Creamos un objeto DataTemplate

                bindingId = new System.Windows.Data.Binding(); // Creamos un objeto DataBinding
                bindingId.Path = new PropertyPath("Id"); //Asignamos al objeto DataBinding el Id de datosNotas

                bindingTitulo = new System.Windows.Data.Binding(); // Creamos un objeto DataBinding
                bindingTitulo.Path = new PropertyPath("Titulo"); //Asignamos al objeto DataBinding el Titulo de datosNotas

                bindingContenido = new System.Windows.Data.Binding(); // Creamos un objeto DataBinding
                bindingContenido.Path = new PropertyPath("Contenido"); //Asignamos al objeto DataBinding el Contenido de datosNotas

                bindingFecha = new System.Windows.Data.Binding(); // Creamos un objeto DataBinding
                bindingFecha.Path = new PropertyPath("Fecha"); //Asignamos al objeto DataBinding la Fecha de datosNotas
                #endregion
                #region -> Objetos y Propiedades.
                textoTitulo = new FrameworkElementFactory(typeof(TextBlock)); // Creamos un objeto FrameworkElementFactory de tipo TextBlock
                textoTitulo.SetBinding(TextBlock.TextProperty, bindingTitulo); // Le añadimos un bind (bindingTitulo).
                textoTitulo.SetValue(MarginProperty, new Thickness(15d, 5d, 0d, 0d)); // Alteramos las propiedades del objeto.
                textoTitulo.SetValue(ForegroundProperty, Brushes.White);
                textoTitulo.SetValue(FontWeightProperty, FontWeights.Bold);
                textoTitulo.SetValue(FontSizeProperty, 21d);

                textoContenido = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBox)); // Creamos un objeto FrameworkElementFactory de tipo TextBox
                textoContenido.SetBinding(System.Windows.Controls.TextBox.TextProperty, bindingContenido); // Le añadimos un bind (bindingContenido).
                textoContenido.SetValue(MarginProperty, new Thickness(0d, 2d, 15d, 0d)); // Alteramos las propiedades del objeto.
                textoContenido.SetValue(PaddingProperty, new Thickness(0d, 0d, 10d, 0d));
                textoContenido.SetValue(System.Windows.Controls.TextBox.HeightProperty, 145d);
                textoContenido.SetValue(System.Windows.Controls.TextBox.MaxHeightProperty, 145d);
                textoContenido.SetValue(System.Windows.Controls.TextBox.MinWidthProperty, 1055d);
                textoContenido.SetValue(System.Windows.Controls.TextBox.MaxWidthProperty, 1060d);
                textoContenido.SetValue(ForegroundProperty, Brushes.DimGray);
                textoContenido.SetValue(FontWeightProperty, FontWeights.Regular);
                textoContenido.SetValue(BackgroundProperty, Brushes.Transparent);
                textoContenido.SetValue(System.Windows.Controls.TextBox.IsReadOnlyProperty, true);
                textoContenido.SetValue(FontSizeProperty, 12d);
                textoContenido.SetValue(System.Windows.Controls.TextBox.TextWrappingProperty, TextWrapping.Wrap);
                textoContenido.SetValue(System.Windows.Controls.TextBox.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                textoContenido.SetValue(BorderThicknessProperty, new Thickness(0));

                textoFecha = new FrameworkElementFactory(typeof(TextBlock)); // Creamos un objeto FrameworkElementFactory de tipo TextBlock
                textoFecha.SetBinding(TextBlock.TextProperty, bindingFecha); // Le añadimos un bind (bindingFecha).
                textoFecha.SetValue(MarginProperty, new Thickness(945d, 15d, 20d, 0d)); // Alteramos las propiedades del objeto.
                textoFecha.SetValue(ForegroundProperty, Brushes.DimGray);
                textoFecha.SetValue(FontWeightProperty, FontWeights.Regular);
                textoFecha.SetValue(FontStyleProperty, FontStyles.Italic);
                textoFecha.SetValue(FontSizeProperty, 11d);

                botonEliminar = new FrameworkElementFactory(typeof(System.Windows.Controls.Button)); // Creamos un objeto FrameworkElementFactory de tipo button
                botonEliminar.SetBinding(System.Windows.Controls.Button.NameProperty, bindingId); // Le añadimos un bind (bindingId).
                botonEliminar.SetValue(System.Windows.Controls.Button.ContentProperty, "Eliminar"); // Alteramos las propiedades del objeto.
                botonEliminar.SetValue(WidthProperty, 80d);
                botonEliminar.SetValue(HeightProperty, 40d);
                botonEliminar.SetValue(ForegroundProperty, Brushes.IndianRed);
                botonEliminar.SetValue(BackgroundProperty, Brushes.Transparent);
                botonEliminar.SetValue(FontWeightProperty, FontWeights.Bold);
                botonEliminar.SetValue(BorderThicknessProperty, new Thickness(0, 0, 1, 0));
                botonEliminar.SetValue(BorderBrushProperty, Brushes.Wheat);
                botonEliminar.AddHandler(System.Windows.Controls.Button.ClickEvent, new RoutedEventHandler(BotonEliminar_Click)); // Añadimos un Click event.

                botonEditar = new FrameworkElementFactory(typeof(System.Windows.Controls.Button)); // Creamos un objeto FrameworkElementFactory de tipo button
                botonEditar.SetBinding(System.Windows.Controls.Button.NameProperty, bindingId); // Le añadimos un bind (bindingId).
                botonEditar.SetValue(ContentProperty, "Editar"); // Alteramos las propiedades del objeto.
                botonEditar.SetValue(WidthProperty, 80d);
                botonEditar.SetValue(HeightProperty, 40d);
                botonEditar.SetValue(ForegroundProperty, Brushes.White);
                botonEditar.SetValue(BackgroundProperty, Brushes.Transparent);
                botonEditar.SetValue(FontWeightProperty, FontWeights.Bold);
                botonEditar.SetValue(BorderThicknessProperty, new Thickness(0));
                botonEditar.AddHandler(System.Windows.Controls.Button.ClickEvent, new RoutedEventHandler(BotonEditar_Click)); // Añadimos un Click event.

                stackTitulo = new FrameworkElementFactory(typeof(StackPanel)); // Creamos un objeto FrameworkElementFactory de tipo StackPanel.
                stackTitulo.SetValue(MinWidthProperty, 910d); // Alteramos las propiedades del objeto.
                stackTitulo.SetValue(MaxWidthProperty, 910d);
                stackTitulo.AppendChild(textoTitulo); // Agregamos un hijo (textTitulo).

                wrap = new FrameworkElementFactory(typeof(WrapPanel)); // Creamos un objeto FrameworkElementFactory de tipo WrapPanel.
                wrap.SetValue(BackgroundProperty, Brushes.LightSteelBlue); // Alteramos las propiedades del objeto.
                wrap.SetValue(MinWidthProperty, 1080d);
                wrap.SetValue(MaxWidthProperty, 1080d);
                wrap.AppendChild(stackTitulo); // Agregamos los hijos.
                wrap.AppendChild(botonEliminar);
                wrap.AppendChild(botonEditar);

                stack = new FrameworkElementFactory(typeof(StackPanel)); // Creamos un objeto FrameworkElementFactory de tipo StackPanel.
                stack.SetValue(HeightProperty, 220d); // Alteramos las propiedades del objeto.
                stack.SetValue(MinWidthProperty, 1080d);
                stack.SetValue(MaxWidthProperty, 1080d);
                stack.SetValue(MarginProperty, new Thickness(5));
                stack.AppendChild(wrap); // Agregamos los hijos.
                stack.AppendChild(textoContenido);
                stack.AppendChild(textoFecha);

                border = new FrameworkElementFactory(typeof(Border)); // Creamos un objeto FrameworkElementFactory de tipo Border.
                border.SetValue(MinWidthProperty, 1080d); // Alteramos las propiedades del objeto.
                border.SetValue(MaxWidthProperty, 1080d);
                border.SetValue(MarginProperty, new Thickness(0d, 10d, 0d, 0d));
                border.SetValue(BackgroundProperty, Brushes.White);
                border.SetValue(BorderBrushProperty, Brushes.LightGray);
                border.SetValue(BorderThicknessProperty, new Thickness(2));
                border.SetValue(Border.CornerRadiusProperty, new CornerRadius(3));
                border.AppendChild(stack); // Agregamos el hijo (stack).
                #endregion

                dt.VisualTree = border; // Añadimos el objeto border que amacena todos los restantes al DataTemplate (dt).

                pruebaLista.ItemTemplate = dt; // Añadimos dt al listbox (pruebalista) ItemTemplate que agrega el diseño del template.

                pruebaLista.ItemsSource = listNotas; // Añadimos la lista (listNotas) ItemsSource que agrega los objetos de la lista.
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
        /*
         * EL metodo LeerNotas() lee las notas de la carpeta seleccionada y crea los objetos correspondientes. 
         */
        private void LeerNotas()
        {
            //try
            //{
            //    contador = 0; // Reinicia el contador a 0.
            //    listNotas.Clear(); // Limpia la lista que almacena las notas.
            //    Array.Clear(Titulos, 0, Titulos.Length); // Limpia el array titulos.
            //    GenerarObjetos(); // Genera los objetos nota.

            //    foreach (string file in Directory.EnumerateFiles(carpeta[4], "*.txt")) // Da una vuelta por cada archivo en la carpeta actual.
            //    {
            //        string Titulo = System.IO.Path.GetFileName(file); // Almacena el titulo del archivo actual.

            //        Titulos[contador] = Titulo; // Añade el titulo al array.

            //        string[] partesfecha = Convert.ToString(File.GetLastAccessTime(file)).Split(' '); // Separa la fecha por espacios.
            //        string fecha = partesfecha[1] + "  -  " + partesfecha[0]; // Agrega los espacios 1 y 0 del array partesfecha.

            //        string Texto = File.ReadAllText(file).ToString(); // Almacena el texto del archivo.

            //        listNotas.Add(new datosNotas(Titulo, Texto, fecha, "identificador_" + contador)); // Crea un nuevo objeto y lo añade a la lista con los valores indicados.
            //        contador++; // Suma 1 al contador por cada vuelta.
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

            string consulta = "SELECT * FROM notas WHERE Ruta = " + expanderRutas.Header.ToString();

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    try
                    {
                        contador = 0;
                        listNotas.Clear();
                        Array.Clear(Titulos, 0, Titulos.Length);
                        GenerarObjetos();

                        MySqlDataReader reader;
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string rt = reader.GetValue(0).ToString();
                            string Titulo = rt; // Almacena el titulo del archivo actual.
                            Titulos[contador] = Titulo; // Añade el titulo al array.

                            listNotas.Add(new datosNotas(reader.GetValue(0).ToString(), reader.GetValue(3).ToString(), reader.GetValue(2).ToString(), "identificador_" + contador));
                            contador++;
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
         * El metodo BuscarNota() sirve para identificar la nota que corresponde con el texto del textbox buscar.
         */
        private void BuscarNota()
        {
            try
            {
                string busqueda = textBox.Text; // Almacena el valor del textbox "buscar".

                //if (busqueda != "") // Si busqueda no esta vacio ejecuta la acción.
                //{
                //    contador = 0; // Reinicia el contador a 0.
                //    listNotas.Clear(); // Limpia la lista de objetos.
                //    Array.Clear(Titulos, 0, Titulos.Length); // Limpia el array Titulos.
                //    GenerarObjetos(); // Genera los objetos notas.

                //    //foreach (string file in Directory.EnumerateFiles(carpeta[4], busqueda + ".txt")) // Da vueltas por cada arcchivo que coincida con el criterio.
                //    //{
                //    //    string Titulo = System.IO.Path.GetFileName(file); // Almacena el titulo de la nota.

                //    //    Titulos[contador] = Titulo; // Guarda el titulo en el array.

                //    //    string[] partesfecha = Convert.ToString(File.GetLastAccessTime(file)).Split(' '); // Separa las partes de la fecha.
                //    //    string fecha = partesfecha[1] + "  -  " + partesfecha[0]; // Organiza las partes en otro orden.

                //    //    string Texto = File.ReadAllText(file).ToString(); // Almacena el texto de la nota.

                //    //    listNotas.Add(new datosNotas(Titulo, Texto, fecha, "identificador_" + contador)); // Crea un objeto datos nota y lo agrega a la lista.
                //    //    contador++; // Suma 1 al contador.
                //    //}
                //}
                //else // Si busqueda esta vacio vuelve a cargar todas las notas.
                //{
                //    LeerNotas(); // Ejecuta el metodo LeerNotas().
                //}

                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
                {
                    Server = "Localhost",
                    UserID = "root",
                    Password = "",
                    Database = "bloc_notas"
                };

                string consulta = "SELECT * FROM notas WHERE Titulo = '" + busqueda + "'";

                using (MySqlConnection con = new MySqlConnection(builder.ToString()))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        contador = 0;
                        listNotas.Clear();
                        Array.Clear(Titulos, 0, Titulos.Length);
                        GenerarObjetos();

                        MySqlDataReader reader;
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string rt = reader.GetValue(0).ToString();
                            string Titulo = rt; // Almacena el titulo del archivo actual.
                            Titulos[contador] = Titulo; // Añade el titulo al array.

                            listNotas.Add(new datosNotas(reader.GetValue(0).ToString(), reader.GetValue(3).ToString(), reader.GetValue(2).ToString(), "identificador_" + contador));
                            contador++;
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
        /*
         * El metodo GenerarRutas() genera un diseño para las rutas.
         */
        private void GenerarRutas()
        {
            try
            {
                dtRutas = new DataTemplate(); // Crea un objeto DataTemplate "dtRutas".

                bindingRuta = new System.Windows.Data.Binding(); // Crea un objeto DataBinding.
                bindingRuta.Path = new PropertyPath("Ruta"); //Asignamos al objeto DataBinding la Ruta de datosRutas.

                botonRuta = new FrameworkElementFactory(typeof(System.Windows.Controls.Button)); // Creamos un objeto FrameworkElementFactory de tipo Button.
                botonRuta.SetBinding(ContentProperty, bindingRuta); // Alteramos las propiedades.
                botonRuta.SetValue(NameProperty, "botonRuta");
                botonRuta.SetValue(ForegroundProperty, Brushes.Black);
                botonRuta.SetValue(BackgroundProperty, Brushes.Transparent);
                botonRuta.SetValue(FontSizeProperty, 12d);
                botonRuta.SetValue(MinWidthProperty, 255d);
                botonRuta.SetValue(MinHeightProperty, 35d);
                botonRuta.SetValue(BorderThicknessProperty, new Thickness(0));
                botonRuta.SetValue(BorderThicknessProperty, new Thickness(0, 1, 0, 1));
                botonRuta.SetValue(BorderBrushProperty, Brushes.LightGray);
                botonRuta.AddHandler(System.Windows.Controls.Button.ClickEvent, new RoutedEventHandler(BotonRuta_Click)); // Añadimos un ClickEvent.

                wrapRutas = new FrameworkElementFactory(typeof(WrapPanel)); // Creamos un objeto FrameworkElementFactory de tipo WraPanel
                wrapRutas.SetValue(MaxWidthProperty, 271d); // Alteramos las propiedades.
                wrapRutas.AppendChild(botonRuta); // Añadimos un hijo (botonRuta).

                dtRutas.VisualTree = wrapRutas; // Añadimos el Panel (wrap) al DataTemplate.

                listaRutas.ItemTemplate = dtRutas; // Agregamos el DataTemplate(dt) al ItemTemplate de listaRutas.
                listaRutas.ItemsSource = listRutas; // Agregamos la lista (listRutas) al ItemSource de listaRutas.
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
        /*
         * El metodo LeerRutas() lee el archivo leer rutas.txt y genera objetos por cada ruta escrita.
         */
        public void LeerRutas()
        {
            //try
            //{
            //    int numLineas = File.ReadLines(carpeta[3] + "rutas.txt").Count(); // Cuenta el numero de lineas del archivo.

            //    listRutas.Clear(); // Limpiamos la lista (listRutas).
            //    GenerarRutas(); // Generamos el DataTemplate.

            //    for (int i = 0; i < numLineas; i++) // Damos una vuelta por cada numero de lineas en el documento.
            //    {
            //        string[] rt = File.ReadLines(carpeta[3] + "rutas.txt").ToArray(); // Agregamos las lineas al array.
            //        listRutas.Add(new datosRutas(rt[i].ToString())); // Creamos un objeto datosRutas con los datos especificados y lo añadimos a la lista.
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
                            string rt = reader.GetValue(0).ToString();
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
         * El metodo BuscarCarpeta() nos abre una ventana que nos permite seleccionar o crear una carpeta en nuestro expander.
         */
        private void BuscarCarpeta()
        {
            //try
            //{
            //    using (var fbd = new FolderBrowserDialog()) // usamos una variable de tipo FolderBrowserDialog.
            //    {
            //        DialogResult result = fbd.ShowDialog(); // Almacenamos el valor de la respuesta en una variable.

            //        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath)) // Si el resultado es Ok (Aceptar) y no esta en blanco o es null ejecuta el if.
            //        {
            //            string path = fbd.SelectedPath; // Guardamos la ruta en un String path 

            //            System.IO.File.AppendAllText(carpeta[3] + "rutas.txt", path + Environment.NewLine); // Escribimos la ruta nueva en el archivo rutas.txt
            //            LeerRutas(); // Ejecutamos el metodo LeerRutas();
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
        #endregion

        #region >>>>> Todos los Eventos.
        /*
         * Iniciamos la ventana el en centro de la pantalla.
         * Iniciamos el metodo CrearDirectoriosDefault().
         * Asignamos el espacio 5 del array "Carpeta Actual" al header del expander.
         * Iniciamos el metodo LeerRutas().
         * Iniciamos el metodo LeerNotas().
         */
        private void Window_Initialized(object sender, EventArgs e)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            carpetaActual = expanderRutas.Header.ToString();
            try
            {
                //CrearBaseDeDatos();
            }
            catch (Exception er)
            {

                Console.Write(er.ToString());
            }

            try
            {
                LeerRutas();
                LeerNotas();
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonEditar_Click nos permite editar el la nota al hacer click.
         */
        private void BotonEditar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button botonPulsado = (System.Windows.Controls.Button)sender; // Creamos un objeto Sender para trabajar con el boton que ha sido accionado.

            try
            {
                String[] nombreObjeto = botonPulsado.Name.ToString().Split('_'); // Hacemos un split para acceder al valor numerico del identificador.
                int numeroObjeto = Convert.ToInt16(nombreObjeto[1]); // Almacenamos el valor numerico en una variable int.

                string archivo = Titulos[numeroObjeto]; // Buscamos el titulo con el numeroObjeto en el array y lo agregamos al archivo.

                this.Hide(); // Ocultamos la ventana actual.
                CrearNotaVentana crearnota = new CrearNotaVentana(archivo, carpetaActual); // Pasamos la carpeta actual y el titulo del archivo y creamos un objeto CrearNotaVentana.
                crearnota.Show(); // Mostramos la venta.
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonEliminar_Click nos permite editar el la nota al hacer click.
         */
        private void BotonEliminar_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button botonPulsado = (System.Windows.Controls.Button)sender; // Creamos un objeto Sender para trabajar con el boton que ha sido accionado.

            try
            {
                String[] nombreObjeto = botonPulsado.Name.ToString().Split('_'); // Hacemos un split para acceder al valor numerico del identificador.
                int numeroObjeto = Convert.ToInt16(nombreObjeto[1]); // Almacenamos el valor numerico en una variable int.

                string archivo = Titulos[numeroObjeto];

                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
                {
                    Server = "Localhost",
                    UserID = "root",
                    Password = "",
                    Database = "bloc_notas"
                };

                string consulta = "DELETE FROM notas WHERE Titulo = '" + archivo + "'";

                using (MySqlConnection con = new MySqlConnection(builder.ToString()))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                LeerNotas(); // Ejecutamos el metodo LeerNotas().
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonRuta_Click nos permite cambiar la ruta al hacer click.
         */
        private void BotonRuta_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button botonPulsado = (System.Windows.Controls.Button)sender; // Creamos un objeto Sender para trabajar con el boton que ha sido accionado.

            try
            {
                expanderRutas.Header = botonPulsado.Content; // Cambiamos el texto del Header al contenido del boton pulsado.
                //carpeta[4] = expanderRutas.Header.ToString(); // Cambiamos la carpeta actual al texto del header (expander).

                LeerNotas(); // Ejecutamos el metodo LeerNotas().
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }
        }
        /*
         * El evento BotonNuevanota_Click nos permite acceder a otra pestaña al hacer click.
         */
        private void BotonNuevanota_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide(); // Ocultamos la ventana actual.
                CrearNotaVentana notanueva = new CrearNotaVentana(null, null); // Creamos un objeto CrearNotaVentana y le pasamos los datos como null.
                notanueva.Show(); // Mostramos la ventana notanueva.
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
                BuscarCarpeta(); // Ejecutamos el metodo BuscarCarpeta().
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }

        }
        /*
         * El evento BotonBorrartodo_Click nos permite borrar todas las notas de la ruta actual al hacer click.
         */
        private void BotonBorrartodo_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    System.IO.DirectoryInfo di = new DirectoryInfo(carpeta[4]); // Creamos un objeto de tipo DirectoryInfo con ruta carpeta actual (carpeta[4]).

            //    foreach (FileInfo file in di.EnumerateFiles()) // Da una vuelta por cada archivo en el objeto di.
            //    {
            //        file.Delete(); // Borra el archivo.
            //    }

            //    LeerNotas(); // Ejecuta el metodo LeerNotas(). 
            //}
            //catch (Exception er)
            //{
            //    Console.Write(er.ToString());
            //}

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "Localhost",
                UserID = "root",
                Password = "",
                Database = "bloc_notas"
            };

            string consulta = "DELETE FROM notas WHERE Ruta = '" + carpetaActual + "'";

            using (MySqlConnection con = new MySqlConnection(builder.ToString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }
        /*
         * El evento TextBox_PreviewKeyDown nos permite ejecutar el metodo BuscarNota() al apretar la tecla enter.
         */
        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    BuscarNota();
                }
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }

        }
        /*
         * El evento TextBox_PreviewMouseDown nos permite limpiar y formatear el texto al hacer click en el textbox.
         */
        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                textBox.Text = "";
                textBox.FontStyle = FontStyles.Normal;
                textBox.FontWeight = FontWeights.Regular;
            }
            catch (Exception er)
            {
                Console.Write(er.ToString());
            }

        }
        /*
         * El evento TextBox_LostFocus nos permite limpiar y formatear el texto cuando pierde el foco.
         */
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBox.Text = "Buscar algo...";
                textBox.FontStyle = FontStyles.Italic;
                textBox.FontWeight = FontWeights.Regular;
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
        /*
         * El evento BotonRefrescar_Click vuelve a cargar todas las notas al hacer click. 
         */
        private void BotonRefrescar_Click(object sender, RoutedEventArgs e)
        {
            LeerNotas();
        }
        #endregion
    }
}