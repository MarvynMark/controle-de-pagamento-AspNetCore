using ControleDePagamento.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ControleDePagamento.Aplication.Interfaces;
using ControleDePagamento.Aplication.Services;
using ControleDePagamento.Domain.Models;
using ControleDePagamento.Domain.Interfaces;

namespace ControleDePagamento.Web.Controllers
{
    public class ImportacaoArquivosController : Controller
    {
        private readonly IImportadorDeDadosServices _importadorDeDados;
        private readonly IFechamentoDePontoDepartamento _fechamentoDepartamento;
        private readonly IFechamentoDePontoFuncionario _fechamentoFuncionario;
        private readonly IExportadorDeDadosServices _exportadorDeDadosServices;

        public ImportacaoArquivosController(IImportadorDeDadosServices importadorDeDados, IFechamentoDePontoDepartamento fechamentoDepartamento, IFechamentoDePontoFuncionario fechamentoFuncionario, IExportadorDeDadosServices exportadorDeDadosServices)
        {
            _importadorDeDados = importadorDeDados;
            _fechamentoDepartamento = fechamentoDepartamento;
            _fechamentoFuncionario = fechamentoFuncionario;
            _exportadorDeDadosServices = exportadorDeDadosServices;
        }

        public ActionResult Index(ImportacaoArquivoViewModel importacaoAquivoViewModel)
        {
            if (string.IsNullOrEmpty(importacaoAquivoViewModel.MsgErro))
                importacaoAquivoViewModel.DiretorioValidado = true;

            return View(importacaoAquivoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Importar(ImportacaoArquivoViewModel importacaoArquivoViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(importacaoArquivoViewModel.ImportacaoAquivo.DiretorioAquivo) && Directory.Exists(importacaoArquivoViewModel.ImportacaoAquivo.DiretorioAquivo))
                {
                    importacaoArquivoViewModel.DiretorioValidado = true;
                    importacaoArquivoViewModel.MsgErro = string.Empty;

                    var inicioImportacao = DateTime.Now;
                    var folhaPontoArquivos = await _importadorDeDados.Importar(importacaoArquivoViewModel.ImportacaoAquivo.DiretorioAquivo);
                    var tempoImportacaoArquivo = DateTime.Now.Subtract(inicioImportacao);

                    var inicioAgrupandoDepartamento = DateTime.Now;
                    var departamentos = await _fechamentoDepartamento.ConsolidaDepartamentosComFuncionarios(folhaPontoArquivos);
                    var tempoAgrupandoDepartamentos = DateTime.Now.Subtract(inicioAgrupandoDepartamento);

                    var inicioFechamentoFunc = DateTime.Now;
                    var funcionarios = await _fechamentoFuncionario.RealizaBalancoFuncionarioPorDepartamento(departamentos, folhaPontoArquivos);
                    var tempoRealizandoFechamentoFuncionarios = DateTime.Now.Subtract(inicioFechamentoFunc);

                    var inicioFechamentoDepartmento = DateTime.Now;
                    var fechamentoGeral = await _fechamentoDepartamento.RealizaBalancoPorDepartamento(departamentos, folhaPontoArquivos);
                    var tempoRealizandoFechamentoDepartamentos = DateTime.Now.Subtract(inicioFechamentoDepartmento);

                    _exportadorDeDadosServices.ExportaJson(fechamentoGeral, importacaoArquivoViewModel.ImportacaoAquivo.DiretorioAquivo);

                    importacaoArquivoViewModel.MsgSucesso = new List<string>
                    {
                        "Processo de importação de arquivos realizado com sucesso!",
                        $"Tempo gasto na importação: {tempoImportacaoArquivo.ToString(@"hh\:mm\:ss\.fff")}",
                        $"Tempo gasto agrupando departamentos: {tempoAgrupandoDepartamentos.ToString(@"hh\:mm\:ss\.fff")}",
                        $"Tempo gasto realizando fechamento dos funcionários: {tempoRealizandoFechamentoFuncionarios.ToString(@"hh\:mm\:ss\.fff")}",
                        $"Tempo gasto realizando fechamento dos departamentos: {tempoRealizandoFechamentoDepartamentos.ToString(@"hh\:mm\:ss\.fff")}",
                        $"Tempo total gasto: {(tempoImportacaoArquivo + tempoAgrupandoDepartamentos + tempoRealizandoFechamentoFuncionarios + tempoRealizandoFechamentoDepartamentos).ToString(@"hh\:mm\:ss\.fff")}",
                        "Total de dados importados: " + string.Format("{0:n0}", folhaPontoArquivos.Count()),
                        $"Total de dados não importados: {folhaPontoArquivos.Count(x => !x.DadosValidos)}" 
                    };

                    importacaoArquivoViewModel.ProcessoConcluido = true;
                }
                else
                {
                    importacaoArquivoViewModel.MsgSucesso = new List<string>();
                    importacaoArquivoViewModel.ProcessoConcluido = false;
                    importacaoArquivoViewModel.DiretorioValidado = false;
                    importacaoArquivoViewModel.MsgErro = "Diretório informado não encontrado!";
                }

                return RedirectToAction(nameof(Index), importacaoArquivoViewModel);
            }
            catch (Exception e)
            {
                importacaoArquivoViewModel.MsgSucesso = new List<string>();
                importacaoArquivoViewModel.ProcessoConcluido = false;
                importacaoArquivoViewModel.DiretorioValidado = false;
                importacaoArquivoViewModel.MsgErro = $"Falha no processo de importação. Erro: {e.Message}" ;
                return RedirectToAction(nameof(Index), importacaoArquivoViewModel);
            }
        }
    }
}
