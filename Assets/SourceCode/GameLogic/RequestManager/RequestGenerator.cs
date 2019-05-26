using System.Collections.Generic;
using UnityEngine;
using Declarations;

class RequestGenerator: IExecuteble
{
    int m_numberOfFloor;
    float m_requestGenerationCycleDuration = 10.0f;
    float m_timer = 0.0f;

    List<REQUEST_TYPE> m_typeToGenerate;
    List<DIRECTION> m_directionToGenerate;

    IElevatorSimulation m_elevSimulation;

    public void Execute()
    {
        m_timer -= Time.deltaTime;

        if (m_timer <= 0.0)
        {
            GenerateRequest();
            m_timer += m_requestGenerationCycleDuration;
        }
    }

    protected void GenerateRequest()
    {
        Request req = new Request();
        req.Type = m_typeToGenerate[Random.Range(0, m_typeToGenerate.Count)];
        req.ReqDirection = m_directionToGenerate[Random.Range(0, m_directionToGenerate.Count)];
        req.Floor = Random.Range(1, m_numberOfFloor + 1);

        m_elevSimulation.ApplyRequest(req);
    }

    public void Initialize()
    {
        m_numberOfFloor = GameSettingStorageCtr.Instance.NumOfFloors;

        m_typeToGenerate = new List<REQUEST_TYPE> { REQUEST_TYPE.ELEVATOR_CALL };
        m_directionToGenerate = new List<DIRECTION> { DIRECTION.DOWN, DIRECTION.UP };
    }

    public void SetElevatorSimulation(IElevatorSimulation simulation)
    {
        m_elevSimulation = simulation;
    }
}

