using UnityEngine;
using System.Collections;

public class GiveWeaponAction : ScriptAction {

    public static GiveWeaponAction create(GameObject actor, GameObject weapon)
    {
        return new GiveWeaponAction() { actor = actor, weapon = weapon };
    }

    public GameObject actor;
    public GameObject weapon;

    protected override string getDebugId()
    {
        return "give " + weapon.name + " to " + actor.name;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        Instant();
        complete();
    }

    public override void Instant()
    {
        weapon.SetActive(true);
        actor.GetComponent<Ritualist>().inRightHand = weapon;
    }

}
