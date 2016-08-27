using UnityEngine;
using System.Collections;

public class GiveWeaponAction : ScriptAction {

    public static GiveWeaponAction createRight(GameObject actor, GameObject weapon)
    {
        return new GiveWeaponAction() { actor = actor, weapon = weapon, hand = "right" };
    }

    public static GiveWeaponAction createLeft(GameObject actor, GameObject weapon)
    {
        return new GiveWeaponAction() { actor = actor, weapon = weapon, hand = "left" };
    }

    public GameObject actor;
    public GameObject weapon;
    public string hand;

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
        if (hand == "right") actor.GetComponent<Ritualist>().inRightHand = weapon;
        if (hand == "left") actor.GetComponent<Ritualist>().inLeftHand = weapon;
    }

}
