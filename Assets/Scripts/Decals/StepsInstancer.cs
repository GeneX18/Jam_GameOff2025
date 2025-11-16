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

    Vector3 lastPos = Vector3.zero;

    int isRightStep = 1;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(lastPos, transform.position) > 0.35f)
        {
            if (stepPrefab != null)
            {
                Quaternion rot = Quaternion.identity;
                rot.SetLookRotation(Vector3.down);

                rot.eulerAngles -= new Vector3(0, 0, transform.eulerAngles.y);

                Vector3 pos = transform.position + new Vector3(transform.position.x * (isRightStep == 0 ? 0.005f : -0.005f), 0.05f, 0);
                
                GameObject step = Instantiate(stepPrefab, pos, rot, null);

                if (isRightStep == 0)
                {
                    Material mat = new Material(leftStep);
                    step.GetComponent<DecalProjector>().material = mat;
                    isRightStep = 1;
                }
                else
                {
                    Material mat = new Material(rightStep);
                    step.GetComponent<DecalProjector>().material = mat;
                    isRightStep = 0;
                }
                
                lastPos = pos;
            }
        }
    }
}
