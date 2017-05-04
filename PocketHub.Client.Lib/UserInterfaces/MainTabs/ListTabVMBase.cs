using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.InputCommands;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.SDK.WPF45.ViewModelTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketHub.Client.Lib.UserInterfaces.MainTabs
{
    public abstract class ListTabVMBase<TDto, TRow> : R2ViewModelBase
    {
        public ListTabVMBase()
        {
            UpdateTitle(TabTitle);

            ReloadCmd = R2Command.Async(ReloadRows);
        }

        protected abstract string                         TabTitle  { get; }
        protected abstract Func<Task<Reply<List<TDto>>>>  GetAll    { get; }
        protected abstract Func<TDto, TRow>               CreateRow { get; }

        public IR2Command         ReloadCmd  { get; }
        public Observables<TRow>  Rows       { get; } = new Observables<TRow>();


        private async Task ReloadRows()
        {
            AsUI(_ => Rows.Clear());

            var reply = await GetAll();
            if (reply.Failed)
            {
                SetStatus(reply.ErrorsText);
                return;
            }

            var vms = reply.Result.Select(x 
                    => CreateRow(x)).ToList();

            AsUI(_ => Rows.Swap(vms));
        }
    }
}
