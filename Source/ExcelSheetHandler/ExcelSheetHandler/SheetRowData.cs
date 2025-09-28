using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZeroFormatter;
using System.Data;

namespace ExcelSheetHandler
{
    /// <summary>
    /// 시트에서 하나의 행 데이터를 저장하는 클래스
    /// </summary>
    [ZeroFormattable]
    public class SheetRowData : DataHandler.DataSet
    {
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
            if(StringData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                StringData.Add(name, new List<string>(){ value });
        }

        public void SetIntData(string name, int value)
        {
            if(IntData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                IntData.Add(name, new List<int>(){ value });
        }

        public void SetFloatData(string name, float value)
        {
            if(FloatData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                FloatData.Add(name, new List<float>(){ value });
        }
        
        public void SetBoolData(string name, bool value)
        {
            if(BoolData.TryGetValue(name, out var datas))
                datas.Add(value);
            else
                BoolData.Add(name, new List<bool>(){ value });
        }
        
        public List<string> GetStringData(string name)
        {
            if(StringData.TryGetValue(name, out List<string> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public List<int> GetIntData(string name)
        {
            if(IntData.TryGetValue(name, out List<int> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public List<float> GetFloatData(string name)
        {
            if(FloatData.TryGetValue(name, out List<float> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
        
        public List<bool> GetBoolData(string name)
        {
            if(BoolData.TryGetValue(name, out List<bool> value))
                return value;
            throw new InvalidOperationException($"Key {name} not found");
        }
    }
}