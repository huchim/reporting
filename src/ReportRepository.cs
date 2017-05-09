// -----------------------------------------------------------------------
// <copyright file="ReportRepository.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Administra el listado de reportes.
    /// </summary>
    public class ReportRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReportRepository"/>.
        /// </summary>
        /// <param name="reportRepository">Directorio de donde se cargarán los reportes.</param>
        public ReportRepository(string reportRepository)
        {
            this.LoadReports(reportRepository);
        }

        /// <summary>
        /// Obtiene o establece la ruta donde se encuentran los reportes cargados.
        /// </summary>
        public string HomeDirectory { get; set; }

        /// <summary>
        /// Obtiene la lista de reportes.
        /// </summary>
        public ReportHandler[] Reports { get; private set; }

        /// <summary>
        /// Obtiene o establece la carpeta donde se procesan los resultados.
        /// </summary>
        public string WorkDirectory { get; set; }

        /// <summary>
        /// Devuelve el reporte con el identificado solicitado.
        /// </summary>
        /// <param name="id">Identificador del tipo de reporte.</param>
        /// <returns>Información del reporte.</returns>
        public ReportHandler GetReport(string id)
        {
            return this.Reports.SingleOrDefault(x => x.Name == id);
        }

        /// <summary>
        /// Carga la lista de repotes del sistema.
        /// </summary>
        /// <param name="reportRepository">Directorio de donde se cargarán los reportes.</param>
        public void LoadReports(string reportRepository)
        {
            this.HomeDirectory = reportRepository;

            // Buscar la lista de reportes disponibles.
            var allFiles = Directory.GetFiles(this.HomeDirectory, "report.json", SearchOption.AllDirectories);
            var reportList = new List<ReportHandler>();

            foreach (var fileName in allFiles)
            {
                // Recuperar el contenido del archivo.
                var jsonData = this.LoadJsonData(fileName);

                // Asignar el área de trabajo del reporte. Su carpeta.
                var report = this.ParseReport(jsonData);
                report.WorkDirectory = Path.Combine(this.HomeDirectory, report.Name);

                // Agregar a la lista.
                reportList.Add(report);
            }

            this.Reports = reportList.ToArray();
        }

        private string LoadJsonData(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        private ReportHandler ParseReport(string reportData)
        {
            return JsonConvert.DeserializeObject<ReportHandler>(reportData);
        }
    }
}