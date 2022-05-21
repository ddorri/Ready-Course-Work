using _2DSpriteGameLib;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX
{
    public class GameCore
    {
        FactoryFigure2D Factory;
        Figure2D figure1;
        Figure2D preFigure1;

        Figure2D figure2;
        Figure2D preFigure2;

        bool ifSmthFalling1;
        bool ifSmthFalling2;

        int col_num = 10;
        int row_num = 20;

        int _firstScore;
        int _secondScore;

        private Field2D _field1;
        private Field2D _field2;
        private Field2D _preField1;
        private Field2D _preField2;
        int _fieldWidth;
        int _preFieldWidth;
        int _preFieldRows = 4;
        int _preFieldCols = 4;
        DX2D _dx2d;
        WindowRenderTarget _target;

        bool _gameEnded;
        public bool GameEnded { get => _gameEnded; set => _gameEnded = value; }

        Random rnd = new Random();

        public int time;
        public int gameTime;
        Size2F _targetSize;
        private RectangleF _clientRect;

        private TimeHelper _timeHelper;

        public string winner;

        public GameCore(RenderForm _renderForm, DX2D dx2d, WindowRenderTarget target, Size2F targetSize)
        {
            _dx2d = dx2d;
            _target = target;
            _targetSize = targetSize;
            _timeHelper = new TimeHelper();
            _fieldWidth = Convert.ToInt32(targetSize.Width * 0.2) / 10 * 10;
            int field1_x = Convert.ToInt32(_targetSize.Width * 0.1);
            int field1_y = Convert.ToInt32(_targetSize.Height * 0.1);
            int field2_x = field1_x + Convert.ToInt32(_targetSize.Width) / 2;
            int field2_y = Convert.ToInt32(_targetSize.Height * 0.1);
            _field1 = new Field2D(col_num, row_num, _fieldWidth, "FIRST", new Vector2(field1_x, field1_y));
            _field2 = new Field2D(col_num, row_num, _fieldWidth, "SECOND", new Vector2(field2_x, field2_y));
            _preFieldWidth = _field1.Square_size.Width * _preFieldCols;
            _preField1 = new Field2D(_preFieldCols, _preFieldRows, _preFieldWidth, "", new Vector2(field1_x + _field1.Size.Width + 10, field1_y));
            _preField2 = new Field2D(_preFieldCols, _preFieldRows, _preFieldWidth, "", new Vector2(field2_x + _field2.Size.Width + 10, field2_y));


            figure1 = CreateFigure();
            figure1.SetPosition(Convert.ToInt32((_field1.Col_num - figure1.Dimension.Width) / 2), Convert.ToInt32(1 - figure1.Dimension.Height));
            preFigure1 = CreateFigure();

            figure2 = CreateFigure();
            figure2.SetPosition(Convert.ToInt32((_field1.Col_num - figure2.Dimension.Width) / 2), Convert.ToInt32(1 - figure2.Dimension.Height));
            preFigure2 = CreateFigure();
            ifSmthFalling1 = true;
            ifSmthFalling2 = true;

            _firstScore = 0;
            _secondScore = 0;

            _gameEnded = false;

            winner = "";

            time = 0;
            gameTime = 30;
        }

        public Figure2D CreateFigure()
        {
            int value = rnd.Next(1, 7);
            Figure2D figure;
            switch (value)
            {
                case 1: Factory = new FactoryOFigure2D(); break;
                case 2: Factory = new FactoryIFigure2D(); break;
                case 3: Factory = new FactorySFigure2D(); break;
                case 4: Factory = new FactoryZFigure2D(); break;
                case 5: Factory = new FactoryLFigure2D(); break;
                case 6: Factory = new FactoryJFigure2D(); break;
                case 7: Factory = new FactoryTFigure2D(); break;
                default: Factory = new FactorySFigure2D(); break;
            }
            figure = Factory.FactoryMethod();
            int color = rnd.Next(1, DX2D.Colors.Length - 3);
            figure.ChangeColor(color);

            int superPower = rnd.Next(1, 10);
            figure = SetSuperPower(figure, superPower);
            return figure;
        }
        Figure2D SetSuperPower(Figure2D figure, int superPower)
        {
            switch (superPower)
            {
                case 1:
                    {
                        figure.ChangeColor(DX2D.Colors.Length - 2);
                        return figure = new RowDeleter(figure);
                    }
                case 5:
                    {
                        figure.ChangeColor(DX2D.Colors.Length - 1);
                        return figure = new ColorDeleter(figure);
                    }
                default: return figure;
            }

        }

        public void GameEnd(int first, int second)
        {
            winner = first > second ? "First is winner!" : first < second ? "Second is winner!" : "Both are winners!";
            winner.ToUpper();
            _gameEnded = true;
        }

        public void Render()
        {
            _clientRect.Width = _targetSize.Width;
            _clientRect.Height = _targetSize.Height;
            _target.Transform = Matrix3x2.Identity;

            RectangleF timeRect = new RectangleF();

            timeRect.Width = Convert.ToSingle(_targetSize.Width * 0.2);
            timeRect.Height = Convert.ToSingle(_targetSize.Height * 0.2);

            timeRect.X = _targetSize.Width / 2 - timeRect.Width / 2;
            timeRect.Y = Convert.ToSingle(_targetSize.Height * 0.3);

            _target.DrawText($"TIME:\n {gameTime - time}\n\nPlayer 1:\n {_firstScore} \nPlayer 2:\n {_secondScore}",
                _dx2d.TextFormatScoreTime, timeRect, _dx2d.WhiteBrush);
            int field1_x = Convert.ToInt32(_targetSize.Width * 0.1);
            int field1_y = Convert.ToInt32(_targetSize.Height * 0.1);

            int field2_x = field1_x + Convert.ToInt32(_targetSize.Width) / 2;
            int field2_y = Convert.ToInt32(_targetSize.Height * 0.1);

            _dx2d._drawer.DrawField(_field1);
            _dx2d._drawer.DrawField(_field2);
            _dx2d._drawer.DrawField(_preField1);
            _dx2d._drawer.DrawField(_preField2);
            _dx2d._drawer.FillFigure(figure1, _field1);
            _dx2d._drawer.FillFigure(figure2, _field2);

            preFigure1.SetPosition(Convert.ToInt32((_preField1.Col_num - preFigure1.Dimension.Width) / 2), Convert.ToInt32((_preField1.Row_num - preFigure1.Dimension.Height) / 2));
            preFigure2.SetPosition(Convert.ToInt32((_preField2.Col_num - preFigure2.Dimension.Width) / 2), Convert.ToInt32((_preField2.Row_num - preFigure2.Dimension.Height) / 2));

            _dx2d._drawer.FillFigure(preFigure1, _preField1);
            _dx2d._drawer.FillFigure(preFigure2, _preField2);

            IfDeleteRow(_field1, ref _firstScore);
            IfDeleteRow(_field2, ref _secondScore);

            IfCrossSmthUp(1);
            IfCrossSmthUp(2);
            if (gameTime == time)
            {
                GameEnd(_firstScore, _secondScore);
            }
        }

        public void IfCrossSmthUp(int pers)
        {
            switch (pers)
            {
                case 1:
                    {
                        for(int i = 0; i < _field1.Col_num; i++)
                        {
                            if (_field1.Grid[1, i] != 0)// && _field1.Grid[row_num - 1, i] != figure1.Color)
                            {

                                GameEnd(0, _secondScore + 1);
                            }
                        }
                        //if (TouchSmtgDown(_field1, figure1) && figure1.Position.Y <= 0)
                        //{
                        //}
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < _field2.Col_num; i++)
                        {
                            if (_field2.Grid[1, i] != 0)// && _field2.Grid[row_num - 1, i] != figure2.Color)
                            {

                                GameEnd(_firstScore + 1, 0);
                            }
                        }
                        break;
                    }

            }

        }

        public void Update()
        {
            if (!ifSmthFalling1)
            {
                if (_field1.SetFigure(figure1, out int col_del))
                {
                    _firstScore += col_del;
                    figure1 = preFigure1;
                    figure1.SetPosition(Convert.ToInt32((_field1.Col_num - figure1.Dimension.Width) / 2), Convert.ToInt32(0 - figure1.Dimension.Height));
                    ifSmthFalling1 = true;
                    preFigure1 = CreateFigure();
                }

            }
            if (!TouchSmtgDown(_field1, figure1) && !CrossSmtg(figure1, _field1)) figure1.FallDown();
            else ifSmthFalling1 = false;

            if (!ifSmthFalling2)
            {
                if (_field2.SetFigure(figure2, out int col_del))
                {
                    _secondScore += col_del;
                    figure2 = preFigure2;
                    figure2.SetPosition(Convert.ToInt32((_field2.Col_num - figure2.Dimension.Width) / 2), Convert.ToInt32(0 - figure2.Dimension.Height));
                    ifSmthFalling2 = true;
                    preFigure2 = CreateFigure();
                }

            }
            if (!TouchSmtgDown(_field2, figure2) && !CrossSmtg(figure2, _field2)) figure2.FallDown();
            else ifSmthFalling2 = false;

            time++;
        }

        public bool TouchSmtgLeft(Field2D field, Figure2D figure)
        {
            int posX = (int)figure.Position.X;
            int posY = Convert.ToInt32(Math.Floor(figure.Position.Y));
            if (figure.Position.X <= 0) return true;
            for (int i = posY; i < posY + figure.Dimension.Height; i++)
            {

                if (i < 0) continue;
                if (field.Grid[i, posX - 1] != 0 && figure.vertexs[i - posY, 0] != 0) return true;
            }
            return false;
        }
        public bool TouchSmtgRight(Field2D field, Figure2D figure)
        {
            int posX = (int)figure.Position.X;
            int posY = Convert.ToInt32(Math.Floor(figure.Position.Y));
            if (figure.Position.X + figure.Dimension.Width >= _field1.Col_num) return true;
            for (int i = posY; i < posY + figure.Dimension.Height; i++)
            {
                if (i < 0) continue;
                if (field.Grid[i, posX + figure.Dimension.Width] != 0 && figure.vertexs[i - posY, figure.Dimension.Width - 1] != 0) return true;
            }
            return false;
        }
        public bool TouchSmtgDown(Field2D field, Figure2D figure)
        {
            int posX = (int)figure.Position.X;
            int posY = Convert.ToInt32(Math.Floor(figure.Position.Y));
            if (posY + figure.Dimension.Height == field.Row_num) return true;
            for (int i = posX; i < posX + figure.Dimension.Width; i++)
            {
                if (posY < 0) continue;
                if (field.Grid[posY + figure.Dimension.Height, i] != 0 && figure.vertexs[figure.Dimension.Height - 1, i - posX] != 0) return true;
            }
            return false;
        }

        public void FirstLeft()
        {
            if (!TouchSmtgLeft(_field1, figure1)) figure1.Left();
        }
        public void FirstRight()
        {
            if (!TouchSmtgRight(_field1, figure1)) figure1.Right();

        }
        public void FirstDown()
        {
            if (!TouchSmtgDown(_field1, figure1) && !CrossSmtg(figure1, _field1)) figure1.FallDown();
        }

        public void FirstFastDown()
        {
            if (!TouchSmtgDown(_field1, figure1) && !CrossSmtg(figure1, _field1)) figure1.FastFallDown();
        }
        public void FirstTurn()
        {
            figure1.Turn();
            int posX = (int)figure1.Position.X;
            int posY = Convert.ToInt32(Math.Floor(figure1.Position.Y));
            if (CrossSmtg(figure1, _field1))
            {
                for (int i = 0; i < 3; i++)
                {
                    figure1.Turn();
                }
            }
        }

        public void SecondLeft()
        {
            if (!TouchSmtgLeft(_field2, figure2)) figure2.Left();
        }
        public void SecondRight()
        {
            if (!TouchSmtgRight(_field2, figure2)) figure2.Right();

        }
        public void SecondDown()
        {
            if (!TouchSmtgDown(_field2, figure2) && !CrossSmtg(figure2, _field2)) figure2.FallDown();
        }
        public void SecondFastDown()
        {
            if (!TouchSmtgDown(_field2, figure2) && !CrossSmtg(figure2, _field2)) figure2.FastFallDown();
        }
        public void SecondTurn()
        {
            figure2.Turn();
            int posX = (int)figure2.Position.X;
            int posY = Convert.ToInt32(Math.Floor(figure2.Position.Y));
            if (CrossSmtg(figure2, _field2))
            {
                for (int i = 0; i < 3; i++)
                {
                    figure2.Turn();
                }
            }
        }

        public bool CrossSmtg(Figure2D fig, Field2D field)
        {
            if (fig.Position.X + fig.Dimension.Width > field.Col_num || fig.Position.X + fig.Dimension.Width < 0 || fig.Position.Y + fig.Dimension.Height >= field.Row_num) return true;

            float posX = fig.Position.X;
            int posY = Convert.ToInt32(Math.Floor(fig.Position.Y));

            for (int i = 0; i < fig.Dimension.Height; i++)
            {
                for (int j = 0; j < fig.Dimension.Width; j++)
                {
                    if (i + posY + 1 < 0) continue;
                    if (fig.vertexs[i, j] != 0 && field.Grid[(int)(i + posY + 1),(int) (j + posX)] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void IfDeleteRow(Field2D field, ref int score)
        {
            for (int i = field.Row_num - 1; i > 0; i--)
            {
                int k = 0;
                for (int j = 0; j < field.Col_num; j++)
                {
                    if (field.Grid[i, j] == 0) k++;
                }
                if (k == 0)
                {
                    field.DeleteRow(i, out int col_del);
                    score += col_del;
                }
            }
        }

        //void FillFigure(Field field, Figure figure)
        //{
        //    int posX = figure.current_pos_x;
        //    int posY = Convert.ToInt32(Math.Floor(figure.current_pos_y));

        //    for (double i = posY; i < figure.lenght + posY && i < field.row_num; i++)
        //    {

        //        for (int j = posX; j < figure.width + posX && j < field.col_num; j++)
        //        {
        //            if (figure.vertexs[Convert.ToInt32(Math.Floor(i - posY)), j - posX] != 0 && i >= 0)
        //            {
        //                int left = (j) * field.Square_size + field.leftSide;
        //                double top = (i) * field.Square_size + field.topSide;
        //                SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF(left, Convert.ToSingle(Math.Floor(top)), left + field.Square_size, Convert.ToInt32(Math.Floor(top + field.Square_size)));
        //                _target.FillRectangle(rect, new SolidColorBrush(_target, colors[figure.vertexs[Convert.ToInt32(Math.Floor(i - posY)), j - posX]]));

        //            }
        //        }
        //    }

        //}

        //void FillField(Field field)
        //{

        //    for (int i = 0; i < field.row_num; i++)
        //    {
        //        for (int j = 0; j < field.col_num; j++)
        //        {
        //            if (field.grid[i, j] != 0)
        //            {
        //                int left = (j) * field.Square_size + field.leftSide;
        //                int top = (i) * field.Square_size + field.topSide;
        //                SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF(left, top, left + field.Square_size, top + field.Square_size);
        //                _target.FillRectangle(rect, new SolidColorBrush(_target, colors[field.grid[i, j]]));
        //            }
        //        }
        //    }

        //}

        public void Dispose()
        {
            _fieldWidth = Convert.ToInt32(_targetSize.Width * 0.2) / 10 * 10;

            int field1_x = Convert.ToInt32(_targetSize.Width * 0.1);
            int field1_y = Convert.ToInt32(_targetSize.Height * 0.1);
            int field2_x = field1_x + Convert.ToInt32(_targetSize.Width) / 2;
            int field2_y = Convert.ToInt32(_targetSize.Height * 0.1);
            _field1 = new Field2D(col_num, row_num, _fieldWidth, "FIRST", new Vector2(field1_x, field1_y));
            _field2 = new Field2D(col_num, row_num, _fieldWidth, "SECOND", new Vector2(field2_x, field2_y));
            _preFieldWidth = _field1.Square_size.Width * _preFieldCols;
            _preField1 = new Field2D(_preFieldCols, _preFieldRows, _preFieldWidth, "", new Vector2(field1_x + _field1.Size.Width + 10, field1_y));
            _preField2 = new Field2D(_preFieldCols, _preFieldRows, _preFieldWidth, "", new Vector2(field2_x + _field2.Size.Width + 10, field2_y));


            figure1 = CreateFigure();
            figure1.SetPosition(Convert.ToInt32((_field1.Col_num - figure1.Dimension.Width) / 2), Convert.ToInt32(1 - figure1.Dimension.Height));
            preFigure1 = CreateFigure();

            figure2 = CreateFigure();
            figure2.SetPosition(Convert.ToInt32((_field1.Col_num - figure2.Dimension.Width) / 2), Convert.ToInt32(1 - figure2.Dimension.Height));
            preFigure2 = CreateFigure();
            ifSmthFalling1 = true;
            ifSmthFalling2 = true;

            _firstScore = 0;
            _secondScore = 0;

            _gameEnded = false;

            winner = "";

            time = 0;
        }
    }
}
