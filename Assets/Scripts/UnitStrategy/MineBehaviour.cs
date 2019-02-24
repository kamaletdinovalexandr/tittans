using GameEntitties;

namespace Strategy {
	public class MineBehaviour : BaseCollideBehaviour {

		public MineBehaviour(Unit unit) : base(unit) { }

		public override void DoCollide(Unit unit) {
			if (unit.UnitPower == Power.mine)
				return;

			_unit.Speed = 5f;
			_unit.NearEnemyTransform = unit.transform;
		}
	}
}