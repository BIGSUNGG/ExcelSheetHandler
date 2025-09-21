using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExcelSheetHandler
{
    /// <summary>
    /// SheetRawData의 JSON 직렬화/역직렬화를 처리하는 클래스
    /// </summary>
    public class SheetRawDataJsonConvert : JsonConverter<SheetRawData>
    {
        private readonly SheetRawData _sheetRawData;

        /// <param name="sheetRawData">역직렬화할 SheetRawData 객체</param>
        /// <exception cref="ArgumentNullException">sheetRawData가 null인 경우</exception>
        public SheetRawDataJsonConvert(SheetRawData sheetRawData)
        {
            _sheetRawData = sheetRawData ?? throw new ArgumentNullException(nameof(sheetRawData));
        }

        public override void WriteJson(JsonWriter writer, SheetRawData value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            // String 데이터 직렬화
            foreach (var kvp in value.StringData)
            {
                writer.WritePropertyName(kvp.Key);
                writer.WriteValue(kvp.Value);
            }

            // Int 데이터 직렬화
            foreach (var kvp in value.IntData)
            {
                writer.WritePropertyName(kvp.Key);
                writer.WriteValue(kvp.Value);
            }

            // Float 데이터 직렬화
            foreach (var kvp in value.FloatData)
            {
                writer.WritePropertyName(kvp.Key);
                writer.WriteValue(kvp.Value);
            }

            // Bool 데이터 직렬화
            foreach (var kvp in value.BoolData)
            {
                writer.WritePropertyName(kvp.Key);
                writer.WriteValue(kvp.Value);
            }

            writer.WriteEndObject();
        }

        public override SheetRawData ReadJson(JsonReader reader, Type objectType, SheetRawData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var sheetRawData = hasExistingValue ? existingValue : new SheetRawData();

            if (reader.TokenType == JsonToken.StartObject)
            {
                var jObject = JObject.Load(reader);

                foreach (var property in jObject.Properties())
                {
                    var propertyName = property.Name;
                    var propertyValue = property.Value;

                    switch (propertyValue.Type)
                    {
                        case JTokenType.String:
                            sheetRawData.SetStringData(propertyName, propertyValue.Value<string>());
                            break;
                        case JTokenType.Integer:
                            sheetRawData.SetIntData(propertyName, propertyValue.Value<int>());
                            break;
                        case JTokenType.Float:
                            sheetRawData.SetFloatData(propertyName, propertyValue.Value<float>());
                            break;
                        case JTokenType.Boolean:
                            sheetRawData.SetBoolData(propertyName, propertyValue.Value<bool>());
                            break;
                        default:
                            // 기타 타입은 문자열로 처리
                            sheetRawData.SetStringData(propertyName, propertyValue.ToString());
                            break;
                    }
                }
            }

            return sheetRawData;
        }

        /// <summary>
        /// SheetRawData 객체를 JSON 문자열로 직렬화합니다.
        /// 딕셔너리의 키-값 쌍이 실제 클래스 멤버처럼 표시됩니다.
        /// 예: {"Name": "ABC", "Id": 1}
        /// </summary>
        /// <param name="sheetRawData">직렬화할 SheetRawData 객체</param>
        /// <returns>JSON 문자열</returns>
        public static string SerializeData(SheetRawData sheetRawData)
        {
            var converter = new SheetRawDataJsonConvert(sheetRawData);
            var settings = new JsonSerializerSettings
            {
                Converters = { converter },
                Formatting = Formatting.Indented
            };
            
            return JsonConvert.SerializeObject(sheetRawData, settings);
        }

        /// <summary>
        /// JSON 문자열을 SheetRawData 객체로 역직렬화합니다.
        /// </summary>
        /// <param name="json">JSON 문자열</param>
        /// <returns>SheetRawData 객체</returns>
        public static SheetRawData DeserializeData(string json)
        {
            var converter = new SheetRawDataJsonConvert(new SheetRawData());
            var settings = new JsonSerializerSettings
            {
                Converters = { converter }
            };
            
            return JsonConvert.DeserializeObject<SheetRawData>(json, settings);
        }

        /// <summary>
        /// List<SheetRawData>를 JSON 문자열로 직렬화합니다.
        /// 각 SheetRawData는 개별 JSON 문자열로 직렬화되어 배열에 포함됩니다.
        /// </summary>
        /// <param name="sheetRawDatas">직렬화할 SheetRawData 리스트</param>
        /// <returns>JSON 문자열</returns>
        public static string SerializeDatas(List<SheetRawData> sheetRawDatas)
        {
            List<string> rowJsons = new List<string>(sheetRawDatas.Count);
            foreach (var rowData in sheetRawDatas)
            {
                rowJsons.Add(SerializeData(rowData));
            }
            
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            
            return JsonConvert.SerializeObject(rowJsons, settings);
        }

        /// <summary>
        /// JSON 문자열을 List<SheetRawData>로 역직렬화합니다.
        /// </summary>
        /// <param name="json">JSON 문자열</param>
        /// <returns>SheetRawData 리스트</returns>
        public static List<SheetRawData> DeserializeDatas(string json)
        {
            List<string> rowJsons = JsonConvert.DeserializeObject<List<string>>(json);
            List<SheetRawData> rowDatas = new List<SheetRawData>(rowJsons.Count);

            var converter = new SheetRawDataJsonConvert(new SheetRawData());
            var settings = new JsonSerializerSettings
            {
                Converters = { converter }
            };

            foreach (var rowJson in rowJsons)
            {
                rowDatas.Add(JsonConvert.DeserializeObject<SheetRawData>(rowJson, settings));
            }
            return rowDatas;
        }
    }
}
