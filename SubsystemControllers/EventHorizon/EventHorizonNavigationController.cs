using Godot;
using System;
using System.Collections.Generic;

public class EventHorizonNavigationController : AbstractNavigationController
{
    EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}    
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
    EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

    public override void NavigationUpdate(ShipStatusInfo shipStatusInfo, GalaxyMapData galaxyMapData, float deltaTime)
    {
        //Student code goes here

        GD.Print("Forward Vector:\n", shipStatusInfo.forwardVector);

        // keep list of all warp gates identified
        List<WarpGateData> warpGatesIdentified = new List<WarpGateData>();

        warpGatesIdentified.Add(SensorsController.solSystemWarpGate);
        warpGatesIdentified.Add(SensorsController.alphaCentauriSystemWarpGate);

        //position of the ship
        Vector2 shipPosition = shipStatusInfo.positionWithinSystem;

        //check which system the ship is in
        if (shipStatusInfo.currentSystemName == SolarSystemNames.Sol)
        {
            //go to Alpha Centauri

            // get position of the warp gate to Alpha Centauri
            Vector2 warpGatePosition = new Vector2(0,0);
            foreach(WarpGateData warpGate in warpGatesIdentified) {
                GD.Print(warpGate.name, warpGate.position);
                if (warpGate.name == "WarpGate to Alpha Centauri System") {
                    warpGatePosition = warpGate.position;
                }
            }

            //Vector2 warpGatePosition = new Vector2(800, 0);

            //shortest path
            Vector2 shortestPath = new Vector2(warpGatePosition.x - shipPosition.x, warpGatePosition.y - shipPosition.y);

            shipStatusInfo.forwardVector = shortestPath;
        }
        else if (shipStatusInfo.currentSystemName == SolarSystemNames.AlphaCentauri)
        {
            //go to Kepler 438

            //position of the warp gate to Kepler 438
            //Vector2 warpGatePosition = new Vector2((float)312.978, (float)386.366);

            // get position of the warp gate to Kepler 438
            Vector2 warpGatePosition = new Vector2(0,0);
            foreach(WarpGateData warpGate in warpGatesIdentified) {
                if (warpGate.name == "WarpGate to Kepler 438 System") {
                    warpGatePosition = warpGate.position;
                }
            }

            //shortest path
            Vector2 shortestPath = new Vector2(warpGatePosition.x - shipPosition.x, warpGatePosition.y - shipPosition.y);

            shipStatusInfo.forwardVector = shortestPath;
        }

        GD.Print(shipStatusInfo.positionWithinSystem);
    }   

    public override void DebugDraw(Font font)
    {
        //Student code goes here
    }
}
