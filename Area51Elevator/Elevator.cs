namespace Area51Elevator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    class Elevator
    {
        public ElevatorDoor Door { get; private set; }

        public List<Floor> SupportedFloors { get; private set; }

        public List<ElevatorButton> Buttons { get; private set; }

        public List<Agent> AgentsInside { get; private set; }

        public Floor CurrentFloor { get; private set; }

        public bool IsBusy { get; private set; }

        public Elevator(List<Floor> supportedFloors)
        {
            this.Door = new ElevatorDoor(this);
            this.SupportedFloors = supportedFloors;
            this.Buttons = this.SupportedFloors.Select(f => new ElevatorButton(f, this)).ToList();

            this.AgentsInside = new List<Agent>();

            this.CurrentFloor = this.SupportedFloors.First();
            this.IsBusy = false;
        }

        public void GoToFloor(Floor floor)
        {
            this.Door.Close();
            this.IsBusy = true;

            Thread thread = new Thread(this.Move);
            thread.Start(floor);
        }

        public void AcceptAgent(Agent agent)
        {
            lock (this.AgentsInside)
            {
                this.AgentsInside.Add(agent);
            }
        }
        
        public void RemoveAgent(Agent agent)
        {
            lock (this.AgentsInside)
            {
                this.AgentsInside.Remove(agent);
            }
        }

        private void Move(object obj)
        {
            Floor floor = obj as Floor;

            Thread.Sleep(Math.Abs(floor.Level - this.CurrentFloor.Level) * 1000);

            Console.WriteLine($"Elevator going to floor {floor.Name}");

            this.CurrentFloor = floor;
            Console.WriteLine($"Elevator stopped on floor {this.CurrentFloor.Name}.");

            this.Door.Open();
            this.IsBusy = false;
        }
    }
}
