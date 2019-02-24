using GameEntitties;

namespace Strategy {
	public class BaseCollideBehaviour : ICollideBehaviour {
		protected readonly Unit _unit;

		public BaseCollideBehaviour(Unit unit) {
			_unit = unit;
		}

		public virtual void DoCollide(Unit unit) { }
	}
}