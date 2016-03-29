using System;
using System.Collections.Generic;
using System.Linq;

namespace Bau.Libraries.LibExpressionParser.Variables
{
	/// <summary>
	///		Colección de <see cref="Variable"/>
	/// </summary>
	public class VariablesCollection : List<Variable>
	{
		/// <summary>
		///		Infiere un tipo y lo añade
		/// </summary>
		public void AddCheckType(string strName, string strValue)
		{ if (Parser.Lexical.ParserHelper.IsExternalNumeric(strValue))
				Add(strName, Parser.Lexical.ParserHelper.ConvertToNumeric(strValue));
			else
				Add(strName, strValue);			
		}

		/// <summary>
		///		Añade una variable con un valor numérico
		/// </summary>
		public void Add(string strName, double dblValue)
		{ Add(strName, new ValueNumeric(dblValue));
		}

		/// <summary>
		///		Añade una variable
		/// </summary>
		public void Add(string strName, string strValue)
		{ Add(strName, new ValueString(strValue));
		}

		/// <summary>
		///		Añade una variable
		/// </summary>
		internal void Add(string strName, ValueBase objValue)
		{ int intIndexFound = IndexOf(strName);
				
				if (intIndexFound >= 0)
					this[intIndexFound].Value = objValue;
				else
					Add(new Variable(strName, objValue));
		}

		/// <summary>
		///		Obtiene el índice de una variable
		/// </summary>
		public int IndexOf(string strName)
		{ // Recorre la colección
				for (int intIndexCollection = 0; intIndexCollection < Count; intIndexCollection++)
					if (!string.IsNullOrEmpty(this[intIndexCollection].Name) &&
							this[intIndexCollection].Name.Equals(strName, StringComparison.CurrentCultureIgnoreCase))
						return intIndexCollection;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
				return -1;
		}

		/// <summary>
		///		Busca una variable
		/// </summary>
		public Variable Search(string strName)
		{ return this.FirstOrDefault<Variable>(objSearchVariable => objSearchVariable.Name != null && 
																					 objSearchVariable.Name.Equals(strName, StringComparison.CurrentCultureIgnoreCase));
		}

		/// <summary>
		///		Clona una colección de variables
		/// </summary>
		internal VariablesCollection Clone()
		{ VariablesCollection objColVariables = new VariablesCollection();

				// Clona la colección
					foreach (Variable objVariable in this)
						objColVariables.Add(objVariable.Clone());
				// Devuelve la colección
					return objColVariables;
		}
	}
}
