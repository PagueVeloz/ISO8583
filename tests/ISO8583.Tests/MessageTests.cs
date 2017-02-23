using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace ISO8583.Tests
{
    [TestClass]
    public class MessageTests
    {
        private Message CreateMessage()
        {
            return new Message("0800") //Message Type
                .Append(3, "008000") //Process Code
                .Append(7, "0223141530") //GMT MMddHHmmss
                .Append(11, "000001") //NSU Terminal
                .Append(12, "111530") //Local HHmmss
                .Append(13, "0223") //Local MMdd
                .Append(32, "1100000000001") //Institute Code (it's LLV so "11" is the size and "00000000001" is the code)
                .Append(41, "00000001") //Terminal ID
                .Append(42, "A1B2C3D4E5F6G7H") //Establishment Code
                .Append(48, "0071270011006") //Additional data
                .Append(61, "0200SE") //Version
                .Encode(Encoding.ASCII);
        }

        [TestMethod]
        public void Message_ShouldToString()
        {
            var expected = "08002238000100C1000800800002231415300000011115300223110000000000100000001A1B2C3D4E5F6G7H00712700110060200SE";
            var generated = CreateMessage().Build().ToString();

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        public void Message_ShouldPack()
        {
            var expected = 109;
            var generated = CreateMessage().Build().Pack().Length;

            Assert.AreEqual(expected, generated);
        }
    }
}
