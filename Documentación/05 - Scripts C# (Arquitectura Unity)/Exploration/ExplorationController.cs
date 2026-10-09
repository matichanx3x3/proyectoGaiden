using UnityEngine;
using Gaiden.Core;
using Gaiden.Interfaces;

namespace Gaiden.Exploration {
    /// <summary>
    /// Control top-down del superviviente activo, soporte de sprint y apertura de inventario.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class ExplorationController : MonoBehaviour {
        [SerializeField] private float _walkSpeed = 2.5f;
        [SerializeField] private float _sprintSpeed = 4.5f;
        [SerializeField] private LayerMask _interactLayers;

        private Rigidbody2D _rb;
        private Vector2 _input;
        private Vector2 _facingDir = Vector2.down;
        private bool _sprinting;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            if (GaidenGameManager.Instance.CurrentState != GaidenGameState.Exploration) {
                _input = Vector2.zero;
                return;
            }

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            _input = new Vector2(h, v).normalized;

            if (_input != Vector2.zero) {
                if (Mathf.Abs(h) > Mathf.Abs(v)) _facingDir = new Vector2(Mathf.Sign(h), 0);
                else _facingDir = new Vector2(0, Mathf.Sign(v));

                _sprinting = Input.GetButton("Fire2") || Input.GetKey(KeyCode.LeftShift);
            } else {
                _sprinting = false;

                if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.I)) {
                    GaidenGameManager.Instance.ChangeState(GaidenGameState.Inventory);
                    return;
                }
            }

            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E)) {
                CheckInteraction();
            }
        }

        private void FixedUpdate() {
            float speed = _sprinting ? _sprintSpeed : _walkSpeed;
            _rb.linearVelocity = _input * speed;
        }

        private void CheckInteraction() {
            Vector2 checkPos = (Vector2)transform.position + (_facingDir * 0.8f);
            var hit = Physics2D.OverlapCircle(checkPos, 0.3f, _interactLayers);
            if (hit != null && hit.TryGetComponent<IInteractable>(out var interactable)) {
                interactable.Interact(this);
            }
        }
    }
}
