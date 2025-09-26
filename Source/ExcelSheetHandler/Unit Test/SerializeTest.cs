using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ExcelSheetHandler;
using System.Linq;
using System.Collections.Generic;

namespace Unit_Test
{
    [TestClass]
    public class SerializeTest
    {
        [TestMethod]
        public void RowDataSerializeTest()
        {
            var row = new SheetRowData();
            row.SetStringData("K1", "V1");
            var rows = new List<SheetRowData> { row };

            var secretKey = Enumerable.Range(0, 16).Select(i => (byte)i).ToArray();
            var bytes = SheetRowDataSerializer.Instance.Serialize(rows, secretKey);

            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length >= 16 + 1 + 32); // IV + ciphertext(>=1) + HMAC
        }

        [TestMethod]
        public void RowDataDeserializeTest()
        {
            var row = new SheetRowData();
            row.SetStringData("Name", "Alice");
            row.SetIntData("Count", 3);
            row.SetFloatData("Rate", 1.5f);
            row.SetBoolData("Active", true);
            row.SetIntData("HasItemId", 1);
            row.SetIntData("HasItemId", 2);

            var rows = new List<SheetRowData> { row };
            var secretKey = Enumerable.Range(0, 16).Select(i => (byte)i).ToArray();
            var protectedBytes = SheetRowDataSerializer.Instance.Serialize(rows, secretKey);
            var restored = SheetRowDataSerializer.Instance.Deserialize(protectedBytes, secretKey);

            Assert.IsNotNull(restored);
            Assert.AreEqual(1, restored.Count);

            var clone = restored[0];
            Assert.AreEqual("Alice", clone.GetStringData("Name")[0]);
            Assert.AreEqual(3, clone.GetIntData("Count")[0]);
            Assert.AreEqual(1.5f, clone.GetFloatData("Rate")[0]);
            Assert.AreEqual(true, clone.GetBoolData("Active")[0]);
            Assert.AreEqual(1, clone.GetIntData("HasItemId")[0]);
            Assert.AreEqual(2, clone.GetIntData("HasItemId")[1]);
        }
    }
}


