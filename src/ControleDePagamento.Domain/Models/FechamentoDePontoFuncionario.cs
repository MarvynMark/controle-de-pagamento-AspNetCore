
using ControleDePagamento.Domain.Interfaces;

namespace ControleDePagamento.Domain.Models
{
    public class FechamentoDePontoFuncionario : IFechamentoDePontoFuncionario
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public double TotalReceber { get; }
        public double HorasExtras { get; }
        public double HorasDebito { get; }
        public int DiasFalta { get; }
        public int DiasExtras { get; }
        public int DiasTrabalhados { get; }

        public FechamentoDePontoFuncionario()
        {

        }

        public double SomarHorasFuncionario()
        {
            return 0;
        }
    }
}
