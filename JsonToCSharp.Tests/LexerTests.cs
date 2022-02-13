﻿using System.Linq;
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
    }
}