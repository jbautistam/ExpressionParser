using System;

namespace Bau.Libraries.LibExpressionParser.Variables
{
	/// <summary>
	///		Clase base para las variables
	/// </summary>
	public class Variable
	{ 
		public Variable(string strName, ValueBase objValue = null)
		{ Name = strName;
			Value = objValue;
		}

		/// <summary>
		///		Clona el contenido de una variable
		/// </summary>
		internal Variable Clone()
		{ return new Variable(Name, Value);
		}

		/// <summary>
		///		Nombre de la variable
		/// </summary>
		public string	Name { get; set; }

		/// <summary>
		///		Valor de la variable
		/// </summary>
		public ValueBase Value { get; set; }
	}
}
