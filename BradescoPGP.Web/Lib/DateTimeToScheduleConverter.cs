//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace BradescoPGP.Web
//{
//    public class DateTimeToScheduleConverter : DateTimeConverterBase
//    {
//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            writer.WriteValue(((DateTime)value).ToString("yyyy-MM-dd"));
//        }
//    }
//}