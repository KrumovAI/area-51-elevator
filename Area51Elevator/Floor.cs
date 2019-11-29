namespace Area51Elevator
{
    class Floor
    {
        public string Name { get; private set; }

        public int Level { get; private set; }

        public SecurityLevel MinSecurityLevel { get; private set; }

        public ElevatorButton FloorButton { get; private set; }

        public Floor(string floorName, int level, SecurityLevel minSecurityLevel)
        {
            this.Name = floorName;
            this.Level = level;
            this.MinSecurityLevel = minSecurityLevel;
        }

        public void MountElevatorButton(ElevatorButton elevatorButton)
        {
            this.FloorButton = elevatorButton;
        }
    }
}
