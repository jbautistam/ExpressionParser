using System;

namespace Bau.Libraries.LibExpressionParser.Parser.Expressions
{
	/// <summary>
	///		Expresión de tipo variable
	/// </summary>
	public class ExpressionIdentifier : ExpressionBase
	{
		internal ExpressionIdentifier(Tokens.Token objToken) : base(objToken) 
		{ Name = objToken.Content;
		}

		/// <summary>
		///		Clona el identificador de variable
		/// </summary>
		internal override ExpressionBase Clone()
		{ return new ExpressionIdentifier(base.Token);
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		internal override string GetDebugInfo()
		{ return "Variable: " + Name;
		}

		/// <summary>
		///		Nombre de la variable
		/// </summary>
		internal string Name { get; set; }
	}
}
