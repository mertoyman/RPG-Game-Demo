﻿using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] GameObject impactEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        float damage = 0;
        Health target = null;
        
        void Start()
        {
            if (target == null) return;
            transform.LookAt(GetAimLocation());
        }

        private void Update() {
            if(target == null) return;            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null) return target.transform.position;
            
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        public void SetTarget(Health target, float damage){
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Health>() != target) return;
            if(target.IsDead()) return;
            target.TakeDamage(damage);
            speed = 0;

            foreach(GameObject toDestroy in destroyOnHit){
                Destroy(toDestroy);
            } 

            if(impactEffect!= null){
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
