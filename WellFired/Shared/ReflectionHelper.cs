namespace WellFired.Shared
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ReflectionHelper : IReflectionHelper
    {
        [DebuggerHidden]
        private IEnumerable GetBaseTypes(Type type) => 
            new <GetBaseTypes>c__Iterator43 { 
                type = type,
                <$>type = type,
                <>f__this = this,
                $PC = -2
            };

        public FieldInfo GetField(Type type, string name) => 
            type.GetField(name);

        public MethodInfo GetMethod(Type type, string name) => 
            type.GetMethod(name);

        public FieldInfo GetNonPublicInstanceField(Type type, string name) => 
            type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

        public PropertyInfo GetNonPublicInstanceProperty(Type type, string name) => 
            type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);

        public MethodInfo GetNonPublicMethod(Type type, string name) => 
            type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);

        public MethodInfo GetNonPublicStaticMethod(Type type, string name) => 
            type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);

        public PropertyInfo GetProperty(Type type, string name) => 
            type.GetProperty(name);

        public bool IsAssignableFrom(Type first, Type second) => 
            first.IsAssignableFrom(second);

        public bool IsEnum(Type type) => 
            type.IsEnum;

        public bool IsValueType(Type type) => 
            type.IsValueType;

        [CompilerGenerated]
        private sealed class <GetBaseTypes>c__Iterator43 : IEnumerator, IDisposable, IEnumerable, IEnumerator<object>, IEnumerable<object>
        {
            internal object $current;
            internal int $PC;
            internal Type <$>type;
            internal IEnumerator <$s_1742>__1;
            internal ReflectionHelper <>f__this;
            internal Type <baseType>__0;
            internal object <t>__2;
            internal Type type;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            IDisposable disposable = this.<$s_1742>__1 as IDisposable;
                            if (disposable == null)
                            {
                            }
                            disposable.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$current = this.type;
                        this.$PC = 1;
                        goto Label_00EF;

                    case 1:
                        this.<baseType>__0 = this.type.BaseType;
                        if (this.<baseType>__0 == null)
                        {
                            goto Label_00E6;
                        }
                        this.<$s_1742>__1 = this.<>f__this.GetBaseTypes(this.<baseType>__0).GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_00ED;
                }
                try
                {
                    while (this.<$s_1742>__1.MoveNext())
                    {
                        this.<t>__2 = this.<$s_1742>__1.Current;
                        this.$current = this.<t>__2;
                        this.$PC = 2;
                        flag = true;
                        goto Label_00EF;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    IDisposable disposable = this.<$s_1742>__1 as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
            Label_00E6:
                this.$PC = -1;
            Label_00ED:
                return false;
            Label_00EF:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new ReflectionHelper.<GetBaseTypes>c__Iterator43 { 
                    <>f__this = this.<>f__this,
                    type = this.<$>type
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

