using ControleDePagamento.Aplication.Interfaces;
using ControleDePagamento.Domain.Models;
using Newtonsoft.Json;

namespace ControleDePagamento.Aplication.Services
{
    public class ExportadorDeDadosServices : IExportadorDeDadosServices
    {
        public void ExportaJson(IList<FechamentoDePontoDepartamento> departamentos, string diretórioSave)
        {
            try
            {
                string json = JsonConvert.SerializeObject(departamentos);
                File.WriteAllText($@"{diretórioSave}\ordem-de-pagamento.json", json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
