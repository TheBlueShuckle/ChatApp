using ChatClient.MVVM.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp
{
    public partial class MainWindow : Window
    {
        MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = DataContext as MainViewModel;
        }

        public void TextBox_KeyEnterUpdate(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;

                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    DependencyProperty property = TextBox.TextProperty;

                    BindingExpression binding = BindingOperations.GetBindingExpression(textBox, property);

                    binding?.UpdateSource();

                    viewModel?.DirectToCorrectMethod(textBox.Text);
                    textBox.Clear();
                }
            }
        }
    }
}