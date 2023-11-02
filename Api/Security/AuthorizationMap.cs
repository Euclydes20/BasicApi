using System.Collections;
using System.Collections.ObjectModel;

namespace Api.Security
{
    public class AuthorizationMap
    {
        public class AuthorizationBase
        {
            public readonly string AuthorizationGroupCode;
            public readonly string AuthorizationTypeCode;
            public readonly string AuthorizationTitle;
            public readonly string AuthorizationDescription;

            public AuthorizationBase(AuthorizationGroup authorizationGroup, AuthorizationType authorizationType, string authorizationTitle, string authorizationDescription)
            {
                this.AuthorizationGroupCode = authorizationGroup.ToString();
                this.AuthorizationTypeCode = authorizationType.ToString();
                this.AuthorizationTitle = authorizationTitle;
                this.AuthorizationDescription = authorizationDescription;
            }
        }

        private static readonly List<AuthorizationBase> _authorizationList = new()
        {
            new AuthorizationBase(AuthorizationGroup.User, AuthorizationType.UserCreate, "Create User", "Define if is allowed create a user."),
            new AuthorizationBase(AuthorizationGroup.User, AuthorizationType.UserEdit, "Edit User", "Define if is allowed edit a user."),
            new AuthorizationBase(AuthorizationGroup.User, AuthorizationType.UserDelete, "Delete User", "Define if is allowed delete a user."),
            new AuthorizationBase(AuthorizationGroup.User, AuthorizationType.UserView, "View User", "Define if is allowed view a user."),

            new AuthorizationBase(AuthorizationGroup.Annotation, AuthorizationType.AnnotationCreate, "Create Annotation", "Define if is allowed create a annotation."),
            new AuthorizationBase(AuthorizationGroup.Annotation, AuthorizationType.AnnotationEdit, "Edit Annotation", "Define if is allowed edit a annotation."),
            new AuthorizationBase(AuthorizationGroup.Annotation, AuthorizationType.AnnotationDelete, "Delete Annotation", "Define if is allowed delete a annotation."),
            new AuthorizationBase(AuthorizationGroup.Annotation, AuthorizationType.AnnotationView, "View Annotation", "Define if is allowed view a annotation."),
        };

        public static IReadOnlyCollection<AuthorizationBase> AuthorizationList => _authorizationList.AsReadOnly();
    }

    public enum AuthorizationGroup
    {
        User = 1,
        Annotation,
    }

    public enum AuthorizationType
    {
        UserCreate,
        UserEdit,
        UserDelete,
        UserView,

        AnnotationCreate,
        AnnotationEdit,
        AnnotationDelete,
        AnnotationView
    }
}
