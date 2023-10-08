/*
 * Simulation.cs
 * ---
 * Author: René Schütz
 * Das Solarmodul soll basierend auf der aktuellen Uhrzeit und der Position der Sonne die aktuelle Leistung berechnen.
 * in der GetOutput Methode soll die aktuelle Leistung ausgegeben werden.
 * ---
 */

using System;
using System.Collections.Generic;

namespace A01
{
    public class Batteriespeicher : ISimulator
    {
        public double CurrentCapacity { get; set; }

        public double MaxCapacity { get; set; } = 1000000;
        public long CurrentTime { get; set; }
        public List<ISimulator> ConnectedInputs { get; set; } = new List<ISimulator>();
        public List<ISimulator> ConnectedOutputs { get; set; } = new List<ISimulator>();

        public void Connect(ISimulator input)
        {
            if (input is Batteriemodul)
            {
                ConnectedInputs.Add(input);
                input.ConnectedOutputs.Add(this);
            }
            else
            {
                throw new InvalidOperationException("An attempt was made to connect an incompatible module.");
            }
        }



        public void Step(long timeMs)
        {
            if (timeMs == CurrentTime) return; //Don't step twice

            CurrentCapacity = 0;

            foreach (var input in ConnectedInputs)
            {
                input.Step(timeMs);
                // Check if module output is a double and add it to the total power
                if (input.GetOutput() is double output)
                {
                    CurrentCapacity += output;
                }
            }
        }

        public object GetOutput()
        {
            return CurrentCapacity;
        }
    }
}