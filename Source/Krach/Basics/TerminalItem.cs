using System;
using System.IO;
using System.Linq;
using Krach.Basics;
using System.Collections.Generic;
using Krach.Design;
using Krach.Extensions;
using Krach.Formats.Mpeg;
using Krach.Formats.Tags.ID3v2;
using Krach.Formats.Mpeg.MetaData;
using System.Text;

namespace Krach.Basics
{
	public class TerminalItem
	{
		readonly int column;
		readonly string text;
		readonly ConsoleColor foregroundColor;
		readonly ConsoleColor backgroundColor;

		public TerminalItem(int column, string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
		{
			if (column < 0) throw new ArgumentOutOfRangeException("column");
			if (text == null) throw new ArgumentNullException("text");

			this.column = column;
			this.text = text;
			this.foregroundColor = foregroundColor;
			this.backgroundColor = backgroundColor;
		}
		public TerminalItem(int column, string text, ConsoleColor foregroundColor) : this(column, text, foregroundColor, Console.BackgroundColor) { }
		public TerminalItem(int column, string text) : this(column, text, Console.ForegroundColor) { }
		public TerminalItem(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor) : this(0, text, foregroundColor, backgroundColor) { }
		public TerminalItem(string text, ConsoleColor foregroundColor) : this(text, foregroundColor, Console.BackgroundColor) { }
		public TerminalItem(string text) : this(text, Console.ForegroundColor) { }

		public void Write(int indentation)
		{
			Terminal.Write(column + indentation, text, foregroundColor, backgroundColor);
			Terminal.WriteLine();
		}
		public void Write()
		{
			Write(0);
		}
	}
}
