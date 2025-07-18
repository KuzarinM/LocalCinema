using PIHelperSh.Core.Attributes;

namespace OnlineСinema.Models.Enums
{
    [Flags]
    public enum Permitions
    {
        [TypeValue<string>("user")]
        IsAuthorised = 1<<0,

        [TypeValue<string>("edit_titles")]
        EditTitles = 1<<5,

        [TypeValue<string>("edit_galery")]
        EditGalery = 1<<6,

        [TypeValue<string>("load_from_disk")]
        LoadFromDisk = 1<<7,

        [TypeValue<string>("manage_user")]
        ManageUser = 1<<8,
    }
}
