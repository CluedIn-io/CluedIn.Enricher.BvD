
namespace CluedIn.ExternalSearch.Providers.BvD.Vocabularies
{
    /// <summary>The BvD vocabulary</summary>
    public static class BvDVocabulary
    {
        /// <summary>
        /// Initializes static members of the <see cref="BvDVocabulary" /> class.
        /// </summary>
        static BvDVocabulary()
        {
            Organization = new BvDOrganizationVocabulary();
        }

        /// <summary>Gets the organization.</summary>
        /// <value>The organization.</value>
        public static BvDOrganizationVocabulary Organization { get; private set; }
    }
}
