using System.Collections;
using UnityEngine;

namespace FinalAct
{
    public class PlanetBullet : MonoBehaviour
    {
        [SerializeField] private float speed=.05f,timeAlive;
        private Vector3 player;
        private Vector3 reference;
        void Start()
        {
            player = GameObject.FindWithTag("Player").transform.position;
            Destroy(gameObject,timeAlive);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, player, ref reference, speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HealthManager.Instance.TakeDamage(20);
             
            }

            StartCoroutine(Collide(other));

        }

        IEnumerator Collide(Collider other)
        {
            yield return new WaitForSeconds(.5f);
            if (other.CompareTag("Wall") ||  other.CompareTag("Ground") || other.gameObject.layer==8)
            {
            
                Destroy(gameObject);
            }
            
        }
    }
}
