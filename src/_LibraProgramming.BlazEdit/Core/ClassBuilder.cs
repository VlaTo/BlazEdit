using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LibraProgramming.BlazEdit.Components;

namespace LibraProgramming.BlazEdit.Core
{
    internal static class ClassBuilder
    {
        public static ClassBuilder<TComponent> CreateFor<TComponent>(string classNamePrefix, string componentPrefix = null)
            where TComponent : ToolComponent
        {
            return new ClassBuilder<TComponent>(classNamePrefix, componentPrefix);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    internal class ClassBuilder<TComponent>
        where TComponent : ToolComponent
    {
        private const char ClassNameSeparator = ' ';
        private const string DashSeparator = "-";

        private readonly string classNamePrefix;
        private readonly StringBuilder builder;
        private readonly IList<ClassDefinition> classDefinitions;

        public ClassBuilder(string classNamePrefix, string componentPrefix = default(string))
        {
            this.classNamePrefix = classNamePrefix;

            builder = new StringBuilder();
            classDefinitions = new List<ClassDefinition>();

            if (false == String.IsNullOrWhiteSpace(componentPrefix))
            {
                if (false == String.IsNullOrWhiteSpace(this.classNamePrefix))
                {
                    this.classNamePrefix += (DashSeparator + componentPrefix);
                }
                else
                {
                    throw new ArgumentException("", nameof(classNamePrefix));
                }
            }
        }

        public string Build(TComponent component, string extras = "", bool addClassNamePrefix = true)
        {
            if (0 == classDefinitions.Count)
            {
                return String.Empty;
            }

            builder.Clear();

            if (addClassNamePrefix)
            {
                builder.Append(classNamePrefix);
            }

            foreach (var definition in classDefinitions)
            {
                if (false == definition.Condition.Invoke(component))
                {
                    continue;
                }

                if (0 < builder.Length)
                {
                    builder.Append(ClassNameSeparator);
                }

                var hasPrefix = false == String.IsNullOrWhiteSpace(definition.Prefix);

                if (hasPrefix)
                {
                    builder.Append(definition.Prefix);
                }

                var count = 0;

                foreach (var modifier in definition.Modifiers)
                {
                    if (false == modifier.Condition.Invoke(component))
                    {
                        continue;
                    }

                    if (0 < count || hasPrefix)
                    {
                        builder.Append(DashSeparator);
                    }

                    var value = modifier.Accessor.Invoke(component);

                    builder.Append(value);
                    count++;
                }

                if (0 < definition.Modifiers.Count || hasPrefix)
                {
                    builder.Append(definition.PrefixSeparator);
                }

                builder.Append(definition.Accessor.Invoke(component));
            }

            if (false == String.IsNullOrWhiteSpace(extras))
            {
                builder.Append(ClassNameSeparator).Append(extras);
            }

            return builder.ToString();
        }

        public ClassBuilder<TComponent> DefineClass(Action<IClassBuilder<TComponent>> configurator)
        {
            if (null == configurator)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            var classBuilder = new InternalClassBuilder();

            configurator.Invoke(classBuilder);

            classDefinitions.Add(classBuilder.Build(classNamePrefix));

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        private class ClassDefinition
        {
            public string Prefix
            {
                get;
            }

            public Func<TComponent, string> Accessor
            {
                get;
            }

            public IReadOnlyList<ClassModifier> Modifiers
            {
                get;
            }

            public Predicate<TComponent> Condition
            {
                get;
            }

            public string PrefixSeparator
            {
                get;
            }

            public ClassDefinition(
                string prefix,
                Func<TComponent, string> accessor,
                IList<ClassModifier> modifiers,
                Predicate<TComponent> condition,
                string prefixSeparator)
            {
                Prefix = prefix;
                Accessor = accessor;
                Modifiers = new ReadOnlyCollection<ClassModifier>(modifiers);
                Condition = condition;
                PrefixSeparator = prefixSeparator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class ClassModifier
        {
            public Func<TComponent, string> Accessor
            {
                get;
            }

            public Predicate<TComponent> Condition
            {
                get;
            }

            public ClassModifier(Func<TComponent, string> accessor, Predicate<TComponent> condition)
            {
                Accessor = accessor;
                Condition = condition;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class InternalClassBuilder : IClassBuilder<TComponent>
        {
            private bool noPrefix;
            private Predicate<TComponent> condition;
            private Func<TComponent, string> accessor;
            private readonly IList<ClassModifier> modifiers;

            public InternalClassBuilder()
            {
                noPrefix = false;
                modifiers = new List<ClassModifier>();
            }

            public IClassBuilder<TComponent> NoPrefix()
            {
                noPrefix = true;
                return this;
            }

            public IClassBuilder<TComponent> Modifier(Func<TComponent, string> func, Predicate<TComponent> predicate)
            {
                modifiers.Add(new ClassModifier(func, predicate));
                return this;
            }

            public IClassBuilder<TComponent> Name(Func<TComponent, string> valueAccessor)
            {
                accessor = valueAccessor;
                return this;
            }

            public IClassBuilder<TComponent> Condition(Predicate<TComponent> predicate)
            {
                if (null == predicate)
                {
                    throw new ArgumentNullException(nameof(predicate));
                }

                if (null != condition)
                {
                    throw new ArgumentException("", nameof(predicate));
                }

                condition = predicate;

                return this;
            }

            internal ClassDefinition Build(string prefix)
            {
                return new ClassDefinition(
                    false == noPrefix ? prefix : null,
                    accessor,
                    modifiers,
                    condition ?? True,
                    DashSeparator
                );
            }

            private static bool True(TComponent _) => true;
        }
    }
}