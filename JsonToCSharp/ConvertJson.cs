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
        
        public static string ToCSharp(string json)
        {
            json = json.Trim();
            if (json.ElementAt(0) != '{' || json.ElementAt(json.Length - 1) != '}')
            {
                throw new Exception("Invalid JSON");
            }

            json = json.Substring(1, json.Length - 2);

            json = RemoveAllWhitespace(json);


            var result  = new Lexer(json);

            var parser = new Parser(result.Tokens);

            return parser.Output;
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
        
    }
}