using Godot;
using System;

public class EventHorizonDefenceController : AbstractDefenceController
{
    EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}
    EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
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

        speed = Torpedo.LaunchSpeed;
        explosionRadius = Torpedo.ExplosionRadius;
        // List<Asteroid> = EventHorizonSensorsController.asteroidsList;

        shootTorpedoes(turretControls);
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
    public float timeToCollide(Vector2 position, Vector2 velocity, float collisionRadius = 50f) {
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

    /*
        Takes the relative position of the asteroid and the absolute
        velocity to calculate the next target for the turret.
    */
    // public Vector2 getNextTargetVector(Vector2 position, Vector2 velocity) {
        

    // }

    public void shootTorpedoes(TurretControls turretControls) 
    {
        turretControls.aimTo = shipCoordinates + new Vector2(1, 0);  // Always aim straight right
        turretControls.TriggerTube(readyTube(turretControls), 0.5f);
    }
    
    public int readyTube(TurretControls turretControls) 
    {
        
        // Loops through tubes to check which one is ready
        for(int i = 0; i < 4; i++)
        {
            // TurretControls tc = new TurretControls();
            float tubeCooldown = turretControls.GetTubeCooldown(i);
            GD.Print(tubeCooldown); 
            if(tubeCooldown==0)
            {
                return i; 
            }
        }
        return 4; // 4 represents that no tubes are ready
    }
    public override void DebugDraw(Font font)
    {
        
    }
}
