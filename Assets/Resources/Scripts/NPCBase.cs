using System;
using UnityEngine;
//abstract base class for NPC
//all detailed NPC implementations extend from this class
abstract public class NPCBase: MonoBehaviour
{
    private Role role;
    public NPCBase() {
        
    }
}
