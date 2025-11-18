using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{

    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange;

    private Interactable currentInteractable;

    private void Update()
    {
        CheckInteraction();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(currentInteractable && context.phase == InputActionPhase.Performed)
        {
            currentInteractable.Interact();
        }
    }

    private void CheckInteraction()
    {
        RaycastHit hit;
        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        //se colpisce qualcosa nella visuale del giocatore
        if (Physics.Raycast(r, out hit, interactRange))
        {
            if(hit.collider.tag == "Interactable") //se guardo un oggetto "Interactable"
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if(newInteractable != currentInteractable)
                {                
                    if (newInteractable.enabled)
                    {
                        SetNewCurrentInteractable(newInteractable);
                    }
                    else //se non lo è
                    {
                        DisableCurrentInteractable();
                    }
                }
            }
            else //se non lo è
            {
                DisableCurrentInteractable();
            }
        }
        else //se nulla è a portata
        {
            DisableCurrentInteractable();
        }
    }

    private void SetNewCurrentInteractable(Interactable newInteractable)
    {
        DisableCurrentInteractable();
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.istance.EnableInteractionText(currentInteractable.message);
    }

    private void DisableCurrentInteractable()
    {
        HUDController.istance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
