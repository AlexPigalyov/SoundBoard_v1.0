using System.Windows;
using System.Windows.Input;
using Data;
using Data.Repositories;
using Data.Repositories.Abstract;
using Services;
using Services.Abstract;
using Shell.ViewModels;

namespace Shell {
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;

    public class AppBootstrapper : BootstrapperBase {
        SimpleContainer container;

        public AppBootstrapper() {
            Initialize();
        }

        protected override void Configure() {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();


            container.PerRequest<ShellViewModel>();

            container.PerRequest<ISoundService, SoundService>();

            container.PerRequest<ISoundRepository, SoundRepository>();

            container.Singleton<SoundBoardContext>();
        }

        protected override object GetInstance(Type service, string key) {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}