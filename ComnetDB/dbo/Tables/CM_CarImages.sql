CREATE TABLE [dbo].[CM_CarImages] (
    [UnqGUID]           UNIQUEIDENTIFIER    NOT NULL DEFAULT (newid()) ,
    [URL]               NVARCHAR (4000)     NULL,
    [CarID]             UNIQUEIDENTIFIER    NOT NULL,
    [Order]             INT                 NULL,
    [IsDeleted]         BIT                 NULL DEFAULT 0,
    [CreatedDate]       DATETIME            NULL DEFAULT GETUTCDATE(),
    [CreatedBy]         NVARCHAR (128)      NULL,
    [UpdatedDate]       DATETIME            NULL,
    [UpdatedBy]         NVARCHAR (128)      NULL,
    CONSTRAINT [PK_CMCarImages] PRIMARY KEY CLUSTERED ([UnqGUID] ASC),
    CONSTRAINT [FK_CMCar_CMCarImage] FOREIGN KEY ([CarID]) REFERENCES [dbo].[CM_Cars] ([UnqGUID]),
);