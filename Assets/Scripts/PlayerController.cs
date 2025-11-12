using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;

    [Header("Input")]
    private Vector2 moveInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * walkSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
    }
   
}
