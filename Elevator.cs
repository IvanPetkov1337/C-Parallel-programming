using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject
{
    enum ElevatorState
    {
        Moving,
        DoorsOpen,
        DoorsClosed
    }

    class Elevator
    {
        const int capacity = 2;
        List<Agent> agentsInElevator;
        Semaphore semaphore;
        internal Floor currentFloor;
        internal Floor destinationFloor;
        internal ElevatorState elevatorState;
        object locker;

        public Elevator()
        {
            this.agentsInElevator = new List<Agent>(capacity);
            this.semaphore = new Semaphore(capacity, capacity);
            this.currentFloor = Floor.Ground;
            this.elevatorState = ElevatorState.DoorsOpen;
            this.locker = new object();
        }

        public void SetDestination(Agent agent)
        {
            lock (locker)
            {

                destinationFloor = agent.destination;
                Console.WriteLine($"{agent} Send Elevator to {destinationFloor}");
                Move();
            }
        }

        public void CallElevator(Agent agent)
        {
            lock (locker)
            {
                destinationFloor = agent.currnentFloor;
                Console.WriteLine($"{agent} Called Elevator to {destinationFloor}");
                Move();
            }
        }

        private void Move()
        {
            lock (agentsInElevator)
            {
                int movementTime = Math.Abs(currentFloor - destinationFloor) * 1000;
                elevatorState = ElevatorState.Moving;
                Console.WriteLine($"Elevator Moving for {movementTime} seconds from {currentFloor} to {destinationFloor}");
                Thread.Sleep(movementTime);
                currentFloor = destinationFloor;
                elevatorState = ElevatorState.DoorsOpen;
            }
        }

        public bool Enter(Agent agent)
        {
            semaphore.WaitOne();
            lock (agentsInElevator)
            {
                if (currentFloor.Equals(agent.currnentFloor) && agent.state == State.Waiting && agentsInElevator.Count < capacity)
                {
                    Console.WriteLine($"{agent}Enters the elevator");
                    agentsInElevator.Add(agent);
                    agent.state = State.InElevator;
                    return true;
                }
            }
            return false;
        }

        public void Exit(Agent agent)
        {
            semaphore.Release();
            lock (agentsInElevator)
            {
                agentsInElevator.Remove(agent);
            }
        }
    }
}
