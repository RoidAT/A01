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
    public class BatterieModul : ISimulator
    {
        public double CurrentCapacity { get; set; }

        public double MaxCapacity { get; set; } = 1000000;

        public double CurrentPower { get; set; }
        public long CurrentTime { get; set; }
        public List<ISimulator> ConnectedInputs { get; set; } = new List<ISimulator>();
        public List<ISimulator> ConnectedOutputs { get; set; } = new List<ISimulator>();

        public void Connect(ISimulator input)
        {
            if (input is PhotovoltaikAnlage)
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

            foreach (var input in ConnectedInputs)
            {
                // Check if module output is a double and add it to the total power
                if (input.GetOutput() is double output)
                {
                    CurrentPower += output;
                }
            }

            CurrentCapacity = ((timeMs - CurrentTime) * CurrentPower) / 3600000; // Calculate the CurrentCapacity based on time passed and CurrentPower in Wh

            CurrentCapacity = Math.Min(CurrentCapacity, MaxCapacity); // Clamp CurrentCapacity to MaxCapacity

            CurrentTime = timeMs;
        }

        public object GetOutput()
        {
            return CurrentCapacity;
        }
    }
}