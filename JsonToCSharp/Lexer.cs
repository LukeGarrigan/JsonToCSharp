using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{

    public class Lexer
    {
        
        private readonly string input;

        private readonly char[] jsonSynax = {'{', '}', '[', ']', ':', ','};
        
        
        public Lexer(string input)
        {
            this.input = input;
        }

        public IEnumerable<object> Lex()
        {
            var output = new List<object>();

            var currentInput = input;

            while (currentInput.Any())
            {
                var stringResult  = LexString(currentInput);
                if (stringResult != "")
                {
                    currentInput = currentInput.Substring(stringResult.Length + 2);
                    output.Add(stringResult);
                    continue;
                }

                var numberResult = LexNumber(currentInput);

                if (numberResult != "")
                {
                    currentInput = currentInput.Substring(numberResult.Length);
                    output.Add(int.Parse(numberResult));
                    continue;
                }

                if (jsonSynax.Any(e => e == currentInput[0]))
                {
                    output.Add(currentInput[0]);
                    currentInput = currentInput.Substring(1);
                }
            }
            return output;
        }

        private string LexNumber(string currentInput)
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

        private string LexString(string currentInput)
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