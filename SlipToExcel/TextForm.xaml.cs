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

namespace SlipToExcel
{
    /// <summary>
    /// Interaction logic for TextForm.xaml
    /// </summary>
    public partial class TextForm : UserControl
    {
        public event EventHandler TextFormClicked;
        public string TextContent
        {
            get { return (string)GetValue(TextContentProperty); }
            set { SetValue(TextContentProperty, value); }
        }

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public bool isProtected
        {
            get { return (bool)GetValue(isProtectedProperty); }
            set { SetValue(isProtectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isProtected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isProtectedProperty =
            DependencyProperty.Register("isProtected", typeof(bool), typeof(TextForm), null);

        // Using a DependencyProperty as the backing store for ButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(string), typeof(TextForm), null);

        // Using a DependencyProperty as the backing store for TextContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextContentProperty =
            DependencyProperty.Register("TextContent", typeof(string), typeof(TextForm), null);


        public TextForm()
        {
            InitializeComponent();
        }

        private void buttonLabel_Click(object sender, RoutedEventArgs e)
        {
            if (TextFormClicked != null)
            {
                TextFormClicked(this, EventArgs.Empty);
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isProtected)
            {
                return;
            }
            else
            {
                TextContent = "";
            }
        }
    }
}
