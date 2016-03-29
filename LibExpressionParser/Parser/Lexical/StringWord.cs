using System;

namespace Bau.Libraries.LibExpressionParser.Parser.Lexical
{
	/// <summary>
	///		Clase para obtener los caracteres de una cadena
	/// </summary>
	internal class StringWord
	{
		/// <summary>
		///		Estructura con los datos de una palabra
		/// </summary>
		internal struct WordStruct
			{ internal bool IsEof;
				internal string Content;
			}

		internal StringWord(string strSource)
		{ Source = strSource;
			IndexActualChar = 0;
			PreviousChar = ' ';
			PreviousWord = new WordStruct();
		}

		/// <summary>
		///		Obtiene la siguiente palabra
		/// </summary>
		internal WordStruct GetNextWord()
		{	WordStruct objWord = new WordStruct();

				// Obtiene la siguiente cadena (o null si ha terminado con el archivo)
					if (IsEof())
						objWord.IsEof = true;
					else
						{ // Salta los espacios
								SkipSpaces();
							// Dependiendo del modo ...
								objWord.Content = GetNextStringExpression();
						}
				// Guarda la palabra anterior
					PreviousWord = objWord;
				// Devuelve la palabra
					return objWord;
		}

		/// <summary>
		///		Obtiene la sigiente cadena para una expresión
		/// </summary>
		private string GetNextStringExpression()
		{ char chrChar = GetChar();
			string strResult = chrChar.ToString();
			char chrNextChar = GetNextChar();

				// Dependiendo del carácter inicial, lee el resto de los caracteres
					if (chrChar == '\"')
						strResult += ReadToEndString();
					else if (IsAlphabetic(strResult) || strResult == "_")
						strResult += ReadToEndVariable();
					else if (IsDigit(chrChar.ToString()))
						strResult += ReadToEndNumber();
					else if (!IsArithmeticOperator(chrChar))
						strResult += ReadToEndWord();
				// Devuelve la cadena de código
					return strResult;
		}

		/// <summary>
		///		Lee hasta el final de la palabra (letras y dígitos)
		/// </summary>
		private string ReadToEndWord()
		{ string strResult = "";
			string strNextChar = GetFirstChars(1, false);

				// Busca el carácter final para la palabra (letras y dígitos)
					while (!IsEof() && (IsAlphabetic(strNextChar) || IsDigit(strNextChar)))
						{ // Añade el carácter al resultado
								strResult += GetChar();
							// Obtiene el siguiente carácter por adelantado
								strNextChar = GetFirstChars(1, false);
						}
				// Devuelve la cadena
					return strResult;
		}

		/// <summary>
		///		Lee hasta el final de la variable (letras, dígitos y carácter de subrayado)
		/// </summary>
		private string ReadToEndVariable()
		{ string strResult = "";
			string strNextChar = GetFirstChars(1, false);

				// Busca el carácter final para la variable (letras y dígitos y los caracteres _)
					while (!IsEof() && (IsAlphabetic(strNextChar) || IsDigit(strNextChar) || strNextChar == "_"))
						{ // Añade el carácter al resultado
								strResult += GetChar();
							// Obtiene el siguiente carácter por adelantado
								strNextChar = GetFirstChars(1, false);
						}
				// Devuelve la cadena
					return strResult;
		}

		/// <summary>
		///		Lee hasta el final de la cadena
		/// </summary>
		private string ReadToEndString()
		{ string strResult = "";
			string strNextChar = GetFirstChars(1, true);

				// Busca el carácter final para la cadena
					while (!IsEof() && strNextChar != "\"")
						{ // Añade el carácter al resultado
								strResult += GetChar();
							// Obtiene el siguiente carácter por adelantado
								strNextChar = GetFirstChars(1, false);
						}
				// Añade las comillas finales
					if (strNextChar == "\"")
						strResult += GetChar();
				// Devuelve la cadena
					return strResult;
		}

		/// <summary>
		///		Lee hasta el final del número
		/// </summary>
		private string ReadToEndNumber()
		{ string strResult = "";
			string strNextChar = GetFirstChars(1, true);

				// Busca el carácter final para la cadena
					while (!IsEof() && (IsDigit(strNextChar) || strNextChar == "."))
						{ // Añade el carácter al resultado
								strResult += GetChar();
							// Obtiene el siguiente carácter por adelantado
								strNextChar = GetFirstChars(1, false);
						}
				// Devuelve la cadena
					return strResult;
		}

