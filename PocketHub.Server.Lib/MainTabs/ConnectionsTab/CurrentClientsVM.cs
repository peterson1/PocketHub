using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.SDK.WPF45.ViewModelTools;

namespace PocketHub.Server.Lib.MainTabs.ConnectionsTab
{
    [ImplementPropertyChanged]
    public class CurrentClientsVM : R2ViewModelBase
    {
        public Observables<ClientRowVM> Rows { get; } = new Observables<ClientRowVM>();
    }
}
