using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    public sealed class DropDown : ToolComponent, IMessageHandler<CommandMessage>
    {
        private static readonly ClassBuilder<DropDown> classBuilder;
        private readonly EventCallback<MouseEventArgs> clickCallback;
        private bool hasRendered;
        private IDisposable subscription;
        private ElementReference titleElement;

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
        public IEnumerable Items
        {
            get; 
            set;
        }

        [Parameter]
        public int SelectedIndex
        {
            get; 
            set;
        }

        [Parameter]
        public object SelectedValue
        {
            get;
            set;
        }

        [Parameter]
        public string TitleProperty
        {
            get; 
            set;
        }

        [Parameter]
        public string ValueProperty
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
        public EventCallback<MouseEventArgs> OnClick
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback<int> SelectedIndexChanged
        {
            get; 
            set;
        }

        [Parameter]
        public EventCallback<object> SelectedValueChanged
        {
            get; 
            set;
        }

        internal string ClassString
        {
            get;
            private set;
        }

        internal string SelectedTitle
        {
            get;
            private set;
        }

        public DropDown()
        {
            clickCallback = EventCallback.Factory.Create<MouseEventArgs>(this, DoClickAsync);

            Enabled = true;
            SelectedValue = null;
            SelectedIndex = -1;
        }

        static DropDown()
        {
            classBuilder = ClassBuilder.CreateFor<DropDown>(null)
                .DefineClass(@class => @class
                    .Name("editor-toolbar-item")
                    .NoPrefix()
                )
                .DefineClass(@class => @class
                    .Name("toolbar-selection")
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
            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            subscription = MessageDispatcher.Subscribe(this);

            UpdateClassString();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                hasRendered = true;
            }
        }

        protected override void OnDispose()
        {
            subscription.Dispose();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "tabindex", -1);
            builder.AddAttribute(3, "class", ClassString);
            builder.AddAttribute(4, "style", "user-select: none;");
            builder.AddAttribute(5, "onclick", clickCallback);

            BuildTitle(builder);
            BuildChevron(builder);

            builder.CloseElement();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (null != Items)
            {
                var enumerator = Items.GetEnumerator();
                var count = 0 > SelectedIndex ? 0 : SelectedIndex;

                while (enumerator.MoveNext() && 0 <= count)
                {
                    if (0 == count)
                    {
                        var item = enumerator.Current;
                        SelectedTitle = GetItemTitle(item);
                        //StateHasChanged();
                    }
                }
            }
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
            /*var command = Command;

            if (null != command && command.CanInvoke())
            {
                await command.InvokeAsync();
            }

            await OnClick.InvokeAsync(e);*/

            if (null != Items)
            {
                var selectionItems = new Collection<KeyValuePair<string, object>>();

                foreach (var item in Items)
                {
                    var title = GetItemTitle(item);
                    var value = GetItemValue(item);

                    selectionItems.Add(new KeyValuePair<string, object>(title, value));
                }
                
                MessageDispatcher.Publish(new RequireSelectionMessage(selectionItems.ToArray()));
            }
            else
            {
                Debug.WriteLine("No Items");
            }

            await Task.CompletedTask;
        }

        private Task UpdateSelectedIndexAsync(int value)
        {
            SelectedIndex = value;
            return SelectedIndexChanged.InvokeAsync(value);
        }

        private Task UpdateSelectedValueAsync(object value)
        {
            SelectedValue = value;
            return SelectedValueChanged.InvokeAsync(value);
        }

        private void BuildTitle(RenderTreeBuilder builder)
        {
            builder.AddElementReferenceCapture(6, reference => titleElement = reference);

            builder.OpenElement(5, "span");
            builder.AddAttribute(7, "class", "selection-label");

            builder.AddContent(8, SelectedTitle);

            builder.CloseElement();
        }

        private static void BuildChevron(RenderTreeBuilder builder)
        {
            builder.OpenElement(9, "div");
            {
                builder.OpenElement(10, "svg");
                builder.AddAttribute(11, "width", 10);
                builder.AddAttribute(12, "height", 10);

                {
                    builder.OpenElement(13, "path");
                    builder.AddAttribute(14, "d", "M8.7 2.2c.3-.3.8-.3 1 0 .4.4.4.9 0 1.2L5.7 7.8c-.3.3-.9.3-1.2 0L.2 3.4a.8.8 0 0 1 0-1.2c.3-.3.8-.3 1.1 0L5 6l3.7-3.8z");
                    builder.AddAttribute(15, "fill-rule", "nonzero");

                    builder.CloseElement();
                }

                builder.CloseElement();
            }

            builder.CloseElement();
        }

        private string GetItemTitle(object item)
        {
            if (null == item)
            {
                return null;
            }

            var itemType = item.GetType();
            var titleProperty = itemType.GetProperty(
                TitleProperty ?? "Title",
                BindingFlags.Instance | BindingFlags.Public
            );

            if (null == titleProperty)
            {
                return null;
            }

            var value = titleProperty.GetValue(item) ?? String.Empty;

            return value.ToString();
        }
        private string GetItemValue(object item)
        {
            if (null == item)
            {
                return null;
            }

            var itemType = item.GetType();
            var titleProperty = itemType.GetProperty(
                TitleProperty ?? "Title",
                BindingFlags.Instance | BindingFlags.Public
            );

            if (null == titleProperty)
            {
                return null;
            }

            var value = titleProperty.GetValue(item) ?? String.Empty;

            return value.ToString();
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