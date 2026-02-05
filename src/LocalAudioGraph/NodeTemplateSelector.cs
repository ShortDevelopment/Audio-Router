using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace VBAudioRouter.LocalAudioGraph;

public partial class NodeTemplateSelector : DataTemplateSelector
{
    public TemplateLookup Templates { get; set; } = [];

    protected override DataTemplate SelectTemplateCore(object item)
        => Templates[(Type)item];
}

public sealed partial class TemplateLookup : Dictionary<Type, DataTemplate>;
