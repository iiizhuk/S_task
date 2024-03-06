using Autofac;

namespace FunctionalFitting
{
    public sealed class ProjectModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FittingModelResolver>().As<IFittingModelResolver>();

            builder.RegisterType<LinearFittingModelAlgorithm>().As<IFittingModelAlgorithm>();
            builder.RegisterType<PowerFittingModelAlgorithm>().As<IFittingModelAlgorithm>();
            builder.RegisterType<ExponentialFittingModelAlgorithm>().As<IFittingModelAlgorithm>();
        }
    }
}
