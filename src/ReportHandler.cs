// -----------------------------------------------------------------------
// <copyright file="ReportHandler.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Administra la información del reporte.
    /// </summary>
    public class ReportHandler
    {
        /// <summary>
        /// Obtiene o establece el nombre del reporte.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece el ícono que será mostrado al usuario.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Obtiene o establece la etiqueta que será mostrada al usuario.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción que será mostrada al usuario.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el número de versión semántica.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Obtiene o establece las palabras usadas para buscar por palabras clave.
        /// </summary>
        /// <remarks>
        /// Ayuda a que este reporte sea encontrado sin que el usuario conozca su nombre.
        /// </remarks>
        [JsonProperty("keywords")]
        public string[] Keywords { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de campos adicionales al formato.
        /// </summary>
        /// <remarks>Es útil para agregar opciones a los generadores de reportes.</remarks>
        [JsonExtensionData]
        public Dictionary<string, object> Options { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de autores del reporte.
        /// </summary>
        [JsonProperty("authors")]
        public string[] Authors { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de roles que pueden visualizar este reporte.
        /// </summary>
        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        /// <summary>
        /// Obtiene o establece la URL que muestra más información sobre el reporte.
        /// </summary>
        [JsonProperty("homepage")]
        public Uri Homepage { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de sentencias SQL a ejecutar
        /// </summary>
        [JsonProperty("sql")]
        public ReportSqlItem[] Sql { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el reporte puede ser visto por usuarios no identificados.
        /// </summary>
        [JsonProperty("private")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el reporte se puede ejecutar sin intervenció obligatorio del usuario.
        /// </summary>
        [JsonProperty("autorun")]
        public bool IsAutoRun { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de argumentos disponibles para el reporte.
        /// </summary>
        [JsonProperty("args")]
        public ReportArgumentItem[] ArgumentList { get; set; }

        /// <summary>
        /// Obtiene o establece la carpeta donde se procesan los resultados.
        /// </summary>
        [JsonIgnore]
        public string WorkDirectory { get; set; }
    }
}