using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameLib
{
    public enum SP
    {
        rowDeleting,
        colorDeliting,
    }
    public abstract class FigureDecorator : Figure2D, ISuperPower
    {
        protected Figure2D figure;
        protected SP _superPower;
        public SP superPower { get { return _superPower; } }
        public FigureDecorator(Figure2D figure) : base()
        {
            this.figure = figure;
            this._vertexs = figure.vertexs;
        }
        public abstract void SuperPower();

    }

    public class RowDeleter : FigureDecorator
    {
        public RowDeleter(Figure2D figure)
            : base(figure)
        {
            SuperPower();
        }
        public override void SuperPower()
        {
            base._superPower = SP.rowDeleting;
        }



    }

    public class ColorDeleter : FigureDecorator
    {
        public ColorDeleter(Figure2D figure)
            : base(figure)
        {
            SuperPower();
        }

        public override void SuperPower()
        {
            base._superPower = SP.colorDeliting;
        }

    }
}
