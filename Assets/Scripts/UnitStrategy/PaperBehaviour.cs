using GameEntitties;
using UnityEngine;
using System.Linq;

namespace Strategy {
	public class PaperBehaviour : BaseUnitBehaviour {

		public PaperBehaviour(Unit unit) : base(unit) { }

		public override void Behave() {
			Vector2 target = _unit.Team.EnemyBasePosition;
			var scissors = _unit.NearEnemies.Where(u =>  u.UnitPower == Power.scissors);

			if (scissors.Any())
				target = target * -1;

			_unit.transform.position = Vector2.MoveTowards(_unit.transform.position, target, _unit.DefaultSpeed * Time.deltaTime);
		}
	}
}
