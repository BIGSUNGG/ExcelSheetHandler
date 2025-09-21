using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExcelSheetHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Unit_Test
{
    [TestClass]
    public class SerializationTest
    {
        /// <summary>
        /// SheetRawData와 ExampleClassA 간의 호환성 테스트를 실행합니다.
        /// </summary>    
        [TestMethod]
        public void SheetRowDataToClassTest()
        {
            try
            {
                // 1. SheetRawData에 데이터 설정
                var sheetData = new SheetRawData();
                sheetData.SetStringData("Name", "ABC");
                sheetData.SetIntData("Id", 1);
                sheetData.SetFloatData("Price", 99.99f);
                sheetData.SetBoolData("IsActive", true);

                // 2. SheetRawData를 JSON으로 직렬화
                string json = SheetRawDataJsonConvert.SerializeData(sheetData);

                Trace.WriteLine($"SheetRawData JSON 직렬화 결과:\n\n{json}");

                // 3. 같은 JSON을 ExampleClassA로 역직렬화
                var classA = JsonConvert.DeserializeObject<ExampleClass>(json);

                // 4. 결과 확인
                Trace.WriteLine($"역직렬화 결과 : {JsonConvert.SerializeObject(classA)}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }

        /// <summary>
        /// JSON을 SheetRawData로 역직렬화하는 테스트를 실행합니다.
        /// </summary>
        [TestMethod]
        public void JsonToSheetDataTest()
        {
            try
            {
                // 1. SheetRawData에 데이터 설정
                var deserializeTargetSheetData = new SheetRawData();
                deserializeTargetSheetData.SetStringData("Name", "TestUser");
                deserializeTargetSheetData.SetIntData("Id", 1);
                deserializeTargetSheetData.SetFloatData("Price", 99.99f);
                deserializeTargetSheetData.SetBoolData("IsActive", true);

                string testJson = SheetRawDataJsonConvert.SerializeData(deserializeTargetSheetData);
                Trace.WriteLine($"테스트용 JSON:\n{testJson}");

                // 2. JSON을 SheetRawData로 역직렬화
                var serializedSheetData = SheetRawDataJsonConvert.DeserializeData(testJson);

                // 3. 결과 확인
                string result = "SheetRawData 역직렬화 결과:\n\n";
                result += $"Name: {serializedSheetData.GetStringData("Name")}\n";
                result += $"Id: {serializedSheetData.GetIntData("Id")}\n";
                result += $"Price: {serializedSheetData.GetFloatData("Price")}\n";
                result += $"IsActive: {serializedSheetData.GetBoolData("IsActive")}\n";

                Trace.WriteLine(result);

                // 4. 검증
                Assert.AreEqual("TestUser", serializedSheetData.GetStringData("Name"));
                Assert.AreEqual(1, serializedSheetData.GetIntData("Id"));
                Assert.AreEqual(99.99f, serializedSheetData.GetFloatData("Price"));
                Assert.AreEqual(true, serializedSheetData.GetBoolData("IsActive"));
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"DeserializeTest 오류: {ex}");
                Assert.Fail();
            }
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