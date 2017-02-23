using System.Collections.Generic;

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
        public ReceiveMessage(string content)
        {
            _fields = new Dictionary<short, string>();

            content = content.Substring(2);

            _type = content.Substring(0, 4);
            content = content.Remove(0, 4);

            _bitmap = content.Substring(0, 16);
            content = content.Remove(0, 16);
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
        /// Get the bits that are present in the message.
        /// </summary>
        /// <returns>An array with the present bits.</returns>
        public short[] GetPresentBits()
        {
            return Util.ReadActiveBitsFromHexaBitmap(_bitmap);
        }
    }
}
