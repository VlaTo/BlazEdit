using LibraProgramming.BlazEdit.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Components
{
    public class ToolButtonBase : ToolComponent
    {
        private readonly ClassBuilder classBuilder;
        private bool enabled;

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
        public string Tooltip
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

        protected string ClassString
        {
            get; 
            private set;
        }

        protected bool HasRendered
        {
            get;
            private set;
        }

        protected EventCallback<MouseEventArgs> ClickCallback
        {
            get;
        }

        protected ToolButtonBase(ClassBuilder classBuilder)
        {
            this.classBuilder = classBuilder;

            Enabled = true;
            HasRendered = false;
            ClickCallback = EventCallback.Factory.Create<MouseEventArgs>(this, DoClickAsync);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            UpdateState();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                HasRendered = true;
            }
        }

        protected virtual async Task DoClickAsync(MouseEventArgs e)
        {
            await OnClick.InvokeAsync(e);
        }

        protected virtual bool IsDisabled() => false == Enabled;

        protected void UpdateState()
        {
            ClassString = classBuilder.Build(this);
            
            if (HasRendered)
            {
                StateHasChanged();
            }
        }
    }
}