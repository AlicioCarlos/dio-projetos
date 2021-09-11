using dio_bank.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace dio_bank.Domain
{
    public class Conta
    {
		public string TipoConta { get; private set; }
		public int NumeroConta { get; private set; }
		public string Nome { get; private set; }
		public string Documento { get; private set; }
		public double Saldo { get; private set; }
		public double Credito { get; private set; }

        public Conta(string tipoConta, string nome, string Documento, double saldo, int NumeroConta = 0, double Credito = 0)
		{
			this.TipoConta = tipoConta;
			this.NumeroConta = NumeroConta !=  0 ? NumeroConta : Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(3, 7));
			this.Documento = Documento;
			this.Nome = nome;
			this.Saldo = saldo;
			if (Credito == 0)
			{
				this.Credito = tipoConta.Equals(TiposUtils.Conta.INVESTIMENTO) ? 10000
								: tipoConta.Equals(TiposUtils.Conta.CORRENTE) ? 1000 : 0;
			}
			else
			{
				this.Credito = Credito;
			}
		}

		public bool Sacar(double valorSaque)
		{

			if (this.Saldo - valorSaque < (this.Credito * -1))
			{
				Console.WriteLine("Saldo insuficiente!");
				return false;
			}
			this.Saldo -= valorSaque;

			Console.WriteLine($"Saldo atual da conta {this.NumeroConta} Titular {this.Nome} é {this.Saldo}");

			return true;
		}

		public void Depositar(double valorDeposito)
		{
			this.Saldo += valorDeposito;

			Console.WriteLine($"Saldo atual da conta {this.NumeroConta} Titular {this.Nome} é {this.Saldo}");
		}

		public void Transferir(double valorTransferencia, Conta contaDestino)
		{
			if (this.Sacar(valorTransferencia))
			{
				contaDestino.Depositar(valorTransferencia);
			}
		}

		public void AtualizarCredito(double novoValor)
		{
			this.Credito = novoValor;
		}
		public double VerificarSaldo()
		{
			return this.Saldo;
		}

		public string Extrato()
		{
			string retorno = "";
			retorno += "Conta  " + this.NumeroConta + " | ";
			retorno += "TipoConta " + this.TipoConta + " | ";
			retorno += "Titular " + this.Nome + " | ";
			retorno += "Documento " + this.Documento + " | ";
			retorno += "Saldo " + this.Saldo + " | ";
			retorno += "Crédito " + this.Credito;
			return retorno;
		}
	}
}
