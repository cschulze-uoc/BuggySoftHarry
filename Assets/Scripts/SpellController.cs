using UnityEngine;

public class SpellController : MonoBehaviour
{
    public float speed = 3f;
    public float lifeTime = 3f;

    // DAÑO DINÁMICO: normal o cargado
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        DementorController dementor = other.GetComponentInParent<DementorController>();

        if (dementor != null)
        {
            dementor.TakeDamage(damage);

            if (ARDementorGame.Instance != null)
                ARDementorGame.Instance.PlaySpellImpact();

            Destroy(gameObject);
        }
    }
}
