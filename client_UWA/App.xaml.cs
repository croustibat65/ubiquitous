using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;



namespace client_UWA
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private MessageWebSocket messageWebSocket;
        private DataWriter messageWriter;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            OnConnect();
            OnSend();
        }

        public async void OnConnect()
        {
            await ConnectAsync();
        }
        async void OnSend()
        {
            await SendAsync();
        }
        async Task SendAsync()
        {
            string message = "hello bru";

            // Buffer any data we want to send.
            messageWriter.WriteString(message);

            try
            {
                // Send the data as one complete message.
                await messageWriter.StoreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private async Task ConnectAsync()
        {
            string message = "test";

            // Validating the URI is required since it was received from an untrusted source (user input).
            // The URI is validated by calling TryGetUri() that will return 'false' for strings that are not
            // valid WebSocket URIs.
            // Note that when enabling the text box users may provide URIs to machines on the intrAnet
            // or intErnet. In these cases the app requires the "Home or Work Networking" or
            // "Internet (Client)" capability respectively.
            Uri server = new Uri("ws://127.0.0.1:8080");
            if (server == null)
            {
                return;
            }

            messageWebSocket = new MessageWebSocket();
            messageWebSocket.Control.MessageType = SocketMessageType.Utf8;
            messageWebSocket.MessageReceived += MessageReceived;
            messageWebSocket.Closed += OnClosed;

            // If we are connecting to wss:// endpoint, by default, the OS performs validation of
            // the server certificate based on well-known trusted CAs. We can perform additional custom
            // validation if needed.

            try
            {
                await messageWebSocket.ConnectAsync(server);
            }
            catch (Exception ex) // For debugging
            {
                // Error happened during connect operation.
                messageWebSocket.Dispose();
                messageWebSocket = null;

                return;
            }

            // The default DataWriter encoding is Utf8.
            messageWriter = new DataWriter(messageWebSocket.OutputStream);
            //rootPage.NotifyUser("Connected", NotifyType.StatusMessage);
        }

        private async void OnClosed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            // Dispatch the event to the UI thread so we do not need to synchronize access to messageWebSocket.
                //AppendOutputLine("Closed; Code: " + args.Code + ", Reason: " + args.Reason);

                //if (messageWebSocket == sender)
                //{
                //    CloseSocket();
                //    UpdateVisualState();
                //}
        }

        private void MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            using (DataReader reader = args.GetDataReader())
            {
                reader.UnicodeEncoding = UnicodeEncoding.Utf8;

                try
                {
                    string read = reader.ReadString(reader.UnconsumedBufferLength);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //Console.WriteLine("Error receiving message");
                }
            }
        }



        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
