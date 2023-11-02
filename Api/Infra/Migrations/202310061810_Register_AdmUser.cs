using Api.Security;
using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202310061810)]
    public class _202310061810_Register_AdmUser : Migration
    {
        public override void Down() { }

        public override void Up()
        {
            Insert.IntoTable("User")
                .Row(new
                {
                    Name = "Administrador",
                    Login = "Adm",
                    Password = "123".Hash(),
                    Bibliography = "Usuário administrador do sistema",
                    Super = true,
                    ProvisoryPassword = false,
                    CreationDate = DateTime.Now,
                    Blocked = false
                });
        }
    }
}
