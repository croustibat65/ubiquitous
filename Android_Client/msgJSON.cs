using System.IO;


namespace Android_Client
{

	public class MsgJSON
	{
		public string name;
		public string msg;
		public string type;

		// constructor
		public MsgJSON(string s)
		{
            name = "ruireur";//File.OpenText("../../../id.txt").ReadLine();
			msg = null;
			type = s;
		}
	}

}
