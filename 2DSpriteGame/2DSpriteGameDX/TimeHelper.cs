using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX
{
    class TimeHelper
    {
        // Таймер
        private Stopwatch _watch;

        // Счетчик кадров
        private int _counter = 0;
        // Количество кадров за прошлую секунду
        private int _fps = 0;
        public int FPS { get => _fps; }

        // Момент времени при прошлом обновлении значения FPS
        private long _previousFPSMeasurementTime;

        // Количество тиков на момент прошлого кадра
        private long _previousTicks;

        // Текущее время в секундах
        private float _time;
        public float Time { get => _time; }

        // Сколько времени прошло с прошлого кадра
        private float _dT;
        public float dT { get => _dT; }

        public bool ifSec = false;

        // В конструкторе создаем экземпляр таймера и выполняем сброс
        public TimeHelper()
        {
            _watch = new Stopwatch();
            Reset();
        }

        // Обновление подсчитываемых значений
        // Должен вызываться в начале каждого кадра
        public void Update()
        {
            // Текущее значение счетчика тиков
            long ticks = _watch.Elapsed.Ticks;
            // Вычисляем текущее время и интервал между текущим и прошлым кадрами
            _time = (float)ticks / TimeSpan.TicksPerSecond;
            _dT = (float)(ticks - _previousTicks) / TimeSpan.TicksPerSecond;
            // Запоминаем текущее значение счетчика тиков для вычислений в будущем кадре
            _previousTicks = ticks;

            // Инкремент счетчика кадров
            _counter++;
            // Если истекла секунда, то обновляем значение FPS и фиксируем момент времени для отсчета следующей секунды
           
            if (_watch.ElapsedMilliseconds - _previousFPSMeasurementTime >= 1000)
            {
                _fps = _counter;
                _counter = 0;
                _previousFPSMeasurementTime = _watch.ElapsedMilliseconds;
                ifSec = true;
            }
            else ifSec = false;
        }

        // Сброс таймера и счетчиков
        public void Reset()
        {
            _watch.Reset();
            _counter = 0;
            _fps = 0;
            _watch.Start();
            _previousFPSMeasurementTime = _watch.ElapsedMilliseconds;
            _previousTicks = _watch.Elapsed.Ticks;
        }
    }
}
