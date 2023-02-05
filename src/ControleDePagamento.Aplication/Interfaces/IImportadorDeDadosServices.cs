using ControleDePagamento.Domain.Models;
using System.Collections.Concurrent;

namespace ControleDePagamento.Aplication.Interfaces
{
    public interface IImportadorDeDados
    {
        ConcurrentBag<FolhaPontoArquivo> Importar(string folderPath);
    }
}
