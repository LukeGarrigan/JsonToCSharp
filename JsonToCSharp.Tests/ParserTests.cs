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
            var input = "{\"name\":\"Luke\"}";

            var lexer = new Lexer(input);

            Assert.DoesNotThrow(() => new Parser(lexer.Tokens));
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
        public void Should_Return_Two_Props()
        {
            var input = "{\"name\":\"Luke\",\"lastName\":\"Garrigan\"}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            parser.Output.Should().Be("public class Root { public string name { get; set; }public string lastName { get; set; }}");
        }
        
        [Test]
        public void Should_Return_Multiple_Props()
        {
            var input = "{\"name\":\"Luke\",\"lastName\":\"Garrigan\",\"nickName\":\"whoop\"}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            parser.Output.Should().Be("public class Root { public string name { get; set; }public string lastName { get; set; }public string nickName { get; set; }}");
        }

        [Test]
        public void Should_Parse_Nested_Object()
        {
            var input = "{\"name\":\"Luke\",\"address\":{\"street\":\"123 Main St\",\"city\":\"Anywhere\",\"state\":\"CA\"}}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            
            parser.Output.Should().Be("public class address { public string street { get; set; }public string city { get; set; }public string state { get; set; }}public class Root { public string name { get; set; }public string address { get; set; }}");
        }

    }
}