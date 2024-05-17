using RH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Models;

namespace AvaliacaoServidorBlazor.Model
{
    internal class ModelView_Home
    {
        public string Nome { get; set; }
        public string idFuncional { get; set; }
        public List<PainelAvisos> ListaNoticiais { get; set; }
        public CicloAvaliativo  Ciclo { get; set; }
    }
}
