using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class StepsInstancer : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] GameObject stepPrefab = null;

    [Header("Materials")]
    [SerializeField] Material leftStep = null;
    [SerializeField] Material rightStep = null;

    [Header("Step Settings")]
    [SerializeField] private float stepDistance = 0.35f;
    [SerializeField] private float stepOffset = 0.15f; // Distanza laterale del piede dal centro
    [SerializeField] private float stepHeight = 0.05f;
    [SerializeField] private int stepsPool = 30;

    private Vector3 lastPos = Vector3.zero;
    private int isRightStep = 1;
    private GameObject[] steps;
    private int stepIndex = 0;

    void Start()
    {
        lastPos = transform.position;
        Quaternion rot;
        Vector3 pos;
        (pos, rot) = StepPosition();

        steps = new GameObject[stepsPool];
        for (int i = 0; i < stepsPool; i++)
        {
            GameObject step = StepGeneration(pos, rot);
            step.SetActive(false);
            steps[i] = step;
        }
    }

    void Update()
    {
        if (Vector3.Distance(lastPos, transform.position) >= stepDistance)
        {
            GameObject newStep = steps[stepIndex];
            Vector3 pos;
            Quaternion rot;

            // Alterna tra piede sinistro e destro
            if (isRightStep == 0)
            {
                Material mat = new Material(leftStep);
                newStep.GetComponent<DecalProjector>().material = mat;
                isRightStep = 1;
            }
            else
            {
                Material mat = new Material(rightStep);
                newStep.GetComponent<DecalProjector>().material = mat;
                isRightStep = 0;
            }

            (pos, rot) = StepPosition();

            newStep.transform.position = pos;
            newStep.transform.rotation = rot;
            newStep.GetComponent<DecalProjector>().material.SetFloat("_Opacity", 1.0f);
            newStep.SetActive(true);

            lastPos = transform.position;

            // Applica fade a tutti gli step attivi
            for (int i = 0; i < stepsPool; i++)
            {
                if (steps[i].activeSelf)
                    Fade(i);
            }

            stepIndex = (stepIndex + 1) % stepsPool;
        }
    }

    private (Vector3 pos, Quaternion rot) StepPosition()
    {
        // Calcola la direzione di movimento effettiva
        Vector3 movementDirection = (transform.position - lastPos).normalized;

        // Se non c'è movimento, usa la direzione in cui guarda il personaggio come fallback
        if (movementDirection.sqrMagnitude < 0.01f)
        {
            movementDirection = transform.forward;
        }

        // Calcola la rotazione guardando verso il basso e allineata con la direzione di movimento
        Quaternion rot = Quaternion.LookRotation(Vector3.down, movementDirection);

        // Calcola il vettore destro basato sulla direzione di movimento
        Vector3 rightDirection = Vector3.Cross(movementDirection, Vector3.up).normalized;

        // Calcola l'offset laterale basato sulla direzione di movimento
        float sideOffset = (isRightStep == 0) ? -stepOffset : stepOffset;
        Vector3 lateralOffset = rightDirection * sideOffset;

        // Posizione finale: posizione corrente + offset laterale + altezza
        Vector3 pos = transform.position + lateralOffset + Vector3.up * stepHeight;

        return (pos, rot);
    }

    private GameObject StepGeneration(Vector3 pos, Quaternion rot)
    {
        GameObject step = Instantiate(stepPrefab, pos, rot, null);
        return step;
    }

    void Fade(int index)
    {
        GameObject step = steps[index];
        float fadeValue = 1.0f;
        int longevity = ((stepIndex - index) + stepsPool) % stepsPool;

        if (longevity > 15)
        {
            float x = longevity - 15;
            fadeValue -= 0.2f * x;
            step.GetComponent<DecalProjector>().material.SetFloat("_Opacity", fadeValue);

            if (fadeValue < -1.0f)
            {
                step.SetActive(false);
            }
        }
    }
}

