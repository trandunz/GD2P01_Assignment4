using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Jail : MonoBehaviour
{
    [SerializeField] bool RedTeam = false;
    private void Start()
    {
        
    }

    public bool IsRedTeam()
    {
        return RedTeam;
    }
}
