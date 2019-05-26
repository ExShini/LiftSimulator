using System.Collections.Generic;
using UnityEngine;

class SimulationExecuterCtr : MonoBehaviour
{
    public static SimulationExecuterCtr Instance { get; private set; }

    List<IExecuteble> m_executebleObjects = new List<IExecuteble>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddToExecution(IExecuteble executebleObj)
    {
        m_executebleObjects.Add(executebleObj);
    }

    void FixedUpdate()
    {
        foreach(var exeObj in m_executebleObjects)
        {
            exeObj.Execute();
        }
    }
}
