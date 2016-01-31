using UnityEngine;
using System.Collections;

namespace Characters
{
    public class CharacterIA : MonoBehaviour
    {
        #region Properties

        [SerializeField] private float m_Speed;
        [SerializeField] private float m_Duration;

        #endregion

        #region Unity

        private void Awake()
        {
            ChangeDirection();
        }

        private void Update()
        {
            if (Time.time < m_WalkTime)
            {
                transform.position += transform.right*Time.deltaTime*m_Speed;
            }
            else
            {
                ChangeDirection();
            }
        }

        #endregion

        #region Privates

        private float m_WalkTime;

        private void ChangeDirection()
        {
            transform.Rotate(transform.up, Random.Range(-180, 180));

            float duration = Random.Range(0, m_Duration);

            m_WalkTime = Time.time + duration;


        }

        #endregion

    }
}
