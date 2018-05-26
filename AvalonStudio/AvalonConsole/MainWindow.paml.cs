using Avalonia.Markup.Xaml;
using Avalonia.Controls;

namespace AvalonConsole
{
  public class MainWindow : Window
  {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}