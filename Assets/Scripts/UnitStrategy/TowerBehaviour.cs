using GameEntitties;
using UnityEngine;
using System.Linq;

namespace Strategy {
	public class TowerBehaviour : BaseUnitBehaviour {

		public TowerBehaviour(UnitContext unit) : base(unit) { }

		public override void Behave() {
			if (_unit.NearEnemies.Any())
				_unit.NearEnemies.ForEach(x => x.Speed = 0.5f);
		}
	}
}