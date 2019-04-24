using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloc_notas_wpf
{
    class datosRutas
    {
        /*
         * Creamos una clase que obtendrá la ruta en un string.
         * Creamos el getter y setter correspondiente.
         */
        public datosRutas(string ruta)
        {
            Ruta = ruta;
        }

        public string Ruta { get; set; }
    }
}
