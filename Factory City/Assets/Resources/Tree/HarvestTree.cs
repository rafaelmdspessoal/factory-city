using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestTree : MonoBehaviour
{
    [SerializeField] private GameObject hitAarea;
    [SerializeField] private int attackDamage;


    private int minDamage;
    private int maxDamage;


    private void Start()
    {
        minDamage = attackDamage - 2;
        maxDamage = attackDamage + 2;
    }

    private void Update()
    {
        ChopTree();
    }

    private void ChopTree()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 colliderSize = Vector3.one * .3f;
            
            Collider[] colliderArray = Physics.OverlapBox(hitAarea.transform.position, colliderSize);
            foreach (Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<ITreeDamageable>(out ITreeDamageable treeDamageable))
                {
                    int damageAmount = Random.Range(minDamage, maxDamage);
                    treeDamageable.Damage(damageAmount);
                    print(collider.GetComponent<Tree>().CurrentHealth());
                }
            }
        }
    }
}
