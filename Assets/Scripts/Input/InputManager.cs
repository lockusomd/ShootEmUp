using UnityEngine;

namespace ShootEmUp
{
    public sealed class InputManager : MonoBehaviour
    {
        [SerializeField] private GameObject _character;

        public float HorizontalDirection { get; private set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _character.GetComponent<CharacterController>()._fireRequired = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.HorizontalDirection = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                this.HorizontalDirection = 1;
            }
            else
            {
                this.HorizontalDirection = 0;
            }
        }
        
        private void FixedUpdate()
        {
            this._character.GetComponent<MoveComponent>().MoveByRigidbodyVelocity(new Vector2(this.HorizontalDirection, 0) * Time.fixedDeltaTime);
        }
    }
}