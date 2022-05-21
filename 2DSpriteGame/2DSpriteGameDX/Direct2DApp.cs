using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using SharpDX.DirectInput;

namespace _2DSpriteGameDX
{
    class Direct2DApp : IDisposable
    {
        // Для расчета коэффициента масштабирования принимаем, что вся область окна по высоте вмещает 20 единиц длинны виртуального игрового пространства
        private static float _unitsPerHeight = 20.0f;
        public static float UnitsPerHeight { get => _unitsPerHeight; }

        // Окно программы
        private RenderForm _renderForm;
        public RenderForm RenderForm { get => _renderForm; }

        // Инфраструктурные объекты
        private DX2D _dx2d;
        public DX2D DX2D { get => _dx2d; }

        // Клиетская область порта отрисовки в устройство-независимых пикселях
        private RectangleF _clientRect;

        // Коэффициент масштабирования
        private float _scale;
        public float Scale { get => _scale; }

        // Помощник для работы со временем
        private TimeHelper _timeHelper;

        // Спрайт фона
        private Sprite _background;

        private GameCore _gameCore;

        DInput _dInput;
        WindowRenderTarget target;
        Size2F targetSize;

        private bool _lPressed;
        private bool _rPressed;
        private bool _PdPressed;
        private bool _PuPressed;

        private bool _aPressed;
        private bool _dPressed;
        private bool _sPressed;
        private bool _wPressed;

        private bool _aPressedPast;
        private bool _sPressedPast;
        private bool _dPressedPast;
        private bool _wPressedPast;

        private bool _PuPressedPast;
        private bool _PdPressedPast;
        private bool _lPressedPast;
        private bool _rPressedPast;

        MainMenu mainMenu;
        AfterMenu afterMenu;
        // В конструкторе создаем форму, инфраструктурные объекты, подгружаем спрайты, создаем помощник для работы со временем
        // В конце дергаем ресайзинг формы для вычисления масштаба и установки пределов по горизонтали и вертикали
        public Direct2DApp()
        {

            _renderForm = new RenderForm("TETRIS");
            _renderForm.WindowState = FormWindowState.Maximized;
            _renderForm.IsFullscreen = true;
            //_renderForm.Size = new System.Drawing.Size(1920, 1080);

            _dInput = new DInput(_renderForm);
            _dx2d = new DX2D(_renderForm);
            target = _dx2d.RenderTarget;
            targetSize = target.Size;
            // targetSize = new Size2F(target.PixelSize.Width, target.PixelSize.Height);
            _gameCore = new GameCore(_renderForm, _dx2d, target, targetSize);
            mainMenu = new MainMenu(_renderForm, _dx2d, target, targetSize, _dInput);
            afterMenu = new AfterMenu(_renderForm, _dx2d, target, targetSize, _dInput);

            int backgroundIndex = _dx2d.LoadBitmap("D:\\Styding\\4 семестр\\Course-Project-main\\2DSpriteGameDX\\bg.bmp");
            _background = new Sprite(_dx2d, backgroundIndex, 0.0f, 0.0f, 0.0f);

            _timeHelper = new TimeHelper();
            RenderForm_Resize(this, null);

        }

