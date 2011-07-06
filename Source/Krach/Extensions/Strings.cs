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
	}
}
