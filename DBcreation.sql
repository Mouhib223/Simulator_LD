------------------Creaction of FIXMessages Table-----------------
CREATE TABLE [dbo].[FIXMessages](
	[MsgId] [int]  IDENTITY(1,1) PRIMARY KEY,
	[msgBody] [nvarchar](max) NOT NULL,);

 
SELECT * FROM FIXMessages;
------------------Creaction of Orders Table-----------------
CREATE TABLE [dbo].[Orders](
	[OrderId] [int]  IDENTITY(1,1) PRIMARY KEY,
	[BeginString] [nvarchar](max) NOT NULL,
	[SenderCompID] [nvarchar](max) NOT NULL,
	[ClientCompID] [nvarchar](max) NOT NULL,
	[Symbol] [nvarchar](max) NOT NULL,
	[Price] [nvarchar](max) NOT NULL,
	[OrderQuantity] [nvarchar](max) NOT NULL,
	[Side] [nvarchar](max) NOT NULL,
	[TransactTime] [datetime2](7) NOT NULL,
	[RuleId] [int] NOT NULL, );
 
SELECT * FROM Orders;
 
------------------Creaction of Rules Table-----------------
CREATE TABLE [dbo].[Rules](
	[RuleId] [int]  IDENTITY(1,1) PRIMARY KEY,
	[ruleType] [nvarchar](max) ,
	[symbol] [nvarchar](max) ,
	[MinPrice] [real] ,
	[MaxPrice] [real] ,
	[MinQty] [real] ,
	[MaxQty] [real] ,
	[Description] [nvarchar](max) ,);
 
SELECT * FROM Rules;