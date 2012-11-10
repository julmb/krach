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
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Krach.Extensions
{
	public static class Marshaling
	{
		public static IntPtr Copy<T>(this IEnumerable<T> source)
		{
			IntPtr beginnning = Marshal.AllocCoTaskMem(source.Count() * Marshal.SizeOf(typeof(T)));

			IntPtr pointer = beginnning;
			foreach (T item in source)
			{
				Marshal.StructureToPtr(item, pointer, false);

				pointer += Marshal.SizeOf(typeof(T));
			}

			return beginnning;
		}
		public static IEnumerable<T> ReadLazy<T>(this IntPtr pointer, int length)
		{
			for (int index = 0; index < length; index++)
			{
				yield return (T)Marshal.PtrToStructure(pointer, typeof(T));

				pointer += Marshal.SizeOf(typeof(T));
			}
		}
		public static IEnumerable<T> Read<T>(this IntPtr pointer, int length)
		{
			return pointer.ReadLazy<T>(length).ToArray();
		}
		public static void Write<T>(this IntPtr pointer, IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				Marshal.StructureToPtr(item, pointer, false);

				pointer += Marshal.SizeOf(typeof(T));
			}
		}
	}
}

