using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using UmaTools.Models;

namespace UmaTools.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "UmaTools";
        public string Title { 
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        // Contoller
        private ProcessCollecter ProcessCollecter = new ProcessCollecter();
        private CaptureWindow CaptureWindow = new CaptureWindow();

        // view data source
        private ObservableCollection<Item> Source { get; }
        public ReadOnlyReactiveCollection<ItemViewModel> Items { get; }
        public ReactivePropertySlim<ItemViewModel> SelectedItem { get; }

        public ReactiveProperty<string> PathOfScreencapture { get; private set; }
        public ReactiveCommand CaptureCommand { get; }
        public ReactiveCommand SaveCommand { get; }
        public ReactiveCommand LoadedCommand { get; }

        // view data source
        static private int SelectedItemId = new int();
        static private string SelectedItemName = "";
        static private int CaptureCount = 0;

        // view data source

        public MainWindowViewModel()
        {
            //コンボボックスの初期化
            Source = new ObservableCollection<Item>(ProcessCollecter.collect().Select(x => new Item(x.ProcessName, x.ProcessId)));
            Items = Source.ToReadOnlyReactiveCollection(x => new ItemViewModel(x));
            SelectedItem = new ReactivePropertySlim<ItemViewModel>(Items.First());

            SelectedItem.Subscribe(x =>
            {
                var res = Source.Where(y => y.Name.Value == x.Name.Value).ToList();
                if (res.Count > 0)
                {
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
                    SelectedItemId = res[0].Id.Value;
                    SelectedItemName = res[0].Name.Value;
                }
            });

            //スクリーンキャプチャ保存先
            PathOfScreencapture = new ReactiveProperty<string>();

            //キャプチャボタン
            CaptureCommand = new ReactiveCommand();
            CaptureCommand.Subscribe(_ => hookCapture(null, null));

            //セーブコマンド
            SaveCommand = new ReactiveCommand();
            SaveCommand.Subscribe(_ => saveSetting());

            //
            LoadedCommand = new ReactiveCommand();
            LoadedCommand.Subscribe(_ => setupdata());            
        }

        //
        public void setupdata()
        {
            var loaded = DataAccessor.load();
            if (!loaded.res)
            {
                return;
            }

            var res = Items.Where(x => x.Name.Value == loaded.data.ProcessName).ToList();
            if (res.Count != 0)
            {
                SelectedItem.Value = res[0];
            }
            PathOfScreencapture.Value = loaded.data.FolderPath;
        }
        
        //
        public void hookCapture(object sender, EventArgs e)
        {
            try
            {
                string dpath = PathOfScreencapture.Value;
                if (string.IsNullOrEmpty(dpath))
                {
                    return;
                }

                DateTime dt = DateTime.Now;
                string result = dt.ToString("yyyyMMdd_HHmmss");
                string path = $"{dpath}\\{result}_{CaptureCount}.png"; 
                
                CaptureWindow.Capture(SelectedItemId, path);
                ++CaptureCount;
            }
            catch
            {

            }
        }

        //
        public void saveSetting()
        {
            Setting setting = new Setting
            {
                ProcessName = SelectedItemName,
                FolderPath  = PathOfScreencapture.Value
            };
            DataAccessor.save(setting);
        }
    }
}
