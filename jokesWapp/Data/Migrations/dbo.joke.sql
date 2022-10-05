CREATE TABLE [dbo].[joke] (
    [id]       INT            IDENTITY (1, 1) NOT NULL,
    [question] NVARCHAR (MAX) NULL,
    [answer]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_joke] PRIMARY KEY CLUSTERED ([id] ASC)
);

