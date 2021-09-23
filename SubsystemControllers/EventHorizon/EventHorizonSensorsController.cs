using Godot;
using System;
using System.Collections.Generic;

public class EventHorizonSensorsController : AbstractSensorsController
{
	EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}    
	EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
	EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

	public List<EMSReading> asteroidList = new List<EMSReading>(); 

	public override void SensorsUpdate(ShipStatusInfo shipStatusInfo, IActiveSensors activeSensors, PassiveSensors passiveSensors, float deltaTime)
	{
		//Student code goes here   
		asteroidList = activeSensors.PerformScan(0, 360, 1000);
		if (asteroidList.Count > 0) 
			GD.Print(asteroidList[0].Amplitude * activeSensors.GConstant);
		
	}

	public override void DebugDraw(Font font)
	{
		//Student code goes here
	}
}
