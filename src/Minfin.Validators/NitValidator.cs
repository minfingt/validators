using System;
using System.Text.RegularExpressions;

namespace Minfin.Validators
{
    /// <summary>
    ///     Clase que contiene rutinas para validaciones relacionadas al Numero de Identificación
    ///     Tributaria (NIT).
    /// </summary>
    public class NitValidator : INitValidator
    {
        private static Regex expression = new Regex("^[0-9]+[kK]?$", RegexOptions.Compiled);

        /// <summary>
        ///     Verifica que el Numero de Identificación Tributaria (NIT) posea un formato válido.
        /// </summary>
        /// <param name="nit">
        ///     Número de Identificación Tributaria.
        /// </param>
        /// <returns>
        ///     <c>true</c> si el NIT posee el formato #######K o <c>false</c> en caso contrario.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Si el parámetro <paramref name="nit"/> es nulo.
        /// </exception>
        public bool HasValidFormat(string nit)
        {

            if (nit == null)
                throw new ArgumentNullException("nit");

            return expression.IsMatch(nit);

        }

        /// <summary>
        ///     Verifica que el Número de Identificación Tributaria (NIT) sea valido según el
        ///     algoritmo del complemento 11.
        /// </summary>
        /// <param name="nit">
        ///     Número de Identificación Tributaria.
        /// </param>
        /// <returns>
        ///     Retorna <c>true</c> si el NIT es valido o <c>false</c> en caso contrario.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Si el parámetro <paramref name="nit"/> es nulo
        /// </exception>
        /// <exception cref="System.FormatException">
        ///     Si el parámetro <paramref name="nit"/> no posee el formato #######K (Utilice el 
        ///     método HasValidFormat si solo quiere validar el formato).
        /// </exception>
        public bool IsValid(string nit)
        {

            if (nit == null)
                throw new ArgumentNullException("nit");

            if (!HasValidFormat(nit))
                throw new FormatException("El NIT debe tener el formato #######K.");

            int separatorIndex = nit.Length - 1;
            var number = nit.Substring(0, separatorIndex);
            var expectedCheker = nit.Substring(separatorIndex).ToLower();

            var factor = number.Length + 1;
            var total = 0;

            for (int i = 0; i < separatorIndex; i++)
            {

                string character = number.Substring(i, 1);
                int digit = int.Parse(character);

                total += (digit * factor);
                factor = factor - 1;

            }

            var modulus = (11 - (total % 11)) % 11;

            var computedChecker = (modulus == 10 ? "k" : modulus.ToString());

            return expectedCheker == computedChecker;

        }

    }
}
