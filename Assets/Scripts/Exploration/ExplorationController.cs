using UnityEngine;
using UnityEngine.InputSystem;
using Game.Core;
using Game.Interfaces;

namespace Game.Exploration {
    /// <summary>
    /// Controlador de exploración en perspectiva Top-Down.
    /// Movimiento en el plano 3D (X/Z), rotación suave hacia la dirección de avance,
    /// sprint, interacción 3D, soporte para escaleras/verticalidad y empuje de cajas físicas.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ExplorationController : MonoBehaviour {
        [Header("Velocidad de Movimiento")]
        [SerializeField] private float _walkSpeed = 3.5f;
        [SerializeField] private float _sprintSpeed = 5.8f;
        [SerializeField] private float _rotationSpeed = 12f;

        [Header("Detección de Interacción")]
        [SerializeField] private float _interactionRadius = 1.6f;
        [SerializeField] private float _interactionDistance = 0.8f;
        [SerializeField] private LayerMask _interactLayers = ~0;

        [Header("Física y Verticalidad")]
        [SerializeField] private float _gravity = -15f;
        [SerializeField] private float _stepOffset = 0.45f;
        [SerializeField] private float _slopeLimit = 50f;
        [SerializeField] private float _boxPushPower = 2.5f;

        private CharacterController _characterController;
        private Vector3 _moveDirection;
        private float _verticalVelocity;
        private bool _isSprinting;
        private float _lastInventoryToggleTime;
        private bool _isClimbingLadder;
        private float _ladderClimbSpeed = 3f;

        public Vector3 FacingDirection { get; private set; } = Vector3.forward;
        public bool IsMoving => _moveDirection.sqrMagnitude > 0.01f;
        public bool IsClimbingLadder => _isClimbingLadder;

        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            if (_characterController != null) {
                _characterController.center = Vector3.zero;
                _characterController.height = 2f;
                _characterController.radius = 0.4f;
                _characterController.stepOffset = _stepOffset;
                _characterController.slopeLimit = _slopeLimit;
            }
        }

        private void Update() {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Exploration) {
                _moveDirection = Vector3.zero;
                return;
            }

            HandleInput();
            HandleInteractionInput();
        }

        private void FixedUpdate() {
            ApplyMovement();
        }

        private void HandleInput() {
            Vector2 inputVector = Vector2.zero;

            var keyboard = Keyboard.current;
            if (keyboard != null) {
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) inputVector.y += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) inputVector.y -= 1f;
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) inputVector.x -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) inputVector.x += 1f;
            }

            var gamepad = Gamepad.current;
            if (gamepad != null) {
                Vector2 stick = gamepad.leftStick.ReadValue();
                if (stick.sqrMagnitude > 0.04f) {
                    inputVector = stick;
                } else {
                    Vector2 dpad = gamepad.dpad.ReadValue();
                    if (dpad.sqrMagnitude > 0.04f) inputVector = dpad;
                }
            }

            if (_isClimbingLadder) {
                // En escalera, W/S sube y baja verticalmente
                _verticalVelocity = inputVector.y * _ladderClimbSpeed;
                _moveDirection = new Vector3(inputVector.x * 0.5f, 0f, 0f);
            } else {
                Vector3 rawDir = new Vector3(inputVector.x, 0f, inputVector.y);
                _moveDirection = rawDir.sqrMagnitude > 1f ? rawDir.normalized : rawDir;

                _isSprinting = false;
                if (keyboard != null && (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed)) {
                    _isSprinting = true;
                }
                if (gamepad != null && (gamepad.buttonEast.isPressed || gamepad.leftStickButton.isPressed || gamepad.rightTrigger.isPressed)) {
                    _isSprinting = true;
                }

                if (_moveDirection.sqrMagnitude > 0.001f) {
                    FacingDirection = _moveDirection.normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(FacingDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                }
            }

            bool openInventory = false;
            if (keyboard != null && (keyboard.iKey.wasPressedThisFrame || keyboard.tabKey.wasPressedThisFrame)) {
                openInventory = true;
            }
            if (gamepad != null && (gamepad.startButton.wasPressedThisFrame || gamepad.selectButton.wasPressedThisFrame)) {
                openInventory = true;
            }

            if (openInventory && (Time.unscaledTime - _lastInventoryToggleTime > 0.35f) && GameManager.Instance != null) {
                _lastInventoryToggleTime = Time.unscaledTime;
                GameManager.Instance.ChangeState(GameState.Inventory);
            }
        }

        private void ApplyMovement() {
            if (_characterController == null) return;

            float currentSpeed = _isSprinting ? _sprintSpeed : _walkSpeed;
            Vector3 velocity = _moveDirection * currentSpeed;

            if (_isClimbingLadder) {
                velocity.y = _verticalVelocity;
            } else {
                if (_characterController.isGrounded) {
                    _verticalVelocity = -1f;
                } else {
                    _verticalVelocity += _gravity * Time.fixedDeltaTime;
                }
                velocity.y = _verticalVelocity;
            }

            _characterController.Move(velocity * Time.fixedDeltaTime);
        }

        private void HandleInteractionInput() {
            bool interactPressed = false;
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            var mouse = Mouse.current;

            if (keyboard != null && keyboard.eKey.wasPressedThisFrame) interactPressed = true;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame) interactPressed = true;
            if (gamepad != null && (gamepad.buttonSouth.wasPressedThisFrame || gamepad.buttonWest.wasPressedThisFrame)) interactPressed = true;

            if (interactPressed) {
                CheckInteraction();
            }
        }

        private void CheckInteraction() {
            Vector3 center = transform.position + Vector3.up * 0.3f + (FacingDirection * _interactionDistance);
            Collider[] colliders = Physics.OverlapSphere(center, _interactionRadius, _interactLayers);

            foreach (var col in colliders) {
                if (col.gameObject == gameObject) continue;

                if (col.TryGetComponent<IInteractable>(out var interactable)) {
                    interactable.Interact(this);
                    Debug.Log($"[Exploration] Interactuando con: {interactable.GetInteractionPrompt()}");
                    break;
                }
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit) {
            var body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic) return;

            // Ignorar colisiones con el suelo por debajo de los pies
            if (hit.normal.y > 0.6f) return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
            if (pushDir.sqrMagnitude < 0.001f) {
                pushDir = -hit.normal;
                pushDir.y = 0f;
            }
            if (pushDir.sqrMagnitude < 0.001f) return;

            float power = _boxPushPower;
            var pushable = hit.collider.GetComponent<PushableBox>();
            if (pushable != null) {
                power = pushable.PushForce;
            }

            body.linearVelocity = pushDir.normalized * power;
        }

        public void SetClimbingLadder(bool isClimbing) {
            _isClimbingLadder = isClimbing;
            if (isClimbing) {
                _verticalVelocity = 0f;
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position + Vector3.up * 0.3f + (FacingDirection * _interactionDistance);
            Gizmos.DrawWireSphere(center, _interactionRadius);
        }
    }
}
