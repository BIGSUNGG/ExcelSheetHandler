using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ExcelSheetHandler;

namespace Unit_Test
{
    [TestClass]
    public class SerializeTest
    {
        [TestMethod]
        public void RowDataDeserializeTest()
        {
            var row = new SheetRowData();
            row.SetStringData("Name", "Alice");
            row.SetIntData("Count", 3);
            row.SetFloatData("Rate", 1.5f);
            row.SetBoolData("Active", true);

            var json = JsonConvert.SerializeObject(row);
            var clone = JsonConvert.DeserializeObject<SheetRowData>(json);

            Assert.IsNotNull(json);
            Assert.IsNotNull(clone);
            Assert.AreEqual("Alice", clone.GetStringData("Name"));
            Assert.AreEqual(3, clone.GetIntData("Count"));
            Assert.AreEqual(1.5f, clone.GetFloatData("Rate"));
            Assert.AreEqual(true, clone.GetBoolData("Active"));
        }

        [TestMethod]
        public void RowDataSerializeTest()
        {
            var row = new SheetRowData();
            row.SetStringData("K1", "V1");

            var json = JsonConvert.SerializeObject(row);

            Assert.IsTrue(json.Contains("\"StringData\""));
            Assert.IsTrue(json.Contains("K1"));
            Assert.IsTrue(json.Contains("V1"));
        }
    }
}


