using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO8583.Message
{
    /// <summary>
    /// Represents a message ISO8583 to send
    /// </summary>
    public class SendMessage
    {
        private string _type;
        private Encoding _encoding;
        private IDictionary<short, string> _fields;
        private string _built;
        private MessageLayout _layout;

        /// <summary>
        /// Instantiate a new message determining the type.
        /// </summary>
        /// <param name="type">The type of the message. Usually a 4 digit number ("0800").</param>
        public SendMessage(string type)
        {
            _type = type;
            _encoding = Encoding.ASCII;
            _fields = new Dictionary<short, string>();
            _built = null;
        }

        /// <summary>
        /// Instantiate a new message determining the type.
        /// </summary>
        /// <param name="layout">The layout of the message.</param>
        public SendMessage(MessageLayout layout) : this(layout.Type)
        {
            _layout = layout;
        }

        /// <summary>
        /// Instantiate a new message determining the type and encoding.
        /// </summary>
        /// <param name="type">The type of the message. Usually a 4 digit number ("0800").</param>
        /// <param name="encoding">The encoding to use when packing (Pack()) message into bytes to send.</param>
        public SendMessage(string type, Encoding encoding) : this(type)
        {
            _encoding = encoding; ;
        }

        /// <summary>
        /// Instantiate a new message determining the type and encoding.
        /// </summary>
        /// <param name="layout">The layout of the message.</param>
        /// <param name="encoding">The encoding to use when packing (Pack()) message into bytes to send.</param>
        public SendMessage(MessageLayout layout, Encoding encoding) : this(layout.Type, encoding)
        {
            _layout = layout;
        }

        /// <summary>
        /// Insert bit into the message.
        /// </summary>
        /// <param name="bit">The bit to be added.</param>
        /// <param name="value">The value of the bit. Must be in the correct format and size.</param>
        public void Add(short bit, string value)
        {
            if (_layout != null)
            {
                var field = _layout.GetField(bit);

                if (field != null)
                {
                    if (field.Type == FieldType.FIX)
                    {
                        switch (field.DataType)
                        {
                            case DataType.N:
                                value = value.PadLeft(field.Size.Value, '0');
                                break;
                            case DataType.A:
                            case DataType.AN:
                                value = value.PadRight(field.Size.Value, ' ');
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        var length = value.Length.ToString();

                        switch (field.Type)
                        {
                            case FieldType.LV:
                                length = length.PadLeft(1, '0');
                                break;
                            case FieldType.LLV:
                                length = length.PadLeft(2, '0');
                                break;
                            case FieldType.LLLV:
                                length = length.PadLeft(3, '0');
                                break;
                        }

                        value = length + value;
                    }
                }
            }

            _fields.Add(bit, value);
        }

        /// <summary>
        /// Insert bit into the message.
        /// </summary>
        /// <param name="bit">The bit to be added.</param>
        /// <param name="value">The value of the bit. Must be in the correct format and size.</param>
        /// <returns>The message appended to use fluently.</returns>
        public SendMessage Append(short bit, string value)
        {
            Add(bit, value);

            return this;
        }

        /// <summary>
        /// Set or update the encoding of the message.
        /// </summary>
        /// <param name="encoding">The encoding to use when packing (Pack()) message into bytes to send.</param>
        /// <returns>The message encoded to use fluently.</returns>
        public SendMessage Encode(Encoding encoding)
        {
            _encoding = encoding;

            return this;
        }

        /// <summary>
        /// Resolve the "bitmap" of the message and build the structure using type, bitmap and the bits added.
        /// </summary>
        /// <returns>The message built to use fluently.</returns>
        public SendMessage Build()
        {
            var keys = _fields.OrderBy(x => x.Key).Select(x => x.Key).ToArray();

            var builder = new StringBuilder();
            builder.Append(_type);
            builder.Append(Util.BuildHexaBitmap(keys));

            foreach (var key in keys)
            {
                builder.Append(_fields[key]);
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
