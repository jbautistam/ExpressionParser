using System;
using System.Collections.Generic;

using Bau.Libraries.LibExpressionParser.Parser.Expressions;
using Bau.Libraries.LibExpressionParser.Variables;

namespace Bau.Libraries.LibExpressionParser.Parser.Evaluator
{
	/// <summary>
	///		Clase para el cálculo de expresiones
	/// </summary>
	internal class ExpressionCompute
	{
		internal ExpressionCompute(VariablesCollection objColVariables, CompilerProperties objProperties)
		{ Properties = objProperties;
			if (objColVariables == null)
				Variables = new VariablesCollection();
			else
				Variables = objColVariables.Clone();
		}

		/// <summary>
		///		Evalúa una serie de expresiones
		/// </summary>
		internal ValueBase Evaluate(ExpressionsCollection objStackExpressions, out string strError)
		{ return Compute(objStackExpressions.Clone(), out strError);
		}

		/// <summary>
		///		Calcula una expresión
		/// </summary>
		private ValueBase Compute(ExpressionsCollection objStackExpressions, out string strError)
		{ Stack<ValueBase> objStackOperators = new Stack<ValueBase>();
			ValueBase objResult = null;

				// Inicializa los argumentos de salida
					strError = "";
				// Calcula el resultado
					foreach (ExpressionBase objExpression in objStackExpressions)
						if (string.IsNullOrEmpty(strError))
							{ if (objExpression.Token.Type == Tokens.Token.TokenType.String)
									objStackOperators.Push(new ValueString(objExpression.Token.Content));
								else if (objExpression.Token.Type == Tokens.Token.TokenType.Number)
									objStackOperators.Push(new ValueNumeric(Lexical.ParserHelper.ConvertToNumeric(objExpression.Token.Content)));
								else if (objExpression is ExpressionIdentifier)
									{ ExpressionIdentifier objVariableIdentifier = objExpression as ExpressionIdentifier;

											if (objVariableIdentifier != null) // ... esto no debería pasar nunca
												{ ValueBase objVariableValue = GetValueVariable(objVariableIdentifier);

														// Añade el resultado a la pila si no hay ningún error
															if (objVariableValue != null)
																{ if (objVariableValue.HasError)
																		strError = objVariableValue.Error;
																	else
																		objStackOperators.Push(objVariableValue);
																}
															else
																strError = "No se encuentra el valor de la variable '" + objVariableIdentifier + "'";
												}
											else
												strError = "No se reconoce el tipo de expresión de variable";
									}
								else
									{ ValueBase objComputeResult = null;

											// Realiza la operación
												switch (objExpression.Token.Content)
													{ case "+":
														case "-":
														case "*":
														case "/":
																objComputeResult = ComputeBinary(objStackOperators, objExpression.Token.Content);
															break;
														default:
																objComputeResult = ValueBase.GetError("Operador desconocido: " + objExpression.Token.Content);
															break;
													}
											// Añade el resultado a la pila (aunque haya error, para que así sea el último operador de la pila)
												if (objComputeResult.HasError)
													strError = objComputeResult.Error;
												else
													{ if (objComputeResult is ValueNumeric)
															{ ValueNumeric objValue = objComputeResult as ValueNumeric;

																	objValue.Value = Math.Round(objValue.Value, Properties.Decimals);
															}
													}
												objStackOperators.Push(objComputeResult);
									}
							}
				// Obtiene el resultado si no hay errores
					if (string.IsNullOrEmpty(strError))
						{ if (objStackOperators.Count == 1)
								objResult = objStackOperators.Pop();
							else if (objStackOperators.Count == 0)
								strError = "No hay ningún operador en la pila de operaciones";
							else 
								strError = "Hay más de un operador en la pila de instrucciones";
						}
				// Si hay algún error, lo convierte en el resultado
					if (!string.IsNullOrEmpty(strError))
						objResult = ValueBase.GetError(strError);
				// Devuelve el resultado
					return objResult;
		}

		/// <summary>
		///		Obtiene el valor contenido en una variable
		/// </summary>
		private ValueBase GetValueVariable(ExpressionIdentifier objVariableIdentifier)
		{ Variable objVariable = Variables.Search(objVariableIdentifier.Name);

				if (objVariable == null)
					return null;
				else
					return objVariable.Value;
		}

		/// <summary>
		///		Calcula una operación con dos valores
		/// </summary>
		private ValueBase ComputeBinary(Stack<ValueBase> objStackOperators, string strOperator)
		{ if (objStackOperators.Count < 2)
				return ValueBase.GetError("No existen suficientes operandos en la pila para ejecutar el operador '" + strOperator + "'");
			else
				return objStackOperators.Pop().Execute(objStackOperators.Pop(), strOperator);
		}

		/// <summary>
		///		Variables
		/// </summary>
		internal VariablesCollection Variables { get; set; }

		/// <summary>
		///		Propiedades de compilación
		/// </summary>
		private CompilerProperties Properties { get; set; }
	}
}
