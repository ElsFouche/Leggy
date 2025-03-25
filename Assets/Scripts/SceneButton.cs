using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    public int sceneIndex;  // Scene index to load when button is pressed

    public void OnButtonPress()
    {
        // Get the TransitionManager and call the method with the scene index
        TransitionManager transitionManager = FindObjectOfType<TransitionManager>();
        if (transitionManager != null)
        {
            transitionManager.TransitionToScene(sceneIndex);
        }
    }
}
