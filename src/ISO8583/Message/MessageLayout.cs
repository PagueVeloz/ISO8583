using System;
using System.Collections.Generic;

namespace ISO8583.Message
{
    /// <summary>
    /// Represents the layout of a message.
    /// </summary>
    public class MessageLayout
    {
        private IDictionary<short, FieldLayout> _fields;

        /// <summary>
        /// Instantiante a new message layout.
        /// </summary>
        public MessageLayout()
        {
            _fields = new Dictionary<short, FieldLayout>();
        }

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="size">The size of the field.</param>
        public void AddField(short bit, int size)
        {
            _fields.Add(bit, new FieldLayout(size, FieldLayout.FieldType.FIX));
        }

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="type">The type of the field.</param>
        public void AddField(short bit, FieldLayout.FieldType type)
        {
            if (type == FieldLayout.FieldType.FIX)
            {
                throw new InvalidOperationException("The type FIX must have a size.");
            }

            _fields.Add(bit, new FieldLayout(null, type));
        }

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="size">The size of the field.</param>
        /// <returns>The message layout to use fluently.</returns>
        public MessageLayout AppendField(short bit, int size)
        {
            AddField(bit, size);

            return this;
        }

        /// <summary>
        /// Insert a new field layout.
        /// </summary>
        /// <param name="bit">The bit of the field.</param>
        /// <param name="type">The type of the field.</param>
        /// <returns>The message layout to use fluently.</returns>
        public MessageLayout AppendField(short bit, FieldLayout.FieldType type)
        {
            AddField(bit, type);

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
