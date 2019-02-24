using GameEntitties;
using UnityEngine;
using System.Linq;

namespace Strategy {
	public class TowerBehaviour : BaseUnitBehaviour {

		public TowerBehaviour(Unit unit) : base(unit) { }

		public override void Behave() {
			var enemies = _unit.NearEnemies.Where(u => u.UnitPower == Power.scissors);

			if (enemies.Any())
				enemies.Select(u => u.DefaultSpeed = 0.5f);
		}
	}
}