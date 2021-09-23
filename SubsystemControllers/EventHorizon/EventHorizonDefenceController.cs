using Godot;
using System;

public class EventHorizonDefenceController : AbstractDefenceController
{
    EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}
    EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
    public override void DefenceUpdate(ShipStatusInfo shipStatusInfo, TurretControls turretControls, float deltaTime)
    {
        Vector2 shipCoordinates = shipStatusInfo.positionWithinSystem;
        Vector2 shipVelocity = shipStatusInfo.linearVelocity;
        float[] tubeCooldowns = new float[] {
            turretControls.GetTubeCooldown(0),
            turretControls.GetTubeCooldown(1),
            turretControls.GetTubeCooldown(2),
            turretControls.GetTubeCooldown(3),
        };

        float speed = Torpedo.LaunchSpeed;
        float explosionRadius = Torpedo.ExplosionRadius;
        // List<Asteroid> = EventHorizonSensorsController.asteroidsList;
    }

    public override void DebugDraw(Font font)
    {
        //Student code goes here
    }
}
