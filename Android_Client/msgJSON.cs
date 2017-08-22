

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

            name = MainActivity.id;
			msg = null;
			type = s;
		}
	}

}
