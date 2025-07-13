using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Internal.Titles;

namespace OnlineСinema.Models.Queries.Titles
{
    public class TitleVideoQuery:IRequestModel<TitleVideoModel>
    {
        public Guid Id { get; set; }    
    }
}
