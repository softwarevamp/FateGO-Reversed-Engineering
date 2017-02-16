using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Internal/Property Binding")]
public class PropertyBinding : MonoBehaviour
{
    public Direction direction;
    public bool editMode = true;
    private object mLastValue;
    public PropertyReference source;
    public PropertyReference target;
    public UpdateCondition update = UpdateCondition.OnUpdate;

    private void FixedUpdate()
    {
        if (this.update == UpdateCondition.OnFixedUpdate)
        {
            this.UpdateTarget();
        }
    }

    private void LateUpdate()
    {
        if (this.update == UpdateCondition.OnLateUpdate)
        {
            this.UpdateTarget();
        }
    }

    private void OnValidate()
    {
        if (this.source != null)
        {
            this.source.Reset();
        }
        if (this.target != null)
        {
            this.target.Reset();
        }
    }

    private void Start()
    {
        this.UpdateTarget();
        if (this.update == UpdateCondition.OnStart)
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        if (this.update == UpdateCondition.OnUpdate)
        {
            this.UpdateTarget();
        }
    }

    [ContextMenu("Update Now")]
    public void UpdateTarget()
    {
        if (((this.source != null) && (this.target != null)) && (this.source.isValid && this.target.isValid))
        {
            if (this.direction == Direction.SourceUpdatesTarget)
            {
                this.target.Set(this.source.Get());
            }
            else if (this.direction == Direction.TargetUpdatesSource)
            {
                this.source.Set(this.target.Get());
            }
            else if (this.source.GetPropertyType() == this.target.GetPropertyType())
            {
                object obj2 = this.source.Get();
                if ((this.mLastValue == null) || !this.mLastValue.Equals(obj2))
                {
                    this.mLastValue = obj2;
                    this.target.Set(obj2);
                }
                else
                {
                    obj2 = this.target.Get();
                    if (!this.mLastValue.Equals(obj2))
                    {
                        this.mLastValue = obj2;
                        this.source.Set(obj2);
                    }
                }
            }
        }
    }

    public enum Direction
    {
        SourceUpdatesTarget,
        TargetUpdatesSource,
        BiDirectional
    }

    public enum UpdateCondition
    {
        OnStart,
        OnUpdate,
        OnLateUpdate,
        OnFixedUpdate
    }
}

