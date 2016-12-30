﻿using UnityEngine;

public class Projectile: MonoBehaviour {
  public LayerMask collisionMask;

  float speed;
  float damage = 1;

  void Update() {
    float distance = Time.deltaTime * speed;
    CheckCollision(distance);
    transform.Translate(Vector3.forward * distance);
  }

  public void SetSpeed(float newSpeed) {
    speed = newSpeed;
  }

  void CheckCollision(float distance) {
    Ray ray = new Ray(transform.position, transform.forward);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, distance, collisionMask, QueryTriggerInteraction.Collide)) {
      OnObjectHit(hit);
    }
  }

  void OnObjectHit(RaycastHit hit) {
    IDamageable damageable = hit.collider.GetComponent<IDamageable>();

    if (damageable != null) {
      damageable.TakeHit(damage, hit);
    }

    Destroy(gameObject);
  }
}