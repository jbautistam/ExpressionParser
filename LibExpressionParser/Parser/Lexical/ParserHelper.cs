using System;

namespace Bau.Libraries.LibExpressionParser.Parser.Lexical
{
	/// <summary>
	///		Métodos de ayuda de tratamiento de cadenas
	/// </summary>
	internal static class ParserHelper
	{
		/// <summary>
		///		Comprueba si es un número
		/// </summary>
		internal static bool IsNumeric(string strContent)
		{ bool blnExistDot = false;

				// Comprueba todos los caracteres o si la cadena es sólo un punto
					if (strContent == ".") 
						return false;
					else
						foreach (char chrChar in strContent)
							if (!char.IsDigit(chrChar) && chrChar != '.')
								return false;
							else if (chrChar == '.')
								{ if (blnExistDot)
										return false;
									else
										blnExistDot = true;
								}
				// Si ha llegado hasta aquí es porqe es numérico
					return !string.IsNullOrWhiteSpace(strContent);
		}

		/// <summary>
		///		Convierte un valor a numérico
		/// </summary>
		internal static double ConvertToNumeric(string strValue)
		{ double dblValue = 0;

				// Convierte la cadena en un número
					if (string.IsNullOrEmpty(strValue) || !double.TryParse(NormalizeNumber(strValue), 
																																 System.Globalization.NumberStyles.AllowDecimalPoint,
																																 System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"),
																																 out dblValue))
						dblValue = 0;
				// Devuelve el valor
					return dblValue;
		}

		/// <summary>
		///		Comprueba si un valor pasado externamente (es decir, que no ha pasado por el intérprete) es numérico
		/// </summary>
		internal static bool IsExternalNumeric(string strValue)
		{ return IsNumeric(NormalizeNumber(strValue));
		}

		/// <summary>
		///		Normaliza una cadena con número (cambia las , por .)
		/// </summary>
		private static string NormalizeNumber(string strValue)
		{ if (string.IsNullOrEmpty(strValue))
				return "";
			else 
				return strValue.Replace(',', '.');
		}
	}
}
