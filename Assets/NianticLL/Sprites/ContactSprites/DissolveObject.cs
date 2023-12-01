using UnityEngine;

//[RequireComponent(typeof(Renderer))]
public class DissolveObject : MonoBehaviour
{
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;
    [SerializeField] private float dissolveSpeed = 1.0f;

    //private Material material;

    private void Awake()
    {
        //material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            Material material = r.material;
            if(material.GetFloat("_CutoffHeight") >= -(transform.position.y + objectHeight))
            {
                var time = Time.time * Mathf.PI * dissolveSpeed;

                float height = transform.position.y;
                height += Mathf.Sin(time) * (objectHeight);
                SetHeight(material, height);
            }
        }
    }

    private void SetHeight(Material material, float height)
    {
        material.SetFloat("_CutoffHeight", height);
        material.SetFloat("_NoiseStrength", noiseStrength);
    }
}