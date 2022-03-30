CREATE TABLE [dbo].[Transactions](
	[TransactionId] [int] NOT NULL,
	[WalletId] [int] NOT NULL,
	[Direction] [nvarchar](25) NOT NULL,
	[Amount] [decimal](10, 2) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Wallet](
	[WalletId] [int] IDENTITY(1000,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[Balance] [decimal](10, 2) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED 
(
	[WalletId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[Transactions] ([TransactionId], [WalletId], [Direction], [Amount], [CreatedBy], [CreatedOn]) VALUES (100, 1001, N'1', CAST(1.00 AS Decimal(10, 2)), 1, CAST(N'2022-03-30T17:53:19.7033333' AS DateTime2))
GO
INSERT [dbo].[Transactions] ([TransactionId], [WalletId], [Direction], [Amount], [CreatedBy], [CreatedOn]) VALUES (101, 1001, N'1', CAST(32.00 AS Decimal(10, 2)), 1, CAST(N'2022-03-30T18:03:24.3333333' AS DateTime2))
GO
INSERT [dbo].[Transactions] ([TransactionId], [WalletId], [Direction], [Amount], [CreatedBy], [CreatedOn]) VALUES (102, 1001, N'2', CAST(32.00 AS Decimal(10, 2)), 1, CAST(N'2022-03-30T18:03:41.1900000' AS DateTime2))
GO

SET IDENTITY_INSERT [dbo].[Wallet] ON 
GO
INSERT [dbo].[Wallet] ([WalletId], [Name], [Balance], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (1000, N'First', CAST(100.00 AS Decimal(10, 2)), 0, CAST(N'2022-03-30T17:16:44.7326990' AS DateTime2), 0, CAST(N'2022-03-30T17:16:44.7326990' AS DateTime2))
GO
INSERT [dbo].[Wallet] ([WalletId], [Name], [Balance], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (1001, N'Second', CAST(124.00 AS Decimal(10, 2)), 0, CAST(N'2022-03-30T17:18:30.0366667' AS DateTime2), 1, CAST(N'2022-03-30T18:03:41.1933333' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Wallet] OFF
GO

ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_Amount]  DEFAULT ((0.00)) FOR [Amount]
GO
ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_CreatedBy]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_CreatedOn]  DEFAULT (sysutcdatetime()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[Wallet] ADD  CONSTRAINT [DF_Wallet_Balance]  DEFAULT ((0.00)) FOR [Balance]
GO
ALTER TABLE [dbo].[Wallet] ADD  CONSTRAINT [DF_Wallet_CreatedBy]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Wallet] ADD  CONSTRAINT [DF_Wallet_CreatedOn]  DEFAULT (sysutcdatetime()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Wallet] ADD  CONSTRAINT [DF_Wallet_ModifiedBy]  DEFAULT ((0)) FOR [ModifiedBy]
GO
ALTER TABLE [dbo].[Wallet] ADD  CONSTRAINT [DF_Wallet_ModifiedOn]  DEFAULT (sysutcdatetime()) FOR [ModifiedOn]
GO

ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Wallet] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([WalletId])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Wallet]
GO