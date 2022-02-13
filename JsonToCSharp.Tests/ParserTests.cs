using System;
using System.Collections.Generic;
using ConsoleApp1;
using FluentAssertions;
using NUnit.Framework;

namespace JsonToCsharp.Tests
{
    public class ParserTests
    {

        [Test]
        public void Should_Throw_An_Error_As_Does_Not_Start_With_Opening_Brace()
        {
            var input = new List<object> {'}'};
            Assert.Throws<Exception>(() => new Parser(input));
        }
        
        [Test]
        public void Should_Not_Throw_An_Error_When_It_Does_Start_With_Opening_Brace()
        {
            var input = new List<object> {'{'};
            Assert.DoesNotThrow(() => new Parser(input));
        }

        [Test]
        public void Should_Return_Name_Prop()
        {
            var input = "{\"name\":\"Luke\"}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            parser.Output.Should().Be("public class Root { public string name { get; set; }}");
        }
        
        [Test]
        public void Should_Return_Multiple_Props()
        {
            var input = "{\"name\":\"Luke\",\"age\":\"27\",\"height\":\"2.1\"}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            parser.Output.Should().Be("public class Root { public string name { get; set; }}");
        }
    }
}