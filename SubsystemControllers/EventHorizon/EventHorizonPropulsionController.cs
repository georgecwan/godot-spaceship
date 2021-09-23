using Godot;
using System;

public class EventHorizonPropulsionController : AbstractPropulsionController
{
	EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}    
	EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}    
	EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

	public override void PropulsionUpdate(ShipStatusInfo shipStatusInfo, ThrusterControls thrusters, float deltaTime)
	{
		//Student code goes here

		//Enable the UFO drive override
		// thrusters.IsUFODriveEnabled = true;
		//fly down and to the right at a speed of 141 pixels per second
		// Vector2 velocity = new Vector2(100, 100);
		// thrusters.UFODriveVelocity = velocity;
	}

	public override void DebugDraw(Font font)
	{
		//Student code goes here
	}
}
