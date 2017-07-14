using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipToExcel
{
    //Models an excel workbook and the contained worksheets
    class WorkbookModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _Workbook = "Select a workbook";
        private string _WorkbookPath;
        private string _Worksheet = DateTime.Today.ToString("M-d");
        private string _Template = "Choose a template";
        private string _TemplatePath;
        public string Workbook
        {
            get { return _Workbook; }
            set
            {
                _WorkbookPath = value;
                _Workbook = System.IO.Path.GetFileName(value);
                OnPropertyChanged("Workbook");
            }
        }
        public string WorkbookPath { get { return _WorkbookPath; } }
        public string Worksheet
        {
            get { return _Worksheet; }
            set
            {
                _Worksheet = value;
                OnPropertyChanged("Worksheet");
            }
        }
        public string Template
        {
            get { return _Template; }
            set
            {
                _TemplatePath = value;
                _Template = System.IO.Path.GetFileName(value);
                OnPropertyChanged("Template");
            }
        }
        public string TemplatePath { get { return _TemplatePath; } }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
