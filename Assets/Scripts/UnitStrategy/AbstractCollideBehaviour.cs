using GameEntitties;

namespace Strategy {
	public class AbstractCollideBehaviour : ICollideBehaviour {
		protected readonly Unit _unit;

		public AbstractCollideBehaviour(Unit unit) {
			_unit = unit;
		}

		public virtual void DoCollide(Unit unit) { }
	}
}