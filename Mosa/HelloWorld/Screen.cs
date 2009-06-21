﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Screen
	{
		/// <summary>
		/// 
		/// </summary>
		public static int Column = 0;
		/// <summary>
		/// 
		/// </summary>
		public static int Row = 0;
		/// <summary>
		/// 
		/// </summary>
		public static byte Color = 0x0A;

		/// <summary>
		/// 
		/// </summary>
		public const int Columns = 80;

		/// <summary>
		/// 
		/// </summary>
		public const int Rows = 40;

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <returns></returns>
		private unsafe static byte* GetAddress()
		{
			return (byte*)(0xB8000 + ((Row * Columns + Column) * 2));
		}

		/// <summary>
		/// Nexts 
		/// </summary>
		private static void Next()
		{
			Column++;

			if (Column >= Columns) {
				Column = 0;
				Row++;
			}
		}

		/// <summary>
		/// Skips the specified skip.
		/// </summary>
		/// <param name="skip">The skip.</param>
		private static void Skip(int skip)
		{
			for (int i = 0; i < skip; i++)
				Next();
		}

		/// <summary>
		/// Writes the character.
		/// </summary>
		/// <param name="chr">The character.</param>
		public unsafe static void Write(char chr)
		{
			byte* address = GetAddress();

			*address = (byte)chr;
			*(address + 1) = Color;

			Next();
		}

		/// <summary>
		/// Goto the top.
		/// </summary>
		public static void GotoTop()
		{
			Column = 0;
			Row = 0;
		}

		/// <summary>
		/// Goto the next line.
		/// </summary>
		public static void NextLine()
		{
			Column = 0;
			Row++;
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public static void Clear()
		{
			GotoTop();

			byte c = Color;
			Color = 0x0A;

			for (int i = 0; i < Columns * Rows; i++)
				Write(' ');

			Color = c;
			GotoTop();
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="val">The val.</param>
		public static void Write(uint val)
		{
			int count = 0;
			uint temp = val;

			do {
				temp /= 10;
				count++;
			}
			while (temp != 0);

			int x = Column;
			int y = Row;

			for (int i = 0; i < count; i++) {
				Column = x;
				Row = y;
				Skip(count - 1 - i);
				Write((char)('0' + (val % 10)));
				val /= 10;
			}

			Column = x;
			Row = y;
			Skip(count);
		}
	}
}