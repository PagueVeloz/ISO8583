using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ISO8583.Tests
{
    [TestClass]
    public class UtilTests
    {
        [TestMethod]
        public void Util_ShouldBuildBinaryBitmap()
        {
            var expected = "0010001000111000000000000000000100000000110000010000000000001000";
            var generated = Util.BuildBinaryBitmap(3, 7, 11, 12, 13, 32, 41, 42, 48, 61);

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        public void Util_ShouldBuildHexaBitmap()
        {
            var expected = "2238000100C10008";
            var generated = Util.BuildHexaBitmap(3, 7, 11, 12, 13, 32, 41, 42, 48, 61);

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        public void Util_ShouldConvertBinaryToHexa()
        {
            var expected = "2238000100C10008";
            var generated = Util.ConvertBinaryToHexa("0010001000111000000000000000000100000000110000010000000000001000");

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        public void Util_ShouldConvertHexaToBinary()
        {
            var expected = "0010001000111000000000000000000100000000110000010000000000001000";
            var generated = Util.ConvertHexaToBinary("2238000100C10008");

            Assert.AreEqual(expected, generated);
        }

        [TestMethod]
        public void Util_ReadActiveBitsFromBinaryBitmap()
        {
            var expected = new short[] { 3, 7, 11, 12, 13, 32, 41, 42, 48, 61 };
            var generated = Util.ReadActiveBitsFromBinaryBitmap("0010001000111000000000000000000100000000110000010000000000001000");

            Assert.AreEqual(expected.Length, generated.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], generated[i]);
            }
        }

        [TestMethod]
        public void Util_ReadActiveBitsFromHexaBitmap()
        {
            var expected = new short[] { 3, 7, 11, 12, 13, 32, 41, 42, 48, 61 };
            var generated = Util.ReadActiveBitsFromHexaBitmap("2238000100C10008");

            Assert.AreEqual(expected.Length, generated.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], generated[i]);
            }
        }
    }
}
