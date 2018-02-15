using UnityEngine;


public class Person : MonoBehaviour
{
    public bool IsAnimated;

    private float density;

    private Renderer modelRenderer;
    private Animator animator;

    private MaterialPropertyBlock properties;

    public void Initialize ()
    {
        properties = new MaterialPropertyBlock();
        if (IsAnimated)
        {
            animator = GetComponent<Animator>();
            modelRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }
        else
        {
            modelRenderer = GetComponentInChildren<MeshRenderer>();
        }
        modelRenderer.GetPropertyBlock (properties);
    }

    public void AddDensity (float addDensity)
    {
        SetDensity (density + addDensity);
    }

    public void SetDensity (float density)
    {
        this.density = density;
        properties.SetFloat ("_Density", density);
        modelRenderer.SetPropertyBlock (properties);
    }

    public void UpdateAnimation (float speed)
    {
        animator.SetFloat ("speed", speed);
        animator.speed = speed <= 0f ? 1f : speed;
    }
}
