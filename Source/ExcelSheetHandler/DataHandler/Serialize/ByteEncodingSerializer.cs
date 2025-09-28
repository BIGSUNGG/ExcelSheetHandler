using System.Collections.Generic;
using ZeroFormatter;

namespace DataHandler.Serialize
{
    public class ByteEncodingSerializer
    {
        public byte[] Serialize(List<DataSet> datas)
        {
            return ZeroFormatterSerializer.Serialize(datas);
        }

        public List<DataSet> Deserialize(byte[] bytes)
        {
            return ZeroFormatterSerializer.Deserialize<List<DataSet>>(bytes);
        }
    }
}


