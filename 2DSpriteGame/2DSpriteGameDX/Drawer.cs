using _2DSpriteGameLib;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX
{
    public class Drawer
    {
        DX2D _dx2d;
        WindowRenderTarget _target;
        public Drawer(DX2D dX2d, WindowRenderTarget target)
        {
            _dx2d = dX2d;
            _target = target;
        }
        public void DrawField(Field2D field)
        {
            float left = field.Position.X;
            float top = field.Position.Y;
            _target.DrawLine(new Vector2(left, top), new Vector2(left, top + field.Size.Height), _dx2d.WhiteBrush);
            for (float i = left + field.Square_size.Width; i < field.Size.Width + left; i += field.Square_size.Width)
            {
                _target.DrawLine(new Vector2(i, top), new Vector2(i, top + field.Size.Height), _dx2d.SemiLightBrush);
            }
            _target.DrawLine(new Vector2(field.Size.Width + left, top), new Vector2(field.Size.Width + left, top + field.Size.Height), _dx2d.WhiteBrush);
            _target.DrawLine(new Vector2(left, top), new Vector2(left + field.Size.Width, top), _dx2d.WhiteBrush);
            for (float i = top + field.Square_size.Height; i < field.Size.Height + top; i += field.Square_size.Height)
            {
                _target.DrawLine(new Vector2(left, i), new Vector2(left + field.Size.Width, i), _dx2d.SemiLightBrush);
            }

            _target.DrawLine(new Vector2(left, field.Size.Height + top), new Vector2(left + field.Size.Width, field.Size.Height + top), _dx2d.WhiteBrush);


            RectangleF title = new RectangleF();
            string titleText = field.Name;

            title.Width = field.Size.Width;
            title.Height = Convert.ToInt32(_target.Size.Height * 0.1);

            title.X = Convert.ToInt32(field.Position.X);
            title.Y = Convert.ToInt32(field.Size.Height + field.Position.Y + _target.Size.Height * 0.05);

            _target.DrawText(titleText,
                _dx2d.TextFormatScoreTime, title, new SolidColorBrush(_target, new Color(255, 255, 255)));

            FillField(field);
        }
        void FillField(Field2D field)
        {
            for (int i = 0; i < field.Row_num; i++)
            {
                for (int j = 0; j < field.Col_num; j++)
                {
                    if (field.Grid[i, j] != 0)
                    {
                        float left = (j) * field.Square_size.Width + field.Position.X;
                        float top = (i) * field.Square_size.Height + field.Position.Y;
                        SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF(left, top, left + field.Square_size.Width, top + field.Square_size.Height);
                        _target.FillRectangle(rect, new SolidColorBrush(_target, DX2D.Colors[field.Grid[i, j]]));
                    }
                }
            }
        }
        public void FillFigure(Figure2D figure, Field2D field)
        {
            int posX = (int)figure.Position.X;
            int posY = Convert.ToInt32(Math.Floor(figure.Position.Y));

            for (double i = posY; i < figure.Dimension.Height + posY && i < field.Row_num; i++)
            {

                for (int j = posX; j < figure.Dimension.Width + posX && j < field.Col_num; j++)
                {
                    if (figure.vertexs[Convert.ToInt32(Math.Floor(i - posY)), j - posX] != 0 && i >= 0)
                    {
                        float left = (j) * field.Square_size.Width + field.Position.X;
                        double top = (i) * field.Square_size.Height + field.Position.Y;
                        SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF(left, Convert.ToSingle(Math.Floor(top)), left + field.Square_size.Width, Convert.ToInt32(Math.Floor(top + field.Square_size.Height)));
                        _target.FillRectangle(rect, new SolidColorBrush(_target, DX2D.Colors[figure.vertexs[Convert.ToInt32(Math.Floor(i - posY)), j - posX]]));

                    }
                }
            }
        }
    }
}
