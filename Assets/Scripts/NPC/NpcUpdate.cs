using UnityEngine;
using GameEntitties;
using GameCore;
using Factory;

namespace NPCInput {
	public class NpcUpdate : IUpdate {
		private float _npcDelay = 3f;
		private Power _currentRedUnit;
		private Team _team;

		public NpcUpdate(Team team) {
			_team = team;
		}

		public void MakeAction() {
			NpcSpawn();
		}

		private void NpcSpawn() {
			if (_npcDelay > 0f) {
				_npcDelay -= Time.deltaTime;
				return;
			}

			_npcDelay = Random.Range(2, 4);

			if (_currentRedUnit == Power.none)
				_currentRedUnit = GetRandomPower();

			if (!_team.IsEnergyAvailable(_currentRedUnit))
				return;

			_team.Energy -= UnitFactory.Instance.GetUnitCost(_currentRedUnit);
			var position = GetRandomPosition(_team.AreaPosition, _team.HalfScale);
			UnitFactory.Instance.CreateUnit(_currentRedUnit, _team, position);
			_currentRedUnit = Power.none;
		}

		private Power GetRandomPower() {
			return (Power)Random.Range(1, System.Enum.GetValues(typeof(Power)).Length);
		}

		private Vector2 GetRandomPosition(Vector2 areaPosition, Vector2 halfScale) {
			var randomX = Random.Range(areaPosition.x - halfScale.x, areaPosition.x + halfScale.x);
			var randomY = Random.Range(areaPosition.y - halfScale.y, areaPosition.y + halfScale.y);
			return new Vector2(randomX, randomY);
		}
	}
}
