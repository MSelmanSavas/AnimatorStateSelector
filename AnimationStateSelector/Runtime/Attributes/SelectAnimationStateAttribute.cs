using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
public class SelectAnimationStateAttribute : PropertyAttribute
{
    public string AnimatorPropertyName;

    public SelectAnimationStateAttribute() { }

    public SelectAnimationStateAttribute(string animatorPropertyName)
    {
        this.AnimatorPropertyName = animatorPropertyName;
    }
}
