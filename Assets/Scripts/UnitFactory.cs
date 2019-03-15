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
		[SerializeField] private GameObject _prefab;
		[SerializeField] private List<UnitSetup> UnitSetups = new List<UnitSetup>();
#endregion

		private void Start() {
			_instance = this;
		}

		public void CreateUnit(Power power, Team team, Vector2 spawnPosition) {
			var go = Instantiate(_prefab, spawnPosition, Quaternion.identity);
			var unitSetup = GetSetupForUnit(power);
			if (unitSetup == null) {
				throw new System.Exception("Try spawn unit of unknown type!!!");
			}
            Unit unit = null;
            switch(power) {
                case Power.mine:
                    unit = go.AddComponent<MineUnit>();
                    break;
                case Power.paper:
                    unit = go.AddComponent<PaperUnit>();
                    break;
                case Power.rock:
                    unit = go.AddComponent<RockUnit>();
                    break;
                case Power.scissors:
                    unit = go.AddComponent<ScissorsUnit>();
                    break;
                case Power.titan:
                    unit = go.AddComponent<TitanUnit>();
                    break;
                case Power.tower:
                    unit = go.AddComponent<TowerUnit>();
                    break;
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
