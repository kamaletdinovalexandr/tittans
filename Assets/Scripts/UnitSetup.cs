using UnityEngine;

namespace FlyWeight {

	[System.Serializable]
	public class UnitSetup {
		public Power Power;
		public Sprite Sprite;
		public int Cost;
		public float DefaultSpeed;
		public float Scale;
        public float TriggerColliderRadius;
        public int StartLives;
	}
}
