using System;
using System.Collections.Generic;

namespace A01
{
    public class PhotovoltaikAnlage : ISimulator
    {
        public long CurrentTime { get; set; }
        public List<ISimulator> ConnectedInputs { get; set; } = new List<ISimulator>();
        public List<ISimulator> ConnectedOutputs { get; set; } = new List<ISimulator>();
        public Wechselrichter Inverter { get; private set; }

        public double CurrentPower { get; private set; }

        public void Connect(ISimulator input)
        {
            if (input is SolarmodulString solarmodulString)
            {
                ConnectedInputs.Add(solarmodulString);
                // Wenn ein Wechselrichter bereits verbunden ist, füge den SolarmodulString hinzu
                Inverter?.Connect(solarmodulString);
            }
            else if (input is Wechselrichter wechselrichter)
            {
                Inverter = wechselrichter;
                // Füge alle bisher verbundenen SolarmodulStrings zum Wechselrichter hinzu
                foreach (var moduleString in ConnectedInputs)
                {
                    Inverter.Connect(moduleString);
                }
                input.ConnectedOutputs.Add(this);
            }
            else
            {
                throw new InvalidOperationException("Nur SolarmodulStrings oder Wechselrichter können an eine PhotovoltaikAnlage angeschlossen werden.");
            }

        }

        public void Step(long timeMs)
        {
            if(timeMs == CurrentTime) return;
            CurrentTime = timeMs;

            if (Inverter == null)
            {
                throw new InvalidOperationException("Es wurde kein Wechselrichter mit der PhotovoltaikAnlage verbunden.");
            }

            Inverter.Step(timeMs);
            CurrentPower = (double)Inverter.GetOutput();
        }

        public object GetOutput()
        {
            return CurrentPower;
        }
    }
}