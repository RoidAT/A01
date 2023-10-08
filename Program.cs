using System;
using System.Threading;
using A01;

var simulation = new Simulation();

// Erstellen Sie einige Simulatoren
var solarmodul1 = new Solarmodul();
var solarmodul2 = new Solarmodul();
var solarmodul3 = new Solarmodul();
var solarmodulString = new SolarmodulString();
var wechselrichter = new Wechselrichter();
var photovoltaikAnlage = new PhotovoltaikAnlage();
var batteriemodul = new Batteriemodul(100);
var batteriemodul2 = new Batteriemodul(1000);
var batteriespeicher = new Batteriespeicher();
var ladestation = new Ladestation(1000);
var ladestation2 = new Ladestation(10000);
var ladestation3 = new Ladestation(100);
var ladestation4 = new Ladestation(3000);
var ladepark = new Ladepark();

// Verknüpfen Sie die Simulatoren
solarmodulString.Connect(solarmodul1);
solarmodulString.Connect(solarmodul2);
solarmodulString.Connect(solarmodul3);
photovoltaikAnlage.Connect(solarmodulString);
photovoltaikAnlage.Connect(wechselrichter);
batteriemodul.Connect(photovoltaikAnlage);
batteriemodul2.Connect(photovoltaikAnlage);
batteriespeicher.Connect(batteriemodul);
batteriespeicher.Connect(batteriemodul2);
ladepark.Connect(batteriespeicher);
ladestation.Connect(ladepark);
ladestation2.Connect(ladepark);
ladestation3.Connect(ladepark);
ladestation4.Connect(ladepark);


// Fügen Sie die Simulatoren zur Simulation hinzu
simulation.AddSimulator(photovoltaikAnlage);
simulation.AddSimulator(batteriespeicher);
simulation.AddSimulator(ladepark);

// Führen Sie die Simulation aus


for(int i = 1000; i < 100000; i+= 1000)
{
    simulation.Run(i);
    Console.WriteLine($"Ausgangsleistung der PhotovoltaikAnlage: {photovoltaikAnlage.GetOutput()} W");
    Console.WriteLine($"Cap1: {batteriemodul.GetOutput()} Wh");
    Console.WriteLine($"Cap2: {batteriemodul2.GetOutput()} Wh");
    Console.WriteLine($"CapGes: {batteriespeicher.GetOutput()} Wh");
    Console.WriteLine($"Station1: {ladestation.GetOutput()} W");
    Console.WriteLine($"Station3: {ladestation2.GetOutput()} W");
    Console.WriteLine($"Station3: {ladestation3.GetOutput()} W");
    Console.WriteLine($"Station4: {ladestation4.GetOutput()} W");
    Console.WriteLine($"Park: {ladepark.GetOutput()} W");
    Thread.Sleep(1000);
}


