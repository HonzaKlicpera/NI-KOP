using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackAnnealing.Solver
{
    public static class AnnealingSolverConfig
    {
        public const float BASE_STARTING_TEMPERATURE = 1;
        public const int SCORE_PENALTY_MULTIPLIER = 10;

        public const int MIN_CONST_TEMPERATURE = 10;
        public const int CONST_EQUILIBRIUM_LOOP_COUNT = 50;

        public const int MAX_UNSUCCESSFUL_TRIES = 50;

        public const float COOLING_COEFFICIENT = 0.95f;
    }
}
