using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FlagPos
{
    public FlagPos(Transform _transform, bool _filled)
    {
        filled = _filled;
        transform = _transform;
    }
    public bool filled;
    public Transform transform;
}

public class Script_FlagHolder : MonoBehaviour
{
    [SerializeField] bool RedTeam = false;
    [SerializeField] Transform[] FlagPositions = null;
    [SerializeField] List<FlagPos> FlagPoses = new List<FlagPos>();
    private void Start()
    {
        for (int i = 0; i < FlagPositions.Length; i++)
        {
            FlagPoses.Add(new FlagPos(FlagPositions[i], false));
        }
    }
    public bool IsRedTeam()
    {
        return RedTeam;
    }
    public void AttachFlag(Transform _flag)
    {
        foreach (FlagPos flag in FlagPoses)
        {
            if (!flag.filled)
            {
                _flag.transform.position = flag.transform.position;
            }
        }
    }
}
