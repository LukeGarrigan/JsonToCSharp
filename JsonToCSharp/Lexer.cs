using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{

    public class Lexer
    {
        private readonly  char[] jsonSynax = {'{', '}', '[', ']', ':', ','};
        public IEnumerable<object> Tokens { get; set; }

        public Lexer(string json)
        {
            Lex(json);
        }

        private void Lex(string input)
        {
            var output = new List<object>();
            while (input.Any())
            {
                if (LexString(input, out var stringOutput))
                {
                    input = input.Substring(stringOutput.Length + 2);
                    output.Add(stringOutput);
                } else if (LexNumber(input, out var numberResult))
                {
                    input = input.Substring(numberResult.Length);
                    output.Add(int.Parse(numberResult));
                } else if (input.Length > 4 && input.Substring(0, 4) == "true")
                {
                    input = input.Substring(4);
                    output.Add(true);
                } else if (input.Length > 5 && input.Substring(0, 5) == "false")
                {
                    input = input.Substring(5);
                    output.Add(false);
                } else if (input.Length > 4 && input.Substring(0, 4) == "null")
                {
                    input = input.Substring(4);
                    output.Add(null);
                } else if (jsonSynax.Any(e => e == input[0])) 
                {
                    output.Add(input[0]);
                    input = input.Substring(1);
                }
            }
            Tokens = output;
        }

        private static bool LexNumber(string currentInput, out string output)
        {
            var numbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            output = "";

            foreach (var character in currentInput)
            {
                if (numbers.Any(n => n == character))
                {

                    output += character;
                }
                else
                {
                    if (output.Length > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        

        private static bool LexString(string currentInput, out string result)
        {
            result = "";
            if (currentInput[0] == '"')
            {
                currentInput = currentInput.Substring(1);
            }
            else
            {
                return false;
            }
            
            for (var i = 0; i < currentInput.Length; i++)
            {
                if (currentInput[i] == '"')
                {
                    return true;
                }

                result += currentInput[i];
            }

            return true;
        }
    }
}