namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Start a Coroutine in a Behaviour on a Game Object. See Unity StartCoroutine docs."), ActionCategory(ActionCategory.ScriptControl)]
    public class StartCoroutine : FsmStateAction
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map3B;
        [UIHint(UIHint.Behaviour), HutongGames.PlayMaker.Tooltip("The Behaviour that contains the method to start as a coroutine."), RequiredField]
        public FsmString behaviour;
        private MonoBehaviour component;
        [HutongGames.PlayMaker.Tooltip("The name of the coroutine method."), RequiredField, UIHint(UIHint.Coroutine)]
        public FunctionCall functionCall;
        [HutongGames.PlayMaker.Tooltip("The game object that owns the Behaviour."), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Stop the coroutine when the state is exited.")]
        public bool stopOnExit;

        private void DoStartCoroutine()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                this.component = ownerDefaultTarget.GetComponent(this.behaviour.Value) as MonoBehaviour;
                if (this.component == null)
                {
                    this.LogWarning("StartCoroutine: " + ownerDefaultTarget.name + " missing behaviour: " + this.behaviour.Value);
                }
                else
                {
                    string parameterType = this.functionCall.ParameterType;
                    if (parameterType != null)
                    {
                        int num;
                        if (<>f__switch$map3B == null)
                        {
                            Dictionary<string, int> dictionary = new Dictionary<string, int>(13) {
                                { 
                                    "None",
                                    0
                                },
                                { 
                                    "int",
                                    1
                                },
                                { 
                                    "float",
                                    2
                                },
                                { 
                                    "string",
                                    3
                                },
                                { 
                                    "bool",
                                    4
                                },
                                { 
                                    "Vector2",
                                    5
                                },
                                { 
                                    "Vector3",
                                    6
                                },
                                { 
                                    "Rect",
                                    7
                                },
                                { 
                                    "GameObject",
                                    8
                                },
                                { 
                                    "Material",
                                    9
                                },
                                { 
                                    "Texture",
                                    10
                                },
                                { 
                                    "Quaternion",
                                    11
                                },
                                { 
                                    "Object",
                                    12
                                }
                            };
                            <>f__switch$map3B = dictionary;
                        }
                        if (<>f__switch$map3B.TryGetValue(parameterType, out num))
                        {
                            switch (num)
                            {
                                case 0:
                                    this.component.StartCoroutine(this.functionCall.FunctionName);
                                    return;

                                case 1:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.IntParameter.Value);
                                    return;

                                case 2:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.FloatParameter.Value);
                                    return;

                                case 3:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.StringParameter.Value);
                                    return;

                                case 4:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.BoolParameter.Value);
                                    return;

                                case 5:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.Vector2Parameter.Value);
                                    return;

                                case 6:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.Vector3Parameter.Value);
                                    return;

                                case 7:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.RectParamater.Value);
                                    return;

                                case 8:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.GameObjectParameter.Value);
                                    return;

                                case 9:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.MaterialParameter.Value);
                                    break;

                                case 10:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.TextureParameter.Value);
                                    break;

                                case 11:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.QuaternionParameter.Value);
                                    break;

                                case 12:
                                    this.component.StartCoroutine(this.functionCall.FunctionName, this.functionCall.ObjectParameter.Value);
                                    return;
                            }
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoStartCoroutine();
            base.Finish();
        }

        public override void OnExit()
        {
            if ((this.component != null) && this.stopOnExit)
            {
                this.component.StopCoroutine(this.functionCall.FunctionName);
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.behaviour = null;
            this.functionCall = null;
            this.stopOnExit = false;
        }
    }
}

