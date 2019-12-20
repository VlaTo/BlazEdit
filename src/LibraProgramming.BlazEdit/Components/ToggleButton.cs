using LibraProgramming.BlazEdit.Commands;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using LibraProgramming.BlazEdit.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Components
{
    public sealed class ToggleButton : ToolButtonBase, IMessageHandler<ToggleButtonMessage>
    {
        private static readonly ClassBuilder<ToggleButton> ButtonClassBuilder;
        private readonly CompositeDisposable subscriptions;
        private IDisposable subscription;
        private bool toggled;
        private IToolCommand command;

        [Parameter]
        public IToolCommand Command
        {
            get => command;
            set
            {
                if (ReferenceEquals(command, value))
                {
                    return;
                }

                if (null != subscription)
                {
                    if (subscriptions.Remove(subscription))
                    {
                        subscription.Dispose();
                    }

                    subscription = null;
                }

                command = value;

                if (command is IObservableToolCommand observable)
                {
                    subscription = observable.Subscribe(OnCommandUpdated);
                    subscriptions.Add(subscription);
                }
            }
        }

        [Parameter]
        public bool IsToggled
        {
            get => toggled;
            set
            {
                if (toggled == value)
                {
                    return;
                }

                toggled = value;

                if (toggled)
                {
                    MessageDispatcher.Publish(new ToggleButtonMessage(this));
                }

                UpdateState();
            }
        }

        [Parameter]
        public string GroupName
        {
            get;
            set;
        }

        public ToggleButton()
            : base(ButtonClassBuilder)
        {
            toggled = false;
            subscriptions = new CompositeDisposable(4);
        }

        static ToggleButton()
        {
            ButtonClassBuilder = ClassBuilder
                .CreateFor<ToggleButton>(null)
                .DefineClass(@class => @class.NoPrefix().Name("editor-toolbar-item"))
                .DefineClass(@class => @class.NoPrefix().Name("toolbar-button"))
                .DefineClass(@class => @class.NoPrefix().Name("toggled").Condition(component => component.IsToggled))
                .DefineClass(@class => @class.NoPrefix().Name("disabled").Condition(component => component.IsDisabled()));
        }

        Task IMessageHandler<ToggleButtonMessage>.HandleAsync(ToggleButtonMessage message)
        {
            if (false == ReferenceEquals(message.Button, this))
            {
                var sameGroup = false == String.IsNullOrEmpty(GroupName) &&
                                String.Equals(GroupName, message.Button.GroupName);
                if (sameGroup)
                {
                    IsToggled = false;
                }
            }

            return Task.CompletedTask;
        }

        private void OnCommandUpdated(IToolCommand value)
        {
            if (ReferenceEquals(value, Command))
            {
                IsToggled = Command.IsApplied;
                Debug.WriteLine($"Toggle button IsToggled: {IsToggled}");
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            subscriptions.Add(MessageDispatcher.Subscribe(this));

            UpdateState();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(1, "button");
            builder.AddAttribute(2, "class", ClassString);
            builder.AddAttribute(3, "type", "button");
            builder.AddAttribute(4, "role", "button");

            if (false == String.IsNullOrEmpty(Tooltip))
            {
                builder.AddAttribute(5, "aria-label", Tooltip);
                builder.AddAttribute(6, "title", Tooltip);
            }

            builder.AddAttribute(7, "disabled", IsDisabled());
            builder.AddAttribute(8, "onclick", ClickCallback);

            builder.AddContent(9, ChildContent);

            builder.CloseElement();
        }

        protected override void OnDispose()
        {
            subscriptions.Dispose();
        }

        protected override async Task DoClickAsync(MouseEventArgs e)
        {
            var actor = Command;

            if (null != actor && actor.CanInvoke())
            {
                await actor.InvokeAsync();
            }

            await base.DoClickAsync(e);
        }

        protected override bool IsDisabled() => base.IsDisabled() || (null != Command && false == Command.CanInvoke());
    }
}