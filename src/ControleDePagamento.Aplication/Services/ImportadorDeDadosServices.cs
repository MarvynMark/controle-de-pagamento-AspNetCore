using ControleDePagamento.Aplication.Interfaces;
using ControleDePagamento.Domain.Mapping;
using ControleDePagamento.Domain.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// processo de importação dos arquivos
namespace ControleDePagamento.Aplication.Services
{
    public class ImportadorDeDados : IImportadorDeDados
    {
        public ConcurrentBag<FolhaPontoArquivo> Importar(string folderPath)
        {
            try
            {
                var files = Directory.GetFiles(folderPath, "*.csv");
                var folhaPontoArquivos = new ConcurrentBag<FolhaPontoArquivo>();

                foreach (var file in files)
                //Parallel.ForEach(files, file =>
                {
                    var fileName = new FileInfo(file).Name.Replace(".csv", "");
                    var lines = File.ReadAllLines(file, Encoding.GetEncoding("iso-8859-1"));
                    var delimiter = lines.First().Contains(";") ? ";" : ",";
                    var columns = lines.First().Split(delimiter);
                    var colCodigo = Array.IndexOf(columns, "Código");
                    var colNome = Array.IndexOf(columns, "Nome");
                    var colValorHora = Array.IndexOf(columns, "Valor hora");
                    var colData = Array.IndexOf(columns, "Data");
                    var colEntrada = Array.IndexOf(columns, "Entrada");
                    var colSaida = Array.IndexOf(columns, "Saída");
                    var colAlmoco = Array.IndexOf(columns, "Almoço");

                    foreach (var line in lines.Skip(1))
                    {
                        var values = line.Split(delimiter);
                        folhaPontoArquivos.Add(new FolhaPontoArquivo
                        (
                           fileName,
                           values[colCodigo],
                           values[colNome],
                           values[colValorHora],
                           values[colData],
                           values[colEntrada],
                           values[colSaida],
                           values[colAlmoco]
                        ));
                    }
                };
                return folhaPontoArquivos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

