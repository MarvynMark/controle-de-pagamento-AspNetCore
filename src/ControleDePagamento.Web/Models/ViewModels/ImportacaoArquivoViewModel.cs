namespace ControleDePagamento.Web.Models.ViewModels
{
    public class ImportacaoArquivoViewModel
    {
        public ImportacaoAquivo ImportacaoAquivo { get; set; }
        public bool DiretorioValidado { get; set; }
        public bool ProcessoConcluido { get; set; }
        public string MsgErro { get; set; }
        public IList<string> MsgSucesso { get; set; }



        public ImportacaoArquivoViewModel()
        {

        }
    }
}
