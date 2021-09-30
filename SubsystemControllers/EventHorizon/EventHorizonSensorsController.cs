using Godot;
using System;
using System.Collections.Generic;


public class EventHorizonSensorsController : AbstractSensorsController
{
	EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}    
	EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
	EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

	Vector2 shipVelocity;
	Vector2 shipPosition;

	float scanAngle;
	float scanDistance;

	public List<EMSReading> asteroidRawData = new List<EMSReading>(); 
	public List<AsteroidData> asteroidList = new List<AsteroidData>();

	public override void SensorsUpdate(ShipStatusInfo shipStatusInfo, IActiveSensors activeSensors, PassiveSensors passiveSensors, float deltaTime)
	{
		asteroidList.Clear();
		//Student code goes here   

		shipVelocity = shipStatusInfo.linearVelocity;
		shipPosition = shipStatusInfo.positionWithinSystem;

		float angle = (float) Math.Atan2(shipVelocity.y, shipVelocity.x);
		
		scanAngle = Mathf.Clamp(Mathf.Pi/(shipVelocity.Length()/100.0f), Mathf.Pi/4, Mathf.Pi); // Scales the scan angle based on the ship's velocity.
		scanDistance = Mathf.Clamp(shipVelocity.Length()+50, 50, 300); // Scales the scan distance based off of the ship's velocity. Clamps between (100, 300)

		asteroidRawData = activeSensors.PerformScan(
			angle, 
			scanAngle, 
			scanDistance
		);
		// Calculate the position of the asteroid relative to the spaceship


		foreach(EMSReading a in asteroidRawData) {
			if (a.ScanSignature != "Rock:90|Common:10")
				continue;
			
			// Calculate the position of the asteroid in cartesian coordinates
			float x = (float)Math.Cos(a.Angle) * a.Amplitude * activeSensors.GConstant;
			float y = (float)Math.Sin(a.Angle) * a.Amplitude * activeSensors.GConstant;

			// Calculate the distance of the asteroid relative to the spaceship
			float dist = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
			
			AsteroidData newAsteroid = new AsteroidData(a.ContactID, new Vector2(x, y), a.Velocity, a.Radius, dist);

			// Sort the asteroids by distance using insertion sort
			bool flag = false;
			for (int i = 0; i < asteroidList.Count; i++) {
				AsteroidData ass = asteroidList[i];
				if (dist <= ass.distance) {
					asteroidList.Insert(i, newAsteroid);
					flag = true;
					break;
				}
			}

			if (!flag) {
				asteroidList.Add(newAsteroid);
			}
		}
	}

	public override void DebugDraw(Font font)
	{
		Vector2 shipDirection = shipVelocity.Normalized();
		
		// Draw debug lines for the ship's scan distance and angle
		DrawLine(shipPosition, shipPosition + (scanDistance * shipDirection), Color.ColorN("green"), 5f);
		DrawLine(shipPosition, shipPosition + scanDistance * rotateVector(shipDirection, -scanAngle/2.0f), Color.ColorN("green"), 5f);
		DrawLine(shipPosition, shipPosition + scanDistance * rotateVector(shipDirection, scanAngle/2.0f), Color.ColorN("green"), 5f);

	}

	//Rotate a vector by a given angle
	Vector2 rotateVector(Vector2 initial, float angle) {
		Vector2 result;
		
		result.x = initial.x * Mathf.Cos(angle) - initial.y * Mathf.Sin(angle);
		result.y = initial.x * Mathf.Sin(angle) + initial.y * Mathf.Cos(angle);

		return result;
	}
};


public struct AsteroidData {
	public ulong id;
	public Vector2 position;
	public Vector2 velocity;
	public float radius;
	public float distance;

	public AsteroidData(ulong id, Vector2 p, Vector2 v, float r, float d) {
		this.id = id;
		position = p;
		velocity = v;
		radius = r;
		distance = d;
	}
}
