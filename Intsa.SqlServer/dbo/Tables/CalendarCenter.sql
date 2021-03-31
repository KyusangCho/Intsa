CREATE TABLE [dbo].[CalendarCenter]
(
	[Id] INT NOT NULL PRIMARY KEY identity (1,1),	-- 
	[IsAllDay] bit not null,							-- 종일여부 
	[CenterId] Int Null,							-- 공장/사무실 분류코드 
	[Subject] nvarchar(255) null,					-- 
	[Location] nvarchar(255) null,					-- 위치
	[StartTime] datetime default(getdate()) null,	-- 시작일시
	[EndTime] datetime default(getdate()) null,		-- 종료일시 
	[Description] nvarchar(255) null,			    -- 내용
	[RecurrenceRule] nvarchar(255) null,			    -- 
	[RecurrenceException] nvarchar(255) null,			    -- 
	[RecurrenceID] Int Null	
)
