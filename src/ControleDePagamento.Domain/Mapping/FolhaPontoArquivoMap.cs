using ControleDePagamento.Domain.Models;
using CsvHelper.Configuration;


namespace ControleDePagamento.Domain.Mapping
{
    public class FolhaPontoArquivoMap : ClassMap<FolhaPontoArquivo>
    {
        public FolhaPontoArquivoMap()
        {
            Map(m => m.Codigo).Name("Código");
            Map(m => m.Funcionario).Name("Nome");
            Map(m => m.ValorHora).Name("Valor hora");
            Map(m => m.Data).Name("Data");
            Map(m => m.HoraEntrada).Name("Entrada");
            Map(m => m.HoraSaida).Name("Saída");
            Map(m => m.PeriodoHoraAlmoco).Name("Almoço");
        }
    }
}
