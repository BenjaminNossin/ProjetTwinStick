using UnityEngine;
using UnityEngine.InputSystem;
using Game.Systems.GlobalFramework; 

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IController
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerShoot _playerShoot;
    [SerializeField] private PlayerTake _playerTake;
    [SerializeField] private PlayerDrop _playerDrop;
    [SerializeField] private PlayerThrow _playerThrow;
    [SerializeField] private PlayerAim _playerAim;
    private bool isActive = false;
    [SerializeField] GameObject playerObject;
    [SerializeField] private PlayerRenderer _playerRenderer;
    [SerializeField]
    private PlayerInput _playerInput;
    
    public void InstantiateRenderer(PlayerRendererLinker rendererPrefab)
    {
       
        PlayerRendererLinker renderer =  Instantiate(rendererPrefab, _playerRenderer.pivotRenderer);
        renderer.Link(_playerRenderer);
    }
    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        InputAction movement = _playerInput.currentActionMap["Movement"];
        movement.performed += UpdateMovementInput;
        movement.canceled += UpdateMovementInput;
        
        InputAction take = _playerInput.currentActionMap["Take"];
        take.performed += TryTakeInput;
        
        InputAction shoot = _playerInput.currentActionMap["Shoot"];
        shoot.performed += UpdateShootInput;
        shoot.canceled += UpdateShootInput;
        
        InputAction throwAction = _playerInput.currentActionMap["Throw"];
        throwAction.performed += StartAiming;
        throwAction.canceled += TryThrow;
        
        InputAction cancelThrow = _playerInput.currentActionMap["CancelThrow"];
        cancelThrow.performed += TryDrop;
        cancelThrow.performed += CancelThrow;
    }



    #region InputCallbacks

    private void Start()
    {
        Debug.Log("Spawning new player");
        AddSelfToCurrentState();
    }

    private void AddSelfToCurrentState()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddControllerToCurrentState(this); 
        }
    }

    private void StartAiming(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            _playerThrow.PerformAction(false);
        }
    }

    private void TryThrow(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            _playerThrow.PerformAction(true);
        }
    }

    private void CancelThrow(InputAction.CallbackContext obj)
    {
        if (playerObject != null && isActive)
        {
            _playerThrow.CancelAction();
        }
    }

    private void UpdateShootInput(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            Vector2 inputs = context.ReadValue<Vector2>();
            _playerAim.PerformAction(inputs);
            _playerShoot.PerformAction(inputs);
        }
    }

    private void UpdateMovementInput(InputAction.CallbackContext context)
    {
        if (playerObject != null && isActive)
        {
            Vector2 inputs = context.ReadValue<Vector2>();
            _playerMovement.PerformAction(inputs);
        }
    }

    private void TryTakeInput(InputAction.CallbackContext context)
    {
        _playerTake.PerformAction();
    }

    private void TryDrop(InputAction.CallbackContext context)
    {
        Debug.Log("try drop");
        _playerDrop.PerformAction();
    }
    
    #endregion

    public void SetUpController()
    {
        _playerMovement.SetupAction();
        _playerShoot.SetupAction();
        _playerTake.SetupAction();
        _playerDrop.SetupAction();
        _playerThrow.SetupAction();
        _playerAim.SetupAction();
    }

    public void ActivateController()
    {
        isActive = true;
        _playerDrop.EnableAction();
        _playerThrow.EnableAction();
        _playerShoot.EnableAction();
        _playerTake.EnableAction();
        _playerMovement.EnableAction();
        _playerAim.EnableAction();

    }

    public void DeactivateController()
    {
        isActive = false;
        _playerDrop.DisableAction();
        _playerThrow.DisableAction();
        _playerShoot.DisableAction();
        _playerTake.DisableAction();
        _playerMovement.DisableAction();
        _playerAim.DisableAction();
    }

    public void SetControllerSpawnPosition(Vector3 _pos)
    {
        _playerMovement.SetControllerSpawnPosition(_pos); 
    }
}