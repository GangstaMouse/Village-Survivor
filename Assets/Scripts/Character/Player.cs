using Frameworks.Navigation;
using UnityEngine;

[RequireComponent(typeof(Character))]
public sealed class Player : MonoBehaviour
{
    // Internal variables
    public static Player Instance { get; private set; }
    public Character Character { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }

        Instance = this;   

        Character = GetComponent<Character>();
        Navigation2D.target = transform;
    }
}
