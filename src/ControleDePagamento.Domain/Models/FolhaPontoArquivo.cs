
namespace ControleDePagamento.Domain.Models
{
    public class FolhaPontoArquivo
    {
        public string NomeArquivo { get; set; }
        public int Codigo { get; set; }
        public string Funcionario { get; set; }
        public double ValorHora { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly HoraEntrada { get; set; }
        public TimeOnly HoraSaida { get; set; }
        public string PeriodoHoraAlmoco { get; set; }
        public TimeOnly HoraInicioAlmoco { get; set; }
        public TimeOnly HoraFimAlmoco { get; set; }
        public string Departamento { get; set; }
        public string MesVigente { get; set; }
        public int AnoVigente { get; set; }
        public double HorasExtras { get; set; }
        public double HorasDebito { get; set; }
        public bool DiaExtra { get; set; }
        public double HorasTrabalhadas { get; set; }
        public bool DadosValidos { get; set; } = true;

        public FolhaPontoArquivo()
        {

        }

        public FolhaPontoArquivo(string nomeArquivo, string codigo, string funcionario, string valorHora, string data, string horaEntrada, string horaSaida, string periodoHoraAlmoco)
        {
            NomeArquivo = nomeArquivo;
            Funcionario = funcionario;
            PeriodoHoraAlmoco = periodoHoraAlmoco;
            AjustaDados(codigo, valorHora, data, horaEntrada, horaSaida, nomeArquivo, periodoHoraAlmoco);
            CalculaHoras(HoraEntrada, HoraSaida, HoraInicioAlmoco, HoraFimAlmoco, Data);
        }

        private void CalculaHoras(TimeOnly horaEntrada, TimeOnly horaSaida, TimeOnly horaInicioAlmoco, TimeOnly horaFimAlmoco, DateOnly data)
        {
            var tempoDeAlmoco = horaFimAlmoco.ToTimeSpan().Subtract(horaInicioAlmoco.ToTimeSpan());
            var horasTrabalhadas = horaSaida.ToTimeSpan().Subtract(horaEntrada.ToTimeSpan()).Subtract(tempoDeAlmoco);
            DiaExtra = (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday);

            if (DiaExtra)
                HorasExtras = horasTrabalhadas.TotalHours;
            else if (horasTrabalhadas.TotalHours > 8)
                HorasExtras = horasTrabalhadas.Subtract(TimeSpan.FromHours(8)).TotalHours;
            else
                HorasDebito = Math.Abs(horasTrabalhadas.Subtract(TimeSpan.FromHours(8)).TotalHours);

            HorasTrabalhadas = horasTrabalhadas.TotalHours;
        }

        private void AjustaDados(string codigoStr, string valorHoraStr, string dataStr, string horaEntradaStr, string horaSaidaStr, string nomeArquivo, string periodoHoraAlmoco)
        {
            try
            {
                // Codigo
                int codigo;
                if (int.TryParse(codigoStr, out codigo))
                    Codigo = codigo;
                else
                    DadosValidos = false;

                // ValorEntrada
                double valorHora;
                if (double.TryParse(valorHoraStr.ToUpper().Replace("R$", "").Replace(" ", ""), out valorHora))
                    ValorHora = valorHora;
                else
                    DadosValidos = false;

                // Data
                DateOnly data;
                if (DateOnly.TryParse(dataStr, out data))
                    Data = data;
                else
                    DadosValidos = false;

                // HoraEntrada
                TimeOnly horaEntrada;
                if (TimeOnly.TryParse(horaEntradaStr, out horaEntrada))
                    HoraEntrada = horaEntrada;
                else
                    DadosValidos = false;
                
                // HoraSaida
                TimeOnly horaSaida;
                if (TimeOnly.TryParse(horaSaidaStr, out horaSaida))
                    HoraSaida = horaSaida;
                else
                    DadosValidos = false;

                // HoraInicioAlmoco
                var periodoAlmocoArray = periodoHoraAlmoco.Replace(" ", "").Split("-");
                if (periodoAlmocoArray.Length > 0)
                {
                    var horaStr = periodoAlmocoArray[0];
                    TimeOnly hora;
                    if (TimeOnly.TryParse(horaStr, out hora))
                        HoraInicioAlmoco = hora;
                    else
                        DadosValidos = false;    
                }
                else
                    DadosValidos = false;

                // HoraFimAlmoco
                if (periodoAlmocoArray.Length > 1)
                {
                    var horaStr = periodoAlmocoArray[1];
                    TimeOnly hora;
                    if (TimeOnly.TryParse(horaStr, out hora))
                        HoraFimAlmoco = hora;
                    else
                        DadosValidos = false;
                }
                else
                    DadosValidos = false;

                // Departamento
                var nomeArray = nomeArquivo.Split('-');
                if (nomeArray.Length > 0)
                    Departamento = nomeArray[0];
                else
                {
                    DadosValidos = false;
                    Departamento = string.Empty;
                }

                // Mes Vigente
                if (nomeArray.Length > 1)
                    MesVigente = nomeArray[1];
                else
                {
                    DadosValidos = false;
                    MesVigente = string.Empty;
                }

                // Ano Vigente
                if (nomeArray.Length > 2)
                {
                    var anoStr = nomeArray[2];
                    int ano;
                    if (int.TryParse(anoStr, out ano))
                        AnoVigente = ano;
                    else
                        DadosValidos = false;
                }
                else
                    DadosValidos = false;
            }
            catch (Exception)
            {
                DadosValidos = false;
            }
        }
    }
}
