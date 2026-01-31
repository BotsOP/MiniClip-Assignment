using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Components.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton<Injector>
    {
        private const BindingFlags k_bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

        protected override void Awake()
        {
            base.Awake();

            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (IDependencyProvider provider in providers)
            {
                RegisterProvider(provider);
            }

            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (MonoBehaviour monoBehaviour in injectables)
            {
                Inject(monoBehaviour);
            }
        }
    
        void Inject(object instance)
        {
            Type type = instance.GetType();
            IEnumerable<FieldInfo> injectableFields = type.GetFields(k_bindingFlags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (FieldInfo injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to inject {fieldType.Name} into {type.Name}");
                }
            
                injectableField.SetValue(instance, resolvedInstance);
                Debug.Log($"Field Injected {fieldType.Name} into {type.Name}");
            }

            var injectableMethods = type.GetMethods(k_bindingFlags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (MethodInfo injectableMethod in injectableMethods)
            {
                Type[] requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                object[] resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    throw new Exception($"Failed to inject {type.Name}.{injectableMethod.Name}");
                }
            
                injectableMethod.Invoke(instance, resolvedInstances);
                Debug.Log($"Method Injected {type.Name}.{injectableMethod.Name}");
            }
        }

        object Resolve(Type type)
        {
            if (!registry.TryGetValue(type, out object resolvedInstance))
            {
                throw new Exception($"Could not find provider for {type.Name} in scene");
            }
            return resolvedInstance;
        }

        static bool IsInjectable(MonoBehaviour obj)
        {
            MemberInfo[] members = obj.GetType().GetMembers(k_bindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider provider)
        {
            MethodInfo[] methods = provider.GetType().GetMethods(k_bindingFlags);

            foreach (MethodInfo method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute)))
                {
                    continue;
                }
            
                Type returnType = method.ReturnType;
                object providedInstance = method.Invoke(provider, null);
                if (registry.ContainsKey(returnType))
                {
                    throw new Exception($"Duplicate provider for {returnType.Name}");
                }
                if (providedInstance != null)
                {
                    registry.Add(returnType, providedInstance);
                    Debug.Log($"Registered {returnType.Name} from {provider.GetType().Name}");
                }
                else
                {
                    throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}");
                }
            }
        }

        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}
