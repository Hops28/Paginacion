using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paginacion
{
    internal class Proceso
    {
        public Proceso(string name, int size)
        {
            this.name = name;
            this.size = size;
        }

        public string name { get; set; }
        public int size { get; set; }
    }
}
