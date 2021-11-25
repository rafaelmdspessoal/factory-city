using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, ITreeDamageable
{
    public enum Type
    {
        Tree,
        Log,
        LogHalf,
        Stump,
    }

    [SerializeField] private Transform treeLog;
    [SerializeField] private Transform treeLogHalf;
    [SerializeField] private Transform treeStump;

    public Type treeType;
    public HealthSystem healthSystem;

    private void Awake()
    {
        int healthAmount;

        switch (treeType)
        {
            default:
            case Type.Tree:
                healthAmount = 30;
                Trees.trees.Add(this.transform);
                break;
            case Type.Log: healthAmount = 30; break;
            case Type.LogHalf: healthAmount = 30; break;
            case Type.Stump: 
                healthAmount = 30;
                Destroy(this.gameObject, 60);
                break;
        }

        healthSystem = new HealthSystem(healthAmount);
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        switch (treeType)
        {
            default:
            case Type.Tree:
                Vector3 treeLogOffset = transform.up * .8f;
                Instantiate(treeLog, transform.position + treeLogOffset, Quaternion.Euler(Random.Range(-.8f, .8f), 0, Random.Range(-.8f, .8f)));
                Instantiate(treeStump, transform.position, transform.rotation);
                Trees.trees.Remove(this.transform);
                break;
            case Type.Log:
                float logYPositionAboveStump = .8f;
                treeLogOffset = transform.up * logYPositionAboveStump;
                Instantiate(treeLogHalf, transform.position + treeLogOffset, transform.rotation);

                float logYPositionAboveFirstLog = 5.1f;
                treeLogOffset = transform.up * logYPositionAboveFirstLog;
                Instantiate(treeLogHalf, transform.position + treeLogOffset, transform.rotation);
                break;
            case Type.LogHalf:
                // Destroy after x time
                break;
            case Type.Stump:
                // Destroy after x time
                break;
        }
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }

    public int CurrentHealth()
    {
        return healthSystem.GetHealth();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<ITreeDamageable>(out ITreeDamageable treeDamageable))
        {
            if (collision.relativeVelocity.magnitude > 1f)
            {
                int damageAmount = Random.Range(5, 15);
                treeDamageable.Damage(damageAmount);
            }
        }
    }
}
