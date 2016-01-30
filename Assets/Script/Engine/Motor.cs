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

        public void SetDirection(Quaternion rotation)
        {
            setDirection(rotation);
        }

        public void OffsetDirection(Quaternion offsetRotation)
        {
            setDirection(transform.rotation * offsetRotation);
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

        }
        #endregion

        #region Private
        [SerializeField]
        private float m_Speed = 10;


        private void setDirection(Quaternion Rotation)
        {
            transform.rotation = Rotation;
        }
        #endregion
    }
}
