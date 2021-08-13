using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject goodItem;
    public GameObject badItem;

    [Range(10, 50)]
    public int goodItemCount = 30;
    [Range(10, 50)]
    public int badItemCount = 20;

    public List<GameObject> goodList = new List<GameObject>();
    public List<GameObject> badList = new List<GameObject>();

    public void InitStage()
    {
        // 기존에 생성됐던 아이템 삭제 및 List 초기화
        foreach (var obj in goodList)
        {
            Destroy(obj);
        }

        // GoodItem 생성
        for (int i = 0; i < goodItemCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-22.0f, 22.0f),
                                      0.05f,
                                      Random.Range(-22.0f, 22.0f));
            Quaternion rot = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

            goodList.Add(Instantiate(goodItem, transform.position + pos, rot, transform));
        }
        // BadItem 생성
        for (int i = 0; i < badItemCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-22.0f, 22.0f),
                                      0.05f,
                                      Random.Range(-22.0f, 22.0f));
            Quaternion rot = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

            badList.Add(Instantiate(badItem, transform.position + pos, rot, transform));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitStage();
    }


}
