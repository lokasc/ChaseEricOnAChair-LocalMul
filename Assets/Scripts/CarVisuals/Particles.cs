using UnityEngine;

public class Particles : MonoBehaviour
{
    private ParticleSystem ps;

            void Start()
            {
                ps = GetComponent<ParticleSystem>();
                ps.Play();
            }
}
