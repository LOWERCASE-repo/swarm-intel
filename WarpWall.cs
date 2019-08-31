using UnityEngine;

public class WarpWall : MonoBehaviour {
  
  [SerializeField]
  private bool horizontalWarping;
  
  private void OnTriggerEnter2D(Collider2D collider) {
    // if (horizontalWarping) {
    //   collider.transform.parent.position = new Vector2(-transform.position.x * 0.9f, collider.transform.position.y);
    // } else {
    //   collider.transform.parent.position = new Vector2(collider.transform.position.x, -transform.position.y * 0.9f);
    // }
    collider.transform.parent.position = Vector2.zero;
  }
}
