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
    float shipCollisionRadius;  // Hardcoded to 30.8946f
    float speed = Torpedo.LaunchSpeed;
    float explosionRadius = Torpedo.ExplosionRadius;
    
    public override void DefenceUpdate(ShipStatusInfo shipStatusInfo, TurretControls turretControls, float deltaTime)
    {
        shipCoordinates = shipStatusInfo.positionWithinSystem;
        shipVelocity = shipStatusInfo.linearVelocity;
        shipCollisionRadius = shipStatusInfo.shipCollisionRadius;
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

    /*
        Author: Jason Yuan

        Takes relative position and absolute velocity 
        of the asteroid (because absolute position doesn't matter).
        Also needs the collisionRadius of the asteroid.
        
        Returns a float that represents the time in seconds until a collision will occur.
        If no collision will occur, it returns +Infinity, which can later be tested with
        float.IsInfinity(x).

        Note that the time provided is not necessarily the closest time. Since this function only checks
        if the collision boxes will intersect, it's possible that the intersection found by the function
        is not the first instance of intersection (but it should be close unless the collision boxes are huge).
    */
    public float timeToCollide(Vector2 position, Vector2 velocity, float collisionRadius) {
        // Loop around various points on the collision circle of the asteroid and ship
        // to check if a collision will occur.

        // Originally, my idea was to simply check if the center of the ship and the center of the asteroid would collide,
        // but I completely forgot about the collision radius.
        for (double a = 0; a < 2 * Math.PI; a += 0.1) {
            // Break down x and y components of the asteroid's position vector,
            // for every rotation around the collision circle
            float asteroidX = (float) ((double) position.x + (double) collisionRadius * Math.Cos(a));
            float asteroidY = (float) ((double) position.y + (double) collisionRadius * Math.Sin(a));

            for (double b = 0; b < 2 * Math.PI; b += 0.1) {
                // Break down x and y components of the ships's position vector (center = 0,0),
                // for every rotation around the collision circle
                float shipX = (float) ((double) shipCollisionRadius *  Math.Cos(b));
                float shipY = (float) ((double) shipCollisionRadius *  Math.Sin(b));

                // Calculate the time for each component to collide
                float timeX = (shipX - asteroidX) / (velocity.x - shipVelocity.x);
                float timeY = (shipY - asteroidY) / (velocity.y - shipVelocity.y);


                // If the times are close enough, return the time before collision
                float tolerance = 0.0001f;
                
                if (Math.Abs(timeX - timeY) <= tolerance) {
                    return timeX;
                }
            }
        }
        
        // If no points in the circle will ever intersect with each other, return positive infinity
        return float.PositiveInfinity;
    }

    
    public void shootTorpedoes() 
    {
        // turretControls.aimTo = ; // will call function that Jason makes
        TurretControls.TriggerTube(readyTube, timeToCollide);
    }
    

    public int readyTube() 
    {
        //Loops thugh tubes to check which is ready
        for(int i=0; a<3; a++){
            float tubeCooldown = turretControls.getTubeCooldown(i);
            GD.Print(tubeCooldown); 
            if(tubeCooldown==0){
                return i; 
            }
        }
    }
    
    public override void DebugDraw(Font font)
    {
        
    }
}
