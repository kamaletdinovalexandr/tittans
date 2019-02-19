using GameEntitties;

namespace Strategy {
	public class ScissorsBehaviour : AbstractCollideBehaviour {

		public ScissorsBehaviour(Unit unit) : base(unit) { }

		public override void DoCollide(Unit unit) {
			if (unit.UnitPower == Power.paper) {
				_unit.NearEnemyTransform = unit.transform;
				_unit.RunAway = false;
				return;
			}

			if (unit.UnitPower == Power.rock)
				_unit.NearEnemyTransform = unit.transform;
			_unit.RunAway = true;
		}
	}
}