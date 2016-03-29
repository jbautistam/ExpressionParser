using System;

using Bau.Libraries.LibExpressionParser.Parser.Tokens;

namespace Bau.Libraries.LibExpressionParser.Parser.Lexical
{
	/// <summary>
	///		Conversor de una cadena en tokens
	/// </summary>
	internal class StringTokenizer
	{ 
		internal StringTokenizer(string strSource)
		{ Wordizer = new StringWord(strSource);
		}

		/// <summary>
		///		Obtiene todos los tokens de la cadena
		/// </summary>
		internal TokensCollection GetAllTokens()
		{ TokensCollection objColTokens = new TokensCollection();

				// Obtiene todos los tokens
					do
						{ objColTokens.Add(GetNextToken());
						}
					while (objColTokens[objColTokens.Count - 1].Type != Token.TokenType.EOF);
				// Devuelve la colección de tokens
					return objColTokens;
		}

		/// <summary>
		///		Obtiene el siguiente token
		/// </summary>
		internal Token GetNextToken()
		{ Token objToken = new Token();
			StringWord.WordStruct objWord = Wordizer.GetNextWord();

				// Pasa los datos de la cadena al token
					objToken.Content = objWord.Content;
					if (!string.IsNullOrEmpty(objToken.Content))
						objToken.Content = objToken.Content.Trim();
				// Obtiene el tipo de token
					if (objWord.IsEof)
						objToken.Type = Token.TokenType.EOF;
					else
						objToken.Type = GetType(objToken.Content);
				// Devuelve el token
					return objToken;
		}

		/// <summary>
		///		Obtiene el tipo de contenido
		/// </summary>
		private Token.TokenType GetType(string strContent)
		{ if (strContent.StartsWith("\"") && strContent.EndsWith("\""))
				return Token.TokenType.String;
			else if (strContent == "(")
				return Token.TokenType.LeftParentesis;
			else if (strContent == ")")
				return Token.TokenType.RightParentesis;
			else if (strContent == "+" || strContent == "-" || strContent == "*" || strContent == "/" || strContent == "\\")
				return Token.TokenType.ArithmeticOperator;
			else if (ParserHelper.IsNumeric(strContent))
				return Token.TokenType.Number;
			else if (IsVariableName(strContent))
				return Token.TokenType.Variable;
			else
				return Token.TokenType.Unknown;
		}

		/// <summary>
		///		Comprueba si una cadena es un nombre de variable
		/// </summary>
		private bool IsVariableName(string strContent)
		{ bool blnIsVariable = true;

				// Comprueba si el contenido es una variable
					if (string.IsNullOrEmpty(strContent))
						blnIsVariable = false;
					else
						{ // Normaliza el contenido
								strContent = strContent.Trim().ToUpper();
							// Comprueba si se corresponde con las restricciones de una variable: letras o dígitos o _ 
							// comenzando por una letra o un _
								if (strContent.Length > 0 && char.IsDigit(strContent[0]))
									blnIsVariable = false;
								else
									foreach (char chrChar in strContent)
										if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ_".IndexOf(chrChar) < 0 && !char.IsDigit(chrChar))
											blnIsVariable = false;
						}
				// Devuelve el valor que indica si es una variable			
					return blnIsVariable;
		}

		/// <summary>
		///		Objeto para interpretación de palabras
		/// </summary>
		private StringWord Wordizer { get; set; }
	}
}
