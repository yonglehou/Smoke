using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke
{
	public interface ISenderFactory
	{
		/// <summary>
		/// Gets a bool flag indicating whether the sender is available for construction
		/// </summary>
		bool Available { get; }


		/// <summary>
		/// Creates a new instance of an ISender
		/// </summary>
		ISender Sender();
	}
}
