using UnityEngine;

namespace FlyWeight {
	
	public class UnitFlyweight {
		public Power Power;
		public int Cost;
		public Color Color;
		public float Speed;
		public float Scale;

		public UnitFlyweight (Power power, int cost, Color color, float speed, float scale) {
			Power = power;
			Cost = cost;
			Color = color;
			Speed = speed;
			Scale = scale;
		}
	}
}
