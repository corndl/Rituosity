using UnityEngine;
using System.Collections.Generic;
using Characters;

namespace Engine
{
    public class SelectTarget : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        Motor m_Player = null;

        [SerializeField]
        Target m_Target = null;

        [SerializeField]
        GameObject m_Arrow = null;

        [SerializeField]
        Procedural.CityGenerator m_CityGenerator = null;
        #endregion

        #region Private
        void Select(List<Target> targets)
        {
            if (targets.Count == 0)
            {
                return;
            }

            float dist = 0f;
            float minDist = Vector3.Distance(m_Player.transform.position, targets[0].transform.position);
            Target selectedTarget = targets[0];

            foreach (Target target in targets)
            {
                dist = Vector3.Distance(m_Player.transform.position, target.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    selectedTarget = target;
                }
            }
            
            m_Target = selectedTarget;
        }
        #endregion

        #region Unity
        void Awake()
        {
            m_Player = GameObject.FindObjectOfType<Motor>();
            m_CityGenerator = GameObject.FindObjectOfType<Procedural.CityGenerator>();
        }

        void Update()
        {
            Select(m_CityGenerator.Targets);
            if (m_Target != null)
            {
                m_Arrow.transform.LookAt(m_Target.transform.position);
            }
        }
        #endregion
    }
}
