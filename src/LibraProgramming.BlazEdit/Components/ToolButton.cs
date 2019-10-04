using System;
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
    /// <summary>
    /// 
    /// </summary>
    public class ToolButton : ToolComponent, IMessageHandler<CommandMessage>
    {
        private static readonly ClassBuilder<ToolButton> classBuilder;
        private readonly EventCallback<MouseEventArgs> clickCallback;
        private bool hasRendered;
        private IDisposable subscription;

        [Parameter]
        public bool Enabled
        {
            get; 
            set;
        }

        [Parameter]
        public IToolCommand Command
        {
            get; 
            set;
        }

        [Parameter]
        public string Tooltip
        {
            get; 
            set;
        }

        [Inject]
        public IMessageAggregator MessageAggregator
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

        protected string ClassString
        {
            get; 
            private set;
        }

        public ToolButton()
        {
            clickCallback = EventCallback.Factory.Create<MouseEventArgs>(this, DoClickAsync);
            Enabled = true;
        }

        static ToolButton()
        {
            classBuilder = ClassBuilder.CreateFor<ToolButton>(null)
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
                );
        }

        Task IMessageHandler<CommandMessage>.HandleAsync(CommandMessage message)
        {
            if (ReferenceEquals(message.Command, Command))
            {
                UpdateClassString();
            }

            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            subscription = MessageAggregator.Subscribe(this);

            UpdateClassString();
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
            builder.AddAttribute(8, "onclick", clickCallback);

            builder.AddContent(9, ChildContent);

            builder.CloseElement();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
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

        private void UpdateClassString()
        {
            ClassString = classBuilder.Build(this);

            if (hasRendered)
            {
                StateHasChanged();
            }
        }

        private async Task DoClickAsync(MouseEventArgs e)
        {
            var command = Command;

            if (null != command && command.CanInvoke())
            {
                await command.InvokeAsync();
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