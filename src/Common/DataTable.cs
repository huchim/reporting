// -----------------------------------------------------------------------
// <copyright file="DataTable.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Tabla de datos.
    /// </summary>
    public class DataTable
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DataTable"/>.
        /// </summary>
        /// <param name="tableName">Nombre de la tabla.</param>
        public DataTable(string tableName)
            : this()
        {
            this.TableName = tableName ?? string.Empty;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DataTable"/>.
        /// </summary>
        public DataTable()
        {
        }

        /// <summary>
        /// Obtiene o establece la lista de columnas de la tabla.
        /// </summary>
        public ICollection<DataColumn> Columns { get; set; } = new List<DataColumn>();

        /// <summary>
        /// Obtiene un valor que indica si la tabla tiene registros.
        /// </summary>
        public bool HasRows
        {
            get
            {
                return this.Rows == null ? false : this.Rows.Count > 0;
            }
        }

        /// <summary>
        /// Obtiene o establece la lista de registros.
        /// </summary>
        public ICollection<DataRow> Rows { get; set; } = new List<DataRow>();

        /// <summary>
        /// Obtiene o establece el nombre de la tabla.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Agrega una fila a la tabla.
        /// </summary>
        /// <param name="row">Información de la fila.</param>
        /// <returns>Fila agregada.</returns>
        public DataRow Add(DataRow row)
        {
            this.Rows.Add(row);
            return row;
        }
    }
}