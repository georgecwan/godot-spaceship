using Godot;
using System;
using System.Collections.Generic;

public class EventHorizonSensorsController : AbstractSensorsController
{
    EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}
    EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

    public List<EMSReading> smallSpaceObjects = new List<EMSReading>();
    public List<PassiveSensorReading> bigSpaceObjects = new List<PassiveSensorReading>();
    public List<float> warpGateHeadings = new List<float>();
    public WarpGateData solSystemWarpGate = new WarpGateData("WarpGate to Alpha Centauri System", new Vector2( (float)800,(float)0 ), "Alpha Centauri System");
    public WarpGateData alphaCentauriSystemWarpGate = new WarpGateData("WarpGate to Kepler 438 System", new Vector2( (float)312.978, (float)386.366 ), "Kepler 438 System");

    public override void SensorsUpdate(ShipStatusInfo shipStatusInfo, IActiveSensors activeSensors, PassiveSensors passiveSensors, float deltaTime)
    {

        // HARD CODED DATA FOR GALAXY ALPHA:
        // WarpGateData solSystemWarpGate = new WarpGateData("WarpGate to Alpha Centauri System", new Vector2( (float)800,(float)0 ), "Alpha Centauri System");
        // WarpGateData alphaCentauriSystemWarpGate = new WarpGateData("WarpGate to Kepler 438 System", new Vector2( (float)312.978, (float)386.366 ), "Kepler 438 System");

        GD.Print("WARP GATE DATA: \n");
        GD.Print("Name: " + solSystemWarpGate.name + ". Destination: " + solSystemWarpGate.destinationSolarSystemName.ToString() + ". Position: " + solSystemWarpGate.position.ToString() + ".");
        GD.Print("Name: " + alphaCentauriSystemWarpGate.name + ". Destination: " + alphaCentauriSystemWarpGate.destinationSolarSystemName.ToString() + ". Position: " + alphaCentauriSystemWarpGate.position.ToString() + ".");

        PlanetData kepler438Planet = new PlanetData("Planet Kepler 438", new Vector2( (float)1276.38, (float)107.665 ));

        GD.Print("\nPLANET WARP DATA: \n");
        GD.Print("Name: " + kepler438Planet.name + ". Position: " + kepler438Planet.position.ToString() + ".\n");

        // foreach (PassiveSensorReading bigSpaceObject in bigSpaceObjects) {
        //     //bigSpaceObject.Signature.toString();
        //     //GD.Print(bigSpaceObject.Signature.toString());
            
        //     // GD.Print("isPlanetoid: ");
        //     // bool isPlanetoid = bigSpaceObject.Signature.toString() == "Planetoid";
        //     // GD.Print(isPlanetoid);
            
        //     // GD.Print("isUnknown: ");
        //     // bool isUnknown = bigSpaceObject.Signature.toString() == "Unknown";
        //     // GD.Print(isUnknown);
            
        //     // GD.Print("isWarpGate: ");
        //     // bool isWarpGate = bigSpaceObject.Signature.toString() == "WarpGate";
        //     // GD.Print(isWarpGate);

        //     string printing = "";
        //     for (int i = 0; i < bigSpaceObjects.Count; i++) {
        //         PassiveSensorReading psr = bigSpaceObjects[i];
        //         printing += (psr.Signature.ToString() + " done ");
        //         if (psr.Signature.ToString() == "WarpGate") {
        //             warpGateHeadings.append
        //         }
        //     }
        //     GD.Print(printing);


        // }

    }

    public override void DebugDraw(Font font)
    {
        //Student code goes here
    }
}

public struct WarpGateData {
    public String name;
	public Vector2 position;
	public String destinationSolarSystemName;

	public WarpGateData(String n, Vector2 p, String d) {
		name = n;
        position = p;
		destinationSolarSystemName = d;
	}
}

public struct PlanetData {
    public String name;
	public Vector2 position;

	public PlanetData(String n, Vector2 p) {
		name = n;
		position = p;
	}
}
