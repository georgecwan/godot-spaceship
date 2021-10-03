using Godot;
using System;

public class EventHorizonPropulsionController : AbstractPropulsionController
{
	EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}    
	EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}    
	EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

	public override void PropulsionUpdate(ShipStatusInfo shipStatusInfo, ThrusterControls thrusters, float deltaTime)
	{

		// [Old testing code for reference] Enable the UFO drive override
		thrusters.IsUFODriveEnabled = true;
		// Set the velocity as the displacement from the target
		thrusters.UFODriveVelocity = shipStatusInfo.forwardVector;

		// Activate sequence whenever we get close enough to the destination
		if(isInRange(shipStatusInfo.forwardVector, shipStatusInfo.shipCollisionRadius)){
			thrusters.TriggerWarpJump();
			thrusters.TriggerLandingSequence();
		}
		//Vector2 linearVelocity = shipStatusInfo.linearVelocity;
		//translationUpdate(shipStatusInfo, thrusters);
	}
	
	public bool isInRange(Vector2 displacement, float dist){
		float x = (float) Math.Pow((double) displacement.x, 2);
		float y = (float) Math.Pow((double) displacement.y, 2);
		return x + y <= Math.Pow(dist, 2);
	}

	public void translationUpdate(ShipStatusInfo shipStatusInfo, ThrusterControls thrusters)
	{
		/*
			Main thruster --> goes forward in current direction
			Port Retro and Starboard Retro --> goes backward in current direction
			
			Note: This won't work properly until we are able to determine the angle of the ship :x
		*/
		Vector2 displacement = shipStatusInfo.forwardVector;
		// Only correct if the ship is facing right/east 
		if(displacement.x < 0){ 
			thrusters.MainThrottle = 0;
			thrusters.PortRetroThrottle = 1;
			thrusters.StarboardRetroThrottle = 1;
		} else if(displacement.x == 0){
			thrusters.MainThrottle = 0;
			thrusters.PortRetroThrottle = 0;
			thrusters.StarboardRetroThrottle = 0;
		} else {
			thrusters.MainThrottle = 1;
			thrusters.PortRetroThrottle = 0;
			thrusters.StarboardRetroThrottle = 0;
		}
	}

	public override void DebugDraw(Font font)
	{
		//Student code goes here
	}
}
