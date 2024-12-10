#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[ExecuteInEditMode]
public class PinToViewport : MonoBehaviour
{
    private Material material;
    private bool destroyOnToolSwitch;
    private float? fixedZ = null;

    public void Activate(Material newMaterial, bool newDestroyOnToolSwitch, float? fixedPositionZ = null)
    {
        destroyOnToolSwitch = newDestroyOnToolSwitch;
        fixedZ = fixedPositionZ;

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.SetParent(transform);
        plane.transform.localScale = Vector3.one;

        if (fixedPositionZ != null)
        {
            plane.transform.SetLocalPositionAndRotation(Vector3.forward * fixedPositionZ.Value, Quaternion.identity);
        }
        plane.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        plane.transform.Rotate(Vector3.left, 90f);

        plane.GetComponent<MeshRenderer>().material = newMaterial;
        material = plane.GetComponent<MeshRenderer>().sharedMaterial;

        EditorApplication.update += OnEditorUpdate;
        ToolManager.activeToolChanged += OnToolChanged;
    }

    public void SetMaterialColor(Color newColor)
    {
        material.color = newColor;
    }

    private void OnToolChanged()
    {
        if (destroyOnToolSwitch)
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnEditorUpdate()
    {
        Camera sceneViewCamera = SceneView.lastActiveSceneView.camera;

        Vector3 newPosition = sceneViewCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, sceneViewCamera.nearClipPlane + 1f));
        if (fixedZ != null)
        {
            newPosition.z = fixedZ.Value;
        }
        transform.position = newPosition;

        var x = Mathf.Abs(sceneViewCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 1f)).x - sceneViewCamera.ViewportToWorldPoint(new Vector3(1f, .5f, 1f)).x);
        var y = Mathf.Abs(sceneViewCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 1f)).y - sceneViewCamera.ViewportToWorldPoint(new Vector3(.5f, 1f, 1f)).y);
        transform.localScale = new Vector3(x / 5, y / 5, 1);
    }

    public void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
        ToolManager.activeToolChanged -= OnToolChanged;
    }
}

#endif