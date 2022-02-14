using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
                var stringResult  = LexString(input);
                if (stringResult != "")
                {
                    input = input.Substring(stringResult.Length + 2);
                    output.Add(stringResult);
                    continue;
                }

                var numberResult = LexNumber(input);

                if (numberResult != "")
                {
                    input = input.Substring(numberResult.Length);
                    output.Add(int.Parse(numberResult));
                    continue;
                }
                
                if (input.Length > 4 && input.Substring(0, 4) == "true")
                {
                    input = input.Substring(4);
                    output.Add(true);
                    continue;
                }
                
                if (input.Length > 5 && input.Substring(0, 5) == "false")
                {
                    input = input.Substring(5);
                    output.Add(false);
                    continue;
                }

                if (jsonSynax.Any(e => e == input[0]))
                {
                    output.Add(input[0]);
                    input = input.Substring(1);
                }
            }
            Tokens = output;
        }

        private static string LexNumber(string currentInput)
        {
            var numbers = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            var number = "";

            foreach (var character in currentInput)
            {
                if (numbers.Any(n => n == character))
                {

                    number += character;
                }
                else
                {
                    return number;
                }
            }

            return "";
        }
        

        private static string LexString(string currentInput)
        {

            var output = "";
            if (currentInput[0] == '"')
            {
                currentInput = currentInput.Substring(1);
            }
            else
            {
                return "";
            }
            
            for (var i = 0; i < currentInput.Length; i++)
            {
                if (currentInput[i] == '"')
                {
                    return currentInput.Substring(0, i);
                }

                output += currentInput[i];
            }

            return currentInput;
        }
    }
}