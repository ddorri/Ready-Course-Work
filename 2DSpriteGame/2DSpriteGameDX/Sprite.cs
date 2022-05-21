using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace _2DSpriteGameDX
{
    class Sprite
    {
        private static readonly float _Pi = (float)Math.PI;
        private static readonly float _2Pi = 2.0f * (float)Math.PI;

        // 16 пикселей текстуры соответствуют единице игрового пространства
        private static readonly float _pu = 1.0f / 16.0f;

        // Инфаструктурный объект
        private DX2D _dx2d;

        // Положение центра спрайта в игровом пространстве (20 единиц измерения на высоту поля отображения)
        private Vector2 _positionOfCenter;
        public Vector2 PositionOfCenter { get => _positionOfCenter; set => _positionOfCenter = value; }

        // Угол поворота спрайта
        private float _angle;
        public float Angle
        {
            get => _angle;
            set {
                _angle = value;
                if (_angle > _Pi) _angle -= _2Pi;
                else if (_angle < -_Pi) _angle += _2Pi;
            }
        }

        // Индекс кпртинки спрайта в коллекции
        private int _bitmapIndex;

        // Ширина и высота картинки в пикселях, положение центра спрайта в пикселях относительно верхнего левого края картинки
        private float _width;
        private float _height;
        private Vector2 _center;

        // Вектор для операции перемещения картинки
        private Vector2 _translation;
        // Матрица координатных преобразований
        private Matrix3x2 _matrix;

        // В конструкторе инициализируем поля
        public Sprite(DX2D dx2d, int bitmapIndex, float centerX, float centerY, float angle)
        {
            _dx2d = dx2d;
            _bitmapIndex = bitmapIndex;
            _positionOfCenter.X = centerX;
            _positionOfCenter.Y = centerY;
            _angle = angle;
            SharpDX.Direct2D1.Bitmap bitmap = _dx2d.Bitmaps[_bitmapIndex];
            _width = bitmap.Size.Width;
            _height = bitmap.Size.Height;
            _center.X = _width / 2.0f;
            _center.Y = _height / 2.0f;
        }

        // Отрисовка
        public void Draw(float opacity, float scale, float height)
        {
            // Готовим матрицу координатных преобразований
            //   Переносим центр спрайта в начало координат (-center), потом в текущую позицию (+position), потом умножаем на маштаб (*scale)
            _translation.X = (-_center.X * _pu + _positionOfCenter.X) * scale;
            _translation.Y = (_center.Y * _pu + _positionOfCenter.Y + 1) * scale; //height - (
            //   Порядок перемножения матриц в "прямом иксе 2 измерения" перевернут с ног на голову:
            //   слева - вращение и масштаб (в произвольном порядке между собой), справа - перенос
            _matrix = Matrix3x2.Rotation(-_angle, _center) *
                Matrix3x2.Scaling(scale * _pu, scale * _pu, Vector2.Zero) *
                Matrix3x2.Translation(_translation);

            // Получаем из инфраструктурного объекта "цель" отрисовки
            WindowRenderTarget r = _dx2d.RenderTarget;
            // Устанавливаем матрицу координатных преобразований
            r.Transform = _matrix;

            // Нарисовываемся
            SharpDX.Direct2D1.Bitmap bitmap = _dx2d.Bitmaps[_bitmapIndex];
            r.DrawBitmap(bitmap, opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        // Отрисовка спрайта для фона
        public void DrawBackground(float opacity, float scale, float textureScale, float height)
        {
            // Готовим матрицу координатных преобразований
            //   Переносим спрайт. Умножаем на масштаб (*scale) для пересчета в пиксели.
            _translation.X = _positionOfCenter.X * scale;
            _translation.Y = - _positionOfCenter.Y * scale;
            //   Порядок перемножения матриц в "прямом иксе 2 измерения" перевернут с ног на голову:
            //   слева - вращение и масштаб (в произвольном порядке между собой), справа - перенос
            _matrix = Matrix3x2.Rotation(-_angle, _center) *
                Matrix3x2.Scaling(scale * textureScale, scale * textureScale, Vector2.Zero) *
                Matrix3x2.Translation(_translation);

            // Получаем из инфраструктурного объекта "цель" отрисовки
            WindowRenderTarget r = _dx2d.RenderTarget;
            // Устанавливаем матрицу координатных преобразований
            r.Transform = _matrix;

            // Нарисовываемся
            SharpDX.Direct2D1.Bitmap bitmap = _dx2d.Bitmaps[_bitmapIndex];
            r.DrawBitmap(bitmap, opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }
    }
}
