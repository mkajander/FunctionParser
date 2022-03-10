using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FunctionParser.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestBasicParsing()
    {
        var parser = new FunctionParser();
        var result = parser.Parse("10^2+2*2+12");
        Assert.AreEqual(116, result);
        Assert.Pass();
    }
    [Test]
    public void TestParsingWithStringValuePairs()
    {
        var parser = new FunctionParser();
        Dictionary<string, Decimal> values = new Dictionary<string, Decimal>();
        values.Add("tappio", 10);
        values.Add("kipu", 3);
        values.Add("liike voitto", 5000);
        values.Add("nahka", 2);
        var result = parser.Parse("tappio*kipu + liike voitto ^nahka+12/2", values);
        Assert.AreEqual(25000036, result);
        Assert.Pass();
    }
}