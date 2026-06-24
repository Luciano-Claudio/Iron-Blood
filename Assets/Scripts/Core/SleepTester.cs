using UnityEngine;
using UnityEngine.InputSystem;

// Script temporário de teste do SleepManager — deletar quando a cama existir
// K → TrySleep() | ForceSleep: aguardar o horário forçado ou mudar forcedSleepHour para 1
public class SleepTester : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private void Awake() => inputActions = new PlayerInputActions();

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Teste.performed += OnTest;
    }

    private void OnDisable()
    {
        inputActions.Player.Teste.performed -= OnTest;
        inputActions.Player.Disable();
    }

    private void OnTest(InputAction.CallbackContext ctx)
        => SleepManager.Instance.TrySleep();
}
