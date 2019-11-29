using System.Threading;

namespace Area51Elevator
{
    class ElevatorButton
    {
        public Floor ToFloor { get; set; }

        public Elevator Elevator { get; set; }

        public bool IsDisabled => this.Elevator.IsBusy;

        public ElevatorButton(Floor toFloor, Elevator elevator)
        {
            this.ToFloor = toFloor;
            this.Elevator = elevator;
        }

        public void Push()
        {
            this.Elevator.GoToFloor(this.ToFloor);
        }
    }
}
