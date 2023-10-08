/*
 * TestApp.cs
 * ---
 * Author: René Schütz
 * Diese Test App erstellt einige Simulatoren und verknüpft diese miteinander.
 * Anschließend wird die Simulation ausgeführt und die Ausgangsleistung der PhotovoltaikAnlage ausgegeben.
 * ---
 */

using System;
using A01;

var simulation = new Simulation();

// Erstellen Sie einige Simulatoren
var solarmodul1 = new Solarmodul();
var solarmodul2 = new Solarmodul();
var solarmodul3 = new Solarmodul();
var solarmodulString = new SolarmodulString();
var wechselrichter = new Wechselrichter();
var photovoltaikAnlage = new PhotovoltaikAnlage();

// Verknüpfen Sie die Simulatoren
solarmodulString.Connect(solarmodul1);
solarmodulString.Connect(solarmodul2);
solarmodulString.Connect(solarmodul3);
photovoltaikAnlage.Connect(solarmodulString);
photovoltaikAnlage.Connect(wechselrichter);

// Fügen Sie die Simulatoren zur Simulation hinzu
simulation.AddSimulator(photovoltaikAnlage);

// Führen Sie die Simulation aus
simulation.Run(1000);
