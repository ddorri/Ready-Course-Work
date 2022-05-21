using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DSpriteGameDX
{
    class AfterMenu
    {
        RenderForm _renderForm;
        DX2D _dx2d;
        DInput _dInput;
        WindowRenderTarget _target;
        Size2F _targetSize;
        RectangleF winnerRect;
        RectangleF rec;
        RectangleF okayRect;
        string winnerText;
        string okayText;

        bool _okayClicked;
        bool _okayClickedLast;
        public bool OkayClicked { get => _okayClicked; set => _okayClicked = value; }

        float scale;
        int titleHeight;

        public AfterMenu(RenderForm renderForm, DX2D dx2d, WindowRenderTarget target, Size2F targetSize, DInput dInput)
        {

            scale = target.Size.Width / target.PixelSize.Width;
            _renderForm = renderForm;
            _dx2d = dx2d;
            _dInput = dInput;
            _target = target;
            _targetSize = targetSize;
            _target.Transform = Matrix3x2.Identity * scale;
            _targetSize.Height /= scale;
            _targetSize.Width /= scale;

            winnerRect = new RectangleF();

            winnerRect.Width = Convert.ToInt32(_targetSize.Width * 0.4);
            winnerRect.Height = Convert.ToInt32(_targetSize.Height * 0.20);

            winnerRect.X = Convert.ToInt32(_targetSize.Width / 2 - winnerRect.Width / 2);
            winnerRect.Y = Convert.ToInt32(_targetSize.Height * 0.3);

            okayRect = new RectangleF();

            okayText = "O K A Y";

            okayRect.Width = Convert.ToInt32(_targetSize.Width * 0.4);
            okayRect.Height = Convert.ToInt32(_targetSize.Height * 0.07);

            okayRect.X = Convert.ToInt32(_targetSize.Width / 2 - okayRect.Width / 2);
            okayRect.Y = Convert.ToInt32(winnerRect.Y + winnerRect.Height);

            System.Drawing.Rectangle screenRectangle = _renderForm.RectangleToScreen(_renderForm.ClientRectangle);
            titleHeight = screenRectangle.Top - _renderForm.Top;

            _okayClicked = false;
            _okayClickedLast = false;
        }

        public void Draw(string winner)
        {
            _target.Transform = Matrix3x2.Identity * scale;

            winnerText = "Game Over!\n";
            winnerText += winner;

            _target.FillRectangle(winnerRect, new SolidColorBrush(_target, new Color(255, 255, 255)));

            _target.DrawText(winnerText,
            _dx2d.TextFormatScoreTime, winnerRect, new SolidColorBrush(_target, new Color(0, 0, 0)));


            _target.FillRectangle(okayRect, new SolidColorBrush(_target, new Color(0, 0, 0)));

            _target.DrawText(okayText,
            _dx2d.TextFormatScoreTime, okayRect, new SolidColorBrush(_target, new Color(255, 255, 255)));
        }

        public void OnClick()
        {
            rec = new RectangleF((Cursor.Position.X - ((_renderForm.Location.X < 0) ? 0 : _renderForm.Location.X)), (Cursor.Position.Y - _renderForm.Location.Y) - titleHeight, 20, 20);


            if (rec.Intersects(okayRect))
            {
                if (_dInput.MouseState.Buttons[0])
                {
                    _okayClickedLast = true;
                }
                if (!_dInput.MouseState.Buttons[0] && _okayClickedLast)
                {
                    _okayClicked = true;
                    _okayClickedLast = false;
                }
            }
        }

        public void UpdateAfterMenu(string winner)
        {
            Draw(winner);
            _dInput.UpdateMouseState();
            if (_dInput.MouseUpdated)
            {
                OnClick();
            }
        }
    }
}
