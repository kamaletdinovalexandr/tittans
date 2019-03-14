using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;

namespace GameEntitties {
	public class Unit : MonoBehaviour {

        public Team Team;
        public Power Power;
        public int Lives;
        public float Speed;
        public float CurrentSpeed;
        public bool IsAlive = true;

		public List<Unit> NearEnemies = new List<Unit>();

        [SerializeField] private SpriteRenderer _unitColor;
		[SerializeField] private SpriteRenderer _icon;
		
        public void Init(Team team, UnitSetup setup) {
            Team = team;
            Speed = setup.DefaultSpeed;
            CurrentSpeed = Speed;
            Lives = setup.StartLives;
            InitGameObject(setup);
        }

        private void InitGameObject(UnitSetup setup) {
            _icon.sprite = setup.Sprite;
            _unitColor.color = GetCurrentTeamColor();
            transform.localScale = new Vector2(setup.Scale, setup.Scale);
            SetTriggerRadius(setup.TriggerColliderRadius);
        }

        private Color GetCurrentTeamColor() {
            return Team.TeamColor == TeamColor.blue ? Color.blue : Color.red;
        }

        public void SetTriggerRadius(float radius) {
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

		#region Monobehaviour
		void Update() {
            if (!IsAlive) {
                Destroy(gameObject);
                return;
            }

			switch (Power) {
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
				SlowDownNearestEnemy();
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

            TryGiveDamage(otherUnit);
        }

        private void TryGiveDamage(Unit enemy) {
            if (CanGiveDamage(enemy.Power)) {
                enemy.TakeDamage();
                if (enemy.Lives <= 0)
                    RemoveNearEnemy(enemy);
            }

            if (Power == Power.mine || Power == Power.tower)
                TakeDamage();
        }

        private void OnTriggerEnter2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit == null || Team == unit.Team)
				return;

			Debug.Log(Power + ": enemy unit is entered trigger area: " + unit.Power);
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
			var enemies = NearEnemies.Where(u => u != null && (u.Power != Power.tower || u.Power != Power.mine));
			if (!enemies.Any()) {
				CurrentSpeed = Speed;
				return;
			}

			var enemy = enemies.First();
            CurrentSpeed = 2 * enemy.CurrentSpeed;
			transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, Speed * Time.deltaTime);
		}

		private void MoveBackwardIfScissors() {
			var scissors = NearEnemies.Where(u => u != null && (u.Power == Power.scissors));
			var target = Team.EnemyBasePosition;
			if (scissors.Any())
				target = target * -1;

            transform.position = Vector2.MoveTowards(transform.position, target, CurrentSpeed * Time.deltaTime);
		}

		private void AttackScissors() {
			var scissors = NearEnemies.Where(u => u != null && (u.Power == Power.scissors));
			var target = Team.EnemyBasePosition;
			if (scissors.Any())
				target = scissors.First().transform.position;

            transform.position = Vector2.MoveTowards(transform.position, target, CurrentSpeed * Time.deltaTime);
		}

        private void SlowDownNearestEnemy() {
			var enemies = NearEnemies.Where(u => u != null && (u.Power != Power.tower || u.Power != Power.mine));
			if (!enemies.Any()) {
				return;
			}

            enemies.ToList().ForEach(u => u.CurrentSpeed = 0.5f);
		}

		private void AttackEnemyBase() {
            transform.position = Vector2.MoveTowards(transform.position, Team.EnemyBasePosition, CurrentSpeed * Time.deltaTime);
		}
		#endregion

		private bool CanGiveDamage(Power otherPower) {
			return Power == Power.mine && otherPower != Power.mine
                || Power == Power.titan && otherPower != Power.titan
                || Power == Power.rock && otherPower == Power.scissors
                || Power == Power.scissors && otherPower == Power.paper
                || Power == Power.paper && otherPower == Power.rock
                || Power == Power.tower;
		}

		public void TakeDamage() {
			Lives--;
            Debug.Log(Power + " is damaged");
			if (Lives <= 0) {
				Debug.Log(Power + " no lives left");
                IsAlive = false;
				Destroy(gameObject);
			}
		}
	}
}
