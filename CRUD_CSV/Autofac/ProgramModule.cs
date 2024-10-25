using Autofac;
using CRUD_CSV.Manager;
using CRUD_CSV.MenuTools;
using CRUD_CSV.Service;
using CRUD_CSV.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_CSV.Autofac
{
    public class ProgramModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(CsvManager.Instance)
               .SingleInstance();
            builder.RegisterType<CsvService>()
               .As<ICsvService>();

            // Menu strategier
            builder.RegisterType<CreateMenuStrategy>().As<IMenuStrategy>();
            builder.RegisterType<ReadMenuStrategy>().As<IMenuStrategy>();
            builder.RegisterType<UpdateMenuStrategy>().As<IMenuStrategy>();
            builder.RegisterType<DeleteMenuStrategy>().As<IMenuStrategy>();

            builder.RegisterType<Menu>()
                           .AsSelf();
        }

        public static IContainer Setup()
        {
            var myContainerBuilder = new ContainerBuilder();
            myContainerBuilder.RegisterModule<ProgramModule>();
            return myContainerBuilder.Build();
        }
    }
}
