using Godot;
using System;

public class EventHorizonPropulsionController : AbstractPropulsionController
{
	EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}    
	EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}    
	EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

	public override void PropulsionUpdate(ShipStatusInfo shipStatusInfo, ThrusterControls thrusters, float deltaTime)
	{

		// Enable the UFO drive override
		thrusters.IsUFODriveEnabled = true;
		// Set the velocity as the displacement from the target
		Vector2 velocity = shipStatusInfo.forwardVector;
		thrusters.UFODriveVelocity = velocity;
		
	}

	public override void DebugDraw(Font font)
	{
		//Student code goes here
	}
}
