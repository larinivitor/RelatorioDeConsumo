using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleDeGastos
{
	public class Gasto
	{
		public IList<Veiculo> ImportaDados(string caminhoArquivo)
		{
			List<string> linhas = System.IO.File.ReadAllLines(caminhoArquivo, Encoding.UTF8).ToList<string>();
			
			List<Veiculo> veiculoLst = new List<Veiculo>();
			int registrosDeVeiculosNaoImportados = 0;

			for (int linhaAtual = 1; linhaAtual < linhas.Count; linhaAtual++)
			{
				string[] coluna = linhas[linhaAtual].Split(',');

				if (coluna.Length != 6)
				{
					registrosDeVeiculosNaoImportados++;
					continue;
				}

				Veiculo veiculo = veiculoLst.FirstOrDefault(w => w.Marca == RemoveAspas(coluna[0]) && w.Modelo == RemoveAspas(coluna[1]));
				if (veiculo == null)
				{
					veiculo = new Veiculo();
					veiculo.Abastecimentos = new List<Abastecimento>();
					veiculo.Marca = RemoveAspas(coluna[0]);
					veiculo.Modelo = RemoveAspas(coluna[1]);
					veiculo.Abastecimentos.Add(LeDadosAbastecimento(coluna));
					veiculoLst.Add(veiculo);
				}
				else veiculo.Abastecimentos.Add(LeDadosAbastecimento(coluna));
			}
			return veiculoLst;
		}

		Abastecimento LeDadosAbastecimento(string[] coluna)
		{
			System.Globalization.NumberStyles numberStyle = System.Globalization.NumberStyles.AllowDecimalPoint;
			return new Abastecimento()
			{
				Combustivel = float.Parse(RemoveAspas(coluna[4]), numberStyle, System.Globalization.CultureInfo.InvariantCulture),
				Data = DateTime.Parse(RemoveAspas(coluna[2])),
				Preco = Decimal.Parse(RemoveAspas(coluna[5]), numberStyle, System.Globalization.CultureInfo.InvariantCulture),
				Quilometragem = float.Parse(RemoveAspas(coluna[3]), numberStyle, System.Globalization.CultureInfo.InvariantCulture),
			};
		}
		string RemoveAspas(string conteudo)
		{
			return conteudo.Replace("\"", "");
		}
	}
}
