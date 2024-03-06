using Autofac;
using WpfChart.GUI.ViewModels;

namespace WpfChart.GUI;

internal sealed class MainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MainWindowViewModel>().AsSelf();
    }
}