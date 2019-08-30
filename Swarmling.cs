using UnityEngine;

public class Swarmling : Entity {
  
  
  private void PlayerMove() {
    Vector2 lookDir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.position;
    float targAng = Vector2.SignedAngle(Vector2.up, lookDir);
    Rotate(Mathf.DeltaAngle(rb.rotation, targAng));
    Vector2 moveDir = new Vector2(Input.GetAxisRaw("MoveHorz"), Input.GetAxisRaw("MoveVert"));
    Move(moveDir);
  }
  
  protected override void Start() {
    base.Start();
  }
  
  private void FixedUpdate() {
    Move(transform.up);
    int layer = LayerMask.NameToLayer("Walls");
    float left = Physics2D.Raycast(rb.position - (Vector2)transform.right * 0.5f, transform.up, 4).distance;
    float right = Physics2D.Raycast(rb.position + (Vector2)transform.right * 0.5f, transform.up, 4).distance;
    Debug.Log(left + " " + right); // TODO just avoid trigger closest point
    Debug.DrawRay(rb.position - (Vector2)transform.right * 0.5f, transform.up * 4);
    Debug.DrawRay(rb.position + (Vector2)transform.right * 0.5f, transform.up * 4);
    if (left != 0f || right != 0f) {
      Rotate(right - left);
    }
  }
}
