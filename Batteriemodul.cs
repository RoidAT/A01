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
    public class Batteriemodul : ISimulator
    {
        public double CurrentCapacity { get; set; }

        public double MaxCapacity { get; set; }

        public bool isFull { get; set; } = false;
        public double CurrentPower { get; set; }
        public long CurrentTime { get; set; }
        public List<ISimulator> ConnectedInputs { get; set; } = new List<ISimulator>();
        public List<ISimulator> ConnectedOutputs { get; set; } = new List<ISimulator>();

        public Batteriemodul(double maxCap)
        {
            MaxCapacity = maxCap;
        }

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

            CurrentPower = 0;

            foreach (var input in ConnectedInputs)
            {
                int connectedModules = 0;
                int allModules = 0;
                foreach(var module in input.ConnectedOutputs)
                {
                    if(module is Batteriemodul b && !b.isFull)
                    {
                        connectedModules++;
                    }
                    if(module is Batteriemodul b2)
                    {
                        allModules++;
                    }
                }
                // Check if module output is a double and add it to the total power
                if (input.GetOutput() is double output && connectedModules != 0)
                {
                    CurrentPower += output / connectedModules;
                }

                foreach(var battery in ConnectedOutputs)
                {
                    foreach(var park in battery.ConnectedOutputs)
                    {
                        if(park.GetOutput() is double used)
                        {
                            CurrentPower -= used / allModules;
                        }
                    }
                }
            }

            CurrentCapacity += ((timeMs - CurrentTime) * CurrentPower) / 3600000; // Calculate the CurrentCapacity based on time passed and CurrentPower in Wh

            if(CurrentCapacity >= MaxCapacity)
            {
                isFull = true;
                CurrentCapacity = MaxCapacity;
            }
            else
            {
                isFull = false;
            }

            CurrentTime = timeMs;
        }

        public object GetOutput()
        {
            return CurrentCapacity;
        }
    }
}