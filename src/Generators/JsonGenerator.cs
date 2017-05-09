// -----------------------------------------------------------------------
// <copyright file="JsonGenerator.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Jaguar.Reporting.Common;
    using Newtonsoft.Json;

    /// <summary>
    /// Genera la información en formato CSV.
    /// </summary>
    public class JsonGenerator : IGeneratorEngine
    {
        /// <inheritdoc/>
        public string FileExtension => ".json";

        /// <inheritdoc/>
        public Guid Id => new Guid("f25be034-e1a0-42ee-a804-a3dcdd1d3b03");

        /// <inheritdoc/>
        public bool IsEmbed => false;

        /// <inheritdoc/>
        public string MimeType => "application/javascript";

        /// <inheritdoc/>
        public string Name => "Archivo JSON (Para programadores)";

        /// <inheritdoc/>
        public byte[] GetAllBytes(ReportHandler report, List<DataTable> data, Dictionary<string, object> variables)
        {
            return Encoding.UTF8.GetBytes(this.GetString(report, data, variables));
        }

        /// <inheritdoc/>
        public string GetString(ReportHandler report, List<DataTable> data, Dictionary<string, object> variables)
        {
            return JsonConvert.SerializeObject(this.MergeData(data), Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
            });
        }

        private Dictionary<string, List<Dictionary<string, object>>> MergeData(List<DataTable> data)
        {
            var c = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (var table in data)
            {
                // Agregar la llave para poder usarla.
                c.Add(table.TableName, new List<Dictionary<string, object>>());

                // Hacer referencia al nodo.
                var m = c[table.TableName];

                foreach (var dataRow in table.Rows)
                {
                    var row = new Dictionary<string, object>();

                    foreach (var column in dataRow.Columns)
                    {
                        if (column.Value is string)
                        {
                            column.Value = column.Value.ToString();
                        }

                        if (column.Value is DBNull)
                        {
                            column.Value = null;
                        }

                        row.Add(column.Name, column.Value);
                    }

                    m.Add(row);
                }
            }

            return c;
        }
    }
}