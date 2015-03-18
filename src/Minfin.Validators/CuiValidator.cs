using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Minfin.Validators
{
    /// <summary>
    ///     Clase que brinda las rutinas necesarias para verificar que el CUI (Códido Único de 
    ///     Identificación) de un DPI (Documento Personal de Identificación) es válido segun
    ///     las especificaciones de RENAP.
    /// </summary>
    public class CuiValidator : ICuiValidator
    {

        private static Regex expression = new Regex("[0-9]{13}", RegexOptions.Compiled);

        /// <summary>
        ///     Verifica que el formato del CUI sea el correcto.
        /// </summary>
        /// <param name="cui">
        ///     Código Único de Identificación.
        /// </param>
        /// <returns>
        ///     <c>true</c> si el cui cuenta con exactamente 13 dígitos o <c>false</c> en caso contrario.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Si el parámetro <paramref name="cui"/> es nulo.
        /// </exception>
        public bool HasValidFormat(string cui)
        {

            if (cui == null)
                throw new ArgumentNullException("cui");

            return expression.IsMatch(cui);

        }

        /// <summary>
        ///     Verifica que el CUI sea válido según el formato especificado por RENAP.
        /// </summary>
        /// <param name="cui">
        ///     Código Único de Identificación.
        /// </param>
        /// <returns>
        ///     <c>true</c> si el CUI cumple con un formato válido o <c>false</c> en caso contrario.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Si el parámetro <paramref name="cui"/> es nulo.
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Si el parámetro <paramref name="cui"/> no posee exactamente 13 dígitos (Utilice 
        ///     HasValidFormat si solo quiere validar el formato).
        /// </exception>
        public bool IsValid(string cui)
        {

            if (cui == null)
                throw new ArgumentNullException("cui");

            if (!HasValidFormat(cui))
                throw new FormatException("El CUI debe tener exactamente 13 dígitos y no puede contener espacios en blanco, signos de puntuación ni letras.");

            // Se separan los dígitos en grupos
            string cuiNumber = cui.Substring(0, 8);
            string cheker = cui.Substring(8, 1);
            string stateCodeString = cui.Substring(9, 2);
            string cityCodeString = cui.Substring(11, 2);

            // Se obtiene el valor numérico del código de departamento y del código de municipio.
            int stateCode = int.Parse(stateCodeString);
            int cityCode = int.Parse(cityCodeString);
            int checkerCode = int.Parse(cheker);

            // Se asume que la codificación de Municipios y departamentos es la misma que esta
            // publicada en http://www.mineduc.gob.gt/DIGEESP/documents/adecuacionesCurriculares/Documentos%20de%20Apoyo/C%C3%B3digos%20Departamentos-Municipios-Idiomas.pdf

            // Listado actualizado utilizando como fuente http://es.wikipedia.org/wiki/Anexo:Municipios_de_Guatemala

            // Este listado contiene la cantidad de municipios existentes en cada departamento
            // para poder determinar el código máximo aceptado por cada uno de los departamentos.
            int[] stateCityCounts = new int[] 
            { 
                /* 01 - Guatemala:      */ 17 /* municipios. */, 
                /* 02 - El Progreso:    */  8 /* municipios. */, 
                /* 03 - Sacatepéquez:   */ 16 /* municipios. */, 
                /* 04 - Chimaltenango:  */ 16 /* municipios. */, 
                /* 05 - Escuintla:      */ 13 /* municipios. */, 
                /* 06 - Santa Rosa:     */ 14 /* municipios. */, 
                /* 07 - Sololá:         */ 19 /* municipios. */, 
                /* 08 - Totonicapán:    */  8 /* municipios. */, 
                /* 09 - Quetzaltenango: */ 24 /* municipios. */, 
                /* 10 - Suchitepéquez:  */ 21 /* municipios. */, 
                /* 11 - Retalhuleu:     */  9 /* municipios. */, 
                /* 12 - San Marcos:     */ 30 /* municipios. */, 
                /* 13 - Huehuetenango:  */ 32 /* municipios. */, 
                /* 14 - Quiché:         */ 21 /* municipios. */, 
                /* 15 - Baja Verapaz:   */  8 /* municipios. */, 
                /* 16 - Alta Verapaz:   */ 17 /* municipios. */, 
                /* 17 - Petén:          */ 14 /* municipios. */, 
                /* 18 - Izabal:         */  5 /* municipios. */, 
                /* 19 - Zacapa:         */ 11 /* municipios. */, 
                /* 20 - Chiquimula:     */ 11 /* municipios. */, 
                /* 21 - Jalapa:         */  7 /* municipios. */, 
                /* 22 - Jutiapa:        */ 17 /* municipios. */ 
            };

            if (stateCode == 0 || cityCode == 0)
            {
                Trace.TraceInformation("{0}: Código de departamento o de municipio incorrectos", cui);
                return false;
            }

            if (stateCode > stateCityCounts.Length || cityCode > stateCityCounts.Max())
            {
                Trace.TraceInformation("{0}: Código de departamento o de municipio fuera del rango", cui);
                return false;
            }

            if (cityCode > stateCityCounts[stateCode - 1])
            {
                Trace.TraceInformation("{0}: Código de municipio incorrecto", cui);
                return false;
            }

            // Se verifica el correlativo con base en algoritmo del complemento 11.
            int total = 0;

            // notar que se ignora el primer dígito
            for (int i = 0; i < cuiNumber.Length; i++)
            {
                total += cuiNumber[i] * (i + 2);
            }

            int modulus = (total % 11);

            var valid = checkerCode == modulus;

            if (!valid)
            {
                Trace.TraceInformation("{0}: Módulo {1} distinto a {2}", cui, modulus, checkerCode);
            }

            return valid;

        }

    }
}
