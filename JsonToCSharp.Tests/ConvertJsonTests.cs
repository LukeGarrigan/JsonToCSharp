using ConsoleApp1;
using FluentAssertions;
using NUnit.Framework;

namespace JsonToCsharp.Tests
{
    public class ConvertJsonTests
    {

        [Test]
        public void Should_Convert_Json_To_CSharp()
        {
            var json = "{\"Person\":{\"Name\":{\"FirstName\":\"Luke\"}}}";
            var csharp = json.ToCSharp();
            
            AssertIgnoreSpaces(csharp, "public class Name { public string FirstName { get; set; }}public class Person { public Name Name { get; set; }}public class Root { public Person Person { get; set; }}");
        }
        
        private void AssertIgnoreSpaces(string parserOutput, string s)
        {
            RemoveAllWhitespace(parserOutput).Should().Be(RemoveAllWhitespace(s));
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