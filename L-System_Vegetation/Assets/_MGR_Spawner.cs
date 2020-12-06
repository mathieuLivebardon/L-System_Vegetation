using System.Collections.Generic;
using UnityEngine;

public class _MGR_Spawner : MonoBehaviour
{
    private static _MGR_Spawner pInstance = null;
    public static _MGR_Spawner Instance { get { return pInstance; } }
    public int nTrees;
    public LSystem prefab;
    public List<LSystem> trees;

    public GameObject branch;
    public List<GameObject> flowers;
    public int multiply;

    private void Awake()
    {
        if(pInstance == null)
        {
            pInstance = this;
        }
        else if (pInstance !=this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        trees = new List<LSystem>();

        for (int i = 0; i < nTrees; i++)
        {
            GameObject EGOParent = new GameObject();
            EGOParent.name = "Tree" + i;
            //EGOParent.transform.position = new Vector3(transform.position.x + Random.Range(i * multiply - 10f, i * multiply + 10f), 0.0f, transform.position.z + Random.Range(i * multiply - 10f, i * multiply + 10f));
            Vector2 Randompos = Random.insideUnitCircle * nTrees * multiply;
            EGOParent.transform.position = new Vector3(Randompos.x, 0.0f, Randompos.y);
            LSystem newLS = Instantiate(prefab, EGOParent.transform);
            newLS.transform.localPosition = Vector3.zero;
            newLS.branch = this.branch;
            newLS.flower = flowers[Random.Range(0, flowers.Count)];
            newLS.Initialise();
            trees.Add(newLS);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}