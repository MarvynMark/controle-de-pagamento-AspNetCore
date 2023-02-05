using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDePagamento.Infra.Data
{
    public class FolhaPontoArquivo
    {
        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public string AnoVigencia { get; set; }
        public string Codigo { get; set; }
        public string NomeFuncionario { get; set; }
        public string ValorHora { get; set; }
        public string Data { get; set; }
        public string Entrada { get; set; }
        public string Saida { get; set; }
        public string Almoco { get; set; }
    }
}
