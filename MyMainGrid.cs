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
    
    

    class MyMainGrid : Grid
    {
        //class attributes
        public int NumberOfWindows { get; set; } 
        public double XCoordinate { get; private set; }
        public double YCoordinate { get; private set; }
        public int ColumnPosition { get; private set; }
        public int RowPosition { get; private set; }
        public string [,] Images { get; set; }                  //contains the image's source of each occupied cell in the grid
       // private object dragSourceControl;
        private TempImageViewer[,] TmpWindow { get; set; }      //contain all the temp open windows
        public string [,] TempImageviewerSources { get; set; }  // contain the image sources for the open windows

       // public string DragandDropPath { get; private set; }
        public bool IsDragging { get; set; } = false;           //true if image been dragged from one of the cells
        private bool IstmpWindowShow { get; set; } = false;     //true if there is at least one open temp window

        public StartPoint DragStartPoint { get; set; }          //start point of dragging from inside, has X,Y location as row an column
        public Viewbox[,] ImagesMatViewBox { get; set; }        //matrix of viewboxes for the grid, to display the images.

       
        public MyMainGrid ( int numberofWindows)
        {
            this.NumberOfWindows = numberofWindows;
            Images = new string[NumberOfWindows , NumberOfWindows];
            TempImageviewerSources = new string[NumberOfWindows, NumberOfWindows];
            TmpWindow = new TempImageViewer[NumberOfWindows, NumberOfWindows];
            for (int i = 0; i < NumberOfWindows; i++)
                for (int j = 0; j < NumberOfWindows; j++)
                {
                    Images[i, j] = String.Empty;
                    TempImageviewerSources[i, j] = String.Empty;
                }
            
            ViewBoxMatInit();
            EventsInit();
            Create_panels();
           

        }

        private void EventsInit ()
        {
           
            this.AllowDrop = true;
            this.MouseDown += InnerGrid_PreviewMouseLeftButtonDown;
            this.Drop += File_Drop;
          
            
        }


        private void ViewBoxMatInit ()
        {
            ImagesMatViewBox = new Viewbox[NumberOfWindows, NumberOfWindows];
            for (int i = 0; i < NumberOfWindows; i++)
                for (int j = 0; j < NumberOfWindows; j++)
                    ImagesMatViewBox[i,j] = new Viewbox();
        }

        private void Create_panels()
        {
            //  Grid this = new Grid();
            int totalNumberOfCells = NumberOfWindows;
            int numberOfCellsInRow = totalNumberOfCells / 2;
            int numberOfRows = totalNumberOfCells - numberOfCellsInRow;

            //allow each cell responsively change in dimenssion
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.Background = Brushes.White;

            //adding rows to the main window
            for (int i = 0; i < numberOfRows; i++)
            {
                RowDefinition r = new RowDefinition();
                this.RowDefinitions.Add(r);
                //add small row for gridsplitter as long as it's not the last row
                if (i < numberOfRows - 1)
                {
                    RowDefinition rowSplitter = new RowDefinition();
                    rowSplitter.Height = new GridLength(4);
                    this.RowDefinitions.Add(rowSplitter);
                }
            }
            //adding colmns to the main window
            for (int j = 0; j < numberOfCellsInRow; j++)
            {
                ColumnDefinition c = new ColumnDefinition();
                this.ColumnDefinitions.Add(c);
                //add a small column for gridsplitter as long as it's not the last column
                if (j < numberOfCellsInRow - 1)
                {
                    ColumnDefinition colSplitter = new ColumnDefinition();
                    colSplitter.Width = new GridLength(4);
                    this.ColumnDefinitions.Add(colSplitter);
                }
            }

            //adding a row splitter
            for (int i = 1; i < 2 * numberOfRows - 1; i += 2)
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
                    this.Children.Add(gs);
                    counter++;
                }
            }
            //adding a column splitter
            for (int j = 1; j < 2 * numberOfCellsInRow - 1; j += 2)
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
                    this.Children.Add(gs);
                    counter++;
                }

            }


        }

        //dropping the draged object into some cell
        private void File_Drop(object sender, DragEventArgs e)
        {
            if (IsDragging)
            {
                setDraggedOutBlank();
                IsDragging = false;
            }
      
           // Image img = new Image();
            string[] files;

            //get the dropping position - the specific cell in the grid
            SetMousePosition(e.GetPosition(this));

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
                StartPoint dragFromTmpWindow;

                if (files[0].ToUpper().EndsWith("JPEG") || files[0].ToUpper().EndsWith("BMP") || files[0].ToUpper().EndsWith("PNG") || files[0].ToUpper().EndsWith("JPG"))
                {
                    if (IstmpWindowShow) //
                    {
                        dragFromTmpWindow = IsImageOutOfTheWindow(files[0]);
                        //if dragging occured fromtmpwindow
                        if (dragFromTmpWindow != null)
                        {//if image was dragged out, close the temporary window that contains it.
                            TmpWindow[dragFromTmpWindow.StartRow, dragFromTmpWindow.StartColumn].Close();
                            TempImageviewerSources[dragFromTmpWindow.StartRow, dragFromTmpWindow.StartColumn] = "";
                        }
                    }
                    //if the same image (path) is been dragged into the same cell don't add it
                    if (!(this.Images[this.RowPosition / 2, this.ColumnPosition / 2].Equals(files[0])))
                    {
                        this.Images[this.RowPosition / 2, this.ColumnPosition / 2] = files[0];
                        AddImageToCell();
                    }
                }
                else
                {
                    MessageBox.Show("You can only add file with :\n*JPEG* , *PNG* , *BMP* , *JPG*", "Wrong Image Type", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
            
           
        }

        //searching in the TempImageviewerSources if the dropped image was dragged from a temp window
        private StartPoint IsImageOutOfTheWindow (string path)
        {
            for (int i=0; i<NumberOfWindows; i++)
                for(int j=0; j<NumberOfWindows; j++)
                {   //checking if the dragging been from the specific window
                    if (TempImageviewerSources[i, j].Equals(path) && TmpWindow[i, j].IsDraggingOut)
                    {
                        TmpWindow[i, j].IsDraggingOut = false; // draggind was from temp window, disable dragging from now
                        return new StartPoint(i, j);
                    }
                }

            return null;
        }

        //setting the sell which the image been dragged from to image-less after the drop has been made
        private void setDraggedOutBlank()
        {

            this.RemoveVisualChild(ImagesMatViewBox[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2]);
      
            Images[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2] = "";
            ImagesMatViewBox[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2].Visibility = Visibility.Hidden;
            
        }

        private void AddImageToCell ()
        {
            Image img = CreateImageElFromPath(this.Images[this.RowPosition/2 , this.ColumnPosition/2]);

            int column = this.ColumnPosition / 2; //div2 - because all the odd rows and cols are GRIDSPLITTER, don't contain image
            int row = this.RowPosition / 2;

            
            if (this.RowPosition % 2 == 0 && this.ColumnPosition % 2 == 0) // only even rows and cols contain image
            {
                // in case that there is already some image set to this cell (but now other image - other source is trying to be added)
                this.RemoveVisualChild(ImagesMatViewBox[row, column]); 

                this.ImagesMatViewBox[row, column].Child = img as UIElement; //adding the image to the relative viewbox
                this.ImagesMatViewBox[row, column].Visibility = Visibility.Visible; // in case that some image was dragged out of that cell
                Grid.SetColumn(ImagesMatViewBox[row, column], this.ColumnPosition);
                Grid.SetRow(ImagesMatViewBox[row, column], this.RowPosition);
                this.Children.Add(ImagesMatViewBox[row, column]);

            }
        }

        //creating an image for assinging it into the cell
        private Image CreateImageElFromPath (string path)
        {
            Image img = new Image();
            Uri uri = new Uri(path);
            BitmapImage imgSource = new BitmapImage(uri);
            img.Source = imgSource;
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;

            return img;
        }

        
        

        private void InnerGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetMousePosition(e.GetPosition(this));
            //setting the MainWindow dimenssions 
            double mainWindowOriginX =Application.Current.MainWindow.Left;
            double mainWindowOriginY = Application.Current.MainWindow.Top;
            

            if (ImagesMatViewBox[this.RowPosition / 2, this.ColumnPosition / 2].Child != null)
            {
                DragStartPoint = new StartPoint(this.XCoordinate, this.YCoordinate, this.ColumnPosition, this.RowPosition);
                IsDragging = true;
                

                string[] arrayDataForDrop = new string[2];
                arrayDataForDrop[0] = Images[this.RowPosition / 2, this.ColumnPosition / 2];
                DataObject data = new DataObject(DataFormats.FileDrop, arrayDataForDrop);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                
                
                //if dragged outsit of the main window, do that:
                //save the image source for the correct window, and init correct window
                if (Images[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2] != "") // while not dragging from the inside
                {
                    TmpWindow[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2] = new TempImageViewer(Images[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2]);
                    TmpWindow[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2].Show();
                    TempImageviewerSources[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2] = Images[DragStartPoint.StartRow / 2, DragStartPoint.StartColumn / 2];

                    IstmpWindowShow = true;
                    this.setDraggedOutBlank();
                }
                
            }
            
        }

        //get the dropping position - the specific cell in the grid
        private void SetMousePosition (Point e)
        {
            this.XCoordinate = e.X;
            this.YCoordinate = e.Y;
            this.ColumnPosition = (int)ColumnComputation(this.ColumnDefinitions, e.X);
            this.RowPosition = (int)RowComputation(this.RowDefinitions, e.Y);
        }

        //calculate the column of the grid that the mouse cursor is passing over
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
        //calculate the row of the grid that the mouse cursor is passing over
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

       
    }
}

