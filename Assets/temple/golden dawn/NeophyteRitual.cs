using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class NeophyteRitual : MonoBehaviour
{

    public ScriptAction fastForwardTo;

    public AudioClip knock;
    public AudioClip[] clips;
    public AudioClip[] receptionClips;
    
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

        queueVoiceAction(001, kerux, 1);

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
        queueVoiceAction(002, hierophant, 1);

        queue.add(MoveAction.create("Facing Door", kerux));
        queueVoiceAction(knock, door, 1);
        queueVoiceAction(knock, door, 1);
        queue.add(MoveAction.create("Kerux Start", kerux));
        queueVoiceAction(003, kerux, 1);
        // he [kerux] salutes the hierophant's throne. Remains by door

        queueVoiceAction(004, hierophant, 1);

        // hiereus goes to the door, stands before it with sword erect
        // kerux being on his right
        queue.add(MoveAction.create("Hiereus Start Throne Stand", hiereus));

        queue.add(AllAction.create(
            MoveAction.create("Kerux Standby", kerux),
            CircleMoveAction.create("Kerux Start", hiereus)
        ));

        queueVoiceAction(005, hiereus, 1);

        queueVoiceAction(006, hiereus, 1);

        // hiereus and kerux return to their places
        queue.add(AllAction.create(
                MoveAction.create("Kerux Start", kerux),
                CircleMoveAction.create("Hiereus Start Throne Stand", hiereus)
        ));


        queueVoiceAction(007, hierophant, 1);
        queueVoiceAction(008, hierophant, 1);
        queueVoiceAction(009, hiereus, 1);
        queueVoiceAction(010, hierophant, 1);
        queueVoiceAction(011, hiereus, 1);
        queueVoiceAction(012, hierophant, 1);
        queueVoiceAction(013, hiereus, 1);


        queueVoiceAction(014, hierophant, 1);
        queueVoiceAction(015, hiereus, 1);
        queueVoiceAction(016, hierophant, 1);
        queueVoiceAction(017, dadouchos, 1);
        queueVoiceAction(018, hierophant, 1);
        queueVoiceAction(019, stolistes, 1);
        queueVoiceAction(020, hierophant, 1);
        queueVoiceAction(021, kerux, 1);
        queueVoiceAction(022, hierophant, 1);
        queueVoiceAction(023, hegemon, 1);
        queueVoiceAction(024, hierophant, 1);
        queueVoiceAction(025, hiereus, 1);
        queueVoiceAction(026, hierophant, 1);

        queueVoiceAction(027, hierophant, 1);
        queue.add(AllAction.create(CircleMoveAction.create("East", stolistes), CircleMoveAction.create("North", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("South", stolistes), CircleMoveAction.create("East", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("West", stolistes), CircleMoveAction.create("South", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("North", stolistes), CircleMoveAction.create("West", dadouchos)));
        queue.add(AllAction.create(CircleMoveAction.create("East", stolistes), CircleMoveAction.create("North", dadouchos)));


        queue.add(AllAction.create(
            createVoiceAction(031, stolistes, 1)
                .then(CircleMoveAction.create("Stolistes Start Throne Stand", stolistes)),
            CircleMoveAction.create("East", dadouchos, 2)
                .then(createVoiceAction(035, dadouchos, 1)
                .then(CircleMoveAction.create("Dadouchos Start Throne Stand", dadouchos)))
        ));

        //fastForwardTo = 
        queueVoiceAction(036, hierophant, 1);

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

        queueVoiceAction(037, hierophant, 1);
        queueVoiceAction(038, hierophant, 1);
        queueVoiceAction(039, hierophant, 1);
        queueVoiceAction(040, hierophant, 1);
        queueVoiceAction(041, hierophant, 1);

        queueVoiceAction(042, hierophant, 1);
        queueVoiceAction(043, kerux, 1);

        queueVoiceAction(044, hierophant, 1);
        queueVoiceAction(045, hiereus, 1);
        queueVoiceAction(046, hegemon, 1);

        queueVoiceAction(047, hiereus, 1);
        queueVoiceAction(048, hegemon, 1);
        queueVoiceAction(049, hierophant, 1);

        queueVoiceAction(050, hegemon, 1);
        queueVoiceAction(051, hierophant, 1);
        queueVoiceAction(052, hiereus, 1);
    }

    void initReception()
    {

    }



    void queueReception()
    {
        queueVoiceActionR(001, hierophant, 1);
        queueVoiceActionR(002, hierophant, 1);

        // hegemon rises and removes his chair from between the pillars
        queue.add(new HideAction { actor = hegemonChair });

        // add the candidate in the anitochamber
        candidate = addActor(candidatePrefab, "Candidate Start");

        //queue.add(ExecuteVoidFunctionAction.create(attachCameraToCandidate, "attachCameraToCandidate"));

        removeBlindfold();

        // hegemon enters the antichamber and kerux shuts the door
        fastForwardTo = queue.add(AllAction.create(
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
        
        queueVoiceActionR(003, kerux, 1);
        queueVoiceActionR(004, hierophant, 1);
        queueVoiceActionR(005, hierophant, 1);
        queueVoiceActionR(006, hierophant, 1);



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
        queueVoiceActionR(007, hegemon, 1);

        queue.add(AllAction.create(
            MoveAction.create("Inside Door 1", candidate),
            MoveAction.create("Inside Door 2", hegemon)
        ));

        queue.add(MoveAction.createNoRotate("Door Close", door));


        queueVoiceActionR(008, stolistes, 1);
        queueVoiceActionR(009, dadouchos, 1);
        queueVoiceActionR(010, hierophant, 1);
        queueVoiceActionR(011, kerux, 1);

        // S comes forward and dipping his thumb in the lustral water, makes with it a cross on the candidate's brow and sprikles him three times, saying:
        queue.add(MoveAction.create("Facing Candidate", stolistes));
        queueVoiceActionR(012, stolistes, 1);
        queue.add(MoveAction.create("enter stolistes", stolistes));

        // D comes forward and makes a Cross over candidate with his censer, and waving it three times says:
        queue.add(MoveAction.create("Facing Candidate", dadouchos));
        queueVoiceActionR(013, dadouchos, 1);
        queue.add(MoveAction.create("enter dadouchos", dadouchos));

        queueVoiceActionR(014, hierophant, 1);

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

        queueVoiceActionR(015, hierophant, 1);
        queueVoiceActionR(016, hegemon, 1);
        queueVoiceActionR(017, hierophant, 1);
        queueVoiceActionR(018, hegemon, 1);
        queueVoiceActionR(019, hierophant, 1);
        queueVoiceActionR(020, hegemon, 1);
        queueVoiceActionR(021, hierophant, 1);

        queue.add(MoveAction.create("altar hierophant", hierophant));

        queueVoiceActionR(022, hierophant, 1);
        queueVoiceActionR(023, hierophant, 1);
        queueVoiceActionR(024, hierophant, 1);
        queueVoiceActionR(025, hierophant, 1);
        queueVoiceActionR(026, hierophant, 1);
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
        followBase = head.transform.position;
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

    private ScriptAction queueVoiceActionR(int clipId, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return queueVoiceAction(getClipR(clipId), actor, waitAfter, waitBefore);
    }
    private ScriptAction queueVoiceAction(int clipId, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return queueVoiceAction(getClip(clipId), actor, waitAfter, waitBefore);
    }
    private ScriptAction queueVoiceAction(AudioClip clip, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return queue.add(VoiceAction.create(clip = clip, actor = actor, waitAfter = waitAfter, waitBefore = waitBefore ));
    }

    private AudioClip getClip(int id)
    {
        return getClip(id, clips);
    }
    private AudioClip getClipR(int id)
    {
        return getClip(id, receptionClips);
    }
    private AudioClip getClip(int id, AudioClip[] source)
    {
        return source[id - 1];
    }

    private ScriptAction createVoiceAction(int clipId, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return VoiceAction.create(getClip(clipId), actor, waitAfter, waitBefore);
    }

    private ScriptAction createStandAction(string markName, GameObject actor, float waitAfter = 1f)
    {
        return new MoveAction { markName = markName + " Stand", actor = actor, speed = UnityEngine.Random.RandomRange(0.5f, 1), waitAfter = waitAfter, waitBefore = UnityEngine.Random.RandomRange(0.1f, 1) };
    }

}
