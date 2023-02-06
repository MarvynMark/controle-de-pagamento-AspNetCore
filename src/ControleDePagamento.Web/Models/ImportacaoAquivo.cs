using ControleDePagamento.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ControleDePagamento.Web.Models
{
    public class ImportacaoAquivo
    {
        public string DiretorioAquivo { get; set; }
        
        public ImportacaoAquivo() { }

        public ImportacaoAquivo(string diretorioAquivo)
        {
            DiretorioAquivo = diretorioAquivo;
        }
    }
}
