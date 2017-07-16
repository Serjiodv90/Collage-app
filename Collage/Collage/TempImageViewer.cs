using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Collage
{
    class TempImageViewer : Window
    {


        private string path { get; set; }
        private int DragFromRow { get; set; }
        public bool IsDraggingOut { get; set; } = false;

        public TempImageViewer() { }

        public TempImageViewer(string path)
        {

            this.MouseDown += TempWindow_MouseDown;
            this.path = path;
            this.AddChild((CreateImageElFromPath(this.path) as UIElement));
            this.Height = 300;
            this.Width = 300;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
            
        }

        private void TempWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsDraggingOut = true;
            string[] arrayDataForDrop = new string[1];
            arrayDataForDrop[0] = this.path;
            DataObject data = new DataObject(DataFormats.FileDrop, arrayDataForDrop);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
        }

        private Image CreateImageElFromPath(string path)
        {
            Image img = new Image();
            Uri uri = new Uri(path);
            BitmapImage imgSource = new BitmapImage(uri);
            img.Source = imgSource;
             img.HorizontalAlignment = HorizontalAlignment.Stretch;
             img.VerticalAlignment = VerticalAlignment.Stretch;
            
            return img;
        }  

        public bool IsDragOut ()
        {
            return IsDraggingOut ? true : false;
        }





    }
}