        // Делегат, вызываемый для формирования каждого кадра
        private void RenderCallback()
        {
            _timeHelper.Update();

            int fps = _timeHelper.FPS;
            float dT = _timeHelper.dT;

            // Начинаем вывод графики
            target.BeginDraw();
            // Очистить область отображения
            target.Clear(SharpDX.Color.Black);

            Vector2 backgroundPos = new Vector2(0, 0);
            _background.PositionOfCenter = backgroundPos;
            _background.DrawBackground(1.0f, 300, _unitsPerHeight / 1080.0f, _clientRect.Height);

            if (!mainMenu.startGame)
            {
                mainMenu.UpdateMainMenu();
            }
            else if (_gameCore.GameEnded && !afterMenu.OkayClicked)
            {
                _gameCore.Render();
                afterMenu.UpdateAfterMenu(_gameCore.winner);
            }
            else if (afterMenu.OkayClicked)
            {
                mainMenu.startGame = false;
                afterMenu.OkayClicked = false;
                _gameCore.Dispose();
            }
            else if (!_gameCore.GameEnded)
            {
                _dInput.UpdateKeyboardState();
                _dInput.UpdateMouseState();
                if (_dInput.KeyboardUpdated)
                {
                    bool wPressed = _dInput.KeyboardState.IsPressed(Key.W);
                    bool sPressed = _dInput.KeyboardState.IsPressed(Key.S);
                    bool aPressed = _dInput.KeyboardState.IsPressed(Key.A);
                    bool dPressed = _dInput.KeyboardState.IsPressed(Key.D);
                    if (wPressed && !_wPressedPast)
                    {
                        _gameCore.FirstTurn();
                    }
                    if (sPressed && !_sPressedPast)
                    {
                        _gameCore.FirstDown();
                    }
                    else if (sPressed && _sPressedPast)
                    {
                        _gameCore.FirstFastDown();
                    }
                    if (aPressed && !_aPressedPast)
                    {
                        _gameCore.FirstLeft();
                    }
                    if (dPressed && !_dPressedPast)
                    {
                        _gameCore.FirstRight();
                    }
                    _aPressedPast = aPressed;
                    _dPressedPast = dPressed;
                    _wPressedPast = wPressed;
                    _sPressedPast = sPressed;
                    _aPressed = false;
                    _dPressed = false;
                    _wPressed = false;
                    _sPressed = false;

                    bool PuPressed = _dInput.KeyboardState.IsPressed(Key.Up);
                    bool PdPressed = _dInput.KeyboardState.IsPressed(Key.Down);
                    bool lPressed = _dInput.KeyboardState.IsPressed(Key.Left);
                    bool rPressed = _dInput.KeyboardState.IsPressed(Key.Right);
                    if (PuPressed && !_PuPressedPast)
                    {
                        _gameCore.SecondTurn();
                    }
                    if (PdPressed && !_PdPressedPast)
                    {
                        _gameCore.SecondDown();
                    }
                    else if (PdPressed && _PdPressedPast)
                    {
                        _gameCore.SecondFastDown();
                    }
                    if (lPressed && !_lPressedPast)
                    {
                        _gameCore.SecondLeft();
                    }
                    if (rPressed && !_rPressedPast)
                    {
                        _gameCore.SecondRight();
                    }
                    _PuPressedPast = PuPressed;
                    _PdPressedPast = PdPressed;
                    _lPressedPast = lPressed;
                    _rPressedPast = rPressed;
                    _PuPressed = false;
                    _PdPressed = false;
                    _lPressed = false;
                    _rPressed = false;
                }

               
                if (_timeHelper.ifSec)
                {
                    _gameCore.Update();
                }
                _gameCore.Render();
            }

            // Готово!
            target.EndDraw();
        }



        // При ресайзинге обновляем размер области отображения и масштаб
        private void RenderForm_Resize(object sender, EventArgs e)
        {
            int width = _renderForm.ClientSize.Width;
            int height = _renderForm.ClientSize.Height;
            _dx2d.RenderTarget.Resize(new Size2(width, height));
            //_clientRect.Width = _dx2d.RenderTarget.Size.Width;
            //_clientRect.Height = _dx2d.RenderTarget.Size.Height;
            _clientRect.Width = _dx2d.RenderTarget.Size.Width;
            _clientRect.Height = _dx2d.RenderTarget.Size.Height;
            _scale = _clientRect.Height / _unitsPerHeight;
        }

        // ПОЕХАЛИ!!! Запуск рабочего цикла приложения 
        public void Run()
        {

            _renderForm.Resize += RenderForm_Resize;
            RenderLoop.Run(_renderForm, RenderCallback);
        }

        // Убираем за собой, удаляя неуправляемые ресурсы (здесь мамы с веником, чтобы убрала за нами нету, легко спровоцировать утечку памяти)
        public void Dispose()
        {
            _dInput.Dispose();
            _dx2d.Dispose();
            _renderForm.Dispose();
        }
    }
}