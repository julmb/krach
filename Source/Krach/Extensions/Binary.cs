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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krach.Extensions
{
	static class Binary
	{
		public static UInt16 SwitchEndianness(UInt16 value)
		{
			return (UInt16)((UInt16)((value & 0x00FF) << 8) | (UInt16)((value & 0xFF00) >> 8));
		}
		public static Int16 SwitchEndianness(Int16 value)
		{
			return (Int16)((Int16)((value & 0x00FF) << 8) | (Int16)((value & 0xFF00) >> 8));
		}
		public static UInt32 SwitchEndianness(UInt32 value)
		{
			return (UInt32)((UInt32)((value & 0x000000FF) << 24) | (UInt32)((value & 0x0000FF00) << 8) | (UInt32)((value & 0x00FF0000) >> 8) | (UInt32)((value & 0xFF000000) >> 24));
		}
		public static Int32 SwitchEndianness(Int32 value)
		{
			return (Int32)((Int32)((value & 0x000000FF) << 24) | (Int32)((value & 0x0000FF00) << 8) | (Int32)((value & 0x00FF0000) >> 8) | (Int32)((value & 0xFF000000) >> 24));
		}
	}
}
