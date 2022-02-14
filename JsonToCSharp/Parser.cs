using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

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

        private string Parse(bool root = false)
        {
            var currentToken = tokens.First();

            if (root && !currentToken.Equals('{'))
            {
                throw new Exception("Root must be an object");
            }

            if (currentToken.Equals('{'))
            {
                tokens = tokens.Skip(1);
                ParseObject();
            }
            else
            {

                tokens = tokens.Skip(1);
                return currentToken.ToString();
            }

            return Output += "}";
        }

        private void ParseObject()
        {
            // capitalise first letter of nextClassName
            var output = $"public class {nextClassName ?? "Root"} {{ ";
            
            var currentToken = tokens.First();

            if (currentToken == "}")
            {
                Output += output + "}";
                return;
            }

            while (true)
            {
                var jsonKey = tokens.First();
                this.nextClassName = CapitaliseFirstLetter(jsonKey);
                tokens = tokens.Skip(2); // also skip colon

                var type = GetType();
                Parse();
                output += $"public {type} {jsonKey} {{ get; set; }}";
                
                if (tokens.First().Equals('}'))
                {
                    tokens = tokens.Skip(1);
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
            else if (tokens.First().Equals('{'))
            {
                type = nextClassName;
            }

            return type;
        }

        private static string CapitaliseFirstLetter(object jsonKey)
        {
            return jsonKey.ToString().First().ToString().ToUpper() + jsonKey.ToString().Substring(1);
        }
    }
}