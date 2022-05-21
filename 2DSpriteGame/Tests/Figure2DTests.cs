using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2DSpriteGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameLib.Tests
{
    [TestClass()]
    public class Figure2DTests
    {

        [TestMethod()]
        public void SetPositionTest()
        {
            Figure2D figure = new Figure2D();
            figure.SetPosition(1, 1);
            Assert.IsTrue(figure.Position.X == 1);
        }

        [TestMethod()]
        public void FallDownTest()
        {
            Figure2D figure = new Figure2D();
            figure.SetPosition(1, 1);
            figure.FallDown();
            Assert.IsTrue(figure.Position.Y == 2);
        }

        [TestMethod()]
        public void FastFallDownTest()
        {
            Figure2D figure = new Figure2D();
            figure.SetPosition(1, 1);
            figure.FastFallDown();
            Assert.IsTrue(figure.Position.Y == 1.2f);
        }


        [TestMethod()]
        public void LeftTest()
        {
            Figure2D figure = new Figure2D();
            figure.SetPosition(1, 1);
            figure.Left();
            Assert.IsTrue(figure.Position.X == 0);
        }

        [TestMethod()]
        public void RightTest()
        {

            Figure2D figure = new Figure2D();
            figure.SetPosition(1, 1);
            figure.Right();
            Assert.IsTrue(figure.Position.X == 2);
        }
    }
}