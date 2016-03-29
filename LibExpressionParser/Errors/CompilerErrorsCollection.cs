using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibExpressionParser.Errors
{
	/// <summary>
	///		Colección de <see cref="CompilerError"/>
	/// </summary>
	public class CompilerErrorsCollection : List<CompilerError>
	{
		/// <summary>
		///		Añade un error a partir de un token
		/// </summary>
		internal void Add(Parser.Tokens.Token objToken, string strError)
		{ CompilerError objError = new CompilerError();

				// Asigna las propiedades
					objError.Token = objToken.Content;
					objError.Description = strError;
				// Añade el error
					Add(objError);
		}
	}
}
