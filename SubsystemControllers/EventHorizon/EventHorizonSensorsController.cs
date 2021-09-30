using Godot;
using System;
using System.Collections.Generic;

public class EventHorizonSensorsController : AbstractSensorsController
{
    EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}
    EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

    // public List<EMSReading> smallSpaceObjects = new List<EMSReading>();
    // public List<EMSReading> bigSpaceObjects = new List<EMSReading>();
    // public List<float> warpGateHeadings = new List<float>();

    public List<PassiveSensorReadingData> passiveSensorReadingData = new List<PassiveSensorReadingData>();
    public List<EMSReading> planetRawData = new List<EMSReading>();

    public override void SensorsUpdate(ShipStatusInfo shipStatusInfo, IActiveSensors activeSensors, PassiveSensors passiveSensors, float deltaTime)
    {
        passiveSensors.GeneratePassiveSensorReadings();
        List<PassiveSensorReading> passiveSensorReadings = passiveSensors.PassiveReadings;

        GD.Print("PASSIVE SENSOR READING DATA: \n");

        Vector2 shipVelocity = shipStatusInfo.linearVelocity;
        float shipHeading = (float) Math.Atan2(shipVelocity.y, shipVelocity.x);
        float scanAngle = Mathf.Clamp(Mathf.Pi/(shipVelocity.Length()/100.0f), Mathf.Pi/4, Mathf.Pi);
        float scanDistance = Mathf.Clamp(shipVelocity.Length()+50, 50, 300);

        foreach(PassiveSensorReading passiveSensorReading in passiveSensorReadings) {
            
            float heading = passiveSensorReading.Heading;
            float adjustedHeading = heading - shipHeading;

            PassiveSensorReadingData newPassiveSensorReadingData = new PassiveSensorReadingData(adjustedHeading, passiveSensorReading.ContactID);
            passiveSensorReadingData.Add(newPassiveSensorReadingData);

            GD.Print("\nContact ID: " + newPassiveSensorReadingData.contactID.ToString() + ". Adjusted Heading: " + newPassiveSensorReadingData.heading.ToString() + "\n");

            if(passiveSensorReading.Signature.ToString() == "Planetoid") {
                planetRawData = activeSensors.PerformScan(
                    adjustedHeading,
                    scanAngle,
                    scanDistance
                );
            }
            // bigSpaceObjects = activeSensors.PerformScan(
                
            // )

            // output will be list of objects containing proper heading and contact id

            // GD.Print("\nPASSIVE SENSOR READINGS: \n");
            // GD.Print(passiveSensorReading.Signature.ToString() + "  " + passiveSensorReading.Heading.ToString());

        }

        foreach(EMSReading planetData in planetRawData) {
            GD.Print("scan signature: " + planetData.ScanSignature);
        }

        // get contact id and heading from passiveSensorReadings, 
        // and use the heading to perform an active scan - since they have contact id they know ht ecircumference of the object - the second parameeter to the scan is the arc length, and because the size of the beam uses energy we want to match what we're looking for

        // HARD CODED DATA FOR GALAXY ALPHA:
        WarpGateData solSystemWarpGate = new WarpGateData("WarpGate to Alpha Centauri System", new Vector2( (float)800,(float)0 ), "Alpha Centauri System");
        WarpGateData alphaCentauriSystemWarpGate = new WarpGateData("WarpGate to Kepler 438 System", new Vector2( (float)312.978, (float)386.366 ), "Kepler 438 System");

        GD.Print("\nWARP GATE DATA: \n");
        GD.Print("Name: " + solSystemWarpGate.name + ". Destination: " + solSystemWarpGate.destinationSolarSystemName.ToString() + ". Position: " + solSystemWarpGate.position.ToString() + ".");
        GD.Print("Name: " + alphaCentauriSystemWarpGate.name + ". Destination: " + alphaCentauriSystemWarpGate.destinationSolarSystemName.ToString() + ". Position: " + alphaCentauriSystemWarpGate.position.ToString() + ".");

        PlanetData kepler438Planet = new PlanetData("Planet Kepler 438", new Vector2( (float)1276.38, (float)107.665 ));

        GD.Print("\nPLANET WARP DATA: \n");
        GD.Print("Name: " + kepler438Planet.name + ". Position: " + kepler438Planet.position.ToString() + ".\n");

        // foreach (PassiveSensorReading bigSpaceObject in bigSpaceObjects) {
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

public struct PassiveSensorReadingData {
    public float heading;
    public ulong contactID;
	public PassiveSensorReadingData(float h, ulong c) {
		heading = h;
		contactID = c;
	}    
}