using System.Diagnostics;
using Windows.UI.Xaml.Controls;


namespace App9
{
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }
    void OnSwiped(object sender, SwipeEventArgs args)
    {
      Debug.WriteLine($"We were swiped {args.Direction}");
    }
  }
}
