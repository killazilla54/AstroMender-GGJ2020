namespace HarmonyQuest.Util
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    [System.Serializable]
    public class ProcObject
    {
        public string name;
        public GameObject spawnObject;
        public int weight = 1; // chance
        public int priority = 1; 
   
    }

    public class ProcObjectManager : MonoBehaviour
    {
        //Player Object
        [SerializeField]
        public GameObject player;

        private WeightedList<ProcObject> weightedList;

        public int maxObjectCount;

        public float spawnRadius; 

        private Vector3  spawnVector;

        public float waitToSpawnEnemies;

        [SerializeField]
        public ProcObject[] allObjects;

        private GameObject[] instantiatedObjects;

        public int numInitEnergyPickups = 15;
        public GameObject initEnergyPrefab;

        private int numEnemiesInGame;
        public int maxNumEnemiesinGame;
       

        void InitWeightedList()
        {
            weightedList = new WeightedList<ProcObject>();
            for(int list = 0; list < allObjects.Length; list++)
            {
                weightedList.Add(allObjects[list], allObjects[list].weight, allObjects[list].priority);
            }
        }

        void PopulateProcObjects()
        {
            for (int i = 0; i  < instantiatedObjects.Length; i++)
            {
                if(instantiatedObjects[i] == null || IsOutOfRange(instantiatedObjects[i]))
                {
                    
                    ProcObject obj = weightedList.GetRandomWeightedEntry();
                    
                    while ((numEnemiesInGame > maxNumEnemiesinGame || waitToSpawnEnemies > 0f) && obj.name.Equals("Enemy"))
                    {
                        obj = weightedList.GetRandomWeightedEntry();
                    }
                    
                    instantiatedObjects[i] = Allocate(obj);
                    
                    instantiatedObjects[i].transform.position = randomPoint(player.transform.position + Vector3.Scale(player.transform.forward * 10f, new Vector3(1f, 0f, 1f)));

                }
            }
        }

        Vector3 randomPoint(Vector3 pos)
        {
            float x = Random.Range(0.4f, 1.0f); // RAndom value between 10% and 100% of spawn radius
            float y = Random.Range(0.4f, 0.8f);
            float z = Random.Range(0.4f, 1.0f);

            x *= Random.Range(0, 2) * 2 - 1; // Random negative or positive value 
            y *= (Random.Range(0, 2) * 2 - 1)/2f;
            z *= Random.Range(0, 2) * 2 - 1;
            Vector3 point = new Vector3(x, y, z);
            point = point *= spawnRadius;       //scale to spawnRadius  

            if(y > CombatBalancing.yRange)
            {
                y = CombatBalancing.yRange;
            }
            else if(y < -CombatBalancing.yRange)
            {
                y = -CombatBalancing.yRange;
            }        
          
            point += pos;      // Spawn in range away from the palyer
            return point;
        }

        bool IsOutOfRange(GameObject o)
        {
            //print("TEST 2");
            float dist = Vector3.Distance(o.transform.position, player.transform.position);
            if(dist > spawnRadius * 1.2f)
            {
                // print("IS OUT OF RANGE");
                Deallocate(o);
                return true;
            }
            return false;

        }

        GameObject Allocate(ProcObject o)
        {
            if (o.spawnObject.GetComponent<StrafeEnemy>() != null || o.spawnObject.GetComponent<DumbEnemy>() != null)
            {
                numEnemiesInGame++;
            }
            return Instantiate(o.spawnObject);
        }

        public void Deallocate(GameObject o)
        {
            if (o.GetComponent<StrafeEnemy>() != null || o.GetComponent<DumbEnemy>() != null)
            {
                numEnemiesInGame--;
            }
            Destroy(o);
        }

        void CreateInitEnergyPickups()
        {
            for (int i = 0; i < numInitEnergyPickups; i++)
            {
                print("CREATING ENERGY " + i);
                Instantiate(initEnergyPrefab, randomPoint(player.transform.position), Random.rotation);
            }
        }

            // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerController>().gameObject;
            InitWeightedList();
            instantiatedObjects = new GameObject [maxObjectCount ];
            PopulateProcObjects();
            CreateInitEnergyPickups();
        }

        // Update is called once per frame
        void Update()
        {
            if (waitToSpawnEnemies > 0)
            {
                waitToSpawnEnemies -= Time.deltaTime;
                if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
                {
                    if(waitToSpawnEnemies > 20f) {
                        waitToSpawnEnemies = 20f;
                    }
                }
            }

            PopulateProcObjects();
        }


        // Update is called once per frame

    }
}