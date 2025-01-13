using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMgr : Singleton<SpawnerMgr>
{
    [SerializeField] SpawnerController controller;
    public SpawnerController SpawnerController { get { return controller; } set { controller = value; } }


}
