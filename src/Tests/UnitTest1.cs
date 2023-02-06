using ControleDePagamento.Aplication.Interfaces;
using ControleDePagamento.Aplication.Services;
using ControleDePagamento.Domain.Models;
using ControleDePagamento.Domain.Interfaces ;

namespace Tests
{
    public class Tests
    {
        private IImportadorDeDadosServices _importadorDeDadosServices;
        private IFechamentoDePontoDepartamento _fechamentoDepartamento;
        private IFechamentoDePontoFuncionario _fechamentoFuncionario;
        private IExportadorDeDadosServices _exportadorDeDadosServices;

        [SetUp]
        public void Setup()
        {
            _importadorDeDadosServices = new ImportadorDeDadosServices();
            _fechamentoDepartamento = new FechamentoDePontoDepartamento();
            _fechamentoFuncionario = new FechamentoDePontoFuncionario();
            _exportadorDeDadosServices = new ExportadorDeDadosServices();
        }

        [Test]
        public async Task Test1()
        {
            try
            {
                //Assert.Pass();
                var diretorio = @"C:\ArquivosCSV";
                var folhaPontoArquivos = await _importadorDeDadosServices.Importar(diretorio);
                var existeDadosInvalidos = folhaPontoArquivos.Any(x => !x.DadosValidos);
                var departamentos = await _fechamentoDepartamento.ConsolidaDepartamentosComFuncionarios(folhaPontoArquivos);
                var funcionarios = await _fechamentoFuncionario.RealizaBalancoFuncionarioPorDepartamento(departamentos, folhaPontoArquivos);
                var fechamentoGeral = await _fechamentoDepartamento.RealizaBalancoPorDepartamento(departamentos, folhaPontoArquivos);
                _exportadorDeDadosServices.ExportaJson(fechamentoGeral, diretorio);
                
            }
            catch (Exception e)
            {
                
                throw;
            }
        }
    }
}

