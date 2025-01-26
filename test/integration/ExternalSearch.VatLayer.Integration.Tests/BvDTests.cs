// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatLayerTests.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the VatLayer tests class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Messages.Processing;
using CluedIn.ExternalSearch.Providers.BvD;
using CluedIn.Testing.Base.ExternalSearch;
using Moq;
using Xunit;

namespace ExternalSearch.BvD.Integration.Tests
{
    public class BvDTests : BaseExternalSearchTest<BvDExternalSearchProvider>
    {
        private const string ApiToken = "118edd4591f3cf622af6e72e4eded7ea";

        [Theory]
        [InlineData("DK36548681")]
        public void TestValidVATNumber(string vatNumber)
        {
            //Arrange
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.VatNumber, vatNumber);
            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            var list = new List<string>(new string[] { ApiToken });
            object[] parameters = { list };

            //Act
            this.Setup(parameters, entityMetadata);

            // Assert
            this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);
            Assert.True(this.clues.Count == 1);
        }

        [Theory]
        [InlineData("asdasd")]
        public void TestInvalidVATNumber(string vatNumber)
        {
            //Arrange
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.VatNumber, vatNumber);
            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            var list = new List<string>(new string[] { ApiToken });
            object[] parameters = { list };

            //Act
            this.Setup(parameters, entityMetadata);

            // Assert
            this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);
        }

        [Theory]
        [InlineData("DK12345")]
        public void TestNonExistingVATNumber(string vatNumber)
        {
            //Arrange
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.VatNumber, vatNumber);
            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            var list = new List<string>(new string[] { ApiToken });
            object[] parameters = { list };

            //Act
            this.Setup(parameters, entityMetadata);

            // Assert
            Assert.True(this.clues.Count == 0);
        }

        [Fact]
        public void TestMissingApiToken()
        {
            //Arrange
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.VatNumber, "asdasd");
            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };


            // Assert
            Assert.Throws<InvalidOperationException>(() => this.Setup(null, entityMetadata));
            this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);
        }

        [Fact]
        public void TestIncorrectApiToken()
        {
            //Arrange
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.VatNumber, "DK12345");
            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            var list = new List<string>(new string[] { "laskfjfklj" });
            object[] parameters = { list };

            //Act
            this.Setup(parameters, entityMetadata);

            // Assert
            this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.Never);
        }
    }
}
