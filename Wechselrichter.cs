/*
 * Wechselrichter.cs
 * ---
 * Author: René Schütz
 * Der Wechselrichter soll die Leistung aller angeschlossenen Solarmodule addieren und die Effizienz berücksichtigen.
 * Die GetOutput Methode soll die aktuelle Leistung ausgeben.
 * ---
 */
using System.Collections.Generic;

namespace A01
{
    public class Wechselrichter : ISimulator
    {
        private const double Efficiency = 0.99;
        public long CurrentTime { get; set; }
        public List<ISimulator> ConnectedInputs { get; set; } = new List<ISimulator>();
        public List<ISimulator> ConnectedOutputs { get; set; } = new List<ISimulator>();

        public double CurrentPower { get; private set; }

        public void Connect(ISimulator input)
        {
            ConnectedInputs.Add(input);
            input.ConnectedOutputs.Add(this);
        }

        public void Step(long timeMs)
        {
            if(timeMs == CurrentTime) return;
            CurrentTime = timeMs;
            double inputPower = 0;
            foreach (var input in ConnectedInputs)
            {
                input.Step(timeMs);
                if (input.GetOutput() is double output)
                {
                    inputPower += output;
                }
            }

            CurrentPower = inputPower * Efficiency;
        }

        public object GetOutput()
        {
            return CurrentPower;
        }
    }

}