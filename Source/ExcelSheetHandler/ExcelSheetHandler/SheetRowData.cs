using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZeroFormatter;

namespace ExcelSheetHandler
{
    /// <summary>
    /// 시트에서 하나의 행 데이터를 저장하는 클래스
    /// </summary>
    [ZeroFormattable]
    public class SheetRowData
    {
        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(0)]
        private Dictionary<string, List<string>> _stringData = new Dictionary<string, List<string>>();
        [IgnoreFormat]
        public IEnumerable<KeyValuePair<string, List<string>>> StringData => _stringData;

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(1)]
        private Dictionary<string, List<int>> _intData = new Dictionary<string, List<int>>();
        [IgnoreFormat]
        public IEnumerable<KeyValuePair<string, List<int>>> IntData => _intData;

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(2)]
        private Dictionary<string, List<float>> _floatData = new Dictionary<string, List<float>>();
        [IgnoreFormat]
        public IEnumerable<KeyValuePair<string, List<float>>> FloatData => _floatData;

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(3)]
        private Dictionary<string, List<bool>> _boolData = new Dictionary<string, List<bool>>();
        [IgnoreFormat]
        public IEnumerable<KeyValuePair<string, List<bool>>> BoolData => _boolData;

        /// <summary>
        /// Key : 타입 이름
        /// Value : SetData 함수 액션(변수 이름, 변수 값)
        /// </summary>
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
            if(_stringData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                _stringData.Add(name, new List<string>(){ value });
        }

        public void SetIntData(string name, int value)
        {
            if(_intData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                _intData.Add(name, new List<int>(){ value });
        }

        public void SetFloatData(string name, float value)
        {
            if(_floatData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                _floatData.Add(name, new List<float>(){ value });
        }
        
        public void SetBoolData(string name, bool value)
        {
            if(_boolData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                _boolData.Add(name, new List<bool>(){ value });
        }
        
        public List<string> GetStringData(string name)
        {
            if(_stringData.TryGetValue(name, out List<string> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public List<int> GetIntData(string name)
        {
            if(_intData.TryGetValue(name, out List<int> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public List<float> GetFloatData(string name)
        {
            if(_floatData.TryGetValue(name, out List<float> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public List<bool> GetBoolData(string name)
        {
            if(_boolData.TryGetValue(name, out List<bool> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
    }
}