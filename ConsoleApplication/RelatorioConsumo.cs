using System;

namespace ControleDeGastos
{
	public class Consumo
	{
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public float KM { get; set; }
		public Decimal ValorGasto { get; set; }
		public float Litros { get; set; }
		public DateTime DataInicial { get; set; }
		public int Dias { get; set; }
		public float MediaKmL { get; set; }
		public float PiorKmL { get; set; }
		public float MelhorKmL { get; set; }
		public float ValorGastoKmL { get; set; }
	}
}
