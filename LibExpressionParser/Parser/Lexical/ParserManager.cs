using System;

using Bau.Libraries.LibExpressionParser.Parser.Expressions;
using Bau.Libraries.LibExpressionParser.Parser.Tokens;

namespace Bau.Libraries.LibExpressionParser.Parser.Lexical
{
	/// <summary>
	///		Intérprete de expresiones
	/// </summary>
	internal class ParserManager
	{ // Variables privadas
			private Evaluator.ExpressionConversorRpn objExpressionEvaluator;

		internal ParserManager(Compiler objCompiler)
		{ Compiler = objCompiler;
			Tokens = new TokensCollection();
			objExpressionEvaluator = new Evaluator.ExpressionConversorRpn();
		}

		/// <summary>
		///		Interpreta una cadena
		/// </summary>
		internal ExpressionsCollection Parse(string strSource)
		{ ExpressionsCollection objColExpressions = new ExpressionsCollection();
			string strError;

				// Crea la colección de tokens
					Tokens = new StringTokenizer(strSource.TrimEnd()).GetAllTokens();
				// Lee las expresiones
					objColExpressions = ReadExpression(out strError);
				// Añade los errores
					if (!string.IsNullOrEmpty(strError))
						AddError(null, strError);
					else if (!IsEof(GetToken(true)))
						AddError(null, "Fin de expresión desconocido");
				// Devuelve las expresiones
					return objColExpressions;
		}

		/// <summary>
		///		Lee las expresiones
		/// </summary>
		private ExpressionsCollection ReadExpression(out string strError)
		{ ExpressionsCollection objColExpressions = new ExpressionsCollection();
			Token objNextToken = GetToken();

				// Inicializa los valores de salida
					strError = "";
				// Lee las expresiones
					while (!IsEof(objNextToken) && objNextToken.IsExpressionPart && string.IsNullOrEmpty(strError))
						{ // Añade el token a la colección de expresiones
								if (objNextToken.Type == Token.TokenType.Variable)
									objColExpressions.Add(new ExpressionIdentifier(objNextToken));
								else
									objColExpressions.Add(new ExpressionBase(objNextToken));
							// Obtiene el siguiente token
								objNextToken = GetToken();
						}
				// Devuelve la colección de expresiones
					return objColExpressions;
		}

		/// <summary>
		///		Obtiene el siguiente token
		/// </summary>
		private Token GetToken(bool blnIsSimulated = false)
		{ if (Tokens == null || IndexToken > Tokens.Count - 1)
				return new Token { Type = Token.TokenType.EOF };
			else if (blnIsSimulated)
				return Tokens[IndexToken];
			else
				return Tokens[IndexToken++];
		}

		/// <summary>
		///		Añade un error
		/// </summary>
		private void AddError(Token objToken, string strMessage)
		{ Compiler.LocalErrors.Add(objToken, strMessage);
		}

		/// <summary>
		///		Comprueba si es el final del archivo
		/// </summary>
		private bool IsEof(Token objToken)
		{ return objToken.Type == Token.TokenType.EOF;
		}

		/// <summary>
		///		Compilador
		/// </summary>
		private Compiler Compiler { get; set; }

		/// <summary>
		///		Tokens
		/// </summary>
		internal TokensCollection Tokens { get; private set; }

		/// <summary>
		///		Indice del token actual
		/// </summary>
		private int IndexToken { get; set; }
	}
}
