using System;

using Bau.Libraries.LibExpressionParser.Parser.Expressions;
using Bau.Libraries.LibExpressionParser.Variables;

namespace Bau.Libraries.LibExpressionParser.Parser.Evaluator
{
	/// <summary>
	///		Validación de expresiones
	/// </summary>
	internal class ExpressionValidator
	{
		/// <summary>
		///		Comprueba si se puede evaluar una colección de expresiones
		/// </summary>
		internal bool Validate(ExpressionsCollection objColExpressionsRPN, VariablesCollection objColVariables, 
													 CompilerProperties objProperties, out string strError)
		{ ExpressionCompute objGenerator = new ExpressionCompute(objColVariables, objProperties);

				// Inicializa los argumentos de salida
					strError = "";
				// Evalúa las expresiones
					objGenerator.Evaluate(objColExpressionsRPN, out strError);
				// Devuelve el valor que indica si se puede evaluar
					return string.IsNullOrEmpty(strError);
		}

		/// <summary>
		///		Crea una colección de variables vacías de acuerdo con las expresiones
		/// </summary>
		private VariablesCollection CreateEmptyVariables(ExpressionsCollection objColExpressionsRPN)
		{ VariablesCollection objColVariables = new VariablesCollection();

				// Asigna las variables
					foreach (ExpressionBase objExpression in objColExpressionsRPN)
						if (objExpression is ExpressionIdentifier)
						objColVariables.Add((objExpression as ExpressionIdentifier).Name, new ValueNumeric(1));
				// Devuelve la colección de variables
					return objColVariables;
		}
	}
}
