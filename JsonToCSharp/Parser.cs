using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Parser
    {
        private IEnumerable<object> tokens;
        private string? nextClassName;
        public string Output { get; set; }

        public Parser(IEnumerable<object> tokens)
        {
            this.tokens = tokens;
            Parse(true);
        }

        private void Parse(bool root = false)
        {
            var currentToken = tokens.First();

            if (root && !currentToken.Equals('{'))
            {
                throw new Exception("Root must be an object");
            }

            if (currentToken != null && currentToken.Equals('{'))
            {
                tokens = tokens.Skip(1);
                ParseObject();
            }
            else if (currentToken != null && currentToken.Equals('['))
            {
                tokens = tokens.Skip(1);
                ParseArray();
            }
            else
            {
                tokens = tokens.Skip(1);
            }

         
        }

        private void ParseArray()
        {       
            var initialToken = tokens.First();
            if (initialToken != null && initialToken.Equals(']'))
            {
                tokens = tokens.Skip(1);
                return;
            }

            while (true)
            {
                var value = tokens.First();
                tokens = tokens.Skip(1);

                var currentToken = tokens.First();
                if (currentToken != null && currentToken.Equals(']'))
                {
                    tokens = tokens.Skip(1);
                    return;
                }
            }
        }

        private void ParseObject()
        {
            var output = $"public class {nextClassName ?? "Root"} {{ ";
            
            var firstChar = tokens.First();
            if (firstChar.Equals('}'))
            {
                tokens = tokens.Skip(1);
                return;
            }
            while (true)
            {
                var jsonKey = tokens.First();
                
                nextClassName = CapitaliseFirstLetter(jsonKey);
                tokens = tokens.Skip(2); // also skip colon

                var type = GetType();
                Parse();
                output += $"public {type} {jsonKey} {{ get; set; }}";
                
                if (tokens.First().Equals('}'))
                {
                    tokens = tokens.Skip(1);
                    output += '}';
                    break;
                }
                tokens = tokens.Skip(1); // skips ,
            }

            Output += output;
        }

        private string GetType()
        {
            var type = "string";
            if (tokens.First() is int)
            {
                type = "int";
            }
            else if (tokens.First() is bool)
            {
                type = "bool";
            }
            else if (tokens.First() is null)
            {
                type = "object";
            }
            else if (tokens.First().Equals('{'))
            {
                type = nextClassName;
            }
            else if (tokens.First().Equals('['))
            {
                type = GetArrayType(type);
            }
            return type;
        }

        private string GetArrayType(string type)
        {
            var secondElement = tokens.ToList().ElementAt(1);
            if (secondElement is null || secondElement.Equals(']') || secondElement.Equals('{'))
            {
                type = "List<object>";
            }
            else if (secondElement is int)
            {
                type = "List<int>";
            }
            else if (secondElement is bool)
            {
                type = "List<bool>";
            }
            else if (secondElement is string)
            {
                type = "List<string>";
            }

            return type;
        }

        private static string CapitaliseFirstLetter(object jsonKey)
        {
            return jsonKey.ToString().First().ToString().ToUpper() + jsonKey.ToString().Substring(1);
        }
    }
}