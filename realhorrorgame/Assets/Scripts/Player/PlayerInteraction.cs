using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction: MonoBehaviour {

    [Header("Variables")]
    [SerializeField] float interactionDistance;
    [SerializeField] Text interactionText;
    [SerializeField] KeyCode interactionKey = KeyCode.E;
    Camera cam;

    void Start() {
        cam = GetComponentInChildren<Camera>();
    }

    void Update() {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            Interactable interactable = hit.collider.GetComponent < Interactable > ();

            if (interactable != null) {
                HandleInteraction(interactable);
                interactionText.text = interactable.GetDescription();
                successfulHit = true;
            }
        }

        if (!successfulHit) {
            interactionText.text = "";
        }
    }

    void HandleInteraction(Interactable interactable) {
        if (Input.GetKeyDown(interactionKey)) {
            interactable.Interact();
        }
    }
}