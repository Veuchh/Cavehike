using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    //Singleton pattern with awake
    #region Singleton pattern
    public static LightManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one LightsRenderer in scene!");
            return;
        }
        Instance = this;
    }
    #endregion

    [Tooltip("The game is in 2.5D, what is our Z \"Reference\" ? The one where the light is computed, the one we take as reference for the range of the light, the one where the player moves ?")]
    public float ZReferenceLayer=0;
}
