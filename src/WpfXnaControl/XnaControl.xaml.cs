using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WpfXnaControl
{
	public partial class XnaControl : UserControl
	{
		private TimeSpan _totalTime;
		private TimeSpan _elapsedTime;

		private GraphicsDeviceService _graphicsDeviceService;
		private XnaImageSource _imageSource;
		private bool isInit = false;

		private readonly ServiceContainer _services = new();
		private ContentManager contentManager;

		public event Action Initialize;
		public event Action<ContentManager> LoadContent;
		public event Action<GameTime> Update;
		public event Action Draw;

		public GraphicsDevice GraphicsDevice => _graphicsDeviceService.GraphicsDevice;
		public ServiceContainer Services => _services;
		public ContentManager ContentManager => contentManager;

		public bool IsXNAInitialized => isInit;

		public XnaControl()
		{
			InitializeComponent();
			Loaded += XnaControl_Loaded;

		}

		~XnaControl()
		{
			_imageSource?.Dispose();
			_graphicsDeviceService?.Release();
		}

		private void XnaControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (PresentationSource.FromVisual(this) == null || isInit)
				return;
			isInit = true;
			if (DesignerProperties.GetIsInDesignMode(this) == false)
			{
				InitializeGraphicsDevice();
				Initialize?.Invoke();
				if (LoadContent != null)
				{
					contentManager = new ContentManager(_services, "Content");
					LoadContent(ContentManager);
				}
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			if (!DesignerProperties.GetIsInDesignMode(this) && _graphicsDeviceService != null)
			{
				_imageSource?.Dispose();
				_imageSource = new XnaImageSource(GraphicsDevice, (int)ActualWidth, (int)ActualHeight);
				RootImage.Source = _imageSource.WriteableBitmap;
			}
			base.OnRenderSizeChanged(sizeInfo);
		}

		private void InitializeGraphicsDevice()
		{
			if (_graphicsDeviceService != null)
				return;
			_graphicsDeviceService = GraphicsDeviceService.AddRef((PresentationSource.FromVisual(this) as HwndSource).Handle);

			_imageSource = new XnaImageSource(GraphicsDevice, (int)ActualWidth, (int)ActualHeight);
			RootImage.Source = _imageSource.WriteableBitmap;

			_services.AddService(typeof(IGraphicsDeviceService), _graphicsDeviceService);

			_totalTime = new TimeSpan(DateTime.Now.Ticks);
			_elapsedTime = new TimeSpan(DateTime.Now.Ticks);
		}

		protected virtual void RenderOverride()
		{
			if (Update != null)
			{
				var now = new TimeSpan(DateTime.Now.Ticks);
				Update(new GameTime(now - _totalTime, now - _elapsedTime));
				_elapsedTime = new TimeSpan(DateTime.Now.Ticks);
			}
			Draw?.Invoke();
		}

		public void Render()
		{
			GraphicsDevice.SetRenderTarget(_imageSource.RenderTarget);
			RenderOverride();
			GraphicsDevice.SetRenderTarget(null);
			_imageSource.Commit();
		}
	}
}