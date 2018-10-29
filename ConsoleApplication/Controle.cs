using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleDeGastos
{
	public class Controle
	{
		public void Carregar(string caminhoArquivo)
		{
			Gasto controleDeGasto = new Gasto();
			IList<Veiculo> listaDeVeiculos = controleDeGasto.ImportaDados(caminhoArquivo);

			List<Consumo> listaDeConsumo = new List<Consumo>();
			foreach (Veiculo veiculo in listaDeVeiculos)
			{

				Consumo consumo = new Consumo()
				{
					Marca = veiculo.Marca,
					Modelo = veiculo.Modelo,
					DataInicial = primeiroAbastecimento(veiculo.Abastecimentos),
					Dias = intervaloDias(veiculo.Abastecimentos),
					KM = kmsPercorridosTotais(veiculo.Abastecimentos),
					Litros = quantidadeLitrosAbastecidos(veiculo.Abastecimentos),
					ValorGasto = valorGasto(veiculo.Abastecimentos),
					MelhorKmL = melhorKm(veiculo.Abastecimentos),
					PiorKmL = piorKm(veiculo.Abastecimentos),
				};
				consumo.MediaKmL = consumo.KM / consumo.Litros;
				consumo.ValorGastoKmL = float.Parse(consumo.ValorGasto.ToString()) / consumo.KM;
				listaDeConsumo.Add(consumo);
			}

			GerarRelatorio(listaDeConsumo);
		}

		void GerarRelatorio(IList<Consumo> listaDeConsumo)
		{
			try
			{
				StringBuilder relatorioConsumo = new StringBuilder();
				
				relatorioConsumo.AppendLine("\"MARCA\",\"MODELO\",\"KM\",\"R$\",\"LITROS\",\"DATAINI\",\"DIAS\",\"MEDIAKM/L\",\"PIORKM/L\",\"MELHORKM/L\",\"R$/KM\"");

				foreach (Consumo consumo in listaDeConsumo)
				{
					StringBuilder relatorio = new StringBuilder();
					relatorio.AppendFormat("\"{0}\",", consumo.Marca);
					relatorio.AppendFormat("\"{0}\",", consumo.Modelo);
					relatorio.AppendFormat("\"{0}\",", consumo.KM);
					relatorio.AppendFormat("\"{0}\",", consumo.ValorGasto);
					relatorio.AppendFormat("\"{0}\",", consumo.Litros);
					relatorio.AppendFormat("\"{0}\",", consumo.DataInicial.ToString("yyyy-MM-dd"));
					relatorio.AppendFormat("\"{0}\",", consumo.Dias);
					relatorio.AppendFormat("\"{0}\",", consumo.MediaKmL);
					relatorio.AppendFormat("\"{0}\",", consumo.PiorKmL);
					relatorio.AppendFormat("\"{0}\",", consumo.MelhorKmL);
					relatorio.AppendFormat("\"{0}\"", consumo.ValorGastoKmL);
					relatorioConsumo.AppendLine(relatorio.ToString());
				}
				string folder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
				string filePath = string.Format(@"../../../RelatorioConsumo.csv", folder);
				System.IO.File.WriteAllText(filePath, relatorioConsumo.ToString());
				Console.WriteLine("Relatorio criado com sucesso.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		DateTime primeiroAbastecimento(IList<Abastecimento> abastecimentos)
		{
			return abastecimentos.OrderBy(w => w.Data).FirstOrDefault().Data;
		}
		int intervaloDias(IList<Abastecimento> abastecimentos)
		{
			DateTime dataInicial = primeiroAbastecimento(abastecimentos);
			
			int totalDays = 0;
			foreach (Abastecimento item in abastecimentos)
			{
				if (item.Data > dataInicial)
					totalDays = (item.Data - dataInicial).Days;
			}
			return totalDays;
		}
		float kmsPercorridosTotais(IList<Abastecimento> abastecimentos)
		{
			float kmInicial = abastecimentos.OrderBy(w => w.Quilometragem).FirstOrDefault().Quilometragem;
			float kmFinal = abastecimentos.OrderByDescending(w => w.Quilometragem).FirstOrDefault().Quilometragem;
			return kmFinal - kmInicial;
		}
		float quantidadeLitrosAbastecidos(IList<Abastecimento> abastecimentos)
		{
			float litros = 0;
			foreach (Abastecimento item in abastecimentos)
				litros += item.Combustivel;

			return litros;
		}
		float melhorKm(IList<Abastecimento> abastecimentos)
		{
			float valorAtual = 0;
			float melhorKmL = 0;

			List<Abastecimento> listaDeAbastecimentos = abastecimentos.OrderBy(w => w.Data).ToList<Abastecimento>();
			
			for (int index = 1; index < listaDeAbastecimentos.Count; index++)
			{
				float kmPercorrido = listaDeAbastecimentos[index].Quilometragem - listaDeAbastecimentos[index - 1].Quilometragem;
				valorAtual = kmPercorrido / listaDeAbastecimentos[index - 1].Combustivel;

				if ((melhorKmL == 0) || (valorAtual > melhorKmL))
					melhorKmL = valorAtual;
			}
			return melhorKmL;
		}
		float piorKm(IList<Abastecimento> abastecimentos)
		{
			float valorAtual = 0;
			float piorKmL = 0;
			
			List<Abastecimento> listaDeAbastecimentos = abastecimentos.OrderBy(w => w.Data).ToList<Abastecimento>();

			for (int index = 1; index < listaDeAbastecimentos.Count; index++)
			{
				float kmPercorrido = listaDeAbastecimentos[index].Quilometragem - listaDeAbastecimentos[index - 1].Quilometragem;
				valorAtual = kmPercorrido / listaDeAbastecimentos[index - 1].Combustivel;

				if ((piorKmL == 0) || (valorAtual < piorKmL))
					piorKmL = valorAtual;
			}
			return piorKmL;
		}
		Decimal valorGasto(IList<Abastecimento> abastecimentos)
		{
			decimal valor = 0;
			foreach (Abastecimento item in abastecimentos)
				valor += (item.Preco * Convert.ToDecimal(item.Combustivel));

			return valor;
		}
	}
}
