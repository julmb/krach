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

			int rows = text1.Length + 1;
			int columns = text2.Length + 1;

			int[,] distance = new int[rows, columns];

			for (int row = 0; row < rows; row++) distance[row, 0] = row;
			for (int column = 0; column < columns; column++) distance[0, column] = column;

			for (int row = 1; row < rows; row++)
				for (int column = 1; column < columns; column++)
					if (text1[row - 1] == text2[column - 1]) distance[row, column] = distance[row - 1, column - 1];
					else
					{
						int deletionDistance = distance[row - 1, column] + 1;
						int insertionDistance = distance[row, column - 1] + 1;
						int replacementDistance = distance[row - 1, column - 1] + 1;

						distance[row, column] = Comparables.Minimum(deletionDistance, insertionDistance, replacementDistance);
					}

			return distance[rows - 1, columns - 1];
		}
	}
}
