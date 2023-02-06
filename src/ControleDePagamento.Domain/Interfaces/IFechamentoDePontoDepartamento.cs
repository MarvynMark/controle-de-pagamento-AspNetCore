using ControleDePagamento.Domain.Models;
using System.Collections.Concurrent;

namespace ControleDePagamento.Domain.Interfaces
{
    public interface IFechamentoDePontoDepartamento
    {
        Task<IList<FechamentoDePontoDepartamento>> ConsolidaDepartamentosComFuncionarios(ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo);
        Task<IList<FechamentoDePontoDepartamento>> RealizaBalancoPorDepartamento(IList<FechamentoDePontoDepartamento> departamentos, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo);
    }
}
