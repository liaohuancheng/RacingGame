using CarCommon;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Tool
{
    public class ParamaterTool
    {
        public static T GetParameter<T>(Dictionary<byte, object> parameters, ParamaterCode paramaterCode)
        {
            object o = null;
            object jsonObject = null;
            parameters.TryGetValue((byte)paramaterCode, out jsonObject);
            if(jsonObject == null)
            {
                return default;
            }
            o =JsonMapper.ToObject<T>(jsonObject.ToString());
            return (T)o;
        }
    }
}
