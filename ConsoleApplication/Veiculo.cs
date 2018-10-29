using System.Collections.Generic;
namespace ControleDeGastos
{
	public class Veiculo
	{
		/// <summary>
		/// Marca
		/// </summary>
		public string Marca { get; set; }
		/// <summary>
		/// Modelo
		/// </summary>
		public string Modelo { get; set; }
		/// <summary>
		/// Lista de abastecimentos 
		/// </summary>
		public List<Abastecimento> Abastecimentos { get; set; }
	}
}
