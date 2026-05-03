using System;
using UnityEngine;

namespace MaiNull
{
    public class CameraPivot : MonoBehaviour
    {
        // Class to handle PlayerCamera pivot transform

        public Transform AttachedCamera { get; set; }

        private void Update()
        {
            if (!AttachedCamera) return;
            transform.rotation = AttachedCamera.rotation;
        }
    }
}