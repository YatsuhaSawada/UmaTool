using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaTools.ViewModels
{
    public class Item
    {
        //public ReadOnlyReactivePropertySlim<string> Name { get; }
        //public ReactivePropertySlim<string> Name { get; }
        public ReactiveProperty<string> Name = new ReactiveProperty<string>("hogehoge");
        public ReactiveProperty<int> Id = new ReactiveProperty<int>(0);

        public Item(string name, int id)
        {
            Name.Value = name;
            Id.Value = id;
        }
    }

    public class ItemViewModel : IDisposable
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReadOnlyReactivePropertySlim<string> Name { get; }
        public ItemViewModel(Item item)
        {
            Name = item
                .Name
                .ObserveOnUIDispatcher()
                .ToReadOnlyReactivePropertySlim().AddTo(Disposable);
        }

        public void Dispose() => Disposable.Dispose();
    }
}
