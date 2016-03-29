using System;
using System.Collections.Generic;

using Bau.Libraries.LibExpressionParser.Parser.Expressions;

namespace Bau.Libraries.LibExpressionParser.Parser.Evaluator
{
	/// <summary>
	///		Conversor de expresiones a notación polaca inversa
	/// </summary>
	internal class ExpressionConversorRpn
	{	
		/// <summary>
		///		Convierte una colección de expresiones en una pila de expresiones en notación polaca inversa (sin paréntesis)
		/// </summary>
		internal ExpressionsCollection ConvertToRPN(ExpressionsCollection objColExpressions, out string strError)
		{ ExpressionsCollection objStackOutput = new ExpressionsCollection();
			Stack<ExpressionBase> objStackOperators = new Stack<ExpressionBase>();

				// Inicializa los argumentos de salida
					strError = "";
				// Convierte las expresiones en una pila
					foreach (ExpressionBase objExpression in objColExpressions)
						if (objExpression is ExpressionBase)
							{ switch (objExpression.Token.Type)
									{ case Tokens.Token.TokenType.LeftParentesis:
												// Paréntesis izquierdo, se mete directamente en la pila de operadores
													objStackOperators.Push(objExpression);
											break;
										case Tokens.Token.TokenType.RightParentesis:
												bool blnEnd = false;

													// Paréntesis derecho. Saca todos los elementos del stack hasta encontrar un paréntesis izquierdo
														while (objStackOperators.Count > 0 && !blnEnd)
															{ ExpressionBase objExpressionOperator = objStackOperators.Pop();

																	if (objExpressionOperator.Token.Type == Tokens.Token.TokenType.LeftParentesis)
																		blnEnd = true;
																	else
																		objStackOutput.Add(objExpressionOperator);
															}
											break;
										case Tokens.Token.TokenType.ArithmeticOperator:
												bool blnEndOperator = false;

													// Recorre los operadores de la pila 
														while (objStackOperators.Count > 0 && !blnEndOperator)
															{ ExpressionBase objLastOperator = null;
												
																	// Obtiene el último operador de la pila (sin sacarlo)
																		objLastOperator = objStackOperators.Peek();
																	// Si no hay ningún operador en la pila o la prioridad del operador actual es mayor que la del último de la pila se mete el último operador
																		if (objLastOperator == null || objExpression.Token.Type == Tokens.Token.TokenType.LeftParentesis || 
																				objExpression.Priority > objLastOperator.Priority)
																			blnEndOperator = true;
																		else // ... si el operador tiene una prioridad menor que el último de la fila, se quita el último operador de la pila y se compara de nuevo
																			objStackOutput.Add(objStackOperators.Pop());
															}
													// Añade el operador a la pila de operadores
														objStackOperators.Push(objExpression);
											break;
										case Tokens.Token.TokenType.Number:
										case Tokens.Token.TokenType.String:
										case Tokens.Token.TokenType.Variable:
												// Si es un número o una cadena o una variable se copia directamente en la pila de salida
													objStackOutput.Add(objExpression);
											break;
										default:
												strError = "Expresión desconocida";
											break;
									}
							}
				// Añade todos los elementos que queden en el stack de operadores al stack de salida
					while (objStackOperators.Count > 0)
						objStackOutput.Add(objStackOperators.Pop());
				// Devuelve la pila convertida a notación polaca inversa
					return objStackOutput;
		}
	}
}
