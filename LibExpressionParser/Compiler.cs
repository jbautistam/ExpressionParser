using System;

namespace Bau.Libraries.LibExpressionParser
{
	/// <summary>
	///		Compilador de expresiones
	/// </summary>
	public class Compiler
	{ 
		public Compiler()
		{ LocalErrors = new Errors.CompilerErrorsCollection();
			Properties = new CompilerProperties();
		}

		/// <summary>
		///		Interpreta una cadena (devuelve una colección de expresiones)
		/// </summary>
		public Parser.Expressions.ExpressionsCollection Parse(string strSource)
		{ Parser.Lexical.ParserManager objParser = new Parser.Lexical.ParserManager(this);
			Parser.Expressions.ExpressionsCollection objColExpressions = new Parser.Expressions.ExpressionsCollection();

				// Limpia los errores 
					LocalErrors.Clear();
				// Interpreta las líneas de programa
					objColExpressions = objParser.Parse(strSource);
				// Si no hay ningún error, transforma las expresiones a notación polaca inversa
					if (LocalErrors.Count == 0)
						{ string strError = "";
							
								// Genera las expresiones
									objColExpressions = new Parser.Evaluator.ExpressionConversorRpn().ConvertToRPN(objColExpressions, out strError);
								// Añade los errores de generación
									if (!string.IsNullOrEmpty(strError))
										LocalErrors.Add(null, "Error en la conversión de expresiones a RPN: " + strError);
						}
				// Si hay algún error, limpia la colección de expresiones (no tiene sentido evaluarlas)
					if (LocalErrors.Count != 0)
						objColExpressions.Clear();
				// Devuelve la colección
					return objColExpressions;
		}

		/// <summary>
		///		Evalúa una serie de expresiones
		/// </summary>
		public Variables.ValueBase Evaluate(Parser.Expressions.ExpressionsCollection objColExpressionsRPN, 
																				Variables.VariablesCollection objColVariables, out string strError)
		{ return new Parser.Evaluator.ExpressionCompute(objColVariables, Properties).Evaluate(objColExpressionsRPN, out strError);
		}

		/// <summary>
		///		Valida una serie de expresiones
		/// </summary>
		public bool Validate(Parser.Expressions.ExpressionsCollection objColExpressionsRPN, 
												 Variables.VariablesCollection objColVariables, out string strError)
		{ return new Parser.Evaluator.ExpressionValidator().Validate(objColExpressionsRPN, objColVariables, Properties, 
																																 out strError);
		}

		/// <summary>
		///		Errores
		/// </summary>
		public Errors.CompilerErrorsCollection LocalErrors { get; private set; }

		/// <summary>
		///		Propiedades de compilación
		/// </summary>
		public CompilerProperties Properties { get; private set; }
	}
}
