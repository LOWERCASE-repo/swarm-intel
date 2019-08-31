using UnityEngine;
using System.Linq;

public class Swarmling : Mover {
  
  [Header("Swarmling")]
  [SerializeField]
  private Sight avoidance;
  [SerializeField]
  private Sight mimicry;
  [SerializeField]
  private Sight cohesion;
  [SerializeField]
  private float[] weights; // target, avoidance, mimicry, cohesion
  
  private void PlayerMove() {
    Vector2 lookDir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.position;
    float targAng = Vector2.SignedAngle(Vector2.up, lookDir);
    Rotate(Mathf.DeltaAngle(rb.rotation, targAng));
    Vector2 moveDir = new Vector2(Input.GetAxisRaw("MoveHorz"), Input.GetAxisRaw("MoveVert"));
    Move(moveDir);
  }
  
  private float GetSide(Vector2 dirStart, Vector2 dirEnd, Vector2 pos) {
    float determinant = (pos.x - dirStart.x) * (dirEnd.y - dirStart.y) - (pos.y - dirStart.y) * (dirEnd.x - dirStart.x);
    return determinant;
  }
  
  private float TurnTarget() {
    Vector2 lookDir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.position;
    float targAng = Vector2.SignedAngle(Vector2.up, lookDir);
    return Mathf.Sign(Mathf.DeltaAngle(rb.rotation, targAng));
  }
  
  private float TurnAvoid() {
    if (avoidance.targets.Count == 0) return 0f;
    Vector2 closest = avoidance.targets
    .OrderBy(t => (rb.position - t.ClosestPoint(rb.position)).sqrMagnitude)
    .FirstOrDefault().ClosestPoint(rb.position);
    return Mathf.Sign(GetSide(rb.position, transform.up, closest));
  }
  
  private float TurnMimic() {
    if (mimicry.targets.Count == 0) return 0f;
    Vector2 heading = Vector2.zero;
    string names = "";
    foreach (Collider2D target in mimicry.targets) {
      heading += (Vector2)target.transform.up;
      names += target.gameObject.name;
    }
    // Debug.Log(names);
    return Mathf.Sign(GetSide(rb.position, transform.up, heading));
  }
  
  private float TurnCohere() {
    if (cohesion.targets.Count == 0) return 0f;
    Vector2 centroid = Vector2.zero;
    foreach (Collider2D target in cohesion.targets) {
      if (target.gameObject.layer == LayerMask.NameToLayer("Swarmling")) {
        centroid += target.attachedRigidbody.position;
      }
    }
    return Mathf.Sign(GetSide(rb.position, transform.up, centroid));
  }
  
  protected override void Start() {
    base.Start();
    transform.position = Random.insideUnitCircle;
  }
  
  private void FixedUpdate() {
    Move(transform.up);
    float ang = TurnTarget() * weights[0] + TurnAvoid() * weights[1] + TurnMimic() * weights[2] + TurnCohere() * weights[3];
    Rotate(ang);
  }
}
