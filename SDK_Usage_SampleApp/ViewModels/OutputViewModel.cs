using System;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using KasaGE;
using KasaGE.Core;
using SDK_Usage_SampleApp.Messaging.Events;
using SDK_Usage_SampleApp.Utils;

namespace SDK_Usage_SampleApp.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class OutputViewModel : Screen
    {

        [ImportingConstructor]
        public OutputViewModel(IMessageAggregator messenger)
        {
            Logs = new BindableCollection<LogViewModel>();
            messenger.GetStream<EcrRespondedEvent>()
                .Subscribe(e => Log(e.Response));
            messenger.GetStream<EcrThrewExceptionEvent>()
                .Subscribe(e => Log(e.Exception));
        }

        public BindableCollection<LogViewModel> Logs { get; set; }

        private void Log(object obj)
        {
            var type = obj.GetType();
            var logEntry = new LogViewModel { TypeName = type.Name };
            var propertyInfos = type.GetProperties();
            if (type == typeof(Exception))
                propertyInfos = propertyInfos.Where(x => x.Name == "Message").ToArray();
            foreach (var pi in propertyInfos)
            {
                var value = pi.GetValue(obj, null);
                var logItem = new LogItemViewModel
                {
                    PropertyName = pi.Name,
                    Value = value != null ? value.ToString() : "null"
                };
                logEntry.Props.Add(logItem);
            }
            Logs.Insert(0, logEntry);
        }
    }

    public class LogViewModel
    {
        public LogViewModel()
        {
            Props = new BindableCollection<LogItemViewModel>();
        }

        public string TypeName { get; set; }
        public BindableCollection<LogItemViewModel> Props { get; set; }
    }

    public class LogItemViewModel
    {
        public string PropertyName { get; set; }
        public string Value { get; set; }
    }
}