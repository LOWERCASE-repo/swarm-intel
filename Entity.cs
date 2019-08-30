using UnityEngine;

public abstract class Entity : MonoBehaviour {

  [SerializeField]
  internal float speed;
  [SerializeField]
  internal float accel;
  [SerializeField]
  internal float turnSpeed;
  [SerializeField]
  internal float turnAccel;
  [SerializeField]
  internal Rigidbody2D rb;

  protected Vector2 Predict(Vector2 pos, Vector2 rVel, float iSpeed) {
    Vector2 rPos = pos - rb.position;

    float a = iSpeed * iSpeed - rVel.sqrMagnitude;
    float b = -2f * Vector2.Dot(rVel, rPos);
    float c = -rPos.sqrMagnitude;
    float determinant = b * b - 4f * a * c;

    if (determinant > -Mathf.Epsilon) {
      float time1 = (-b + Mathf.Sqrt(determinant)) / (2f * a);
      float time2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);

      if (time1 > -Mathf.Epsilon && time2 > -Mathf.Epsilon) pos += rVel * Mathf.Min(time1, time2);
      else if (time1 > -Mathf.Epsilon) pos += rVel * time1;
      else if (time2 > -Mathf.Epsilon) pos += rVel * time2;
    }

    return pos;
  }

  protected void Move(Vector2 dir) {
    Vector2 mag = dir.normalized * accel;
    rb.AddForce(mag);
  }

  protected void Rotate(float dir) {
    // if (dir.Equals(Vector2.zero)) return;
    // float ang = Vector2.SignedAngle(Vector2.up, dir);
    // rb.rotation = Mathf.LerpAngle(rb.rotation, ang, 1f / Mathf.PI);
    // Debug.Log(rb.angularVelocity * Mathf.Deg2Rad);
    
    float mag = Mathf.Sign(dir) * turnAccel;
    rb.AddTorque(mag);
  }

  protected virtual void Start() {
    rb.drag = accel / speed;
    rb.angularDrag = turnAccel / turnSpeed;
  }
}
