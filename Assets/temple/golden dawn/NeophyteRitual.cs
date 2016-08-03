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

    private float[] lineUp = { 315, 300, 285, 270, 255, 240, 225 };



    // Use this for initialization
    void Start()
    {
        knock = Resources.Load<AudioClip>("knock");
        if (knock == null) throw new Exception("can't load knock");

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

        fastForwardTo = queue.add(AllAction.create(
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

        queueVoiceAction("r_30", stolistes);
        queueVoiceAction("r_31", dadouchos);
        queueVoiceAction("r_32", hegemon);
        queueVoiceAction("r_33", hiereus);
        queueVoiceAction("r_34", hegemon);
        queueVoiceAction("r_35", hiereus);
        queueVoiceAction("r_36", kerux);
        queueVoiceAction("r_37", stolistes);
        queueVoiceAction("r_37.1", dadouchos);
        queueVoiceAction("r_38", hegemon);
        queueVoiceAction("r_39", hierophant);

        queueVoiceAction("r_40", hegemon);
        queueVoiceAction("r_41", hierophant);
        queueVoiceAction("r_42", hierophant);
        queueVoiceAction("r_43", hierophant);
        queueVoiceAction("r_44", hegemon);
        queueVoiceAction("r_45", hiereus);
        queueVoiceAction("r_46", hierophant);

        queueVoiceAction("r_47", hegemon);
        queueVoiceAction("r_48", hiereus);
        queueVoiceAction("r_49", hierophant);

        queueVoiceAction("r_50", hierophant);
        queueVoiceAction("r_51", hiereus);
        queueVoiceAction("r_52", hegemon);

        queueVoiceAction("r_53", hiereus);
        queueVoiceAction("r_54", hegemon);
        queueVoiceAction("r_55", hierophant);
        queueVoiceAction("r_56", hegemon);
        queueVoiceAction("r_57", hierophant);
        queueVoiceAction("r_58", hiereus);
        queueVoiceAction("r_59", hierophant);

        queueVoiceAction("r_60", hierophant);
        queueVoiceAction("r_61", hierophant);
        queueVoiceAction("r_62", hiereus);
        queueVoiceAction("r_63", hiereus);
        queueVoiceAction("r_64", hiereus);
        queueVoiceAction("r_65", hiereus);
        queueVoiceAction("r_66", hiereus);
        queueVoiceAction("r_67", hiereus);
        queueVoiceAction("r_68", hiereus);
        queueVoiceAction("r_69", hiereus);

        queueVoiceAction("r_70", hiereus);
        queueVoiceAction("r_71", hiereus);
        queueVoiceAction("r_72", hiereus);
        queueVoiceAction("r_73", hiereus);
        queueVoiceAction("r_74", hiereus);
        queueVoiceAction("r_75", hiereus);
        queueVoiceAction("r_76", stolistes);
        queueVoiceAction("r_77", dadouchos);
        queueVoiceAction("r_78", hierophant);
        queueVoiceAction("r_79", hegemon);

        queueVoiceAction("r_80", hierophant);
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
