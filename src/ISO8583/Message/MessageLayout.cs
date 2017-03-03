using System;
using System.Collections.Generic;

namespace ISO8583.Message
{
    /// <summary>
    /// Represents the layout of a message.
    /// </summary>
    public class MessageLayout
    {
        private string _type;
        private IDictionary<short, FieldLayout> _fields;

        /// <summary>
        /// Instantiante a new message layout.
        /// </summary>
        public MessageLayout(string type)
        {
            _type = type;
            _fields = new Dictionary<short, FieldLayout>();
        }

        /// <summary>
        /// The type of the message.
        /// </summary>
        public string Type => _type;

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="size">The size of the field.</param>
        public void AddField(short bit, int size, DataType dataType = DataType.N)
        {
            _fields.Add(bit, new FieldLayout(size, FieldType.FIX, dataType));
        }

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="type">The type of the field.</param>
        public void AddField(short bit, FieldType type, DataType dataType = DataType.N)
        {
            if (type == FieldType.FIX)
            {
                throw new InvalidOperationException("The type FIX must have a size.");
            }

            _fields.Add(bit, new FieldLayout(null, type, dataType));
        }

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="size">The size of the field.</param>
        /// <returns>The message layout to use fluently.</returns>
        public MessageLayout AppendField(short bit, int size, DataType dataType = DataType.N)
        {
            AddField(bit, size, dataType);

            return this;
        }

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="type">The type of the field.</param>
        /// <returns>The message layout to use fluently.</returns>
        public MessageLayout AppendField(short bit, FieldType type, DataType dataType = DataType.N)
        {
            AddField(bit, type, dataType);

            return this;
        }

        /// <summary>
        /// Get a field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <returns>The field layout of the respective bit.</returns>
        public FieldLayout GetField(short bit)
        {
            if (_fields.ContainsKey(bit))
            {
                return _fields[bit];
            }

            return null;
        }
    }
}
