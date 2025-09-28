using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ZeroFormatter;

namespace DataHandler
{
    /// <summary>
    /// 데이터 정보 모음
    /// </summary>
    [ZeroFormattable]
    public class DataSet
    {
        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(0)]
        public virtual Dictionary<string, List<string>> StringData { get; set; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(1)]
        public virtual Dictionary<string, List<int>> IntData { get; set; } = new Dictionary<string, List<int>>();

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(2)]
        public virtual Dictionary<string, List<float>> FloatData { get; set; } = new Dictionary<string, List<float>>();

        /// <summary>
        /// Key : 변수 이름
        /// Value : 변수 값
        /// </summary>
        [Index(3)]
        public virtual Dictionary<string, List<bool>> BoolData { get; set; } = new Dictionary<string, List<bool>>();
    }
}