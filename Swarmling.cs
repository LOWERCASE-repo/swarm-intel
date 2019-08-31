using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Swarmling : Mover {
  
  [SerializeField]
  private Sight avoidance;
  
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
    filter.SetLayerMask(LayerMask.NameToLayer("Swarmlings"));
    Debug.Log(LayerMask.NameToLayer("Swarmlings"));
  }
  private ContactFilter2D filter = new ContactFilter2D();
  
  private void FixedUpdate() {
    Move(transform.up);
    Debug.Log(sight.OverlapCollider(filter, new Collider2D[10]));
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
}
