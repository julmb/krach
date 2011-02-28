# Copyright © Julian Brunner 2010 - 2011

# This file is part of Krach.
#
# Krach is free software: you can redistribute it and/or modify it under the
# terms of the GNU Lesser General Public License as published by the Free
# Software Foundation, either version 3 of the License, or (at your option) any
# later version.
#
# Krach is distributed in the hope that it will be useful, but WITHOUT ANY
# WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
# A PARTICULAR PURPOSE. See the GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License along with
# Krach. If not, see <http:#www.gnu.org/licenses/>.

#!/bin/sh

referenceHeader1=`cat Header\ 1.txt`
referenceHeader2=`cat Header\ 2.txt`

totalFiles=0
incorrectFiles=0

for sourcePath in `find "$1" '-name' '*.cs' '-o' '-name' '*.h' '-o' '-name' '*.c' '-o' '-name' '*.cpp' '-o' '-name' '*.sh' '-o' '-name' 'Makefile'`
do
	totalFiles=`expr $totalFiles + 1`
	testHeader=`head -16 $sourcePath`
	if [ "$testHeader" != "$referenceHeader1" -a "$testHeader" != "$referenceHeader2" ]
	then
		incorrectFiles=`expr $incorrectFiles + 1`
		echo "Incorrect header: ${sourcePath#../}"
	fi
done

echo "Files checked:     $totalFiles"
echo "Incorrect headers: $incorrectFiles"

if [ $incorrectFiles -gt 0 ]
then exit 1
fi
