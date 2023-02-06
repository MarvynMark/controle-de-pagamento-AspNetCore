using ControleDePagamento.Domain.Models;
using System.Collections.Concurrent;

namespace ControleDePagamento.Aplication.Interfaces
{
    public interface IImportadorDeDadosServices
    {
       Task<ConcurrentBag<FolhaPontoArquivo>> Importar(string folderPath);
    }
}
