using ControleDePagamento.Domain.Models;

namespace ControleDePagamento.Aplication.Interfaces
{
    public interface IExportadorDeDadosServices
    {
        void ExportaJson(IList<FechamentoDePontoDepartamento> departamentos, string diretorioSave);
    }
}
