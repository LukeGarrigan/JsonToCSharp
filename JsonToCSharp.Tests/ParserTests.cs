using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            AssertIgnoreSpaces(parser.Output,"public class Root { public string name { get; set; }}");
        }
        
        [Test]
        public void Should_Return_Two_Props()
        {
            var input = "{\"name\":\"Luke\",\"lastName\":\"Garrigan\"}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output,"public class Root { public string name { get; set; }public string lastName { get; set; }}");
        }
        
        [Test]
        public void Should_Parse_Number()
        {
            var input = "{\"age\":400}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output,"public class Root { public int age { get; set; }}");
        }
        
        [Test]
        public void Should_Parse_Boolean()
        {
            var input = "{\"happy\":true}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output,"public class Root { public bool happy { get; set; }}");
        }
        
        [Test]
        public void Should_Parse_Null()
        {
            var input = "{\"happy\":null}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output,"public class Root { public object happy { get; set; }}");
        }
        
        [Test]
        public void Should_Return_Multiple_Props()
        {
            var input = "{\"name\":\"Luke\",\"lastName\":\"Garrigan\",\"nickName\":\"whoop\"}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output,"public class Root { public string name { get; set; }public string lastName { get; set; }public string nickName { get; set; }}" );
        }

        [Test]
        public void Should_Parse_Nested_Object()
        {
            var input = "{\"name\":\"Luke\",\"address\":{\"street\":\"123 Main St\",\"city\":\"Anywhere\",\"state\":\"CA\"}}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            
            AssertIgnoreSpaces(parser.Output, "public class Address {  public string street { get; set; }public string city { get; set; }public string state { get; set; }}public class Root { public string name { get; set; }public Address address { get; set; }}");
        }
        
        [Test]
        public void Should_Parse_Double_Nested_Object()
        {
            var input = "{\"Person\":{\"Name\":{\"FirstName\":\"Luke\"}}}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            
            AssertIgnoreSpaces(parser.Output, "public class Name { public string FirstName { get; set; }}public class Person { public Name Name { get; set; }}public class Root { public Person Person { get; set; }}");
        }
        

        [Test]
        public void Should_Parse_Object_That_is_Followed_By_Attributes()
        {
            var input = "{\"name\":\"Luke\",\"address\":{\"postcode\":\"pe321da\"},\"favColor\":\"blue\"}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            
            AssertIgnoreSpaces(parser.Output, "  public class Address{ public string postcode {get; set;}}  public class Root {  public string name { get; set; }  public Address address { get; set; }  public string favColor { get; set; }}");
        }
        
        
        [Test]
        public void Should_Parse_Empty_Object()
        {
            var input = "{\"address\":{}}";

            var lexer = new Lexer(input);

            var parser =  new Parser(lexer.Tokens);

            AssertIgnoreSpaces(parser.Output, "public class Root { public Address address { get; set; }}");
        }

        [Test]
        public void Should_Parse_Empty_Array()
        {
            var input = "{\"address\":[]}";

            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output, "public class Root { public List<object> address { get; set; }}");
        }

        [Test]
        public void Should_Parse_Array_Of_Ints()
        {
            var input = "{\"numbers\":[1,2,3]}";
            
            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output, "public class Root { public List<int> numbers { get; set; }}");
        }

        [Test]
        public void Should_Parse_Array_Then_Property()
        {
            var input = "{\"numbers\":[1,2,3],\"age\":27}";
            
            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output, "public class Root { public List<int> numbers { get; set; } public int age { get; set; }}");
        }

        [Test]
        public void Should_Parse_String_Array()
        {
            var input = "{\"names\":[\"Luke\",\"Garrigan\"]}";
            
            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output, "public class Root { public List<string> names { get; set; }}");
        }
        
        [Test]
        public void Should_Parse_Boolean_Array()
        {
            var input = "{\"answers\":[true,false,true]}";
            
            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output, "public class Root { public List<bool> answers { get; set; }}");
        }

        [Test]
        public void Should_Parse_Array_Of_Objects_If_Array_Values_Are_Null()
        {
            var input = "{\"nullers\":[null,null]}";
            
            var lexer = new Lexer(input);

            var parser = new Parser(lexer.Tokens);
            AssertIgnoreSpaces(parser.Output, "public class Root { public List<object> nullers { get; set; }}");
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