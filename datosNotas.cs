using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bloc_notas_wpf
{
    public class datosNotas
    {
        /*
         * Creamos una clase que obtendrá los datos de las notas (Titulo, contenido, fecha, id) .
         * Creamos los getters y setters para cada uno.     
         */
        public datosNotas(string titulo, string contenido, string fecha, string id)
        {
            Titulo = titulo;
            Contenido = contenido;
            Fecha = fecha;
            Id = id;
        }

        public string Titulo { get; set; }

        public string Contenido { get; set; }

        public string Fecha { get; set; }

        public string Id { get; set; }

    }


}
