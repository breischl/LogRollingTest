using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.LayoutRenderers;

namespace LogRollingTest
{
	/// <summary>
	/// An NLog LayoutRenderer that writes out the AssemblyVersion of a specified assembly. 
	/// If the specified assembly cannot be found (or is not configured) then it will attempt to use the entry assembly. 
	/// </summary>
	/// <remarks>
	/// The primary motivation for this is that the ${assembly-version} layout renderer that comes with NLog does not work with ASP.NET applications.
	/// Secondarily, the standard renderer the class, assembly, and version for each log message, whereas this one does it once and caches the results, so this should perform slightly better.
	/// </remarks>
	[LayoutRenderer("asmver")]
	[ThreadAgnostic]
	public class AssemblyVersionLayoutRenderer : LayoutRenderer
	{
		/// <summary>
		/// Specifies the assembly name for which the version will be displayed.
		/// If no name is specified tries to get the entry assembly.
		/// </summary>
		public string AssemblyName { get; set; }

		private readonly Lazy<string> _assemblyVersion;

		public AssemblyVersionLayoutRenderer()
		{
			//Initialize the Lazy that will find our assembly version
			_assemblyVersion = new Lazy<string>(() =>
			{
				InternalLogger.Debug("Loading version for assembly '{0}'", AssemblyName);

				Assembly assembly = null;
				if (!string.IsNullOrEmpty(AssemblyName))
				{
					// try to get assembly based on its name
					assembly = AppDomain.CurrentDomain.GetAssemblies()
										  .FirstOrDefault(a => string.Equals(a.GetName().Name, AssemblyName, StringComparison.InvariantCultureIgnoreCase));
				}

				if (assembly == null)
				{
					//This will be null for ASP.NET apps
					assembly = Assembly.GetEntryAssembly();
				}

				if (assembly != null)
				{
					return assembly.GetName().Version.ToString();
				}
				else
				{
					return "<Assembly version not found>";
				}
			});
		}

		/// <summary>
		/// Renders the current trace activity ID.
		/// </summary>
		/// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
		/// <param name="logEvent">Logging event.</param>
		protected override void Append(StringBuilder builder, LogEventInfo logEvent)
		{
			builder.Append(_assemblyVersion.Value);
		}
	}
}
