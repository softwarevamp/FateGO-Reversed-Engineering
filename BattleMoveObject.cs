using System;
using System.Collections;
using UnityEngine;

public class BattleMoveObject : BaseMonoBehaviour
{
    public Vector3 checkvec;
    public int count;
    public GameObject destoryobject;
    private bool dropflg;
    private bool moveflg;
    public BattlePerformance perf;
    private Rigidbody rigibody;
    private Spawner spawner;
    public Transform targetTr;
    public Vector3 targetvec;
    public TYPE type;

    public bool isMoveToItems() => 
        (this.type == TYPE.ITEM);

    public void onOpenComplete()
    {
        if (this.perf != null)
        {
            if (this.type == TYPE.ITEM)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.GET_ITEM);
                this.perf.updateDropItemCount();
            }
            else
            {
                SoundManager.playSe("ba12");
                this.perf.addCriticalPont(this.count);
            }
        }
        if (this.destoryobject != null)
        {
            base.createObject(this.destoryobject, base.gameObject.transform.parent, null).transform.position = base.transform.position;
        }
        this.spawner.Despawn(base.gameObject, false);
    }

    private void OnSpawn()
    {
        this.type = TYPE.NONE;
        this.dropflg = false;
        this.moveflg = false;
        this.targetTr = null;
        this.perf = null;
        this.destoryobject = null;
    }

    public void setAction(Vector3 vec)
    {
        this.checkvec = vec;
        this.checkvec.y += UnityEngine.Random.Range((float) 0.09f, (float) 0.12f);
        this.dropflg = true;
    }

    public void setParam(BattlePerformance perf, int count)
    {
        this.perf = perf;
        this.count = count;
    }

    public void setTarget(Vector3 vec)
    {
        this.targetvec = vec;
    }

    public void setTargetTr(Transform tr)
    {
        this.targetTr = tr;
    }

    public void setTypeItem()
    {
        this.type = TYPE.ITEM;
    }

    private void Start()
    {
        this.spawner = SingletonMonoBehaviour<Spawner>.Instance;
        this.rigibody = base.GetComponent<Rigidbody>();
    }

    public void startMoveTarget()
    {
        this.moveflg = true;
        float num = UnityEngine.Random.Range((float) 0.2f, (float) 0.4f);
        Hashtable args = new Hashtable();
        args.Clear();
        args.Add("position", this.targetTr.position);
        args.Add("easetype", iTween.EaseType.easeInBack);
        args.Add("oncomplete", "onOpenComplete");
        args.Add("time", num);
        iTween.MoveTo(base.gameObject, args);
    }

    private void Update()
    {
        if (this.dropflg && (base.gameObject.transform.position.y < this.checkvec.y))
        {
            base.gameObject.transform.position = new Vector3(base.gameObject.transform.position.x, this.checkvec.y, base.gameObject.transform.position.z);
            if (this.rigibody != null)
            {
                this.rigibody.velocity = Vector3.zero;
                this.rigibody.angularVelocity = Vector3.zero;
                this.rigibody.useGravity = false;
            }
            this.dropflg = false;
        }
        if (this.moveflg)
        {
        }
    }

    public enum TYPE
    {
        NONE,
        CRITICAL,
        ITEM
    }
}

