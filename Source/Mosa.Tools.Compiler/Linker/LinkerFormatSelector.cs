/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Compiler.Linker;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using NDesk.Options;

namespace Mosa.Tools.Compiler.Linker
{
	/// <summary>
	/// Proxy type, which selects the appropriate linker for an output format.
	/// </summary>
	public sealed class LinkerFormatSelector : BaseAssemblyCompilerStage, IAssemblyLinker, IAssemblyCompilerStage, IHasOptions, IPipelineStage
	{
		#region Data Members

		/// <summary>
		/// Holds the real linker implementation to use.
		/// </summary>
		private IAssemblyLinker implementation;

		/// <summary>
		/// Holds the PE linker.
		/// </summary>
		private PortableExecutableLinkerWrapper peLinker;

		/// <summary>
		/// Holds the ELF32 linker.
		/// </summary>
		private Elf32LinkerWrapper elf32Linker = null;

		/// <summary>
		/// Holds the ELF64 linker.
		/// </summary>
		private Elf64LinkerWrapper elf64Linker = null;

		/// <summary>
		/// Holds the output file of the linker.
		/// </summary>
		private string outputFile;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the LinkerFormalSelector class.
		/// </summary>
		public LinkerFormatSelector()
		{
			this.peLinker = new PortableExecutableLinkerWrapper();
			this.elf32Linker = new Elf32LinkerWrapper();
			this.elf64Linker = new Elf64LinkerWrapper();
			this.implementation = null;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets a value indicating wheter an implementation has been selected.
		/// </summary>
		public bool IsConfigured
		{
			get
			{
				return (implementation != null);
			}
		}

		/// <summary>
		/// Gets or sets the output file.
		/// </summary>
		/// <value>The output file.</value>
		public string OutputFile
		{
			get { return this.outputFile; }
			set { this.outputFile = value; }
		}

		#endregion // Properties

		#region IAssemblyCompilerStage Members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			CheckImplementation();
			((IAssemblyCompilerStage)this.implementation).Setup(compiler);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			CheckImplementation();

			ITypeModule mainModule = typeSystem.MainTypeModule;

			if (mainModule.MetadataModule.EntryPoint.RID != 0)
			{
				RuntimeMethod entrypoint = mainModule.GetMethod(mainModule.MetadataModule.EntryPoint);

				implementation.EntryPoint = GetSymbol(entrypoint.ToString());
			}

			// Run the real linker
			IAssemblyCompilerStage assemblyCompilerStage = implementation as IAssemblyCompilerStage;
			Debug.Assert(assemblyCompilerStage != null, @"Linker doesn't implement IAssemblyCompilerStage.");
			if (assemblyCompilerStage != null)
			{
				assemblyCompilerStage.Run();
			}
		}

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get
			{
				if (implementation == null)
					return @"Not bootable";

				return ((IPipelineStage)implementation).Name;
			}
		}

		#endregion // IAssemblyCompilerStage Members

		#region IHasOptions Members

		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public void AddOptions(OptionSet optionSet)
		{
			IHasOptions options;

			optionSet.Add(
				"f|format=",
				"Select the format of the binary file to create [{ELF32|ELF64|PE}].",
				delegate(string format)
				{
					this.implementation = SelectImplementation(format);
				}
			);

			optionSet.Add(
				"o|out=",
				"The name of the output {file}.",
				delegate(string file)
				{
					this.outputFile =
						peLinker.Wrapped.OutputFile =
						elf32Linker.Wrapped.OutputFile =
						elf64Linker.Wrapped.OutputFile =
							file;
				}
			);

			options = peLinker as IHasOptions;
			if (options != null)
				options.AddOptions(optionSet);

			options = elf32Linker as IHasOptions;
			if (options != null)
				options.AddOptions(optionSet);

			options = elf64Linker as IHasOptions;
			if (options != null)
				options.AddOptions(optionSet);
		}
		#endregion // IHasOptions Members

		#region IAssemblyLinker Members

		/// <summary>
		/// Gets the base virtualAddress.
		/// </summary>
		/// <value>The base virtualAddress.</value>
		public long BaseAddress
		{
			get
			{
				CheckImplementation();
				return this.implementation.BaseAddress;
			}
		}

		/// <summary>
		/// Gets the entry point symbol.
		/// </summary>
		/// <value>The entry point symbol.</value>
		public LinkerSymbol EntryPoint
		{
			get
			{
				CheckImplementation();
				return this.implementation.EntryPoint;
			}

			set
			{
				CheckImplementation();
				this.implementation.EntryPoint = value;
			}
		}

