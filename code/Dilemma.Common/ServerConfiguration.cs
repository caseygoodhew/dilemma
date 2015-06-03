using System;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using Disposable.Common.Extensions;

namespace Dilemma.Common
{
	/// <summary>
	/// The server configuration model.
	/// </summary>
	public class ServerConfiguration
	{
		public ServerRole ServerRole { get; set; }

		public static ServerConfiguration Read()
		{
			var serverRoleValue = WebConfigurationManager.AppSettings["ServerRole"];

			var serverRole = ServerRole.Offline;

			if (!serverRoleValue.IsNullOrEmpty())
			{
				var serverRoleDynamic =
					EnumExtensions.All<ServerRole>()
						.Select(x => new {Value = x, StringValue = x.ToString().ToUpper()})
						.SingleOrDefault(x => x.StringValue == serverRoleValue.ToUpper());

				if (serverRoleDynamic != null)
				{
					serverRole = serverRoleDynamic.Value;
				}
			}

			return new ServerConfiguration {ServerRole = serverRole};
		}

		public static void Write(ServerConfiguration serverConfiguration)
		{
			var webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");
			webConfigApp.AppSettings.Settings["ServerRole"].Value = serverConfiguration.ServerRole.ToString();
			webConfigApp.Save();
		}
	}
}
