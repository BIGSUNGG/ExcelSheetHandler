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
            var raw = new SheetRawData();
            raw.SetIntData("Id", 123);
            raw.SetFloatData("Price", 45.67f);
            raw.SetStringData("Name", "Sample");
            raw.SetBoolData("IsActive", true);

            var target = new ExampleClass();

            SheetRawDataInjector.Instance.Inject(raw, target);

            Assert.AreEqual(123, target.Id);
            Assert.AreEqual(45.67f, target.Price);
            Assert.AreEqual("Sample", target.Name);
            Assert.AreEqual(true, target.IsActive);
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
    }
}