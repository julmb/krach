#include <cstring>
#include <casadi/symbolic/casadi.hpp>
#include <casadi/interfaces/ipopt/ipopt_solver.hpp>

using namespace std;
using namespace CasADi;

struct IpoptProblem
{
	SXFunction F;
	SXFunction G;
	SXFunction H;
	SXFunction J;
	SXFunction GF;
	SXMatrix ConstraintLowerBounds;
	SXMatrix ConstraintUpperBounds;
};

extern "C"
{
	const IpoptProblem* IpoptProblemCreate(SXFunction* objectiveFunction, SXFunction* constraintFunction, SXMatrix** constraintLowerBounds, SXMatrix** constraintUpperBounds)
	{
		IpoptProblem* problem = new IpoptProblem();

		problem->F = *objectiveFunction;

		problem->G = *constraintFunction;

		SXMatrix position = objectiveFunction->inputExpr(0);
		SXMatrix sigma = ssym("sigma", objectiveFunction->getNumScalarOutputs());
		SXMatrix lambda = ssym("lambda", constraintFunction->getNumScalarOutputs());
		vector<SXMatrix> lagrangeVariables;
		lagrangeVariables.push_back(position);
		lagrangeVariables.push_back(lambda);
		lagrangeVariables.push_back(sigma);
		SXMatrix lagrangeValue = sigma * objectiveFunction->eval(position) + (constraintFunction->getNumScalarOutputs() == 0 ? 0 : inner_prod(lambda, constraintFunction->eval(position)));
		SXFunction lagrangeFunction = SXFunction(lagrangeVariables, lagrangeValue);
		lagrangeFunction.init();
		problem->H = SXFunction(lagrangeFunction.hessian());

		problem->J = constraintFunction->getNumScalarOutputs() == 0 ? SXFunction(position, SXMatrix(0, position.size1())) : SXFunction(constraintFunction->jacobian());

		problem->GF = SXFunction(objectiveFunction->gradient());

		for (int index = 0; index < constraintFunction->getNumScalarOutputs(); index++)
		{
			problem->ConstraintLowerBounds.append(*constraintLowerBounds[index]);
			problem->ConstraintUpperBounds.append(*constraintUpperBounds[index]);
		}

		return problem;
	}
	void IpoptProblemDispose(IpoptProblem* problem)
	{
		delete problem;
	}
	const IpoptProblem* IpoptProblemSubstitute(IpoptProblem* problem, SXMatrix** variables, SXMatrix** values, int count)
	{
		vector<SXMatrix> outputValuesVector;
		outputValuesVector.push_back(problem->F.outputExpr(0));
		outputValuesVector.push_back(problem->G.outputExpr(0));
		outputValuesVector.push_back(problem->H.outputExpr(0));
		outputValuesVector.push_back(problem->J.outputExpr(0));
		outputValuesVector.push_back(problem->GF.outputExpr(0));
		outputValuesVector.push_back(problem->ConstraintLowerBounds);
		outputValuesVector.push_back(problem->ConstraintUpperBounds);

		vector<SXMatrix> variablesVector;
		for (int index = 0; index < count; index++) variablesVector.push_back(*variables[index]);

		vector<SXMatrix> valuesVector;
		for (int index = 0; index < count; index++) valuesVector.push_back(*values[index]);

		outputValuesVector = substitute(outputValuesVector, variablesVector, valuesVector);

		IpoptProblem* newProblem = new IpoptProblem();

		newProblem->F = SXFunction(problem->F.inputExpr(), outputValuesVector[0]);
		newProblem->G = SXFunction(problem->G.inputExpr(), outputValuesVector[1]);
		newProblem->H = SXFunction(problem->H.inputExpr(), outputValuesVector[2]);
		newProblem->J = SXFunction(problem->J.inputExpr(), outputValuesVector[3]);
		newProblem->GF = SXFunction(problem->GF.inputExpr(), outputValuesVector[4]);
		newProblem->ConstraintLowerBounds = outputValuesVector[5];
		newProblem->ConstraintUpperBounds = outputValuesVector[6];

		return newProblem;
	}

	const IpoptSolver* IpoptSolverCreate(IpoptProblem* problem)
	{
		return new IpoptSolver(problem->F, problem->G, problem->H, problem->J, problem->GF);
	}
	void IpoptSolverDispose(IpoptSolver* solver)
	{
		delete solver;
	}
	void IpoptSolverInitialize(IpoptSolver* solver, IpoptProblem* problem)
	{
		solver->init();

		vector<double> constraintLowerBoundsValues;
		vector<double> constraintUpperBoundsValues;

		for (int index = 0; index < problem->G.getNumScalarOutputs(); index++)
		{
			constraintLowerBoundsValues.push_back(problem->ConstraintLowerBounds.elem(index, 0).getValue());
			constraintUpperBoundsValues.push_back(problem->ConstraintUpperBounds.elem(index, 0).getValue());
		}

		solver->setInput(constraintLowerBoundsValues, NLP_LBG);
		solver->setInput(constraintUpperBoundsValues, NLP_UBG);
	}
	void IpoptSolverSetInitialPosition(IpoptSolver* solver, double* position, int positionCount)
	{
		vector<double> positionValues;

		for (int index = 0; index < positionCount; index++) positionValues.push_back(position[index]);

		solver->setInput(positionValues, NLP_X_INIT);
	}
	const char* IpoptSolverSolve(IpoptSolver* solver)
	{
		solver->solve();

		string returnStatus = solver->getStat("return_status").toString();
		char* result = new char[returnStatus.size() + 1];
		strcpy(result, returnStatus.c_str());

		return result;
	}
	void IpoptSolverGetResultPosition(IpoptSolver* solver, double* position, int positionCount)
	{
		vector<double> positionValues = vector<double>(positionCount);
		solver->getOutput(positionValues, NLP_X_OPT);
		memcpy(position, positionValues.data(), sizeof(double) * positionCount);
	}
}
