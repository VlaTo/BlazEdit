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
    public sealed class ToolButton : ToolButtonBase, IMessageHandler<CommandMessage>, IToolButton
    {
        private static readonly ClassBuilder<ToolButton> ButtonClassBuilder;
        private IDisposable subscription;


        [Parameter]
        public IToolCommand Command
        {
            get; 
            set;
        }

        public ToolButton() 
            : base(ButtonClassBuilder)
        {
        }

        static ToolButton()
        {
            ButtonClassBuilder = ClassBuilder
                .CreateFor<ToolButton>(null)
                .DefineClass(@class => @class.NoPrefix().Name("editor-toolbar-item"))
                .DefineClass(@class => @class.NoPrefix().Name("toolbar-button"))
                .DefineClass(@class => @class.NoPrefix().Name("disabled").Condition(component => component.IsDisabled()));
        }

        Task IMessageHandler<CommandMessage>.HandleAsync(CommandMessage message)
        {
            if (ReferenceEquals(message.Command, Command))
            {
                UpdateState();
            }

            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            subscription = MessageDispatcher.Subscribe(this);
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

        protected override void OnDispose() => subscription.Dispose();

        protected override async Task DoClickAsync(MouseEventArgs e)
        {
            var command = Command;

            if (null != command && command.CanInvoke())
            {
                await command.InvokeAsync();
            }

            await base.DoClickAsync(e);
        }

        protected override bool IsDisabled() => base.IsDisabled() || (null != Command && false == Command.CanInvoke());
    }
}