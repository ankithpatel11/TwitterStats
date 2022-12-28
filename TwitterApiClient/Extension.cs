using Newtonsoft.Json;

namespace TwitterApiClient
{
    public static class Extension
    {
        public static Rootobject? ToRootObject(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return JsonConvert.DeserializeObject<Rootobject>(str);
            }
            return null;
        }

        //public static JObject ToEnumerableRoot(this StreamReader reader)
        //{
        //    //while (true)
        //    //{
        //        string s = reader.ReadLine();
        //        if (string.IsNullOrWhiteSpace(s))
        //            yield break;
        //        yield return JsonConvert.DeserializeObject<Rootobject>(s);
        //    //}
        //}
    }
}
