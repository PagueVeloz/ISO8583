using static ISO8583.Message.Enums;

namespace ISO8583.Message
{
    public class Layouts
    {
        public static MessageLayout TransacaoDeAbertura
        {
            get
            {
                return new MessageLayout("0800")
                    .AppendField(3, 6)
                    .AppendField(7, 10)
                    .AppendField(11, 6)
                    .AppendField(12, 6)
                    .AppendField(13, 4)
                    .AppendField(32, FieldType.LLV)
                    .AppendField(37, 12, DataType.AN)
                    .AppendField(39, 2, DataType.AN)
                    .AppendField(41, 8)
                    .AppendField(42, 15, DataType.AN)
                    .AppendField(48, FieldType.LLLV, DataType.AN)
                    .AppendField(60, FieldType.LV, DataType.AN)
                    .AppendField(61, FieldType.LLLV, DataType.AN)
                    .AppendField(62, FieldType.LLLV, DataType.AN);
            }
        }

        public static MessageLayout TransacaoDeConsulta
        {
            get
            {
                return new MessageLayout("0100")
                    .AppendField(3, 6)
                    .AppendField(4, 12)
                    .AppendField(5, 12)
                    .AppendField(6, 12)
                    .AppendField(7, 10)
                    .AppendField(11, 6)
                    .AppendField(12, 6)
                    .AppendField(13, 4)
                    .AppendField(32, FieldType.LLV)
                    .AppendField(37, 12, DataType.AN)
                    .AppendField(39, 2, DataType.AN)
                    .AppendField(41, 8)
                    .AppendField(42, 15, DataType.AN)
                    .AppendField(48, FieldType.LLLV, DataType.AN)
                    .AppendField(49, 3)
                    .AppendField(60, FieldType.LV, DataType.AN)
                    .AppendField(61, FieldType.LLLV, DataType.AN)
                    .AppendField(62, FieldType.LLLV, DataType.AN);
            }
        }

        public static MessageLayout TransacaoDePagamento
        {
            get
            {
                return new MessageLayout("0200")
                    .AppendField(3, 6)
                    .AppendField(4, 12)
                    .AppendField(5, 12)
                    .AppendField(6, 12)
                    .AppendField(7, 10)
                    .AppendField(11, 6)
                    .AppendField(12, 6)
                    .AppendField(13, 4)
                    .AppendField(32, FieldType.LLV)
                    .AppendField(37, 12, DataType.AN)
                    .AppendField(39, 2, DataType.AN)
                    .AppendField(41, 8)
                    .AppendField(42, 15, DataType.AN)
                    .AppendField(48, FieldType.LLLV, DataType.AN)
                    .AppendField(49, 3)
                    .AppendField(52, 16)
                    .AppendField(60, FieldType.LV, DataType.AN)
                    .AppendField(61, FieldType.LLLV, DataType.AN)
                    .AppendField(62, FieldType.LLLV, DataType.AN)
                    .AppendField(63, FieldType.LLLV, DataType.AN);
            }
        }

        public static MessageLayout TransacaoDeConfirmacao
        {
            get
            {
                return new MessageLayout("0202")
                    .AppendField(3, 6)
                    .AppendField(4, 12)
                    .AppendField(7, 10)
                    .AppendField(11, 6)
                    .AppendField(12, 6)
                    .AppendField(13, 4)
                    .AppendField(32, FieldType.LLV)
                    .AppendField(37, 12, DataType.AN)
                    .AppendField(39, 2, DataType.AN)
                    .AppendField(41, 8)
                    .AppendField(42, 15, DataType.AN)
                    .AppendField(49, 3)
                    .AppendField(61, FieldType.LLLV, DataType.AN);
            }
        }

        public static MessageLayout TransacaoDeDesfazimento
        {
            get
            {
                return new MessageLayout("0420")
                    .AppendField(3, 6)
                    .AppendField(4, 12)
                    .AppendField(7, 10)
                    .AppendField(11, 6)
                    .AppendField(12, 6)
                    .AppendField(13, 4)
                    .AppendField(32, FieldType.LLV)
                    .AppendField(39, 2, DataType.AN)
                    .AppendField(41, 8)
                    .AppendField(42, 15, DataType.AN)
                    .AppendField(61, FieldType.LLLV, DataType.AN)
                    .AppendField(90, 42, DataType.AN)
                    .AppendField(95, FieldType.LLLV, DataType.AN);
            }
        }
    }
}
