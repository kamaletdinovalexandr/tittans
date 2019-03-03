using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;

namespace GameEntitties {
	public class Unit : MonoBehaviour {

		public Team Team { get; set; }public int Lives;
		public List<Unit> NearEnemies = new List<Unit>();

        [SerializeField] private SpriteRenderer _unitColor;
		[SerializeField] private SpriteRenderer _icon;

		public float Speed { get; private set; }

		private UnitSetup _unitSetup;

		public UnitSetup UnitSetup {
			
			get { return _unitSetup; }

			set {
                _unitSetup = value;
				Init();
            }
        }

        private void Init() {
            _icon.sprite = _unitSetup.Sprite;
			_unitColor.color = Team.TeamColor == TeamColor.blue ? Color.blue : Color.red;
            transform.localScale = new Vector2(_unitSetup.Scale, _unitSetup.Scale);
            SetTriggerCollider(_unitSetup.TriggerColliderRadius);
			Speed = _unitSetup.DefaultSpeed;
        }

        void Update() {
			Vector2 target = Team.EnemyBasePosition;
			var scissors = NearEnemies.Where(u => u != null && u.UnitSetup.Power == Power.scissors);

			switch (UnitSetup.Power) {
				case Power.mine:
				if (NearEnemies.Any()) {
					var enemy = NearEnemies.FirstOrDefault(u => u != null && u.UnitSetup.Power != Power.tower);
					if (enemy == null) { 
						Speed = _unitSetup.DefaultSpeed;
						return;
					}

					Speed = 2 * enemy.Speed;
					target = enemy.transform.position;
				}
				break;

				case Power.paper:
				target = scissors.Any() ? target * -1 : target;
				break;

				case Power.rock:
				if (scissors.Any()) {
					//var scissor = scissors.FirstOrDefault(u => !(u == null));
					//if (scissor != null)
					target = scissors.First().transform.position;
				}
				break;

				case Power.tower:
				NearEnemies.ForEach(x => x.Speed = 0.5f);
				Speed = _unitSetup.DefaultSpeed;
				break;
			}
			transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
		}

		private void OnCollisionEnter2D(Collision2D other) {
			var otherUnit = other.gameObject.GetComponent<Unit>();
			if (otherUnit == null || Team == otherUnit.Team)
				return;

			if (IsPowerfullThen(otherUnit.UnitSetup.Power)) {
				Debug.Log(UnitSetup.Power + " is damage unit " + otherUnit.UnitSetup.Power);
				otherUnit.TakeDamage();
				if (otherUnit.Lives <= 0)
					RemoveNearEnemy(otherUnit);
			}

			if (UnitSetup.Power == Power.mine || UnitSetup.Power == Power.tower)
				TakeDamage();
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit == null || Team == unit.Team)
				return;

			Debug.Log(UnitSetup.Power + ": enemy unit is entered trigger area: " + unit.UnitSetup.Power);
			if (!NearEnemies.Contains(unit))
				NearEnemies.Add(unit);
		}

		private void OnTriggerExit2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<Unit>();
			RemoveNearEnemy(unit);
		}

		private void RemoveNearEnemy(Unit unit) {
			if (NearEnemies.Contains(unit)) {
				NearEnemies.Remove(unit);
			}
		}

		private bool IsPowerfullThen(Power otherPower) {
			return UnitSetup.Power == Power.mine && otherPower != Power.mine
                || UnitSetup.Power == Power.titan && otherPower != Power.titan
                || UnitSetup.Power == Power.rock && otherPower == Power.scissors
                || UnitSetup.Power == Power.scissors && otherPower == Power.paper
                || UnitSetup.Power == Power.paper && otherPower == Power.rock
                || UnitSetup.Power == Power.tower;
		}

		public void TakeDamage() {
			Lives--;
			Debug.Log(UnitSetup.Power + " is damaged: Lives:" + Lives);
			if (Lives <= 0) {
				Debug.Log(UnitSetup.Power + " no lives left");
				Destroy(gameObject);
			}
		}

        public void SetTriggerCollider(float radius) {
            var triggerColliders = GetComponents<CircleCollider2D>();
            if (triggerColliders == null)
                return;
            
            foreach (var coll in triggerColliders) {
                if (coll.isTrigger) {
                    coll.radius = radius;
                    return;
                }
            }
        }
	}
}
