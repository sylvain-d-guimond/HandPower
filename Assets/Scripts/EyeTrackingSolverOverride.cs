using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SolverHandler))]
public class EyeTrackingSolverOverride : MonoBehaviour
{
    public Transform Target;

    private IMixedRealityGazeProvider _gazeProvider;

    private void Awake()
    {
        var solverHandler = GetComponent<SolverHandler>();
        _gazeProvider = CoreServices.InputSystem?.EyeGazeProvider;

        if (_gazeProvider != null) {
            solverHandler.TransformOverride = Target;
        }
    }

    private void Update()
    {
        if (_gazeProvider != null)
        {
            Debug.Log($"Gaze direction: {_gazeProvider.GazeDirection}");
            Target.position = _gazeProvider.GazeOrigin;
            Target.rotation = Quaternion.LookRotation(_gazeProvider.GazeDirection, Vector3.up);
        }
    }
}
