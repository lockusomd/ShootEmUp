using UnityEngine;

namespace ShootEmUp
{
    public sealed class EnemyMoveAgent : MonoBehaviour
    {
        [SerializeField] private MoveComponent _moveComponent;

        private Vector2 _destination;
        public bool IsReached { get; private set; } // Оставил только свойство IsReached

        public void SetDestination(Vector2 endPoint)
        {
            this._destination = endPoint;
            this.IsReached = false;
        }

        private void FixedUpdate()
        {
            if (this.IsReached)
            {
                return;
            }
            
            var vector = this._destination - (Vector2) this.transform.position;
            if (vector.magnitude <= 0.25f)
            {
                this.IsReached = true;
                return;
            }

            var direction = vector.normalized * Time.fixedDeltaTime;
            this._moveComponent.MoveByRigidbodyVelocity(direction);
        }
    }
}