namespace ConsoleApplication4
{
    internal class PublicationIdentifier
    {
        public PublicationIdentifier(string doi)
        {
            Doi = doi;
        }
        public string Doi { get; }

        public int PublicationId { get; }
    }
}