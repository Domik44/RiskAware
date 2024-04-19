using Microsoft.Playwright;
using RiskAware.Client.Tests.Infrastructure;

namespace RiskAware.Client.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : ReactTest
    {
        [Test]
        public async Task Count_Increments_WhenButtonIsClicked()
        {
            await Page.GotoAsync($"{RootUri.AbsoluteUri}");

            await Page.PauseAsync();
        }
    }
}
