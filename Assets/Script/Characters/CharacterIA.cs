using UnityEngine;
using System.Collections;

namespace Characters
{
    public class CharacterIA : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        float m_Speed;
        #endregion

        #region Unity
        void Awake()
        {
            StartCoroutine(Move());
        }
        #endregion

        #region Routines
        IEnumerator Move()
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
            float duration = Random.Range(0, 5f);
            float time = Time.time;

            while (Time.time < time + duration)
            {
                transform.position += transform.forward * Time.deltaTime * m_Speed;
                yield return new WaitForEndOfFrame();
            }

            StartCoroutine(Move());
        }
        #endregion
    }
}
