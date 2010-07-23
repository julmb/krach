using System.ComponentModel;
using System.Xml.Linq;

namespace Utility
{
	public abstract class XSerializable
	{
		readonly string xElementName;

		[Browsable(false)]
		public string XElementName { get { return xElementName; } }
		[Browsable(false)]
		public abstract XElement XElement { get; set; }

		public XSerializable(string xElementName)
		{
			this.xElementName = xElementName;
		}
	}
}
