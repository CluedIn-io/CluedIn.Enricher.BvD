using System.Reflection;
using Castle.MicroKernel.Registration;
using CluedIn.Core;
using CluedIn.Core.Providers;
using CluedIn.Core.Server;
using ComponentHost;
using Constants = CluedIn.ExternalSearch.Providers.BvD.Constants;

namespace CluedIn.Provider.BvD;

[Component(Constants.ComponentName, "Providers", ComponentType.Service, ServerComponents.ProviderWebApi,
    Components.Server, Components.DataStores, Isolation = ComponentIsolation.NotIsolated)]
public sealed class BvDProviderProviderComponent : ServiceApplicationComponent<IServer>
{
    /**********************************************************************************************************
     * CONSTRUCTOR
     **********************************************************************************************************/

    /// <summary>
    ///     Initializes a new instance of the <see cref="BvDProviderProviderComponent" /> class.
    /// </summary>
    /// <param name="componentInfo">The component information.</param>
    public BvDProviderProviderComponent(ComponentInfo componentInfo) : base(componentInfo)
    {
        // Dev. Note: Potential for compiler warning here ... CA2214: Do not call overridable methods in constructors
        //   this class has been sealed to prevent the CA2214 waring being raised by the compiler
        Container.Register(Component.For<BvDProviderProviderComponent>().Instance(this));
    }

    /**********************************************************************************************************
     * METHODS
     **********************************************************************************************************/

    /// <summary>Starts this instance.</summary>
    public override void Start()
    {
        var asm = Assembly.GetAssembly(typeof(BvDProviderProviderComponent));
        Container.Register(Types.FromAssembly(asm).BasedOn<IProvider>().WithServiceFromInterface()
            .If(t => !t.IsAbstract).LifestyleSingleton());

        State = ServiceState.Started;
    }

    /// <summary>Stops this instance.</summary>
    public override void Stop()
    {
        if (State == ServiceState.Stopped)
        {
            return;
        }

        State = ServiceState.Stopped;
    }
}
