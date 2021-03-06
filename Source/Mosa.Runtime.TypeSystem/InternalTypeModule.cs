﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem.Cil;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Runtime.TypeSystem
{
	public class InternalTypeModule : ITypeModule
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		private readonly ITypeSystem typeSystem;

		/// <summary>
		/// 
		/// </summary>
		private readonly List<RuntimeType> types;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public InternalTypeModule(ITypeSystem typeSystem)
		{
			Debug.Assert(typeSystem != null);

			this.typeSystem = typeSystem;
			this.types = new List<RuntimeType>();
		}

		#endregion // Construction

		#region ITypeModule interface

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		ITypeSystem ITypeModule.TypeSystem { get { return typeSystem; } }

		/// <summary>
		/// Gets the metadata module.
		/// </summary>
		/// <value>The metadata module.</value>
		IMetadataModule ITypeModule.MetadataModule { get { return null; } }

		/// <summary>
		/// Gets all types from module.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> ITypeModule.GetAllTypes()
		{
			foreach (RuntimeType type in types)
				yield return type;
		}

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType ITypeModule.GetType(string nameSpace, string name)
		{
			foreach (RuntimeType type in types)
			{
				if (type.Name == name && type.Namespace == nameSpace)
				{
					return type;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="fullname">The fullname.</param>
		/// <returns></returns>
		RuntimeType ITypeModule.GetType(string fullname)
		{
			int dot = fullname.LastIndexOf(".");

			if (dot < 0)
				return null;

			return ((ITypeModule)this).GetType(fullname.Substring(0, dot), fullname.Substring(dot + 1));
		}

		/// <summary>
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		RuntimeType ITypeModule.GetType(Token token)
		{
			return null;
		}

		/// <summary>
		/// Retrieves the field definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the field to retrieve.</param>
		/// <returns></returns>
		RuntimeField ITypeModule.GetField(Token token)
		{
			return null;
		}

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod ITypeModule.GetMethod(Token token)
		{
			return null;
		}

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="callingType">Type of the calling.</param>
		/// <returns></returns>
		RuntimeMethod ITypeModule.GetMethod(Token token, RuntimeType callingType)
		{
			return null;
		}

		/// <summary>
		/// Gets the module's name.
		/// </summary>
		/// <value>The module's name.</value>
		string ITypeModule.Name { get { return "<internal>"; } }

		/// <summary>
		/// Gets the open generic.
		/// </summary>
		/// <param name="baseGenericType">Type of the base generic.</param>
		/// <returns></returns>
		CilGenericType ITypeModule.GetOpenGeneric(RuntimeType baseGenericType)
		{
			return null;
		}

		/// <summary>
		/// Gets the name of the external.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		string ITypeModule.GetExternalName(Token token)
		{
			return null;
		}

		#endregion

		/// <summary>
		/// Adds the type.
		/// </summary>
		/// <param name="type">The type.</param>
		public void AddType(RuntimeType type)
		{
			types.Add(type);
		}
	}
}
