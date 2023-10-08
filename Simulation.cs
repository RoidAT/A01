/*
 * Simulation.cs
 * ---
 * Author: René Schütz
 * Contains the Simulation class.
 * This class is used to create a simulation object.
 * ---
 */

using System;
using System.Collections.Generic;

namespace A01
{
    public class Simulation
    {
        public List<ISimulator> Simulators { get; private set; } = new List<ISimulator>();

        // Ein Simulator zur Simulation hinzufügen
        public void AddSimulator(ISimulator simulator)
        {
            Simulators.Add(simulator);
        }

        // Die Step Methode für alle Simulatoren ausführen
        public void Run(long timeMs)
        {
            foreach (var simulator in Simulators)
            {
                simulator.Step(timeMs);
            }
        }
    }
}