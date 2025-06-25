using FluentMigrator;

namespace ArlequimPetShop.Migrations
{
    [Migration(20231127145400)]
    public class CreateTableNLogger : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"CREATE TABLE [dbo].[NLogger](
								[Id] [int] IDENTITY(1,1) NOT NULL,
								[EventDate] [datetime] NULL,
								[MethodName] [nvarchar](255) NULL,
								[Action] [nvarchar](500) NULL,
								[LogLevel] [nvarchar](50) NULL,
								[Message] [nvarchar](max) NULL,
								[Exception] [nvarchar](max) NULL,
								[StackTrace] [nvarchar](max) NULL,
								[Application] [nvarchar](200) NULL,
								[Host] [nvarchar](40) NULL,
								[Machine] [nvarchar](255) NULL,
								[Parameters] [nvarchar](max) NULL,
								[OnCreated] [datetime] NOT NULL,
							PRIMARY KEY CLUSTERED 
							(
								[Id] ASC
							)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
							) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
							GO");
        }

        public override void Down()
        {
            Delete.Table("NLogger");
        }
    }
}