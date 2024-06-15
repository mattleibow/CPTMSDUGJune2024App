using Microsoft.Maui.Platform;

namespace Exciting.Mobile;

public partial class MainWindow : Window
{
	IDisposable? _frameObserver;

	public MainWindow()
	{
		InitializeComponent();
	}

	protected override void OnHandlerChanged()
	{
		_frameObserver?.Dispose();

		base.OnHandlerChanged();

#if IOS || MACCATALYST

		// workaround for https://github.com/dotnet/maui/issues/19197
		// code from https://github.com/dotnet/maui/pull/20987

		if (Handler?.PlatformView is UIKit.UIWindow window)
		{
			_frameObserver = window.AddObserver(
				"frame",
				Foundation.NSKeyValueObservingOptions.New,
				FrameAction);

			void FrameAction(Foundation.NSObservedChange obj)
			{
				((IWindow)this).FrameChanged(window.Bounds.ToRectangle());
			}
		}

#endif
	}
}
