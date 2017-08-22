using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.IO;
using System;

namespace Android_Client
{
    [Activity(Label = "Android_Client", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        // Android Service
        Intent serviceToStart;
        public static Button serviceButton, serverButton;
        bool isStarted;
        public static string id,pwd;
        public static TextView connexionStatus;





		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Reading ID and Password
            StreamReader Sr = new StreamReader(Assets.Open("id.txt"));
            id = Sr.ReadLine();
			pwd = Sr.ReadLine();

			// Services variables
			serviceToStart = new Intent(this, typeof(WatchClipboard));
			isStarted = WatchClipboard.isStarted;

            // Service Button management
            serviceButton = FindViewById<Button>(Resource.Id.serviceButton);
			serverButton = FindViewById<Button>(Resource.Id.serverButton);
			if (isStarted == false)
            {
				serviceButton.Text = ("Start Service");
				serviceButton.Click += startService_Click;
			} 
            else
            {
				serviceButton.Text = ("Stop Service");
				serviceButton.Click += stopService_Click;
			}

            // Server Button Management
            serverButton.Text = ("Login/Logout");
            serverButton.Enabled = false;
            serverButton.Click += openCloseConnexion;

			// Connexion Status
            connexionStatus = FindViewById<TextView>(Resource.Id.connexionStatus);
            connexionStatus.Text = "OffLine";
            WatchClipboard.client.OnError += ConnexionError;
            WatchClipboard.client.OnClose += ConnexionError;
		}


        void startService_Click(object sender, EventArgs e)
		{
			StartService(serviceToStart);

		    serviceButton.Click -= startService_Click;
			serviceButton.Click += stopService_Click;
			serviceButton.Text = ("Stop Service");   
		}

		void stopService_Click(object sender, EventArgs e)
		{
			StopService(serviceToStart);

			serviceButton.Click -= stopService_Click;
			serviceButton.Click += startService_Click;
			serviceButton.Text = ("Start Service");
		}

		// Open or close the connexion
		static void openCloseConnexion(object sender, EventArgs e)
		{
			serviceButton.Click -= openCloseConnexion;


			if (WatchClipboard.client.IsAlive)
			{
                WatchClipboard.client.Close();
                serverButton.Text = ("Login");
				connexionStatus.Text = "OffLine";
			}
			else
			{
				WatchClipboard.serverConnexion();
                serverButton.Text = ("Logout");
			}

		}

		// connexionStatus
		static void ConnexionError(object sender, EventArgs e)
        {
			connexionStatus.Text = "OffLine";
		}


		
    }
}

