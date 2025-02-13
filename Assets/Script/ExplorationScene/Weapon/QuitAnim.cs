using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitAnim : MonoBehaviour
{
    private Animator effectAnim;
    private PolygonCollider2D effectCollider;
    private GameObject effectObject;
    private Transform baseParent;
    private CloseTypeWeapon baseClass = new CloseTypeWeapon();
    // Start is called before the first frame update
    void Awake()
    {

        effectObject = this.gameObject;
        effectAnim = effectObject.GetComponent<Animator>();
        effectCollider = effectObject.GetComponent<PolygonCollider2D>();
    }

    public void Initialized(CloseTypeWeapon baseClass,Transform baseObject)
    {
        this.baseClass = baseClass;
        baseParent = baseObject;
        this.gameObject.SetActive(false);
        this.transform.SetParent(baseParent);
    }

    public void mQuitAnim()
    {
        effectAnim.SetBool("isActive", false);
        baseClass.Effects.Enqueue((this.gameObject, effectAnim));
        effectObject.SetActive(false);
    }
}
