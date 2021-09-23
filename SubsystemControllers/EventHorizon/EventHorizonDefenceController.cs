using Godot;
using System;
using System.Collections.Generic;

public class EventHorizonDefenceController : AbstractDefenceController
{
    EventHorizonSensorsController SensorsController {get{return parentShip.SensorsController as EventHorizonSensorsController;}}
    EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}
    EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
    Vector2 shipCoordinates;
    Vector2 shipVelocity;
    float shipCollisionRadius;  // Hardcoded to 30.8946f
    float speed = Torpedo.LaunchSpeed;  // Hardcoded to 600f
    float explosionRadius = Torpedo.ExplosionRadius;  // Hardcoded to 150f
    
    /*
        A list of asteroids compiled by sensors. Each element will contain:
         - Asteroid position (Vector2)
         - Asteroid velocity (Vector2)
         - Asteroid radius (float)
         - Asteroid distance (float)

        The list will be sorted by increasing distance.
    */
    List<AsteroidData> asteroidList;
    
    public override void DefenceUpdate(ShipStatusInfo shipStatusInfo, TurretControls turretControls, float deltaTime)
    {
        // Initialize global variables
        shipCoordinates = shipStatusInfo.positionWithinSystem;
        shipVelocity = shipStatusInfo.linearVelocity;
        shipCollisionRadius = shipStatusInfo.shipCollisionRadius;
        speed = Torpedo.LaunchSpeed;
        explosionRadius = Torpedo.ExplosionRadius;
        asteroidList = SensorsController.asteroidList;

        AsteroidData target;
        // TODO: create prioritization algorithm to calculate next shot
        foreach (AsteroidData data in asteroidList) {
            float collisionTime = timeToCollide(data);
            // target = data
        }

        // TODO: Get target direction vector
        // Currently set to a random direction
        (Vector2 direction, float time) = getTargetVector(null);

        shootTorpedoes(turretControls, direction, time);
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
    public float timeToCollide(AsteroidData data)
    {
        Vector2 position = data.position;
        Vector2 velocity = data.velocity;
        float collisionRadius = data.radius;
        // Loop around various points on the collision circle of the asteroid and ship
        // to check if a collision will occur.

        for (float a = 0; a < 2 * Mathf.Pi; a += 0.1f) {
            // Break down x and y components of the asteroid's position vector,
            // for every rotation around the collision circle
            float asteroidX = position.x + collisionRadius * Mathf.Cos(a);
            float asteroidY = position.y + collisionRadius * Mathf.Sin(a);

            for (float b = 0; b < 2 * Mathf.Pi; b += 0.1f) {
                // Break down x and y components of the ships's position vector (center = 0,0),
                // for every rotation around the collision circle
                float shipX = shipCollisionRadius *  Mathf.Cos(b);
                float shipY = shipCollisionRadius *  Mathf.Sin(b);

                // Calculate the time for each component to collide
                float timeX = (shipX - asteroidX) / (velocity.x - shipVelocity.x);
                float timeY = (shipY - asteroidY) / (velocity.y - shipVelocity.y);


                // If the times are close enough, return the time before collision
                float tolerance = 0.0001f;
                
                if (Mathf.Abs(timeX - timeY) <= tolerance) {
                    return timeX;
                }
            }
        } 
        // If no points in the circle will ever intersect with each other, return positive infinity
        return float.PositiveInfinity;
    }

    /*
        Takes the relative position of the asteroid and the absolute
        velocity to calculate the next target vector for the turret,
        relative to the turret, and the time it takes before it hits.
    */
    public (Vector2, float) getTargetVector(AsteroidData? data)
    {
        Random random = new System.Random();
        Vector2 direction = new Vector2((float) (random.NextDouble() - 0.5), (float) (random.NextDouble() - 0.5));
        float time = (float) random.NextDouble() * 10;
        return (direction, time);
    }
    
    /*
        Fires a single torpedo towards the given relative direction with the given time.
    */
    public void shootTorpedoes(TurretControls turretControls, Vector2 relativeDirection, float fuseTime) 
    {
        turretControls.aimTo = shipCoordinates + relativeDirection;  // Aiming in random directions
        turretControls.TriggerTube(readyTube(turretControls), fuseTime); // Change time to collide
        
    }
    
    /*
        Returns the index of the first tube that is ready to fire.
    */
    public int readyTube(TurretControls turretControls) 
    {
        
        // Loops through tubes to check which one is ready
        for(int i = 0; i < 4; i++)
        {
            // TurretControls tc = new TurretControls();
            float tubeCooldown = turretControls.GetTubeCooldown(i);
            // GD.Print(tubeCooldown); 
            if(tubeCooldown == 0)
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
