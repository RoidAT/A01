/*
 * Simulation.cs
 * ---
 * Author: René Schütz
 * Der Simulationsklasse kann mehrere Simulations Objekte gegeben werden welche durch die Step Methode simuliert werden.
 * 
 * ---
 */

using System;
using System.Collections.Generic;

namespace A01{
    public class Simulation {
        public List<ISimulator> Simulators { get; private set; } = new List<ISimulator>();
        public long StepSize { get; set; } = 1; // Standardwert ist 1
        public double SimulationSpeed { get; set; } = 1.0; // Standardwert ist Echtzeit
        
        public void AddSimulator(ISimulator simulator) {
            Simulators.Add(simulator);
        }
        
        public void Run(long timeMs) {
            long adjustedTime = (long)(timeMs * SimulationSpeed);
            for(long i = 0; i < adjustedTime; i += StepSize) {
                foreach (var simulator in Simulators) {
                    simulator.Step(StepSize);
                    Console.WriteLine($"Ausgangsleistung der PhotovoltaikAnlage: {simulator.GetOutput()} W");
                }
            }
        }
    }
}
