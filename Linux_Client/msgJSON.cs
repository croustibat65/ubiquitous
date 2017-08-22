using System.IO;

namespace Linux_Client
{
	class msgJSON
	{
		public string name;// = File.OpenText("id.txt").ReadToEnd();
		public string msg;
		public string type;

		// constructor
		public msgJSON(string s)
		{
			name = File.OpenText("id.txt").ReadLine();
			msg = null;
			type = s;
		}

	}
}
