using ControleDePagamento.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDePagamento.Domain.Interfaces
{
    public interface IFechamentoDePontoFuncionario
    {
        Task<IList<FechamentoDePontoDepartamento>> RealizaBalancoFuncionarioPorDepartamento(IList<FechamentoDePontoDepartamento> departamentos, ConcurrentBag<FolhaPontoArquivo> folhaPontoArquivo);
    }
}
