using AdstractHelpers.Mediator.Interfaces;

namespace OnlineСinema.Models.Queries.Titles
{
    public class CreateTitleDescriptionQuery: IRequestModel<string>
    {
        public Guid TitleId { get; set; }   
    }
}
