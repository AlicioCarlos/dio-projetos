using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;

namespace dio_bank.Domain
{
    public class Balanco
    {
        private static readonly string PATH_BALANCO = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\res\balanco.txt"));
        
        public static List<Conta> CarregarBalanco()
        {
            List<Conta> listContas = new List<Conta>();

            var linhas = File.ReadAllLines(PATH_BALANCO, Encoding.Default);

            foreach (var cliente in linhas)
            {
                var dado = cliente.Split("|");

                var conta = new Conta(tipoConta: dado[0].Trim(), 
                                      NumeroConta: Convert.ToInt32(dado[1].Trim()), 
                                      nome: dado[2].Trim(), 
                                      Documento: dado[3].Trim(), 
                                      saldo: Convert.ToDouble(dado[4].Trim()), 
                                      Credito: Convert.ToDouble(dado[5].Trim())
                                     );

                listContas.Add(conta);
            }

            return listContas;
        }

        public static void FecharBalanco(List<Conta> listContas)
        {
            StringBuilder output = new StringBuilder();

            foreach (var item in listContas)
            {
                output.AppendLine(string.Concat(item.TipoConta, " | ", 
                                            item.NumeroConta, " | ", 
                                            item.Nome, " | ",
                                            item.Documento, " | ",
                                            item.Saldo, " | ",
                                            item.Credito, " | "));
            }

            System.IO.File.WriteAllText(PATH_BALANCO, output.ToString(), Encoding.Default);

            File.ReadAllLines(PATH_BALANCO, Encoding.Default);
        }

    }
}
