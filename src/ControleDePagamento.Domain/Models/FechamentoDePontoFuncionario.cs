
using ControleDePagamento.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Globalization;

namespace ControleDePagamento.Domain.Models
{
    public class FechamentoDePontoFuncionario : IFechamentoDePontoFuncionario
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public double TotalReceber { get; private set; }
        public double HorasExtras { get; private set; }
        public double HorasDebito { get; private set; }
        public int DiasFalta { get; private set; }
        public int DiasExtras { get; private set; }
        public int DiasTrabalhados { get; private set; }

        public FechamentoDePontoFuncionario() { }
        public FechamentoDePontoFuncionario(string nome, int codigo, double totalReceber, double horasExtras, double horasDebito, int diasFalta, int diasExtras, int diasTrabalhados)
        {
            Nome = nome;
            Codigo = codigo;
            TotalReceber = totalReceber;
            HorasExtras = horasExtras;
            HorasDebito = horasDebito;
            DiasFalta = diasFalta;
            DiasExtras = diasExtras;
            DiasTrabalhados = diasTrabalhados;
        }

        public async Task<IList<FechamentoDePontoDepartamento>> RealizaBalancoFuncionarioPorDepartamento(IList<FechamentoDePontoDepartamento> departamentos, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            Parallel.ForEach(departamentos.OrderBy(x => DateTime.ParseExact(x.MesVigencia, "MMMM", new CultureInfo("pt-BR")).Month), async dpto =>
            {
                foreach (var func in dpto.Funcionarios.OrderBy(x => x.Codigo))
                {

                    Func<FolhaPontoArquivo, bool> filtroFuncMesAno = fp => fp.AnoVigente == dpto.AnoVigencia && fp.MesVigente == dpto.MesVigencia && fp.Codigo == func.Codigo && fp.Departamento == dpto.Departamento;

                    func.DiasExtras = await CalculaDiasExtras(dpto, func, folhaPontoArquivo);
                    func.DiasFalta = await CalculaDiasFalta(dpto, func, folhaPontoArquivo);
                    func.DiasTrabalhados = await CalculaDiasTrabalhados(dpto, func.DiasFalta);
                    func.HorasExtras = Math.Round(folhaPontoArquivo.Where(filtroFuncMesAno).Sum(x => x.HorasExtras), 2);
                    func.HorasDebito = await CalculaHorasDebito(filtroFuncMesAno, folhaPontoArquivo, func.DiasFalta);
                    func.TotalReceber = await CalculaTotalReceber(dpto, func, folhaPontoArquivo, func.DiasFalta);
                }
            });
                        
            return await Task.FromResult(departamentos);
        }

        private async Task<int> CalculaDiasExtras(FechamentoDePontoDepartamento dpto, FechamentoDePontoFuncionario func, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            return await Task.FromResult(folhaPontoArquivo.Where(x => x.AnoVigente == dpto.AnoVigencia && x.MesVigente == dpto.MesVigencia && x.Codigo == func.Codigo && x.Departamento == dpto.Departamento && x.DiaExtra).Count());
        }
        private async Task<double> CalculaHorasDebito(Func<FolhaPontoArquivo, bool> filtroFuncMesAno, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo, int diasFalta)
        {
            var horasDebito = folhaPontoArquivo.Where(filtroFuncMesAno).Sum(x => x.HorasDebito);
            var totalHorasDebitoPorFaltas = diasFalta * 8;

            return await Task.FromResult(Math.Round(horasDebito + totalHorasDebitoPorFaltas, 1));

        }
        private async Task<int> CalculaDiasFalta(FechamentoDePontoDepartamento dpto, FechamentoDePontoFuncionario func, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo)
        {
            int numeroMes = DateTime.ParseExact(dpto.MesVigencia, "MMMM", new CultureInfo("pt-BR")).Month;

            var diasUteisMes = Enumerable.Range(1, DateTime.DaysInMonth(dpto.AnoVigencia, numeroMes))
           .Where(day => (new DateTime(dpto.AnoVigencia, numeroMes, day).DayOfWeek != DayOfWeek.Saturday) && (new DateTime(dpto.AnoVigencia, numeroMes, day).DayOfWeek != DayOfWeek.Sunday)).ToList();

            var diasUteisTrabalhados = folhaPontoArquivo.Where(x => x.Codigo == func.Codigo && x.Departamento == dpto.Departamento && x.Data.Year == dpto.AnoVigencia && x.Data.Month == numeroMes && x.Data.DayOfWeek != DayOfWeek.Saturday && x.Data.DayOfWeek != DayOfWeek.Sunday).Select(d => d.Data.Day).ToList();

            return await Task.FromResult(diasUteisMes.Count() - diasUteisTrabalhados.Count());
        }

        private async Task<double> CalculaTotalReceber(FechamentoDePontoDepartamento dpto, FechamentoDePontoFuncionario func, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo, double diasFalta)
        {
            Func<FolhaPontoArquivo, bool> filtroFuncMesAno = fp => fp.AnoVigente == dpto.AnoVigencia && fp.MesVigente == dpto.MesVigencia && fp.Codigo == func.Codigo && fp.Departamento == dpto.Departamento;
            int numeroMes = DateTime.ParseExact(dpto.MesVigencia, "MMMM", new CultureInfo("pt-BR")).Month;

            var valorHora = folhaPontoArquivo.First(filtroFuncMesAno).ValorHora;
            var totalPorHorasTrabalhadas = folhaPontoArquivo
                .Where(filtroFuncMesAno)
                .Sum(x => x.HorasTrabalhadas);

            var diasFds = Enumerable.Range(1, DateTime.DaysInMonth(dpto.AnoVigencia, numeroMes))
           .Where(day => (new DateTime(dpto.AnoVigencia, numeroMes, day).DayOfWeek == DayOfWeek.Saturday) || (new DateTime(dpto.AnoVigencia, numeroMes, day).DayOfWeek == DayOfWeek.Sunday)).Count();

            var totalHorasFds = diasFds * 8;
            var totalHorasFalta = diasFalta * 8;
            var totalReceber = ((totalPorHorasTrabalhadas + totalHorasFds) - totalHorasFalta) * valorHora;

            return await Task.FromResult(Math.Round(totalReceber, 1));
        }

        private async Task<int> CalculaDiasTrabalhados(FechamentoDePontoDepartamento dpto, int qtdeDiasFalta)
        {
            int numeroMes = DateTime.ParseExact(dpto.MesVigencia, "MMMM", new CultureInfo("pt-BR")).Month;
            var qteDiasMes = Enumerable.Range(1, DateTime.DaysInMonth(dpto.AnoVigencia, numeroMes)).Count();
            return await Task.FromResult(qteDiasMes - qtdeDiasFalta);
        }
    }
}
