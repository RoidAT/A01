/*
 * Ladestation.cs
 * ---
 * Author: Marcel Schörghuber
 * Das Solarmodul soll basierend auf der aktuellen Uhrzeit und der Position der Sonne die aktuelle Leistung berechnen.
 * in der GetOutput Methode soll die aktuelle Leistung ausgegeben werden.
 * ---
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace A01
{
    public class Ladestation : ISimulator
    {
        public double MaxPower { get; set; }
        public double CurrentPower { get; set; }
        public long CurrentTime { get; set; }

        public bool IsCharging { get; set; }

        
        public List<ISimulator> ConnectedInputs { get; set; } = new List<ISimulator>();
        public List<ISimulator> ConnectedOutputs { get; set; } = new List<ISimulator>();

        public Ladestation(double maxP)
        {
            MaxPower = maxP;
        }

        public void Connect(ISimulator input)
        {
            if (input is Ladepark)
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

            Random random = new Random();

            int r = random.Next(20);

            if(r == 0)
            {
                IsCharging = !IsCharging;
            }

            CurrentPower = 0;
            double availableCapacity = 0;
            foreach (var input in ConnectedInputs)
            {
                foreach (var battery in input.ConnectedInputs)
                {
                    if (battery is Batteriespeicher b)
                    {
                        availableCapacity += b.CurrentCapacity;
                    }
                }

                double availablePower = (availableCapacity / (((double)timeMs - CurrentTime) / 1000 / 60 / 60)) - 5000;

                if(availablePower > 0 && IsCharging)
                {
                    CurrentPower = Math.Min(MaxPower, availablePower); 
                }
                //else
                //{
                //    isCharging = false;
                //}

            }

            CurrentTime = timeMs;
            Thread.Sleep(10);
        }

        public object GetOutput()
        {
            return CurrentPower;
        }
    }
}