using System.Collections.Generic;
using System.Linq;

namespace ISO8583.Message
{
    /// <summary>
    /// Represents a received message ISO8583
    /// </summary>
    public class ReceiveMessage
    {
        private string _type;
        private string _bitmap;
        private IDictionary<short, string> _fields;

        /// <summary>
        /// Instantiate a new message based on the content received.
        /// </summary>
        /// <param name="type">The content received.</param>
        public ReceiveMessage(MessageLayout layout, string content)
        {
            _fields = new Dictionary<short, string>();

            content = content.Substring(2);

            _type = content.Substring(0, 4);
            content = content.Remove(0, 4);

            _bitmap = content.Substring(0, 16);
            content = content.Remove(0, 16);

            foreach (var bit in GetPresentBits())
            {
                var fieldLayot = layout.GetField(bit);
                var size = fieldLayot.Size ?? 0;

                if (fieldLayot.Type != FieldLayout.FieldType.FIX)
                {
                    var headSize = 0;

                    switch (fieldLayot.Type)
                    {
                        case FieldLayout.FieldType.LV:
                            headSize = 1;
                            break;
                        case FieldLayout.FieldType.LLV:
                            headSize = 2;
                            break;
                        case FieldLayout.FieldType.LLLV:
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
    }
}
