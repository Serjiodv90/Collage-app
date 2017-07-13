using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Collage
{
    class MyDockPanel : DockPanel
    {
        public Grid CenterGrid { get; set; }
        public int NumberOfWindows { get; set; }
        public ComboBox Cb { get; set; }
        public GroupBox Generalgb { get; set; }

        public MyDockPanel()
        {
            this.SetGBSettings();
            this.CreateGrid();
            this.AddTextBlock();
            this.AddComboBox();
            this.AddGenerateButton();
            this.CenterGrid.VerticalAlignment = VerticalAlignment.Center;
            this.CenterGrid.HorizontalAlignment = HorizontalAlignment.Center;
            this.CenterGrid.Width = 250;
            this.Generalgb.Content = CenterGrid;
            DockPanel.SetDock(Generalgb, Dock.Top);
            this.LastChildFill = true;
            this.Children.Add(this.Generalgb);
            
            
        }

        private void SetGBSettings()
        {
            this.Generalgb = new GroupBox();
            
            this.Generalgb.Width = 250;
            this.Generalgb.Height = 200;
            this.Generalgb.Header = "Collage size";
        }


        //creating 2 rows for the welcome window
        private void CreateGrid ()
        {
            this.CenterGrid = new Grid();
            for (int i = 0; i < 3; i++)
            {
                RowDefinition r = new RowDefinition();
                r.Height = new GridLength(1,GridUnitType.Star);
                
                this.CenterGrid.RowDefinitions.Add(r);
                
            }
        }

        private void AddTextBlock ()
        {
            TextBlock tb = new TextBlock();
            tb.Text = "Choose the size :";
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Margin = new Thickness(10)  ;
            Grid.SetRow(tb, 0);
            this.CenterGrid.Children.Add(tb);
            
        }

        private void AddComboBox ()
        {
             Cb = new ComboBox();
            
            for(int i=2; i<=8; i+=2)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i+ "x"+ i;
                Cb.Items.Add(item);
            }
            Cb.Width = 60;
            Cb.SelectionChanged += ComboBox_SelectionChanged;
            Cb.VerticalAlignment = VerticalAlignment.Center;
            Cb.HorizontalAlignment = HorizontalAlignment.Center;
            Cb.Margin = new Thickness(10);
            Grid.SetRow(Cb, 1);
            this.CenterGrid.Children.Add(Cb);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selection = Cb.SelectedIndex;
                

            switch (selection)
            {
                case 0:
                    {
                        this.NumberOfWindows = 4;
                        break;
                    }
                case 1:
                    {
                        this.NumberOfWindows = 8;
                        break;
                    }
                case 2:
                    {
                        this.NumberOfWindows = 12;
                        break;
                    }
                case 3:
                    {
                        this.NumberOfWindows = 16;
                        break;
                    }
               
            }
           
        }

        private void AddGenerateButton ()
        {
            Button bt = new Button();
            bt.HorizontalAlignment = HorizontalAlignment.Right;
            bt.VerticalAlignment = VerticalAlignment.Center;
            bt.Content = "Generate";
            bt.Click += ButtonGenerate_Click;
            bt.Margin = new Thickness(10);
            Grid.SetRow(bt, 2);
            this.CenterGrid.Children.Add(bt);
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            MyMainGrid mainGrid = new MyMainGrid(this.NumberOfWindows);
            this.RemoveVisualChild(Generalgb);
            this.Generalgb.Visibility = Visibility.Collapsed;
            mainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            mainGrid.VerticalAlignment = VerticalAlignment.Stretch;
            DockPanel.SetDock(mainGrid, Dock.Top);
            this.Children.Add(mainGrid);
        }
    }
}