		/// <summary>
		///		Salta los espacios
		/// </summary>
		private void SkipSpaces()
		{ if (!IsEof())
				{ string strNextChars = GetFirstChars(1, false);

						// Se salta los espacios
							while (!IsEof() && IsSpace(strNextChars))
								{ char chrChar = GetChar();

										// Incrementa el número de tabuladores
											if (chrChar == '\t')
												Indent++;
										// Obtiene el siguiente carácter (sin sacarlo del buffer)
											strNextChars = GetFirstChars(1, false);
								}
				}
		}

		/// <summary>
		///		Se salta una serie de caracteres
		/// </summary>
		private void SkipChars(int intLength)
		{ for (int intIndex = 0; intIndex < intLength; intIndex++)
				GetChar();
		}

		/// <summary>
		///		Obtiene el carácter actual
		/// </summary>
		private char GetChar()
		{ char chrChar = ' ';

				// Obtiene el siguiente carácter (si existe)
					if (!IsEof())
						{ // Obtiene el carácter
								chrChar = Source[IndexActualChar];
							// Incrementa el índice actual
								IndexActualChar++;
							// Guarda el carácter en los caracteres previos
								PreviousChar = chrChar;
						}
				// Devuelve el carácter
					return chrChar;
		}

		/// <summary>
		///		Obtiene los primeros n caracteres (sin sacarlos del buffer)
		/// </summary>
		private string GetFirstChars(int intLength, bool blnAtString)
		{ string strNextChars = "";
			int intStartPosition = IndexActualChar;

				// Obtiene los siguientes carácter (si existe)
					for (int intIndex = 0; intIndex < intLength; intIndex++)
						if (intStartPosition + intIndex < Source.Length)
							{ // Se salta la barra actual
									if (!blnAtString && Source[intStartPosition + intIndex] == '\\')
										intStartPosition++;
								// Obtiene el carácter
									strNextChars += Source[intStartPosition + intIndex];
							}
				// Devuelve la cadena de siguientes caracteres
					return strNextChars;
		}

		/// <summary>
		///		Obtiene el siguiente carácter sin sacarlo del buffer
		/// </summary>
		private char GetNextChar()
		{ string strNextChars = GetFirstChars(1, false);

				// Obtiene el primer carácter
				if (!string.IsNullOrWhiteSpace(strNextChars))
					return strNextChars[0];
				else
					return ' ';
		}

		/// <summary>
		///		Indica si es final de archivo
		/// </summary>
		private bool IsEof()
		{ return IndexActualChar >= Source.Length;
		}

		/// <summary>
		///		Comprueba si un carácter es un espacio
		/// </summary>
		private bool IsSpace(string strChar)
		{ return strChar == " " || strChar == "\t" || strChar == "\r" || strChar == "\n";
		}

		/// <summary>
		///		Comprueba si es un dígito
		/// </summary>
		private bool IsDigit(string strNextChar)
		{ return !string.IsNullOrWhiteSpace(strNextChar) && strNextChar.Length > 0 && char.IsDigit(strNextChar[0]);
		}

		/// <summary>
		///		Comprueba si es un carácter alfabético
		/// </summary>
		private bool IsAlphabetic(string strNextChar)
		{ return !string.IsNullOrWhiteSpace(strNextChar) && strNextChar.Length > 0 && char.IsLetter(strNextChar[0]);
		}

		/// <summary>
		///		Comprueba si es un operador aritmético
		/// </summary>
		private bool IsArithmeticOperator(char chrChar)
		{ return chrChar == '+' || chrChar == '-' || chrChar == '/' || chrChar == '*' || chrChar == '\\' || 
						 chrChar == '(' || chrChar == ')';
		}

		/// <summary>
		///		Texto original
		/// </summary>
		internal string Source { get; private set; }

		/// <summary>
		///		Carácter anterior
		/// </summary>
		internal char PreviousChar { get; private set; }

		/// <summary>
		///		Palabra anterior
		/// </summary>
		internal WordStruct PreviousWord { get; private set; }

		/// <summary>
		///		Carácter actual
		/// </summary>
		internal int IndexActualChar { get; private set; }

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; private set; }
	}
}
