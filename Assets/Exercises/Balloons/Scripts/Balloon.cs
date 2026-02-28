using UnityEngine;

[ExecuteInEditMode]
public class Balloon : MonoBehaviour
{
    public BalloonData data;
    private new MeshRenderer renderer;
    private MaterialPropertyBlock mpb;

    private void OnValidate()
    {
        ApplyData();
    }

    private void OnEnable()
    {
        ApplyData();
    }

    private void ApplyData()
    {
        if (data == null)
            return;

        if (renderer == null)
            renderer = GetComponent<MeshRenderer>();

        if (renderer == null)
            return;

        if (mpb == null)
            mpb = new MaterialPropertyBlock();

        mpb.SetTexture("_PositionMap", data.positions);
        mpb.SetTexture("_NormalMap", data.normals);
        mpb.SetVector("_BoundsMin", data.boundsMin);
        mpb.SetVector("_BoundsMax", data.boundsMax);
        renderer.SetPropertyBlock(mpb);
    }
}
