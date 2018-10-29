using System;
namespace ControleDeGastos
{
	public class Abastecimento
	{
		/// <summary>
		/// Quantidade de litros
		/// </summary>
		public float Combustivel { get; set; }
		/// <summary>
		/// Datao
		/// </summary>
		public DateTime Data { get; set; }
		/// <summary>
		/// Valor total
		/// </summary>
		public decimal Preco { get; set; }
		/// <summary>
		/// Quilometragem 
		/// </summary>
		public float Quilometragem { get; set; }
	}
}
