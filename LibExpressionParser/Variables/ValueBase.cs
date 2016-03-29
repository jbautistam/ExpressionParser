using System;

namespace Bau.Libraries.LibExpressionParser.Variables
{
	/// <summary>
	///		Valor
	/// </summary>
	public abstract class ValueBase
	{ 
		/// <summary>
		///		Obtiene un valor predeterminado con un error
		/// </summary>
		public static ValueBase GetError(string strError)
		{ ValueString objValue = new ValueString("ERROR");

				// Asigna el error
					objValue.Error = strError;
				// Devuelve el valor
					return objValue;
		}

		/// <summary>
		///		Ejecuta una operación
		/// </summary>
		internal abstract ValueBase Execute(ValueBase objValue, string strOperator);

		/// <summary>
		///		Contenido del valor (numérico, cadena ...)
		/// </summary>
		public abstract string Content { get; }

		/// <summary>
		///		Comprueba si hay algún error en el valor
		/// </summary>
		public bool HasError 
		{ get { return !string.IsNullOrEmpty(Error); }
		}

		/// <summary>
		///		Error encontrado en la última operación
		/// </summary>
		public string Error { get; private set; }
	}
}
