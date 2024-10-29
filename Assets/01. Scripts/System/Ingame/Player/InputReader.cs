using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    public event Action<Vector2> MovementEvent;
    public event Action AttackEvent;
    public event Action ShieldEvent;

    public Vector2 mousePos;

    private Controls _inputAction;

    private void OnEnable()
    {
        if (_inputAction == null)
        {
            _inputAction = new Controls();
            _inputAction.Player.SetCallbacks(this);
        }

        Enable();
    }

    public void Enable() => _inputAction.Player.Enable();

    public void Disable() => _inputAction.Player.Disable();

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        MovementEvent?.Invoke(value);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            AttackEvent?.Invoke();
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.performed)
            ShieldEvent?.Invoke();
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }
}
