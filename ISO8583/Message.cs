using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO8583
{
    public class Message
    {
        private string _type;
        private Encoding _encoding;
        private IDictionary<short, string> _bits;
        private string _built;

        /// <summary>
        /// Instantiate a new message determining the type.
        /// </summary>
        /// <param name="type">The type of the message. Usually a 4 digit number ("0800").</param>
        public Message(string type)
        {
            _type = type;
            _encoding = Encoding.ASCII;
            _bits = new Dictionary<short, string>();
            _built = null;
        }

        /// <summary>
        /// Instantiate a new message determining the type and encoding.
        /// </summary>
        /// <param name="type">The type of the message. Usually a 4 digit number ("0800").</param>
        /// <param name="encoding">The encoding to use when packing (Pack()) message into bytes to send.</param>
        public Message(string type, Encoding encoding) : this(type)
        {
            _encoding = encoding; ;
        }

        /// <summary>
        /// Build a hexa string telling the active bits.
        /// </summary>
        /// <param name="activeBits">The bits to activate.</param>
        /// <returns>An hexa string.</returns>
        private string BuildHexaBitmap(params short[] activeBits)
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
        private string BuildBinaryBitmap(params short[] activeBits)
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
        private string ConvertBinaryToHexa(string binary)
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
        /// Insert bit into the message.
        /// </summary>
        /// <param name="bit">The bit to be added.</param>
        /// <param name="value">The value of the bit. Must be in the correct format and size.</param>
        public void Add(short bit, string value)
        {
            _bits.Add(bit, value);
        }

        /// <summary>
        /// Insert bit into the message.
        /// </summary>
        /// <param name="bit">The bit to be added.</param>
        /// <param name="value">The value of the bit. Must be in the correct format and size.</param>
        /// <returns>The message appended to use fluently.</returns>
        public Message Append(short bit, string value)
        {
            Add(bit, value);

            return this;
        }

        /// <summary>
        /// Set or update the encoding of the message.
        /// </summary>
        /// <param name="encoding">The encoding to use when packing (Pack()) message into bytes to send.</param>
        /// <returns>The message encoded to use fluently.</returns>
        public Message Encode(Encoding encoding)
        {
            _encoding = encoding;

            return this;
        }

        /// <summary>
        /// Resolve the "bitmap" of the message and build the structure using type, bitmap and the bits added.
        /// </summary>
        /// <returns>The message built to use fluently.</returns>
        public Message Build()
        {
            var keys = _bits.OrderBy(x => x.Key).Select(x => x.Key).ToArray();

            var builder = new StringBuilder();
            builder.Append(_type);
            builder.Append(BuildHexaBitmap(keys));

            foreach (var key in keys)
            {
                builder.Append(_bits[key]);
            }

            _built = builder.ToString();

            return this;
        }

        /// <summary>
        /// Return the packed message.
        /// </summary>
        /// <returns>A byte array of the built message ready to send.</returns>
        public byte[] Pack()
        {
            if (_built == null)
            {
                throw new InvalidOperationException("The message must be previously built.");
            }

            var bytes = _encoding.GetBytes(_built).ToList();

            bytes.InsertRange(0, BitConverter.GetBytes((short)bytes.Count));

            return bytes.ToArray();
        }

        /// <summary>
        /// Return the message as string.
        /// </summary>
        /// <returns>A string that represents de built message.</returns>
        public override string ToString()
        {
            if (_built == null)
            {
                throw new InvalidOperationException("The message must be previously built.");
            }

            return _built;
        }
    }
}
