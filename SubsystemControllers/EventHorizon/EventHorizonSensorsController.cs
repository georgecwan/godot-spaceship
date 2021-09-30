using Godot;
using System;
using System.Collections.Generic;


public class EventHorizonSensorsController : AbstractSensorsController
{
	EventHorizonNavigationController NavigationController {get{return parentShip.NavigationController as EventHorizonNavigationController;}}    
	EventHorizonPropulsionController PropulsionController {get{return parentShip.PropulsionController as EventHorizonPropulsionController;}}    
	EventHorizonDefenceController DefenceController {get{return parentShip.DefenceController as EventHorizonDefenceController;}}

	public List<EMSReading> asteroidRawData = new List<EMSReading>(); 
	public List<AsteroidData> asteroidList = new List<AsteroidData>();

	public override void SensorsUpdate(ShipStatusInfo shipStatusInfo, IActiveSensors activeSensors, PassiveSensors passiveSensors, float deltaTime)
	{
		asteroidList.Clear();
		//Student code goes here   

		Vector2 shipVelocity = shipStatusInfo.linearVelocity;
		float angle = (float) Math.Atan2(shipVelocity.y, shipVelocity.x);
		
		asteroidRawData = activeSensors.PerformScan(
			angle, 
			Mathf.Clamp(Mathf.Pi/(shipVelocity.Length()/100.0f), 0, Mathf.Pi), 
			Mathf.Clamp(shipVelocity.Length()*2, 0, 300)
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
		//Student code goes here
	}
}

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
