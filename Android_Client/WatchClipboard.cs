using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using WebSocketSharp;
using Newtonsoft.Json;
using System.IO;


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
		static private WebSocket client;
		static string pwd;
 


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

                // Open Password Information
                //StreamReader Sr = File.OpenText("id.txt");
                //Sr.ReadLine(); pwd = Sr.ReadLine();
                pwd = "passwd";

				// Websocket Server
				//client = new WebSocket ("ws://34.248.0.158:8080"); 
				client = new WebSocket("ws://192.168.1.64:8080");

				// Connexion + send ID
				serverConnexion();

                // Clipboard Manager
				ruir = new MyClipChangedListener();
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
			base.OnDestroy();
		}


        public static void sendClipboard()
        {

			// Encryption of the pasteboard
			//string decryptedMsg = clipBoard.PrimaryClip.GetItemAt(0).Text;
			//string encryptedMsg = StringCipher.Encrypt(decryptedMsg,pwd);
            string encryptedMsg = StringCipher.Encrypt(clipBoard.Text, pwd);

			// JSON
			MsgJSON message = new MsgJSON("msg");
			message.msg = encryptedMsg;
			string sMsgJson = JsonConvert.SerializeObject(message);

			// Sending 
			client.Send(sMsgJson);

            // Debug
            Log.Debug("lol", clipBoard.Text);

		}


        public static void ReceiveClipboard(object sender, MessageEventArgs e)
        {
			// Decryption
			string decryptedMsg = StringCipher.Decrypt(e.Data, pwd);

            // Setting the clipboard without resending it
            clipBoard.RemovePrimaryClipChangedListener(ruir);
            clipBoard.Text = decryptedMsg;
            clipBoard.AddPrimaryClipChangedListener(ruir);
			
        }


		// Connect + Send ID
		public void serverConnexion()
		{
			client.Connect();
			MsgJSON message = new MsgJSON("id");
			string sMsgJson = JsonConvert.SerializeObject(message);
			client.Send(sMsgJson);

			//if (client.IsAlive)
			//{
			//	connexionStatus.Title = "Status: OnLine";
			//	connectDisconnectTitle.Title = "Log Out";
			//}
			//else
			//{
			//	connexionStatus.Title = "Status: OffLine";
			//	connectDisconnectTitle.Title = "Log In";
			//}

		}




	}

    public class MyClipChangedListener : Java.Lang.Object, ClipboardManager.IOnPrimaryClipChangedListener
    {




		public void OnPrimaryClipChanged()
		{
            // Selection of the clipBoard

            WatchClipboard.sendClipboard();
		}
	}




}

