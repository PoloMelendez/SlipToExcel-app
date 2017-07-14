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
using System.Windows.Xps.Packaging;

namespace SlipToExcel
{
    //Takes txt document packing slips given by a supplier and populates
    //an excel sheet with the relevant values, for recording a profit and loss
    public partial class MainWindow : Window
    {
        public static StackPanel currentSlips;
        public static TextBox prevSlips;
        WorkbookModel activeWorkbook;
       public MainWindow()
        {
            InitializeComponent();
            currentSlips = SlipsPanel;
            prevSlips = PreviewSlips;
            activeWorkbook = new WorkbookModel();
            initWorkbook();
            WorkBookName.DataContext =  activeWorkbook;
            SheetName.DataContext = activeWorkbook;
            TemplateName.DataContext = activeWorkbook;
        }

        private void WorkBookName_TextFormClicked(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xls";
            dlg.Filter = "Excel Files| *.xls; *.xlsx; *.xlsm";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                activeWorkbook.Workbook = dlg.FileName;
                Properties.Settings.Default.LastExcelWB = dlg.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void PackingSlipName_TextFormClicked(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files (*.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string path = dlg.FileName;
                string name = System.IO.Path.GetFileName(path);
                SlipBox box = new SlipBox(name, path);
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            SlipParser toParse = new SlipParser();
            try
            {
                string path = System.IO.Path.GetDirectoryName(SlipBox.slips[SlipBox.slips.Count - 1].slipPath);
                path += "/temp.txt";
                SlipCombiner tmp = new SlipCombiner(SlipBox.slips, path);
                tmp.combine();
                toParse.ReadFile(path);
                ExcelBuilder convert = new ExcelBuilder(activeWorkbook.WorkbookPath);
                convert.AddWorksheet(activeWorkbook.Worksheet, activeWorkbook.TemplatePath);
                convert.Convert(toParse.ExcelData);
                File.Delete(path);
                convert.Close();
                CheckMark.Visibility = Visibility.Visible;
            } catch(Exception ex)
            {
                MessageBox.Show("Make sure all to select a packing slip, and that all forms are filled!");
                return;
            }
        }

        private void TemplateName_TextFormClicked(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xltx";
            dlg.Filter = "Excel Files| *.xlt; *.xltx; *.xltm";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                activeWorkbook.Template = dlg.FileName;
                Properties.Settings.Default.LastExcelTemplate = dlg.FileName;
                Properties.Settings.Default.Save();
            }
        }

        void initWorkbook()
        {
            activeWorkbook.Workbook = Properties.Settings.Default.LastExcelWB;
            activeWorkbook.Template = Properties.Settings.Default.LastExcelTemplate;
        }
    }
}
