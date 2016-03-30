# ExpressionParser
Uno de los requisitos que me encuentro frecuentemente en mis proyectos es la interpretación y el cálculo de expresiones matemáticas desde una aplicación.

Una librería que pueda interpretar y ejecutar expresiones matemáticas no es un proceso excesivamente complejo, por he separado esta sección de mi librería de compilación de NHaml de BauPlugStudio y la he subido a GitHub para poder utilizarla por separado.

## Evaluación de expresiones
Las expresiones que interpreta la librería son de este tipo:

	
    (3 + 5) * 8 - 2
	
es decir, operaciones aritméticas sencillas.

Para hacerlo más flexible, he añadido además la opción del tratamiento de variables que nos permite evaluar expresiones de este tipo:

	
    (3 + a) * 8 - b
	
Además de valores numéricos, también podemos utilizar cadenas:

	
    a / b + "%"
	
Por supuesto, las cadenas sólo se pueden sumar (concatenar), no se pueden ni multiplicar, ni dividir.

## Utilización de la librería
Usar la librería de evaluación de expresiones es bastante sencillo, de hecho podemos completar el proceso en tres pasos.

En primer lugar debemos interpretar la cadena. Para ello utilizamos el método Parse de la clase Compiler . Este método nos devuelve una pila de expresiones:

    Compiler objCompiler = new Compiler();
    ExpressionsCollection objColExpressions;
    
    // Asigna las propiedades de compilación
    objCompiler.Properties.Decimals = 2;
    // Interpreta la expresión de una cadena de texto
    objColExpressions = objCompiler.Parse(strSource);
	
Esta pila de expresiones (de tipo ExpressionsCollection ) la podemos utilizar tantas veces como deseemos para realizar nuestros cálculos. Así no tenemos que volver a interpretar la cadena cada vez que deseemos realizar un cálculo con diferentes variables y ahorraremos tiempo.

En el caso que la interpretación haya producido algún error, lo encontraermos en la colección LocalErrors de la clase Compiler :

    foreach (Bau.Libraries.LibExpressionParser.Errors.CompilerError objError in objCompiler.LocalErrors)
      Console.WriteLine($"Token: {objError.Token} Descripción: {objError.Description}");
	
Si no ha habido ningún error, podemos ejecutar esta colección de expresiones y obtener el resultado, para ello primero creamos nuestra colección de variables (si es necesario):

    VariablesCollection objColVariables = new VariablesCollection();
    // Añade la variable
    objColVariables.AddCheckType("a", "1");
    objColVariables.AddCheckType("b", "2");
	
En este caso añadimos un par de variables al azar. El método AddCheckType comprueba si la cadena que le enviamos es numérica o alfanumérica y crea la variable de tipo adecuada.

Lo último que nos queda entonces es ejecutar la colección de expresiones y obtener el resultado:

    ValueBase objResult;
    // Ejecuta las expresiones
    objResult = objCompiler.Evaluate(objColExpressions, objColVariables, out strError);
    // Muestra el resultado
    if (!string.IsNullOrEmpty(strError))
      Console.WriteLine("Error en la ejecución: " + strError);
    else
      Console.WriteLine("Resultado: " + objResult.Content);
	
El resultado de la ejecución puede ser de dos tipos: numérico (del tipo ValueNumeric ) o cadena (del tipo ValueString ). Ambos tipos de valores descienden de ValueBase . Si únicamente queremos mostrar el resultado podemos utilizar directamente la propiedad Content del resultado. Si no debemos evaluar el tipo devuelto y utilizar directamente el valor numérico o el valor de cadena del resultado.
