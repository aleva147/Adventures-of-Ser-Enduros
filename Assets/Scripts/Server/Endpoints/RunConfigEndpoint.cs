/*
    RunConfigSO is located in the Resources folder 
    and dynamically loaded here when the client starts a new game starts. 
*/

using UnityEngine;

public class RunConfigEndpoint
{
    public RunConfigDTO HandleRequest()
    {
        RunConfigSO runConfigSO = Resources.Load<RunConfigSO>("RunConfig");
        return runConfigSO.ToRunConfigDTO();
    }
}