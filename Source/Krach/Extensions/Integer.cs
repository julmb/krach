using System;

namespace Krach.Extensions
{
	public static class Integer
	{
		public static string ToSubscriptString(this int value) 
		{
			string result = string.Empty;
			
			foreach (char character in value.ToString()) 
			{
				if (character == '-') result += '\u208B';
				else result += '\u2080' + int.Parse(character.ToString());
			}
			
			return result;
		}
		public static string ToSuperscriptString(this int value) 
		{
			string result = string.Empty;
			
			foreach (char character in value.ToString()) 
			{
				switch (character) 
				{
					case '-': result += '\u207B'; break;
					case '0': result += '\u2070'; break;
					case '1': result += '\u00B8'; break;
					case '2': result += '\u00B2'; break;
					case '3': result += '\u00B3'; break;
					case '4': result += '\u2074'; break;
					case '5': result += '\u2075'; break;
					case '6': result += '\u2076'; break;
					case '7': result += '\u2077'; break;
					case '8': result += '\u2078'; break;
					case '9': result += '\u2079'; break;
					default: throw new InvalidOperationException();
				}
			}
			
			return result;
		}
	}
}

