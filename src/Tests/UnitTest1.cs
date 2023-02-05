using ControleDePagamento.Aplication.Interfaces;
using ControleDePagamento.Aplication.Services;

namespace Tests
{
    public class Tests
    {
        private IImportadorDeDados _importadorDeDados;

        [SetUp]
        public void Setup()
        {
            _importadorDeDados = new ImportadorDeDados();
        }

        [Test]
        public void Test1()
        {
            try
            {
                //Assert.Pass();
                var folhaPontoAquivos = _importadorDeDados.Importar(@"C:\ArquivosCSV");
                var existeDadosInvalidos = folhaPontoAquivos.Any(x => !x.DadosValidos);
            }
            catch (Exception e)
            {
                throw;
            }
            
            
        }
    }
}