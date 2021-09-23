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
		asteroidRawData = activeSensors.PerformScan(0, 360, 500);

		// Calculate the position of the asteroid relative to the spaceship


		foreach(EMSReading a in asteroidRawData) {
			if (a.ScanSignature != "Rock:90|Common:10")
				continue;

			float x = (float)Math.Cos(a.Angle) * a.Amplitude * activeSensors.GConstant;
			float y = (float)Math.Sin(a.Angle) * a.Amplitude * activeSensors.GConstant;

			float dist = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
			
			AsteroidData newAsteroid = new AsteroidData(new Vector2(x, y), a.Velocity, a.Radius, dist);

			bool flag = false;
			for (int i = 0; i < asteroidList.Count; i++) {
				AsteroidData ass = asteroidList[i];
				if (dist <= ass.distance) {
					asteroidList.Insert(i, newAsteroid);
					flag = true;
				}
			}

			if (!flag) {
				asteroidList.Add(newAsteroid);
			}
		}
		string printing = "";
		for (int i = 0; i < asteroidList.Count; i++) {
			AsteroidData ass = asteroidList[i];
			printing += (ass.distance.ToString() + " ");
		}
		GD.Print(printing);
		
	}

	public override void DebugDraw(Font font)
	{
		//Student code goes here
	}
}

public struct AsteroidData {
	public Vector2 position;
	public Vector2 velocity;
	public float radius;
	public float distance;

	public AsteroidData(Vector2 p, Vector2 v, float r, float d) {
		position = p;
		velocity = v;
		radius = r;
		distance = d;
	}
}
