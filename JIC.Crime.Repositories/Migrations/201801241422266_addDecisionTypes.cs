namespace JIC.Crime.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDecisionTypes : DbMigration
    {
        public override void Up()
        {
            Sql(@"
SET IDENTITY_INSERT [dbo].[Configurations_Lookups] ON
INSERT INTO [dbo].[Configurations_Lookups] ([ID], [Name], [CategoryID], [CreatedAt], [CreatedBy], [LastModifiedAt], [LastModifiedBy]) VALUES (89, N'قرار  ', 9, N'2018-01-03 00:00:00', N'Heba Basyony', NULL, NULL)

SET IDENTITY_INSERT [dbo].[Configurations_Lookups] OFF
");

            Sql(@"
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (1, N'تحقيق  ', 29)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (2, N'إحاله الى الخبراء  ', 29)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (3, N'إحاله الى الطب الشرعى  ', 29)

INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (4, N'براءه  ', 28)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (5, N'إدانه  ', 28)


INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (6, N'إحاله للمكتب الفنى  ', 89)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (7, N'تأجيل نظر الدعوى  ', 89)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (8, N'توقف عن النظام -وقف تعليقى  ', 89)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (9, N'توقف عن النظام -وقف سير  ', 89)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (10, N'عدم اخنصاص ولائى  ', 89)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (11, N'عدم اخنصاص نوعى  ', 89)
INSERT INTO [dbo].[Configurations_DecisionTypes] ([ID], [Name], [DecisionLevel]) VALUES (12, N'عدم اخنصاص محلى  ', 89)
");
        }
        
        public override void Down()
        {
        }
    }
}
