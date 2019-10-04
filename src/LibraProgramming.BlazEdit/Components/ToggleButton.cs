using System;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using Microsoft.AspNetCore.Components;

namespace LibraProgramming.BlazEdit.Components
{
    public sealed class ToggleButton : ToolButton, IMessageHandler<ToggleButtonMessage>
    {
        [Parameter]
        public bool IsToggled
        {
            get; 
            set;
        }

        [Parameter]
        public string GroupName
        {
            get; 
            set;
        }

        public ToggleButton()
        {
        }

        Task IMessageHandler<ToggleButtonMessage>.HandleAsync(ToggleButtonMessage message)
        {
            if (ReferenceEquals(message.Button, this))
            {
                return Task.CompletedTask;
            }

            if (false == String.IsNullOrEmpty(GroupName))
            {
                if (String.Equals(GroupName, message.Button.GroupName))
                {
                    IsToggled = false;
                    //StateHasChanged();
                }
            }

            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (false == String.IsNullOrEmpty(GroupName))
            {
                MessageAggregator.Publish(new ToggleButtonMessage(this));
            }
        }
    }
}