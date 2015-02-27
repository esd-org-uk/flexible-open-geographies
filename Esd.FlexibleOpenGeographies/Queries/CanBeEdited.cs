using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class CanBeEdited : IQuerySingle<bool>
    {
        private readonly string _creator;
        private readonly string _organisation;
        private readonly UserBasic _currentUser;

        public CanBeEdited(string creator, string organisation, UserBasic currentUser)
        {
            _creator = creator;
            _organisation = organisation;
            _currentUser = currentUser;
        }

        public bool Find()
        {
            /*TODO: need to decide on how security will work.
            Who can edit data, load kml, change hierarchies, add or remove composing areas or parents, upload/download metrics?
            For now allow signed in users to do everything.*/
            return _currentUser != null && _currentUser.UserId != null;
            return _currentUser.OrganisationId != null
                ? _currentUser.OrganisationId == _organisation
                : _currentUser.UserId == _creator;
        }
    }
}
