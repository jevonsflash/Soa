using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Hangfire;
using Abp;

namespace Soa
{
    [DependsOn(typeof(AbpKernelModule))]
    public class SoaModule : AbpModule
    {
        public override void Initialize()
        {
            var thisAssembly = typeof(SoaModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

        }

    }
}
