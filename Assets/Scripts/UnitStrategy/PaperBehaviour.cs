using GameEntitties;

namespace Strategy {
	public class PaperBehaviour : BaseCollideBehaviour {

		public PaperBehaviour(Unit unit) : base(unit) { }

		public override void DoCollide(Unit unit) {
			if (unit.UnitPower == Power.scissors) {
				_unit.NearEnemyTransform = unit.transform;
				_unit.RunAway = true;
			}
		}
	}
}
