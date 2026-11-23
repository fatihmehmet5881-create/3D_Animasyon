using UnityEngine;

public class BasketBallDie : MonoBehaviour
{
    [SerializeField] private float lifetime = 30f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}