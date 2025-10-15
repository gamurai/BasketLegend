using UnityEngine;
using UnityEngine.InputSystem;

namespace BasketLegend.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [Header("Input Actions")]
        [SerializeField] private InputActionAsset inputActions;

        private InputAction backAction;
        private InputAction selectAction;
        private InputAction pauseAction;

        public bool IsBackPressed { get; private set; }
        public bool IsSelectPressed { get; private set; }
        public bool IsPausePressed { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeInputActions();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeInputActions()
        {
            if (inputActions != null)
            {
                backAction = inputActions.FindAction("Back");
                selectAction = inputActions.FindAction("Select");
                pauseAction = inputActions.FindAction("Pause");

                if (backAction != null)
                {
                    backAction.performed += OnBackPressed;
                    backAction.canceled += OnBackReleased;
                }

                if (selectAction != null)
                {
                    selectAction.performed += OnSelectPressed;
                    selectAction.canceled += OnSelectReleased;
                }

                if (pauseAction != null)
                {
                    pauseAction.performed += OnPausePressed;
                    pauseAction.canceled += OnPauseReleased;
                }
            }
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        public void EnableInput()
        {
            backAction?.Enable();
            selectAction?.Enable();
            pauseAction?.Enable();
        }

        public void DisableInput()
        {
            backAction?.Disable();
            selectAction?.Disable();
            pauseAction?.Disable();
        }

        private void OnBackPressed(InputAction.CallbackContext context)
        {
            IsBackPressed = true;
        }

        private void OnBackReleased(InputAction.CallbackContext context)
        {
            IsBackPressed = false;
        }

        private void OnSelectPressed(InputAction.CallbackContext context)
        {
            IsSelectPressed = true;
        }

        private void OnSelectReleased(InputAction.CallbackContext context)
        {
            IsSelectPressed = false;
        }

        private void OnPausePressed(InputAction.CallbackContext context)
        {
            IsPausePressed = true;
        }

        private void OnPauseReleased(InputAction.CallbackContext context)
        {
            IsPausePressed = false;
        }

        private void Update()
        {
            HandleBackInput();
        }

        private void HandleBackInput()
        {
            if (IsBackPressed)
            {
                IsBackPressed = false;
                
                // Handle back navigation based on current state
                var gameStateManager = FindObjectOfType<Core.GameStateManager>();
                if (gameStateManager != null)
                {
                    gameStateManager.ChangeState("MainMenu");
                }
            }
        }

        private void OnDestroy()
        {
            if (backAction != null)
            {
                backAction.performed -= OnBackPressed;
                backAction.canceled -= OnBackReleased;
            }

            if (selectAction != null)
            {
                selectAction.performed -= OnSelectPressed;
                selectAction.canceled -= OnSelectReleased;
            }

            if (pauseAction != null)
            {
                pauseAction.performed -= OnPausePressed;
                pauseAction.canceled -= OnPauseReleased;
            }
        }
    }
}