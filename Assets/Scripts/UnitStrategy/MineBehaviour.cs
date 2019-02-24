using GameEntitties;
using UnityEngine;
using System.Linq;

namespace Strategy {
	public class MineBehaviour : BaseUnitBehaviour {

		public MineBehaviour(Unit unit) : base(unit) { }

		public override void Behave() {
			if (_unit.NearEnemies.Any()) {
				_unit.transform.position = Vector2.MoveTowards(_unit.transform.position, 
				_unit.NearEnemies.First().transform.position,
				_unit.DefaultSpeed * Time.deltaTime);
			}
		}
	}
}