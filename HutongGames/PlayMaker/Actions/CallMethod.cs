namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Reflection;
    using UnityEngine;

    [ActionCategory(ActionCategory.ScriptControl)]
    public class CallMethod : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Store the component in an Object variable.\nNOTE: Set theObject variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera."), ObjectType(typeof(MonoBehaviour))]
        public FsmObject behaviour;
        private UnityEngine.Object cachedBehaviour;
        private MethodInfo cachedMethodInfo;
        private ParameterInfo[] cachedParameterInfo;
        private System.Type cachedType;
        private string errorString;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Name of the method to call on the component")]
        public FsmString methodName;
        [HutongGames.PlayMaker.Tooltip("Method paramters. NOTE: these must match the method's signature!")]
        public FsmVar[] parameters;
        private object[] parametersArray;
        [UIHint(UIHint.Variable), ActionSection("Store Result"), HutongGames.PlayMaker.Tooltip("Store the result of the method call.")]
        public FsmVar storeResult;

        private bool DoCache()
        {
            this.cachedBehaviour = this.behaviour.Value as MonoBehaviour;
            if (this.cachedBehaviour == null)
            {
                this.errorString = this.errorString + "Behaviour is invalid!\n";
                base.Finish();
                return false;
            }
            this.cachedType = this.behaviour.Value.GetType();
            this.cachedMethodInfo = this.cachedType.GetMethod(this.methodName.Value);
            if (this.cachedMethodInfo == null)
            {
                this.errorString = this.errorString + "Method Name is invalid: " + this.methodName.Value + "\n";
                base.Finish();
                return false;
            }
            this.cachedParameterInfo = this.cachedMethodInfo.GetParameters();
            return true;
        }

        private void DoMethodCall()
        {
            if (this.behaviour.Value == null)
            {
                base.Finish();
            }
            else
            {
                if (this.cachedBehaviour != this.behaviour.Value)
                {
                    this.errorString = string.Empty;
                    if (!this.DoCache())
                    {
                        Debug.LogError(this.errorString);
                        base.Finish();
                        return;
                    }
                }
                object obj2 = null;
                if (this.cachedParameterInfo.Length == 0)
                {
                    obj2 = this.cachedMethodInfo.Invoke(this.cachedBehaviour, null);
                }
                else
                {
                    for (int i = 0; i < this.parameters.Length; i++)
                    {
                        FsmVar var = this.parameters[i];
                        var.UpdateValue();
                        this.parametersArray[i] = var.GetValue();
                    }
                    obj2 = this.cachedMethodInfo.Invoke(this.cachedBehaviour, this.parametersArray);
                }
                this.storeResult.SetValue(obj2);
            }
        }

        public override string ErrorCheck()
        {
            this.errorString = string.Empty;
            this.DoCache();
            if (!string.IsNullOrEmpty(this.errorString))
            {
                return this.errorString;
            }
            if (this.parameters.Length != this.cachedParameterInfo.Length)
            {
                object[] objArray1 = new object[] { "Parameter count does not match method.\nMethod has ", this.cachedParameterInfo.Length, " parameters.\nYou specified ", this.parameters.Length, " paramaters." };
                return string.Concat(objArray1);
            }
            for (int i = 0; i < this.parameters.Length; i++)
            {
                FsmVar var = this.parameters[i];
                System.Type realType = var.RealType;
                System.Type parameterType = this.cachedParameterInfo[i].ParameterType;
                if (!object.ReferenceEquals(realType, parameterType))
                {
                    object[] objArray2 = new object[] { "Parameters do not match method signature.\nParameter ", i + 1, " (", realType, ") should be of type: ", parameterType };
                    return string.Concat(objArray2);
                }
            }
            if (object.ReferenceEquals(this.cachedMethodInfo.ReturnType, typeof(void)))
            {
                if (!string.IsNullOrEmpty(this.storeResult.variableName))
                {
                    return "Method does not have return.\nSpecify 'none' in Store Result.";
                }
            }
            else if (!object.ReferenceEquals(this.cachedMethodInfo.ReturnType, this.storeResult.RealType))
            {
                return ("Store Result is of the wrong type.\nIt should be of type: " + this.cachedMethodInfo.ReturnType);
            }
            return string.Empty;
        }

        public override void OnEnter()
        {
            this.parametersArray = new object[this.parameters.Length];
            this.DoMethodCall();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoMethodCall();
        }
    }
}

