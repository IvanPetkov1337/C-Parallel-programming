using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject
{
    enum State
    {
        ComingToWork,
        LeavingWork,
        CallElevator,
        InElevator,
        Waiting,
        Working
    }
    enum SecurityLevel
    {
        Confidential,
        Secret,
        TopSecret,
    }
    class Agent
    {
        int id;
        internal State state;
        internal SecurityLevel securityLevel;
        internal Floor currnentFloor;
        internal Elevator elevator;
        internal Floor destination;
        Thread thread;

        public Agent(int id, Elevator elevator)
        {
            this.id = id;
            this.state = State.ComingToWork;
            this.securityLevel = GenerateRandomSecurityLevel();
            this.elevator = elevator;
        }

        private SecurityLevel GenerateRandomSecurityLevel()
        {
            return (SecurityLevel)new Random().Next(Enum.GetNames(typeof(SecurityLevel)).Length);
        }

        public void GoToWork()
        {
            thread = new Thread(StartWork);
            thread.Start();
        }

        private void StartWork()
        {
            ComeToWork();

            while (true)
            {
                destination = GetDesiredFloor(currnentFloor);

                if (destination == Floor.Ground) state = State.LeavingWork;
                if (state == State.Waiting) SetActionWhenWaiting();
                if (state == State.InElevator && elevator.elevatorState == ElevatorState.DoorsOpen)
                {
                    if (HasAccessToFloor())
                    {
                        elevator.Exit(this);
                        Work();
                    }
                    else destination = GetDesiredFloor(destination);
                }

                if (LeaveWork()) break;
                
            }
        }

        public bool HasAccessToFloor()
        {
            switch (securityLevel)
            {
                case SecurityLevel.Confidential: return destination == Floor.Ground;
                case SecurityLevel.Secret: return destination <= Floor.Secret;
                case SecurityLevel.TopSecret: return destination <= Floor.TopSecret_2;
                default: throw new Exception("Security level non-existent");
            }
        }

        private void SetActionWhenWaiting()
        {
            if (elevator.currentFloor == currnentFloor)
            {
                if (elevator.elevatorState == ElevatorState.DoorsOpen) elevator.Enter(this);
            }
            else elevator.CallElevator(this);
        }

        internal void Work()
        {
            int workTime = new Random().Next(5, 100);
            Console.WriteLine($"{this}Is working for {workTime} time");
            currnentFloor = destination;
            state = State.Working;
            Thread.Sleep(workTime);
            destination = GetDesiredFloor(currnentFloor);
            state = State.Waiting;
        }

        private void ComeToWork()
        {
            currnentFloor = Floor.Ground;
            state = State.Waiting;
            Console.WriteLine($"{this}Is comming to work");
        }
        private bool LeaveWork()
        {
            if (destination != Floor.Ground) return false;
            Console.WriteLine($"{this}Leaves work");
            return true;
        }

        private Floor GetDesiredFloor(Floor repeatedFloor)
        {
            while (true)
            {
                Floor newFloor = (Floor)new Random().Next(Enum.GetValues(typeof(Floor)).Length);
                    if (newFloor != repeatedFloor) return newFloor;
            }
        }

        public bool IsAtWork()
        {
            return state != State.LeavingWork && currnentFloor == Floor.Ground;
        }

        public static void PrintLabel()
        {
            Console.WriteLine("{0,-2}│{1,-15}│{2,-15}│{3,-15}│{4,-15}", "ID", "Security Level", "State", "Current Floor", "Desired Floor");
            Console.Write("──");
            for (int i = 0; i < 4; i++)
            {
                Console.Write("{0}", "┼" + new String('─', 15));
            }
            Console.WriteLine();
        }
        public override string ToString()
        {
            return new string($"{id,-2}│{securityLevel,-15}│{state,-15}│{currnentFloor,-15}│{destination,-15}");
        }
    }
}
