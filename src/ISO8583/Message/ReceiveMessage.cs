using System.Collections.Generic;
using System.Linq;
using static ISO8583.Message.Enums;

namespace ISO8583.Message
{
    /// <summary>
    /// Represents a received message ISO8583
    /// </summary>
    public class ReceiveMessage
    {
        private string _type;
        private string _bitmap;
        private string _content;
        private IDictionary<short, string> _fields;

        /// <summary>
        /// Instantiate a new message based on the content received.
        /// </summary>
        /// <param name="layout">The layout of the message.</param>
        /// <param name="content">The content received.</param>
        /// <param name="removeHeader">Indicate the removal or not of the 2 head bits.</param>
        public ReceiveMessage(MessageLayout layout, string content, bool removeHeader = true)
        {
            _fields = new Dictionary<short, string>();

            if (removeHeader)
            {
                content = content.Substring(2);
            }

            _content = content;

            _type = content.Substring(0, 4);
            content = content.Remove(0, 4);
            
            var tamanhoDoMapa = _type == "0420" || _type == "0430" ? 32 : 16;

            _bitmap = content.Substring(0, tamanhoDoMapa);
            content = content.Remove(0, tamanhoDoMapa);

            var presentBits = GetPresentBits();

            foreach (var bit in presentBits.Where(x => x != 1))
            {
                var fieldLayot = layout.GetField(bit);
                var size = fieldLayot.Size ?? 0;

                if (fieldLayot.Type != FieldType.FIX)
                {
                    var headSize = 0;

                    switch (fieldLayot.Type)
                    {
                        case FieldType.LV:
                            headSize = 1;
                            break;
                        case FieldType.LLV:
                            headSize = 2;
                            break;
                        case FieldType.LLLV:
                            headSize = 3;
                            break;
                        default:
                            break;
                    }

                    size = int.Parse(content.Substring(0, headSize));
                    content = content.Remove(0, headSize);
                }

                _fields.Add(bit, content.Substring(0, size));
                content = content.Remove(0, size);
            }
        }

        /// <summary>
        /// Get the bits that are present in the message.
        /// </summary>
        /// <returns>An array with the present bits.</returns>
        private short[] GetPresentBits()
        {
            return Util.ReadActiveBitsFromHexaBitmap(_bitmap).OrderBy(x => x).ToArray();
        }

        /// <summary>
        /// Get the value of a bit.
        /// </summary>
        /// <param name="bit">The bit to return the value.</param>
        /// <returns>The value of the bit or null if the bit isn't present.</returns>
        public string Get(short bit)
        {
            if (_fields.ContainsKey(bit))
            {
                return _fields[bit];
            }

            return null;
        }

        /// <summary>
        /// Get the value of a TLV field of a bit.
        /// </summary>
        /// <param name="bit">The bit to return the value.</param>
        /// <param name="tlv">The code of the TLV field.</param>
        /// <returns>The value of the TLV value or null if the bit isn't present.</returns>
        public string GetTLV(short bit, string tlv)
        {
            var bitValue = Get(bit);

            while (!string.IsNullOrWhiteSpace(bitValue))
            {
                var code = bitValue.Substring(0, 3);
                var size = int.Parse(bitValue.Substring(3, 3));
                var value = bitValue.Substring(6, size);

                if (code == tlv)
                {
                    return value;
                }

                bitValue = bitValue.Remove(0, 6 + size);
            }

            return null;
        }

        /// <summary>
        /// Return the message as string.
        /// </summary>
        /// <returns>A string with the message content.</returns>
        public override string ToString()
        {
            return _content;
        }
    }
}
