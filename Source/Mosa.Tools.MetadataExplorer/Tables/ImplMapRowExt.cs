/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	/// <summary>
	/// 
	/// </summary>
	public class ImplMapRowExt : TableRow
	{
		protected ImplMapRow row;

		public ImplMapRowExt(IMetadataProvider metadata, ImplMapRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.ImportNameStringIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("ImportName", row.ImportNameStringIdx);
			yield return Value("ImportNameStringIdx", row.ImportNameStringIdx);
			yield return Value("MappingFlags", row.MappingFlags.ToString());

			yield return Value("ImportScopeTableIdx", row.ImportScopeTableIdx);
			yield return Value("MemberForwardedTableIdx", row.MemberForwarded);
		}
	}
}
