using UnityEngine;

namespace FlyWeight {

	[System.Serializable]
	public class UnitFlyweight {
		public Power Power;
		public Sprite Sprite;
		public Color Color;
		public int Cost;
		public float Speed;
		public float Scale;
        public float TriggerColliderRadius;
        public int StartLives;
	}
}
