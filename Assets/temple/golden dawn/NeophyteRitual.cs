using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class NeophyteRitual : MonoBehaviour
{

    private Dictionary<string, GameObject> actors = new Dictionary<string, GameObject>();

    public ScriptAction fastForwardTo;

    private AudioClip knock;

    private GameObject hierophant, hiereus, hegemon, kerux, stolistes, dadouchos, candidate, sentinel;
    private GameObject hegemonChair, door, antiDoor;

    private GameObject follow, rig, rigCenter, blindfold;

    public GameObject[] markPrefabs;

    private ScriptActionQueue queue = new ScriptActionQueue();

    private float[] lineUp = { 315, 300, 285, 270, 255, 240, 225 };



    // Use this for initialization
    void Start()
    {
        addMarks();
        hideMarks();

        knock = Resources.Load<AudioClip>("knock");
        if (knock == null) throw new Exception("can't load knock");

        rig = GameObject.Find("OVRCameraRig");
        rigCenter = rig.transform.GetComponentInChildren<Camera>().gameObject;

        

        initOpening();
        queueOpening();

        queueReception();

    }

    private GameObject loadPrefabFromResource(string name)
    {
        var path = "golden-dawn/" + name;
        var prefab = Resources.Load<GameObject>(path);

        if (prefab == null) throw new Exception("can't find resource: " + path);

        return prefab;
    }

    private string initPrefab(GameObject prefab)
    {
        var o = Instantiate(prefab);
        o.name = o.name.Replace("(Clone)", "");
        actors[o.name] = o;
        o.transform.parent = this.transform;
        hideMarks();
        o.SetActive(false);
        return o.name;
    }

    private string initPrefabFromResource(string name)
    {
        var prefab = loadPrefabFromResource(name);
        return initPrefab(prefab);
    }

    private GameObject addActor(string prefab, string markName)
    {
        var name = initPrefabFromResource(prefab);
        if (!actors.ContainsKey(name)) throw new Exception("invalid prefab: " + name);
        queue.add(new SetPositionAction { markName = markName, actor = actors[name], waitAfter = 0 });
        return actors[name];
    }

    private GameObject addWeapon(string prefab, string actorName)
    {
        var name = initPrefabFromResource(prefab);
        if (!actors.ContainsKey(name)) throw new Exception("invalid prefab: " + name);
        if (!actors.ContainsKey(actorName)) throw new Exception("invalid actor: " + actorName);
        queue.add(GiveWeaponAction.create(actors[actorName], actors[name]));
        return actors[name];
    }

    private GameObject addThrone(string markName)
    {
        var prefab = loadPrefabFromResource("furniture/Throne");

        var actor = Instantiate(prefab);
        actor.transform.parent = this.transform;

        actor.name = markName + " Throne";

        queue.add(new SetPositionAction { markName = markName, actor = actor, waitAfter = 0, offset = new Vector3(0, 0, -0.05f) });
        return actor;
    }

    void initOpening()
    {
        initPrefabFromResource("furniture/Malkuth Altar");




        door = GameObject.Find("Temple Door");
        antiDoor = GameObject.Find("Anti Door");

        hierophant = addActor("officers/Hierophant", "Hierophant Start");
        hiereus = addActor("officers/Hiereus", "Hiereus Start");
        hegemon = addActor("officers/Hegemon", "Hegemon Start");
        kerux = addActor("officers/Kerux", "Kerux Start");
        stolistes = addActor("officers/Stolistes", "Stolistes Start");
        dadouchos = addActor("officers/Dadouchos", "Dadouchos Start");
        sentinel = addActor("officers/Sentinel", "Sentinel Start");

        addWeapon("weapons/Hierophant Wand", "Hierophant");
        addWeapon("weapons/Hegemon Wand", "Hegemon");

        //addActor(altarPrefab, "Altar");
        addActor("furniture/Malkuth Altar", "Altar");
        addActor("weapons/Banner of the East", "Banner of the East");
        addActor("weapons/Banner of the West", "Banner of the West");
        addActor("furniture/Black Pillar", "Black Pillar");
        addActor("furniture/Black Pillar", "White Pillar");

        addThrone("Hierophant Start");
        addThrone("Hiereus Start");
        hegemonChair = addThrone("Hegemon Start");
        addThrone("Stolistes Start");
        addThrone("Dadouchos Start");

        addThrone("Imperator Start");
        addThrone("Cancellarius Start");
        addThrone("Past Hierophant Start");
        addThrone("Praemonstrator Start");

        addActor("officers/Imperator", "Imperator Start");
        addActor("officers/Cancellarius", "Cancellarius Start");
        addActor("officers/Past Hierophant", "Past Hierophant Start");
        addActor("officers/Praemonstrator", "Praemonstrator Start");
    }

    void queueOpening()
    {

        //var camera = "OVRCameraRig"
        //Camera.main.transform.parent = kerux.transform.FindChild("Head").transform;

        queueVoiceAction(knock, hierophant, 1, 2);

        queue.add(CircleMoveAction.create("Kerux Proclaim", kerux));

        queueVoiceAction("o_001", kerux);

        queue.add(CircleMoveAction.create("Kerux Start", kerux));

        //queueVoiceAction(knock, hierophant);
        queueVoiceAction(knock, hierophant);

        queue.add(AllAction.create(
            createStandAction("Hierophant Start Throne", hierophant),
            createStandAction("Hegemon Start Throne", hegemon),
            createStandAction("Hiereus Start Throne", hiereus),
            createStandAction("Stolistes Start Throne", stolistes),
            createStandAction("Dadouchos Start Throne", dadouchos)
        ));

        queueVoiceAction(knock, hierophant);
        queueVoiceAction("o_002", hierophant);

        queue.add(MoveAction.create("Facing Door", kerux));
        queueVoiceAction(knock, door);
        queueVoiceAction(knock, door);
        queue.add(MoveAction.create("Kerux Start", kerux));
        queueVoiceAction("o_003", kerux);
        // he [kerux] salutes the hierophant's throne. Remains by door

        queueVoiceAction("o_004", hierophant);

        // hiereus goes to the door, stands before it with sword erect
        // kerux being on his right
        queue.add(MoveAction.create("Hiereus Start Throne Stand", hiereus));

        queue.add(AllAction.create(
            MoveAction.create("Kerux Standby", kerux),
            CircleMoveAction.create("Kerux Start", hiereus)
        ));

        queueVoiceAction("o_005", hiereus);

        queueVoiceAction("o_006", hiereus);

        // hiereus and kerux return to their places
        queue.add(AllAction.create(
                MoveAction.create("Kerux Start", kerux),
                CircleMoveAction.create("Hiereus Start Throne Stand", hiereus)
        ));


        queueVoiceAction("o_007", hierophant);
        queueVoiceAction("o_008", hierophant);
        queueVoiceAction("o_009", hiereus);
        queueVoiceAction("o_010", hierophant);
        queueVoiceAction("o_011", hiereus);
        queueVoiceAction("o_012", hierophant);
        queueVoiceAction("o_013", hiereus);


        queueVoiceAction("o_014", hierophant);
        queueVoiceAction("o_015", hiereus);
        queueVoiceAction("o_016", hierophant);
        queueVoiceAction("o_017", dadouchos);
        queueVoiceAction("o_018", hierophant);
        queueVoiceAction("o_019", stolistes);
        queueVoiceAction("o_020", hierophant);
        queueVoiceAction("o_021", kerux);
        queueVoiceAction("o_022", hierophant);
        queueVoiceAction("o_023", hegemon);
        queueVoiceAction("o_024", hierophant);
        queueVoiceAction("o_025", hiereus);
        queueVoiceAction("o_026", hierophant);

        queueVoiceAction("o_027", hierophant);
        queue.add(AllAction.create(CircleMoveAction.create("East", stolistes), CircleMoveAction.create("North", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("South", stolistes), CircleMoveAction.create("East", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("West", stolistes), CircleMoveAction.create("South", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("North", stolistes), CircleMoveAction.create("West", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("East", stolistes), CircleMoveAction.create("North", dadouchos)));


        queue.add(AllAction.create(
            createVoiceAction("o_031", stolistes, 1)
                .then(CircleMoveAction.create("Stolistes Start Throne Stand", stolistes)),
            CircleMoveAction.create("East", dadouchos, 2)
                .then(createVoiceAction("o_035", dadouchos, 1)
                .then(CircleMoveAction.create("Dadouchos Start Throne Stand", dadouchos)))
        ));

        //fastForwardTo = 
        queueVoiceAction("o_036", hierophant);

        queue.add(CircleMoveAction.createMoveToDegree(315, kerux));
        queue.add(CircleMoveAction.createMoveToDegree(300, hegemon, 60));
        queue.add(CircleMoveAction.createMoveToDegree(285, hiereus));
        queue.add(CircleMoveAction.createMoveToDegree(270, stolistes));
        queue.add(CircleMoveAction.createMoveToDegree(255, dadouchos));

        fastForwardTo = queue.add(AllAction.create(
            CircleMoveAction.createMoveToDegree(314.9f, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(299.9f, hegemon, null, 1, 0),
            CircleMoveAction.create("Hiereus Start Throne Stand", hiereus, 2, 0),
            CircleMoveAction.createMoveToDegree(269.9f, stolistes, null, 3, 0).then(CircleMoveAction.createMoveToDegree(284.9f, stolistes, null, 0, 0)),
            CircleMoveAction.createMoveToDegree(254.9f, dadouchos, null, 4, 0).then(CircleMoveAction.createMoveToDegree(269.9f, dadouchos, null, 0, 0))
        ));

        queue.add(AllAction.create(
            CircleMoveAction.createMoveToDegree(314.8f, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(70, hegemon, null, 1, 0).then(MoveAction.create("Hegemon Start Throne Stand", hegemon)),
            CircleMoveAction.createMoveToDegree(284.8f, stolistes, null, 2, 0).then(CircleMoveAction.createMoveToDegree(299.8f, stolistes, null, 0, 0)),
            CircleMoveAction.createMoveToDegree(269.8f, dadouchos, null, 3, 0).then(CircleMoveAction.createMoveToDegree(284.8f, dadouchos, null, 0, 0))
        ));

        queue.add(AllAction.create(
            CircleMoveAction.create("Kerux Start", kerux, 0, 0),
            CircleMoveAction.create("Stolistes Start Throne Stand", stolistes, 1, 0),
            CircleMoveAction.create("Dadouchos Start Throne Stand", dadouchos, 2, 0)
        ));

        queueVoiceAction("o_037", hierophant);
        queueVoiceAction("o_038", hierophant);
        queueVoiceAction("o_039", hierophant);
        queueVoiceAction("o_040", hierophant);
        queueVoiceAction("o_041", hierophant);

        queueVoiceAction("o_042", hierophant);
        queueVoiceAction("o_043", kerux);

        queueVoiceAction("o_044", hierophant);
        queueVoiceAction("o_045", hiereus);
        queueVoiceAction("o_046", hegemon);

        queueVoiceAction("o_047", hiereus);
        queueVoiceAction("o_048", hegemon);
        queueVoiceAction("o_049", hierophant);

        queueVoiceAction("o_050", hegemon);
        queueVoiceAction("o_051", hierophant);
        queueVoiceAction("o_052", hiereus);
    }

    void initReception()
    {

    }



    void queueReception()
    {
        queueVoiceAction("r_001", hierophant);
        queueVoiceAction("r_002", hierophant);

        // hegemon rises and removes his chair from between the pillars
        queue.add(new HideAction { actor = hegemonChair });

        // add the candidate in the anitochamber
        candidate = addActor("officers/Candidate", "Candidate Start");

        queue.add(ExecuteVoidFunctionAction.create(attachCameraToCandidate, "attachCameraToCandidate"));

        removeBlindfold();

        // hegemon enters the antichamber and kerux shuts the door
        queue.add(AllAction.create(
                MoveAction.create("Kerux Standby", kerux),
                CircleMoveAction.createMoveDirected("Facing Door", hegemon, 70)
                    .then(MoveAction.createNoRotate("Door Open", door))
                    .then(MoveAction.createNoRotate("Sentinel Aside", sentinel))
        ));

        queue.add(AllAction.create(
            MoveAction.create("Facing Anti Door", hegemon),
            MoveAction.create("Facing Door", kerux).then(MoveAction.createNoRotate("Door Close", door))
        ));

        // hegemon opens antichamber door, admits candidate, puts blind fold on

        queue.add(MoveAction.createNoRotate("Anti Door Open", antiDoor));

        queue.add(AllAction.create(
            MoveAction.createNoRotate("Inside Anti Door 1", hegemon),
            MoveAction.create("Inside Anti Door 2", candidate, 2f, 4f)
        ));


        //putOnBlindfold();

        // and goes out followed by the sentinal, who carries the hoodwink and rope
        // hegemon sees that the candidate is properly robed and hood winked and that the rope goes three times about his waist

        
        queueVoiceAction(knock, hegemon);
        queueVoiceAction(knock, kerux);
        
        queueVoiceAction("r_003", kerux);
        queueVoiceAction("r_004", hierophant);
        queueVoiceAction("r_005", hierophant);
        queueVoiceAction("r_006", hierophant);



        // S and D stand behind K who is facing the entrance, ready to open the door
        // as soon as candidate is well in the hall, these three officers stand before him in a triangular fashion, and sentinal is behind him.

        queue.add(MoveAction.createNoRotate("Door Open", door));

        //fastForwardTo =
        queue.add(AllAction.create(
            CircleMoveAction.create("enter kerux", kerux),
            CircleMoveAction.create("enter stolistes", stolistes),
            MoveAction.create("enter dadouchos", dadouchos)
        ));

        // 
        queue.add(CircleMoveAction.createMoveAround("Behind Candidate", hegemon, "Candidate Center", "Behind Candidate"));
        queueVoiceAction("r_007", hegemon);

        queue.add(AllAction.create(
            MoveAction.create("Inside Door 1", candidate),
            MoveAction.create("Inside Door 2", hegemon)
        ));

        queue.add(MoveAction.createNoRotate("Door Close", door));


        queueVoiceAction("r_008", stolistes);
        queueVoiceAction("r_009", dadouchos);
        queueVoiceAction("r_010", hierophant);
        queueVoiceAction("r_011", kerux);

        // S comes forward and dipping his thumb in the lustral water, makes with it a cross on the candidate's brow and sprikles him three times, saying:
        queue.add(MoveAction.create("Facing Candidate", stolistes));
        queueVoiceAction("r_012", stolistes);
        queue.add(MoveAction.create("enter stolistes", stolistes));

        // D comes forward and makes a Cross over candidate with his censer, and waving it three times says:
        queue.add(MoveAction.create("Facing Candidate", dadouchos));
        queueVoiceAction("r_013", dadouchos);
        queue.add(MoveAction.create("enter dadouchos", dadouchos));

        queueVoiceAction("r_014", hierophant);

        queue.add(AllAction.create(
            MoveAction.create("altar candidate", candidate),
            MoveAction.create("altar candidate led", hegemon)
        ));

        queue.add(AllAction.create(
            CircleMoveAction.create("altar kerux", kerux),
            CircleMoveAction.create("altar stolistes", stolistes),
            CircleMoveAction.create("altar dadouchos", dadouchos),
            CircleMoveAction.create("altar hiereus", hiereus)
        ));

        queue.add(MoveAction.create("altar hegemon", hegemon));

        queueVoiceAction("r_015", hierophant);
        queueVoiceAction("r_016", hegemon);
        queueVoiceAction("r_017", hierophant);
        queueVoiceAction("r_018", hegemon);
        queueVoiceAction("r_019", hierophant);
        queueVoiceAction("r_020", hegemon);
        queueVoiceAction("r_021", hierophant);

        queue.add(MoveAction.create("altar hierophant", hierophant));

        queueVoiceAction("r_022", hierophant);
        queueVoiceAction("r_023", hierophant);
        queueVoiceAction("r_024", hierophant);
        queueVoiceAction("r_025", hierophant);
        queueVoiceAction("r_026", hierophant);

        // hierophant returns to his throne
        queue.add(MoveAction.create("Hierophant Start Throne Stand", hierophant));

        // hiereus removes hassock and returns to his throne
        queue.add(CircleMoveAction.create("Hiereus Start Throne Stand", hiereus));

        // hegemon assists the candidate to rise
        queue.add(MoveAction.create("Candidate Right Side", hegemon));

        // the other officers resume their seats
        queue.add(AllAction.create(
            CircleMoveAction.create("Kerux Start", kerux),
            CircleMoveAction.create("Stolistes Start Throne Stand", stolistes),
            CircleMoveAction.create("Dadouchos Start Throne Stand", dadouchos)
        ));

        queueVoiceAction("r_027", hierophant);

        // Hegemon takes candidate to the north and faces him east
        // Kerux goes with lamp to NE
        // S and D stand ready to follow in the procession
        queue.add(AllAction.create(
            CircleMoveAction.createMoveToDegree(lineUp[2], kerux),
            CircleMoveAction.createMoveToDegree(lineUp[3], candidate),
            CircleMoveAction.createMoveToDegree(lineUp[4], hegemon),
            CircleMoveAction.createMoveToDegree(lineUp[5], stolistes),
            CircleMoveAction.createMoveToDegree(lineUp[6], dadouchos)
        ));

        queueVoiceAction("r_028", hierophant);

        // Kerux leads forward, followed by hegemon with candidate, S and D coming last
        
        queue.add(AllAction.create(
            CircleMoveAction.createMoveToDegree(lineUp[2]+90, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3]+90, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4]+90, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5]+90, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6]+90, dadouchos, null, 0, 0)
        ));


        // they pass on by south and west
        queue.add(AllAction.create(
            VoiceAction.create(knock, hierophant),// as they pass hierophant gives one knock, just as Candidate passes
            CircleMoveAction.createMoveToDegree(lineUp[2] + 270, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3] + 270, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4] + 270, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5] + 270, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6] + 270, dadouchos, null, 0, 0)
        ));

        // they pass on by the north
        queue.add(AllAction.create(
            VoiceAction.create(knock, hiereus), // and passing hiereus he also gives one knock as candidate passes
            CircleMoveAction.createMoveToDegree(lineUp[2] + 90, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3] + 90, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4] + 90, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5] + 90, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6] + 90, dadouchos, null, 0, 0)
        ));

        //and on passing east
        queue.add(AllAction.create(
            VoiceAction.create(knock, hierophant),//  again hierophant gives one knock and candidate passes
            CircleMoveAction.createMoveToDegree(lineUp[2] + 180, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3] + 180, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4] + 180, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5] + 180, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6] + 180, dadouchos, null, 0, 0)
        ));


        // kerux stops in the south after the second passing of hierophant and barring the way with his wand says:
        queueVoiceAction("r_029", kerux);

        queue.add(CreateMarkAction.create(stolistes, "stol proc 1"));
        queue.add(MoveAction.create("Candidate Proc Blessing Left", stolistes));
        queueVoiceAction("r_30", stolistes);

        queue.add(CreateMarkAction.create(dadouchos, "dad proc 1"));
        queue.add(MoveAction.create("Candidate Proc Blessing Right", dadouchos));
        queueVoiceAction("r_31", dadouchos);

        // S and D then step back to their places in the procession
        queue.add(MoveAction.create("stol proc 1", stolistes));
        queue.add(MoveAction.create("dad proc 1", dadouchos));

        // Kerux leads the procession to throne of hiereus.
        // they pass on by south and west
        queue.add(AllAction.create(
            CircleMoveAction.createMoveToDegree(lineUp[2] + 270, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3] + 270, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4] + 270, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5] + 270, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6] + 270, dadouchos, null, 0, 0)
        ));


        // Hegemon raises the hoodwink for a moment
        // Hiereus stands threatening with his sword


        queueVoiceAction("r_32", hegemon);
        queueVoiceAction("r_33", hiereus);
        queueVoiceAction("r_34", hegemon);
        queueVoiceAction("r_35", hiereus);

        // Kerux leads on.
        queue.add(AllAction.create(
            CircleMoveAction.createMoveToDegree(lineUp[2] + 90, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3] + 90, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4] + 90, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5] + 90, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6] + 90, dadouchos, null, 0, 0)
        ));

        // They pass the hierophant who gives one knock
        queue.add(AllAction.create(
            VoiceAction.create(knock, hierophant),//  again hierophant gives one knock and candidate passes
            CircleMoveAction.createMoveToDegree(lineUp[2] + 270, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3] + 270, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4] + 270, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5] + 270, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6] + 270, dadouchos, null, 0, 0)
        ));

        // hiereus gives one knock as they pass
        queue.add(AllAction.create(
            VoiceAction.create(knock, hiereus), // and passing hiereus he also gives one knock as candidate passes
            CircleMoveAction.createMoveToDegree(lineUp[2], kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3], candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4], hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5], stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6], dadouchos, null, 0, 0)
        ));

        // After this passing kerux halts in the north and raises his wand
        queueVoiceAction("r_36", kerux);

        queue.add(CreateMarkAction.create(stolistes, "stol proc 2"));
        queue.add(MoveAction.create("Candidate Proc Blessing Left", stolistes));
        queueVoiceAction("r_37", stolistes);

        queue.add(CreateMarkAction.create(dadouchos, "dad proc 2"));
        queue.add(MoveAction.create("Candidate Proc Blessing Right", dadouchos));
        queueVoiceAction("r_37.1", dadouchos);

        // S and D then step back to their places in the procession
        queue.add(MoveAction.create("stol proc 2", stolistes));
        queue.add(MoveAction.create("dad proc 2", dadouchos));

        queueVoiceAction("r_38", hegemon);

        queue.add(AllAction.create(
            CircleMoveAction.createMoveToDegree(lineUp[2] + 90, kerux, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[3] + 90, candidate, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[4] + 90, hegemon, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[5] + 90, stolistes, null, 0, 0),
            CircleMoveAction.createMoveToDegree(lineUp[6] + 90, dadouchos, null, 0, 0)
        ));

        queueVoiceAction("r_39", hierophant);

        queueVoiceAction("r_40", hegemon);
        queueVoiceAction("r_41", hierophant);

        // assemble about the altar again
        queue.add(AllAction.create(
            CircleMoveAction.create("altar candidate", candidate),
            CircleMoveAction.create("altar candidate led", hegemon)
        ));

        queue.add(AllAction.create(
            CircleMoveAction.create("altar kerux", kerux),
            CircleMoveAction.create("altar stolistes", stolistes),
            CircleMoveAction.create("altar dadouchos", dadouchos),
            CircleMoveAction.create("altar hiereus", hiereus),
            MoveAction.create("altar hegemon", hegemon)
        ));

        queue.add(MoveAction.create("altar hierophant", hierophant));

        queueVoiceAction("r_42", hierophant);

        queue.add(AllAction.create(
            MoveAction.create("hex kerux", kerux),
            MoveAction.create("hex stolistes", stolistes),
            MoveAction.create("hex dadouchos", dadouchos),
            MoveAction.create("hex hiereus", hiereus),
            MoveAction.create("hex hegemon", hegemon),
            MoveAction.create("hex hierophant", hierophant),
            MoveAction.create("hex candidate", candidate)
        ));


        queueVoiceAction("r_43", hierophant);
        queueVoiceAction("r_44", hegemon);
        queueVoiceAction("r_45", hiereus);
        queueVoiceAction("r_46", hierophant);

        queue.add(AllAction.create(
            createVoiceAction("r_47-1", hegemon, 0),
            createVoiceAction("r_48-1", hiereus, 0),
            createVoiceAction("r_49-1", hierophant, 0)
        ));
        queue.add(AllAction.create(
            createVoiceAction("r_47-2", hegemon, 0),
            createVoiceAction("r_48-2", hiereus, 0),
            createVoiceAction("r_49-2", hierophant, 0)
        ));
        queue.add(AllAction.create(
            createVoiceAction("r_47-3", hegemon),
            createVoiceAction("r_48-3", hiereus),
            createVoiceAction("r_49-3", hierophant)
        ));

        queueVoiceAction("r_50", hierophant);
        queueVoiceAction("r_51", hiereus);
        queueVoiceAction("r_52", hegemon);

        queueVoiceAction("r_53", hiereus);
        queueVoiceAction("r_54", hegemon);
        queueVoiceAction("r_55", hierophant);
        queueVoiceAction("r_56", hegemon);
        queueVoiceAction("r_57", hierophant);
        queueVoiceAction("r_58", hiereus);

        // kerux moves to ne of altar and raises his lamp
        queue.add(MoveAction.create("hex ne", kerux));

        queueVoiceAction("r_59", hierophant);

        // The officers return to their places, hierophant to his throne
        // hegemon and candidate remain west of altar

        // the other officers resume their seats
        queue.add(AllAction.create(
            MoveAction.create("Hierophant Start Throne Stand", hierophant),
            CircleMoveAction.create("Hiereus Start Throne Stand", hiereus),
            CircleMoveAction.create("Kerux Start", kerux),
            CircleMoveAction.create("Stolistes Start Throne Stand", stolistes),
            CircleMoveAction.create("Dadouchos Start Throne Stand", dadouchos),
            MoveAction.create("Candidate Right Side", hegemon)
        ));

        queueVoiceAction("r_60", hierophant);

        // hegemon places candidate to the east, near but not between the pillars
        // then takes his place outside [east of] the white pillar

        queueVoiceAction("r_61", hierophant);

        // hiereus passes by the north to the black pillar
        // he comes round to the east
        // hegemon advances to meet him and take from him his sword and banner
        // hiereus step between the pillars and facing candidate says:


        queueVoiceAction("r_62", hiereus);
        queueVoiceAction("r_63", hiereus);
        queueVoiceAction("r_64", hiereus);
        queueVoiceAction("r_65", hiereus);
        queueVoiceAction("r_66", hiereus);
        queueVoiceAction("r_67", hiereus);
        queueVoiceAction("r_68", hiereus);
        queueVoiceAction("r_69", hiereus);
        queueVoiceAction("r_70", hiereus);
        // they exchange the word
        queueVoiceAction("r_71", hiereus);
        queueVoiceAction("r_72", hiereus);
        queueVoiceAction("r_73", hiereus);
        queueVoiceAction("r_74", hiereus);

        // hiereus leads neophyte forward and then takes back the sword and banner as hegemon hands them to him
        // he stands northeast of the black pillar

        queueVoiceAction("r_75", hiereus);

        // S and D purify hall as in beginning,
        // but on returning to the east, S turns round to N, makes a cross of Water on his brow, sprinkles three times
        queueVoiceAction("r_76", stolistes);

        // D likewise turns round from the east and makes a cross and censes three times
        queueVoiceAction("r_77", dadouchos);

        queueVoiceAction("r_78", hierophant);
        // hegemon comes forward and hands his sceptre and Ritual[blindfold?] to hiereus
        // he removes the rope and puts on the sash over the left shoulder

        queueVoiceAction("r_79", hegemon);

        // hegemon takes Sceptre, etc. and returns to White Pillar

        queueVoiceAction("r_80", hierophant);
        // circumambulation as beginning with 

        queueVoiceAction("r_81", hierophant);
        queueVoiceAction("r_82", hierophant);
        queueVoiceAction("r_83", hierophant);
        queueVoiceAction("r_84", hierophant);
        queueVoiceAction("r_85", hierophant);
        queueVoiceAction("r_86", kerux);
        queueVoiceAction("r_87", kerux);
        queueVoiceAction("r_88", kerux);
        queueVoiceAction("r_89", hierophant);

        queueVoiceAction("r_90", hiereus);
        queueVoiceAction("r_91", hiereus);
        queueVoiceAction("r_92", hierophant);
        queueVoiceAction("r_93", kerux);
        queueVoiceAction("r_94", kerux);
        queueVoiceAction("r_95", hierophant);
    }

    void putOnBlindfold()
    {
        queue.add(ExecuteVoidFunctionAction.create(Blindfold.putOn, "put blindfold on"));
    }

    void removeBlindfold()
    {
        queue.add(ExecuteVoidFunctionAction.create(Blindfold.takeOff, "take blindfold off"));
    }


    void Update()
    {

        queue.Update();

        if (fastForwardTo != null)
        {
            queue.fastForwardTo(fastForwardTo);
            fastForwardTo = null;
        }

        if (follow != null)
        {
            
            //Debug.Log(rigCenter.transform.localPosition);

            // set the rig to the right position
            rig.transform.position = follow.transform.parent.position + followBase;
            rig.transform.rotation = follow.transform.parent.rotation;

            follow.transform.position = rig.transform.position + rigCenter.transform.localPosition;
            follow.transform.localRotation = rigCenter.transform.localRotation;

            //Debug.Log("setting position" + follow.transform.position);
        }
    }

    private Vector3 followBase;

    void attachCameraToCandidate()
    {
        var head = candidate.transform.FindChild("Head").gameObject;
        followBase = head.transform.localPosition;
        follow = head;
    }

    private void addMarks()
    {
        foreach (var markSet in markPrefabs)
        {
            Instantiate(markSet);
        }
    }





    private void hideMarks()
    {
        var marks = GameObject.FindGameObjectsWithTag("Mark");
        foreach (var mark in marks)
        {
            var mr = mark.GetComponentInChildren<MeshRenderer>();
            if (mr != null) Destroy(mr.gameObject);
        }
    }

    private ScriptAction queueVoiceAction(string name, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return queueVoiceAction(getClip(name), actor, waitAfter, waitBefore);
    }
    private ScriptAction queueVoiceAction(AudioClip clip, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return queue.add(VoiceAction.create(clip = clip, actor = actor, waitAfter = waitAfter, waitBefore = waitBefore ));
    }

    private AudioClip getClip(string name)
    {
        var path = "golden-dawn/neophyte/" + name;
        var clip = Resources.Load<AudioClip>(path);

        if (clip == null) throw new Exception("can't find resource: " + path);

        return clip;
    }



    private ScriptAction createVoiceAction(string clipId, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return VoiceAction.create(getClip(clipId), actor, waitAfter, waitBefore);
    }

    private ScriptAction createStandAction(string markName, GameObject actor, float waitAfter = 1f)
    {
        return new MoveAction { markName = markName + " Stand", actor = actor, speed = UnityEngine.Random.RandomRange(0.5f, 1), waitAfter = waitAfter, waitBefore = UnityEngine.Random.RandomRange(0.1f, 1) };
    }

}
