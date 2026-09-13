using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests.Support
{
    [Binding]
    public class Hooks
    {
        private readonly AcceptanceScenarioContext _scenario;

        public Hooks(AcceptanceScenarioContext scenario)
        {
            _scenario = scenario;
        }

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            await AcceptanceTestInfrastructure.StartAsync();
        }

        [BeforeScenario]
        public async Task BeforeScenarioAsync()
        {
            await AcceptanceTestInfrastructure.Database.ResetDatabaseAsync();

            _scenario.Client = AcceptanceTestInfrastructure.ApiFactory.CreateClient();
        }

        [AfterTestRun]
        public static async Task AfterTestRun()
        {
            await AcceptanceTestInfrastructure.StopAsync();
        }

    }
}
