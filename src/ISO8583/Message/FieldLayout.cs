using static ISO8583.Message.Enums;

namespace ISO8583.Message
{
    /// <summary>
    /// Represents the layout of a message field.
    /// </summary>
    public class FieldLayout
    {
        /// <summary>
        /// Instantiate a new field layout.
        /// </summary>
        /// <param name="size">The size of the field.</param>
        /// <param name="type">The type of the field.</param>
        public FieldLayout(int? size, FieldType type, DataType dataType)
        {
            Size = size;
            Type = type;
            DataType = dataType;
        }

        /// <summary>
        /// The size of the field.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// The type of the field.
        /// </summary>
        public FieldType Type { get; set; }

        /// <summary>
        /// The data type of the field.
        /// </summary>
        public DataType DataType { get; set; }
    }
}
