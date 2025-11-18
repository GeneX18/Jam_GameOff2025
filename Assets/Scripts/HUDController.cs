using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController istance;
    private void Awake()
    {
        istance = this;
    }

    [SerializeField] TMP_Text interactionText;

    public void EnableInteractionText(string txt)
    {
        interactionText.text = txt + " (E)";
        interactionText.gameObject.SetActive(true);
    }
    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
