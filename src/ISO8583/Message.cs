﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO8583
{
    /// <summary>
    /// Represents a message ISO8583
    /// </summary>
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
        /// Get the value of a bit.
        /// </summary>
        /// <param name="bit">The bit to return the value.</param>
        /// <returns>The value of the bit or null if the bit isn't present.</returns>
        public string Get(short bit)
        {
            if (_bits.ContainsKey(bit))
            {
                return _bits[bit];
            }

            return null;
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
            builder.Append(Util.BuildHexaBitmap(keys));

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
