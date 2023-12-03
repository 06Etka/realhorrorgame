using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    [Header("Keybinds")]
    [SerializeField] private KeyCode[] indexKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };

    private void Update()
    {
        for (int i = 0; i < indexKeys.Length; i++)
        {
            if (Input.GetKeyDown(indexKeys[i]))
            {
                EnableChild(i);
            }
        }
    }

    private void EnableChild(int index)
    {
        if (index < transform.childCount)
        {
            Transform child = transform.GetChild(index);
            child.gameObject.SetActive(true);

            // Disable other children
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i != index)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("Trying to enable a child with an index out of range.");
        }
    }
}
