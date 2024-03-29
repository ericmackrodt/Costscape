﻿using Autofac;
using Broadcaster;
using Costscape.Common;
using Costscape.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Costscape
{
    public class ViewModelLocator
    {
        private readonly IContainer _container;

        public ViewModelLocator()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<DataManager>().As<IDataManager>();
            containerBuilder.RegisterType<BroadcastContainer>().As<IBroadcaster>().SingleInstance();

            containerBuilder.RegisterType<MainViewModel>();
            containerBuilder.RegisterType<BudgetViewModel>();

            _container = containerBuilder.Build();
        }

        public MainViewModel MainViewModel
        {
            get { return _container.Resolve<MainViewModel>(); }
        }

        public BudgetViewModel BudgetViewModel
        {
            get { return _container.Resolve<BudgetViewModel>(); }
        }
    }
}
