using Assets.Scripts;
using UnityEngine;

[ExecuteInEditMode]
public class AnaglyphEffect : MonoBehaviour
{
    public Shader fxShader;

    public Camera cam2;

    private Material mat;

    private RenderTexture rt;

    public float eyeSeparation = 1.0f;

    public float convergenceDistance = 1.0f;
    
    void Start()
    {
        LabParameters.GetInstance().EyeSeparationValueChanged += AnaglyphEffect_EyeSeparationValueChanged;
        LabParameters.GetInstance().FieldOfViewValueChanged += AnaglyphEffect_FieldOfViewValueChanged;
        LabParameters.GetInstance().NearClippingDistanceValueChanged += AnaglyphEffect_NearClippingDistanceValueChanged;
        LabParameters.GetInstance().ConvergenceDistanceValueChanged += AnaglyphEffect_ConvergenceDistanceValueChanged;
        mat.SetFloat("_ConvergenceDistance", convergenceDistance);
        eyeSeparation = 1.0f;
        transform.localEulerAngles = Vector3.up * eyeSeparation;
        cam2.transform.localEulerAngles = Vector3.up * -eyeSeparation;
    }

    private void AnaglyphEffect_ConvergenceDistanceValueChanged(float value)
    {
        convergenceDistance = value;
        mat.SetFloat("_ConvergenceDistance", convergenceDistance);
    }

    private void AnaglyphEffect_NearClippingDistanceValueChanged(float value)
    {
        cam2.nearClipPlane = value;
    }

    private void AnaglyphEffect_FieldOfViewValueChanged(float value)
    {
        cam2.fieldOfView = value;
    }

    /// <summary>
    /// Тут змінюється параметр eye separation 
    /// </summary>
    /// <param name="value"></param>
    private void AnaglyphEffect_EyeSeparationValueChanged(float value)
    {
        eyeSeparation = value;
        transform.localEulerAngles = Vector3.up * eyeSeparation;
        cam2.transform.localEulerAngles = Vector3.up * -eyeSeparation;
    }

    private void OnEnable()
    {
        if(fxShader == null)
        {
            enabled = false; 
            return;
        }

        mat = new Material(fxShader);
        mat.hideFlags = HideFlags.HideAndDontSave;
        cam2.enabled = false;

        int width = Screen.width;
        int height = Screen.height;


        rt = RenderTexture.GetTemporary(width, height, 8, RenderTextureFormat.Default);
        cam2.targetTexture = rt;
    }

    private void OnDisable()
    {
        if(mat != null)
        {
            DestroyImmediate(mat);
        }
        if (rt != null)
        {
            rt.Release();
        }
        cam2.targetTexture = null;

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (cam2 == null || rt == null || mat == null)
        {
            enabled = false;
            return;
        }

        cam2.Render();
        mat.SetTexture("_MainTex2", rt);
        
        Graphics.Blit(source, destination, mat);

        rt.Release();
    }
}
