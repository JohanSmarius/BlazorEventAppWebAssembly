using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;


namespace UITests
    {
        [Parallelizable(ParallelScope.Self)]
        [TestFixture]
        public class HomePageTest : PageTest
        {
            [Test]
            public async Task ShouldShowWelcomeMessage()
            {
                await Page.GotoAsync("http://localhost:5155/");
                await Page.GetByRole(AriaRole.Link, new() { Name = "Home" }).ClickAsync();
                await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Hello");
                await Expect(Page.GetByRole(AriaRole.Article)).ToContainTextAsync("Hello Welcome to the Event Management App.");
            }
        }
    }
