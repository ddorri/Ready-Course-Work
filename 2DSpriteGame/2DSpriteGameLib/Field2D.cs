using GameLib;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameLib
{
    public class Field2D : Field, I2DGameObject
    {
        string _name;
        public string Name { get => _name; }
        Size2 _size;
        public Size2 Size { get => _size; }

        private Size2 _square_size;
        public Size2 Square_size { get => _square_size; }

        protected Vector2 _position;
        public Vector2 Position { get => _position; }
        public Field2D(int col_num, int row_num, Size2 size, string name, Vector2 position) : base(col_num, row_num)
        {
            _name = name;
            _size = size;
            _square_size.Width = _size.Width / _col_num;
            _square_size.Height = _size.Height / _row_num;
            _position = position;
        }
        public Field2D(int col_num, int row_num, int width, string name, Vector2 position) : base(col_num, row_num)
        {
            _name = name;
            _square_size.Width = width / _col_num;
            _square_size.Height = _square_size.Width;
            _size = new Size2(width, _square_size.Height * _row_num);
            _position = position;
        }

        public bool SetFigure(Figure2D figure, out int col_del)
        {
            float posX = figure.Position.X;
            int posY = Convert.ToInt32(Math.Floor(figure.Position.Y));
            col_del = 0;
            for (int i = posY; i < figure.Dimension.Height + posY && i < _row_num; i++)
            {
                for (double j = posX; j < figure.Dimension.Width + posX && j < _col_num; j++)
                {
                    if (i < 0) return false;
                    if (figure.vertexs[i - posY, Convert.ToInt32(Math.Floor(j - posX))] != 0)
                    {
                        _grid[i, Convert.ToInt32(Math.Floor(j))] = figure.vertexs[i - posY, Convert.ToInt32(Math.Floor(j - posX))];
                    }
                }
            }
            if (figure is FigureDecorator)
            {
                switch (((ISuperPower)figure).superPower)
                {
                    case SP.rowDeleting:
                        {
                            for (int i = 0; i < figure.Dimension.Height; i++)
                            {
                                DeleteRow(posY + i, out int col_d);
                                col_del += col_d;
                            }
                            break;
                        }
                    case SP.colorDeliting:
                        {
                            int color = DefinePopularColor();
                            DeleteColor(color, out col_del);
                            break;
                        }
                }
            }
            return true;
        }
    }
}
