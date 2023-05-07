using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulatorMk4
{
    public class Grid
    {
        public int CellSize;
        public int ColCount;
        public int RowCount;
        public int[] Cells;
        public Particle[] Particles;
        public int Size;
        public int Count;
        public Grid()
        {
            this.CellSize = 2 * Particle.Radius;
            this.ColCount = Simulator.Width/this.CellSize + 1;
            this.RowCount = Simulator.Height/this.CellSize + 1;
            this.Size = this.ColCount * this.RowCount;
            this.Cells = new int[this.Size];
            for(int i = 0; i < this.Cells.Length; i++)
            {
                this.Cells[i] = -1;
            }
            Particles = new Particle[Simulator.MaxParticles];
            Count = 0;
        }
        public void Relocate(Particle p)
        {
            if (p.Cell != Cell(p))
            {
                Remove(p);
                Add(p);
            }
        }
        public void Add(Particle p)
        {
            if (!p.Added)
            {
                p.Index = Count;
                Particles[Count] = p;
                p.Added = true;
            }
            int cell = Cell(p);
            p.Cell = cell;
            p.Next = Cells[cell];
            Cells[cell] = p.Index;
            Count++;
        }
        private void Remove(Particle p)
        {
            int cell = p.Cell;

            int pre = Cells[cell];
            int cur = pre;
            while(cur != -1)
            {
                if(cur == p.Index)
                {
                    if (cur == pre)
                    {
                        Cells[cell] = p.Next;
                    }
                    else
                    {
                        Particles[pre].Next = p.Next;
                    }
                    p.Next = -1;
                    p.Cell = -1;
                    Count--;
                    return;

                }
                pre = cur;
                cur = Particles[cur].Next;
            }
        }
        private int Cell(Particle p)
        {
            return Math.Clamp((int) (p.CurX / CellSize) + (int) (p.CurY / CellSize) * ColCount, 0, Cells.Length - 1);
        }
    }
}
