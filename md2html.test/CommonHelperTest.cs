using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using md2html;

namespace md2html.test
{
    [TestClass]
    public class CommonHelperTest
    {
        [TestMethod]
        public void IsAbsolutePathTest()
        {
            Assert.IsTrue(CommonHelper.IsAbsolutePath(@"C:\a\b\c"));

            Assert.IsTrue(CommonHelper.IsAbsolutePath(@"C:\"));
            
            Assert.IsFalse(CommonHelper.IsAbsolutePath("./a/b/c"));

            Assert.IsFalse(CommonHelper.IsAbsolutePath("../../a/b/c"));
        }
    }
}
