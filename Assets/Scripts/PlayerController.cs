using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    [SerializeField] private GameObject visionObject;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private Vector2 turningSensitivity = new Vector2(0.1f, 0.1f);
    [SerializeField] private float currentPitch = 0f;

    private float PitchLimit = 60f;

    public float CurrentPitch
    {
        get => currentPitch;
        set
        {
            currentPitch = Mathf.Clamp(value, -PitchLimit, PitchLimit);
        }
    }

    [Header("Input")]
    private Vector2 moveInput;
    private Vector2 mouseInput;

    [Header("Stats")]
    private float walking_noise = 0.1f;
    private float stealth_noise;
    private float running_noise;
    [SerializeField] private float noiseProduced;
   

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        stealth_noise = walking_noise / 2;
        running_noise = walking_noise * 2;
    }
    
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 look = new Vector2(mouseInput.x * turningSensitivity.x, mouseInput.y * turningSensitivity.y);

        Vector3 move = new Vector3(moveInput.x, 0.0f, moveInput.y);
        move = transform.TransformDirection(move);
        controller.Move(move * walkSpeed * Time.deltaTime);

        //up&down rotation
        CurrentPitch -= look.y;
        visionObject.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        //left&right rotation
        transform.Rotate(Vector3.up * look.x);
    }


    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }
   
}
