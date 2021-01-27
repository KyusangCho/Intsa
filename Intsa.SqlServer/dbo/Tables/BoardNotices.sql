CREATE TABLE [dbo].[BoardNotices]
(
	[Id] INT NOT NULL PRIMARY KEY identity (1,1),		-- Serial Number 
	[ParentId] Int Null,		-- ParentId, AppId, SiteId, ... 

	[Name] NVarchar(255) Not Null,					-- 작성자 
	[Title] NVarchar(255) Null,						-- 제목
	[Content] nvarchar(max) null,					-- 내용 => Todo: not null 
	[IsPinned] Bit null default(0),					-- 공지글로 올리기 
	[CreatedBy]	nvarchar(255) null,					-- 등록자(creator)
	[Created] datetime default(getdate()) null,		-- 생성일 
	[ModifiedBy] nvarchar(255) null,				-- 수정자
	[Modified] datetime null,						-- 수정일 
)
Go 
