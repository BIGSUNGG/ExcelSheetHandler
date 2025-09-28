using Microsoft.VisualStudio.Tools.Applications.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataHandler.Inject
{
    /// <summary>
    /// DataSet의 데이터를 다른 클래스에 주입하는 클래스
    /// </summary>
    public class DataInjector
    {
        public static DataInjector Instance { get; private set; } = new DataInjector();

        /// <summary>
        /// First Key : 클래스 타입
        /// Second Key : 클래스의 프로퍼티 이름
        /// Value : 클래스의 프로퍼티 필드
        /// </summary>
        Dictionary<Type, Dictionary<string, FieldInfo>> _dataFields = new Dictionary<Type, Dictionary<string, FieldInfo>>();

        /// <summary>
        /// RawData의 값을 target 오브젝트에게 주입
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="target"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Inject(DataSet rowData, object target)
        {
            if (_dataFields.TryGetValue(target.GetType(), out var fields) == false)
                RegisterType(target.GetType(), out fields);

            foreach(var data in rowData.StringData)
            {
                if(fields.TryGetValue(data.Key, out var field) == false)
                    throw new InvalidOperationException($"{target.GetType().Name} type has no {data.Key} set field");

                if(data.Value.Count > 1)
                    field.SetValue(target, data.Value);
                else
                    field.SetValue(target, data.Value.First());
            }

            foreach(var data in rowData.IntData)
            {
                if(fields.TryGetValue(data.Key, out var field) == false)
                    throw new InvalidOperationException($"{target.GetType().Name} type has no {data.Key} set field");

                if(data.Value.Count > 1)
                    field.SetValue(target, data.Value);
                else
                    field.SetValue(target, data.Value.First());
            }

            foreach(var data in rowData.FloatData)
            {
                if(fields.TryGetValue(data.Key, out var field) == false)
                    throw new InvalidOperationException($"{target.GetType().Name} type has no {data.Key} set field");

                if(data.Value.Count > 1)
                    field.SetValue(target, data.Value);
                else
                    field.SetValue(target, data.Value.First());
            }

            foreach(var data in rowData.BoolData)
            {
                if(fields.TryGetValue(data.Key, out var field) == false)
                    throw new InvalidOperationException($"{target.GetType().Name} type has no {data.Key} set field");

                if(data.Value.Count > 1)
                    field.SetValue(target, data.Value);
                else
                    field.SetValue(target, data.Value.First());
            }
        }

        /// <summary>
        /// T 객체 생성 후 데이터 주입
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public T Inject<T>(DataSet rowData) where T : new()
        {
            T created = new T();
            Inject(rowData, created);
            return created;
        }

        /// <summary>
        /// 데이터 주입이 가능하도록 객체 타입 등록
        /// </summary>
        /// <param name="type">등록할 타입</param>
        /// <param name="searchedFields">타입의 필드</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ItemAlreadyInCacheException"></exception>
        void RegisterType(Type type, out Dictionary<string, FieldInfo> searchedFields)
        {
            if (_dataFields.ContainsKey(type))
                throw new InvalidOperationException($"{type.Name} type's fields are already searched");

            searchedFields = new Dictionary<string, FieldInfo>();
            foreach(var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                string propertyName = ConvertNameFieldToProperty(field.Name);

                if (searchedFields.ContainsKey(propertyName))
                    throw new ItemAlreadyInCacheException($"{type.Name} type's {propertyName} field already added");

                searchedFields.Add(propertyName, field);
            }

            _dataFields.Add(type, searchedFields);
        }

        /// <summary>
        /// 필드 이름을 프로퍼티 이름으로 변환
        /// </summary>
        /// <param name="">변환할 필드 이름</param>
        /// <returns>변환된 프로퍼티 이름</returns>
        string ConvertNameFieldToProperty(string fieldName)
        {
            // <name>k__BackingField와 같은 이름에서 name 추출
            int endIndex = fieldName.IndexOf('>');
            var propertyName = fieldName.Substring(1, endIndex - 1);
            return propertyName;
        }
    }
}
