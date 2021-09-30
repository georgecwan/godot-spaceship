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
        Console.Write("Galaxy data:" + galaxyMapData.nodeData[0].edges[0].edgeCost + "\n");
        
        //position of the ship
        Vector2 shipPosition = shipStatusInfo.positionWithinSystem;

        //check which system the ship is in
        if (shipStatusInfo.currentSystemName == SolarSystemNames.Sol)
        {
            //go to Alpha Centauri

            //position of the warp gate to Alpha Centauri
            Vector2 warpGatePosition = new Vector2(800, 0);

            //shortest path
            Vector2 shortestPath = new Vector2(warpGatePosition.x - shipPosition.x, warpGatePosition.y - shipPosition.y);

            shipStatusInfo.forwardVector = shortestPath;
        }
        else if (shipStatusInfo.currentSystemName == SolarSystemNames.AlphaCentauri)
        {
            //go to Kepler 438

            //position of the warp gate to Kepler 438
            Vector2 warpGatePosition = new Vector2((float)312.978, (float)386.366);

            //shortest path
            Vector2 shortestPath = new Vector2(warpGatePosition.x - shipPosition.x, warpGatePosition.y - shipPosition.y);

            shipStatusInfo.forwardVector = shortestPath;
        }
    }

    public static int[] djikstra(GalaxyMapData galaxyMapData, string solarSystemName)
    {
       // foreach ()

        return new int[0];
    }

    public override void DebugDraw(Font font)
    {
        //Student code goes here
    }
}
