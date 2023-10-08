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
    public class Solarmodul : ISimulator
    {
        public double CurrentPower { get; set; }
        public double GeneratedPower { get; set; }
        public long CurrentTime { get; set; }
        public List<ISimulator> ConnectedInputs { get; set; } = new List<ISimulator>();
        public List<ISimulator> ConnectedOutputs { get; set; }= new List<ISimulator>();

        public void Connect(ISimulator input)
        {
            if (input is Solarmodul)
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
            if(timeMs == CurrentTime) return; //Don't step twice
            CurrentTime = timeMs;
            GeneratedPower = 500; //TODO: Implement based on Time of Day (and Sun Position). For now just set to 500
            CurrentPower = GeneratedPower;

            foreach (var input in ConnectedInputs)
            {
                input.Step(timeMs);
                if (input.GetOutput() is double output)
                {
                    CurrentPower += output;
                }
            }
        }

        public object GetOutput()
        {
            return GeneratedPower;
        }
    }
}