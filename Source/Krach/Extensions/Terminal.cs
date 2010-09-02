// Copyright © Julian Brunner 2010

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

namespace Krach.Extensions
{
	public static class Terminal
	{
		public static void Write(int column, string text)
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
		public static void WriteLine(int column, string text)
		{
			Write(column, text);
			Console.WriteLine();
		}
	}
}
