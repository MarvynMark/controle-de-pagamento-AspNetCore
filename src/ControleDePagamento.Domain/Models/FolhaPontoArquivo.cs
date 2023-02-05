using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool DadosValidos { get; set; } = true;

        public FolhaPontoArquivo()
        {

        }

        public FolhaPontoArquivo(string nomeArquivo, string codigo, string funcionario, string valorHora, string data, string horaEntrada, string horaSaida, string periodoHoraAlmoco)
        {
            NomeArquivo = nomeArquivo;
            Funcionario = funcionario;
            PeriodoHoraAlmoco = periodoHoraAlmoco;
            //Codigo = GetCodigo(codigo);
            //ValorHora = GetValorHora(valorHora);
            //Data = GetData(data);
            //HoraEntrada = GetHora(horaEntrada);
            //HoraSaida = GetHora(horaSaida);
            //PeriodoHoraAlmoco = periodoHoraAlmoco;
            //HoraInicioAlmoco = GetHoraAlmoco(periodoHoraAlmoco, 0);
            //HoraFimAlmoco = GetHoraAlmoco(periodoHoraAlmoco, 1);
            //Departamento = GetNomeDepartamento(nomeArquivo);
            //MesVigente = GetMesVigente(nomeArquivo);
            //AnoVigente = GetAnoVigente(nomeArquivo);

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
                var periodoAlmocoArray = periodoHoraAlmoco.Split(" - ");
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

        private int GetCodigo(string codigoStr)
        {
            try
            {
                int codigo;
                if (int.TryParse(codigoStr, out codigo))
                    return codigo;
                else
                {
                    DadosValidos = false;
                    return 0;
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return 0;
            }
        }
        private double GetValorHora(string valorHoraStr)
        {
            try
            {
                double valorHora;
                if (double.TryParse(valorHoraStr.ToUpper().Replace("R$", "").Replace(" ", ""), out valorHora))
                    return valorHora;
                else
                {
                    DadosValidos = false;
                    return 0;
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return 0;
            }
        }
        private DateOnly GetData(string dataStr) 
        {
            try
            {
                DateOnly data;
                if (DateOnly.TryParse(dataStr, out data))
                    return data;
                else
                {
                    DadosValidos = false;
                    return new DateOnly();
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return new DateOnly();
            }
        }
        private TimeOnly GetHora(string horaStr) 
        {
            try
            {
                TimeOnly hora;
                if (TimeOnly.TryParse(horaStr, out hora))
                    return hora;
                else
                {
                    DadosValidos = false;
                    return new TimeOnly();
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return new TimeOnly();
            }
        }

        private string GetNomeDepartamento(string nomeArquivo)
        {
            try
            {
                var nomeArray = nomeArquivo.Split('-');
                if (nomeArray.Length > 0)
                    return nomeArquivo.Split('-')[0];
                else
                {
                    DadosValidos = false;
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return string.Empty;
            }
            
        }
        private string GetMesVigente(string nomeArquivo)
        {
            try
            {
                var nomeArray = nomeArquivo.Split('-');
                if (nomeArray.Length > 1)
                    return nomeArray[1];
                else
                {
                    DadosValidos = false;
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return string.Empty;
            }
            
        }
        private int GetAnoVigente(string nomeArquivo)
        {
            try
            {
                var nomeArray = nomeArquivo.Split('-');
                if (nomeArray.Length > 2)
                {
                    var anoStr = nomeArray[2];
                    int ano;
                    if (int.TryParse(anoStr, out ano))
                        return ano;
                    else
                    {
                        DadosValidos = false;
                        return 0;
                    }
                }
                else
                {
                    DadosValidos = false;
                    return 0;
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return 0;
            }
            
        }
        private TimeOnly GetHoraAlmoco(string periodoHoraAlmoco, int indice)
        {
            try
            {
                var periodoAlmocoArray = periodoHoraAlmoco.Split(" - ");
                if (periodoAlmocoArray.Length > indice)
                {
                    var horaStr = periodoHoraAlmoco.Split(" - ")[indice];
                    TimeOnly hora;
                    if (TimeOnly.TryParse(horaStr, out hora))
                        return hora;
                    else
                    {
                        DadosValidos = false;
                        return new TimeOnly();
                    }
                }
                else
                {
                    DadosValidos = false;
                    return new TimeOnly();
                }
            }
            catch (Exception)
            {
                DadosValidos = false;
                return new TimeOnly();
            }
        }
    }
}
