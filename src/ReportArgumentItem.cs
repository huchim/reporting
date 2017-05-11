// -----------------------------------------------------------------------
// <copyright file="ReportArgumentItem.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Información del argumento requerido para el reporte.
    /// </summary>
    public class ReportArgumentItem
    {
        /// <summary>
        /// Obtiene o establece el nombre del argumento.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción que será mostrada al usuario.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la etiqueta del argumento.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de valor del argumento.
        /// </summary>
        [JsonProperty("type")]
        public string ValueType { get; set; }

        /// <summary>
        /// Obtiene o establece el origen de los datos.
        /// </summary>
        /// <remarks>
        /// En caso de existir, el campo será una lista desplegable con los valores de <see cref="DataSource"/>.
        /// </remarks>
        [JsonProperty("source")]
        public string DataSource { get; set; }

        /// <summary>
        /// Obtiene o establece el valor predeterminado del campo.
        /// </summary>
        /// <remarks>
        /// Puede usar variables como %system.user.claims.*% (%system.user.claims.id%) o %system.now%
        /// </remarks>
        [JsonProperty("value")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el campo estará oculto para el usuario.
        /// </summary>
        [JsonProperty("hidden")]
        public bool IsHidden { get; set; }

        /// <summary>
        /// Obtiene o establece el valor del argumento.
        /// </summary>
        [JsonIgnoreAttribute]
        public object Value { get; set; }

        /// <summary>
        /// Establece el valor del argumento de acuerdo al tipo especificado.
        /// </summary>
        /// <param name="value">Valor del argumento.</param>
        public void TryCastValue(object value)
        {
            var isString = value is string;

            // Unificar el valor resultante.
            if (value == DBNull.Value)
            {
                value = null;
            }

            if (isString && this.ValueType == "text")
            {
                this.Value = value;
                return;
            }

            if (isString && this.ValueType == "numeric")
            {
                if (float.TryParse(value.ToString(), out float m))
                {
                    this.Value = m;
                }

                return;
            }

            if (isString && this.ValueType == "date")
            {
                if (DateTime.TryParse(value.ToString(), out DateTime m))
                {
                    this.Value = m;
                }

                return;
            }

            this.Value = value;
        }
    }
}