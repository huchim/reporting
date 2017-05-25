// -----------------------------------------------------------------------
// <copyright file="ReportManager.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Jaguar.Reporting.Common;
    using Jaguar.Reporting.Generators;
    using SystemData = System.Data;

    /// <summary>
    /// Se encarga de procesar los reportes.
    /// </summary>
    public class ReportManager
    {
        /// <summary>
        /// Conexión a la base de datos.
        /// </summary>
        private readonly SystemData.IDbConnection connection;

        /// <summary>
        /// Prefijo para los parámetros.
        /// </summary>
        private readonly string parameterPreffix = "@";

        /// <summary>
        /// Lista de generadores registrados.
        /// </summary>
        private List<IGeneratorEngine> generators = new List<IGeneratorEngine>();

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReportManager"/>.
        /// </summary>
        /// <param name="connection">Conexión a la base de datos.</param>
        public ReportManager(SystemData.IDbConnection connection)
        {
            this.connection = connection;

            // Agregar la lista de generadores disponibles.
            this.generators.Add(new JsonGenerator());
        }

        /// <summary>
        /// Obtiene el reporte activo.
        /// </summary>
        public ReportHandler ActiveReport { get; private set; }

        /// <summary>
        /// Obtiene o establece la lista de variables a reemplazar en el SQL o en el valor predeterminado de los argumentos.
        /// </summary>
        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Agrega una lista de variables predeterminadas para los reportes.
        /// </summary>
        public void AddDefaultVariables()
        {
            // Agregar variables calculadas.
            this.Variables.Add("system.now", DateTime.Now);
        }

        /// <summary>
        /// Agrega un nuevo generador de reportes a la colección.
        /// </summary>
        /// <param name="generator">Generador de reportes.</param>
        public void AddGenerator(IGeneratorEngine generator)
        {
            this.generators.Add(generator);
        }

        /// <summary>
        /// Genera los datos a utilizar por el reporte.
        /// </summary>
        /// <returns>Conjunto de tablas resultado del reporte.</returns>
        public List<DataTable> GenerateData()
        {
            // Los datos se van almacenando en una lista de filas (Dictionary<string, object>)
            var data = new List<DataTable>();

            foreach (var m in this.ActiveReport.Sql)
            {
                // Recuperar la lista de argumentos requeridos.
                var requiredArguments = this.ActiveReport.ArgumentList.Where(x => m.RequiredArguments.Contains(x.Name)).ToArray();

                // Ejecutar la consulta y agregar a la coleción de resultados.
                data.Add(this.GetDataTable(m.Name, m.Script, requiredArguments));
            }

            return data;
        }

        /// <summary>
        /// Devuelve el resultado de la operación de acuerdo al tipo de reporte.
        /// </summary>
        /// <param name="type">Identificador del tipo de generador.</param>
        /// <returns>Resultado de la operación.</returns>
        public byte[] GetBytes(Guid type)
        {
            var generator = this.generators.Single(x => x.Id == type);

            // Obtener los datos de la base de datos.
            var data = this.GenerateData();

            return generator.GetAllBytes(this.ActiveReport, data, this.Variables);
        }

        /// <summary>
        /// Devuelve el resultado de la operación de acuerdo al tipo de reporte.
        /// </summary>
        /// <param name="generator">Generador de reportes.</param>
        /// <returns>Resultado de la operación.</returns>
        public byte[] GetBytes(IGeneratorEngine generator)
        {
            // Obtener los datos de la base de datos.
            var data = this.GenerateData();

            return generator.GetAllBytes(this.ActiveReport, data, this.Variables);
        }

        /// <summary>
        /// Obtiene la lista de generadores disponibles.
        /// </summary>
        /// <returns>Lista de generadores disponibles.</returns>
        public List<IGeneratorEngine> GetGenerators()
        {
            return this.generators;
        }

        /// <summary>
        /// Configura un reporte como el reporte activo e inicializa los argumentos.
        /// </summary>
        /// <param name="report">Información del reporte.</param>
        /// <param name="arguments">Inicializa los argumentos con los valores.</param>
        public void Open(ReportHandler report, Dictionary<string, object> arguments)
        {
            // Agregar las variables de la consulta.
            foreach (var arg in report.ArgumentList)
            {
                if (arguments.ContainsKey(arg.Name))
                {
                    // Si existe el argumento en la colección, debo hacer dos cosas.
                    // La primera es agregarle su valor.
                    var value = arguments[arg.Name];

                    if (value is string)
                    {
                        // Intentar convertir su valor de acuerdo al tipo de argumento.
                        arg.TryCastValue(value);
                    }
                    else
                    {
                        // En caso contrario asignar el que se ha proveído por el usuario.
                        arg.Value = value;
                    }

                    // Lo segundo, es registrar la variable en la lista de variables para
                    // que si algún sustitución por token lo requiera, la pueda encontrar.
                    this.Variables.Add($"args.{arg.Name}", value);
                }
            }

            // Transferir el control a la función principal para continuar la apertura
            // del reporte.
            this.Open(report);
        }

        /// <summary>
        /// Configura un reporte como el reporte activo a procesar y las variables del reporte.
        /// </summary>
        /// <param name="report">Información del reporte.</param>
        public void Open(ReportHandler report)
        {
            // Activa el reporte.
            this.ActiveReport = report;

            // Configurar la información del reporte.
            this.ConfigureReportVariable("report.name", this.ActiveReport.Name);
            this.ConfigureReportVariable("report.label", this.ActiveReport.Label);
            this.ConfigureReportVariable("report.home", this.ActiveReport.Homepage);
            this.ConfigureReportVariable("report.description", this.ActiveReport.Description);
            this.ConfigureReportVariable("report.version", this.ActiveReport.Version);

            // Modificar el SQL en base a las sustituciones presentes.
            foreach (var sqlCommand in this.ActiveReport.Sql)
            {
                // Cargar el SQL del archivo en la variable Script cuando sea necesario.
                if (!string.IsNullOrEmpty(sqlCommand.FileName))
                {
                    // Buscar archivo.
                    var fileName = Path.Combine(this.ActiveReport.WorkDirectory, sqlCommand.FileName);

                    if (File.Exists(fileName))
                    {
                        sqlCommand.Script = File.ReadAllText(fileName);
                    }
                }

                if (sqlCommand.Tokens == null)
                {
                    break;
                }

                foreach (var token in sqlCommand.Tokens)
                {
                    // Verificar si se encuentra en la lista de argumentos.
                    var requiredArgs = token.RequiredArguments;
                    var enableToken = this.Variables.Count(x => requiredArgs.Contains(x.Key.Replace("args.", string.Empty))) == requiredArgs.Count();

                    if (enableToken)
                    {
                        // Los campos para efectuar la sustitución se encuentran presentes, así que modificaré el SQL.
                        sqlCommand.Script = sqlCommand.Script.Replace($"%{token.Key}%", token.Script);

                        // Obligar a que esos campos sean agregados a la consulta como parámetros en caso de no encontrarse ya.
                        var requiredArgumentsUpdated = sqlCommand.RequiredArguments.ToList();
                        var argumentsNotFound = token.RequiredArguments.Except(requiredArgumentsUpdated).ToList();

                        if (argumentsNotFound.Count() > 0)
                        {
                            // Agregar a la lista los elementos nuevos.
                            requiredArgumentsUpdated.AddRange(argumentsNotFound);

                            // Asignar nuevamente la lista de argumentos actualizada.
                            sqlCommand.RequiredArguments = requiredArgumentsUpdated.ToArray();
                        }
                    }
                    else
                    {
                        // Modificaré el SQL para evitar que el token lo invalide.
                        sqlCommand.Script = sqlCommand.Script.Replace($"%{token.Key}%", string.Empty);
                    }
                }
            }

            // Configura los parámetros requeridos en caso de no existir.
            foreach (var x in this.ActiveReport.ArgumentList)
            {
                // Si el valor no ha sido asignado aún.
                if (x.Value == null)
                {
                    // Recuperar la variable que sea requerida.
                    var variable = this.Variables.FirstOrDefault(c => c.Key == $"args.{x.Name}");

                    // Recuperar el valor predeterminado una vez que ha sido sustituído en caso
                    // de contener variables de sistema.
                    var m = this.ReplaceVariable(x.DefaultValue);

                    // La variable no fue pasada. Intentaré asignar el valor predeterminado.
                    if (variable.Key == null)
                    {
                        x.TryCastValue(m);
                    }
                    else
                    {
                        x.TryCastValue(variable.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el resultado de la operación de acuerdo al tipo de reporte e incluye información sobre el archivo.
        /// </summary>
        /// <param name="type">Identificador del tipo de generador.</param>
        /// <returns>Resultado de la operación.</returns>
        public ReportResults Process(Guid type)
        {
            var generator = this.generators.Single(x => x.Id == type);

            // Obtener los datos de la base de datos.
            var data = this.GenerateData();

            return new ReportResults
            {
                FileExtension = generator.FileExtension,
                MimeType = generator.MimeType,
                Data = generator.GetAllBytes(this.ActiveReport, data, this.Variables),
            };
        }

        /// <summary>
        /// Obtiene los resultados de la operación de acuerdo al tipo de reporte.
        /// </summary>
        /// <param name="type">Identificador del tipo de generador.</param>
        /// <returns>Secuencia de los resultados del reporte.</returns>
        public string GetString(Guid type)
        {
            var generator = this.generators.Single(x => x.Id == type);

            // Obtener los datos de la base de datos.
            var data = this.GenerateData();

            return generator.GetString(this.ActiveReport, data, this.Variables);
        }

        /// <summary>
        /// Obtiene los resultados de la operación de acuerdo al tipo de reporte.
        /// </summary>
        /// <param name="generator">Generador de reportes.</param>
        /// <returns>Secuencia de los resultados del reporte.</returns>
        public string GetString(IGeneratorEngine generator)
        {
            // Obtener los datos de la base de datos.
            var data = this.GenerateData();

            return generator.GetString(this.ActiveReport, data, this.Variables);
        }

        /// <summary>
        /// Devuelve el resultado de la operación de acuerdo al tipo de reporte e incluye información sobre el archivo.
        /// </summary>
        /// <param name="type">Identificador del tipo de generador.</param>
        /// <param name="data">Resultados a insertar en el reporte.</param>
        /// <returns>Resultado de la operación.</returns>
        public ReportResults Process(Guid type, List<DataTable> data)
        {
            var generator = this.generators.Single(x => x.Id == type);

            return new ReportResults
            {
                FileExtension = generator.FileExtension,
                MimeType = generator.MimeType,
                Data = generator.GetAllBytes(this.ActiveReport, data, this.Variables),
            };
        }

        private void CheckConnection()
        {
            // Abrir en caso de que se encuentre cerrada.
            if (this.connection.State == SystemData.ConnectionState.Closed)
            {
                this.connection.Open();
            }
        }

        private void ConfigureReportVariable(string name, object value)
        {
            if (this.Variables.ContainsKey(name))
            {
                this.Variables[name] = value;
            }
            else
            {
                this.Variables.Add(name, value);
            }
        }
        private DataTable GetDataTable(string tableName, string sql, ReportArgumentItem[] reportArguments)
        {
            // Asegurarse que la conexión está abierta y se puede ejecutar la consulta.
            this.CheckConnection();
            var dataTable = new DataTable(tableName);

            // Ejecutar la consulta.
            using (var cmd = this.connection.CreateCommand())
            {
                cmd.CommandText = sql;

                foreach (var argument in reportArguments)
                {
                    // Crear el parámetro.
                    var parameterInfo = cmd.CreateParameter();

                    // Asignar el nombre y valor del parámetro.
                    parameterInfo.ParameterName = $"{this.parameterPreffix}{argument.Name}";
                    parameterInfo.Value = argument.Value;

                    cmd.Parameters.Add(parameterInfo);
                }

                using (var ds = cmd.ExecuteReader(SystemData.CommandBehavior.Default))
                {
                    if (ds != null)
                    {
                        while (ds.Read())
                        {
                            if (dataTable.Columns.Count == 0)
                            {
                                // Agregar la lista de columnas.
                                for (var ci = 0; ci < ds.FieldCount; ci++)
                                {
                                    dataTable.Columns.Add(new DataColumn(ds.GetName(ci), ds.GetFieldType(ci)));
                                }
                            }

                            // Crear la fila.
                            var dataRow = new DataRow();

                            // Agregar datos a la fila.
                            for (var i = 0; i < ds.FieldCount; i++)
                            {
                                dataRow.Add(ds.GetName(i), ds.GetValue(i), ds.GetFieldType(i));
                            }

                            // Agregar la fila a la tabla.
                            dataTable.Add(dataRow);
                        }
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Devuelve una cadena con los valores sustituídos que vienen de las variables.
        /// </summary>
        /// <param name="input">Cadena de entrada.</param>
        /// <returns>Cadena sustituída.</returns>
        private string ReplaceVariable(string input)
        {
            foreach (var variable in this.Variables)
            {
                if (variable.Value != null)
                {
                    input = input.Replace($"%{variable.Key}%", variable.Value.ToString());
                }
            }

            return input;
        }
    }
}