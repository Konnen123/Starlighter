using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config",menuName = "Guns/Gun Trail Config",order=4)]
public class TrailConfigScriptableObject : ScriptableObject
{
    public Material material;
    public AnimationCurve widthCurve;
    public float duration = .5f;
    public float minVertexDistance = .1f;
    public Gradient color;

    public float missDistance = 100f;
    public float simulationSpeed = 100f;

}
