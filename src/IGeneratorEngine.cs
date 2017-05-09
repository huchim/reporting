// -----------------------------------------------------------------------
// <copyright file="IGeneratorEngine.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    using System;
    using System.Collections.Generic;
    using Jaguar.Reporting.Common;

    /// <summary>
    /// Convierte los resultados de la operación en un formato.
    /// </summary>
    public interface IGeneratorEngine
    {
        /// <summary>
        /// Obtiene el identificador del generador.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Obtiene el nombre del generador.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Obtiene el tipo MIME del archivo.
        /// </summary>
        string MimeType { get; }

        /// <summary>
        /// Obtiene la extensión del archivo.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Obtiene un valor que indica si el generador puede ser visualizado en el navegador.
        /// </summary>
        bool IsEmbed { get; }

        /// <summary>
        /// Devuelve los datos del reporte ya generado.
        /// </summary>
        /// <param name="report">Información del reporte.</param>
        /// <param name="data">Datos para el reporte.</param>
        /// <param name="variables">Lista de parámetros disponibles para el reporte.</param>
        /// <returns>Secuencia de datos para el reporte.</returns>
        byte[] GetAllBytes(ReportHandler report, List<DataTable> data, Dictionary<string, object> variables);

        /// <summary>
        /// Devuelve los datos del reporte ya generado.
        /// </summary>
        /// <param name="report">Información del reporte.</param>
        /// <param name="data">Datos para el reporte.</param>
        /// <param name="variables">Lista de parámetros disponibles para el reporte.</param>
        /// <returns>Secuencia de datos para el reporte.</returns>
        string GetString(ReportHandler report, List<DataTable> data, Dictionary<string, object> variables);
    }
}