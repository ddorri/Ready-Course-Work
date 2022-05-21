using GameLib;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameLib
{
    public class Figure2D : Figure, I2DGameObject
    {
        protected Vector2 _position;
        public Vector2 Position { get => _position; }
        public Figure2D() : base()
        {
            _color = 1;
        }
        public void SetPosition(int x, int y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public void FallDown()
        {
            _position.Y++;
        }
        public void FastFallDown()
        {
            _position.Y += 0.2f;
        }

        public void Left()
        {
            _position.X--;
        }
        public void Right()
        {
            _position.X++;
        }

    }

    class OFigure2D : Figure2D
    {
        public OFigure2D()
        {
            _vertexs = new int[,] { { 1, 1 }, { 1, 1 } };
            
        }


    }

    class IFigure2D : Figure2D
    {
        public IFigure2D()
        {
            _vertexs = new int[,] { { 1 }, { 1 }, { 1 }, { 1 } };
        }

    }

    class SFigure2D : Figure2D
    {
        public SFigure2D()
        {
            _vertexs = new int[,] { { 0, 1, 1 }, { 1, 1, 0 } };
        }
    }

    class ZFigure2D : Figure2D
    {
        public ZFigure2D()
        {
            _vertexs = new int[,] { { 1, 1, 0 }, { 0, 1, 1 } };
        }


    }

    class LFigure2D : Figure2D
    {
        public LFigure2D()
        {
            _vertexs = new int[,] { { 1, 0 }, { 1, 0 }, { 1, 1 } };
        }
    }

    class JFigure2D : Figure2D
    {
        public JFigure2D()
        {
            _vertexs = new int[,] { { 0, 1 }, { 0, 1 }, { 1, 1 } };
        }
    }

    class TFigure2D : Figure2D
    {
        public TFigure2D()
        {
            _vertexs = new int[,] { { 1, 1, 1 }, { 0, 1, 0 } };
        }
    }
    public abstract class FactoryFigure2D
    {
        public abstract Figure2D FactoryMethod();
    }

    public class FactoryOFigure2D : FactoryFigure2D
    {
        public override Figure2D FactoryMethod() { return new OFigure2D(); }
    }

    public class FactoryIFigure2D : FactoryFigure2D
    {
        public override Figure2D FactoryMethod() { return new IFigure2D(); }
    }

    public class FactorySFigure2D : FactoryFigure2D
    {
        public override Figure2D FactoryMethod() { return new SFigure2D(); }
    }

    public class FactoryZFigure2D : FactoryFigure2D
    {
        public override Figure2D FactoryMethod() { return new ZFigure2D(); }
    }

    public class FactoryLFigure2D : FactoryFigure2D
    {
        public override Figure2D FactoryMethod() { return new LFigure2D(); }
    }

    public class FactoryJFigure2D : FactoryFigure2D
    {
        public override Figure2D FactoryMethod() { return new JFigure2D(); }
    }

    public class FactoryTFigure2D : FactoryFigure2D
    {
        public override Figure2D FactoryMethod() { return new TFigure2D(); }
    }
}
