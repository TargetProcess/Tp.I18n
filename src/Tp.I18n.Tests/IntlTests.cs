using System.Collections.Generic;
using System.Linq;
using Jeffijoe.MessageFormat;
using NUnit.Framework;

namespace Tp.I18n.Tests
{
    public class IntlTests
    {
        private Intl Intl => new Intl(new MessageFormatter(false));

        [Test]
        public void FormatMessage_ShouldInjectDataTokens()
        {
            var actual = Intl.GetFormattedMessage("{fieldName} should be specified.", new {fieldName = "Project"}).Value;

            Assert.That(actual, Is.EqualTo("Project should be specified."));
        }

        [Test]
        [ExpectedException(typeof (KeyNotFoundException),
            ExpectedMessage = "The variable 'fieldName' was not found in the arguments collection.")]
        public void FormatMessage_ShouldThrowIfDataDoesNotContainKey()
        {
            var token = "{fieldName} should be specified.";
            var data = new {fieldSurname = "Project"};
            Intl.GetFormattedMessage(token, data);
        }

        [Test]
        public void FormatMessage_ShouldInjectEnums()
        {
            var actual =
                Intl.GetFormattedMessage("{fieldName} should be specified.", new {fieldName = EntityKind.Project}).Value;
            Assert.That(actual, Is.EqualTo("Project should be specified."));
        }

        [Test]
        public void FormatMessage_ShouldNotDisplayNulls()
        {
            var actual =
                Intl.GetFormattedMessage("{fieldName} should be specified.", new {fieldName = (EntityKind?) null}).Value;
            Assert.That(actual, Is.EqualTo(" should be specified."));
        }

        [Test]
        public void FormatHierarchyMessageTest()
        {
            var actual = Intl.GetFormattedMessage("{fieldName} should be specified.",
                new
                {
                    fieldName = Intl.GetFormattedMessage("{fieldName} should be specified.", new {fieldName = "nested"})
                });
            Assert.That(actual.Data["fieldName"], Is.AssignableTo<IFormattedMessage>());
            Assert.That(actual.Value, Is.EqualTo("nested should be specified. should be specified."));
        }

        [TestCase("x", Result = "Team 'x' needs to be assigned to project 'Sodom'.")]
        [TestCase("x,y", Result = "Teams 'x', 'y' need to be assigned to project 'Sodom'.")]
        public string FormatMessage_ShouldInjectPlural(string teamsSpec)
        {
            var teams = teamsSpec.Split(',');
            var teamsText = string.Join(", ", teams.Select(i => $"'{i}'"));
            return Intl.GetFormattedMessage(
                "{teamsCount, plural, =1 {Team {teamsText} needs} other {Teams {teamsText} need}} to be assigned to project '{projectName}'.",
                new {teamsCount = teams.Length, teamsText, projectName = "Sodom"}).Value;
        }

        [Test]
        public void FormattedMessage_ShouldPassEqualityChecks()
        {
            var message1 = Intl.GetFormattedMessage("{fieldName} should be specified.", new {fieldName = "Project"});
            var message2 = Intl.GetFormattedMessage("{fieldName} should be specified.",
                new {fieldName = EntityKind.Project});

            Assert.That(message1.Equals(message2));
            Assert.That(message2.Equals(message1));
            Assert.That(message1.Equals((object) message2));
            Assert.That(message2.Equals((object) message1));
        }
    }
}