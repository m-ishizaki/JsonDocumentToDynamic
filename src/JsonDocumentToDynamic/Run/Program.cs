using Rksoftware.JsonDocumentToDynamic;
using System;
using System.Linq;
using System.Text.Json;

namespace Run
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = args.FirstOrDefault();
            Console.WriteLine(json);

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine("null");
                return;
            }

            var expandoObject = JsonToDynamicConverter.Parse(json);

            Console.WriteLine(JsonSerializer.Serialize(expandoObject));
        }
    }
}
