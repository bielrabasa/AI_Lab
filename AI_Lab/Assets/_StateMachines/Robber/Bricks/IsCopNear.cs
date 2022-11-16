using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/Is Cop Near?")]
[Help("Checks whether Cop is near the Treasure.")]
public class IsCopNear : ConditionBase
{
    public override bool Check()
    {
        GameObject cop = GameObject.Find("COP");
        GameObject old = GameObject.Find("OLD_MAN");
        return Vector3.Distance(cop.transform.position, old.transform.position) < 25f;
    }
}