using System.Collections.Generic;

namespace Soa.Helpers
{
    public class PropertiesHelper
    {

        public static Dictionary<string, object> GetProperties(params string[] p)
        {
            var result = new Dictionary<string, object>();
            foreach (var s in p)
            {
                var k = s.Split(',')[0];
                var v = s.Split(',')[1];
                result.Add(k, v);
            }
            return result;
        }
    }
}
