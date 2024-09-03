using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTypeWeapon : MonoBehaviour
{
    private GameObject effectObject;
    private int baseCoolTime;
    private int coolTimer;
    private Animator effectAnim;
    private PolygonCollider2D effectcollider;
    [SerializeField]
    private AttackDirectional playerDirectional;
    private void Awake()
    {
        effectObject = this.gameObject;
        effectAnim = effectObject.GetComponent<Animator>();
        effectcollider = effectObject.GetComponent<PolygonCollider2D>();
        playerDirectional = this.transform.parent.parent.parent.GetChild(1).gameObject.GetComponent<AttackDirectional>();
    }
    // Start is called before the first frame update
    void Start()
    {
        baseCoolTime = 300;
        coolTimer = baseCoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        coolTimer--;
        if(coolTimer <0)
        {
            this.transform.position = playerDirectional.transform.position;
            coolTimer = baseCoolTime;
            effectAnim.SetBool("isActive", true);
            effectcollider.enabled = true;
        }
    }

    public void QuitAnim()
    {
        effectAnim.SetBool("isActive",false);
        effectcollider.enabled = false;
    }
}
