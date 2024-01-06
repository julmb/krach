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
using System.Linq;
using System.Collections.Generic;

namespace Krach.Extensions
{
	public static class Strings
	{
		public static int GetEditDistance(string text1, string text2)
		{
			if (text1 == null) throw new ArgumentNullException("text1");
			if (text2 == null) throw new ArgumentNullException("text2");

			text1 = text1.ToLowerInvariant();
			text2 = text2.ToLowerInvariant();

			int rowCount = text1.Length + 1;
			int columnCount = text2.Length + 1;

			int[,] distance = new int[rowCount, columnCount];

			for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) distance[rowIndex, 0] = rowIndex;
			for (int columnIndex = 0; columnIndex < columnCount; columnIndex++) distance[0, columnIndex] = columnIndex;

			for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
				for (int columnIndex = 1; columnIndex < columnCount; columnIndex++)
					if (text1[rowIndex - 1] == text2[columnIndex - 1]) distance[rowIndex, columnIndex] = distance[rowIndex - 1, columnIndex - 1];
					else
					{
						int deletionDistance = distance[rowIndex - 1, columnIndex] + 1;
						int insertionDistance = distance[rowIndex, columnIndex - 1] + 1;
						int replacementDistance = distance[rowIndex - 1, columnIndex - 1] + 1;

						distance[rowIndex, columnIndex] = int.MaxValue;

						if (deletionDistance < distance[rowIndex, columnIndex]) distance[rowIndex, columnIndex] = deletionDistance;
						if (insertionDistance < distance[rowIndex, columnIndex]) distance[rowIndex, columnIndex] = insertionDistance;
						if (replacementDistance < distance[rowIndex, columnIndex]) distance[rowIndex, columnIndex] = replacementDistance;
					}

			return distance[rowCount - 1, columnCount - 1];
		}
		public static bool IsSubSuperScriptCompatible(this string text)
		{
			IEnumerable<char> allowedCharacters = Enumerables.Create(' ', '0' ,'1' ,'2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '=', '(', ')');
				
			return !text.Except(allowedCharacters).Any();
		}
		public static string ToSubscript(this string text) 
		{
			string result = string.Empty;
			
			foreach (char character in text) 
			{
				switch (character) 
				{
					case ' ': result += ' '; break;
					case '0': result += '\u2080'; break;
					case '1': result += '\u2081'; break;
					case '2': result += '\u2082'; break;
					case '3': result += '\u2083'; break;
					case '4': result += '\u2084'; break;
					case '5': result += '\u2085'; break;
					case '6': result += '\u2086'; break;
					case '7': result += '\u2087'; break;
					case '8': result += '\u2088'; break;
					case '9': result += '\u2089'; break;
					case '+': result += '\u208A'; break;
					case '-': result += '\u208B'; break;
					case '=': result += '\u208C'; break;
					case '(': result += '\u208D'; break;
					case ')': result += '\u208E'; break;
					default: throw new InvalidOperationException();
				}
			}
			
			return result;
		}
		public static string ToSuperscript(this string text) 
		{
			string result = string.Empty;
			
			foreach (char character in text) 
			{
				switch (character) 
				{
					case ' ': result += ' '; break;
					case '0': result += '\u2070'; break;
					case '1': result += '\u00B9'; break;
					case '2': result += '\u00B2'; break;
					case '3': result += '\u00B3'; break;
					case '4': result += '\u2074'; break;
					case '5': result += '\u2075'; break;
					case '6': result += '\u2076'; break;
					case '7': result += '\u2077'; break;
					case '8': result += '\u2078'; break;
					case '9': result += '\u2079'; break;
					case '+': result += '\u207A'; break;
					case '-': result += '\u207B'; break;
					case '=': result += '\u207C'; break;
					case '(': result += '\u207D'; break;
					case ')': result += '\u207E'; break;
					default: throw new InvalidOperationException();
				}
			}
			
			return result;
		}
		public static string ToFileSizeString(this double value)
		{
			if (value >= 0x40000000) return string.Format("{0:F2} GB", value / 0x40000000);
			if (value >= 0x100000) return string.Format("{0:F2} MB", value / 0x100000);
			if (value >= 0x400) return string.Format("{0:F2} kB", value / 0x400);

			return string.Format("{0} B", value);
		}
		public static string ToFileSizeString(this int value)
		{
			return ToFileSizeString((double)value);
		}
		public static string ToFileSizeString(this long value)
		{
			return ToFileSizeString((double)value);
		}
	}
}
