using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GoodsType
{
    gold,
    darkEssense,
    deathEssense,
    exp
}

public class GoodsSpawnManager : MonoBehaviour
{
    private Dictionary<int, Queue<Goods>> goods;

    private GameObject goodsPrefeb;

    [SerializeField]
    private GoodsType type;

    private MonsterManager monsterManager;
    private ResultManager resultManager;
    private GameManager gameManager;

    Goods cachingGoods;
    Vector3 cachingVector3;
    // Start is called before the first frame update
    void Start()
    {
        cachingVector3 = Vector3.zero;
        //Prefeb 서칭
        goodsPrefeb = this.transform.GetChild(0).gameObject;

        //monsterManager, resultManager 찾기
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {
            if (manager.name == "MonsterSpawnManager")
            {
                monsterManager = manager.GetComponent<MonsterManager>();
            }
            if (manager.name == "ResultManager")
            {
                resultManager = manager.GetComponent<ResultManager>();
            }
        }
        Manager = null;

        //gameManager 찾기
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;

        //몬스터 수 만큼 프리팹 생성
        for (int i =0;i<monsterManager.MaxMonster;i++)
        {
            GameObject newGoods = Instantiate(goodsPrefeb);
            newGoods.transform.SetParent(this.transform);
        }

        //Dictionary 초기화
        goods = new Dictionary<int, Queue<Goods>>(monsterManager.MaxMonster);

        //goodsQueue의 Dictionary 구성
        for (int i = 0;i<this.transform.childCount;i++)
        {
            Queue<Goods> goodsQueue = new Queue<Goods>(10);
            for (int j = 0; j < this.transform.GetChild(i).childCount; j++)
            {
                cachingGoods = this.transform.GetChild(i).GetChild(j).GetComponent<Goods>();
                cachingGoods.init(this,i,gameManager.SelectStageID);
                this.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
                goodsQueue.Enqueue(this.transform.GetChild(i).GetChild(j).GetComponent<Goods>());
            }
            goods.Add(i, goodsQueue);
        }

        //Spawner의 타입 정의
        switch(type)
        {
            case GoodsType.darkEssense:
                monsterManager.DarkEssenseSpawn = this;
                break;
            case GoodsType.gold:
                monsterManager.GoldSpawn = this;
                break;
            case GoodsType.exp:
                monsterManager.ExpSpawn = this;
                break;
        }

    }

    public void ReleaseGoods(int key, Vector3 position)
    {
        if(goods[key].Count <= 0) return;
        cachingGoods = goods[key].Dequeue();
        cachingVector3.x = position.x + Random.Range(0, 4) * 0.33f - 0.66f;
        cachingVector3.y = position.y + Random.Range(0, 4) * 0.33f - 0.66f;
        cachingVector3.z = -1;
        cachingGoods.releaseGoods(cachingVector3);
    }

    public void GetGoods(Goods g, float amount)
    {
        switch (type)
        {
            case GoodsType.darkEssense:
                resultManager.DarkEssense += g.Amount;// + Random.Range(0,g.Amount  * 10);
                break;
            case GoodsType.gold:
                resultManager.Gold += g.Amount + Random.Range(0, g.Amount * 2);
                break;
            case GoodsType.deathEssense:
                resultManager.DeathEssense ++;
                break;
            case GoodsType.exp:
                resultManager.Exp += g.Amount;
                break;
        }
        g.gameObject.SetActive(false);
        goods[g.Key].Enqueue(g);
    }


    public void ReturnGoods(Goods g)
    {
        g.gameObject.SetActive(false);
        goods[g.Key].Enqueue(g);
    }
}
