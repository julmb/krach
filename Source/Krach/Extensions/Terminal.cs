// Copyright © Julian Brunner 2010 - 2011

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;

namespace Krach.Extensions
{
	public static class Terminal
	{
		public static bool IsTerminal
		{
			get
			{
				try
				{
					#pragma warning disable 0219
					bool cursorVisible = Console.CursorVisible;
					#pragma warning restore 0219

					return Console.BufferWidth > 0;
				}
				catch (IOException) { return false; }
			}
		}

		public static void Write(string text)
		{
			Console.Write(text);
		}
		public static void Write(string text, ConsoleColor foregroundColor)
		{
			ConsoleColor oldForegroundColor = Console.ForegroundColor;

			Console.ForegroundColor = foregroundColor;
			Write(text);
			Console.ForegroundColor = oldForegroundColor;
		}
		public static void Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
		{
			ConsoleColor oldBackgroundColor = Console.BackgroundColor;

			Console.BackgroundColor = backgroundColor;
			Write(text, foregroundColor);
			Console.BackgroundColor = oldBackgroundColor;
		}
		public static void Write(int column, string text)
		{
			if (IsTerminal)
			{
				Console.CursorLeft = column;
				foreach (string word in text.Split(' '))
				{
					if (Console.CursorLeft + word.Length + 1 + 1 > Console.BufferWidth)
					{
						Console.WriteLine();
						Console.CursorLeft = column;
					}
	
					Console.Write(word);
					Console.Write(" ");
				}
			}
			else Console.Write(new string(' ', column) + text);
		}
		public static void WriteLine()
		{
			Console.WriteLine();
		}
		public static string ReadLine(string caption)
		{
			Write(caption);

			return Console.ReadLine();
		}
	}
}
