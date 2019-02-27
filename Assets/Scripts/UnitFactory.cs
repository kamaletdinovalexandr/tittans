using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;
using GameEntitties;
using FlyWeight;

namespace Factory {
	public class UnitFactory : MonoBehaviour {

		private static UnitFactory _instance;

		public static UnitFactory Instance { get { return _instance; } }

		#region EditorSetups

		[SerializeField] private UnitContext _prefab;
		[SerializeField] private List<UnitFlyweight> Flyweights = new List<UnitFlyweight>();

		#endregion

		private void Start() {
			_instance = this;
		}

		public void CreateUnit(Power power, Team team, Vector2 spawnPosition) {
			UnitContext unit = Instantiate(_prefab, spawnPosition, Quaternion.identity);
			var flyweigh = GetFlytWeight(power);
			if (flyweigh == null) {
				throw new System.Exception("Try spawn unit of unknown type!!!");
			}

			unit.Team = team;
			unit.UnitFlyweight = flyweigh;
			unit.UnitBehaviour = GetUnitBehaviour(unit);
		}

		public UnitFlyweight GetFlytWeight(Power power) {
			foreach (var flyweight in Flyweights) {
				if (flyweight.Power == power) {
					return flyweight;
				}
			}

			return null;
		}

		public IUnitBehaviour GetUnitBehaviour(UnitContext unit) {
			switch (unit.UnitFlyweight.Power) {
				case Power.mine: 
					return new MineBehaviour(unit);

				case Power.titan:
					return new BaseUnitBehaviour(unit);

				case Power.tower:
					return new TowerBehaviour(unit);

				case Power.rock:
					return new RockBehaviour(unit);

				case Power.paper:
					return new PaperBehaviour(unit);

				default:
					return new BaseUnitBehaviour(unit);
			}
		}
	}
}
