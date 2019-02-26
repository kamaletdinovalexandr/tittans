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
		[SerializeField] private Sprite _rockSprite;
		[SerializeField] private Sprite _scissorsSprite;
		[SerializeField] private Sprite _paperSprite;
		[SerializeField] private Sprite _titanSprite;
		[SerializeField] private Sprite _towerSprite;
		[SerializeField] private Sprite _mineSprite;

		[SerializeField] private Dictionary<Power, UnitFlyweight> Flyweights = new Dictionary<Power, UnitFlyweight>();

		#endregion

		private List<UnitFlyweight> UnitFlyweights = new List<UnitFlyweight>();

		private void Start() {
			_instance = this;
		}

		public void SpawnUnit(Power power, Team team, Vector2 spawnPosition) {
			UnitContext unit = Instantiate(_prefab, spawnPosition, Quaternion.identity);

			switch (power) {
				case Power.scissors:
					unit.SpriteRenderer.sprite = _scissorsSprite;

				break;
				case Power.paper:
					unit.SpriteRenderer.sprite = _paperSprite;
				break;
				case Power.titan:
					unit.SpriteRenderer.sprite = _titanSprite;
				break;
				case Power.tower:
					unit.SpriteRenderer.sprite = _titanSprite;
				break;
				case Power.mine:
					unit.SpriteRenderer.sprite = _mineSprite;
				break;
			}


			unit.Team = team;
			SetUnitBehaviour(unit);
		}

		public UnitFlyweight GetFlytWeight(Power power, int cost, Color color, float scale) {
			if (Flyweights.ContainsKey(power))
				return Flyweights[power];
			else {
				var newFlyWeight = new UnitFlyweight(power, cost, color, scale);
				Flyweights.Add(power, newFlyWeight);
				return newFlyWeight;
			}
		}

		public void SetUnitBehaviour(UnitContext unit) {
			switch (unit.UnitFlyweight.Power) {
				case Power.mine: 
					unit.UnitBehaviour = new MineBehaviour(unit);
					break;
				case Power.titan:
				unit.UnitBehaviour = new BaseUnitBehaviour(unit);
					break;
				case Power.tower:
					unit.UnitBehaviour = new TowerBehaviour(unit);
					break;
				case Power.rock:
					unit.UnitBehaviour = new RockBehaviour(unit);
					break;
				case Power.paper:
					unit.UnitBehaviour = new PaperBehaviour(unit);
					break;
				default:
					unit.UnitBehaviour = new BaseUnitBehaviour(unit);
					break;
			}
		}
	}
}
