namespace Area51Elevator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    class Program
    {
        private const int N = 5; // Input number of agents for simulation

        static void Main(string[] args)
        {
            Random rand = new Random();

            List<Floor> floors = new List<Floor>()
            {
                new Floor("G", 1, SecurityLevel.Confidential),
                new Floor("S", 2, SecurityLevel.Secret),
                new Floor("T1", 3, SecurityLevel.TopSecret),
                new Floor("T2", 4, SecurityLevel.TopSecret),
            };

            Elevator elevator = new Elevator(floors);

            floors.ForEach(f => f.MountElevatorButton(new ElevatorButton(f, elevator)));

            List<Agent> agents = new List<Agent>();

            for (int i = 0; i < N; i++)
            {
                agents.Add(new Agent($"#{i + 1}", (SecurityLevel)rand.Next(Enum.GetNames(typeof(SecurityLevel)).Length), elevator));
            }

            List<Thread> threads = agents.Select(a => new Thread(a.DoShit)).ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
        }
    }
}
