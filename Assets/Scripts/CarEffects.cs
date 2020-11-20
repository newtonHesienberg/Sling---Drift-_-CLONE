using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    // Start is called before the first frame update

    public TrailRenderer[] trailRenderers;

    public AudioSource driftSound;

    private bool driftFlag;

    private void Update()
    {
        checkDrift();
    }
    public void checkDrift()
    {
        var car = FindObjectOfType<CarMovement>();
        bool driftInfo = car.GetDriftInfo();

        if (driftInfo) startEmiiter();
        else stopEmitter();
    }

    private void startEmiiter()
    {
        if (driftFlag) return;
        foreach(TrailRenderer t in trailRenderers)
        {
            t.emitting = true;
        }

        driftSound.Play();
        driftFlag = true;
    }

    private void stopEmitter()
    {
        if (!driftFlag) return;
        foreach (TrailRenderer t in trailRenderers)
        {
            t.emitting = false;
        }

        driftSound.Stop();
        driftFlag = false;
    }
}
