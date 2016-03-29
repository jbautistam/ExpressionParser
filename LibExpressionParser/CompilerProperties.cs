using System;

namespace Bau.Libraries.LibExpressionParser
{
	/// <summary>
	///		Propiedades de compilación
	/// </summary>
	public class CompilerProperties
	{
		public CompilerProperties()
		{ Decimals = 2;
		}

		/// <summary>
		///		Número de decimales
		/// </summary>
		public int Decimals { get; set; }
	}
}
