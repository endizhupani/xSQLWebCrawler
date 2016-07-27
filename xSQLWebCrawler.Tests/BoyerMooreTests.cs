using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using xSQLWebCrawler.Services;

namespace xSQLWebCrawler.Tests
{
    [TestClass]
    public class BoyerMooreTests
    {
        [TestMethod]
        public void CheckGoodSuffixRuleCase1Table()
        {
            //Arrange
            string pattern = "abbabab";
            int[] expectedShift = { 0, 0, 0, 0, 2, 0, 4, 1};
            int[] expectedWidestBorderPos = { 5, 6, 4, 5, 6, 7, 7, 8 };
            int[] widestBorderPosition = new int[pattern.Length + 1];
            //Act
            int[] shift = StringSearch.GenerateGoodSuffixRuleCase1Table(pattern.ToCharArray(), ref widestBorderPosition);
            //Assert
            for (int i = 0; i < expectedWidestBorderPos.Length; i++)
            {
                Assert.AreEqual(expectedWidestBorderPos[i], widestBorderPosition[i]);
            }
            for (int i = 0; i < expectedShift.Length; i++)
            {
                Assert.AreEqual(expectedShift[i], shift[i]);
            }
            
        }

        [TestMethod]
        public void CheckGoodSuffixRuleCase2Table()
        {
            //Arrange
            string pattern = "abbabab";
            int[] shift1 = { 0, 0, 0, 0, 2, 0, 4, 1 };
            int[] expectedShift = { 5, 5, 5, 5, 2, 5, 4, 1 };
            int[] expectedWidestBorderPos = { 5, 6, 4, 5, 6, 7, 7, 8 };
            //Act
            int[] shift = StringSearch.GenerateGoodSuffixRuleCase2Table(pattern, expectedWidestBorderPos, shift1);
            //Assert
            for (int i = 0; i < expectedShift.Length; i++)
            {
                Assert.AreEqual(expectedShift[i], shift[i]);
            }
            
        }

        [TestMethod]
        public void CheckBoyerMooreSearch() {
            //Arrange
            string text = File.ReadAllText("C:\\Users\\user\\documents\\visual studio 2015\\Projects\\xSQLWebCrawler\\xSQLWebCrawler\\SearchTest.txt");
            string pattern = "EnDiZHupani19940609";
            int expectedIndex = text.ToLower().IndexOf(pattern.ToLower());
            //Act
            int index = StringSearch.BoyerMooreSearch(pattern, text);
            //Assert
            Assert.AreEqual(expectedIndex, index);
        }
    }
}
