using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO8583
{
    /// <summary>
    /// A set of util methods
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Build a hexa string telling the active bits.
        /// </summary>
        /// <param name="activeBits">The bits to activate.</param>
        /// <returns>An hexa string.</returns>
        public static string BuildHexaBitmap(params short[] activeBits)
        {
            var binary = BuildBinaryBitmap(activeBits);
            var hexa = ConvertBinaryToHexa(binary);

            return hexa;
        }

        /// <summary>
        /// Build a 64 bits binary string activating some bits.
        /// </summary>
        /// <param name="activeBits">The bits to activate.</param>
        /// <returns>A binary string.</returns>
        public static string BuildBinaryBitmap(params short[] activeBits)
        {
            if (activeBits.Any(x => x < 1 || x > 64))
            {
                throw new InvalidOperationException("The bits must be between 1 and 64.");
            }

            var binary = string.Empty.PadRight(64, '0').ToArray();

            foreach (var bit in activeBits)
            {
                binary[bit - 1] = '1';
            }

            return new string(binary);
        }

        /// <summary>
        /// Convert a binary string to an hexa string.
        /// </summary>
        /// <param name="binary">A binary string.</param>
        /// <returns>An hexa string.</returns>
        public static string ConvertBinaryToHexa(string binary)
        {
            int mod = binary.Length % 8;

            if (mod != 0)
            {
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            var result = new StringBuilder(binary.Length / 8 + 1);

            for (int i = 0; i < binary.Length; i += 8)
            {
                result.AppendFormat("{0:X2}", Convert.ToByte(binary.Substring(i, 8), 2));
            }

            return result.ToString();
        }

        /// <summary>
        /// Convert an hexa string to a binary string.
        /// </summary>
        /// <param name="hexa">An hexa string.</param>
        /// <returns>A binary string.</returns>
        public static string ConvertHexaToBinary(string hexa)
        {
            var result = new StringBuilder();

            foreach (var item in hexa.Select(x => x))
            {
                var base16 = Convert.ToInt32(item.ToString(), 16);
                var base02 = Convert.ToString(base16, 2);

                base02 = base02.PadLeft(4, '0');

                result.Append(base02);
            }

            return result.ToString();
        }

        /// <summary>
        /// Read the active bits from an hexa string.
        /// </summary>
        /// <param name="hexa">An hexa string.</param>
        /// <returns>An array with the active bits.</returns>
        public static short[] ReadActiveBitsFromHexaBitmap(string hexa)
        {
            var binary = ConvertHexaToBinary(hexa);
            var bits = ReadActiveBitsFromBinaryBitmap(binary);

            return bits;
        }

        /// <summary>
        /// Read the active bits from an binary string.
        /// </summary>
        /// <param name="binary">An binary string.</param>
        /// <returns>An array with the active bits.</returns>
        public static short[] ReadActiveBitsFromBinaryBitmap(string binary)
        {
            if (binary.Length != 64)
            {
                throw new InvalidOperationException("The binary must have 64 bits.");
            }

            var bits = new List<short>();

            for (short i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1')
                {
                    bits.Add((short)(i + 1));
                }
            }

            return bits.OrderBy(x => x).ToArray();
        }
    }
}
