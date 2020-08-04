using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// I want certain UI to show in front of any level geometry
/// </summary>

public class WorldUIFix : MonoBehaviour
{
    private void Start()
    {
        Image image = GetComponent<Image>();
        if(image != null)
        {
            Material existingGlobalMat = image.materialForRendering;
            Material updatedMaterial = new Material(existingGlobalMat);
            updatedMaterial.SetInt("unity_GUIZTestMode", 0);
            image.material = updatedMaterial;
        }
        Text text = GetComponent<Text>();
        if(text != null)
        {
            Material existingGlobalMat = text.materialForRendering;
            Material updatedMaterial = new Material(existingGlobalMat);
            updatedMaterial.SetInt("unity_GUIZTestMode", 0);
            text.material = updatedMaterial;
        }
    }
}