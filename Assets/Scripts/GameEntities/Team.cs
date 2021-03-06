using UnityEngine;
using Factory;

namespace GameEntitties {
	public class Team {
		public TeamColor TeamColor { get; private set; }
		public float Energy { get; set; }
		public Vector2 EnemyBasePosition { get; private set; }
		public Vector2 AreaPosition { get; private set; }
		public Vector2 HalfScale { get; private set; }
		public IUpdate ActionBehaviour { get; set; }

		public Team(TeamColor teamColor, Vector2 enemyBasePosition, Vector2 areaPosition, Vector2 halfScale) {
			TeamColor = teamColor;
			EnemyBasePosition = enemyBasePosition;
			AreaPosition = areaPosition;
			HalfScale = halfScale;
			Energy = Globals.MAX_ENERGY;
		}

		public void UpdateEnergy() {
			Energy += Time.deltaTime;

			if (Energy > Globals.MAX_ENERGY)
				Energy = Globals.MAX_ENERGY;
		}

		public bool IsInsideArea(Vector2 position) {
			return position.x >= AreaPosition.x - HalfScale.x
				   && position.x <= AreaPosition.x + HalfScale.x
				   && position.y >= AreaPosition.y - HalfScale.y
				   && position.y <= AreaPosition.y + HalfScale.y;
		}

		public bool IsEnergyAvailable(Power power) {
			return Energy - UnitFactory.Instance.GetUnitCost(power) >= 0;
		}
	}
}
