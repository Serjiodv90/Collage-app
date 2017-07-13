using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Collage
{
    public class StartPoint
    {
        public Point Start { get; set; }
        public int StartColumn { get; set; }
        public int StartRow { get; set; }

        public StartPoint(double xCoordinate, double yCoordinate, int startColumn, int startRow)
        {
            this.Start = new Point(xCoordinate, yCoordinate);
            this.StartColumn = startColumn;
            this.StartRow = startRow;
        }

        public StartPoint ( int startRow, int startColumn)
        {
            this.StartColumn = startColumn;
            this.StartRow = startRow;
            this.Start = new Point(0, 0);
        }
        
    }
}
