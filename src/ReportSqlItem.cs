// -----------------------------------------------------------------------
// <copyright file="ReportSqlItem.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Información sobre la ejecución de SQL.
    /// </summary>
    public class ReportSqlItem
    {
        /// <summary>
        /// Obtiene o establece la rutina SQL a ejecutar.
        /// </summary>
        [JsonProperty("script")]
        public string Script { get; set; }

        /// <summary>
        /// Obtiene o establece el archivo con las rutinas SQL a ejecutar.
        /// </summary>
        [JsonProperty("file")]
        public string FileName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la operación.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de campos obligatorios para correr esta consulta.
        /// </summary>
        [JsonProperty("required")]
        public string[] RequiredArguments { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de campos que modificarán la rutina antes de ser ejecutada.
        /// </summary>
        [JsonProperty("replaces")]
        public ReportSqlToken[] Tokens { get; set; }
    }
}
