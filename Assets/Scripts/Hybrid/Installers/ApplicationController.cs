using UnityEngine;

namespace DO.Asteroids.Hybrid
{
    public class ApplicationController : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}