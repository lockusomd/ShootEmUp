using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collision2D> OnCollisionEntered;

        [NonSerialized] public bool IsPlayer;
        [NonSerialized] public int Damage;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.OnCollisionEntered?.Invoke(this, collision);
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.GetComponent<Rigidbody2D>().velocity = velocity; // Убрал поле - стал вызывать компонент из объекта
        }

        public void SetPhysicsLayer(int physicsLayer)
        {
            this.gameObject.layer = physicsLayer;
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }

        public void SetColor(Color color)
        {
            this._spriteRenderer.color = color;
        }
    }
}