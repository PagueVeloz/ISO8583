using System.Collections.Generic;

namespace ISO8583.Message
{
    public class MessageLayout
    {
        public MessageLayout()
        {
            Fields = new Dictionary<short, Field>();
        }

        public IDictionary<short, Field> Fields { get; set; }

        public void Add(short bit, int size, Field.FieldType type = Field.FieldType.FIX)
        {
            Fields.Add(bit, new Field(size, type));
        }

        public MessageLayout Append(short bit, int size, Field.FieldType type = Field.FieldType.FIX)
        {
            Add(bit, size, type);

            return this;
        }

        public class Field
        {
            public Field(int size, FieldType type)
            {
                Size = size;
                Type = type;
            }

            public int Size { get; set; }
            public FieldType Type { get; set; }

            public enum FieldType
            {
                FIX,
                LLV,
                LLLV
            }
        }
    }
}
