using Godot;
using System;

public class EventHorizonDefenceController : AbstractDefenceController
{
    EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}
    EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
    float[] tubeCooldowns;
    Vector2 shipCoordinates;
    Vector2 shipVelocity;
    float speed;
    float explosionRadius;
    
    public override void DefenceUpdate(ShipStatusInfo shipStatusInfo, TurretControls turretControls, float deltaTime)
    {
        shipCoordinates = shipStatusInfo.positionWithinSystem;
        shipVelocity = shipStatusInfo.linearVelocity;
        tubeCooldowns = new float[] {
            turretControls.GetTubeCooldown(0),
            turretControls.GetTubeCooldown(1),
            turretControls.GetTubeCooldown(2),
            turretControls.GetTubeCooldown(3),
        };

        speed = Torpedo.LaunchSpeed;
        explosionRadius = Torpedo.ExplosionRadius;
        // List<Asteroid> = EventHorizonSensorsController.asteroidsList;
    }

    public override void DebugDraw(Font font)
    {

    }
}
