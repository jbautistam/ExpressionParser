using System;

namespace Bau.Libraries.LibExpressionParser.Variables
{
	/// <summary>
	///		Valor de tipo numérico
	/// </summary>
	public class ValueNumeric : ValueBase
	{
		public ValueNumeric(double dblValue)
		{ Value = dblValue;
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
										objNewValue = new ValueNumeric((objValue as ValueNumeric).Value + Value);
									else
										objNewValue = new ValueString((objValue as ValueString).Value + Content);
								break;
							case "-":
									if (objValue is ValueNumeric)
										objNewValue = new ValueNumeric((objValue as ValueNumeric).Value - Value);
								break;
							case "/":
									if (objValue is ValueNumeric)
										{ if (Value == 0)
												objNewValue = ValueBase.GetError("No se puede dividir por cero");
											else
												objNewValue = new ValueNumeric((objValue as ValueNumeric).Value / Value);
										}
								break;
							case "*":
									if (objValue is ValueNumeric)
										objNewValue = new ValueNumeric((objValue as ValueNumeric).Value * Value);
								break;
						}
			// Crea el error
				if (objNewValue == null)
					objNewValue = ValueBase.GetError(string.Format("No se puede utilizar el operador '{0}' entre los valores {1} y {2}", strOperator, Content, objValue.Content));
			// Devuelve el valor
				return objNewValue;
		}

		/// <summary>
		///		Valor numérico
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		public override string Content
		{ get { return Value.ToString(); }
		}
	}
}
