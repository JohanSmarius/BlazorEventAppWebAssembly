using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]

    public class EventsTests : PageTest
    {
        [SetUp]
        public async Task Setup()
        {
            await Context.Tracing.StartAsync(new()
            {
                Title = TestContext.CurrentContext.Test.ClassName + "." + TestContext.CurrentContext.Test.Name,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            var failed = TestContext.CurrentContext.Result.Outcome == NUnit.Framework.Interfaces.ResultState.Error
              || TestContext.CurrentContext.Result.Outcome == NUnit.Framework.Interfaces.ResultState.Failure;

            await Context.Tracing.StopAsync(new()
            {
                Path = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    "playwright-traces",
                    $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.zip"
                )
            });
        }


        [Test]
        public async Task CanAddEvent()
        {
            
            await Page.GotoAsync("http://localhost:5155/");
            await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Hello");

            await Page.GetByRole(AriaRole.Link, new() { Name = "Events" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("Name").ClickAsync();
            await Page.GetByLabel("Name").FillAsync("New event");
            await Page.GetByLabel("Name").PressAsync("Tab");
            await Page.GetByLabel("Location").FillAsync("This is the location for the event");
            await Page.GetByLabel("Start Date and Time").ClickAsync();
            await Page.GetByLabel("Start Date and Time").FillAsync("2025-05-01T00:00");
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").FillAsync("2025-05-02T00:00");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Description").ClickAsync();
            await Page.GetByLabel("Description").FillAsync("This is a description for the event");
            await Page.GetByLabel("Event Type").SelectOptionAsync(new[] { "Music" });
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.WaitForURLAsync(new Regex(".*events"));
            await Expect(Page.Locator("tbody")).ToContainTextAsync("New event");
            await Expect(Page.Locator("tbody")).ToContainTextAsync("This is the location for the event");
        }

        [Test]
        public async Task AddedEventsCanBeEdited()
        {
            await Page.GotoAsync("http://localhost:5155/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Events" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("Name").ClickAsync();
            await Page.GetByLabel("Name").FillAsync("Test event");
            await Page.GetByLabel("Location").ClickAsync();
            await Page.GetByLabel("Location").FillAsync("Event locatoon number 1");
            await Page.GetByLabel("Location").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("0001-05-01T00:00");
            await Page.GetByLabel("Start Date and Time").ClickAsync();
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("2025-05-01T00:00");
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").FillAsync("2025-05-02T00:00");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Description").ClickAsync();
            await Page.GetByLabel("Description").FillAsync("Some description");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("Name").ClickAsync();
            await Page.GetByLabel("Name").FillAsync("Another event");
            await Page.GetByLabel("Location").ClickAsync();
            await Page.GetByLabel("Location").FillAsync("Other event location");
            await Page.GetByLabel("Location").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("0001-06-01T00:00");
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("2025-06-01T00:00");
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").FillAsync("0001-06-02T00:00");
            await Page.GetByLabel("Description").ClickAsync();
            await Page.GetByLabel("Description").FillAsync("Some other description");
            await Page.GetByLabel("Description").PressAsync("Tab");
            await Page.GetByLabel("Event Type").SelectOptionAsync(new[] { "Music" });
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").FillAsync("2025-06-02T00:00");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).First.ClickAsync();
            await Expect(Page.GetByLabel("Name")).ToHaveValueAsync("Test event");
            await Expect(Page.GetByLabel("Location")).ToHaveValueAsync("Event locatoon number 1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Update Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).Nth(1).ClickAsync();
            await Expect(Page.GetByLabel("Name")).ToHaveValueAsync("Another event");
            await Expect(Page.GetByLabel("Location")).ToHaveValueAsync("Other event location");
        }

        [Test]
        public async Task EventCanBeDeleted()
        {
            await Page.GotoAsync("http://localhost:5155/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Events" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("Name").ClickAsync();
            await Page.GetByLabel("Name").FillAsync("Test event");
            await Page.GetByLabel("Location").ClickAsync();
            await Page.GetByLabel("Location").FillAsync("Event locatoon number 1");
            await Page.GetByLabel("Location").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("0001-05-01T00:00");
            await Page.GetByLabel("Start Date and Time").ClickAsync();
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("2025-05-01T00:00");
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").FillAsync("2025-05-02T00:00");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Description").ClickAsync();
            await Page.GetByLabel("Description").FillAsync("Some description");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("Name").ClickAsync();
            await Page.GetByLabel("Name").FillAsync("Another event");
            await Page.GetByLabel("Location").ClickAsync();
            await Page.GetByLabel("Location").FillAsync("Other event location");
            await Page.GetByLabel("Location").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("0001-06-01T00:00");
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("2025-06-01T00:00");
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").FillAsync("0001-06-02T00:00");
            await Page.GetByLabel("Description").ClickAsync();
            await Page.GetByLabel("Description").FillAsync("Some other description");
            await Page.GetByLabel("Description").PressAsync("Tab");
            await Page.GetByLabel("Event Type").SelectOptionAsync(new[] { "Music" });
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").FillAsync("2025-06-02T00:00");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).First.ClickAsync();
            await Expect(Page.GetByLabel("Name")).ToHaveValueAsync("Test event");
            await Expect(Page.GetByLabel("Location")).ToHaveValueAsync("Event locatoon number 1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Update Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).Nth(1).ClickAsync();
            await Expect(Page.GetByLabel("Name")).ToHaveValueAsync("Another event");
            await Expect(Page.GetByLabel("Location")).ToHaveValueAsync("Other event location");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Events" }).ClickAsync();
            void Page_Dialog_EventHandler(object sender, IDialog dialog)
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.AcceptAsync();
                Page.Dialog -= Page_Dialog_EventHandler;
            }
            Page.Dialog += Page_Dialog_EventHandler;
            await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First.ClickAsync();
            await Expect(Page.Locator("tbody")).Not.ToContainTextAsync("Test event");
            await Expect(Page.Locator("tbody")).ToContainTextAsync("Another event");
        }


        [Test]
        public async Task EventDeletionCanBeCanceled()
        {
            await Page.GotoAsync("http://localhost:5155/");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Events" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("Name").ClickAsync();
            await Page.GetByLabel("Name").FillAsync("Test event");
            await Page.GetByLabel("Location").ClickAsync();
            await Page.GetByLabel("Location").FillAsync("Event locatoon number 1");
            await Page.GetByLabel("Location").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("0001-05-01T00:00");
            await Page.GetByLabel("Start Date and Time").ClickAsync();
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("2025-05-01T00:00");
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").FillAsync("2025-05-02T00:00");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Description").ClickAsync();
            await Page.GetByLabel("Description").FillAsync("Some description");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("Name").ClickAsync();
            await Page.GetByLabel("Name").FillAsync("Another event");
            await Page.GetByLabel("Location").ClickAsync();
            await Page.GetByLabel("Location").FillAsync("Other event location");
            await Page.GetByLabel("Location").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("0001-06-01T00:00");
            await Page.GetByLabel("Start Date and Time").PressAsync("Tab");
            await Page.GetByLabel("Start Date and Time").FillAsync("2025-06-01T00:00");
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").FillAsync("0001-06-02T00:00");
            await Page.GetByLabel("Description").ClickAsync();
            await Page.GetByLabel("Description").FillAsync("Some other description");
            await Page.GetByLabel("Description").PressAsync("Tab");
            await Page.GetByLabel("Event Type").SelectOptionAsync(new[] { "Music" });
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByLabel("End Date and Time").ClickAsync();
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByLabel("End Date and Time").FillAsync("2025-06-02T00:00");
            await Page.GetByLabel("End Date and Time").PressAsync("Tab");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Add Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).First.ClickAsync();
            await Expect(Page.GetByLabel("Name")).ToHaveValueAsync("Test event");
            await Expect(Page.GetByLabel("Location")).ToHaveValueAsync("Event locatoon number 1");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Update Event" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).Nth(1).ClickAsync();
            await Expect(Page.GetByLabel("Name")).ToHaveValueAsync("Another event");
            await Expect(Page.GetByLabel("Location")).ToHaveValueAsync("Other event location");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Events" }).ClickAsync();
            void Page_Dialog_EventHandler(object sender, IDialog dialog)
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.DismissAsync();
                Page.Dialog -= Page_Dialog_EventHandler;
            }
            Page.Dialog += Page_Dialog_EventHandler;
            await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First.ClickAsync();
            await Expect(Page.Locator("tbody")).ToContainTextAsync("Test event");
            await Expect(Page.Locator("tbody")).ToContainTextAsync("Another event");
        }
    }
}
