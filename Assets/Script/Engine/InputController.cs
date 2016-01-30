using System.Globalization;
using UnityEngine;

namespace Engine
{
    public class InputController : MonoBehaviour {

        #region Properties

        #endregion

        #region Event
        /// <summary>
        /// Se déclache à chaque Input (à chaque frame à priori)
        /// </summary>
        public QuaternionEvent RotationEvent;

        /// <summary>
        /// Se déclache à chaque Input (à chaque frame à priori)
        /// </summary>
        public QuaternionEvent OffsetRotationEvent;

        #endregion

        #region API

        #endregion

        #region Unity
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }
        #endregion

        #region Private

        void HandleInput()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            /*/
            DebugLog(mouseX.ToString(CultureInfo.CurrentCulture));
            DebugLog(mouseY.ToString(CultureInfo.CurrentCulture));
            //*/
            
            OffsetRotationEvent.Invoke(Quaternion.Euler(-mouseY, mouseX, 0));
        }

        void DebugLog(string debugMessage, string color = "grey")
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("<color=red>" + Time.time + "</color> - <color=blue>Input</color> : <color=" + color + ">" + debugMessage + "</color>");
#endif
        }
        #endregion
    }
}
