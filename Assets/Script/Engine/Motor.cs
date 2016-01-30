using UnityEngine;

namespace Engine
{
    public class Motor : MonoBehaviour {

        #region Properties
        public float Speed
        {
            get
            {
                return m_Speed;
            }

            set
            {
                m_Speed = value;
                SpeedChange.Invoke(value);
            }
        }
        #endregion

        #region Event
        [SerializeField]
        public FloatEvent SpeedChange = new FloatEvent();
        #endregion

        #region API
        /// <summary>
        /// Applique la rotation brut au transform
        /// </summary>
        /// <param name="rotation">transform.rotation = rotation</param>
        public void SetDirection(Quaternion rotation)
        {
            setDirection(rotation);
        }

        /// <summary>
        /// Applique tourne le transforme de offsetRotation par rapport à son orientation d'origine
        /// </summary>
        /// <param name="offsetRotation">Quaternion représentant une rotation par rapport à la rotation de base.</param>
        public void OffsetDirection(Quaternion offsetRotation)
        {
            setDirection(transform.localRotation * offsetRotation);
        }
        #endregion

        #region Unity
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            updatePosition();
            float angle = transform.localEulerAngles.z;
            
            angle = Mathf.Clamp(angle > 180 ? angle - 360 : angle, -m_SpeedRotation,m_SpeedRotation);
            OffsetDirection(Quaternion.Euler(-m_SpeedRotation * angle * Time.deltaTime* Vector3.forward));
        }
        #endregion

        #region Private
        [SerializeField]
        private float m_Speed = 10;
        [SerializeField]
        private float m_SpeedRotation = 10f;

        private void updatePosition()
        {
            transform.position += m_Speed * transform.forward *Time.deltaTime;
        }
        private void setDirection(Quaternion rotation)
        {
            transform.rotation= rotation;
            
        }
        #endregion
    }
}
