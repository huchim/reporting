// -----------------------------------------------------------------------
// <copyright file="DataRow.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Obtiene la información de la columna.
    /// </summary>
    public class DataRow
    {
        /// <summary>
        /// Obtiene o establece la lista de campos de la fila.
        /// </summary>
        public ICollection<DataColumn> Columns { get; set; } = new List<DataColumn>();

        /// <summary>
        /// Devuelve la información sobre la columna.
        /// </summary>
        /// <param name="columnIndex">Índice de la columna.</param>
        /// <returns>Información de la columna.</returns>
        public DataColumn this[int columnIndex]
        {
            get
            {
                return this.Columns.ElementAt(columnIndex);
            }
        }

        /// <summary>
        /// Devuelve la información sobre la columna.
        /// </summary>
        /// <param name="columnName">Nombre de la columna.</param>
        /// <returns>Información de la columna.</returns>
        public DataColumn this[string columnName]
        {
            get
            {
                return this.Columns.SingleOrDefault(x => x.Name == columnName);
            }
        }

        /// <summary>
        /// Agrega una nueva columna a la fila.
        /// </summary>
        /// <param name="columnName">Nombre de la columna.</param>
        /// <param name="value">Valor de la columna.</param>
        /// <param name="columnType">Tipo de datos de la columna.</param>
        /// <returns>Columna agregada.</returns>
        public DataColumn Add(string columnName, object value, Type columnType)
        {
            var column = new DataColumn(columnName, columnType)
            {
                Value = value,
            };

            this.Columns.Add(column);

            return column;
        }
    }
}