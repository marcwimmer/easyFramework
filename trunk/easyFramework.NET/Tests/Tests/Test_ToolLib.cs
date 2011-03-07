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
            Assert.That(Functions.Split("A B C D", " "), Is.EqualTo(new string[] { "A", "B", "C", "D" }));
            Assert.That(Functions.Split("A B C D", " ", 2 ), Is.EqualTo(new string[] { "A", "B C D" }));
            Assert.That(Functions.Mid("ABC", 2), Is.EqualTo("BC"));
            Assert.That(Functions.Mid("ABC", 2, 1), Is.EqualTo("B"));
            Assert.That(Functions.Mid("ABC", 4), Is.EqualTo(""));
            Assert.That(Functions.Replace("ABCC", "C", "D", 3, 1), Is.EqualTo("ABDC"));
            Assert.That(Functions.Replace("ABCCCC", "C", "D", 3, 2), Is.EqualTo("ABDDCC"));
            Assert.That(Functions.Replace("ABCCCC", "C", "D", 4, 2), Is.EqualTo("ABCDDC"));
            Assert.That(Functions.gs2Digit("2"), Is.EqualTo("02"));
            Assert.That(Functions.gs2Digit(""), Is.EqualTo("00"));
            Assert.That(Functions.gs2Digit("111"), Is.EqualTo("111"));
            Assert.That(Functions.Reverse("ABC"), Is.EqualTo("CBA"));
            Assert.That(Functions.Reverse(""), Is.EqualTo(""));
        }
    }
}
