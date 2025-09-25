using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExcelSheetHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Unit_Test
{
    [TestClass]
    public class InjectTest
    {
        /// <summary>
        /// SheetRawData와 ExampleClassA 간의 호환성 테스트를 실행합니다.
        /// </summary>    
        [TestMethod]
        public void InjectToExampleClassTest()
        {
            var raw = new SheetRowData();
            raw.SetIntData("Id", 123);
            raw.SetFloatData("Price", 45.67f);
            raw.SetStringData("Name", "Sample");
            raw.SetBoolData("IsActive", true);
            raw.SetIntData("HasItemId", 1);
            raw.SetIntData("HasItemId", 2);

            var target = new ExampleClass();

            SheetRowDataInjector.Instance.Inject(raw, target);

            Assert.AreEqual(123, target.Id);
            Assert.AreEqual(45.67f, target.Price);
            Assert.AreEqual("Sample", target.Name);
            Assert.AreEqual(true, target.IsActive);
            Assert.AreEqual(1, target.HasItemId[0]);
            Assert.AreEqual(2, target.HasItemId[1]);
        }
    }

    /// <summary>
    /// SheetRawData에서 직렬화된 JSON을 역직렬화할 수 있는 예제 클래스
    /// </summary>
    public class ExampleClass
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> HasItemId { get; set; }
    }
}