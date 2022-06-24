using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




[CreateAssetMenu(fileName = "MasterManager", menuName = "ScriptableObjects/Managers/MasterManager", order = 0)]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField]
    private ItemDatabase _itemDatabase;
    public static ItemDatabase ItemDatabase {get {return Instance._itemDatabase;}}

}
