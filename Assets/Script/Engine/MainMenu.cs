﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Engine
{
    public class MainMenu : MonoBehaviour
    {
        #region Events
        public UnityEngine.Events.UnityEvent onLoad = new UnityEngine.Events.UnityEvent();
        #endregion

        #region Unity
        void Awake()
        {
            Cardboard.Create();
        }

        void Update()
        {
            if((Input.touchSupported && Input.GetTouch(0).tapCount > 0) || Input.GetMouseButton(0) || Cardboard.SDK.Triggered) 
            { 
                Debug.Log("Trigger Chargement Scene");
                onLoad.Invoke();
                SceneManager.LoadScene(1);
            }
        }
        #endregion

    }
}
