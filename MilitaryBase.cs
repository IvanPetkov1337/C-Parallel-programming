using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalProject
{
    enum Floor
    {
        Ground,
        Secret,
        TopSecret_1,
        TopSecret_2
    }
    class MilitaryBase
    {

        internal Elevator elevator;
        int agentCount = 6;
        List<Agent> agents;

        public MilitaryBase()
        {
            this.elevator = new Elevator();
            this.agents = new List<Agent>(agentCount);
        }

        public void StartDay()
        {
            for (int i = 0; i < agentCount; i++) agents.Add(new Agent(i, elevator));

            foreach (Agent agent in agents)
            {
                agent.GoToWork();
            }

            while (agents.Exists(x =>  x.IsAtWork()))
            {

            }
        }

        public void DisplayAgentStates()
        {
            Agent.PrintLabel();
            foreach (Agent agent in agents) Console.WriteLine(agent);
        }


    }
}
