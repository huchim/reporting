// -----------------------------------------------------------------------
// <copyright file="ReportSqlToken.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    using Newtonsoft.Json;

    /// <summary>
    /// Representa las cadenas que deben reemplazarse cuando se encuentren presentes las variables.
    /// </summary>
    public class ReportSqlToken
    {
        /// <summary>
        /// Obtiene o establece la clave que será reemplazada dentro del SQL principal.
        /// </summary>
        [JsonProperty("token")]
        public string Key { get; set; }

        /// <summary>
        /// Obtiene o establece el motivo de esta sustitución.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el SQL que será insertado en el SQL principal.
        /// </summary>
        [JsonProperty("script")]
        public string Script { get; set; }

        /// <summary>
        /// Obtiene o establece las variables que deben estar presentes para que ocurra esta sustitución.
        /// </summary>
        [JsonProperty("when")]
        public string[] RequiredArguments { get; set; }
    }
}