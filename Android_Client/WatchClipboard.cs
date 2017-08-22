using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using WebSocketSharp;
using Newtonsoft.Json;


namespace Android_Client
{

	[Service]
	public class WatchClipboard : Service
	{
        // Android
        static readonly string TAG = "X:" + typeof(WatchClipboard).Name;
	    static public bool isStarted = false;
        // Clipboard
		static ClipboardManager clipBoard;
        static ClipboardManager.IOnPrimaryClipChangedListener ruir;
		// Websocket
		static public WebSocket client = new WebSocket("ws://192.168.1.64:8080");
 


		public override void OnCreate()
		{
			base.OnCreate();
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			Log.Debug(TAG, $"Starting");
			clipBoard = (ClipboardManager)GetSystemService(ClipboardService);


			if (isStarted)
			{


			}
			else
			{
                isStarted = true;

				// Websocket Server
				//client = new WebSocket ("ws://34.248.0.158:8080"); 
				//client = new WebSocket("ws://192.168.1.64:8080");

				// Connexion + send ID
				serverConnexion();

                // Clipboard Manager
				ruir = new MyClipChangedListener();

                // Clipboard Listener
				clipBoard.AddPrimaryClipChangedListener(ruir);

				// Paste/Reception
				client.OnMessage += ReceiveClipboard;


			}
			return StartCommandResult.NotSticky;
		}


		public override IBinder OnBind(Intent intent)
		{
			// This is a started service, not a bound service, so we just return null.
			return null;
		}


		public override void OnDestroy()
		{
			isStarted = false;

            // Clipboard
            clipBoard.RemovePrimaryClipChangedListener(ruir);
            ruir.Dispose();

            // Server
            client.Close();

            // Android
			Log.Debug(TAG, $"Quitting");

            // Button
            MainActivity.serverButton.Text = ("Login/Logout");
			MainActivity.serverButton.Enabled = false;


			base.OnDestroy();
		}


        public static void sendClipboard()
        {

			// Encryption of the pasteboard
            string encryptedMsg = StringCipher.Encrypt(clipBoard.Text, MainActivity.pwd);

			// JSON
			MsgJSON message = new MsgJSON("msg");
			message.msg = encryptedMsg;
			string sMsgJson = JsonConvert.SerializeObject(message);

			// Sending 
			client.Send(sMsgJson);

		}


        public static void ReceiveClipboard(object sender, MessageEventArgs e)
        {
			// Decryption
			string decryptedMsg = StringCipher.Decrypt(e.Data, MainActivity.pwd);

            // Setting the clipboard without resending it
            clipBoard.RemovePrimaryClipChangedListener(ruir);
            clipBoard.Text = decryptedMsg;
            clipBoard.AddPrimaryClipChangedListener(ruir);
			
        }


		// Connect + Send ID
		public static void serverConnexion()
		{
			client.Connect();
            if (client.IsAlive)
            {
				MsgJSON message = new MsgJSON("id");
				string sMsgJson = JsonConvert.SerializeObject(message);
				client.Send(sMsgJson);
                MainActivity.serverButton.Text = ("Logout");
				MainActivity.connexionStatus.Text = "OnLine";
			}
            else
            {
                MainActivity.serverButton.Text = ("Login");
				MainActivity.connexionStatus.Text = "OffLine";
			}
            MainActivity.serverButton.Enabled = true;




		}




	}

    public class MyClipChangedListener : Java.Lang.Object, ClipboardManager.IOnPrimaryClipChangedListener
    {




		public void OnPrimaryClipChanged()
		{

            WatchClipboard.sendClipboard();
		}
	}




}

