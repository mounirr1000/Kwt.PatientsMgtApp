using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace Kwt.PatientsMgtApp.WebUI.Infrastructure
{
    public static class JavaScriptConvert
    {
        public static IHtmlString SerializeObject(object value)
        {
           
            using (TextWriter writer = File.CreateText("LocalJSONFile.JSON"))
            {

                var serializer = new JsonSerializer
                {
                    // Let's use camelCasing as is common practice in JavaScript
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                };
                
                serializer.Serialize(writer, value);
                return new HtmlString(writer.ToString());
            }
            //using (var stringWriter = new StringWriter())
            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //using (var jsonWriter = new JsonTextWriter(sw))
            //{


            //    // We don't want quotes around object names
            //    jsonWriter.QuoteName = false;
            //    var serializer = new JsonSerializer();
            //    serializer.Serialize(jsonWriter, value);
            //    return new HtmlString(sw.ToString());
            //}
        }
    }
}