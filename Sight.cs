using UnityEngine;
using System.Collections.Generic;

public class Sight : MonoBehaviour {
  
  internal HashSet<Collider2D> targets = new HashSet<Collider2D>();
  
  private void OnTriggerEnter2D(Collider2D collider) {
    if (collider.gameObject.layer != LayerMask.NameToLayer("Sight")) { // TODO
      targets.Add(collider);
    }
  }
  
  private void OnTriggerExit2D(Collider2D collider) {
    targets.Remove(collider);
  }
}
