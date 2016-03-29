using System;

namespace Bau.Libraries.LibExpressionParser.Parser.Tokens
{
	/// <summary>
	///		Clase con los datos de un token
	/// </summary>
	internal class Token
	{
		/// <summary>
		///		Tipo de token
		/// </summary>
		internal enum TokenType
		{ Unknown,
			Error,
			String,
			Number,
			ArithmeticOperator,
			LeftParentesis,
			RightParentesis,
			Variable,
			EOF
		}

		/// <summary>
		///		Pasa el token a una cadena
		/// </summary>
		public override string ToString()
		{ return string.Format("{0} - {1}", Type, Content);
		}

		/// <summary>
		///		Tipo de token
		/// </summary>
		internal TokenType Type { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		internal string Content { get; set; }

		/// <summary>
		///		Indica si el token forma parte de una expresión
		/// </summary>
		internal bool IsExpressionPart
		{ get 
				{ return Type == TokenType.ArithmeticOperator ||
								 Type == TokenType.Number ||
								 Type == TokenType.String ||
								 Type == TokenType.Variable || 
								 Type == TokenType.LeftParentesis || 
								 Type == TokenType.RightParentesis;
				}
		}
	}
}
