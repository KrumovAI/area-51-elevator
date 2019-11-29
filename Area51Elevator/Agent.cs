using System;
using System.Collections.Generic;
using System.Linq;

namespace Area51Elevator
{
    class Agent
    {
        private static object elevatorBusyFlagLocker = new object();

        private Random rand = new Random();

        public string Name { get; private set; }

        public SecurityLevel SecurityLevel { get; private set; }

        public AgentPosition Position { get; private set; }

        public Elevator Elevator { get; private set; }

        public Floor CurrentFloor { get; private set; }

        public Agent(string name, SecurityLevel securityLevel, Elevator elevator)
        {
            this.Name = name;
            this.SecurityLevel = securityLevel;
            this.Elevator = elevator;

            this.CurrentFloor = null;
            this.Position = AgentPosition.Outside;
        }

        public void DoShit()
        {
            while (this.Position != AgentPosition.AtHome)
            {
                switch (this.Position)
                {
                    case AgentPosition.InTheElevator:
                        if (this.Elevator.IsBusy)
                        {
                            continue;
                        }

                        this.DoElevatorShit();
                        break;
                    case AgentPosition.AtWork:
                        this.DoWorkShit();
                        break;
                    case AgentPosition.Outside:
                        this.DoLifeShit();
                        break;
                    default:
                        throw new NotSupportedException("The operation is not supported!");
                }
            }
        }

        public override string ToString()
        {
            return $"Agent {this.Name}({this.SecurityLevel.ToString()})";
        }

        private void DoElevatorShit()
        {
            ElevatorAction action = this.GetRandomElevatorAction();

            switch (action)
            {
                case ElevatorAction.ChangeFloor:
                    Floor floor = this.GetRandomFloor();

                    if (floor != null)
                    {
                        lock (elevatorBusyFlagLocker)
                        {
                            if (!this.Elevator.IsBusy)
                            {
                                Console.WriteLine($"{this.ToString()} pushed the elevator button to floor {floor.Name}.");
                                this.Elevator.Buttons.First(f => f.ToFloor.Level == floor.Level).Push();
                            }
                        }
                    }

                    break;
                case ElevatorAction.LookInTheMirror:
                    Console.WriteLine($"{this.ToString()} looked in the mirror. Stunnng view!");
                    break;
                case ElevatorAction.GetOut:
                    if (this.Elevator.Door.IsOpen)
                    {
                        this.Elevator.RemoveAgent(this);
                        this.CurrentFloor = this.Elevator.CurrentFloor;
                        this.Position = AgentPosition.AtWork;

                        Console.WriteLine($"{this.ToString()} got out of the elevator on floor {this.CurrentFloor.Name}.");
                    }

                    break;
                default:
                    throw new NotSupportedException("This operation is not supported!");
            }
        }

        private void DoWorkShit()
        {
            WorkAction action = this.GetRandomWorkAction();

            switch (action)
            {
                case WorkAction.GoToElevator:
                    Console.WriteLine($"{this.ToString()} went to the elevator.");

                    while (this.Elevator.CurrentFloor != this.CurrentFloor && !this.Elevator.Door.IsOpen)
                    {
                        if (!this.CurrentFloor.FloorButton.IsDisabled)
                        {
                            Console.WriteLine($"{this.ToString()} called the elevator to floor {this.CurrentFloor.ToString()}.");
                            this.CurrentFloor.FloorButton.Push();
                        }
                    }

                    this.Elevator.AcceptAgent(this);
                    this.CurrentFloor = null;
                    this.Position = AgentPosition.InTheElevator;

                    Console.WriteLine($"{this.ToString()} got into the elevator.");
                    break;
                case WorkAction.GoToSecretFusionReactor:
                    Console.WriteLine($"{this.ToString()} went to see the secret fusion reactor.");
                    break;
                case WorkAction.GoSeeAlienPrisoners:
                    Console.WriteLine($"{this.ToString()} went to see the alien prisoners.");
                    break;
                case WorkAction.LeaveWork:
                    if (this.CurrentFloor == null || this.CurrentFloor.Level == 1)
                    {
                        this.Position = AgentPosition.Outside;
                        Console.WriteLine($"{this.ToString()} left work.");
                    }

                    break;
                default:
                    break;
            }
        }

        private void DoLifeShit()
        {
            LifeAction action = this.GetRandomLifeAction();

            switch (action)
            {
                case LifeAction.GoToWork:
                    this.CurrentFloor = this.Elevator.SupportedFloors.First();
                    this.Position = AgentPosition.AtWork;

                    Console.WriteLine($"{this.ToString()} went to work.");
                    break;
                case LifeAction.GoToGroceryStore:
                    Console.WriteLine($"{this.ToString()} went to the grocery store for some Lyutenitza.");
                    break;
                case LifeAction.GoHome:
                    this.Position = AgentPosition.AtHome;
                    Console.WriteLine($"{this.ToString()} went home.");
                    break;
                default:
                    throw new NotSupportedException("This action is not supported!");
            }
        }

        private Floor GetRandomFloor()
        {
            List<Floor> availableFloors = this.Elevator.SupportedFloors.Where(f => f.MinSecurityLevel <= this.SecurityLevel && f.Level != this.Elevator.CurrentFloor.Level).ToList();

            if (availableFloors.Count == 0)
            {
                return null;
            }

            return availableFloors[this.rand.Next(availableFloors.Count)];
        }

        private LifeAction GetRandomLifeAction()
        {
            return (LifeAction)rand.Next(Enum.GetNames(typeof(LifeAction)).Length);
        }

        private WorkAction GetRandomWorkAction()
        {
            return (WorkAction)rand.Next(Enum.GetNames(typeof(WorkAction)).Length);
        }

        private ElevatorAction GetRandomElevatorAction()
        {
            return (ElevatorAction)rand.Next(Enum.GetNames(typeof(ElevatorAction)).Length);
        }
    }
}
