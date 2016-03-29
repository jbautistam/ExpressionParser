using System;
using System.Windows.Forms;

using Bau.Libraries.LibExpressionParser;
using Bau.Libraries.LibExpressionParser.Parser.Expressions;
using Bau.Libraries.LibExpressionParser.Variables;

namespace TestParser
{
	/// <summary>
	///		Formulario principal de la aplicación
	/// </summary>
	public partial class frmMain : Form
	{
		public frmMain()
		{ InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{ if (!string.IsNullOrEmpty(Properties.Settings.Default.LastFormula))
				txtSource.Text = Properties.Settings.Default.LastFormula;
			if (!string.IsNullOrEmpty(Properties.Settings.Default.LastVariables))
				txtVariables.Text = Properties.Settings.Default.LastVariables;
		}

		/// <summary>
		///		Interpreta un código fuente
		/// </summary>
		private void Parse(string strSource)
		{ Compiler objCompiler = new Compiler();
			ExpressionsCollection objColExpressions;
			int intDecimals;

				// Obtiene el número de decimales
					if (!int.TryParse(txtDecimals.Text, out intDecimals))
						intDecimals = 2;		
				// Guarda los datos
					Properties.Settings.Default.LastFormula = txtSource.Text;
					Properties.Settings.Default.LastVariables = txtVariables.Text;
					Properties.Settings.Default.Save();
				// Asigna las propiedades de compilación
					objCompiler.Properties.Decimals = intDecimals;
				// Vacía los cuadros de texto de resultado
					txtStack.Text = "";
					txtLog.Text = "";
				// Interpreta una cadena
					objColExpressions = objCompiler.Parse(strSource);
				// Muestra los resultados
					txtStack.Text = objColExpressions.GetDebugInfo();
					if (objCompiler.LocalErrors.Count != 0)
						{ AddLogTitle("ERRORES");
							foreach (Bau.Libraries.LibExpressionParser.Errors.CompilerError objError in objCompiler.LocalErrors)
								AddLog($"Token: {objError.Token} Descripción: {objError.Description}");
						}
					else //... Si no hay errores, evalúa las expresiones
						{	// Valida las expresiones (con variables predefinidas)
								ValidateExpressions(objCompiler, objColExpressions);
							// Y ejecuta las expresiones
								ComputeExpressions(objCompiler, objColExpressions);
						}
		}

		/// <summary>
		///		Comprueba las expresiones con variables predefinidas
		/// </summary>
		private void ValidateExpressions(Compiler objCompiler, ExpressionsCollection objColExpressions)
		{	string strError;

				// Log
					AddLogTitle("VALIDACIÓN");
				// Evalúa las expresiones
					if (objCompiler.Validate(objColExpressions, GetVariables(), out strError))
						AddLog("Se ha evaluado la expresión correctamente");
					else
						AddLog("Error en la validación " + strError);
		}

		/// <summary>
		///		Calcula las expresiones
		/// </summary>
		private void ComputeExpressions(Compiler objCompiler, ExpressionsCollection objColExpressions)
		{ string strError;
			ValueBase objResult;

				// Log
					AddLogTitle("EVALUACIÓN");
				// Ejecuta las expresiones
					objResult = objCompiler.Evaluate(objColExpressions, GetVariables(), out strError);
				// Muestra el resultado
					if (!string.IsNullOrEmpty(strError))
						AddLog("Error en la ejecución: " + strError);
					else
						AddLog("Resultado: " + objResult.Content);
		}

		/// <summary>
		///		Obtiene las variables del cuadro de texto
		/// </summary>
		private VariablesCollection GetVariables()
		{ VariablesCollection objColVariables = new VariablesCollection();

				// Carga las variables
					if (!string.IsNullOrEmpty(txtVariables.Text))
						{ string [] arrStrLines = txtVariables.Text.Split('\n');

								foreach (string strLine in arrStrLines)
									if (!string.IsNullOrEmpty(strLine) && strLine.IndexOf(':') > 0)
										{ string [] arrStrParts = strLine.Split(':');

												if (arrStrParts.Length > 1)
													{ string strValue = arrStrParts[1];

															// Añade al valor el resto de partes que tuvieran dos puntos
																for (int intIndex = 2; intIndex < arrStrParts.Length; intIndex++)
																	{ if (!string.IsNullOrEmpty(strValue))
																			strValue += ": ";
																		strValue += arrStrParts[intIndex];
																	}
															// Añade la variable
																objColVariables.AddCheckType(arrStrParts[0], strValue.Trim());
													}
										}
						}
				// Devuelve la colección de variables
					return objColVariables;
		}

		/// <summary>
		///		Añade un título al log
		/// </summary>
		private void AddLogTitle(string strTitle)
		{	if (!string.IsNullOrEmpty(txtLog.Text))
				txtLog.Text += Environment.NewLine;
			AddLog(strTitle + Environment.NewLine + new string('-', 30) + Environment.NewLine);
		}

		/// <summary>
		///		Añde un texto al log
		/// </summary>
		private void AddLog(string strText)
		{ txtLog.Text += strText + Environment.NewLine;
		}

		private void frmMain_Load(object sender, EventArgs e)
		{ InitForm();
		}

		private void cmdParse_Click(object sender, EventArgs e)
		{ Parse(txtSource.Text);
		}
	}
}
