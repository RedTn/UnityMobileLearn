using UnityEngine;
using System.Collections;

public class LoadUIAtlas : MonoBehaviour {

    public UIAtlas referenceAtlas;

    private int HDThreshold = 640;

    private string HDAtlasName = "UIAtlasHD";
    private string SDAtlasName = "UIAtlasSD";

	void OnEnable()
    {
        if (Screen.height >= HDThreshold)
        {
            Resources.UnloadAsset(referenceAtlas.replacement.texture);
            referenceAtlas.replacement = (Resources.Load(HDAtlasName) as GameObject).GetComponent<UIAtlas>();
        }
    }

    void OnDisable()
    {
        if (Screen.height >= HDThreshold)
        {
            Resources.UnloadAsset(referenceAtlas.replacement.texture);
            referenceAtlas.replacement = (Resources.Load(SDAtlasName) as GameObject).GetComponent<UIAtlas>();
        }
    }
}
