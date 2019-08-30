using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Swarmling : Entity {
  
  [SerializeField]
  private CircleCollider2D sight;
  private HashSet<Collider2D> targets;
  
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
  
  protected override void Start() {
    base.Start();
    targets = new HashSet<Collider2D>();
    ContactFilter2D filter = new ContactFilter2D();
    Debug.Log(sight.OverlapCollider(filter.NoFilter(), new Collider2D[0]));
  }
  
  private void FixedUpdate() {
    Move(transform.up);
    if (targets.Count > 0) {
      Vector2 closest = targets
      .OrderBy(t => (rb.position - t.ClosestPoint(rb.position)).sqrMagnitude)
      .FirstOrDefault().ClosestPoint(rb.position);
      float determinant = GetSide(rb.position, transform.up, closest);
      Rotate(determinant);
    } else {
      Vector2 lookDir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.position;
      float targAng = Vector2.SignedAngle(Vector2.up, lookDir);
      Rotate(Mathf.DeltaAngle(rb.rotation, targAng));
    }
  }
  
  private void OnTriggerEnter2D(Collider2D collider) {
    if (collider.name != "Sight") {
      targets.Add(collider);
    }
  }
  
  private void OnTriggerExit2D(Collider2D collider) {
    targets.Remove(collider);
  }
}
