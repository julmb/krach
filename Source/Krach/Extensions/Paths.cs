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

using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Krach.Extensions
{
	public static class Paths
	{
		readonly static char[] invalidCharacters = new char[] { '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\u000E', '\u000F', '\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001A', '\u001B', '\u001C', '\u001D', '\u001E', '\u001F', ':', '*', '?', '\\', '/', '"', '<', '>', '|' };

		public static string ToFileName(string text)
		{
			return Regex.Replace(text, string.Format("[{0}]+", Regex.Escape(new string(invalidCharacters))), string.Empty).TrimEnd('.');
		}
		public static string GetFreshName(string path, IEnumerable<string> paths)
		{
			if (!Directory.Exists(path) && !File.Exists(path) && !paths.Contains(path)) return path;

			return GetFreshName(path + "_", paths);
		}
		public static void MoveRaw(string sourcePath, string targetPath)
		{
			if ((File.GetAttributes(sourcePath) & FileAttributes.Directory) != 0) Directory.Move(sourcePath, targetPath);
			else File.Move(sourcePath, targetPath);
		}
		public static void Move(string sourcePath, string targetPath)
		{
			string intermediatePath = GetFreshName(sourcePath, Enumerables.Create(sourcePath, targetPath));

			MoveRaw(sourcePath, intermediatePath);
			MoveRaw(intermediatePath, targetPath);
		}
	}
}
