using System;
using ConsoleApp1;
using FluentAssertions;
using NUnit.Framework;

namespace JsonToCsharp.Tests
{
    public class Tests
    {
        
        
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void ShouldReturnOneProperty()
        {
            var input = "{\"name\": \"Luke\"}";

            var result = ConvertJson.ToCSharp(input);
            
            result.Should().Be("public class Root { public string name { get; set; }}");
        }
        
        [Test]
        public void ShouldReturnTwoProperties()
        {
            var input = "{\"name\": \"Luke\", \"lastName\": \"Skywalker\"}";

            var result = ConvertJson.ToCSharp(input);
            
            result.Should().Be("public class Root { public string name { get; set; } public string lastName { get; set; }}");
        }   
        
        [Test]
        public void ShouldCreateAnIntegerAttribute()
        {
            var input = "{\"age\": 27}";

            var result = ConvertJson.ToCSharp(input);
            
            result.Should().Be("public class Root { public int age { get; set; }}");
        }


        [Test]
        public void ShouldCreateAnObjectAttribute()
        {
            var input = "{\"address\": {\"street\": \"123 Main St\", \"city\": \"Anytown\", \"state\": \"CA\"}, \"age\": 27}";

            var result = ConvertJson.ToCSharp(input);
            
            result.Should().Be("public class Root { public Address address { get; set; }} public class Address { public string street { get; set; } public string city { get; set; } public string state { get; set; }}");
        }
    }
}