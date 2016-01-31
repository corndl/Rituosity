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

        public void Update()
        {
            if (Time.time < m_WalkTime)
            {
                MoveForward();
            }
            else
            {
                ChangeDirection();
            }
        }


        void OnTriggerEnter(Collider collider)
        {
            Debug.Log("Character collide with"+collider.tag);
            
            MoveForward();
        }
        #endregion

        #region Privates

        private float m_WalkTime;

        private void Return()
        {
            transform.Rotate(transform.up, 180);

            float duration = Random.Range(1, m_Duration);

            m_WalkTime = Time.time + duration;
        }

        private void MoveForward()
        {
            Vector3 offsetPosition = transform.right*Time.deltaTime*m_Speed;
            Vector3 newRelativePosition = transform.position - transform.parent.position + offsetPosition;

            if (Mathf.Abs(newRelativePosition.x) > 25 || Mathf.Abs(newRelativePosition.y) > 25)
            {
                Return();
            }
            else
            {
                transform.position += offsetPosition;
            }
        }

        private void ChangeDirection()
        {
            transform.Rotate(transform.up, Random.Range(-180, 180));

            float duration = Random.Range(1, m_Duration);

            m_WalkTime = Time.time + duration;


        }

        #endregion

    }
}
