using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Commands;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using LibraProgramming.BlazEdit.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace LibraProgramming.BlazEdit.Components
{
    public sealed class ToggleButton : ToolComponent, IMessageHandler<ToggleButtonMessage>, IObserver<IToolCommand>
    {
        private static readonly ClassBuilder<ToggleButton> classBuilder;
        private readonly EventCallback<MouseEventArgs> clickCallback;
        private string classString;
        private bool hasRendered;
        private IDisposable subscription;
        private IDisposable commanDisposable;
        private bool toggled;
        private bool enabled;
        private IObservableToolCommand command;

        [Parameter]
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value)
                {
                    return;
                }

                enabled = value;

                UpdateState();
            }
        }

        [Parameter]
        public IObservableToolCommand Command
        {
            get => command;
            set
            {
                if (ReferenceEquals(command, value))
                {
                    return;
                }

                if (null != command)
                {
                    commanDisposable?.Dispose();
                    commanDisposable = null;
                }

                command = value;

                if (null != command)
                {
                    commanDisposable = command.Subscribe(this);
                }
            }
        }

        [Parameter]
        public string Tooltip
        {
            get;
            set;
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

        [Inject]
        public IMessageDispatcher MessageDispatcher
        {
            get;
            set;
        }

        [Parameter]
        public RenderFragment ChildContent
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick
        {
            get;
            set;
        }

        public ToggleButton()
        {
            clickCallback = EventCallback.Factory.Create<MouseEventArgs>(this, DoClickAsync);
            Enabled = true;
        }

        static ToggleButton()
        {
            classBuilder = ClassBuilder.CreateFor<ToggleButton>(null)
                .DefineClass(@class => @class
                    .Name("editor-toolbar-item")
                    .NoPrefix()
                )
                .DefineClass(@class => @class
                    .Name("toolbar-button")
                    .NoPrefix()
                )
                .DefineClass(@class => @class
                    .Name("disabled")
                    .NoPrefix()
                    .Condition(component => component.IsDisabled())
                )
                .DefineClass(@class => @class
                    .Name("toggled")
                    .NoPrefix()
                    .Condition(component => component.IsToggled)
                );
        }

        /*Task IMessageHandler<CommandMessage>.HandleAsync(CommandMessage message)
        {
            if (ReferenceEquals(message.Command, Command))
            {
                UpdateClassString();
            }

            return Task.CompletedTask;
        }*/

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

        void IObserver<IToolCommand>.OnCompleted()
        {
            commanDisposable = null;
        }

        void IObserver<IToolCommand>.OnError(Exception error)
        {
            ;
        }

        void IObserver<IToolCommand>.OnNext(IToolCommand value)
        {
            if (ReferenceEquals(value, Command))
            {
                IsToggled = Command.IsApplied;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            subscription = MessageDispatcher.Subscribe(this);

            UpdateState();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(1, "button");
            builder.AddAttribute(2, "class", classString);
            builder.AddAttribute(3, "type", "button");
            builder.AddAttribute(4, "role", "button");

            if (false == String.IsNullOrEmpty(Tooltip))
            {
                builder.AddAttribute(5, "aria-label", Tooltip);
                builder.AddAttribute(6, "title", Tooltip);
            }

            builder.AddAttribute(7, "disabled", IsDisabled());
            builder.AddAttribute(8, "onclick", clickCallback);

            builder.AddContent(9, ChildContent);

            builder.CloseElement();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                Debug.WriteLine("OnAfterRender");
                hasRendered = true;
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override void OnDispose()
        {
            subscription.Dispose();
        }

        private void UpdateState()
        {
            classString = classBuilder.Build(this);

            Debug.WriteLine($"Class string: {classString}");

            if (hasRendered)
            {
                Debug.WriteLine("StateHasRendered");
                StateHasChanged();
            }
        }

        private async Task DoClickAsync(MouseEventArgs e)
        {
            var action = Command;

            if (null != action && action.CanInvoke())
            {
                await action.InvokeAsync();
            }

            await OnClick.InvokeAsync(e);
        }

        private bool IsDisabled()
        {
            if (false == Enabled)
            {
                return true;
            }

            if (null != Command)
            {
                return false == Command.CanInvoke();
            }

            return false;
        }
    }
}