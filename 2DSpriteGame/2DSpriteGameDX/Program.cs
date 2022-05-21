using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Windows;

namespace _2DSpriteGameDX
{
    // При добавлении картинок для спрайтов нельзя забывать задавать свойство копирования: всегда

    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Direct2DApp app = new Direct2DApp();
            app.Run();
            app.Dispose();
        }
    }
}
