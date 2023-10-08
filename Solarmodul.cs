﻿/*
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

        private Random random = new Random();
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

        private double CalculateGeneratedPower(long timeMs)
        {
            // Implement your logic here to calculate GeneratedPower based on timeMs
            // Add randomness to the generated power within a certain range.
            long hour = (long)(timeMs / 3600000) % 24; // Convert timeMs to hours

            // Create a lookup table for power levels throughout the day
            Dictionary<long, double> timeOfDayPower = new Dictionary<long, double>
            {
                { 0, 10 },   // Midnight
                { 1, 80 },    // 1:00 AM
                { 2, 80 },    // 2:00 AM
                { 3, 80 },    // 3:00 AM
                { 4, 100 },   // 4:00 AM
                { 5, 150 },   // 5:00 AM
                { 6, 500 },   // 6:00 AM
                { 7, 800 },   // 7:00 AM
                { 8, 1200 },  // 8:00 AM
                { 9, 1500 },  // 9:00 AM
                { 10, 1800 }, // 10:00 AM
                { 11, 2000 }, // 11:00 AM
                { 12, 2200 }, // Noon
                { 13, 2200 }, // 1:00 PM
                { 14, 2200 }, // 2:00 PM
                { 15, 2000 }, // 3:00 PM
                { 16, 1000 }, // 4:00 PM
                { 17, 1500 }, // 5:00 PM
                { 18, 1200 }, // 6:00 PM
                { 19, 800 },  // 7:00 PM
                { 20, 500 },  // 8:00 PM
                { 21, 300 },  // 9:00 PM
                { 22, 150 },  // 10:00 PM
                { 23, 100 },  // 11:00 PM
                { 24, 10 }   // Midnight
            };
            if (timeOfDayPower.ContainsKey(hour))
            {
                double basePower = timeOfDayPower[hour];
                double minRandomOffset = -10; // Minimum random offset
                double maxRandomOffset = 10;  // Maximum random offset

                // Add randomness to the power level
                double randomOffset = random.NextDouble() * (maxRandomOffset - minRandomOffset) + minRandomOffset;
                double power = basePower + randomOffset;
                return Math.Max(0, power);
            }
            else
            {
                // Default power level for times not in the lookup table
                return 5000; // Adjust this value as needed
            }
            
        }

        public void Step(long timeMs)
        {
            if(timeMs == CurrentTime) return; //Don't step twice
            CurrentTime = timeMs;
            GeneratedPower = CalculateGeneratedPower(timeMs); //TODO: Implement based on Time of Day (and Sun Position). For now just set to 500
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