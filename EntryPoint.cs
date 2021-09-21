using System;
using System.Collections.Generic;

namespace FinalProject
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            MilitaryBase militaryBase = new MilitaryBase();
            Agent.PrintLabel();

            militaryBase.StartDay();
        }
    }
}
