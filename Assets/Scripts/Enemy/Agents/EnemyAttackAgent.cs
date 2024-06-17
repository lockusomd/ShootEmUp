using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemyAttackAgent : MonoBehaviour
    {
        [SerializeField] private WeaponComponent _weaponComponent;
        [SerializeField] private EnemyMoveAgent _moveAgent;
        [SerializeField] private float _countdown;

        public delegate void FireHandler(GameObject enemy, Vector2 position, Vector2 direction);
        public event FireHandler OnFire;

        private GameObject _target;
        private float _currentTime;

        public void SetTarget(GameObject target)
        {
            this._target = target;
        }

        public void Reset()
        {
            this._currentTime = this._countdown;
        }

        private void FixedUpdate()
        {
            if (!this._moveAgent.IsReached)
            {
                return;
            }
            
            if (!this._target.GetComponent<HitPointsComponent>().IsHitPointsExists())
            {
                return;
            }

            this._currentTime -= Time.fixedDeltaTime;
            if (this._currentTime <= 0)
            {
                this.Fire();
                this._currentTime += this._countdown;
            }
        }

        private void Fire()
        {
            var startPosition = this._weaponComponent.Position;
            var vector = (Vector2) this._target.transform.position - startPosition;
            var direction = vector.normalized;
            this.OnFire?.Invoke(this.gameObject, startPosition, direction);
        }
    }
}