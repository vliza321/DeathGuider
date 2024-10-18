using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitAnim : MonoBehaviour
{
    private Animator effectAnim;
    private PolygonCollider2D effectCollider;
    private GameObject effectObject;
    private Transform baseParent;
    // Start is called before the first frame update
    void Awake()
    {
        baseParent = this.transform.parent;
        effectObject = this.gameObject;
        effectAnim = effectObject.GetComponent<Animator>();
        effectCollider = effectObject.GetComponent<PolygonCollider2D>();
    }

    public void mQuitAnim()
    {
        effectAnim.SetBool("isActive", false);
        this.transform.parent = baseParent;
        effectCollider.enabled = false;
    }
}
