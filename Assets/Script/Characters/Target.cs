using UnityEngine;
using Engine;

namespace Characters
{
    public class Target : CharacterIA
    {
        #region Properties
        [SerializeField]
        float m_MaxDistanceToPlayer = 150f;
        [SerializeField]
        GameObject m_Arrow = null;
        #endregion

        #region Events
        public TargetEvent onDestroyed = new TargetEvent();
        public TargetEvent onTooFar = new TargetEvent();
        #endregion

        #region Unity
        void Awake()
        {
            m_Player = FindObjectOfType<Motor>();
        }

        void Update()
        {
            base.Update();
            if (Vector3.Distance(gameObject.transform.position, m_Player.gameObject.transform.position) > m_MaxDistanceToPlayer)
            {
                onTooFar.Invoke(this);
            } 
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.GetComponent<Motor>() != null)
            {
                onDestroyed.Invoke(this);
            }
        }
        #endregion

        #region Private
        Motor m_Player = null;
        #endregion
    }
}
