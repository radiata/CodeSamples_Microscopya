using UnityEngine;

public class SynthaseTurbine : MonoBehaviour
{
    [SerializeField] private GameObject turbineSFX;
    [SerializeField] private GameObject protonAttractors;
    [SerializeField] private GameObject particleEmitter;
    [SerializeField] private float speed = .01825f;

    private bool activated = false;

    public void InitializeTurbine()
    {
        particleEmitter.gameObject.SetActive(false);
        protonAttractors.SetActive(false);
        this.enabled = false;
    }

    [ContextMenu("Activate Turbine")]
    public void ActivateTurbine()
    {
        if(activated == true)
        {
            return;
        }

        particleEmitter.gameObject.SetActive(true);
        protonAttractors.SetActive(true);
        this.enabled = true;
        turbineSFX.SetActive(true);

        activated = true;
    }

    private void FixedUpdate()
    {
        var rot = transform.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, rot.y - 100, rot.z), speed * Time.fixedDeltaTime);
    }
}
