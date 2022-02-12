using System.Linq;
using ConsoleApp1;
using FluentAssertions;
using NUnit.Framework;

namespace JsonToCsharp.Tests
{
    public class LexerTests
    {
        
        [Test]
        public void Should_Return_String_Tokens()
        {
            var input = "{\"name\":\"Luke\"}";

            var lex = new Lexer(input);
            
            var lexResult = lex.Lex();
            lexResult.Count().Should().Be(5);

            lexResult.ElementAt(0).Should().Be('{');
            lexResult.ElementAt(1).Should().Be("name");
            lexResult.ElementAt(2).Should().Be(':');
            lexResult.ElementAt(3).Should().Be("Luke");
            lexResult.ElementAt(4).Should().Be('}');
        }

        [Test]
        public void Should_Return_Number_Token()
        {
            var input = "{\"age\":5}";
            
            var lex = new Lexer(input);
            var lexResults = lex.Lex();

            lexResults.Count().Should().Be(5);
            lexResults.ElementAt(0).Should().Be('{');
            lexResults.ElementAt(1).Should().Be("age");
            lexResults.ElementAt(2).Should().Be(':');
            lexResults.ElementAt(3).Should().Be(5);
            lexResults.ElementAt(4).Should().Be('}');
        }

    }
}