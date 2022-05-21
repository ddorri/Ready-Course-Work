using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameLib
{
    public class Field
    {
        protected int _col_num;
        protected int _row_num;
        public int Col_num { get => _col_num; }
        public int Row_num { get => _row_num; }

        protected int[,] _grid;
        public int[,] Grid { get => _grid; set => _grid = value; }

        public Field(int col_num, int row_num)
        {
            _col_num = col_num;
            _row_num = row_num;
            _grid = new int[_row_num, _col_num];
            for (int i = 0; i < _row_num; i++)
            {
                for (int j = 0; j < _col_num; j++)
                {
                    _grid[i, j] = 0;
                }
            }
        }

        public int DefinePopularColor()
        {
            Dictionary<int, int> colors = new Dictionary<int, int>();
            for (int i = 0; i < _row_num; i++)
            {
                for (int j = 0; j < _col_num; j++)
                {
                    if (_grid[i, j] == 0) continue;
                    if (colors.Count == 0) colors.Add(_grid[i, j], 1);
                    else
                    {
                        bool k = false;
                        for(int o = 0; o < colors.Count; o++)
                        {
                            if(_grid[i, j] == colors.ElementAt(o).Key )
                            {
                                colors[_grid[i, j]]++;
                                k = true;
                                break;
                            }
                        }
                        if (!k)
                        {
                            colors.Add(_grid[i, j], 1);
                        }
                    }
                }
            }
            int popularNum = 0;
            int popularInd = 0;
            for (int m = 0; m < colors.Count; m++)
            {
                if (colors.ElementAt(m).Value > popularNum) 
                {
                    popularNum = colors.ElementAt(m).Value;
                    popularInd = colors.ElementAt(m).Key;
                }
            }
            colors.Clear();
            return popularInd;
        }

        public void DeleteColor(int color, out int col_del)
        {
            col_del = 0;
            for (int i = 0; i < _row_num; i++)
            {
                for (int j = 0; j < _col_num; j++)
                {

                    if (_grid[i, j] == color)
                    {
                        _grid[i, j] = 0;
                        col_del++;
                    }
                }
            }
        }

        public void DeleteRow(int row_num, out int col_del)
        {
            col_del = 0;
            for (int i = 0; i < _col_num; i++)
            {
                if (_grid[row_num, i] != 0) col_del++;
                _grid[row_num, i] = 0;
            }
            RowFall(row_num);
        }

        public void RowFall(int row_num)
        {
            for (int i = row_num; i > 0; i--)
            {
                for (int j = 0; j < _col_num; j++)
                {
                    _grid[i, j] = _grid[i - 1, j];
                }
            }
        }

    }
}
