using UnityEngine;

[System.Serializable]
public class AnimatorStateData
{
    public Animator SelectedAnimator;

    [SelectAnimatorState]
    public string Name;
    public int Hash;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector]
#endif
    public float Length => Clip != null ? Clip.averageDuration : 0f;
    public Motion Clip;
}
