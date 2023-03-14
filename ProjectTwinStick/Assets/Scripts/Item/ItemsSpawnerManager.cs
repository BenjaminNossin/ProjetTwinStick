using System;
using System.Collections;
using System.Collections.Generic;
using Game.Systems.GlobalFramework;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemsSpawnerManager : MonoBehaviour
{
    public Item[] allItems;
    private  List<Item> itemsToSpawn = new List<Item>();
    public ItemSpawner[] allSpawners;
    public Transform centerArena;
   [SerializeField] private List<ItemSpawner> spawnersAvailable;

    [SerializeField] public float maxDistanceToCenterOfArena;
 
    private void Start()
    {
        for (int i = 0; i < allSpawners.Length; i++)
        {
            allSpawners[i].Init(this);
        }
        GameManager.Instance.OnGameStartCallback += OnGameStart;
    }

    public void OnGameStart()
    {
        itemsToSpawn = new List<Item>(allItems);
        spawnersAvailable = new List<ItemSpawner>(allSpawners);
        Debug.Log(spawnersAvailable.Count);
        for (int i =  itemsToSpawn.Count- 1; i >= 0; i--)
        {
         var newItem =  Instantiate(itemsToSpawn[i]);
            newItem.Init(this);
            Spawn(itemsToSpawn[i]);
            itemsToSpawn.Remove(itemsToSpawn[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; 
        if(centerArena)
        Gizmos.DrawWireSphere(centerArena.position,maxDistanceToCenterOfArena);
    }

    public void Spawn(Item item)
    {
        if(spawnersAvailable.Count == 0) return;
        /*int rand = Random.Range(0, spawnersAvailable.Count);  
        Debug.Log("selected" + rand);
        item.SetItemSpawner(spawnersAvailable[rand]);
        item.transform.position = spawnersAvailable[rand].transform.position;
        Debug.Log("removed" + spawnersAvailable[rand].gameObject.name);
        spawnersAvailable.Remove(spawnersAvailable[rand]);*/
        ItemSpawner spawner = spawnersAvailable[Random.Range(0, spawnersAvailable.Count)];
        item.SetItemSpawner(spawner);
        item.transform.position = spawner.transform.position;
        Debug.Log(spawner.name + " : " + spawner.transform.position + "\n" + item.name + ":" + item.transform.position);
        spawnersAvailable.Remove(spawner);
    }

    public void AddSpawnerAvailable(ItemSpawner itemSpawner)
    {
        spawnersAvailable.Add(itemSpawner);
    }
    

}
