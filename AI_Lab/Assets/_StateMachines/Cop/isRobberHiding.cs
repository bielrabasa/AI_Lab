using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/Is robber hiding?")]
public class isRobberHiding : ConditionBase
{
    [InParam("robber")]
    GameObject robber;
    public override bool Check()
    {
        return robber.GetComponent<Movement>().hiding;
    }
}