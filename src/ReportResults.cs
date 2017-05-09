// -----------------------------------------------------------------------
// <copyright file="ReportResults.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    /// <summary>
    /// Información del resultado de un reporte.
    /// </summary>
    public class ReportResults
    {
        /// <summary>
        /// Obtiene o establece el tipo MIME del archivo.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Obtiene o establece la extensión del archivo.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Obtiene o establece la secuencia de datos del reporte.
        /// </summary>
        public byte[] Data { get; set; }
    }
}