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
            var result = Parse(true);
            Output = result;
        }

        private string Parse(bool root = false)
        {
            var currentToken = tokens.First();

            var output = "";

            if (root && !currentToken.Equals('{'))
            {
                throw new Exception("Root must be an object");
            }

            if (currentToken.Equals('{'))
            {
                tokens = tokens.Skip(1);
                
                output += ParseObject();
            }
            else
            {

                tokens = tokens.Skip(1);
                return currentToken.ToString();
            }

            return output += "}";
        }

        private string ParseObject()
        {
            var output = $"public class {nextClassName ?? "Root"} {{ ";
            
            var currentToken = tokens.First();

            if (currentToken == "}")
            {
                return output + "}";
            }

            while (true)
            {
                var jsonKey = tokens.First();
                this.nextClassName = jsonKey.ToString();
                tokens = tokens.Skip(2); // also skip colon
                
                
                var jsonValue = Parse();
                output += $"public string {jsonKey} {{ get; set; }}";
                
                if (tokens.First().Equals('}'))
                {
                    tokens = tokens.Skip(1);
                    break;
                }
            }
            return output;
        }
    }
}