namespace Area51Elevator
{
    using System;
    using System.Linq;

    class ElevatorDoor
    {
        public Elevator Elevator { get; private set; }

        public bool IsOpen { get; private set; }

        public ElevatorDoor(Elevator elevator)
        {
            this.Elevator = elevator;
            this.IsOpen = true;
        }

        public void Open()
        {
            lock (this.Elevator.AgentsInside)
            {
                this.IsOpen = this.Elevator.AgentsInside.All(a => a.SecurityLevel >= this.Elevator.CurrentFloor.MinSecurityLevel);

                if (this.IsOpen)
                {
                    Console.WriteLine("Door is open.");
                }
                else
                {
                    Console.WriteLine("Doors cannot open! Unauthorized access!");
                }
            }
        }

        public void Close()
        {
            bool wasOpen = this.IsOpen;
            this.IsOpen = false;

            if (wasOpen)
            {
                Console.WriteLine("Door is closed.");
            }
        }
    }
}
