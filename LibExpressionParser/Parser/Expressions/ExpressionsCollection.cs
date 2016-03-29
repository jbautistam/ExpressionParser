using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibExpressionParser.Parser.Expressions
{
	/// <summary>
	///		Colección de <see cref="ExpressionBase"/>
	/// </summary>
	public class ExpressionsCollection : List<ExpressionBase>
	{
		/// <summary>
		///		Obtiene una cadena de depuración
		/// </summary>
		public string GetDebugInfo()
		{ string strDebug = "";

				// Añade los datos a la cadena de depuración
					foreach (ExpressionBase objExpression in this)
						strDebug += objExpression.GetDebugInfo() + Environment.NewLine;
				// Devuelve la cadena de depuración
					return strDebug;
		}

		/// <summary>
		///		Clona la colección de expresiones
		/// </summary>
		internal ExpressionsCollection Clone()
		{ ExpressionsCollection objColExpressions = new ExpressionsCollection();

				// Clona las expresiones
					foreach (ExpressionBase objExpression in this)
						objColExpressions.Add(objExpression.Clone());
				// Devuelve la colección
					return objColExpressions;
		}
	}
}
