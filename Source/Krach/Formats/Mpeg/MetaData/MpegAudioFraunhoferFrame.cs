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
using System.IO;
using System.Linq;
using System.Text;
using Krach.Basics;
using Krach.Extensions;
using Krach.Maps;
using Krach.Maps.Abstract;
using Krach.Maps.Scalar;

namespace Krach.Formats.Mpeg.MetaData
{
	public class MpegAudioFraunhoferFrame : MpegAudioSimpleMetaDataFrame
	{
		public override int MetaDataOffset { get { return 32; } }
		public override string MetaDataIdentifier { get { return "VBRI"; } }

		public MpegAudioFraunhoferFrame(BinaryReader reader) : base(reader) { }
	}
}
