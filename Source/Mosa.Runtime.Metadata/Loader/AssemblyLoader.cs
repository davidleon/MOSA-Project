/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader.PE;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Loader;

namespace Mosa.Runtime.Metadata.Loader
{
	/// <summary>
	/// Provides a default implementation of the IAssemblyLoader interface.
	/// </summary>
	public class AssemblyLoader : IAssemblyLoader
	{
		#region Data members

		private List<string> privatePaths = new List<string>();
		private object loaderLock = new object();

		private List<IMetadataModule> modules = new List<IMetadataModule>();

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
		/// </summary>
		public AssemblyLoader()
		{
		}

		#endregion // Construction

		#region IAssemblyLoader

		/// <summary>
		/// Loads the named assembly.
		/// </summary>
		/// <param name="file">The file path of the assembly to load.</param>
		/// <returns>
		/// The assembly image of the loaded assembly.
		/// </returns>
		IMetadataModule IAssemblyLoader.LoadModule(string file)
		{
			lock (loaderLock)
			{
				IMetadataModule module = LoadDependencies(file);

				Debug.Assert(module != null);
				Debug.Assert(module.Metadata != null);

				return module;
			}
		}

		/// <summary>
		/// Gets the modules.
		/// </summary>
		/// <value>The modules.</value>
		IList<IMetadataModule> IAssemblyLoader.Modules { get { return modules.AsReadOnly(); } }

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		void IAssemblyLoader.AddPrivatePath(string path)
		{
			lock (loaderLock)
			{
				if (!privatePaths.Contains(path))
					privatePaths.Add(path);
			}
		}

		/// <summary>
		/// Initializes the private paths.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		void IAssemblyLoader.InitializePrivatePaths(IEnumerable<string> assemblyPaths)
		{
			foreach (string path in FindPrivatePaths(assemblyPaths))
				((IAssemblyLoader)this).AddPrivatePath(path);
		}

		#endregion // IAssemblyLoader

		#region Internals

		private bool isLoaded(string name)
		{
			foreach (IMetadataModule module in modules)
				if (module.Name == name)
					return true;

			return false;
		}

		private IMetadataModule LoadDependencies(string file)
		{
			IMetadataModule metadataModule = LoadAssembly(file);

			if (!isLoaded(metadataModule.Name))
			{
				Token maxToken = metadataModule.Metadata.GetMaxTokenValue(TableType.AssemblyRef);
				//for (TokenTypes token = TokenTypes.AssemblyRef + 1; token <= maxToken; token++)
				foreach (Token token in new Token(TableType.AssemblyRef, 1).Upto(maxToken))
				{
					AssemblyRefRow row = metadataModule.Metadata.ReadAssemblyRefRow(token);
					string assembly = metadataModule.Metadata.ReadString(row.Name);

					LoadDependencies(assembly);
				}

				if (!isLoaded(metadataModule.Name))
					modules.Add(metadataModule);
			}

			return metadataModule;
		}

		public static string CreateFileCodeBase(string file)
		{
			return @"file://" + file.Replace('\\', '/');
		}

		/// <summary>
		/// Loads the PE assembly.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		private IMetadataModule LoadPEAssembly(string file)
		{
			if (!File.Exists(file))
				return null;

			string codeBase = CreateFileCodeBase(file);

			return PortableExecutableImage.Load(new FileStream(file, FileMode.Open, FileAccess.Read), codeBase);
		}

		private IMetadataModule LoadAssembly(string file)
		{
			if (Path.IsPathRooted(file))
			{
				return LoadPEAssembly(file);
			}

			file = Path.GetFileName(file);

			if (!file.EndsWith(".dll"))
				file = file + ".dll";

			IMetadataModule result = TryAssemblyLoadFromPaths(file, privatePaths);

			return result;
		}

		/// <summary>
		/// Does the load assembly From paths.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="paths">The paths.</param>
		/// <returns></returns>
		private IMetadataModule TryAssemblyLoadFromPaths(string name, IEnumerable<string> paths)
		{
			foreach (string path in paths)
			{
				try
				{
					IMetadataModule result = LoadPEAssembly(Path.Combine(path, name));

					if (result != null)
					{ 
						return result;
					}
				}
				catch
				{
					/* Failed to load assembly there... */
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the private paths.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		/// <returns></returns>
		private IEnumerable<string> FindPrivatePaths(IEnumerable<string> assemblyPaths)
		{
			List<string> privatePaths = new List<string>();

			foreach (string assembly in assemblyPaths)
			{
				string path = Path.GetDirectoryName(assembly);
				if (!privatePaths.Contains(path))
					privatePaths.Add(path);
			}

			return privatePaths;
		}

		#endregion // Internals

	}
}