using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEntitties;
using FlyWeight;

namespace Factory {
	public class UnitFactory : MonoBehaviour {

		private static UnitFactory _instance;

		public static UnitFactory Instance { get { return _instance; } }

#region EditorSetups
		[SerializeField] private Unit _prefab;
		[SerializeField] private List<UnitSetup> UnitSetups = new List<UnitSetup>();
#endregion

		private void Start() {
			_instance = this;
		}

		public void CreateUnit(Power power, Team team, Vector2 spawnPosition) {
			Unit unit = Instantiate(_prefab, spawnPosition, Quaternion.identity);
			var unitSetup = GetSetupForUnit(power);
			if (unitSetup == null) {
				throw new System.Exception("Try spawn unit of unknown type!!!");
			}

			unit.Init(team, unitSetup);
		}

		public UnitSetup GetSetupForUnit(Power power) {
			foreach (var flyweight in UnitSetups) {
				if (flyweight.Power == power) {
					return flyweight;
				}
			}

			return null;
		}

		public int GetUnitCost(Power power) {
			var setup = GetSetupForUnit(power);
			return setup == null ? Globals.UNBUYEBLE_COST : setup.Cost;
		}
	}
}
