using GameEntitties;
using UnityEngine;
using System.Linq;

namespace Strategy {
	public class PaperBehaviour : BaseUnitBehaviour {

		public PaperBehaviour(UnitContext unit) : base(unit) { }

		public override void Behave() {
			var scissors = _unit.NearEnemies.Where(u =>  u.UnitFlyweight.Power == Power.scissors);
			Vector2 target = scissors.Any() 
			                         ? _unit.Team.EnemyBasePosition * -1 
			                         : _unit.Team.EnemyBasePosition;

			_unit.transform.position = Vector2.MoveTowards(_unit.transform.position, target, _unit.UnitFlyweight.Speed * Time.deltaTime);
		}
	}
}
