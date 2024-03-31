namespace DynamicMainWindowLayout.Main
{
  using System.IO;
  using System.Text;
  using System.Windows;
  using System.Windows.Baml2006;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Documents;
  using System.Windows.Input;
  using System.Windows.Markup;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using System.Windows.Navigation;
  using System.Windows.Resources;
  using System.Windows.Shapes;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public static RoutedCommand ChangeLayoutCommand { get; }
      = new RoutedCommand(nameof(MainWindow.ChangeLayoutCommand), typeof(MainWindow));

    public MainWindow()
    {
      InitializeComponent();

      var changeLayoutCommandBinding = new CommandBinding(MainWindow.ChangeLayoutCommand, ChangeLayoutCommandExecuted, CanExecuteChangeLayoutCommand);
      this.CommandBindings.Add(changeLayoutCommandBinding);
    }

    private void CanExecuteChangeLayoutCommand(object sender, CanExecuteRoutedEventArgs e)
      => e.CanExecute = e.Parameter is LayoutMode;

    private async void ChangeLayoutCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      var layoutMode = (LayoutMode)e.Parameter;
      Uri resourceUri = layoutMode switch
      {
        LayoutMode.Portrait => new Uri("PortraitLayoutResources.xaml", UriKind.RelativeOrAbsolute),
        _ => new Uri("DefaultLayoutResources.xaml", UriKind.RelativeOrAbsolute),
      };

      await LoadLayoutAsync(resourceUri);
    }

    private async Task LoadLayoutAsync(Uri uri)
    {
      StreamResourceInfo resourceStreamInfo = Application.GetResourceStream(uri);
      await using Stream bamlResourceStream = resourceStreamInfo.Stream;
      bamlResourceStream.Seek(0, SeekOrigin.Begin);
      using var bamlReader = new Baml2006Reader(bamlResourceStream);
      var resources = (ResourceDictionary)XamlReader.Load(bamlReader);
      Application.Current.Resources.MergedDictionaries.RemoveAt(0);
      Application.Current.Resources.MergedDictionaries.Add(resources);
    }
  }
}