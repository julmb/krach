// Copyright © Julian Brunner 2010

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

namespace Krach.Design
{
	public interface IFactory<TResult>
	{
		TResult Create();
	}
	public interface IFactory<TResult, T1>
	{
		TResult Create(T1 parameter1);
	}
	public interface IFactory<TResult, T1, T2>
	{
		TResult Create(T1 parameter1, T2 parameter2);
	}
	public interface IFactory<TResult, T1, T2, T3>
	{
		TResult Create(T1 parameter1, T2 parameter2, T3 parameter3);
	}
	public interface IFactory<TResult, T1, T2, T3, T4>
	{
		TResult Create(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4);
	}
	public interface IFactory<TResult, T1, T2, T3, T4, T5>
	{
		TResult Create(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5);
	}
}
