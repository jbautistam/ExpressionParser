using System;

namespace Bau.Libraries.LibExpressionParser.Errors
{
	/// <summary>
	///		Clase con los datos de un error
	/// </summary>
	public class CompilerError
	{
		/// <summary>
		///		Token que originó el error
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		///		Descripción del error
		/// </summary>
		public string Description { get; set; }
	}
}
