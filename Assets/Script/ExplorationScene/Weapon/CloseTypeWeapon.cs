using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTypeWeapon : MonoBehaviour
{
    private GameObject effectObject;
    private int baseCoolTime;
    private int coolTimer;
    private Animator effectAnim;
    private PolygonCollider2D effectCollider;
    [SerializeField]
    private AttackDirectional playerDirectional;

    private Vector3 cashingVector;
    public CloseTypeWeapon(int baseCoolTime)
    {
        this.baseCoolTime = baseCoolTime;
    }

    private Transform baseParent;
    
    public int BaseCoolTime
    {
        get { return baseCoolTime; }
        set { baseCoolTime = value; }
    }

    private void Awake()
    {
        baseParent = this.transform.parent;
        cashingVector = new Vector3(0,0,0);
        effectObject = this.gameObject;
        effectAnim = effectObject.GetComponent<Animator>();
        effectCollider = effectObject.GetComponent<PolygonCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.parent.parent.CompareTag("follower"))
        {
            playerDirectional = this.transform.parent.parent.GetComponent<PlayerMove>().AttactDirectional;
        }
        else playerDirectional = this.transform.parent.parent.parent.GetChild(1).gameObject.GetComponent<AttackDirectional>();
        baseCoolTime = 300; // -> 기본 쿨타임 set함수로 변경
        coolTimer = baseCoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        coolTimer--;
        if(coolTimer <0)
        {
            cashingVector = playerDirectional.transform.position;
            coolTimer = baseCoolTime;
            effectAnim.SetBool("isActive", true);
            effectCollider.enabled = true;
            effectAnim.transform.parent = this.transform.parent.parent.parent.GetChild(this.transform.parent.parent.parent.childCount - 1);
        }
        this.transform.position = cashingVector;
    }

    public void QuitAnim()
    {
        effectAnim.SetBool("isActive",false);
        effectAnim.transform.parent = baseParent;
        effectCollider.enabled = false;
    }
}
