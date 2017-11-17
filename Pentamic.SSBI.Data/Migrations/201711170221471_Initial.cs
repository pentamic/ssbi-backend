namespace Pentamic.SSBI.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alert",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        MainValueField = c.String(),
                        MainValueModification = c.String(),
                        MainFilterExpression = c.String(),
                        MainCustomExpression = c.String(),
                        TargetValueField = c.String(),
                        TargetValueModification = c.String(),
                        TargetFilterExpression = c.String(),
                        TargetCustomExpression = c.String(),
                        Condition = c.Int(nullable: false),
                        UseThresold = c.Boolean(nullable: false),
                        Thresold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Frequency = c.Int(nullable: false),
                        LastRun = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        NotificationId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notification", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.NotificationId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Column",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        SourceColumn = c.String(),
                        DisplayFolder = c.String(),
                        FormatString = c.String(),
                        SortByColumnId = c.Int(),
                        IsHidden = c.Boolean(nullable: false),
                        Expression = c.String(),
                        DataType = c.Int(nullable: false),
                        ColumnType = c.Int(nullable: false),
                        IsKey = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Column", t => t.SortByColumnId)
                .ForeignKey("dbo.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId)
                .Index(t => t.SortByColumnId);
            
            CreateTable(
                "dbo.Relationship",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        FromColumnId = c.Int(nullable: false),
                        ToColumnId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CrossFilteringBehavior = c.Int(nullable: false),
                        Cardinality = c.Int(nullable: false),
                        DateBehavior = c.Int(),
                        SecurityFilteringBehavior = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Column", t => t.FromColumnId)
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("dbo.Column", t => t.ToColumnId)
                .Index(t => t.ModelId)
                .Index(t => t.FromColumnId)
                .Index(t => t.ToColumnId);
            
            CreateTable(
                "dbo.Model",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DefaultMode = c.Int(nullable: false),
                        GenerateFromTemplate = c.String(),
                        CloneFromModelId = c.Int(),
                        RefreshSchedule = c.String(),
                        RefreshJobId = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        RefreshedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataSource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        ConnectionString = c.String(),
                        Source = c.String(),
                        Catalog = c.String(),
                        User = c.String(),
                        Password = c.String(),
                        IntegratedSecurity = c.Boolean(nullable: false),
                        SourceFileId = c.Int(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("dbo.SourceFile", t => t.SourceFileId)
                .Index(t => t.ModelId)
                .Index(t => t.SourceFileId);
            
            CreateTable(
                "dbo.Partition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        DataSourceId = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        Query = c.String(),
                        Expression = c.String(),
                        SourceType = c.Int(nullable: false),
                        ProcessedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessedBy = c.String(),
                        IsProcessing = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataSource", t => t.DataSourceId)
                .ForeignKey("dbo.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId)
                .Index(t => t.DataSourceId);
            
            CreateTable(
                "dbo.Table",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        SourceTable = c.String(),
                        SourceSchema = c.String(),
                        DataCategory = c.String(),
                        ProcessedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessedBy = c.String(),
                        IsProcessing = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.Hierarchy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DisplayFolder = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
            CreateTable(
                "dbo.Level",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HierarchyId = c.Int(nullable: false),
                        ColumnId = c.Int(nullable: false),
                        Name = c.String(),
                        Ordinal = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hierarchy", t => t.HierarchyId, cascadeDelete: true)
                .Index(t => t.HierarchyId);
            
            CreateTable(
                "dbo.Measure",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DisplayFolder = c.String(),
                        Expression = c.String(),
                        FormatString = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
            CreateTable(
                "dbo.SourceFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModelSharing",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ModelId = c.Int(nullable: false),
                        Permission = c.String(),
                        SharedBy = c.String(),
                        SharedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ModelId })
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.ModelRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        ModelPermission = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.ModelRoleTablePermission",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        TableId = c.Int(nullable: false),
                        FilterExpression = c.String(),
                        MetadataPermission = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.TableId })
                .ForeignKey("dbo.ModelRole", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Table", t => t.TableId)
                .Index(t => t.RoleId)
                .Index(t => t.TableId);
            
            CreateTable(
                "dbo.UserModelRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ModelRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.DashboardComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DashboardId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "dbo.Dashboard",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Ordinal = c.Int(nullable: false),
                        GridConfig = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DashboardFilter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DashboardId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        FilterType = c.String(),
                        DataConfig = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "dbo.DashboardSharing",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DashboardId = c.Int(nullable: false),
                        Permission = c.String(),
                        SharedBy = c.String(),
                        SharedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.DashboardId })
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "dbo.DashboardTile",
                c => new
                    {
                        DashboardId = c.Int(nullable: false),
                        ReportTileId = c.Int(nullable: false),
                        PositionConfig = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.DashboardId, t.ReportTileId })
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .ForeignKey("dbo.ReportTile", t => t.ReportTileId, cascadeDelete: true)
                .Index(t => t.DashboardId)
                .Index(t => t.ReportTileId);
            
            CreateTable(
                "dbo.ReportTile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportPageId = c.Int(nullable: false),
                        ReportId = c.Int(nullable: false),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        PositionConfig = c.String(),
                        DisplayConfig = c.String(),
                        DataConfig = c.String(),
                        DisplayTypeId = c.String(nullable: false, maxLength: 128),
                        SourceType = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DisplayType", t => t.DisplayTypeId, cascadeDelete: true)
                .ForeignKey("dbo.ReportPage", t => t.ReportPageId, cascadeDelete: true)
                .Index(t => t.ReportPageId)
                .Index(t => t.DisplayTypeId);
            
            CreateTable(
                "dbo.DisplayType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Icon = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportPage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Name = c.String(),
                        Ordinal = c.Int(nullable: false),
                        GridConfig = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.Report",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Ordinal = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.ReportFilter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        FilterType = c.String(),
                        DataConfig = c.String(),
                        DefaultValue = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.ReportSharing",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReportId = c.Int(nullable: false),
                        Permission = c.String(),
                        SharedBy = c.String(),
                        SharedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReportId })
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.ReportView",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Selections = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.DashboardView",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DashboardId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Selections = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "dbo.ModelRefreshQueue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        StartedAt = c.DateTimeOffset(precision: 7),
                        EndedAt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationSubscription",
                c => new
                    {
                        NotificationId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        UseEmail = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotificationId, t.UserId });
            
            CreateTable(
                "dbo.PerspectiveColumn",
                c => new
                    {
                        PerspectiveId = c.Int(nullable: false),
                        ColumnId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PerspectiveId, t.ColumnId })
                .ForeignKey("dbo.Column", t => t.ColumnId, cascadeDelete: true)
                .ForeignKey("dbo.Perspective", t => t.PerspectiveId, cascadeDelete: true)
                .Index(t => t.PerspectiveId)
                .Index(t => t.ColumnId);
            
            CreateTable(
                "dbo.Perspective",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportTileRow",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportTileId = c.Int(nullable: false),
                        Code = c.String(),
                        Name = c.String(),
                        Ordinal = c.Int(nullable: false),
                        IsGroup = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        IsFormula = c.Boolean(nullable: false),
                        ValueExpression = c.String(),
                        FilterExpression = c.String(),
                        FormulaExpression = c.String(),
                        Indent = c.Int(nullable: false),
                        IsBold = c.Boolean(nullable: false),
                        IsItalic = c.Boolean(nullable: false),
                        IsUnderline = c.Boolean(nullable: false),
                        IsUppercase = c.Boolean(nullable: false),
                        BackgroundColor = c.String(),
                        TextColor = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReportTile", t => t.ReportTileId, cascadeDelete: true)
                .Index(t => t.ReportTileId);
            
            CreateTable(
                "dbo.UserDashboardActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        DashboardId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "dbo.UserFavoriteDashboard",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DashboardId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.DashboardId })
                .ForeignKey("dbo.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "dbo.UserFavoriteModel",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ModelId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ModelId })
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.UserFavoriteReport",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReportId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReportId })
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.UserModelActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ModelId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.UserNotification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        NotificationId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notification", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.NotificationId);
            
            CreateTable(
                "dbo.UserReportActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ReportId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.ModelRoleColumnPermission",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        ColumnId = c.Int(nullable: false),
                        MetadataPermission = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.ColumnId })
                .ForeignKey("dbo.Table", t => t.ColumnId)
                .ForeignKey("dbo.ModelRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ColumnId);
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModelRoleColumnPermission", "RoleId", "dbo.ModelRole");
            DropForeignKey("dbo.ModelRoleColumnPermission", "ColumnId", "dbo.Table");
            DropForeignKey("dbo.UserReportActivity", "ReportId", "dbo.Report");
            DropForeignKey("dbo.UserNotification", "NotificationId", "dbo.Notification");
            DropForeignKey("dbo.UserModelActivity", "ModelId", "dbo.Model");
            DropForeignKey("dbo.UserFavoriteReport", "ReportId", "dbo.Report");
            DropForeignKey("dbo.UserFavoriteModel", "ModelId", "dbo.Model");
            DropForeignKey("dbo.UserFavoriteDashboard", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.UserDashboardActivity", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.ReportTileRow", "ReportTileId", "dbo.ReportTile");
            DropForeignKey("dbo.PerspectiveColumn", "PerspectiveId", "dbo.Perspective");
            DropForeignKey("dbo.PerspectiveColumn", "ColumnId", "dbo.Column");
            DropForeignKey("dbo.DashboardComment", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.DashboardView", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.DashboardTile", "ReportTileId", "dbo.ReportTile");
            DropForeignKey("dbo.ReportTile", "ReportPageId", "dbo.ReportPage");
            DropForeignKey("dbo.ReportPage", "ReportId", "dbo.Report");
            DropForeignKey("dbo.ReportView", "ReportId", "dbo.Report");
            DropForeignKey("dbo.ReportSharing", "ReportId", "dbo.Report");
            DropForeignKey("dbo.ReportFilter", "ReportId", "dbo.Report");
            DropForeignKey("dbo.ReportComment", "ReportId", "dbo.Report");
            DropForeignKey("dbo.ReportTile", "DisplayTypeId", "dbo.DisplayType");
            DropForeignKey("dbo.DashboardTile", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.DashboardSharing", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.DashboardFilter", "DashboardId", "dbo.Dashboard");
            DropForeignKey("dbo.Column", "TableId", "dbo.Table");
            DropForeignKey("dbo.Column", "SortByColumnId", "dbo.Column");
            DropForeignKey("dbo.Relationship", "ToColumnId", "dbo.Column");
            DropForeignKey("dbo.Relationship", "ModelId", "dbo.Model");
            DropForeignKey("dbo.UserModelRole", "RoleId", "dbo.ModelRole");
            DropForeignKey("dbo.ModelRoleTablePermission", "TableId", "dbo.Table");
            DropForeignKey("dbo.ModelRoleTablePermission", "RoleId", "dbo.ModelRole");
            DropForeignKey("dbo.ModelRole", "ModelId", "dbo.Model");
            DropForeignKey("dbo.ModelSharing", "ModelId", "dbo.Model");
            DropForeignKey("dbo.DataSource", "SourceFileId", "dbo.SourceFile");
            DropForeignKey("dbo.Partition", "TableId", "dbo.Table");
            DropForeignKey("dbo.Table", "ModelId", "dbo.Model");
            DropForeignKey("dbo.Measure", "TableId", "dbo.Table");
            DropForeignKey("dbo.Hierarchy", "TableId", "dbo.Table");
            DropForeignKey("dbo.Level", "HierarchyId", "dbo.Hierarchy");
            DropForeignKey("dbo.Partition", "DataSourceId", "dbo.DataSource");
            DropForeignKey("dbo.DataSource", "ModelId", "dbo.Model");
            DropForeignKey("dbo.Relationship", "FromColumnId", "dbo.Column");
            DropForeignKey("dbo.Alert", "NotificationId", "dbo.Notification");
            DropIndex("dbo.ModelRoleColumnPermission", new[] { "ColumnId" });
            DropIndex("dbo.ModelRoleColumnPermission", new[] { "RoleId" });
            DropIndex("dbo.UserReportActivity", new[] { "ReportId" });
            DropIndex("dbo.UserNotification", new[] { "NotificationId" });
            DropIndex("dbo.UserModelActivity", new[] { "ModelId" });
            DropIndex("dbo.UserFavoriteReport", new[] { "ReportId" });
            DropIndex("dbo.UserFavoriteModel", new[] { "ModelId" });
            DropIndex("dbo.UserFavoriteDashboard", new[] { "DashboardId" });
            DropIndex("dbo.UserDashboardActivity", new[] { "DashboardId" });
            DropIndex("dbo.ReportTileRow", new[] { "ReportTileId" });
            DropIndex("dbo.PerspectiveColumn", new[] { "ColumnId" });
            DropIndex("dbo.PerspectiveColumn", new[] { "PerspectiveId" });
            DropIndex("dbo.DashboardView", new[] { "DashboardId" });
            DropIndex("dbo.ReportView", new[] { "ReportId" });
            DropIndex("dbo.ReportSharing", new[] { "ReportId" });
            DropIndex("dbo.ReportFilter", new[] { "ReportId" });
            DropIndex("dbo.ReportComment", new[] { "ReportId" });
            DropIndex("dbo.ReportPage", new[] { "ReportId" });
            DropIndex("dbo.ReportTile", new[] { "DisplayTypeId" });
            DropIndex("dbo.ReportTile", new[] { "ReportPageId" });
            DropIndex("dbo.DashboardTile", new[] { "ReportTileId" });
            DropIndex("dbo.DashboardTile", new[] { "DashboardId" });
            DropIndex("dbo.DashboardSharing", new[] { "DashboardId" });
            DropIndex("dbo.DashboardFilter", new[] { "DashboardId" });
            DropIndex("dbo.DashboardComment", new[] { "DashboardId" });
            DropIndex("dbo.UserModelRole", new[] { "RoleId" });
            DropIndex("dbo.ModelRoleTablePermission", new[] { "TableId" });
            DropIndex("dbo.ModelRoleTablePermission", new[] { "RoleId" });
            DropIndex("dbo.ModelRole", new[] { "ModelId" });
            DropIndex("dbo.ModelSharing", new[] { "ModelId" });
            DropIndex("dbo.Measure", new[] { "TableId" });
            DropIndex("dbo.Level", new[] { "HierarchyId" });
            DropIndex("dbo.Hierarchy", new[] { "TableId" });
            DropIndex("dbo.Table", new[] { "ModelId" });
            DropIndex("dbo.Partition", new[] { "DataSourceId" });
            DropIndex("dbo.Partition", new[] { "TableId" });
            DropIndex("dbo.DataSource", new[] { "SourceFileId" });
            DropIndex("dbo.DataSource", new[] { "ModelId" });
            DropIndex("dbo.Relationship", new[] { "ToColumnId" });
            DropIndex("dbo.Relationship", new[] { "FromColumnId" });
            DropIndex("dbo.Relationship", new[] { "ModelId" });
            DropIndex("dbo.Column", new[] { "SortByColumnId" });
            DropIndex("dbo.Column", new[] { "TableId" });
            DropIndex("dbo.Alert", new[] { "NotificationId" });
            DropTable("dbo.UserGroup");
            DropTable("dbo.ModelRoleColumnPermission");
            DropTable("dbo.UserReportActivity");
            DropTable("dbo.UserNotification");
            DropTable("dbo.UserModelActivity");
            DropTable("dbo.UserFavoriteReport");
            DropTable("dbo.UserFavoriteModel");
            DropTable("dbo.UserFavoriteDashboard");
            DropTable("dbo.UserDashboardActivity");
            DropTable("dbo.ReportTileRow");
            DropTable("dbo.Perspective");
            DropTable("dbo.PerspectiveColumn");
            DropTable("dbo.NotificationSubscription");
            DropTable("dbo.ModelRefreshQueue");
            DropTable("dbo.DashboardView");
            DropTable("dbo.ReportView");
            DropTable("dbo.ReportSharing");
            DropTable("dbo.ReportFilter");
            DropTable("dbo.ReportComment");
            DropTable("dbo.Report");
            DropTable("dbo.ReportPage");
            DropTable("dbo.DisplayType");
            DropTable("dbo.ReportTile");
            DropTable("dbo.DashboardTile");
            DropTable("dbo.DashboardSharing");
            DropTable("dbo.DashboardFilter");
            DropTable("dbo.Dashboard");
            DropTable("dbo.DashboardComment");
            DropTable("dbo.UserModelRole");
            DropTable("dbo.ModelRoleTablePermission");
            DropTable("dbo.ModelRole");
            DropTable("dbo.ModelSharing");
            DropTable("dbo.SourceFile");
            DropTable("dbo.Measure");
            DropTable("dbo.Level");
            DropTable("dbo.Hierarchy");
            DropTable("dbo.Table");
            DropTable("dbo.Partition");
            DropTable("dbo.DataSource");
            DropTable("dbo.Model");
            DropTable("dbo.Relationship");
            DropTable("dbo.Column");
            DropTable("dbo.Notification");
            DropTable("dbo.Alert");
        }
    }
}
