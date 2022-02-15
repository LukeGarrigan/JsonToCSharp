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

            var lexer = new Lexer(input);

            lexer.Tokens.Count().Should().Be(5);
            lexer.Tokens.ElementAt(0).Should().Be('{');
            lexer.Tokens.ElementAt(1).Should().Be("name");
            lexer.Tokens.ElementAt(2).Should().Be(':');
            lexer.Tokens.ElementAt(3).Should().Be("Luke");
            lexer.Tokens.ElementAt(4).Should().Be('}');
        }
        
        [Test]
        public void Should_Return_Multiple_String_Tokens()
        {
            var input = "{\"name\":\"Luke\",\"nickName\":\"whoop\"}";

            var lexer = new Lexer(input);

            lexer.Tokens.Count().Should().Be(9);
        }
        
                
        [Test]
        public void Should_Return_String_And_Int()
        {
            var input = "{\"name\":\"Luke\",\"nickName\":\"whoop\",\"age\":25}";

            var lexer = new Lexer(input);

            lexer.Tokens.Count().Should().Be(13);
        }
        
        [Test]
        public void Should_Return_Int_Then_String()
        {
            var input = "{\"name\":\"Luke\",\"age\":25,\"nickName\":\"whoop\"}";

            var lexer = new Lexer(input);

            lexer.Tokens.Count().Should().Be(13);
        }

        [Test]
        public void Should_Return_Number_Token()
        {
            var input = "{\"age\":5}";
            var lexer = new Lexer(input);
            lexer.Tokens.Count().Should().Be(5);
            lexer.Tokens.ElementAt(0).Should().Be('{');
            lexer.Tokens.ElementAt(1).Should().Be("age");
            lexer.Tokens.ElementAt(2).Should().Be(':');
            lexer.Tokens.ElementAt(3).Should().Be(5);
            lexer.Tokens.ElementAt(4).Should().Be('}');
        }

        [Test]
        public void Should_Return_Boolean_Token()
        {
            var input = "{\"happy\":true}";
            var lexer = new Lexer(input);
            lexer.Tokens.Count().Should().Be(5);
            lexer.Tokens.ElementAt(0).Should().Be('{');
            lexer.Tokens.ElementAt(1).Should().Be("happy");
            lexer.Tokens.ElementAt(2).Should().Be(':');
            lexer.Tokens.ElementAt(3).Should().Be(true);
            lexer.Tokens.ElementAt(4).Should().Be('}');
        }
        
        [Test]
        public void Should_Return_False_Boolean_Token()
        {
            var input = "{\"happy\":false}";
            var lexer = new Lexer(input);
            lexer.Tokens.Count().Should().Be(5);
            lexer.Tokens.ElementAt(0).Should().Be('{');
            lexer.Tokens.ElementAt(1).Should().Be("happy");
            lexer.Tokens.ElementAt(2).Should().Be(':');
            lexer.Tokens.ElementAt(3).Should().Be(false);
            lexer.Tokens.ElementAt(4).Should().Be('}');
        }
        
                
        [Test]
        public void Should_Parse_Null()
        {
            var input = "{\"happy\":null}";
            var lexer = new Lexer(input);
            lexer.Tokens.Count().Should().Be(5);
            lexer.Tokens.ElementAt(0).Should().Be('{');
            lexer.Tokens.ElementAt(1).Should().Be("happy");
            lexer.Tokens.ElementAt(2).Should().Be(':');
            lexer.Tokens.ElementAt(3).Should().Be(null);
            lexer.Tokens.ElementAt(4).Should().Be('}');
        }

        [Test]
        public void Should_Lex_Nested_Object()
        {
            var input = "{\"name\":\"Luke\",\"address\":{\"street\":\"123 Main St\",\"city\":\"Anywhere\",\"state\":\"CA\"}}";

            var lexer = new Lexer(input);

            lexer.Tokens.Count().Should().Be(21);
        }
    }
}