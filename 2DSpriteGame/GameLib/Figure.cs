using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public abstract class Figure
    {
        protected Size2 _dimension;
        public Size2 Dimension { get { GetSize(); return _dimension; } }
        protected int[,] _vertexs;
        public int[,] vertexs { get { return _vertexs; } set { _vertexs = value; } }

        protected int _color;
        public int Color { get => _color; }
        public Figure()
        {

        }
        void GetSize()
        {
            _dimension = new Size2(_vertexs.GetLength(1), _vertexs.GetLength(0));
        }
        public void ChangeColor(int color)
        {
            for (int i = 0; i < Dimension.Height; i++)
            {
                for (int j = 0; j < Dimension.Width; j++)
                {
                    if (_vertexs[i, j] != 0) _vertexs[i, j] = color;
                }
            }
            _color = color;
        }
        public void Turn()
        {
            int[,] vertex_trans = new int[Dimension.Width, Dimension.Height];
            for (int i = Dimension.Width - 1, m = 0; i >= 0; i--, m++)
            {
                for (int j = 0, n = 0; j < Dimension.Height; j++, n++)
                {
                    vertex_trans[m, n] = vertexs[j, i];
                }
            }
            _vertexs = vertex_trans;
        }
    }
    
}
