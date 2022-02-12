using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace ConsoleApp1
{

    public static class ConvertJson
    {
        
        // "{"name": "Luke", "lastName": Skywalker"}"
        // "{"address": {"street": "123 Main St", "city": "Anytown", "state": "CA"}}"
        
        public static string ToCSharp(string json)
        {
            json = json.Trim();
            if (json.ElementAt(0) != '{' || json.ElementAt(json.Length - 1) != '}')
            {
                throw new Exception("Invalid JSON");
            }

            json = json.Substring(1, json.Length - 2);

            json = RemoveAllWhitespace(json);


            var lex  = new Lexer(json);

            var result = lex.Lex();
            
            // we have an object here
            //var result = ConvertObjectToCSharp(json, "Root");
            
            return result.ToString();
        }

        private static string RemoveAllWhitespace(string json)
        {
            var output = "";
            for (var i = 0; i < json.Length; i++)
            {
                var currentCharacter = json[i];
                if (currentCharacter == '"')
                {
                    var stringEnd = false;
                    output += currentCharacter;
                    var j = i + 1;
                    while (!stringEnd)
                    {
                        output += json[j];
                        if (json[j] == '"')
                        {
                            stringEnd = true;
                        }
                        j++;
                    }
                    i = j-1;
                    continue;
                }

                if (currentCharacter != ' ')
                {
                    output += currentCharacter;
                }
            }

            return output;
        }

        private static string ConvertObjectToCSharp(string json, string className)
        {
            var output = $"public class {className} {{";
            var attributes = json.Split(",");
            
            for (var i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];
                var keyValue = attribute.Split(":");
                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();
                var dataType = GetDataType(value);
                output += $" public {dataType} {key.Substring(1, key.Length-2)} {{ get; set; }}";
            }

            return output + "}";
        }

        private static string GetDataType(string value)
        {

            if (int.TryParse(value, out int result))
            {
                return "int";
            }

            return "string";
        }

    }
}