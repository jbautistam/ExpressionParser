using System;

namespace Bau.Libraries.LibExpressionParser.Variables
{
	/// <summary>
	///		Valor de tipo cadena
	/// </summary>
	public class ValueString : ValueBase
	{
		public ValueString(string strValue)
		{ // Asigna el valor
				Value = strValue;
			// Asigna la cadena de inicio
				if (!string.IsNullOrEmpty(Value) && Value.StartsWith("\""))
					{ if (Value.Length == 1)
							Value = "";
						else
							Value = Value.Substring(1);
					}
			// Asigna la cadena de fin
				if (!string.IsNullOrEmpty(Value) && Value.EndsWith("\""))
					{ if (Value.Length == 1)
							Value = "";
						else
							Value = Value.Substring(0, Value.Length - 1);
					}
		}

		/// <summary>
		///		Ejecuta una operación
		/// </summary>
		internal override ValueBase Execute(ValueBase objValue, string strOperator)
		{ ValueBase objNewValue = null;

				// Ejecuta la operación
					switch (strOperator)
						{ case "+":
									if (objValue is ValueNumeric)
										objNewValue = new ValueString(objValue.Content + Value);
									else
										objNewValue = new ValueString((objValue as ValueString).Value + Value);				
								break;
						}
				// Crea el error
					if (objNewValue == null)
						objNewValue = ValueBase.GetError(string.Format("No se puede utilizar el operador '{0}' entre los valores {1} y {2}", strOperator, Content, objValue.Content));
				// Devuelve el valor
					return objNewValue;
		}

		/// <summary>
		///		Normaliza una cadena: sin nulos, sin espacios y en mayúsculas
		/// </summary>
		private string Normalize(string strValue)
		{	// Normaliza la cadena
				if (string.IsNullOrWhiteSpace(strValue))
					strValue = "";
			// Devuelve la cadena sin espacios y en mayúscula
				return strValue.Trim().ToUpper();
		}

		/// <summary>
		///		Valor
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		public override string Content
		{ get { return Value; }
		}
	}
}
