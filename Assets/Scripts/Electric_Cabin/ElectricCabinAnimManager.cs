using UnityEngine;

public class ElectricCabinAnimManager : MonoBehaviour
{
    [SerializeField] Animator redLamp;
    [SerializeField] Animator greenLamp;
    [SerializeField] Animator lever;

    delegate void Power();
    Power energyPower;

    private void Start()
    {
        energyPower += ActivateEnergy;

        if (energyPower != null) energyPower?.Invoke();
    }

    public void ActivateEnergy()
    {
        lever.Play("ElectricCabin_Lever");
        redLamp.Play("ElectricCabin_RedLamp");
        greenLamp.Play("ElectricCabin_GreenLamp");
    }
}
