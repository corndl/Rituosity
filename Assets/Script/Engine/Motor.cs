using UnityEngine;

namespace Engine
{
    public class Motor : MonoBehaviour {

        #region Properties

        public bool TangageCorrection = false;

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

        public bool IsInternal
        {
            get
            {
                return m_IsInternal;
            }

            set
            {
                m_IsInternal = value;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Se déclache au changement de vitesse pendant une partie.
        /// </summary>
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

        /// <summary>
        /// Switch InternalMode
        /// </summary>
        /// <param name="isInternal"></param>
        public void InternalMode(bool isInternal)
        {
            IsInternal = isInternal;
        }
        #endregion

        #region Unity
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            updatePosition();

            if(TangageCorrection)
                correctTangage();
        }
        #endregion

        #region Private
        [SerializeField]
        private float m_HauteurMax = 3;
        [SerializeField]
        private float m_Speed = 10;
        [SerializeField]
        private float m_SpeedInternal = 2;
        [SerializeField]
        private float m_SpeedRotation = 10f;

        [SerializeField]
        private bool m_IsInternal = false;

        private void correctTangage()
        {
            float angle = transform.localEulerAngles.z;

            angle = Mathf.Clamp(angle > 180 ? angle - 360 : angle, -m_SpeedRotation, m_SpeedRotation);
            OffsetDirection(Quaternion.Euler(-m_SpeedRotation * angle * Time.deltaTime * Vector3.forward));
        }

        private void updatePosition()
        {
            transform.position += (IsInternal?m_SpeedInternal:m_Speed) * transform.forward *Time.deltaTime;
            if (transform.position.y > m_HauteurMax) transform.position -= Vector3.up * (transform.position.y - m_HauteurMax);
        }
        private void setDirection(Quaternion rotation)
        {
            transform.rotation= rotation;
        }
        #endregion
    }
}
