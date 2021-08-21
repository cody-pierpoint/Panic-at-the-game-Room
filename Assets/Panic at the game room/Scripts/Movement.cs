using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codys_Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Vector3 moveDirection;
        [SerializeField] private float gravity = 20f;
        public CharacterController controller;
        public float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;
        public Transform cam;
        public float jumpSpeed;
        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        // Update is called once per frame
        void Update()
        {
            Moveing();
            /*moveDirection.y -= gravity * Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                
                moveDirection.y = jumpSpeed;
            }*/
        }
        private void Moveing()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            //moveDirection = transform.TransformDirection(new Vector3(Input.GetAxisRaw()("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            moveDirection *= speed;
                

                
            if(moveDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                
                controller.Move(moveDirection * speed * Time.deltaTime);
            }
        }
    }
}