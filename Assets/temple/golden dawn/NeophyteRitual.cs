using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class NeophyteRitual : MonoBehaviour
{

    public ScriptAction fastForwardTo;

    public AudioClip knock;
    public AudioClip[] clips;
    
    public GameObject hierophantPrefab, hiereusPrefab, hegemonPrefab, keruxPrefab, stolistesPrefab, dadouchosPrefab;
    public GameObject imperatorPrefab, cancellariusPrefab, pastHierophantPrefab, praemonstratorPrefab;


    public GameObject doorPrefab, thronePrefab, altarPrefab;
    public GameObject blackPillarPrefab, whitePillarPrefab, bannerOfTheEastPrefab, bannerOfTheWestPrefab;

    private GameObject hierophant, hiereus, hegemon, kerux, stolistes, dadouchos;
    private Transform marks;

    private ScriptActionQueue queue = new ScriptActionQueue();

    private AudioClip getClip(int id)
    {
        return clips[id-1];
    }

    // Use this for initialization
    void Start()
    {
        hideMarks();

        var ground = transform.Find("Ground");
        marks = ground.transform.FindChild("Marks");

        hierophant = addActor(hierophantPrefab, "Hierophant Start");
        hiereus = addActor(hiereusPrefab, "Hiereus Start");
        hegemon = addActor(hegemonPrefab, "Hegemon Start");
        kerux = addActor(keruxPrefab, "Kerux Start");
        stolistes = addActor(stolistesPrefab, "Stolistes Start");
        dadouchos = addActor(dadouchosPrefab, "Dadouchos Start");

        addActor(doorPrefab, "Door");
        addActor(altarPrefab, "Altar");
        addActor(bannerOfTheEastPrefab, "Banner of the East");
        addActor(bannerOfTheWestPrefab, "Banner of the West");
        addActor(blackPillarPrefab, "Black Pillar");
        addActor(whitePillarPrefab, "White Pillar");

        addThrone(thronePrefab, "Hierophant Start");
        addThrone(thronePrefab, "Hiereus Start");
        addThrone(thronePrefab, "Hegemon Start");
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

        //var camera = "OVRCameraRig"
        //Camera.main.transform.parent = kerux.transform.FindChild("Head").transform;

        queueVoiceAction(knock, hierophant, 1, 2);

        queue.add(createCircleMoveAction("Kerux Proclaim", kerux));

        queueVoiceAction(001, kerux, 1);

        queue.add(createCircleMoveAction("Kerux Start", kerux));

        //queueVoiceAction(knock, hierophant, 1);
        queueVoiceAction(knock, hierophant, 1);

        queue.add(new AllAction
        {
            actions = new ScriptAction[] {
                createStandAction("Hierophant Start Throne", hierophant),
                createStandAction("Hegemon Start Throne", hegemon),
                createStandAction("Hiereus Start Throne", hiereus),
                createStandAction("Stolistes Start Throne", stolistes),
                createStandAction("Dadouchos Start Throne", dadouchos)
            }
        });

        queueVoiceAction(knock, hierophant, 1);
        queueVoiceAction(002, hierophant, 1);

        queue.add(createMoveAction("Facing Door", kerux));
        queueVoiceAction(knock, kerux, 1);
        queueVoiceAction(knock, kerux, 1);
        queue.add(createMoveAction("Kerux Start", kerux));
        queueVoiceAction(003, kerux, 1);
        // he [kerux] salutes the hierophant's throne. Remains by door

        queueVoiceAction(004, hierophant, 1);

        // hiereus goes to the door, stands before it with sword erect
        // kerux being on his right
        queue.add(createMoveAction("Hiereus Start Throne Stand", hiereus));

        queue.add(new AllAction
        {
            actions = new ScriptAction[] {
                createMoveAction("Kerux Standby", kerux),
                createCircleMoveAction("Kerux Start", hiereus)
            }
        });

        queueVoiceAction(005, hiereus, 1);

        queueVoiceAction(006, hiereus, 1);

        // hiereus and kerux return to their places
        queue.add(new AllAction
        {
            actions = new ScriptAction[] {
                createMoveAction("Kerux Start", kerux),
                createCircleMoveAction("Hiereus Start Throne Stand", hiereus)
            }
        });


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
        queue.add(all(new ScriptAction[] {
            createCircleMoveAction("East", stolistes),
            createCircleMoveAction("North", dadouchos) } ));
        queue.add(all(new ScriptAction[] { createCircleMoveAction("South", stolistes), createCircleMoveAction("East", dadouchos) } ));
        queue.add(all(new ScriptAction[] { createCircleMoveAction("West", stolistes), createCircleMoveAction("South", dadouchos) } ));
        queue.add(all(new ScriptAction[] { createCircleMoveAction("North", stolistes), createCircleMoveAction("West", dadouchos) } ));
        queue.add(all(new ScriptAction[] { createCircleMoveAction("East", stolistes), createCircleMoveAction("North", dadouchos) } ));


        queue.add(all(new ScriptAction[] {
            createVoiceAction(031, stolistes, 1)
                .then(createCircleMoveAction("Stolistes Start Throne Stand", stolistes)),
            createCircleMoveAction("East", dadouchos, 2)
                .then(createVoiceAction(035, dadouchos, 1)
                .then(createCircleMoveAction("Dadouchos Start Throne Stand", dadouchos)))
        } ));

        //fastForwardTo = 
        queueVoiceAction(036, hierophant, 1);

        queue.add(createCircleMoveToDegreeAction(315, kerux));
        queue.add(createCircleMoveToDegreeAction(300, hegemon, 60));
        queue.add(createCircleMoveToDegreeAction(285, hiereus));
        queue.add(createCircleMoveToDegreeAction(270, stolistes));
        fastForwardTo = queue.add(createCircleMoveToDegreeAction(255, dadouchos));

        queue.add(all(new ScriptAction[] {
            createCircleMoveToDegreeAction(314.9f, kerux, null, 0, 0),
            createCircleMoveToDegreeAction(299.9f, hegemon, null, 1, 0),
            createCircleMoveAction("Hiereus Start Throne Stand", hiereus, 2, 0),
            createCircleMoveToDegreeAction(269.9f, stolistes, null, 3, 0).then(createCircleMoveToDegreeAction(284.9f, stolistes, null, 0, 0)),
            createCircleMoveToDegreeAction(254.9f, dadouchos, null, 4, 0).then(createCircleMoveToDegreeAction(269.9f, dadouchos, null, 0, 0))
        }));

        queue.add(all(new ScriptAction[] {
            createCircleMoveToDegreeAction(314.8f, kerux, null, 0, 0),
            createCircleMoveToDegreeAction(70, hegemon, null, 1, 0).then(createMoveAction("Hegemon Start Throne Stand", hegemon)),
            createCircleMoveToDegreeAction(284.8f, stolistes, null, 2, 0).then(createCircleMoveToDegreeAction(299.8f, stolistes, null, 0, 0)),
            createCircleMoveToDegreeAction(269.8f, dadouchos, null, 3, 0).then(createCircleMoveToDegreeAction(284.8f, dadouchos, null, 0, 0))
        }));

        queue.add(all(new ScriptAction[] {
            createCircleMoveAction("Kerux Start", kerux, 0, 0),
            createCircleMoveAction("Stolistes Start Throne Stand", stolistes, 1, 0),
            createCircleMoveAction("Dadouchos Start Throne Stand", dadouchos, 2, 0)
        }));

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

    void Update()
    {

        queue.Update();

        if (fastForwardTo != null)
        {
            queue.fastForwardTo(fastForwardTo);
            fastForwardTo = null;
        }
        
    }


    private GameObject addActor(GameObject prefab, string markName)
    {
        var actor = Instantiate(prefab);
        actor.transform.parent = this.transform;

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
            Destroy(mr.gameObject);
        }
    }


    private ScriptAction queueVoiceAction(int clipId, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return queueVoiceAction(getClip(clipId), actor, waitAfter, waitBefore);
    }
    private ScriptAction queueVoiceAction(AudioClip clip, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return queue.add(new VoiceAction { clip = clip, actor = actor, waitAfter = waitAfter, waitBefore = waitBefore });
    }

    private ScriptAction createVoiceAction(int clipId, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return createVoiceAction(getClip(clipId), actor, waitAfter, waitBefore);
    }
    private ScriptAction createVoiceAction(AudioClip clip, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return new VoiceAction { clip = clip, actor = actor, waitAfter = waitAfter, waitBefore = waitBefore };
    }


    private ScriptAction createMoveAction(string markName, GameObject actor, float waitAfter = 1f)
    {
        return new MoveAction { markName = markName, actor = actor, speed = 2.0f, waitAfter = waitAfter };
    }

    private ScriptAction createStandAction(string markName, GameObject actor, float waitAfter = 1f)
    {
        return new MoveAction { markName = markName + " Stand", actor = actor, speed = UnityEngine.Random.RandomRange(0.5f, 1), waitAfter = waitAfter, waitBefore = UnityEngine.Random.RandomRange(0.1f, 1) };
    }


    private CircleMoveAction createCircleMoveAction(string targetMarkName, GameObject actor, float waitBefore = 0, float waitAfter = 1, float speed = 2)
    {
        return new CircleMoveAction { actor = actor, waitBefore = waitBefore, waitAfter = waitAfter, centerMarkName = "Altar", targetMarkName = targetMarkName, radiusMarkName = "Circumabulation", speed = speed };
    }

    private CircleMoveAction createCircleMoveToDegreeAction(float targetDegree, GameObject actor, float? entryDegree = null, float waitBefore = 0, float waitAfter = 1, float speed = 2)
    {
        return new CircleMoveAction { actor = actor, waitBefore = waitBefore, waitAfter = waitAfter, centerMarkName = "Altar", targetDegree = targetDegree, radiusMarkName = "Circumabulation", speed = speed, entryDegree = entryDegree };
        //return new CircleMoveToDegreeAction { actor = actor, waitAfter = waitAfter, centerMarkName = "Altar", targetDegree = targetDegree, radiusMarkName = "Circumabulation", speed = speed };
    }

    private AllAction all(ScriptAction[] actions)
    {
        return new AllAction { actions = actions };
    }

}
