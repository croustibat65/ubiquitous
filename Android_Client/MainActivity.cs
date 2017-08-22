using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;
using System;

namespace Android_Client
{
    [Activity(Label = "Android_Client", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        // Android Service
        Intent serviceToStart;
        Button myButton;
        bool isStarted;






		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

			// Services variables
			serviceToStart = new Intent(this, typeof(WatchClipboard));
			isStarted = WatchClipboard.isStarted;

			// Button management
			myButton = FindViewById<Button>(Resource.Id.myButton);
            if (isStarted == false)
            {
				myButton.Text = ("Start");
				myButton.Click += startService_Click;
			} 
            else
            {
				myButton.Text = ("Stop");
				myButton.Click += stopService_Click;
			}    
			


		}


        void startService_Click(object sender, EventArgs e)
		{
			StartService(serviceToStart);

			myButton.Click -= startService_Click;
			myButton.Click += stopService_Click;
			myButton.Text = ("Stop");
		}

		void stopService_Click(object sender, EventArgs e)
		{
			StopService(serviceToStart);

			myButton.Click -= stopService_Click;
			myButton.Click += startService_Click;
			myButton.Text = ("Start");
		}

		
    }
}

