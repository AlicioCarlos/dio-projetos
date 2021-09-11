using dio_bank.Domain;
using dio_bank.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace dio_bank
{
    class Program
    {
		static List<Conta> listContas = new List<Conta>();
		static void Main(string[] args)
        {
			listContas = Balanco.CarregarBalanco();

			string opcaoUsuario = ObterOpcaoUsuario();

			while (opcaoUsuario.ToUpper() != "S")
			{
				switch (opcaoUsuario)
				{
					case "1":
						ListarContas();
						break;
					case "2":
						Transferir();
						break;
					case "3":
						Sacar();
						break;
					case "4":
						Depositar();
						break;
					case "5":
						NovaConta();
						break;
					case "6":
						CancelarConta();
						break;						
					case "C":
						Console.Clear();
						break;

					default:
						Console.Clear();
						break;
				}

				opcaoUsuario = ObterOpcaoUsuario();
			}

			Balanco.FecharBalanco(listContas);

			Console.WriteLine("Encerrando...");
			Console.ReadLine();
		}

		private static void Depositar()
		{
			Console.Write("Digite o número da conta: ");
			Int32.TryParse(Console.ReadLine(), out int nuConta);

			if (nuConta <= 0)
			{
				Console.Write("Conta não localizada!");

				return;
			}
						
			var conta = listContas.Where(c => c.NumeroConta.Equals(nuConta)).FirstOrDefault();

			if (conta == null)
			{
				Console.Write("Conta não localizada!");
				return;
			}

			Console.Write("Digite o valor a ser depositado: ");
			Double.TryParse(Console.ReadLine(), out double valorDeposito);

			if (valorDeposito <= 0)
			{
				Console.Write("O valor do depósito deve ser maior que 0.");
				return;
			}

			conta.Depositar(valorDeposito);
			Console.Write("Depósito efetuado com sucesso!");
		}

		private static void Sacar()
		{
			Console.Write("Digite o número da conta: ");
			Int32.TryParse(Console.ReadLine(), out int nuConta);

			if (nuConta <= 0)
			{
				Console.Write("Conta não localizada!");
				return;
			}

			var conta = listContas.Where(c => c.NumeroConta.Equals(nuConta)).FirstOrDefault();

			if (conta == null)
			{
				Console.Write("Conta não localizada!");
				return;
			}

			Console.Write("Digite o valor a ser sacado: ");
			Double.TryParse(Console.ReadLine(), out double valorSaque);

			if (valorSaque <= 0)
			{
				Console.Write("O valor do saque deve ser maior que 0.");
				return;
			}

			conta.Sacar(valorSaque);
			Console.Write("Saque efetuado com sucesso!");

		}

		private static void Transferir()
		{
			Console.Write("Digite o número da conta de origem: ");
			Int32.TryParse(Console.ReadLine(), out int nuContaOrigem);

			if (nuContaOrigem <= 0)
			{
				Console.Write("Conta não localizada!");
				return;
			}

			var contaOrigem = listContas.Where(c => c.NumeroConta.Equals(nuContaOrigem)).FirstOrDefault();

			if (contaOrigem == null)
			{
				Console.Write("Conta Origem não localizada!");
				return;
			}

			Console.Write("Digite o número da conta de destino: ");
			Int32.TryParse(Console.ReadLine(), out int nuContaDestino);

			if (nuContaDestino <= 0)
			{
				Console.Write("Conta não localizada!");
				return;
			}
			
			var contaDestino = listContas.Where(c => c.NumeroConta.Equals(nuContaDestino)).FirstOrDefault();				

			if (contaDestino == null)
			{
				Console.Write("Conta Destino não localizada!");
				return;
			}

			Console.Write("Digite o valor a ser transferido: ");
			Double.TryParse(Console.ReadLine(), out double valorTransferencia);

			if (valorTransferencia <= 0)
			{
				Console.Write("O valor da transferência deve ser maior que 0.");
				return;
			}

			contaOrigem.Transferir(valorTransferencia: valorTransferencia, contaDestino);
			Console.Write("Trensferência realizada com sucesso!");
		}

		private static void NovaConta()
		{
			Console.WriteLine("Nova Conta");

			Console.Write("Digite 1 para Conta Poupança, 2 para Conta Corrente ou 3 para Conta de Investimento: ");
			Int32.TryParse(Console.ReadLine(), out int tipoConta);

			if (tipoConta < 1 || tipoConta > 3)
			{
				Console.Write("Tipo de Conta inválido não localizada!");
				return;
			}

			Console.Write("Nome do Cliente: ");
			string nome = Console.ReadLine();

			Console.Write("Documento: ");
			string documento = Console.ReadLine();

			Console.Write("Saldo Inicial: ");
			Double.TryParse(Console.ReadLine(), out double saldo);

			if (saldo <= 0)
			{
				Console.Write("Saldo inicial inválido!");
				return;
			}

			Conta novaConta = new Conta(tipoConta: tipoConta == 1 ? TiposUtils.Conta.POUPANÇA : tipoConta == 2 ? TiposUtils.Conta.CORRENTE : TiposUtils.Conta.INVESTIMENTO,
										nome: nome,
										Documento: documento,
										saldo: saldo);

			listContas.Add(novaConta);

			Console.Write("Nova conta criada com sucesso!\n");
			Console.WriteLine($"Tipo: {novaConta.TipoConta} | Número {novaConta.NumeroConta} | Titular {novaConta.Nome} | Documento {novaConta.Documento} | Saldo: {novaConta.Saldo} | Crédito: {novaConta.Credito}");

		}

		private static void CancelarConta()
		{
			Console.Write("Informe o nímero da conta a ser cancelada: ");

			Int32.TryParse(Console.ReadLine(), out int nuConta);

			var conta = listContas.Where(c => c.NumeroConta.Equals(nuConta)).FirstOrDefault();

			if (conta == null)
			{
				Console.Write("Conta não localizada!");
				return;
			}

			if (conta.VerificarSaldo() <= 0)
			{
				listContas.Remove(conta);

				Console.Write("Conta cancelada com sucesso!");
			}
			else
			{
				Console.Write("Conta informada possui saldo. Para cancelar realize o saque de todo o saldo da conta!");
			}		

		}

		private static void ListarContas()
		{
			Console.WriteLine("Lista de Contas");

			var count = 0;

			if (listContas.Count == 0)
			{
				Console.WriteLine("Nenhuma conta cadastrada.");
				return;
			}

            foreach (var c in listContas)
            {
				Console.WriteLine($"#{++count} - Tipo: {c.TipoConta} | Número {c.NumeroConta} | Titular {c.Nome} | Documento {c.Documento} | Saldo: {c.Saldo} | Crédito: {c.Credito}");
			}

		}

		private static string ObterOpcaoUsuario()
		{
			Console.WriteLine();
			Console.WriteLine("\n*** Desafio de Projeto Banco DIO ***\n");
			Console.WriteLine("Digite uma opção");
			Console.WriteLine("1 - Consultar Contas");
			Console.WriteLine("2 - Transferir");
			Console.WriteLine("3 - Sacar");
			Console.WriteLine("4 - Depositar");
			Console.WriteLine("5 - Nova Conta");
			Console.WriteLine("6 - Cancelar Conta");
			Console.WriteLine("M - Menu Principal");
			Console.WriteLine("S - Sair");
			Console.WriteLine();

			string opcaoUsuario = Console.ReadLine().ToUpper();
			Console.WriteLine();
			return opcaoUsuario;
		}
	}
}
