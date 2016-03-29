using System;

using Bau.Libraries.LibExpressionParser.Parser.Tokens;

namespace Bau.Libraries.LibExpressionParser.Parser.Expressions
{
	/// <summary>
	///		Clase con los datos de una expresión
	/// </summary>
	public class ExpressionBase
	{
		internal ExpressionBase(Token objToken)
		{ Token = objToken;
		}

		/// <summary>
		///		Información de depuración
		/// </summary>
		internal virtual string GetDebugInfo()
		{ return Token.Type.ToString() + ": " + Token.Content;
		}

		/// <summary>
		///		Token asociado a la expresión
		/// </summary>
		internal Token Token { get; private set; }

		/// <summary>
		///		Prioridad de la expresión
		/// </summary>
		public int Priority
		{ get
				{ if (Token.Content == "*" || Token.Content == "/" || Token.Content == "%")
						return 20;
					else if (Token.Content == "+" || Token.Content == "-")
						return 19;
					else
						return 0;
				}
		}

		/// <summary>
		///		Clona una expresión
		/// </summary>
		internal virtual ExpressionBase Clone()
		{ return new ExpressionBase(Token);
		}
	}
}
