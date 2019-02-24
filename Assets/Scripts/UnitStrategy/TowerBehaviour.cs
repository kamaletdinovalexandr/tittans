using GameEntitties;
using UnityEngine;

namespace Strategy {
	public class TowerBehaviour : BaseCollideBehaviour {

		public TowerBehaviour(Unit unit) : base(unit) { }

		public override void DoCollide(Unit unit) {
			if (unit.UnitPower == Power.tower)
				return;

			Debug.Log("Tower: Setting low speed to unit with power: " + unit.UnitPower);
			unit.AlternateSpeed = 0.2f;
		}
	}
}