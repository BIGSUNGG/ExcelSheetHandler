using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExcelSheetHandler
{
    public class SheetRowData
    {
        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [JsonProperty("StringData")]
        [JsonRequired]
        private Dictionary<string, string> _stringData = new Dictionary<string, string>();
        [JsonIgnore]
        public IEnumerable<KeyValuePair<string, string>> StringData => _stringData;

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [JsonProperty("IntData")]
        [JsonRequired]
        private Dictionary<string, int> _intData = new Dictionary<string, int>();
        [JsonIgnore]
        public IEnumerable<KeyValuePair<string, int>> IntData => _intData;

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [JsonProperty("FloatData")]
        [JsonRequired]
        private Dictionary<string, float> _floatData = new Dictionary<string, float>();
        [JsonIgnore]
        public IEnumerable<KeyValuePair<string, float>> FloatData => _floatData;
        
        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [JsonProperty("BoolData")]
        [JsonRequired]
        private Dictionary<string, bool> _boolData = new Dictionary<string, bool>();
        [JsonIgnore]
        public IEnumerable<KeyValuePair<string, bool>> BoolData => _boolData;


        /// <summary>
        /// Key : 타입 이름
        /// Value : SetData 함수 액션(변수 이름, 변수 값)
        /// </summary>
        [JsonIgnore]
        private Dictionary<string, Action<string, dynamic>> _setActions = new Dictionary<string, Action<string, dynamic>>();

        public SheetRowData()
        {
            _setActions.Add("string", (string name, dynamic value)=>SetStringData(name, value));
            _setActions.Add("int", (string name, dynamic value)=>SetIntData(name, (int)value));
            _setActions.Add("float", (string name, dynamic value)=>SetFloatData(name, (float)value));
            _setActions.Add("bool", (string name, dynamic value)=>SetBoolData(name, (bool)value));
        }

        public void SetData(string type, string name, dynamic value)
        {
            if(_setActions.TryGetValue(type, out Action<string, dynamic> action))
                action(name, value);
            else
                throw new NotSupportedException($"Type {type} not implemented");
        }

        public void SetStringData(string name, string value)
        {
            if(_stringData.ContainsKey(name))
                throw new InvalidOperationException($"Key {name} already exists");

            _stringData.Add(name, value);
        }

        public void SetIntData(string name, int value)
        {
            if(_intData.ContainsKey(name))
                throw new InvalidOperationException($"Key {name} already exists");

            _intData.Add(name, value);
        }

        public void SetFloatData(string name, float value)
        {
            if(_floatData.ContainsKey(name))
                throw new InvalidOperationException($"Key {name} already exists");

            _floatData.Add(name, value);
        }
        
        public void SetBoolData(string name, bool value)
        {
            if(_boolData.ContainsKey(name))
                throw new InvalidOperationException($"Key {name} already exists");

            _boolData.Add(name, value);
        }
        
        public string GetStringData(string name)
        {
            if(_stringData.TryGetValue(name, out string value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public int GetIntData(string name)
        {
            if(_intData.TryGetValue(name, out int value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public float GetFloatData(string name)
        {
            if(_floatData.TryGetValue(name, out float value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        
        public bool GetBoolData(string name)
        {
            if(_boolData.TryGetValue(name, out bool value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
    }
}