using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Minesweeper
{
    internal class MyButton : Button
    {
        //cords
        public int x;
        public int y;

        //flagged or not
        public bool isFlagged;
    }
}
