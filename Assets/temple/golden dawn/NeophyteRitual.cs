using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class NeophyteRitual : MonoBehaviour
{

    public ScriptAction fastForwardTo;

    private AudioClip knock;
    
    public GameObject hierophantPrefab, hiereusPrefab, hegemonPrefab, keruxPrefab, stolistesPrefab, dadouchosPrefab, sentinelPrefab;
    public GameObject imperatorPrefab, cancellariusPrefab, pastHierophantPrefab, praemonstratorPrefab, candidatePrefab;


    public GameObject thronePrefab, altarPrefab;
    public GameObject blackPillarPrefab, whitePillarPrefab, bannerOfTheEastPrefab, bannerOfTheWestPrefab;

    private GameObject hierophant, hiereus, hegemon, kerux, stolistes, dadouchos, candidate, sentinel;
    private Transform marks;
    private GameObject hegemonChair, door, antiDoor;

    private GameObject follow, rig, rigCenter, blindfold;

    private ScriptActionQueue queue = new ScriptActionQueue();

    // Use this for initialization
    void Start()
    {
        var knock = Resources.Load<AudioClip>("knock");


        rig = GameObject.Find("OVRCameraRig");
        rigCenter = rig.transform.GetComponentInChildren<Camera>().gameObject;

        hideMarks();

        initOpening();
        queueOpening();

        queueReception();
    }

    void initOpening()
    {

        door = GameObject.Find("Temple Door");
        antiDoor = GameObject.Find("Anti Door");

        hierophant = addActor(hierophantPrefab, "Hierophant Start");
        hiereus = addActor(hiereusPrefab, "Hiereus Start");
        hegemon = addActor(hegemonPrefab, "Hegemon Start");
        kerux = addActor(keruxPrefab, "Kerux Start");
        stolistes = addActor(stolistesPrefab, "Stolistes Start");
        dadouchos = addActor(dadouchosPrefab, "Dadouchos Start");
        sentinel = addActor(sentinelPrefab, "Sentinel Start");

        addActor(altarPrefab, "Altar");
        addActor(bannerOfTheEastPrefab, "Banner of the East");
        addActor(bannerOfTheWestPrefab, "Banner of the West");
        addActor(blackPillarPrefab, "Black Pillar");
        addActor(whitePillarPrefab, "White Pillar");

        addThrone(thronePrefab, "Hierophant Start");
        addThrone(thronePrefab, "Hiereus Start");
        hegemonChair = addThrone(thronePrefab, "Hegemon Start");
        addThrone(thronePrefab, "Stolistes Start");
        addThrone(thronePrefab, "Dadouchos Start");

        addThrone(thronePrefab, "Imperator Start");
        addThrone(thronePrefab, "Cancellarius Start");
        addThrone(thronePrefab, "Past Hierophant Start");
        addThrone(thronePrefab, "Praemonstrator Start");

        addActor(imperatorPrefab, "Imperator Start");
        addActor(cancellariusPrefab, "Cancellarius Start");
        addActor(pastHierophantPrefab, "Past Hierophant Start");
        addActor(praemonstratorPrefab, "Praemonstrator Start");
    }

    void queueOpening()
    {

        //var camera = "OVRCameraRig"
        //Camera.main.transform.parent = kerux.transform.FindChild("Head").transform;

        queueVoiceAction(knock, hierophant, 1, 2);

        queue.add(CircleMoveAction.create("Kerux Proclaim", kerux));

        queueVoiceAction("o_001", kerux, 1);

        queue.add(CircleMoveAction.create("Kerux Start", kerux));

        //queueVoiceAction(knock, hierophant, 1);
        queueVoiceAction(knock, hierophant, 1);

        queue.add(AllAction.create(
            createStandAction("Hierophant Start Throne", hierophant),
            createStandAction("Hegemon Start Throne", hegemon),
            createStandAction("Hiereus Start Throne", hiereus),
            createStandAction("Stolistes Start Throne", stolistes),
            createStandAction("Dadouchos Start Throne", dadouchos)
        ));

        queueVoiceAction(knock, hierophant, 1);
        queueVoiceAction("o_002", hierophant, 1);

        queue.add(MoveAction.create("Facing Door", kerux));
        queueVoiceAction(knock, door, 1);
        queueVoiceAction(knock, door, 1);
        queue.add(MoveAction.create("Kerux Start", kerux));
        queueVoiceAction("o_003", kerux, 1);
        // he [kerux] salutes the hierophant's throne. Remains by door

        queueVoiceAction("o_004", hierophant, 1);

        // hiereus goes to the door, stands before it with sword erect
        // kerux being on his right
        queue.add(MoveAction.create("Hiereus Start Throne Stand", hiereus));

        queue.add(AllAction.create(
            MoveAction.create("Kerux Standby", kerux),
            CircleMoveAction.create("Kerux Start", hiereus)
        ));

        queueVoiceAction("o_005", hiereus, 1);

        queueVoiceAction("o_006", hiereus, 1);

        // hiereus and kerux return to their places
        queue.add(AllAction.create(
                MoveAction.create("Kerux Start", kerux),
                CircleMoveAction.create("Hiereus Start Throne Stand", hiereus)
        ));


        queueVoiceAction("o_007", hierophant, 1);
        queueVoiceAction("o_008", hierophant, 1);
        queueVoiceAction("o_009", hiereus, 1);
        queueVoiceAction("o_010", hierophant, 1);
        queueVoiceAction("o_011", hiereus, 1);
        queueVoiceAction("o_012", hierophant, 1);
        queueVoiceAction("o_013", hiereus, 1);


        queueVoiceAction("o_014", hierophant, 1);
        queueVoiceAction("o_015", hiereus, 1);
        queueVoiceAction("o_016", hierophant, 1);
        queueVoiceAction("o_017", dadouchos, 1);
        queueVoiceAction("o_018", hierophant, 1);
        queueVoiceAction("o_019", stolistes, 1);
        queueVoiceAction("o_020", hierophant, 1);
        queueVoiceAction("o_021", kerux, 1);
        queueVoiceAction("o_022", hierophant, 1);
        queueVoiceAction("o_023", hegemon, 1);
        queueVoiceAction("o_024", hierophant, 1);
        queueVoiceAction("o_025", hiereus, 1);
        queueVoiceAction("o_026", hierophant, 1);

        queueVoiceAction("o_027", hierophant, 1);
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
        queueVoiceAction("o_036", hierophant, 1);

        queue.add(CircleMoveAction.createMoveToDegree(315, kerux));
        queue.add(CircleMoveAction.createMoveToDegree(300, hegemon, 60));
        queue.add(CircleMoveAction.createMoveToDegree(285, hiereus));
        queue.add(CircleMoveAction.createMoveToDegree(270, stolistes));
        queue.add(CircleMoveAction.createMoveToDegree(255, dadouchos));

        queue.add(AllAction.create(
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

        queueVoiceAction("o_037", hierophant, 1);
        queueVoiceAction("o_038", hierophant, 1);
        queueVoiceAction("o_039", hierophant, 1);
        queueVoiceAction("o_040", hierophant, 1);
        queueVoiceAction("o_041", hierophant, 1);

        queueVoiceAction("o_042", hierophant, 1);
        queueVoiceAction("o_043", kerux, 1);

        queueVoiceAction("o_044", hierophant, 1);
        queueVoiceAction("o_045", hiereus, 1);
        queueVoiceAction("o_046", hegemon, 1);

        queueVoiceAction("o_047", hiereus, 1);
        queueVoiceAction("o_048", hegemon, 1);
        queueVoiceAction("o_049", hierophant, 1);

        queueVoiceAction("o_050", hegemon, 1);
        queueVoiceAction("o_051", hierophant, 1);
        queueVoiceAction("o_052", hiereus, 1);
    }

    void initReception()
    {

    }



    void queueReception()
    {
        queueVoiceAction("r_001", hierophant, 1);
        queueVoiceAction("r_002", hierophant, 1);

        // hegemon rises and removes his chair from between the pillars
        queue.add(new HideAction { actor = hegemonChair });

        // add the candidate in the anitochamber
        candidate = addActor(candidatePrefab, "Candidate Start");

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


        putOnBlindfold();

        // and goes out followed by the sentinal, who carries the hoodwink and rope
        // hegemon sees that the candidate is properly robed and hood winked and that the rope goes three times about his waist

        
        queueVoiceAction(knock, hegemon, 1);
        queueVoiceAction(knock, kerux, 1);
        
        queueVoiceAction("r_003", kerux, 1);
        queueVoiceAction("r_004", hierophant, 1);
        queueVoiceAction("r_005", hierophant, 1);
        queueVoiceAction("r_006", hierophant, 1);



        // S and D stand behind K who is facing the entrance, ready to open the door
        // as soon as candidate is well in the hall, these three officers stand before him in a triangular fashion, and sentinal is behind him.

        queue.add(MoveAction.createNoRotate("Door Open", door));

        queue.add(AllAction.create(
            CircleMoveAction.create("enter kerux", kerux),
            CircleMoveAction.create("enter stolistes", stolistes),
            CircleMoveAction.create("enter dadouchos", dadouchos)
        ));

        // 
        queue.add(CircleMoveAction.createMoveAround("Behind Candidate", hegemon, "Candidate Center", "Behind Candidate"));
        queueVoiceAction("r_007", hegemon, 1);

        queue.add(AllAction.create(
            MoveAction.create("Inside Door 1", candidate),
            MoveAction.create("Inside Door 2", hegemon)
        ));

        queue.add(MoveAction.createNoRotate("Door Close", door));


        queueVoiceAction("r_008", stolistes, 1);
        queueVoiceAction("r_009", dadouchos, 1);
        queueVoiceAction("r_010", hierophant, 1);
        queueVoiceAction("r_011", kerux, 1);

        // S comes forward and dipping his thumb in the lustral water, makes with it a cross on the candidate's brow and sprikles him three times, saying:
        queue.add(MoveAction.create("Facing Candidate", stolistes));
        queueVoiceAction("r_012", stolistes, 1);
        queue.add(MoveAction.create("enter stolistes", stolistes));

        // D comes forward and makes a Cross over candidate with his censer, and waving it three times says:
        queue.add(MoveAction.create("Facing Candidate", dadouchos));
        queueVoiceAction("r_013", dadouchos, 1);
        queue.add(MoveAction.create("enter dadouchos", dadouchos));

        queueVoiceAction("r_014", hierophant, 1);

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

        queueVoiceAction("r_015", hierophant, 1);
        queueVoiceAction("r_016", hegemon, 1);
        queueVoiceAction("r_017", hierophant, 1);
        queueVoiceAction("r_018", hegemon, 1);
        queueVoiceAction("r_019", hierophant, 1);
        queueVoiceAction("r_020", hegemon, 1);
        queueVoiceAction("r_021", hierophant, 1);

        queue.add(MoveAction.create("altar hierophant", hierophant));

        queueVoiceAction("r_022", hierophant, 1);
        queueVoiceAction("r_023", hierophant, 1);
        queueVoiceAction("r_024", hierophant, 1);
        queueVoiceAction("r_025", hierophant, 1);
        queueVoiceAction("r_026", hierophant, 1);
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


    private GameObject addActor(GameObject prefab, string markName)
    {
        var actor = Instantiate(prefab);
        actor.transform.parent = this.transform;
        hideMarks();
        actor.SetActive(false);

        

        queue.add(new SetPositionAction { markName = markName, actor = actor, waitAfter = 0 });
        return actor;
    }

    private GameObject addThrone(GameObject prefab, string markName)
    {
        var actor = Instantiate(prefab);
        actor.transform.parent = this.transform;

        actor.name = markName + " Throne";

        queue.add(new SetPositionAction { markName = markName, actor = actor, waitAfter = 0, offset = new Vector3(0, 0, -0.05f) });
        return actor;
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
        return Resources.Load<AudioClip>("golden-dawn/neophyte/o_" + name);
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
