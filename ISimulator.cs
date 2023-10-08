/*
 * ISimulator.cs
 * ---
 * Author: René Schütz
 * This interface is used to create a simulator object.
 * In the simulation the simulator objects are connected to each other. and are calling the Step method on each other.
 * ---
 */

using System.Collections.Generic;

namespace A01
{
    public interface ISimulator
    {
        long CurrentTime { get; set; }
        List<ISimulator> ConnectedInputs { get; set; }
        List<ISimulator> ConnectedOutputs { get; set; }
        void Connect(ISimulator input);
        void Step(long time);
        object GetOutput();
    }
}