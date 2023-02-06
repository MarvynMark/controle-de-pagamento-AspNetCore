using ControleDePagamento.Domain.Interfaces;
using System.Collections.Concurrent;

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
        public IEnumerable<FechamentoDePontoFuncionario> Funcionarios { get; set; }

        public FechamentoDePontoDepartamento() { }
        
        public FechamentoDePontoDepartamento(string departamento, string mesVigencia, int anoVigencia, double totalPagar, double totalDescontos, double totalExtras, IEnumerable<FechamentoDePontoFuncionario> funcionarios)
        {
            Departamento = departamento;
            MesVigencia = mesVigencia;
            AnoVigencia = anoVigencia;
            TotalPagar = totalPagar;
            TotalDescontos = totalDescontos;
            TotalExtras = totalExtras;
            Funcionarios = funcionarios;
        }

        public async Task<IList<FechamentoDePontoDepartamento>> ConsolidaDepartamentosComFuncionarios(ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            var departamentos = folhaPontoArquivo.GroupBy(x => new { x.AnoVigente, x.Departamento, x.MesVigente })
                         .Select(g => new FechamentoDePontoDepartamento
                         {
                             AnoVigencia = g.Key.AnoVigente,
                             MesVigencia = g.Key.MesVigente,
                             Departamento = g.Key.Departamento
                         }).ToList();

            foreach (var dpto in departamentos)
            {
                dpto.Funcionarios =
                    folhaPontoArquivo
                    .Where(x => x.AnoVigente == dpto.AnoVigencia && x.MesVigente == dpto.MesVigencia && x.Departamento == dpto.Departamento)
                    .GroupBy(y => new { y.Codigo, y.Funcionario })
                    .Select(g => new FechamentoDePontoFuncionario
                    { 
                        Codigo = g.Key.Codigo,
                        Nome = g.Key.Funcionario
                    }).ToList();
            }

            return await Task.FromResult(departamentos);
        }

        public async Task<IList<FechamentoDePontoDepartamento>> RealizaBalancoPorDepartamento (IList<FechamentoDePontoDepartamento> departamentos, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            foreach (var dpto in departamentos)
            {
                dpto.TotalExtras = await CalcularTotalExtras(dpto, folhaPontoArquivo);
                dpto.TotalDescontos = await CalculaTotalDescontos(dpto, folhaPontoArquivo);
                dpto.TotalPagar = Math.Round(dpto.Funcionarios.Sum(x => x.TotalReceber), 1);   
            }
            return await Task.FromResult(departamentos);
        }

        private async Task<double> CalculaTotalDescontos(FechamentoDePontoDepartamento dpto, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            double totalDescontar = 0;
            foreach (var func in dpto.Funcionarios)
                totalDescontar += func.HorasDebito * folhaPontoArquivo.First(x => x.Codigo == func.Codigo).ValorHora;

            return await Task.FromResult(Math.Round(totalDescontar, 1));
        }

        private async Task<double> CalcularTotalExtras(FechamentoDePontoDepartamento dpto, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            double totalExtras = 0;
            foreach (var func in dpto.Funcionarios)
                totalExtras += func.HorasExtras * folhaPontoArquivo.First(x => x.Codigo == func.Codigo).ValorHora;

            return await Task.FromResult(Math.Round(totalExtras, 1));
        }
    }
}
