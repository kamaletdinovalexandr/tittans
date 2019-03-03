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
            Speed = _unitSetup.DefaultSpeed;
			InitTriggerCollider();
		}

		public void InitTriggerCollider() {
			var triggerColliders = GetComponents<CircleCollider2D>();
			if (triggerColliders == null)
				return;

			foreach (var coll in triggerColliders) {
				if (coll.isTrigger) {
					coll.radius = _unitSetup.TriggerColliderRadius;
					return;
				}
			}
		}

		#region Monobehaviour
		void Update() {
			switch (UnitSetup.Power) {
				case Power.mine:
				DetectEnemyAndAttack();
				break;

				case Power.paper:
				MoveBackwardIfScissors();
				break;

				case Power.rock:
				AttackScissors();
				break;

				case Power.tower:
				SlowDawnNearestEnemy();
				break;

				default:
				AttackEnemyBase();
				break;
			}
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
		#endregion

		#region UnitBehaviours
		private void DetectEnemyAndAttack() {
			var enemies = NearEnemies.Where(u => u != null && (u.UnitSetup.Power != Power.tower || u.UnitSetup.Power != Power.mine));
			if (!enemies.Any()) {
				Speed = _unitSetup.DefaultSpeed;
				return;
			}

			var enemy = enemies.First();
			Speed = 2 * enemy.Speed;
			transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, Speed * Time.deltaTime);
		}

		private void MoveBackwardIfScissors() {
			var scissors = NearEnemies.Where(u => u != null && (u.UnitSetup.Power == Power.scissors));
			var target = Team.EnemyBasePosition;
			if (scissors.Any())
				target = target * -1;

			transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
		}

		private void AttackScissors() {
			var scissors = NearEnemies.Where(u => u != null && (u.UnitSetup.Power == Power.scissors));
			var target = Team.EnemyBasePosition;
			if (scissors.Any())
				target = scissors.First().transform.position;

			transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
		}

		private void SlowDawnNearestEnemy() {
			var enemies = NearEnemies.Where(u => u != null && (u.UnitSetup.Power != Power.tower || u.UnitSetup.Power != Power.mine));
			if (!enemies.Any()) {
				return;
			}

			enemies.ToList().ForEach(u => u.Speed = 0.5f);
		}

		private void AttackEnemyBase() {
			transform.position = Vector2.MoveTowards(transform.position, Team.EnemyBasePosition, Speed * Time.deltaTime);
		}
		#endregion

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
	}
}
