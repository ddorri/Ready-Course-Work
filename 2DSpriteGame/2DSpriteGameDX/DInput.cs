using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace _2DSpriteGameDX
{
    class DInput : IDisposable
    {
        // Экземпляр объекта "прямого ввода"
        private DirectInput _directInput;

        // Поля и свойства, связанные с клавиатурой
        private Keyboard _keyboard;
        private KeyboardState _keyboardState;
        public KeyboardState KeyboardState { get => _keyboardState; }
        private bool _keyboardUpdated;
        public bool KeyboardUpdated { get => _keyboardUpdated; }
        private bool _keyboardAcquired;

        // Поля и свойства, связанные с грызуном
        private Mouse _mouse;
        private MouseState _mouseState;
        public MouseState MouseState { get => _mouseState; }
        private bool _mouseUpdated;
        public bool MouseUpdated { get => _mouseUpdated; }
        private bool _mouseAcquired;

        // В конструкторе создаем все объекты и пробуем получить доступ к устройствам
        public DInput(RenderForm renderForm)
        {
            _directInput = new DirectInput();

            _keyboard = new Keyboard(_directInput);
            _keyboard.Properties.BufferSize = 16;
            _keyboard.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireKeyboard();
            _keyboardState = new KeyboardState();

            _mouse = new Mouse(_directInput);
            _mouse.Properties.AxisMode = DeviceAxisMode.Relative;
            _mouse.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireMouse();
            _mouseState = new MouseState();
        }

        // Получение доступа к клавиатуре
        private void AcquireKeyboard()
        {
            try
            {
                _keyboard.Acquire();
                _keyboardAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _keyboardAcquired = false;
            }
        }

        // Получение доступа к грызуну
        private void AcquireMouse()
        {
            try
            {
                _mouse.Acquire();
                _mouseAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _mouseAcquired = false;
            }
        }

        // Обновление состояния клавиатуры
        public void UpdateKeyboardState()
        {
            // Если доступ не был получен, пробуем здесь
            if (!_keyboardAcquired) AcquireKeyboard();

            // Пробуем обновить состояние
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _keyboard.GetCurrentState(ref _keyboardState);
                // Успех
                _keyboardUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                // Отказ
                _keyboardUpdated = false;
            }

            // В большинстве случаев отказ из-за потери фокуса ввода
            // Устанавливаем соответствующий флаг, чтобы в следующем кадре попытаться получить доступ
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _keyboardAcquired = false;
        }

        // Обновление состояния грызуна
        public void UpdateMouseState()
        {
            // Если доступ не был получен, пробуем здесь
            if (!_mouseAcquired) AcquireMouse();

            // Пробуем обновить состояние
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _mouse.GetCurrentState(ref _mouseState);
                // Успех
                _mouseUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                // Отказ
                _mouseUpdated = false;
            }

            // В большинстве случаев отказ из-за потери фокуса ввода
            // Устанавливаем соответствующий флаг, чтобы в следующем кадре попытаться получить доступ
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _mouseAcquired = false;
        }

        // Освобождение выделенных нам неуправляемых ресурсов
        public void Dispose()
        {
            Utilities.Dispose(ref _mouse);
            Utilities.Dispose(ref _keyboard);
            Utilities.Dispose(ref _directInput);
        }
    }
}