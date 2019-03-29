/*
Navicat SQL Server Data Transfer

Source Server         : local_sql_db
Source Server Version : 140000
Source Host           : DESKTOP-R916863\SQLEXPRESS:1433
Source Database       : CarDealer
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 140000
File Encoding         : 65001

Date: 2019-03-28 15:46:04
*/


-- ----------------------------
-- Table structure for Car
-- ----------------------------

CREATE TABLE [dbo].[Car] (
[car_id] int NOT NULL IDENTITY(1,1) ,
[country] varchar(255) NOT NULL ,
[manufacturer] varchar(255) NOT NULL ,
[model] varchar(255) NOT NULL ,
[type] varchar(255) NOT NULL ,
[price] money NOT NULL 
)


GO

-- ----------------------------
-- Records of Car
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Car] ON
GO
INSERT INTO [dbo].[Car] ([car_id], [country], [manufacturer], [model], [type], [price]) VALUES (N'1', N'France', N'Renault', N'FR2', N't', N'755.0000')
GO
GO
INSERT INTO [dbo].[Car] ([car_id], [country], [manufacturer], [model], [type], [price]) VALUES (N'3', N'Italy', N'Fiat', N'CN6', N'for country', N'570.0000')
GO
GO
INSERT INTO [dbo].[Car] ([car_id], [country], [manufacturer], [model], [type], [price]) VALUES (N'1002', N'Germany', N'BMW', N'M3', N'for hipster', N'1300.0000')
GO
GO
SET IDENTITY_INSERT [dbo].[Car] OFF
GO

-- ----------------------------
-- Table structure for Customer
-- ----------------------------
CREATE TABLE [dbo].[Customer] (
[customer_id] int NOT NULL IDENTITY(1,1) ,
[firstName] varchar(255) NOT NULL ,
[lastName] varchar(255) NOT NULL ,
[email] varchar(255) NULL 
)


GO

-- ----------------------------
-- Records of Customer
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Customer] ON
GO
INSERT INTO [dbo].[Customer] ([customer_id], [firstName], [lastName], [email]) VALUES (N'1', N'admin', N'adminovich', N'admin@gmail.com')
GO
GO
INSERT INTO [dbo].[Customer] ([customer_id], [firstName], [lastName], [email]) VALUES (N'2', N'adminz', N'adminovichz', N'adminz@gmail.com')
GO
GO
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO

-- ----------------------------
-- Table structure for Order
-- ----------------------------
CREATE TABLE [dbo].[Order] (
[order_id] int NOT NULL IDENTITY(1,1) ,
[date] datetime NULL ,
[customer] int NOT NULL 
)


GO

-- ----------------------------
-- Records of Order
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Order] ON
GO
INSERT INTO [dbo].[Order] ([order_id], [date], [customer]) VALUES (N'12', N'2019-03-26 00:00:00.000', N'1')
GO
GO
SET IDENTITY_INSERT [dbo].[Order] OFF
GO

-- ----------------------------
-- Table structure for OrderDetail
-- ----------------------------
CREATE TABLE [dbo].[OrderDetail] (
[order_id] int NOT NULL ,
[car_id] int NOT NULL ,
[amount] int NULL 
)


GO

-- ----------------------------
-- Records of OrderDetail
-- ----------------------------
INSERT INTO [dbo].[OrderDetail] ([order_id], [car_id], [amount]) VALUES (N'12', N'1', N'1')
GO

-- ----------------------------
-- Table structure for sysdiagrams
-- ----------------------------
CREATE TABLE [dbo].[sysdiagrams] (
[name] sysname NOT NULL ,
[principal_id] int NOT NULL ,
[diagram_id] int NOT NULL IDENTITY(1,1) ,
[version] int NULL ,
[definition] varbinary(MAX) NULL 
)


GO

-- ----------------------------
-- Records of sysdiagrams
-- ----------------------------
SET IDENTITY_INSERT [dbo].[sysdiagrams] ON
GO
SET IDENTITY_INSERT [dbo].[sysdiagrams] OFF
GO

-- ----------------------------
-- Indexes structure for table Car
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Car
-- ----------------------------
ALTER TABLE [dbo].[Car] ADD PRIMARY KEY ([car_id])
GO

-- ----------------------------
-- Indexes structure for table Customer
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Customer
-- ----------------------------
ALTER TABLE [dbo].[Customer] ADD PRIMARY KEY ([customer_id])
GO

-- ----------------------------
-- Indexes structure for table Order
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Order
-- ----------------------------
ALTER TABLE [dbo].[Order] ADD PRIMARY KEY ([order_id])
GO

-- ----------------------------
-- Indexes structure for table OrderDetail
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table OrderDetail
-- ----------------------------
ALTER TABLE [dbo].[OrderDetail] ADD PRIMARY KEY ([order_id], [car_id])
GO

-- ----------------------------
-- Indexes structure for table sysdiagrams
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table sysdiagrams
-- ----------------------------
ALTER TABLE [dbo].[sysdiagrams] ADD PRIMARY KEY ([diagram_id])
GO

-- ----------------------------
-- Uniques structure for table sysdiagrams
-- ----------------------------
ALTER TABLE [dbo].[sysdiagrams] ADD UNIQUE ([principal_id] ASC, [name] ASC)
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[Order]
-- ----------------------------
ALTER TABLE [dbo].[Order] ADD FOREIGN KEY ([customer]) REFERENCES [dbo].[Customer] ([customer_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[OrderDetail]
-- ----------------------------
ALTER TABLE [dbo].[OrderDetail] ADD FOREIGN KEY ([order_id]) REFERENCES [dbo].[Order] ([order_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[OrderDetail] ADD FOREIGN KEY ([car_id]) REFERENCES [dbo].[Car] ([car_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
