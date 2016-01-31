using UnityEngine;
using System.Collections;
using Engine;
using UnityEngine.Events;

namespace FX
{
    public class InternalView : MonoBehaviour
    {

        #region Properties

        public GameObject InternalSphere;

        #endregion

        #region Event
        /// <summary>
        /// Se déclanche à l'entrée du collider tagué Personnages 
        /// </summary>
        public UnityEvent InternalModeEnterEvent;
        /// <summary>
        /// Se déclanche à la sortie du collider tagué Internal 
        /// </summary>
        public UnityEvent InternalModeExitEvent;

        #endregion

        #region API

        #endregion

        #region Unity

        void OnTriggerEnter(Collider collider)
        {
            
            Debug.Log("Collision Enter with "+collider.tag);
            if (collider.gameObject.CompareTag("Personnages") && !m_IsInternal)
            {
                Destroy(collider.gameObject);
                InternalModeEnterEvent.Invoke();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            Debug.Log("Collision Exit with " + collider.tag);
            if (collider.CompareTag("Internal") && m_IsInternal)
            {
                InternalModeExitEvent.Invoke();
            }
        }

        // Use this for initialization
        private void Start()
        {
            InternalModeEnterEvent.AddListener(InternalModeEnter);
            InternalModeExitEvent.AddListener(InternalModeExit);

            InternalSphere.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion

        #region Private

        private bool m_IsInternal = false;

        private void InternalModeEnter()
        {
            InternalSphere.transform.SetParent(null,true);
            m_IsInternal = true;
            InternalSphere.SetActive(true);
            Camera.main.cullingMask += LayerMask.NameToLayer("Internal");
            Camera.main.cullingMask += LayerMask.NameToLayer("Internal");
        }
        private void InternalModeExit()
        {
            InternalSphere.transform.SetParent(gameObject.transform);
            InternalSphere.transform.position = transform.position;
            InternalSphere.transform.rotation = transform.rotation;

            m_IsInternal = false;
            InternalSphere.SetActive(false);

            Camera.main.cullingMask -= LayerMask.NameToLayer("Internal");
        }
        #endregion
    }
}