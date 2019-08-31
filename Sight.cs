using UnityEngine;

public class Sight : MonoBehaviour {
  
  internal HashSet<Collider2D> targets;
  
  private void OnTriggerEnter2D(Collider2D collider) {
    if (collider.name != "Sight") { // TODO
      targets.Add(collider);
    }
  }
  
  private void OnTriggerExit2D(Collider2D collider) {
    targets.Remove(collider);
  }
  
}
