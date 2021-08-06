using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Codys_Movement
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector2 Lookdir;
        [SerializeField] private Quaternion nextRotation;
        

        [SerializeField] private float rotationPower = 3f;
        [SerializeField] private float rotationLerp = 0.5f;
        public Camera camera;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        /*public void OnLook(InputValue value)
        {
            Lookdir = value.Get<Vector2>();
        }*/

        // Update is called once per frame
        void Update()
        {
         Rotation();
        }

        private void Rotation()
        {
            followTarget.transform.rotation *= Quaternion.AngleAxis(Lookdir.x * rotationPower, Vector3.up);
        #region Vertical Rotation
            followTarget.transform.rotation *= Quaternion.AngleAxis(Lookdir.y * rotationPower, Vector3.right);

            Vector3 angles = followTarget.transform.localEulerAngles;
            angles.z = 0f;

            float angle = followTarget.transform.localEulerAngles.x;
            
            // Clamp the vertical rotation
            if(angle > 180f && angle < 340f)
            {
                angles.x = 340f;
            }
            else if(angle < 180f && angle > 40f)
            {
                angles.x = 40f;
            }

            followTarget.transform.localEulerAngles = angles;

        #endregion

            nextRotation = Quaternion.Lerp(followTarget.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

            transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);

            followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);


        }
        
    }
}