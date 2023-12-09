using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private FloatingJoystick joystick;
        [SerializeField] private float moveSpeed, moveSmoothness, rotateSpeed;

        private CharacterController controller;
        private Quaternion currentRot;


        private Vector3 direction, currentPos, tempCashed;
        private PlayerController playerController;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerController = GetComponent<PlayerController>();
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState obj)
        {
            if (obj == GameState.GameOver)
                joystick.gameObject.SetActive(false);
        }

        private void Start()
        {
            direction = currentPos = tempCashed = Vector3.zero;
        }

        private void Update()
        {
            if (playerController.state == PlayerState.Dead) return;

            direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;

            if (direction == Vector3.zero)
                playerController.SetState(PlayerState.Idle);
            else
                playerController.SetState(PlayerState.Running);
        }

        private void FixedUpdate()
        {
            if (playerController.state == PlayerState.Dead) return;

            var targetPos = direction * (moveSpeed * Time.fixedDeltaTime);
            currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref tempCashed, moveSmoothness);
            currentPos.Set(currentPos.x, 0f, currentPos.z);
            controller.Move(currentPos);

            if (direction == Vector3.zero) return;

            var targetRot = Quaternion.LookRotation(direction);
            currentRot = Quaternion.Slerp(currentRot, targetRot, rotateSpeed * Time.fixedDeltaTime);
            currentRot = Quaternion.Euler(new Vector3(0f, currentRot.eulerAngles.y, 0f));
            transform.rotation = currentRot;
        }
    }
}