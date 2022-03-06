using System;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace WpfXnaControl
{
    public class GraphicsDeviceService : IGraphicsDeviceService
    {
        private static GraphicsDeviceService _singletonInstance;

        private static int _referenceCount;

		public static GraphicsDeviceService Instance => _singletonInstance ??= new GraphicsDeviceService();

		private PresentationParameters _parameters;

        public GraphicsDevice GraphicsDevice { get; private set; }

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
#pragma warning disable CS0067
        [Obsolete("This event won't be triggered")]
        public event EventHandler<EventArgs> DeviceReset;
        [Obsolete("This event won't be triggered")]
        public event EventHandler<EventArgs> DeviceResetting;
#pragma warning restore CS0067

        GraphicsDeviceService() { }

        private void CreateDevice(IntPtr windowHandle)
        {
			_parameters = new PresentationParameters
			{
				BackBufferWidth = 480,
				BackBufferHeight = 320,
				BackBufferFormat = SurfaceFormat.Color,
				DeviceWindowHandle = windowHandle,
				DepthStencilFormat = DepthFormat.Depth24Stencil8,
				IsFullScreen = false
			};

            GraphicsDevice = new GraphicsDevice(
                GraphicsAdapter.DefaultAdapter,
                GraphicsProfile.HiDef,
                _parameters);

			DeviceCreated?.Invoke(this, EventArgs.Empty);
		}

        public static GraphicsDeviceService AddRef(IntPtr windowHandle)
        {
            if (Interlocked.Increment(ref _referenceCount) == 1)
            {
                Instance.CreateDevice(windowHandle);
            }

            return _singletonInstance;
        }

        public void Release()
        {
            if (Interlocked.Decrement(ref _referenceCount) != 0)
                return;

			DeviceDisposing?.Invoke(this, EventArgs.Empty);
			GraphicsDevice.Dispose();
            GraphicsDevice = null;
        }
    }
}