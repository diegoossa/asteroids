using UnityEngine;

namespace DO.Asteroids.Hybrid
{
    /// <summary>
    /// Application controller.
    /// </summary>
    public class ApplicationController : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}