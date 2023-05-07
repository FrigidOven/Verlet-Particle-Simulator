using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulatorMk4
{
    public class PhysicsHandler
    {
        private Solver solver;
        private Grid grid;
        private int substeps;
        private float dt;
        public PhysicsHandler(Grid grid, int substeps, float dt)
        {
            this.grid = grid;
            this.solver = new Solver(grid);
            this.substeps = substeps;
            this.dt = dt/substeps;
        }
        public void Simulate()
        {
            for(int s = 0; s < substeps; s++)
            {
                for(int i = 0; i < grid.Size; i++)
                {
                    if (grid.Cells[i] != -1)
                    {
                        solver.Simulate(i, dt);
                    }
                }
                for(int i = 0; i < grid.Count; i++)
                {
                    grid.Relocate(grid.Particles[i]);
                }
            }
        }
    }
}
