namespace App9
{
  using System;
  using Windows.Foundation;
  using Windows.UI.Xaml;
  using Windows.UI.Xaml.Controls;
  using Windows.UI.Xaml.Input;
  using Windows.UI.Xaml.Markup;
  using Windows.UI.Xaml.Media;

  // these names line up with visual states in generic.xaml so
  // rename at your peril.
  enum SwipeDirection
  {
    Left,
    Right,
    Default
  };
  class SwipeEventArgs : EventArgs
  {
    public SwipeDirection Direction { get; set; }
  }
  [ContentProperty(Name = "Content")]
  class SwipeContentControl : Control
  {
    public event EventHandler<SwipeEventArgs> Swiped;

    public static DependencyProperty LeftContentProperty =
      DependencyProperty.Register(
        "LeftContent", typeof(object), typeof(SwipeContentControl), null);

    public static DependencyProperty RightContentProperty =
      DependencyProperty.Register(
        "RightContent", typeof(object), typeof(SwipeContentControl), null);

    public static DependencyProperty ContentProperty =
      DependencyProperty.Register(
        "Content", typeof(object), typeof(SwipeContentControl), null);

    public static DependencyProperty LeftContentTemplateProperty =
      DependencyProperty.Register(
        "LeftContentTemplate", typeof(DataTemplate), typeof(SwipeContentControl), null);

    public static DependencyProperty RightContentTemplateProperty =
      DependencyProperty.Register(
        "RightContentTemplate", typeof(DataTemplate), typeof(SwipeContentControl), null);

    public static DependencyProperty ContentTemplateProperty =
      DependencyProperty.Register(
        "ContentTemplate", typeof(DataTemplate), typeof(SwipeContentControl), null);


    public object LeftContent
    {
      get
      {
        return (base.GetValue(LeftContentProperty));
      }
      set
      {
        base.SetValue(LeftContentProperty, value);
      }
    }
    public DataTemplate LeftContentTemplate
    {
      get
      {
        return ((DataTemplate)base.GetValue(LeftContentTemplateProperty));
      }
      set
      {
        base.SetValue(LeftContentTemplateProperty, value);
      }
    }
    public DataTemplate RightContentTemplate
    {
      get
      {
        return ((DataTemplate)base.GetValue(RightContentTemplateProperty));
      }
      set
      {
        base.SetValue(RightContentTemplateProperty, value);
      }
    }
    public DataTemplate ContentTemplate
    {
      get
      {
        return ((DataTemplate)base.GetValue(ContentTemplateProperty));
      }
      set
      {
        base.SetValue(ContentTemplateProperty, value);
      }
    }
    public object RightContent
    {
      get
      {
        return (base.GetValue(RightContentProperty));
      }
      set
      {
        base.SetValue(RightContentProperty, value);
      }
    }
    public object Content
    {
      get
      {
        return (base.GetValue(ContentProperty));
      }
      set
      {
        base.SetValue(ContentProperty, value);
      }
    }
    public SwipeContentControl()
    {
      this.DefaultStyleKey = typeof(SwipeContentControl);
      this.ManipulationMode = ManipulationModes.TranslateX;
      this.ManipulationDelta += OnManipulationDelta;
      this.ManipulationCompleted += OnManipulatedCompleted;
    }
    void OnManipulatedCompleted(object sender,
      ManipulationCompletedRoutedEventArgs e)
    {
      this.TranslateContent(0);

      if (IsManipulationSignificant(e.Cumulative.Translation.X))
      {
        SwipeEventArgs args = new SwipeEventArgs()
        {
          Direction = e.Cumulative.Translation.X < 0 ?
            SwipeDirection.Left : SwipeDirection.Right
        };
        this.Swiped?.Invoke(this, args);
      }
    }
    bool IsManipulationSignificant(double x)
    {
      bool significant = Math.Abs(x) >
        (SIGNIFICANT_TRANSLATE_FACTOR * this.contentElement.ActualWidth);

      return (significant);
    }
    void OnManipulationDelta(object sender,
      ManipulationDeltaRoutedEventArgs e)
    {
      this.SetVisualStateForManipulation(e.Cumulative.Translation);
      this.TranslateContent(e.Cumulative.Translation.X);
    }
    void SetVisualStateForManipulation(Point p)
    {
      var direction = this.DirectionOfManipulation(p);

      VisualStateManager.GoToState(this, direction.ToString(), true);
    }
    void TranslateContent(double x)
    {
      if (this.contentElement != null)
      {
        // This may well break if there's already a transform on this element :-(
        TranslateTransform transform = 
          (this.contentElement.RenderTransform as TranslateTransform);

        if (transform == null)
        {
          transform = new TranslateTransform();
          this.contentElement.RenderTransform = transform;
        }
        transform.X = x;
      }
    }
    SwipeDirection DirectionOfManipulation(Point p)
    {
      SwipeDirection d = SwipeDirection.Default;

      if (p.X != 0)
      {
        d = p.X < 0 ? SwipeDirection.Left : SwipeDirection.Right;
      }
      return (d);
    }
    protected override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.contentElement = this.GetTemplateChild("contentPresenter") as FrameworkElement;
    }
    static readonly double SIGNIFICANT_TRANSLATE_FACTOR = 0.20d;
    FrameworkElement contentElement;
  }
}