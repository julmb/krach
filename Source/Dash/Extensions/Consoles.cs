using System;

namespace Utility.Utilities
{
	public static class Consoles
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
