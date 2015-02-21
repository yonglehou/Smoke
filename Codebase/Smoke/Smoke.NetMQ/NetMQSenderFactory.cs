using NetMQ;
using Smoke.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoke.NetMQ
{
	public class NetMQSenderFactory : ISenderFactory
	{
		/// <summary>
		/// Stores the address of the server that this ISenderFactor initializes ISenders to connect to
		/// </summary>
		private readonly String address;


		/// <summary>
		/// Stores a readonly reference to the NetMQContext that creates NetMQSockets
		/// </summary>
		private readonly NetMQContext context;


		/// <summary>
		/// Initializes a new instance of a NetMQSenderFactory
		/// </summary>
		/// <param name="address"></param>
		/// <param name="context"></param>
		public NetMQSenderFactory(String address, NetMQContext context)
		{
			if (String.IsNullOrWhiteSpace(address))
				throw new ArgumentException("Address is null or empty");

			if (context == null)
				throw new ArgumentNullException("NetMQContext");

			this.address = address;
			this.context = context;
		}


		/// <summary>
		/// Gets a bool flag indicating whether the sender is available for construction
		/// </summary>
		public bool Available
		{ get { return true; } }


		/// <summary>
		/// Creates a new instance of an ISender
		/// </summary>
		public ISender Sender()
		{
			var sender = new NetMQSender(context.CreateDealerSocket(), new BinarySerializer(), address);
			sender.Connect();
			return sender;
		}
	}
}
