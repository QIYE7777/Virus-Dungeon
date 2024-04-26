using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class EnemyDeath : MonoBehaviour
{
    MeshRenderer[] _parts;
    public Transform bodyParent;

    public Collider colToDisable;
    public Animator acToDisable;
    public float force = 100;

    void Start()
    {
        _parts = GetComponentsInChildren<MeshRenderer>();
    }
    public void BreakDown()
    {
        Debug.Log(_parts);
        colToDisable.enabled = false;
        acToDisable.enabled = false;

        Vector3 centerOfGravity = Vector3.zero;
        foreach (var p in _parts)
        {
            if (p.GetComponent<DoNotBreakDown>() != null)
                continue;
            centerOfGravity += p.transform.position;
        }


        centerOfGravity /= _parts.Length;

        SphereCollider pCollider;

        foreach (var p in _parts)
        {
            if (p.GetComponent<DoNotBreakDown>() != null)
                continue;

            var rb = p.AddComponent<Rigidbody>();
            p.AddComponent<SphereCollider>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.angularDrag = 1.5f;
            rb.drag = 2;
            var dir = p.transform.position - centerOfGravity;
            rb.AddForce(dir.normalized * force);

            pCollider = p.GetComponent<SphereCollider>();
            StartCoroutine(partSink(pCollider));
        }

        IEnumerator partSink(SphereCollider c)
        {
            yield return new WaitForSeconds(2);
            
            c.enabled = false;
            Debug.Log("sink");
         }

    }

    
}
