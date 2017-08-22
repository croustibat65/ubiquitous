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
            name = MainClass.id;
			msg = null;
			type = s;
		}
	}
	
}
