using System;
using System.Collections.Generic;
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
using System.IO;

namespace SlipToExcel
{
    /// <summary>
    /// Interaction logic for SlipBox.xaml
    /// </summary>
    public partial class SlipBox : UserControl, IEquatable<SlipBox>
    {
        public static List<SlipBox> slips = new List<SlipBox>();
        public string slipName { get; set; }
        public string slipPath { get; set; }

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(SlipBox), null);

        //When the object is created it is autoamtically added to the list in MainWindow()
        //Also sets up a "preview" window for the most recently selected packing slip
        public SlipBox(string name, string path)
        {
            InitializeComponent();
            slipName = name;
            slipPath = path;
            FileName = slipName;
            if(!slips.Contains(this))
            {
                slips.Add(this);
                MainWindow.currentSlips.Children.Add(this);
                StreamReader reader = new StreamReader(slipPath);
                MainWindow.prevSlips.Visibility = Visibility.Visible;
                MainWindow.prevSlips.Text = reader.ReadToEnd();
                reader.Close();
            }
        }
        //When the X is clicked: 
        //  1. remove the object from the list
        //  2. remove it from the mainwindow
        //  3. change the preview, or if no more items in list collapse
        private void InnerButton_Click(object sender, RoutedEventArgs e)
        {
            slips.Remove(this);
            MainWindow.currentSlips.Children.Remove(this);
            if(slips.Count <= 0)
            {
                MainWindow.prevSlips.Visibility = Visibility.Collapsed;
            } else
            {
                MainWindow.prevSlips.Text = new StreamReader(slips[slips.Count - 1].slipPath).ReadToEnd();
            }
        }
        //Override the Equals method to make use of List.Contains() later
        public bool Equals(SlipBox other)
        {
            if (other == null) return false;
            return this.slipName == other.slipName &&
                this.slipPath == other.slipPath;
        }
    }
}
