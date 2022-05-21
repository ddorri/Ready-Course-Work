using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Tests
{
    [TestClass()]
    public class FieldTests
    {

        [TestMethod()]
        public void DeleteRowTest()
        {
            Field field = new Field(4, 4);
            field.Grid = new int[,] { { 0, 0, 2, 2 }, { 1, 1, 0, 0 }, { 1, 3, 3, 3 }, { 1, 1, 1, 0 } };
            field.DeleteRow(0,out int test);
            int expected = 2;
            Assert.IsTrue(expected == test);
        }

        [TestMethod()]
        public void DefinePopularColorTest()
        {
            Field field = new Field(4, 4);
            field.Grid = new int[,] { { 0, 0, 2, 2 }, { 1, 1, 0, 0 }, { 1, 3, 3, 3 }, { 1, 1, 1, 0 } };
            int test = field.DefinePopularColor();
            int expected = 1;
            Assert.IsTrue(expected == test);
        }

        [TestMethod()]
        public void DeleteColorTest()
        {
            Field field = new Field(4, 4);
            field.Grid = new int[,] { { 0, 0, 2, 2 }, { 1, 1, 0, 0 }, { 1, 3, 3, 3 }, { 1, 1, 1, 0 } };
            field.DeleteColor(3, out int test);
            int expected = 3;
            Assert.IsTrue(expected == test);
        }
    }
}