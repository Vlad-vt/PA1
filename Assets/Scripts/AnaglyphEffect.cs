using Assets.Scripts;
using UnityEngine;

[ExecuteInEditMode]
public class AnaglyphEffect : MonoBehaviour
{
    // Shader for the anaglyph effect
    public Shader fxShader;

    // Second camera for rendering the right-eye view
    public Camera cam2;

    // Material for applying the anaglyph effect
    private Material mat;

    // Render texture for storing the right-eye view
    private RenderTexture rt;

    // Eye separation parameter for the anaglyph effect
    public float eyeSeparation = 1.0f;

    // Convergence distance parameter for the anaglyph effect
    public float convergenceDistance = 1.0f;

    void Start()
    {
        // Subscribe to events for updating parameters dynamically
        LabParameters.GetInstance().EyeSeparationValueChanged += AnaglyphEffect_EyeSeparationValueChanged;
        LabParameters.GetInstance().FieldOfViewValueChanged += AnaglyphEffect_FieldOfViewValueChanged;
        LabParameters.GetInstance().NearClippingDistanceValueChanged += AnaglyphEffect_NearClippingDistanceValueChanged;
        LabParameters.GetInstance().ConvergenceDistanceValueChanged += AnaglyphEffect_ConvergenceDistanceValueChanged;

        // Initialize convergence distance and eye separation
        mat.SetFloat("_ConvergenceDistance", convergenceDistance);
        eyeSeparation = 1.0f;
        transform.localEulerAngles = Vector3.up * eyeSeparation;
        cam2.transform.localEulerAngles = Vector3.up * -eyeSeparation;
    }

    // Event handler for changes in convergence distance
    private void AnaglyphEffect_ConvergenceDistanceValueChanged(float value)
    {
        convergenceDistance = value;
        mat.SetFloat("_ConvergenceDistance", convergenceDistance);
    }

    // Event handler for changes in near clipping distance
    private void AnaglyphEffect_NearClippingDistanceValueChanged(float value)
    {
        cam2.nearClipPlane = value;
    }

    // Event handler for changes in field of view
    private void AnaglyphEffect_FieldOfViewValueChanged(float value)
    {
        cam2.fieldOfView = value;
    }

    /// <summary>
    /// Changes the eye separation parameter.
    /// </summary>
    /// <param name="value">New value for eye separation.</param>
    private void AnaglyphEffect_EyeSeparationValueChanged(float value)
    {
        eyeSeparation = value;
        transform.localEulerAngles = Vector3.up * eyeSeparation;
        cam2.transform.localEulerAngles = Vector3.up * -eyeSeparation;
    }

    // Called when the script is enabled
    private void OnEnable()
    {
        // Check if the shader is assigned
        if (fxShader == null)
        {
            enabled = false;
            return;
        }

        // Create a material using the shader
        mat = new Material(fxShader);
        mat.hideFlags = HideFlags.HideAndDontSave;
        cam2.enabled = false;

        // Set up the render texture
        int width = Screen.width;
        int height = Screen.height;
        rt = RenderTexture.GetTemporary(width, height, 8, RenderTextureFormat.Default);
        cam2.targetTexture = rt;
    }

    // Called when the script is disabled
    private void OnDisable()
    {
        // Clean up the material and release the render texture
        if (mat != null)
        {
            DestroyImmediate(mat);
        }
        if (rt != null)
        {
            rt.Release();
        }
        cam2.targetTexture = null;
    }

    // Called when rendering the image
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Check if necessary components are available
        if (cam2 == null || rt == null || mat == null)
        {
            enabled = false;
            return;
        }

        // Render the right-eye view using the second camera
        cam2.Render();
        mat.SetTexture("_MainTex2", rt);

        // Apply the anaglyph effect to the final image
        Graphics.Blit(source, destination, mat);

        // Release the render texture
        rt.Release();
    }
}
