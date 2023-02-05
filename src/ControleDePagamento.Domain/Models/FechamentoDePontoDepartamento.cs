using ControleDePagamento.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDePagamento.Domain.Models
{
    public class FechamentoDePontoDepartamento : IFechamentoDePontoDepartamento
    {
        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public int AnoVigencia { get; set; }
        public double TotalPagar { get; private set; }
        public double TotalDescontos { get; private set; }
        public double TotalExtras { get; private set; }
        public IList<FechamentoDePontoFuncionario> Funcionarios { get; set; }

        public FechamentoDePontoDepartamento() { }
        
        public FechamentoDePontoDepartamento(string departamento, string mesVigencia, int anoVigencia, double totalPagar, double totalDescontos, double totalExtras, IList<FechamentoDePontoFuncionario> funcionarios)
        {
            Departamento = departamento;
            MesVigencia = mesVigencia;
            AnoVigencia = anoVigencia;
            TotalPagar = totalPagar;
            TotalDescontos = totalDescontos;
            TotalExtras = totalExtras;
            Funcionarios = funcionarios;
        }

        public void ConsolidarDadosDepartamento(ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            var resultado = folhaPontoArquivo.GroupBy(x => new { x.AnoVigente, x.Departamento, x.MesVigente })
                         .Select(g => new FechamentoDePontoDepartamento
                         {
                             AnoVigencia = g.Key.AnoVigente,
                             MesVigencia = g.Key.MesVigente,
                             Departamento = g.Key.Departamento
                             
                         });
        }

        private void TotalHorasDepartamento()
        {

        }
    }
}
