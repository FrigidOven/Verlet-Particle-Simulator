
using SFML.Graphics;
using System.Runtime.CompilerServices;

namespace ParticleSimulatorMk4
{
    public class Particle
    {
        public static int Radius = 2;

        public float CurX;
        public float CurY;
        public float PreX;
        public float PreY;
        public float AccX;
        public float AccY;
        public Color Color;
        public int Index;
        public int Cell;
        public bool Added;
        public int Next;
        public Particle(float x, float y, Color color)
        {
            this.CurX = x;
            this.CurY = y;

            this.PreX = x;
            this.PreY = y;

            this.AccY = Solver.Gravity;

            this.Color = color;

            this.Added = false;
        }
        public Particle(float x, float y, float dx, float dy, Color color)
        {
            this.CurX = x;
            this.CurY = y;

            this.PreX = x - dx;
            this.PreY = y - dy;

            this.AccY = Solver.Gravity;

            this.Color = color;

            this.Added = false;
        }
    }
}
