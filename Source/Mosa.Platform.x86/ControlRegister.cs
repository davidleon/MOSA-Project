/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Base class for x86 segment registers.
	/// </summary>
	public sealed class ControlRegister : Register
	{
		#region Types

		/// <summary>
		/// Identifies x86 control registers using their instruction encoding.
		/// </summary>
		public enum ControlRegisterType
		{
			/// <summary>
			/// The x86 CR0 register instruction encoding.
			/// </summary>
			CR0 = 0,

			/// <summary>
			/// The x86 CR2 register instruction encoding.
			/// </summary>
			CR2 = 2,

			/// <summary>
			/// The x86 CR3 register instruction encoding.
			/// </summary>
			CR3 = 3,

			/// <summary>
			/// The x86 CR4 register instruction encoding.
			/// </summary>
			CR4 = 4,
		}

		#endregion // Types

		#region Static data members

		/// <summary>
		/// Represents the CR0 register.
		/// </summary>
		public static readonly ControlRegister CR0 = new ControlRegister(ControlRegisterType.CR0);

		/// <summary>
		/// Represents the CR2 register.
		/// </summary>
		public static readonly ControlRegister CR2 = new ControlRegister(ControlRegisterType.CR2);

		/// <summary>
		/// Represents the CR3 register.
		/// </summary>
		public static readonly ControlRegister CR3 = new ControlRegister(ControlRegisterType.CR3);

		/// <summary>
		/// Represents the CR4 register.
		/// </summary>
		public static readonly ControlRegister CR4 = new ControlRegister(ControlRegisterType.CR4);

		#endregion // Static data members

		/// <summary>
		/// Stores the general purpose register identified by this object instance.
		/// </summary>
		private readonly ControlRegisterType _control;

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="SegmentRegister"/>.
		/// </summary>
		/// <param name="control">The control.</param>
		private ControlRegister(ControlRegisterType control) :
			base((int)control)
		{
			_control = control;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the control.
		/// </summary>
		/// <value>The control.</value>
		public ControlRegisterType Control { get { return _control; } }

		/// <summary>
		/// Returns the index of this register.
		/// </summary>
		public override int RegisterCode
		{
			get { return base.Index; }
		}

		/// <summary>
		/// General purpose register do not support floating point operations.
		/// </summary>
		public override bool IsFloatingPoint
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the caller-save status of this register.
		/// </summary>
		/// <value>True if the register is caller saved; otherwise false.</value>
		public override sealed bool IsCallerSaved
		{
			get { return false; }
		}

		/// <summary>
		/// Returns the width of general purpose registers in bits.
		/// </summary>
		public override int Width
		{
			get { return 32; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Determines if the signature type fits into the register.
		/// </summary>
		/// <param name="type">The signature type to check.</param>
		/// <returns>True if the signature type fits.</returns>
		public override bool IsValidSigType(SigType type)
		{
			return (type.Type == CilElementType.I ||
					type.Type == CilElementType.I1 ||
					type.Type == CilElementType.I2 ||
					type.Type == CilElementType.I4 ||
					type.Type == CilElementType.U1 ||
					type.Type == CilElementType.U2 ||
					type.Type == CilElementType.U4 ||
					type.Type == CilElementType.Ptr ||
					type.Type == CilElementType.ByRef ||
					type.Type == CilElementType.FunctionPtr ||
					type.Type == CilElementType.Object);
		}

		/// <summary>
		/// Returns the name of the control register.
		/// </summary>
		/// <returns>The name of the control register.</returns>
		public override string ToString()
		{
			return _control.ToString();
		}

		#endregion // Methods
	}
}
