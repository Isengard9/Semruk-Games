using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NC.Strategy.Tools
{
    public class CameraMovement : MonoBehaviour
    {
        private float x, z;
        private Vector3 _moveVector;

        [SerializeField] private float MovementMultipiler = 5;
        
        public void MoveLeftRight(int value)
        {
            x = value;
        }

        public void MoveUpDown(int value)
        {
            z = value;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveCamera();
            }
        }

        public void MoveCamera()
        {
            _moveVector.x = x;
            _moveVector.z = z;
            this.transform.Translate(_moveVector* Time.deltaTime * MovementMultipiler, Space.World);
        }
    }
}
