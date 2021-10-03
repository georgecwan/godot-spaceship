// lots of stuff is referenced from other sensors team

using Godot;
using System;
using System.Collections.Generic;

public class EventHorizonSensorsController : AbstractSensorsController
{
    EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}
    EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

    public List<PassiveSensorReadingData> passiveSensorReadingData = new List<PassiveSensorReadingData>();
    public List<EMSReading> planetRawData = new List<EMSReading>();
    public List<EMSReading> warpGateRawData = new List<EMSReading>();
    public List<PlanetData> planetList = new List<PlanetData>();
    public List<WarpGateData> warpGateList = new List<WarpGateData>();

    public override void SensorsUpdate(ShipStatusInfo shipStatusInfo, IActiveSensors activeSensors, PassiveSensors passiveSensors, float deltaTime)
    {
        passiveSensors.GeneratePassiveSensorReadings();
        List<PassiveSensorReading> passiveSensorReadings = passiveSensors.PassiveReadings;

        // PASSIVE SENSOR READING DATA:

        Vector2 shipVelocity = shipStatusInfo.linearVelocity;
        float shipHeading = (float) Math.Atan2(shipVelocity.y, shipVelocity.x);
        float scanAngle = Mathf.Clamp(Mathf.Pi/(shipVelocity.Length()/100.0f), Mathf.Pi/4, Mathf.Pi);
        float scanDistance = Mathf.Clamp(shipVelocity.Length()+50, 50, 300);

        foreach(PassiveSensorReading passiveSensorReading in passiveSensorReadings) {
            
            float heading = passiveSensorReading.Heading;
            float adjustedHeading = heading - shipHeading;

            PassiveSensorReadingData newPassiveSensorReadingData = new PassiveSensorReadingData(adjustedHeading, passiveSensorReading.ContactID);
            passiveSensorReadingData.Add(newPassiveSensorReadingData);

            if(passiveSensorReading.Signature.ToString() == "Planetoid") {
                planetRawData = activeSensors.PerformScan(
                    adjustedHeading,
                    scanAngle,
                    scanDistance
                );
            }

            if(passiveSensorReading.Signature.ToString() == "WarpGate") {
                warpGateRawData = activeSensors.PerformScan(
                    adjustedHeading,
                    scanAngle,
                    scanDistance
                );
            }
        }

        foreach(EMSReading a in planetRawData) {
			// Calculate the position of the planet in cartesian coordinates
			float x = (float)Math.Cos(a.Angle) * a.Amplitude * activeSensors.GConstant;
			float y = (float)Math.Sin(a.Angle) * a.Amplitude * activeSensors.GConstant;
			
			PlanetData newPlanet = new PlanetData("Planet", new Vector2(x, y), a.ScanSignature); // TODO: change name

            planetList.Add(newPlanet);

		}

        foreach(EMSReading a in warpGateRawData) {
			// Calculate the position of the warp gate in cartesian coordinates
			float x = (float)Math.Cos(a.Angle) * a.Amplitude * activeSensors.GConstant;
			float y = (float)Math.Sin(a.Angle) * a.Amplitude * activeSensors.GConstant;

			WarpGateData newWarpGate = new WarpGateData("WarpGate to " + a.SpecialInfo, new Vector2(x, y), a.SpecialInfo);

            warpGateList.Add(newWarpGate);

		}

        // HARD CODED DATA FOR GALAXY ALPHA:
        WarpGateData solSystemWarpGate = new WarpGateData("WarpGate to Alpha Centauri System", new Vector2( (float)800,(float)0 ), "Alpha Centauri System");
        WarpGateData alphaCentauriSystemWarpGate = new WarpGateData("WarpGate to Kepler 438 System", new Vector2( (float)312.978, (float)386.366 ), "Kepler 438 System");

        PlanetData kepler438Planet = new PlanetData("Planet Kepler 438", new Vector2( (float)1276.38, (float)107.665 ), "Common:70|Metal:20|Water:10");
    }

    public override void DebugDraw(Font font)
    {
        //Student code goes here
    }
}

public struct WarpGateData {
    public String name;
	public Vector2 position;
	public String specialInfo;

	public WarpGateData(String n, Vector2 p, String s) {
		name = n;
        position = p;
		specialInfo = s;
	}
}

public struct PlanetData {
    public String name;
	public Vector2 position;
    public String scanSignature;

	public PlanetData(String n, Vector2 p, String s) {
		name = n;
		position = p;
        scanSignature = s;
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