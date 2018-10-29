using System;
using System.Collections.Generic;

namespace ControleDeGastos
{
	class Program
	{
		static void Main(string[] args)
		{
			string caminhoArquivo =  @"../../../LogCombustivel.csv";

			if (!string.IsNullOrEmpty(caminhoArquivo))
			{
				Controle control = new Controle();
				control.Carregar(caminhoArquivo);
			}
			else
			{
				Console.WriteLine("Caminho para o arquivo não foi informado.");
			}
			Console.Read();
		}

	}
}


