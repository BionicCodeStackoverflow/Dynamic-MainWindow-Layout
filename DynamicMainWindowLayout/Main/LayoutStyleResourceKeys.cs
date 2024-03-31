namespace DynamicMainWindowLayout.Main
{
  using System.Windows;

  public sealed class LayoutStyleResourceKeys
  {
    // Define a resource key for each dynamic resource i.e. each resource that appear in each layout style
    public static ComponentResourceKey MainWindowStyleKey = new ComponentResourceKey(typeof(LayoutStyleResourceKeys), nameof(MainWindowStyleKey));
  }
}