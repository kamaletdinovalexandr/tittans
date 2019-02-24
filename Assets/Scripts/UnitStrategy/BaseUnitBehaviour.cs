using GameEntitties;
using UnityEngine;

namespace Strategy {
	public class BaseUnitBehaviour : IUnitBehaviour {
		protected readonly Unit _unit;

		public BaseUnitBehaviour(Unit unit) {
			_unit = unit;
		}

		public virtual void Behave() {
			_unit.transform.position = Vector2.MoveTowards(_unit.transform.position, 
					_unit.Team.EnemyBasePosition, _unit.DefaultSpeed * Time.deltaTime);
		}
	}
}