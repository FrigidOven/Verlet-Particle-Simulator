using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulatorMk4
{
    public class Solver
    {
        public const float Gravity = 1000f;
        private Grid grid;
        public Solver(Grid grid)
        {
            this.grid = grid;
        }
        public void Simulate(int cell, float dt)
        {
            Particle p = grid.Particles[grid.Cells[cell]];
            int[] neighbors =
            {
                cell,
                cell + grid.ColCount,
                cell - 1,
                cell - grid.ColCount,
                cell + 1,
                cell + grid.ColCount + 1,
                cell + grid.ColCount - 1,
                cell - grid.ColCount - 1,
                cell - grid.ColCount + 1,
            };
            while(true)
            {
                HandleCollisions(p, neighbors);
                UpdatePosition(p, dt);
                //UpdateAcceleration(p);
                if (p.Next == -1)
                {
                    break;
                }
                else
                {
                    p = grid.Particles[p.Next];
                }
            }
        }
        public void HandleCollisions(Particle p, int[] neighbors)
        {
            for(int i = 0; i < neighbors.Length; i++)
            {
                if((-1 < neighbors[i] && neighbors[i] < grid.Cells.Length) && grid.Cells[neighbors[i]] != -1)
                {
                    Particle q = grid.Particles[grid.Cells[neighbors[i]]];
                    while(true)
                    {
                        if(p != q)
                        {
                            float distance_x = q.CurX - p.CurX;
                            float distance_y = q.CurY - p.CurY;

                            float distance = (float)Math.Sqrt(distance_x * distance_x + distance_y * distance_y);
                            float correction = 2 * Particle.Radius - distance;

                            if(correction > 0)
                            {
                                if(distance != 0)
                                {
                                    distance_x /= distance;
                                    distance_y /= distance;
                                }
                                else
                                {
                                    distance_x = 1;
                                    distance_y = 1;
                                }

                                float x1 = p.CurX - (distance_x * correction / 2);
                                float y1 = p.CurY - (distance_y * correction / 2);

                                float x2 = q.CurX + (distance_x * correction / 2);
                                float y2 = q.CurY + (distance_y * correction / 2);

                                p.CurX = x1;
                                p.CurY = y1;

                                q.CurX = x2;
                                q.CurY = y2;
                            }
                        }
                        if(q.Next == -1)
                        {
                            break;
                        }
                        else
                        {
                            q = grid.Particles[q.Next];
                        }
                    }
                }
            }
        }
        public void UpdatePosition(Particle p, float dt)
        {
            float temp = p.CurX;
            p.CurX = Math.Clamp(p.CurX * 2 - p.PreX + p.AccX * dt * dt, 0, Simulator.Width);
            p.PreX = Math.Clamp(temp, 0, Simulator.Width);

            temp = p.CurY;
            p.CurY = Math.Clamp(p.CurY * 2 - p.PreY + p.AccY * dt * dt, 0, Simulator.Height);
            p.PreY = Math.Clamp(temp, 0, Simulator.Height);
        }
        public void UpdateAcceleration(Particle p)
        {
            float distance_x = Simulator.Width / 2 - p.CurX;
            float distance_y = Simulator.Height / 2 - p.CurY;

            float distance = (float)Math.Sqrt(distance_x * distance_x + distance_y * distance_y);

            distance_x /= distance;
            distance_y /= distance;

            p.AccX = distance_x * Gravity;
            p.AccY = distance_y * Gravity;
        }
    }
}
