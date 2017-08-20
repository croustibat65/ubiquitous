using System.IO;


namespace Mac_Client
{

	public class MsgJSON
	{
		public string name;
		public string msg;
		public string type;

		// constructor
		public MsgJSON(string s)
		{
			name = File.OpenText("../../../../../id.txt").ReadLine();
			msg = null;
			type = s;
		}
	}
	
}
