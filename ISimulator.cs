/*
 * ISimulator.cs
 * ---
 * Author: René Schütz
 * Das Interface für alle Simulatoren beinhaltet alle Methoden und Properties welche ein Simulator implementieren muss.
 * In der Simulation werden die Simulator Objekte miteinander verbunden und rufen die Step Methoden der verbundenen
 * Objekte auf.
 * ---
 */

using System.Collections.Generic;

public interface ISimulator
{
    long CurrentTime { get; set; }
    List<ISimulator> ConnectedInputs { get; set; }
    List<ISimulator> ConnectedOutputs { get; set; }
    void Connect(ISimulator input);
    void Step(long time);
    object GetOutput();
}
