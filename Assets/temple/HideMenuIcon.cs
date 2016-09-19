using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HideMenuIcon : Activatable {

    public override void onActivate()
    {
        Menu.TriggerHideMenu = true;
    }

    public override void onEnter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void onExit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
