using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Collage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
        // private MainWindow M { get; set; }
        public int NumberOfWindows { get; set; } = 8;
        public double XCoordinate { get; private set; }
        public double YCoordinate { get; private set; }
        public int ColumnPosition { get; private set; }
        public int RowPosition { get; private set; }
        public string[] images = new string[16];
        public string DragandDropPath { get; private set; }
        public bool IsDragging { get; set; } = false;

        public StartPoint DragStartPoint { get; set; }
        */
        public MainWindow()
        {
            InitializeComponent();
            //  MyMainGrid grid = new MyMainGrid();
            //MainGrid.Children.Add(grid);
            MyStackPanel sp = new MyStackPanel();
            MyStackPanel.Children.Add(sp);
            
            /*
            MessageBox.Show("Welcome to window 2");

            for (int i = 0; i < images.Length; i++)
                images[i] = "";
            // M = new MainWindow();
            Create_panels();
            this.PreviewMouseMove += InnerGrid_PreviewMouseMove;
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
            this.MouseLeave += Window_MouseLeave;
          //  this.MouseMove += Window_MouseMove;
          */

        }

        /*

        private void Create_panels()
        {
          //  Grid MainGrid = new Grid();
            int totalNumberOfCells = NumberOfWindows;
            int numberOfCellsInRow = totalNumberOfCells / 2;
            int numberOfRows = totalNumberOfCells - numberOfCellsInRow;
            
            //allow each cell responsively change in dimenssion
            MainGrid.VerticalAlignment = VerticalAlignment.Stretch;
            MainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            MainGrid.Background = Brushes.White;

            //adding rows to the main window
            for (int i = 0; i < numberOfRows; i++)
            {
                RowDefinition r = new RowDefinition();
                MainGrid.RowDefinitions.Add(r);

                if (i < numberOfRows - 1)
                {
                    RowDefinition rowSplitter = new RowDefinition();
                    rowSplitter.Height = new GridLength(4);
                    MainGrid.RowDefinitions.Add(rowSplitter);
                }
            }
            //adding colmns to the main window
            for (int j = 0; j < numberOfCellsInRow ; j++)
            {
                ColumnDefinition c = new ColumnDefinition();
                MainGrid.ColumnDefinitions.Add(c);
                if (j < numberOfCellsInRow - 1)
                {
                    ColumnDefinition colSplitter = new ColumnDefinition();
                    colSplitter.Width = new GridLength(4);
                    MainGrid.ColumnDefinitions.Add(colSplitter);
                }
            }
            
            //adding a row splitter
            for (int i = 1; i < 2* numberOfRows - 1; i += 2)
            {
                Console.WriteLine(i);
                int counter = 0;
                while (counter < 2 * numberOfRows - 1)
                {
                    GridSplitter gs = new GridSplitter()
                    {
                        Height = 4,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        ResizeBehavior = GridResizeBehavior.PreviousAndNext
                    };
                    gs.Background = Brushes.Black;
                    Grid.SetRow(gs, i);
                    Grid.SetColumn(gs, counter);
                    MainGrid.Children.Add(gs);
                    counter++;
                }
            }
            //adding a column splitter
            for (int j=1; j<2*numberOfCellsInRow-1; j+=2)
            {
                int counter = 0;
                while (counter < 2 * numberOfCellsInRow - 1)
                {
                    GridSplitter gs = new GridSplitter()
                    {
                        Width = 4,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        ResizeBehavior = GridResizeBehavior.PreviousAndNext
                    };
                    gs.Background = Brushes.Black;
                    Grid.SetColumn(gs, j);
                    Grid.SetRow(gs, counter);
                    MainGrid.Children.Add(gs);
                    counter++;
                }
                        
            }


            MainGrid.AllowDrop = true;
          //  MainGrid.ShowGridLines = true;
            
            
            

        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            Image img = new Image();
            string[] files;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //   MessageBox.Show(String.Format(files[0]));
                this.images[this.RowPosition + this.ColumnPosition] = files[0];
                
            }
            
            MessageBox.Show(String.Format(this.images[this.RowPosition + this.ColumnPosition]));
            Uri uri = new Uri(this.images[this.RowPosition + this.ColumnPosition]);
            BitmapImage imgSource = new BitmapImage(uri);
            img.Source = imgSource;

          
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetColumn(img, this.ColumnPosition);
            Grid.SetRow(img, this.RowPosition);
            MainGrid.Children.Add(img);

        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            string[] files;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                 files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //   MessageBox.Show(String.Format(files[0]));
                if (files.Length > 1)
                    MessageBox.Show("You can't drop more than 1 file at a time!");
                else DragandDropPath = files[0];
               
                MessageBox.Show(String.Format(this.images[this.RowPosition + this.ColumnPosition]));
            }
            
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {

            Point p;
            p=e.GetPosition(this);
            MessageBox.Show(String.Format("Cursor position: {0}", p));
            /*
            if (IsDragging)
            {
                Window w = new Window();
                w.WindowStartupLocation = WindowStartupLocation.Manual;
                //  w.Height = MainGrid.RowDefinitions[0].Height.Value;
                //w.Width = MainGrid.ColumnDefinitions[0].Width.Value;
                w.Height = 200;
                w.Width = 200;
                
                w.Content = this.images[this.RowPosition + this.ColumnPosition];
                w.Show();
            }*/
      //      MessageBox.Show("Mouse left");
      //  }
    /*
        private void InnerGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //setting the MainWindow dimenssions 
            double mainWindowWidth = this.Width;
            double mainWindowHeigh = this.Height;
            double mainWindowOriginX = Application.Current.MainWindow.Left;
            double mainWindowOriginY = Application.Current.MainWindow.Top;
            double mainWindowRangeX = mainWindowOriginX + mainWindowWidth;
            double mainWindowRangeY = mainWindowOriginY + mainWindowHeigh;
            
            if(!(this.images[this.RowPosition + this.ColumnPosition] == " "))
            {
                IsDragging = true;
            }

            
           
/*
            if()
            DataObject data = new DataObject(this.images[this.RowPosition + this.ColumnPosition]);
*//*
            MessageBox.Show(String.Format("mainWindowWidth : {0}\n" +
                "mainWindowHeigh : {1}\n" +
                "mainWindowOriginX : {2}\n" +
                "mainWindowOriginY : {3}\n" +
                "mainWindowRangeX : {4}\n" +
                "mainWindowRangeY : {5}\n" +
                "XCoordinate : {6}\n" +
                "YCoordinate : {7}", mainWindowWidth, mainWindowHeigh, mainWindowOriginX, mainWindowOriginY, mainWindowRangeX, mainWindowRangeY,
                this.XCoordinate, this.YCoordinate));

            MessageBox.Show(String.Format("XCoordinate : {0}\n" +
                "YCoordinate : {1}",   this.XCoordinate, this.YCoordinate));


            if (this.XCoordinate > mainWindowWidth || this.XCoordinate < 0 
                || this.YCoordinate > mainWindowHeigh || this.YCoordinate < 0)
            {
                MessageBox.Show("Out of bounds");
                
            }

        }
        private void InnerGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            
            this.XCoordinate = e.GetPosition(MainGrid).X;
            this.YCoordinate = e.GetPosition(MainGrid).Y;
            this.ColumnPosition =(int)ColumnComputation(MainGrid.ColumnDefinitions, e.GetPosition(MainGrid).X) ;
            this.RowPosition = (int)RowComputation(MainGrid.RowDefinitions, e.GetPosition(MainGrid).Y);
            //   MessageBox.Show(String.Format("Row: {0}, Column: {1}", RowPosition, ColumnPosition));
             

        }

        private double ColumnComputation(ColumnDefinitionCollection c, double YPosition)
        {
            var columnLeft = 0.0; var columnCount = 0;
            foreach (ColumnDefinition cd in c)
            {
                double actWidth = cd.ActualWidth;
                if (YPosition >= columnLeft && YPosition < (actWidth + columnLeft)) return columnCount;
                columnCount++;
                columnLeft += cd.ActualWidth;
            }
            return (c.Count + 1);
        }
        private double RowComputation(RowDefinitionCollection r, double XPosition)
        {
            var rowTop = 0.0; var rowCount = 0;
            foreach (RowDefinition rd in r)
            {
                double actHeight = rd.ActualHeight;
                if (XPosition >= rowTop && XPosition < (actHeight + rowTop)) return rowCount;
                rowCount++;
                rowTop += rd.ActualHeight;
            }
            return (r.Count + 1);
        }


        #region : handling the drag out 
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragStartPoint = new StartPoint(this.XCoordinate, this.YCoordinate, this.ColumnPosition, this.RowPosition);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //drag is heppen
            //Prepare for Drag and Drop
            InnerGrid_PreviewMouseMove(sender, e);
            Point mpos = e.GetPosition(null);
            Vector diff = this.DragStartPoint.Start - mpos;
            
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                /*
                                //hooking on Mouse Up
                                InterceptMouse.m_hookID = InterceptMouse.SetHook(InterceptMouse.m_proc);

                                //ataching the event for hadling drop
                                this.QueryContinueDrag += queryhandler;
                                *//*
                MessageBox.Show("Out");
                //begin drag and drop
                DataObject dataObj = new DataObject(this.images[this.RowPosition + this.ColumnPosition]);
                DragDrop.DoDragDrop(MainGrid, dataObj, DragDropEffects.Move);

            }
        }
        /*
        private void DragSourceQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //when keystate is non, draop is heppen
            if (e.KeyStates == DragDropKeyStates.None)
            {
                //unsubscribe event
                this.QueryContinueDrag -= queryhandler;
                e.Handled = true;
                //Unhooking on Mouse Up
                InterceptMouse.UnhookWindowsHookEx(InterceptMouse.m_hookID);

                //notifiy user about drop result
                Task.Run(
                    () =>
                {
                    //Drop hepend outside Instantly app
                    if (InterceptMouse.IsMouseOutsideApp)
                        MessageBox.Show("Dragged outside app");
                    else
                        MessageBox.Show("Dragged inside app");
                }
            );
            }
        }*//*
        #endregion
    */
    }
}

