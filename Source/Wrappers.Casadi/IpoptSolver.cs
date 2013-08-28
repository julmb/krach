using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;
using System.Runtime.InteropServices;
using Krach.Design;
using Wrappers.Casadi.Native;
using Krach;

namespace Wrappers.Casadi
{
	public class IpoptSolver : IDisposable
	{
		readonly IntPtr solver;
		readonly int domainDimension;

		bool disposed = false;

		public IpoptSolver(IpoptProblem problem, Settings settings)
		{
			if (problem == null) throw new ArgumentNullException("problem");
			if (settings == null) throw new ArgumentNullException("settings");

			lock (GeneralNative.Synchronization)
			{
				this.solver = IpoptNative.IpoptSolverCreate(problem.Problem);
				this.domainDimension = problem.DomainDimension;

				settings.Apply(solver);

				IpoptNative.IpoptSolverInitialize(solver, problem.Problem);
			}
		}
		~IpoptSolver()
		{
			Dispose();
		}

		public IEnumerable<double> Solve(IEnumerable<double> startPosition)
		{
			if (startPosition.Count() != domainDimension) throw new ArgumentException("Parameter 'startPosition' has the wrong number of items.");

			IntPtr position = startPosition.Copy();

			lock (GeneralNative.Synchronization)
			{
				IpoptNative.IpoptSolverSetInitialPosition(solver, position, domainDimension);

				string returnStatus = IpoptNative.IpoptSolverSolve(solver);

				if (returnStatus != "Solve_Succeeded" && returnStatus != "Solved_To_Acceptable_Level") throw new InvalidOperationException(returnStatus);

				IpoptNative.IpoptSolverGetResultPosition(solver, position, domainDimension);
			}

			IEnumerable<double> resultPosition = position.Read<double>(domainDimension);

			Marshal.FreeCoTaskMem(position);

			return resultPosition;
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;

				lock (GeneralNative.Synchronization) IpoptNative.IpoptSolverDispose(solver);
				
				GC.SuppressFinalize(this);
			}
		}
	}
}