		/// <summary>
		/// Gets the file section alignment.
		/// </summary>
		/// <value>The file section alignment.</value>
		public long LoadSectionAlignment
		{
			get
			{
				CheckImplementation();
				return this.implementation.LoadSectionAlignment;
			}
		}

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		public ICollection<LinkerSection> Sections
		{
			get
			{
				CheckImplementation();
				return this.implementation.Sections;
			}
		}

		/// <summary>
		/// Retrieves the collection of symbols known by the linker.
		/// </summary>
		/// <value>The symbol collection.</value>
		public ICollection<LinkerSymbol> Symbols
		{
			get
			{
				CheckImplementation();
				return this.implementation.Symbols;
			}
		}

		/// <summary>
		/// Gets the time stamp.
		/// </summary>
		/// <value>The time stamp.</value>
		public DateTime TimeStamp
		{
			get
			{
				CheckImplementation();
				return this.implementation.TimeStamp;
			}
		}

		/// <summary>
		/// Gets the virtual alignment of sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		public long VirtualSectionAlignment
		{
			get
			{
				CheckImplementation();
				return this.implementation.VirtualSectionAlignment;
			}
		}

		/// <summary>
		/// Issues a linker request for the given runtime method.
		/// </summary>
		/// <param name="linkType">The type of link required.</param>
		/// <param name="symbolName">The method the patched code belongs to.</param>
		/// <param name="methodOffset">The offset inside the method where the patch is placed.</param>
		/// <param name="methodRelativeBase">The base virtualAddress, if a relative link is required.</param>
		/// <param name="targetSymbolName">The linker symbol to link against.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>
		/// The return value is the preliminary virtualAddress to place in the generated machine
		/// code. On 32-bit systems, only the lower 32 bits are valid. The above are not used. An implementation of
		/// IAssemblyLinker may not rely on 64-bits being stored in the memory defined by position.
		/// </returns>
		public long Link(LinkType linkType, string symbolName, int methodOffset, int methodRelativeBase, string targetSymbolName, IntPtr offset)
		{
			CheckImplementation();
			return this.implementation.Link(linkType, symbolName, methodOffset, methodRelativeBase, targetSymbolName, offset);
		}

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="name">The name of the symbol.</param>
		/// <param name="section">The executable section to allocate From.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>A stream, which can be used to populate the section.</returns>
		public Stream Allocate(string name, SectionKind section, int size, int alignment)
		{
			CheckImplementation();
			return this.implementation.Allocate(name, section, size, alignment);
		}

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <param name="sectionKind">Kind of the section.</param>
		/// <returns>The section of the requested kind.</returns>
		public LinkerSection GetSection(SectionKind sectionKind)
		{
			CheckImplementation();
			return this.implementation.GetSection(sectionKind);
		}

		/// <summary>
		/// Retrieves a linker symbol.
		/// </summary>
		/// <param name="symbolName">The name of the symbol to retrieve.</param>
		/// <returns>The named linker symbol.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="symbolName"/> is null.</exception>
		/// <exception cref="System.ArgumentException">There's no symbol of the given name.</exception>
		public LinkerSymbol GetSymbol(string symbolName)
		{
			CheckImplementation();
			return this.implementation.GetSymbol(symbolName);
		}

		/// <summary>
		/// Determines if a given symbol name is already in use by the linker.
		/// </summary>
		/// <param name="symbolName">The symbol name.</param>
		/// <returns><c>true</c> if the symbol name is already used; <c>false</c> otherwise.</returns>
		public bool HasSymbol(string symbolName)
		{
			this.CheckImplementation();
			return this.implementation.HasSymbol(symbolName);
		}

		#endregion // IAssemblyLinker Members

		#region Internals

		/// <summary>
		/// Checks if a linker implementation is set.
		/// </summary>
		private void CheckImplementation()
		{
			if (this.implementation == null)
				throw new InvalidOperationException(@"LinkerFormatSelector not initialized.");
		}

		/// <summary>
		/// Selects the linker implementation to use.
		/// </summary>
		/// <param name="format">The linker format.</param>
		/// <returns>The implementation of the linker.</returns>
		private IAssemblyLinker SelectImplementation(string format)
		{
			switch (format.ToLower())
			{
				case "elf32":
					return this.elf32Linker.Wrapped;

				case "elf64":
					return this.elf32Linker.Wrapped;

				case "pe":
					return this.peLinker.Wrapped;

				default:
					throw new OptionException(String.Format("Unknown or unsupported binary format {0}.", format), "format");
			}
		}

		#endregion // Internals
	}
}
