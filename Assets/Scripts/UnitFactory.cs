using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;
using GameEntitties;

namespace Factory {
	public class UnitFactory : MonoBehaviour {

		private static UnitFactory _instance;

		public static UnitFactory Instance { get { return _instance; } }

		#region EditorSetups

		[SerializeField] private Unit _rockPrefab;
		[SerializeField] private Unit _scissorsPrefab;
		[SerializeField] private Unit _paperPrefab;
		[SerializeField] private Unit _titanPrefab;
		[SerializeField] private Unit _towerPrefab;
		[SerializeField] private Unit _minePrefab;

		#endregion

		private void Start() {
			_instance = this;
		}

		public void SpawnUnit(Power power, TeamColor teamColor, Vector2 spawnPosition, Vector2 targetPosition) {
			Unit prefab = _rockPrefab;
			switch (power) {
				case Power.scissors:
				prefab = _scissorsPrefab;
				break;
				case Power.paper:
				prefab = _paperPrefab;
				break;
				case Power.titan:
				prefab = _titanPrefab;
				break;
				case Power.tower:
				prefab = _towerPrefab;
				break;
				case Power.mine:
				prefab = _minePrefab;
				break;
			}

			var unit = Instantiate(prefab, spawnPosition, Quaternion.identity);
			unit.UnitPower = power;
			unit.Team = teamColor;
			unit.TargetPosition = targetPosition;
			SetUnitBehaviour(unit);
		}

		public void SetUnitBehaviour(Unit unit) {
			switch (unit.UnitPower) {
				case Power.mine: 
					unit.CollideBehaviour = new MineBehaviour(unit);
					break;
				case Power.titan:
				unit.CollideBehaviour = new BaseCollideBehaviour(unit);
					break;
				case Power.tower:
					unit.CollideBehaviour = new TowerBehaviour(unit);
					break;
				case Power.rock:
					unit.CollideBehaviour = new RockBehaviour(unit);
					break;
				case Power.paper:
					unit.CollideBehaviour = new PaperBehaviour(unit);
					break;
				case Power.scissors:
					unit.CollideBehaviour = new ScissorsBehaviour(unit);
					break;
				default:
					unit.CollideBehaviour = new BaseCollideBehaviour(unit);
				break;
			}
		}
	}
}
