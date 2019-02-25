using GameEntitties;
using UnityEngine;
using System.Linq;

namespace Strategy {
	public class TowerBehaviour : BaseUnitBehaviour {

		public TowerBehaviour(Unit unit) : base(unit) { }

		public override void Behave() {
			if (_unit.NearEnemies.Any())
				_unit.NearEnemies.ForEach(x => x.DefaultSpeed = 0.5f);
		}
	}
}