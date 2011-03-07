using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using easyFramework.Sys.ToolLib;

namespace Tests
{
    [TestFixture]
    public class Test_ToolLib
    {
        [Test]
        public void Test_Left()
        {
            Assert.That(Functions.Left("12345", 3), Is.EqualTo("123"));
            Assert.That(Functions.Left("12345", 6), Is.EqualTo("12345"));
            Assert.That(Functions.Replace("12345", "3", "2"), Is.EqualTo("12245"));
        }
    }
}
