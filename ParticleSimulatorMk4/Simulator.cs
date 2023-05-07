using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSimulatorMk4
{
    public static class Simulator
    {
        public const int Width = 1280;
        public const int Height = 800;
        public const int ParticleCount = 20_000;
        public const int MaxParticles = 20_000;
        public const int Substeps = 8;
        public const int Framerate = 60;
        public const float Dt = 1.0f / Framerate;

        private static void Render(RenderWindow window, Grid grid, CircleShape circle, Text count)
        {
            for(int i = 0; i < grid.Count; i++)
            {
                circle.Position = new Vector2f(grid.Particles[i].CurX, grid.Particles[i].CurY);
                circle.FillColor = grid.Particles[i].Color;
                window.Draw(circle);
            }
            count.DisplayedString = "Count: " + grid.Count;
            window.Draw(count);
        }
        private static void Simulate(Grid grid, PhysicsHandler physics, Random rand)
        {
            if(grid.Count < ParticleCount)
            {
                float cos = (float)Math.Cos(0.025 * grid.Count);

                Color color1 = new Color((byte) rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
                Particle p = new Particle(Width / 2, Particle.Radius, cos, Particle.Radius / 2, color1);
                grid.Add(p);

                //Color color2 = new Color((byte)rand.Next(128), (byte)rand.Next(128), (byte)rand.Next(255));
                //Particle q = new Particle(Width / 2, Height - Particle.Radius, -cos, -Particle.Radius / 2, color2);
                //grid.Add(q);

            }
            physics.Simulate();
        }
        public static void Main(string[] args)
        {
            VideoMode mode = new VideoMode((uint)Width+5, (uint)Height+5);
            RenderWindow window = new RenderWindow(mode, "Particle Simulator");

            window.Closed += (sender, args) => window.Close();
            window.SetVerticalSyncEnabled(true);

            Grid grid = new Grid();
            PhysicsHandler physics = new PhysicsHandler(grid, Substeps, Dt);

            Random rand = new Random();
            CircleShape circle = new CircleShape()
            {
                Radius = Particle.Radius
            };
            Text count = new Text()
            {
                Font = new Font("C:\\Windows\\Fonts\\Arial.ttf"),
                Position = new Vector2f(100, 100),
                CharacterSize = 20,
                FillColor = Color.White
            };
            while (window.IsOpen)
            {
                Thread doPhysics = new Thread(() => Simulate(grid, physics, rand));
                doPhysics.Start();
                Render(window, grid, circle, count);
                doPhysics.Join();

                window.DispatchEvents();
                window.Display();
                window.Clear(Color.Black);
            }
            circle.Dispose();

        }
    }
}
